namespace BaseLibS.Graph.Image.Formats.Png{
	internal sealed class PaletteIndexReader : IColorReader{
		private readonly byte[] palette;
		private readonly byte[] paletteAlpha;
		private int row;
		public PaletteIndexReader(byte[] palette, byte[] paletteAlpha){
			this.palette = palette;
			this.paletteAlpha = paletteAlpha;
		}
		public void ReadScanline(byte[] scanline, Color2[] pixels, PngHeader header){
			byte[] newScanline = GrayscaleReader.ToArrayByBitsLength(scanline,header.BitDepth);
			int offset, index;
			if (paletteAlpha != null && paletteAlpha.Length > 0){
				for (int i = 0; i < header.Width; i++){
					index = newScanline[i];
					offset = (row*header.Width) + i;
					int pixelOffset = index*3;
					byte r = palette[pixelOffset];
					byte g = palette[pixelOffset + 1];
					byte b = palette[pixelOffset + 2];
					byte a = paletteAlpha.Length > index ? paletteAlpha[index] : (byte) 255;
					Color2 color = Color2.FromArgb(a,r, g, b);
					pixels[offset] = color;
				}
			} else{
				for (int i = 0; i < header.Width; i++){
					index = newScanline[i];
					offset = (row*header.Width) + i;
					int pixelOffset = index*3;
					byte r = palette[pixelOffset];
					byte g = palette[pixelOffset + 1];
					byte b = palette[pixelOffset + 2];
					Color2 color = Color2.FromArgb(r, g, b);
					pixels[offset] = color;
				}
			}
			row++;
		}
	}
}