namespace BaseLibS.Graph.Image.Formats.Jpg{
	internal static class JpegConstants{
		public const ushort MaxLength = 65535;
		public static readonly byte[] ChromaFourFourFourHorizontal = {0x11, 0x11, 0x11};
		public static readonly byte[] ChromaFourFourFourVertical = {0x11, 0x11, 0x11};
		public static readonly byte[] ChromaFourTwoTwoHorizontal = {0x22, 0x11, 0x11};
		public static readonly byte[] ChromaFourTwoTwoVertical = {0x11, 0x11, 0x11};
		public static readonly byte[] ChromaFourTwoZeroHorizontal = {0x22, 0x11, 0x11};
		public static readonly byte[] ChromaFourTwoZeroVertical = {0x22, 0x11, 0x11};
		internal static class Components{
			public const byte Y = 1;
			public const byte Cb = 2;
			public const byte Cr = 3;
			public const byte I = 4;
			public const byte Q = 5;
		}
		internal static class Markers{
			public const byte XFF = 0xff;
			public const byte SOI = 0xd8;
			public const byte SOF0 = 0xc0;
			public const byte SOF1 = 0xc1;
			public const byte SOF2 = 0xc2;
			public const byte DHT = 0xc4;
			public const byte DQT = 0xdb;
			public const byte DRI = 0xdd;
			public const byte RST0 = 0xd0;
			public const byte RST7 = 0xd7;
			public const byte SOS = 0xda;
			public const byte COM = 0xfe;
			public const byte EOI = 0xd9;
			public const byte APP0 = 0xe0;
			public const byte APP1 = 0xe1;
			public const byte APP14 = 0xee;
			public const byte APP15 = 0xef;
		}
	}
}