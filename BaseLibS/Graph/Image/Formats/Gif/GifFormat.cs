namespace BaseLibS.Graph.Image.Formats.Gif{
	public class GifFormat : IImageFormat{
		public IImageDecoder Decoder => new GifDecoder();
		public IImageEncoder Encoder => new GifEncoder();
	}
}