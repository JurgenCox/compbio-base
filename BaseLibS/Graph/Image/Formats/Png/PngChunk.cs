namespace BaseLibS.Graph.Image.Formats.Png{
	internal sealed class PngChunk{
		public int Length { get; set; }
		public string Type { get; set; }
		public byte[] Data { get; set; }
		public uint Crc { get; set; }
	}
}