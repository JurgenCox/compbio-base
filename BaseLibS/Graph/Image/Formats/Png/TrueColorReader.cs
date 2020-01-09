namespace BaseLibS.Graph.Image.Formats.Png{
	internal sealed class TrueColorReader : IColorReader{
		private readonly bool useAlpha;
		private int row;
		public TrueColorReader(bool useAlpha){
			this.useAlpha = useAlpha;
		}
		public void ReadScanline(byte[] scanline, Color2[] pixels, PngHeader header) {
			int offset;
			byte[] newScanline = GrayscaleReader.ToArrayByBitsLength(scanline,header.BitDepth);
			if (useAlpha){
				for (int x = 0; x < newScanline.Length; x += 4){
					offset = row*header.Width + (x >> 2);
					byte r = newScanline[x];
					byte g = newScanline[x + 1];
					byte b = newScanline[x + 2];
					byte a = newScanline[x + 3];
					Color2 color = Color2.FromArgb(a,r, g, b);
					pixels[offset] = color;
				}
			} else{
				for (int x = 0; x < newScanline.Length/3; x++){
					offset = (row*header.Width) + x;
					int pixelOffset = x*3;
					byte r = newScanline[pixelOffset];
					byte g = newScanline[pixelOffset + 1];
					byte b = newScanline[pixelOffset + 2];
					Color2 color = Color2.FromArgb(r, g, b);
					pixels[offset] = color;
				}
			}
			row++;
		}
	}
}