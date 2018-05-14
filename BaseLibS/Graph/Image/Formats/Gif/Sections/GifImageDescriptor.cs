namespace BaseLibS.Graph.Image.Formats.Gif.Sections{
	internal sealed class GifImageDescriptor{
		public short Left { get; set; }
		public short Top { get; set; }
		public short Width { get; set; }
		public short Height { get; set; }
		public bool LocalColorTableFlag { get; set; }
		public int LocalColorTableSize { get; set; }
		public bool InterlaceFlag { get; set; }
	}
}