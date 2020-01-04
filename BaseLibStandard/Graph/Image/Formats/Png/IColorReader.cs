namespace BaseLibS.Graph.Image.Formats.Png{
	public interface IColorReader{
		void ReadScanline(byte[] scanline, Color2[] pixels, PngHeader header);
	}
}