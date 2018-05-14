using System;
using System.Threading.Tasks;

namespace BaseLibS.Graph.Image.Quantizers{
	public class QuantizedImage{
		public QuantizedImage(int width, int height, Color2[] palette, byte[] pixels, int transparentIndex = -1){
			if (width <= 0 || height <= 0){
				throw new ArgumentOutOfRangeException();
			}
			if (palette == null || pixels == null){
				throw new ArgumentNullException();
			}
			if (pixels.Length != width*height){
				throw new ArgumentException($"Pixel array size must be {nameof(width)} * {nameof(height)}", nameof(pixels));
			}
			Width = width;
			Height = height;
			Palette = palette;
			Pixels = pixels;
			TransparentIndex = transparentIndex;
		}
		public int Width { get; }
		public int Height { get; }
		public Color2[] Palette { get; }
		public byte[] Pixels { get; }
		public int TransparentIndex { get; }
		public Image2 ToImage(){
			Image2 image = new Image2();
			int pixelCount = Pixels.Length;
			int palletCount = Palette.Length - 1;
			Color2[] pixels = new Color2[pixelCount];
			Parallel.For(0, pixelCount, Bootstrapper.instance.ParallelOptions, i =>{
				Color2 color = Palette[Math.Min(palletCount, Pixels[i])];
				pixels[i] = color;
			});
			image.SetPixels(Width, Height, pixels);
			return image;
		}
	}
}