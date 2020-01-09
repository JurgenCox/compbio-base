namespace BaseLibS.Graph.Image.Formats.Png{
	internal static class PngChunkTypes{
		public const string Header = "IHDR";
		public const string Palette = "PLTE";
		public const string Data = "IDAT";
		public const string End = "IEND";
		public const string PaletteAlpha = "tRNS";
		public const string Text = "tEXt";
		public const string Gamma = "gAMA";
		public const string Physical = "pHYs";
	}
}