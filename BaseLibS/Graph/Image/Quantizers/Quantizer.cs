using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseLibS.Graph.Image.Quantizers{
	public abstract class Quantizer : IQuantizer {
		private readonly bool singlePass;
		protected Quantizer(bool singlePass){
			this.singlePass = singlePass;
		}
		public int TransparentIndex { get; protected set; } = -1;
		public byte Threshold { get; set; }
		public virtual QuantizedImage Quantize(ImageBase image, int maxColors){
			if (image == null){
				throw new ArgumentNullException();
			}
			int height = image.Height;
			int width = image.Width;
			byte[] quantizedPixels = new byte[width*height];
			List<Color2> palette;
			using (IPixelAccessor pixels = image.Lock()){
				if (!singlePass){
					FirstPass(pixels, width, height);
				}
				palette = GetPalette();
				SecondPass(pixels, quantizedPixels, width, height);
			}
			return new QuantizedImage(width, height, palette.ToArray(), quantizedPixels, TransparentIndex);
		}
		protected virtual void FirstPass(IPixelAccessor source, int width, int height){
			for (int y = 0; y < height; y++){
				for (int x = 0; x < width; x++){
					InitialQuantizePixel(source[x, y]);
				}
			}
		}
		protected virtual void SecondPass(IPixelAccessor source, byte[] output, int width, int height){
			Parallel.For(0, source.Height, Bootstrapper.instance.ParallelOptions, y =>{
				for (int x = 0; x < source.Width; x++){
					Color2 sourcePixel = source[x, y];
					output[y*source.Width + x] = QuantizePixel(sourcePixel);
				}
			});
		}
		protected virtual void InitialQuantizePixel(Color2 pixel){}
		protected abstract byte QuantizePixel(Color2 pixel);
		protected abstract List<Color2> GetPalette();
	}
}