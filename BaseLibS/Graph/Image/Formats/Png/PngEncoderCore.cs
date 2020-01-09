using System;
using System.IO;
using System.Threading.Tasks;
using BaseLibS.Graph.Image.Formats.Gif;
using BaseLibS.Graph.Image.Formats.Png.Zlib;
using BaseLibS.Graph.Image.Quantizers;
using BaseLibS.Num;

namespace BaseLibS.Graph.Image.Formats.Png{
	internal sealed class PngEncoderCore{
		private const int maxBlockSize = 65535;
		private byte bitDepth;
		public int Quality { get; set; }
		public int CompressionLevel { get; set; } = 6;
		public bool WriteGamma { get; set; }
		public float Gamma { get; set; } = 2.2F;
		public IQuantizer Quantizer { get; set; }
		public byte Threshold { get; set; } = 128;
		public void Encode(ImageBase image, Stream stream){
			if (image == null || stream == null){
				throw new ArgumentNullException();
			}
			stream.Write(new byte[]{
				0x89, // Set the high bit.
				0x50, // P
				0x4E, // N
				0x47, // G
				0x0D, // Line ending CRLF
				0x0A, // Line ending CRLF
				0x1A, // EOF
				0x0A // LF
			}, 0, 8);
			int quality = Quality > 0 ? Quality : image.Quality;
			Quality = quality > 0 ? NumUtils.Clamp(quality, 1, int.MaxValue) : int.MaxValue;
			bitDepth = Quality <= 256
				? (byte) NumUtils.Clamp(GifEncoderCore.GetBitsNeededForColorDepth(Quality), 1, 8)
				: (byte) 8;
			if (bitDepth == 3){
				bitDepth = 4;
			} else if (bitDepth >= 5 || bitDepth <= 7){
				bitDepth = 8;
			}
			PngHeader header = new PngHeader{
				Width = image.Width,
				Height = image.Height,
				ColorType = (byte) (Quality <= 256 ? 3 : 6),
				BitDepth = bitDepth,
				FilterMethod = 0, // None
				CompressionMethod = 0,
				InterlaceMethod = 0
			};
			WriteHeaderChunk(stream, header);
			QuantizedImage quantized = WritePaletteChunk(stream, header, image);
			WritePhysicalChunk(stream, image);
			WriteGammaChunk(stream);
			using (IPixelAccessor pixels = image.Lock()){
				WriteDataChunks(stream, pixels, quantized);
			}
			WriteEndChunk(stream);
			stream.Flush();
		}
		private static void WriteInteger(byte[] data, int offset, int value){
			byte[] buffer = BitConverter.GetBytes(value);
			Array.Reverse(buffer);
			Array.Copy(buffer, 0, data, offset, 4);
		}
		private static void WriteInteger(Stream stream, int value){
			byte[] buffer = BitConverter.GetBytes(value);
			Array.Reverse(buffer);
			stream.Write(buffer, 0, 4);
		}
		private static void WriteInteger(Stream stream, uint value){
			byte[] buffer = BitConverter.GetBytes(value);
			Array.Reverse(buffer);
			stream.Write(buffer, 0, 4);
		}
		private void WriteHeaderChunk(Stream stream, PngHeader header){
			byte[] chunkData = new byte[13];
			WriteInteger(chunkData, 0, header.Width);
			WriteInteger(chunkData, 4, header.Height);
			chunkData[8] = header.BitDepth;
			chunkData[9] = header.ColorType;
			chunkData[10] = header.CompressionMethod;
			chunkData[11] = header.FilterMethod;
			chunkData[12] = header.InterlaceMethod;
			WriteChunk(stream, PngChunkTypes.Header, chunkData);
		}
		private QuantizedImage WritePaletteChunk(Stream stream, PngHeader header, ImageBase image){
			if (Quality > 256){
				return null;
			}
			if (Quantizer == null){
				Quantizer = new WuQuantizer{Threshold = Threshold};
			}
			QuantizedImage quantized = ((IQuantizer) Quantizer).Quantize(image, Quality);
			Color2[] palette = quantized.Palette;
			int pixelCount = palette.Length;
			int colorTableLength = (int) Math.Pow(2, header.BitDepth)*3;
			byte[] colorTable = new byte[colorTableLength];
			Parallel.For(0, pixelCount, Bootstrapper.instance.ParallelOptions, i =>{
				int offset = i*3;
				byte[] color = palette[i].ToBytes();
				colorTable[offset] = color[0];
				colorTable[offset + 1] = color[1];
				colorTable[offset + 2] = color[2];
			});
			WriteChunk(stream, PngChunkTypes.Palette, colorTable);
			if (quantized.TransparentIndex > -1){
				WriteChunk(stream, PngChunkTypes.PaletteAlpha, new[]{(byte) quantized.TransparentIndex});
			}
			return quantized;
		}
		private void WritePhysicalChunk(Stream stream, ImageBase imageBase){
			Image2 image = imageBase as Image2;
			if (image != null && image.HorizontalResolution > 0 && image.VerticalResolution > 0){
				int dpmX = (int) Math.Round(image.HorizontalResolution*39.3700787D);
				int dpmY = (int) Math.Round(image.VerticalResolution*39.3700787D);
				byte[] chunkData = new byte[9];
				WriteInteger(chunkData, 0, dpmX);
				WriteInteger(chunkData, 4, dpmY);
				chunkData[8] = 1;
				WriteChunk(stream, PngChunkTypes.Physical, chunkData);
			}
		}
		private void WriteGammaChunk(Stream stream){
			if (WriteGamma){
				int gammaValue = (int) (Gamma*100000f);
				byte[] fourByteData = new byte[4];
				byte[] size = BitConverter.GetBytes(gammaValue);
				fourByteData[0] = size[3];
				fourByteData[1] = size[2];
				fourByteData[2] = size[1];
				fourByteData[3] = size[0];
				WriteChunk(stream, PngChunkTypes.Gamma, fourByteData);
			}
		}
		private void WriteDataChunks(Stream stream, IPixelAccessor pixels, QuantizedImage quantized){
			byte[] data;
			int imageWidth = pixels.Width;
			int imageHeight = pixels.Height;
			if (Quality <= 256){
				int rowLength = imageWidth + 1;
				data = new byte[rowLength*imageHeight];
				Parallel.For(0, imageHeight, Bootstrapper.instance.ParallelOptions, y =>{
					int dataOffset = (y*rowLength);
					byte compression = 0;
					if (y > 0){
						compression = 2;
					}
					data[dataOffset++] = compression;
					for (int x = 0; x < imageWidth; x++){
						data[dataOffset++] = quantized.Pixels[(y*imageWidth) + x];
						if (y > 0){
							data[dataOffset - 1] -= quantized.Pixels[((y - 1)*imageWidth) + x];
						}
					}
				});
			} else{
				data = new byte[(imageWidth*imageHeight*4) + pixels.Height];
				int rowLength = (imageWidth*4) + 1;
				Parallel.For(0, imageHeight, Bootstrapper.instance.ParallelOptions, y =>{
					byte compression = 0;
					if (y > 0){
						compression = 2;
					}
					data[y*rowLength] = compression;
					for (int x = 0; x < imageWidth; x++){
						byte[] color = pixels[x, y].ToBytes();
						int dataOffset = (y*rowLength) + (x*4) + 1;
						data[dataOffset] = color[0];
						data[dataOffset + 1] = color[1];
						data[dataOffset + 2] = color[2];
						data[dataOffset + 3] = color[3];
						if (y > 0){
							color = pixels[x, y - 1].ToBytes();
							data[dataOffset] -= color[0];
							data[dataOffset + 1] -= color[1];
							data[dataOffset + 2] -= color[2];
							data[dataOffset + 3] -= color[3];
						}
					}
				});
			}
			byte[] buffer;
			int bufferLength;
			MemoryStream memoryStream = null;
			try{
				memoryStream = new MemoryStream();
				using (ZlibDeflateStream deflateStream = new ZlibDeflateStream(memoryStream, CompressionLevel)){
					deflateStream.Write(data, 0, data.Length);
				}
				bufferLength = (int) memoryStream.Length;
				buffer = memoryStream.ToArray();
			} finally{
				memoryStream?.Dispose();
			}
			int numChunks = bufferLength/maxBlockSize;
			if (bufferLength%maxBlockSize != 0){
				numChunks++;
			}
			for (int i = 0; i < numChunks; i++){
				int length = bufferLength - (i*maxBlockSize);
				if (length > maxBlockSize){
					length = maxBlockSize;
				}
				WriteChunk(stream, PngChunkTypes.Data, buffer, i*maxBlockSize, length);
			}
		}
		private void WriteEndChunk(Stream stream){
			WriteChunk(stream, PngChunkTypes.End, null);
		}
		private void WriteChunk(Stream stream, string type, byte[] data){
			WriteChunk(stream, type, data, 0, data?.Length ?? 0);
		}
		private void WriteChunk(Stream stream, string type, byte[] data, int offset, int length){
			WriteInteger(stream, length);
			byte[] typeArray = new byte[4];
			typeArray[0] = (byte) type[0];
			typeArray[1] = (byte) type[1];
			typeArray[2] = (byte) type[2];
			typeArray[3] = (byte) type[3];
			stream.Write(typeArray, 0, 4);
			if (data != null){
				stream.Write(data, offset, length);
			}
			Crc32 crc32 = new Crc32();
			crc32.Update(typeArray);
			if (data != null){
				crc32.Update(data, offset, length);
			}
			WriteInteger(stream, (uint) crc32.Value);
		}
	}
}