namespace BaseLibS.Graph.Image.Formats.Bmp{
	internal class BmpFileHeader{
		public const int size = 14;
		public short Type { get; set; }
		public int FileSize { get; set; }
		public int Reserved { get; set; }
		public int Offset { get; set; }
	}
}