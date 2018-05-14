using System;
using System.IO;
using System.Threading.Tasks;

namespace BaseLibS.Graph.Image.Formats.Bmp{
	internal sealed class BmpDecoderCore{
		private const int rgb16RMask = 0x00007C00;
		private const int rgb16GMask = 0x000003E0;
		private const int rgb16BMask = 0x0000001F;
		private Stream currentStream;
		private BmpFileHeader fileHeader;
		private BmpInfoHeader infoHeader;
		public void Decode(Image2 image, Stream stream){
			currentStream = stream;
			try{
				ReadFileHeader();
				ReadInfoHeader();
				bool inverted = false;
				if (infoHeader.Height < 0){
					inverted = true;
					infoHeader.Height = -infoHeader.Height;
				}
				int colorMapSize = -1;
				if (infoHeader.ClrUsed == 0){
					if (infoHeader.BitsPerPixel == 1 || infoHeader.BitsPerPixel == 4 || infoHeader.BitsPerPixel == 8){
						colorMapSize = (int) Math.Pow(2, infoHeader.BitsPerPixel)*4;
					}
				} else{
					colorMapSize = infoHeader.ClrUsed*4;
				}
				byte[] palette = null;
				if (colorMapSize > 0){
					// 255 * 4
					if (colorMapSize > 1020){
						throw new Exception($"Invalid bmp colormap size '{colorMapSize}'");
					}
					palette = new byte[colorMapSize];
					currentStream.Read(palette, 0, colorMapSize);
				}
				if (infoHeader.Width > image.MaxWidth || infoHeader.Height > image.MaxHeight){
					throw new ArgumentOutOfRangeException($"The input bitmap '{infoHeader.Width}x{infoHeader.Height}' is " +
														$"bigger then the max allowed size '{image.MaxWidth}x{image.MaxHeight}'");
				}
				Color2[] imageData = new Color2[infoHeader.Width*infoHeader.Height];
				switch (infoHeader.Compression){
					case BmpCompression.Rgb:
						if (infoHeader.HeaderSize != 40){
							throw new Exception($"Header Size value '{infoHeader.HeaderSize}' is not valid.");
						}
						if (infoHeader.BitsPerPixel == 32){
							ReadRgb32(imageData, infoHeader.Width, infoHeader.Height, inverted);
						} else if (infoHeader.BitsPerPixel == 24){
							ReadRgb24(imageData, infoHeader.Width, infoHeader.Height, inverted);
						} else if (infoHeader.BitsPerPixel == 16){
							ReadRgb16(imageData, infoHeader.Width, infoHeader.Height, inverted);
						} else if (infoHeader.BitsPerPixel <= 8){
							ReadRgbPalette(imageData, palette, infoHeader.Width, infoHeader.Height, infoHeader.BitsPerPixel, inverted);
						}
						break;
					default:
						throw new NotSupportedException("Does not support this kind of bitmap files.");
				}
				image.SetPixels(infoHeader.Width, infoHeader.Height, imageData);
			} catch (IndexOutOfRangeException e){
				throw new Exception("Bitmap does not have a valid format.", e);
			}
		}
		private static int Invert(int y, int height, bool inverted){
			int row;
			if (!inverted){
				row = height - y - 1;
			} else{
				row = y;
			}
			return row;
		}
		private void ReadRgbPalette(Color2[] imageData, byte[] colors, int width, int height, int bits, bool inverted){
			// Pixels per byte (bits per pixel)
			int ppb = 8/bits;
			int arrayWidth = (width + ppb - 1)/ppb;

			// Bit mask
			int mask = 0xFF >> (8 - bits);
			byte[] data = new byte[arrayWidth*height];
			currentStream.Read(data, 0, data.Length);

			// Rows are aligned on 4 byte boundaries
			int alignment = arrayWidth%4;
			if (alignment != 0){
				alignment = 4 - alignment;
			}
			Parallel.For(0, height, Bootstrapper.instance.ParallelOptions, y =>{
				int rowOffset = y*(arrayWidth + alignment);
				for (int x = 0; x < arrayWidth; x++){
					int offset = rowOffset + x;

					// Revert the y value, because bitmaps are saved from down to top
					int row = Invert(y, height, inverted);
					int colOffset = x*ppb;
					for (int shift = 0; shift < ppb && (colOffset + shift) < width; shift++){
						if (offset >= data.Length){
							continue;
						}
						int colorIndex = ((data[offset] >> (8 - bits - (shift*bits))) & mask)*4;
						int arrayOffset = (row*width) + (colOffset + shift);

						// Stored in b-> g-> r order.
						Color2 packed = Color2.FromArgb(colors[colorIndex + 2], colors[colorIndex + 1], colors[colorIndex]);
						imageData[arrayOffset] = packed;
					}
				}
			});
		}
		private void ReadRgb16(Color2[] imageData, int width, int height, bool inverted){
			// We divide here as we will store the colors in our floating point format.
			const int scaleR = 8; // 256/32
			const int scaleG = 4; // 256/64
			int alignment;
			byte[] data = GetImageArray(width, height, 2, out alignment);
			Parallel.For(0, height, Bootstrapper.instance.ParallelOptions, y =>{
				int rowOffset = y*((width*2) + alignment);

				// Revert the y value, because bitmaps are saved from down to top
				int row = Invert(y, height, inverted);
				for (int x = 0; x < width; x++){
					int offset = rowOffset + (x*2);
					short temp = BitConverter.ToInt16(data, offset);
					byte r = (byte) (((temp & rgb16RMask) >> 11)*scaleR);
					byte g = (byte) (((temp & rgb16GMask) >> 5)*scaleG);
					byte b = (byte) ((temp & rgb16BMask)*scaleR);
					int arrayOffset = row*width + x;

					// Stored in b-> g-> r order.
					Color2 packed = Color2.FromArgb(r, g, b);
					imageData[arrayOffset] = packed;
				}
			});
		}
		private void ReadRgb24(Color2[] imageData, int width, int height, bool inverted){
			int alignment;
			byte[] data = GetImageArray(width, height, 3, out alignment);
			Parallel.For(0, height, Bootstrapper.instance.ParallelOptions, y =>{
				int rowOffset = y*((width*3) + alignment);

				// Revert the y value, because bitmaps are saved from down to top
				int row = Invert(y, height, inverted);
				for (int x = 0; x < width; x++){
					int offset = rowOffset + (x*3);
					int arrayOffset = ((row*width) + x);

					// We divide by 255 as we will store the colors in our floating point format.
					// Stored in b-> g-> r-> a order.
					Color2 packed = Color2.FromArgb(data[offset + 2], data[offset + 1], data[offset]);
					imageData[arrayOffset] = packed;
				}
			});
		}
		private void ReadRgb32(Color2[] imageData, int width, int height, bool inverted){
			int alignment;
			byte[] data = GetImageArray(width, height, 4, out alignment);
			Parallel.For(0, height, Bootstrapper.instance.ParallelOptions, y =>{
				int rowOffset = y*((width*4) + alignment);

				// Revert the y value, because bitmaps are saved from down to top
				int row = Invert(y, height, inverted);
				for (int x = 0; x < width; x++){
					int offset = rowOffset + (x*4);
					int arrayOffset = ((row*width) + x);

					// Stored in b-> g-> r-> a order.
					Color2 packed = Color2.FromArgb(data[offset + 3], data[offset + 2], data[offset + 1], data[offset]);
					imageData[arrayOffset] = packed;
				}
			});
		}
		private byte[] GetImageArray(int width, int height, int bytes, out int alignment){
			int dataWidth = width;
			alignment = (width*bytes)%4;
			if (alignment != 0){
				alignment = 4 - alignment;
			}
			int size = ((dataWidth*bytes) + alignment)*height;
			byte[] data = new byte[size];
			currentStream.Read(data, 0, size);
			return data;
		}
		private void ReadInfoHeader(){
			byte[] data = new byte[BmpInfoHeader.size];
			currentStream.Read(data, 0, BmpInfoHeader.size);
			infoHeader = new BmpInfoHeader{
				HeaderSize = BitConverter.ToInt32(data, 0),
				Width = BitConverter.ToInt32(data, 4),
				Height = BitConverter.ToInt32(data, 8),
				Planes = BitConverter.ToInt16(data, 12),
				BitsPerPixel = BitConverter.ToInt16(data, 14),
				ImageSize = BitConverter.ToInt32(data, 20),
				XPelsPerMeter = BitConverter.ToInt32(data, 24),
				YPelsPerMeter = BitConverter.ToInt32(data, 28),
				ClrUsed = BitConverter.ToInt32(data, 32),
				ClrImportant = BitConverter.ToInt32(data, 36),
				Compression = (BmpCompression) BitConverter.ToInt32(data, 16)
			};
		}
		private void ReadFileHeader(){
			byte[] data = new byte[BmpFileHeader.size];
			currentStream.Read(data, 0, BmpFileHeader.size);
			fileHeader = new BmpFileHeader{
				Type = BitConverter.ToInt16(data, 0),
				FileSize = BitConverter.ToInt32(data, 2),
				Reserved = BitConverter.ToInt32(data, 6),
				Offset = BitConverter.ToInt32(data, 10)
			};
		}
	}
}