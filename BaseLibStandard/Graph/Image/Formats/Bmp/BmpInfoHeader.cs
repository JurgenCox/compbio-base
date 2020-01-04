namespace BaseLibS.Graph.Image.Formats.Bmp{
	internal class BmpInfoHeader{
		public const int size = 40;
		public int HeaderSize { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public short Planes { get; set; }
		public short BitsPerPixel { get; set; }
		public BmpCompression Compression { get; set; }
		public int ImageSize { get; set; }
		public int XPelsPerMeter { get; set; }
		public int YPelsPerMeter { get; set; }
		public int ClrUsed { get; set; }
		public int ClrImportant { get; set; }
	}
}