namespace BaseLibS.Graph.Image.Formats.Png{
	public sealed class PngHeader{
		public int Width { get; set; }
		public int Height { get; set; }
		public byte BitDepth { get; set; }
		public byte ColorType { get; set; }
		public byte CompressionMethod { get; set; }
		public byte FilterMethod { get; set; }
		public byte InterlaceMethod { get; set; }
	}
}