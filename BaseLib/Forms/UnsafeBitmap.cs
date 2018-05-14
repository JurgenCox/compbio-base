using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BaseLib.Forms {
	public struct PixelData {
		public byte blue;
		public byte green;
		public byte red;
	}

	public unsafe class UnsafeBitmap {
		private readonly Graphics graphics;
		private readonly int bitmapHeight;
		private readonly int bitmapWidth;
		private int width;
		private BitmapData bitmapData;
		private byte* pBase = null;
		public Bitmap Bitmap { get; }

		public UnsafeBitmap(Image bitmap) {
			Bitmap = new Bitmap(bitmap);
			graphics = Graphics.FromImage(bitmap);
			bitmapWidth = Bitmap.Width;
			bitmapHeight = Bitmap.Height;
		}

		public UnsafeBitmap(int width, int height) {
			Bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			graphics = Graphics.FromImage(Bitmap);
			bitmapWidth = width;
			bitmapHeight = height;
		}

		public UnsafeBitmap(UnsafeBitmap other) {
			// copy the bitmap
			Bitmap = (Bitmap) other.Bitmap.Clone();
			graphics = Graphics.FromImage(Bitmap);
			bitmapWidth = other.bitmapWidth;
			bitmapHeight = other.bitmapHeight;
			// the remaining parameters are not set, as the bitmap is in unlocked state
		}

		public void Dispose() {
			graphics.Dispose();
			Bitmap.Dispose();
		}

		public void LockBitmap() {
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = Bitmap.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int) boundsF.X, (int) boundsF.Y, (int) boundsF.Width, (int) boundsF.Height);
			width = (int) boundsF.Width * sizeof(PixelData);
			if (width % 4 != 0) {
				width = 4 * (width / 4 + 1);
			}
			bitmapData = Bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			pBase = (byte*) bitmapData.Scan0.ToPointer();
		}

		public PixelData GetPixel(int x, int y) {
			PixelData returnValue = *PixelAt(x, y);
			return returnValue;
		}

		public void SetPixel(int x, int y, Color c) {
			PixelData pd = new PixelData {red = c.R, green = c.G, blue = c.B};
			SetPixel(x, y, pd);
		}

		public void SetPixel(int x, int y, PixelData color) {
			if (Valid(x, y)) {
				PixelData* pixel = PixelAt(x, y);
				*pixel = color;
			}
		}

		public void SetPixelUnchecked(int x, int y, PixelData color) {
			PixelData* pixel = (PixelData*) (pBase + y * width + x * sizeof(PixelData));
			*pixel = color;
		}

		public void MirrorY() {
			for (int i = 0; i < bitmapWidth; i++) {
				for (int j = 0; j < bitmapHeight / 2; j++) {
					PixelData p = GetPixel(i, j);
					SetPixel(i, j, GetPixel(i, bitmapHeight - 1 - j));
					SetPixel(i, bitmapHeight - 1 - j, p);
				}
			}
		}

		public UnsafeBitmap Transpose() {
			UnsafeBitmap result = new UnsafeBitmap(bitmapHeight, bitmapWidth);
			result.LockBitmap();
			for (int i = 0; i < bitmapWidth; i++) {
				for (int j = 0; j < bitmapHeight; j++) {
					result.SetPixel(j, i, GetPixel(i, j));
				}
			}
			return result;
		}

		private bool Valid(int x, int y) {
			if (x == 0 && y == bitmapHeight - 1) {
				return false;
			}
			return x >= 0 && y >= 0 && x < bitmapWidth && y < bitmapHeight;
		}

		public void UnlockBitmap() {
			Bitmap.UnlockBits(bitmapData);
			bitmapData = null;
			pBase = null;
		}

		public PixelData* PixelAt(int x, int y) {
			return (PixelData*) (pBase + y * width + x * sizeof(PixelData));
		}

		public void DrawPath(Color c, int x, int y, int[] xpath, int[] ypath) {
			PixelData pd = new PixelData {red = c.R, green = c.G, blue = c.B};
			for (int i = 0; i < xpath.Length; i++) {
				SetPixel(x + xpath[i], y + ypath[i], pd);
			}
		}

		public void FillRectangle(Color c, int x, int y, int wid, int height) {
			PixelData pd = new PixelData {red = c.R, green = c.G, blue = c.B};
			for (int i = x; i < x + wid; i++) {
				for (int j = y; j < y + height; j++) {
					SetPixel(i, j, pd);
				}
			}
		}

		public void DrawRectangle(Color c, int x, int y, int wid, int height) {
			PixelData pd = new PixelData {red = c.R, green = c.G, blue = c.B};
			const int lw = 1;
			for (int j = x; j <= x + wid; j++) {
				for (int w = 0; w < lw; w++) {
					SetPixel(j, y + w, pd);
					SetPixel(j, y + height - w, pd);
				}
			}
			for (int i = y; i <= y + height; i++) {
				for (int w = 0; w < lw; w++) {
					SetPixel(x + w, i, pd);
					SetPixel(x + wid - w, i, pd);
				}
			}
		}

		public void DrawLine(Color c, int x1, int y1, int x2, int y2, bool dots) {
			DrawLine(c, x1, y1, x2, y2, dots, 1);
		}

		public void DrawLine(Color c, float x1, float y1, float x2, float y2, bool dots) {
			DrawLine(c, x1, y1, x2, y2, dots, 1);
		}

		public void DrawLine(Color c, float x1, float y1, float x2, float y2, bool dots, int width1) {
			DrawLine(c, (int) x1, (int) y1, (int) x2, (int) y2, dots, width1);
		}

		public void DrawLine(Color c, int x1, int y1, int x2, int y2, bool dots, int width1) {
			PixelData pd = new PixelData {red = c.R, green = c.G, blue = c.B};
			float dx = x1 - x2;
			float dy = y1 - y2;
			if (dx == 0 && dy == 0) {
				SetPixel(x1, y1, pd);
				return;
			}
			if (Math.Abs(dx) > Math.Abs(dy)) {
				double a = (y1 - y2) / (double) (x1 - x2);
				for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++) {
					if (!dots || x % 2 == 1) {
						int y = (int) Math.Round(y1 + a * (x - x1));
						for (int b = -((width1 - 1) / 2); b <= width1 / 2; b++) {
							SetPixel(x, y + b, pd);
						}
					}
				}
			} else {
				double a = (x1 - x2) / (double) (y1 - y2);
				for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++) {
					if (!dots || y % 2 == 1) {
						int x = (int) Math.Round(x1 + a * (y - y1));
						for (int b = -((width1 - 1) / 2); b <= width1 / 2; b++) {
							SetPixel(x + b, y, pd);
						}
					}
				}
			}
		}

		public UnsafeBitmap Darker() {
			UnsafeBitmap result = new UnsafeBitmap(bitmapWidth, bitmapHeight);
			result.LockBitmap();
			for (int i = 0; i < bitmapWidth; i++) {
				for (int j = 0; j < bitmapHeight; j++) {
					PixelData p = GetPixel(i, j);
					result.SetPixel(i, j,
						Color.FromArgb(Math.Max(0, p.red - 20), Math.Max(0, p.green - 20), Math.Max(0, p.blue - 20)));
				}
			}
			return result;
		}

		public UnsafeBitmap Lighter() {
			UnsafeBitmap result = new UnsafeBitmap(bitmapWidth, bitmapHeight);
			result.LockBitmap();
			for (int i = 0; i < bitmapWidth; i++) {
				for (int j = 0; j < bitmapHeight; j++) {
					PixelData p = GetPixel(i, j);
					result.SetPixel(i, j,
						Color.FromArgb(Math.Min(255, p.red + 20), Math.Min(255, p.green + 20), Math.Min(255, p.blue + 20)));
				}
			}
			return result;
		}

		public void DrawString(string s, Font f, Brush b, int x, int y) {
			graphics.DrawString(s, f, b, x, y);
		}

		public SizeF MeasureString(string s, Font f) {
			return graphics.MeasureString(s, f);
		}
	}
}