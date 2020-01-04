using System;

namespace BaseLibS.Graph.Image.Formats.Png{
	internal sealed class GrayscaleReader : IColorReader{
		private readonly bool useAlpha;
		private int row;
		public GrayscaleReader(bool useAlpha){
			this.useAlpha = useAlpha;
		}
		public static byte[] ToArrayByBitsLength(byte[] bytes, int bits){
			if (bytes == null){
				throw new ArgumentNullException();
			}
			if (bits <= 0){
				throw new ArgumentOutOfRangeException();
			}
			byte[] result;
			if (bits < 8){
				result = new byte[bytes.Length*8/bits];
				int mask = 0xFF >> (8 - bits);
				int resultOffset = 0;
				foreach (byte b in bytes){
					for (int shift = 0; shift < 8; shift += bits){
						int colorIndex = (b >> (8 - bits - shift)) & mask; // * (255 / factor);
						result[resultOffset] = (byte) colorIndex;
						resultOffset++;
					}
				}
			} else{
				result = bytes;
			}
			return result;
		}
		public void ReadScanline(byte[] scanline, Color2[] pixels, PngHeader header){
			int offset;
			byte[] newScanline = ToArrayByBitsLength(scanline, header.BitDepth);
			if (useAlpha){
				for (int x = 0; x < header.Width/2; x++){
					offset = row*header.Width + x;
					byte rgb = newScanline[x*2];
					byte a = newScanline[(x*2) + 1];
					Color2 color = Color2.FromArgb(a, rgb, rgb, rgb);
					pixels[offset] = color;
				}
			} else{
				for (int x = 0; x < header.Width; x++){
					offset = row*header.Width + x;
					byte rgb = newScanline[x];
					Color2 color = Color2.FromArgb(rgb, rgb, rgb);
					pixels[offset] = color;
				}
			}
			row++;
		}
	}
}