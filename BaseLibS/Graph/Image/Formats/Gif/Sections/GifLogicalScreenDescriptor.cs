namespace BaseLibS.Graph.Image.Formats.Gif.Sections{
	internal sealed class GifLogicalScreenDescriptor{
		public short Width { get; set; }
		public short Height { get; set; }
		public byte BackgroundColorIndex { get; set; }
		public byte PixelAspectRatio { get; set; }
		public bool GlobalColorTableFlag { get; set; }
		public int GlobalColorTableSize { get; set; }
	}
}