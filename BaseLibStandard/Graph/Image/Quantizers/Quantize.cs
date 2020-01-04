namespace BaseLibS.Graph.Image.Quantizers{
	public static class ImageExtensions{
		public static Image2 Quantize(this Image2 source, Quantization mode = Quantization.Octree, int maxColors = 256){
			IQuantizer quantizer;
			switch (mode){
				case Quantization.Wu:
					quantizer = new WuQuantizer();
					break;
				case Quantization.Palette:
					quantizer = new PaletteQuantizer();
					break;
				default:
					quantizer = new OctreeQuantizer();
					break;
			}
			return Quantize(source, quantizer, maxColors);
		}
		public static Image2 Quantize(this Image2 source, IQuantizer quantizer, int maxColors){
			QuantizedImage quantizedImage = quantizer.Quantize(source, maxColors);
			source.SetPixels(source.Width, source.Height, quantizedImage.ToImage().Pixels);
			return source;
		}
	}
}