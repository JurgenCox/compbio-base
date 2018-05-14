using System;
using System.IO;
using BaseLibS.Parse.Endian;

namespace BaseLibS.Graph.Image.Formats.Bmp{
	internal sealed class BmpEncoderCore{
		private BmpBitsPerPixel bmpBitsPerPixel;
		public void Encode(ImageBase image, Stream stream, BmpBitsPerPixel bitsPerPixel){
			if (image == null || stream == null){
				throw new ArgumentNullException();
			}
			bmpBitsPerPixel = bitsPerPixel;
			int rowWidth = image.Width;

			// TODO: Check this for varying file formats.
			int amount = (image.Width*(int) bmpBitsPerPixel)%4;
			if (amount != 0){
				rowWidth += 4 - amount;
			}

			// Do not use IDisposable pattern here as we want to preserve the stream. 
			EndianBinaryWriter writer = new EndianBinaryWriter(EndianBitConverter.Little, stream);
			int bpp = (int) bmpBitsPerPixel;
			BmpFileHeader fileHeader = new BmpFileHeader{
				Type = 19778, // BM
				Offset = 54,
				FileSize = 54 + image.Height*rowWidth*bpp
			};
			BmpInfoHeader infoHeader = new BmpInfoHeader{
				HeaderSize = 40,
				Height = image.Height,
				Width = image.Width,
				BitsPerPixel = (short) (8*bpp),
				Planes = 1,
				ImageSize = image.Height*rowWidth*bpp,
				ClrUsed = 0,
				ClrImportant = 0
			};
			WriteHeader(writer, fileHeader);
			WriteInfo(writer, infoHeader);
			WriteImage(writer, image);
			writer.Flush();
		}
		private static void WriteHeader(EndianBinaryWriter writer, BmpFileHeader fileHeader){
			writer.Write(fileHeader.Type);
			writer.Write(fileHeader.FileSize);
			writer.Write(fileHeader.Reserved);
			writer.Write(fileHeader.Offset);
		}
		private void WriteInfo(EndianBinaryWriter writer, BmpInfoHeader infoHeader){
			writer.Write(infoHeader.HeaderSize);
			writer.Write(infoHeader.Width);
			writer.Write(infoHeader.Height);
			writer.Write(infoHeader.Planes);
			writer.Write(infoHeader.BitsPerPixel);
			writer.Write((int) infoHeader.Compression);
			writer.Write(infoHeader.ImageSize);
			writer.Write(infoHeader.XPelsPerMeter);
			writer.Write(infoHeader.YPelsPerMeter);
			writer.Write(infoHeader.ClrUsed);
			writer.Write(infoHeader.ClrImportant);
		}
		private void WriteImage(EndianBinaryWriter writer, ImageBase image){
			// TODO: Add more compression formats.
			int amount = (image.Width*(int) bmpBitsPerPixel)%4;
			if (amount != 0){
				amount = 4 - amount;
			}
			using (IPixelAccessor pixels = image.Lock()){
				switch (bmpBitsPerPixel){
					case BmpBitsPerPixel.Pixel32:
						Write32bit(writer, pixels, amount);
						break;
					case BmpBitsPerPixel.Pixel24:
						Write24bit(writer, pixels, amount);
						break;
				}
			}
		}
		private void Write32bit(EndianBinaryWriter writer, IPixelAccessor pixels, int amount){
			for (int y = pixels.Height - 1; y >= 0; y--){
				for (int x = 0; x < pixels.Width; x++){
					// Convert back to b-> g-> r-> a order.
					byte[] bytes = pixels[x, y].ToBytes();
					writer.Write(new[]{bytes[2], bytes[1], bytes[0], bytes[3]});
				}

				// Pad
				for (int i = 0; i < amount; i++){
					writer.Write((byte) 0);
				}
			}
		}
		private void Write24bit(EndianBinaryWriter writer, IPixelAccessor pixels, int amount){
			for (int y = pixels.Height - 1; y >= 0; y--){
				for (int x = 0; x < pixels.Width; x++){
					// Convert back to b-> g-> r order.
					byte[] bytes = pixels[x, y].ToBytes();
					writer.Write(new[]{bytes[2], bytes[1], bytes[0]});
				}

				// Pad
				for (int i = 0; i < amount; i++){
					writer.Write((byte) 0);
				}
			}
		}
	}
}