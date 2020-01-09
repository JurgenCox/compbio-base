using System;

namespace BaseLibS.Graph.Image{
	public abstract class ImageBase : IImageBase{
		protected ImageBase(){}

		protected ImageBase(int width, int height){
			if (width <= 0 || height <= 0){
				throw new ArgumentOutOfRangeException();
			}
			Width = width;
			Height = height;
			Pixels = new Color2[width*height];
		}

		protected ImageBase(ImageBase other){
			if (other == null){
				throw new ArgumentNullException();
			}
			Width = other.Width;
			Height = other.Height;
			Quality = other.Quality;
			FrameDelay = other.FrameDelay;
			Pixels = new Color2[Width*Height];
			Array.Copy(other.Pixels, Pixels, other.Pixels.Length);
		}

		public int MaxWidth { get; set; } = int.MaxValue;
		public int MaxHeight { get; set; } = int.MaxValue;
		public Color2[] Pixels { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public double PixelRatio => (double) Width/Height;
		public RectangleI2 Bounds => new RectangleI2(0, 0, Width, Height);
		public int Quality { get; set; }
		public int FrameDelay { get; set; }

		public void SetPixels(int width, int height, Color2[] pixels){
			if (width <= 0){
				throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than or equals than zero.");
			}
			if (height <= 0){
				throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than or equal than zero.");
			}
			if (pixels.Length != width*height){
				throw new ArgumentException("Pixel array must have the length of Width * Height.");
			}
			Width = width;
			Height = height;
			Pixels = pixels;
		}

		public void ClonePixels(int width, int height, Color2[] pixels){
			if (width <= 0){
				throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than or equals than zero.");
			}
			if (height <= 0){
				throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than or equal than zero.");
			}
			if (pixels.Length != width*height){
				throw new ArgumentException("Pixel array must have the length of Width * Height.");
			}
			Width = width;
			Height = height;
			Pixels = new Color2[pixels.Length];
			Array.Copy(pixels, Pixels, pixels.Length);
		}

		public abstract IPixelAccessor Lock();
	}
}