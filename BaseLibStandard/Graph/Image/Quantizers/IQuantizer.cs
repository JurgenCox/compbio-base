namespace BaseLibS.Graph.Image.Quantizers{
	public interface IQuantizer{
		QuantizedImage Quantize(ImageBase image, int maxColors);
		byte Threshold { get; set; }
	}
}