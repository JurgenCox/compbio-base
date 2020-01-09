using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BaseLibS.Graph.Image.Formats.Png.Zlib;

namespace BaseLibS.Graph.Image.Formats.Png{
	internal class PngDecoderCore{
		private static readonly Dictionary<int, PngColorTypeInformation> colorTypes =
			new Dictionary<int, PngColorTypeInformation>();

		private Stream currentStream;
		private PngHeader header;

		static PngDecoderCore(){
			colorTypes.Add(0, new PngColorTypeInformation(1, new[]{1, 2, 4, 8}, (p, a) => new GrayscaleReader(false)));
			colorTypes.Add(2, new PngColorTypeInformation(3, new[]{8}, (p, a) => new TrueColorReader(false)));
			colorTypes.Add(3, new PngColorTypeInformation(1, new[]{1, 2, 4, 8}, (p, a) => new PaletteIndexReader(p, a)));
			colorTypes.Add(4, new PngColorTypeInformation(2, new[]{8}, (p, a) => new GrayscaleReader(true)));
			colorTypes.Add(6, new PngColorTypeInformation(4, new[]{8}, (p, a) => new TrueColorReader(true)));
		}

		public void Decode(Image2 image, Stream stream){
			Image2 currentImage = image;
			currentStream = stream;
			currentStream.Seek(8, SeekOrigin.Current);
			bool isEndChunkReached = false;
			byte[] palette = null;
			byte[] paletteAlpha = null;
			using (MemoryStream dataStream = new MemoryStream()){
				PngChunk currentChunk;
				while ((currentChunk = ReadChunk()) != null){
					if (isEndChunkReached){
						throw new Exception("Image does not end with end chunk.");
					}
					if (currentChunk.Type == PngChunkTypes.Header){
						ReadHeaderChunk(currentChunk.Data);
						ValidateHeader();
					} else if (currentChunk.Type == PngChunkTypes.Physical){
						ReadPhysicalChunk(currentImage, currentChunk.Data);
					} else if (currentChunk.Type == PngChunkTypes.Data){
						dataStream.Write(currentChunk.Data, 0, currentChunk.Data.Length);
					} else if (currentChunk.Type == PngChunkTypes.Palette){
						palette = currentChunk.Data;
					} else if (currentChunk.Type == PngChunkTypes.PaletteAlpha){
						paletteAlpha = currentChunk.Data;
					} else if (currentChunk.Type == PngChunkTypes.Text){
						ReadTextChunk(currentImage, currentChunk.Data);
					} else if (currentChunk.Type == PngChunkTypes.End){
						isEndChunkReached = true;
					}
				}
				if (header.Width > image.MaxWidth || header.Height > image.MaxHeight){
					throw new ArgumentOutOfRangeException($"The input png '{header.Width}x{header.Height}' is bigger than the " +
														$"max allowed size '{image.MaxWidth}x{image.MaxHeight}'");
				}
				Color2[] pixels = new Color2[header.Width*header.Height];
				PngColorTypeInformation colorTypeInformation = colorTypes[header.ColorType];
				if (colorTypeInformation != null){
					IColorReader colorReader = colorTypeInformation.CreateColorReader(palette, paletteAlpha);
					ReadScanlines(dataStream, pixels, colorReader, colorTypeInformation);
				}
				image.SetPixels(header.Width, header.Height, pixels);
			}
		}

		private static byte PaethPredicator(byte left, byte above, byte upperLeft){
			byte predicator;
			int p = left + above - upperLeft;
			int pa = Math.Abs(p - left);
			int pb = Math.Abs(p - above);
			int pc = Math.Abs(p - upperLeft);
			if (pa <= pb && pa <= pc){
				predicator = left;
			} else if (pb <= pc){
				predicator = above;
			} else{
				predicator = upperLeft;
			}
			return predicator;
		}

		private void ReadPhysicalChunk(Image2 image, byte[] data){
			Array.Reverse(data, 0, 4);
			Array.Reverse(data, 4, 4);
			image.HorizontalResolution = BitConverter.ToInt32(data, 0)/39.3700787d;
			image.VerticalResolution = BitConverter.ToInt32(data, 4)/39.3700787d;
		}

		private int CalculateScanlineLength(PngColorTypeInformation colorTypeInformation){
			int scanlineLength = header.Width*header.BitDepth*colorTypeInformation.ChannelsPerColor;
			int amount = scanlineLength%8;
			if (amount != 0){
				scanlineLength += 8 - amount;
			}
			return scanlineLength/8;
		}

		private int CalculateScanlineStep(PngColorTypeInformation colorTypeInformation){
			int scanlineStep = 1;
			if (header.BitDepth >= 8){
				scanlineStep = (colorTypeInformation.ChannelsPerColor*header.BitDepth)/8;
			}
			return scanlineStep;
		}

		private void ReadScanlines(MemoryStream dataStream, Color2[] pixels, IColorReader colorReader,
			PngColorTypeInformation colorTypeInformation){
			dataStream.Position = 0;
			int scanlineLength = CalculateScanlineLength(colorTypeInformation);
			int scanlineStep = CalculateScanlineStep(colorTypeInformation);
			byte[] lastScanline = new byte[scanlineLength];
			byte[] currentScanline = new byte[scanlineLength];
			int filter = 0, column = -1;
			using (ZlibInflateStream compressedStream = new ZlibInflateStream(dataStream)){
				int readByte;
				while ((readByte = compressedStream.ReadByte()) >= 0){
					if (column == -1){
						filter = readByte;
						column++;
					} else{
						currentScanline[column] = (byte) readByte;
						byte a;
						byte c;
						if (column >= scanlineStep){
							a = currentScanline[column - scanlineStep];
							c = lastScanline[column - scanlineStep];
						} else{
							a = 0;
							c = 0;
						}
						byte b = lastScanline[column];
						if (filter == 1){
							currentScanline[column] = (byte) (currentScanline[column] + a);
						} else if (filter == 2){
							currentScanline[column] = (byte) (currentScanline[column] + b);
						} else if (filter == 3){
							currentScanline[column] = (byte) (currentScanline[column] + (byte) ((a + b)/2));
						} else if (filter == 4){
							currentScanline[column] = (byte) (currentScanline[column] + PaethPredicator(a, b, c));
						}
						column++;
						if (column == scanlineLength){
							colorReader.ReadScanline(currentScanline, pixels, header);
							column = -1;
							Swap(ref currentScanline, ref lastScanline);
						}
					}
				}
			}
		}

		private void ReadTextChunk(Image2 image, byte[] data){
			int zeroIndex = 0;
			for (int i = 0; i < data.Length; i++){
				if (data[i] == 0){
					zeroIndex = i;
					break;
				}
			}
			string name = Encoding.Unicode.GetString(data, 0, zeroIndex);
			string value = Encoding.Unicode.GetString(data, zeroIndex + 1, data.Length - zeroIndex - 1);
			image.Properties.Add(new ImageProperty(name, value));
		}

		private void ReadHeaderChunk(byte[] data){
			header = new PngHeader();
			Array.Reverse(data, 0, 4);
			Array.Reverse(data, 4, 4);
			header.Width = BitConverter.ToInt32(data, 0);
			header.Height = BitConverter.ToInt32(data, 4);
			header.BitDepth = data[8];
			header.ColorType = data[9];
			header.FilterMethod = data[11];
			header.InterlaceMethod = data[12];
			header.CompressionMethod = data[10];
		}

		private void ValidateHeader(){
			if (!colorTypes.ContainsKey(header.ColorType)){
				throw new Exception("Color type is not supported or not valid.");
			}
			if (!colorTypes[header.ColorType].SupportedBitDepths.Contains(header.BitDepth)){
				throw new Exception("Bit depth is not supported or not valid.");
			}
			if (header.FilterMethod != 0){
				throw new Exception("The png specification only defines 0 as filter method.");
			}
			if (header.InterlaceMethod != 0){
				throw new Exception("Interlacing is not supported.");
			}
		}

		private PngChunk ReadChunk(){
			PngChunk chunk = new PngChunk();
			if (ReadChunkLength(chunk) == 0){
				return null;
			}
			byte[] typeBuffer = ReadChunkType(chunk);
			ReadChunkData(chunk);
			ReadChunkCrc(chunk, typeBuffer);
			return chunk;
		}

		private void ReadChunkCrc(PngChunk chunk, byte[] typeBuffer){
			byte[] crcBuffer = new byte[4];
			int numBytes = currentStream.Read(crcBuffer, 0, 4);
			if (numBytes >= 1 && numBytes <= 3){
				throw new Exception("Image stream is not valid!");
			}
			Array.Reverse(crcBuffer);
			chunk.Crc = BitConverter.ToUInt32(crcBuffer, 0);
			Crc32 crc = new Crc32();
			crc.Update(typeBuffer);
			crc.Update(chunk.Data);
			if (crc.Value != chunk.Crc){
				throw new Exception("CRC Error. Png Image chunk is corrupt!");
			}
		}

		private void ReadChunkData(PngChunk chunk){
			chunk.Data = new byte[chunk.Length];
			currentStream.Read(chunk.Data, 0, chunk.Length);
		}

		private byte[] ReadChunkType(PngChunk chunk){
			byte[] typeBuffer = new byte[4];
			int numBytes = currentStream.Read(typeBuffer, 0, 4);
			if (numBytes >= 1 && numBytes <= 3){
				throw new Exception("Image stream is not valid!");
			}
			char[] chars = new char[4];
			chars[0] = (char) typeBuffer[0];
			chars[1] = (char) typeBuffer[1];
			chars[2] = (char) typeBuffer[2];
			chars[3] = (char) typeBuffer[3];
			chunk.Type = new string(chars);
			return typeBuffer;
		}

		private int ReadChunkLength(PngChunk chunk){
			byte[] lengthBuffer = new byte[4];
			int numBytes = currentStream.Read(lengthBuffer, 0, 4);
			if (numBytes >= 1 && numBytes <= 3){
				throw new Exception("Image stream is not valid!");
			}
			Array.Reverse(lengthBuffer);
			chunk.Length = BitConverter.ToInt32(lengthBuffer, 0);
			return numBytes;
		}

		private void Swap<TRef>(ref TRef lhs, ref TRef rhs) where TRef : class{
			TRef tmp = lhs;
			lhs = rhs;
			rhs = tmp;
		}
	}
}