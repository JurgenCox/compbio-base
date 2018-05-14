namespace BaseLibS.Graph.Image.Formats.Gif{
	internal sealed class GifConstants{
		public const string fileType = "GIF";
		public const string fileVersion = "89a";
		public const byte extensionIntroducer = 0x21;
		public const byte graphicControlLabel = 0xF9;
		public const byte applicationExtensionLabel = 0xFF;
		public const string applicationIdentification = "NETSCAPE2.0";
		public const byte applicationBlockSize = 0x0b;
		public const byte commentLabel = 0xFE;
		public const int maxCommentLength = 1024*8;
		public const byte imageDescriptorLabel = 0x2C;
		public const byte plainTextLabel = 0x01;
		public const byte imageLabel = 0x2C;
		public const byte terminator = 0;
		public const byte endIntroducer = 0x3B;
	}
}