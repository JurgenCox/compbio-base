namespace BaseLibS.Graph.Image.Formats{
	public interface IImageFormat{
		IImageEncoder Encoder { get; }
		IImageDecoder Decoder { get; }
	}
}