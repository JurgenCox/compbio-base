using System;

namespace BaseLibS.Graph.Image {
	public sealed class ColorPixelAccessor : IPixelAccessor {
		private Color2[] pixelsBase;
		private bool isDisposed;

		public ColorPixelAccessor(IImageBase image) {
			if (image == null) {
				throw new ArgumentNullException();
			}
			if (image.Width <= 0 || image.Height <= 0) {
				throw new ArgumentOutOfRangeException();
			}
			Width = image.Width;
			Height = image.Height;
			pixelsBase = ((ImageBase) image).Pixels;
		}

		~ColorPixelAccessor() {
			Dispose();
		}

		public int Width { get; }
		public int Height { get; }

		public Color2 this[int x, int y] {
			get => pixelsBase[y * Width + x];
			set => pixelsBase[y * Width + x] = value;
		}

		public void Dispose() {
			if (isDisposed) {
				return;
			}
			pixelsBase = null;
			isDisposed = true;
			GC.SuppressFinalize(this);
		}
	}
}