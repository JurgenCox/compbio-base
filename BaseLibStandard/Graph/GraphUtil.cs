using System;
using System.Collections.Generic;
using System.Text;
using BaseLibS.Util;

namespace BaseLibS.Graph {
	public static class GraphUtil {
		public const bool isJavaScript = false;
		public const int scrollBarWidth = 18;
		public const int minBarSize = 5;
		public const int zoomButtonSize = 14;
		public const float zoomStep = 1.2f;
		public const int maxOverviewSize = 100;
		public static readonly Color2 buttonColor = Color2.CornflowerBlue;
		public static readonly Brush2 buttonBrush = new Brush2(buttonColor);
		public static readonly Brush2 buttonBrushHighlight = new Brush2(Color2.Lighter(buttonColor, 30));
		public static readonly Brush2 buttonBrushPress = new Brush2(Color2.Darker(buttonColor, 30));

		public static void PaintMoveButtons(IGraphics g, int width, int height, MoveButtonState state) {
			const int bsize = zoomButtonSize;
			g.SmoothingMode = SmoothingMode2.AntiAlias;
			Brush2 b = buttonBrush;
			switch (state) {
				case MoveButtonState.HighlightUp:
					b = buttonBrushHighlight;
					break;
				case MoveButtonState.PressUp:
					b = buttonBrushPress;
					break;
			}
			PaintMoveUpButton(g, b, width - 3 * bsize, height - 2 * bsize - 8, bsize);
			b = buttonBrush;
			switch (state) {
				case MoveButtonState.HighlightLeft:
					b = buttonBrushHighlight;
					break;
				case MoveButtonState.PressLeft:
					b = buttonBrushPress;
					break;
			}
			PaintMoveLeftButton(g, b, (int) (width - 3.5 * bsize - 2), (int) (height - 1.5f * bsize - 6), bsize);
			b = buttonBrush;
			switch (state) {
				case MoveButtonState.HighlightRight:
					b = buttonBrushHighlight;
					break;
				case MoveButtonState.PressRight:
					b = buttonBrushPress;
					break;
			}
			PaintMoveRightButton(g, b, (int) (width - 2.5 * bsize + 2), (int) (height - 1.5f * bsize - 6), bsize);
			b = buttonBrush;
			switch (state) {
				case MoveButtonState.HighlightDown:
					b = buttonBrushHighlight;
					break;
				case MoveButtonState.PressDown:
					b = buttonBrushPress;
					break;
			}
			PaintMoveDownButton(g, b, width - 3 * bsize, height - bsize - 4, bsize);
			g.SmoothingMode = SmoothingMode2.Default;
		}

		public static void PaintZoomButtons(IGraphics g, int width, int height, ZoomButtonState state) {
			const int bsize = zoomButtonSize;
			g.SmoothingMode = SmoothingMode2.AntiAlias;
			Brush2 b = buttonBrush;
			switch (state) {
				case ZoomButtonState.HighlightPlus:
					b = buttonBrushHighlight;
					break;
				case ZoomButtonState.PressPlus:
					b = buttonBrushPress;
					break;
			}
			PaintPlusZoomButton(g, b, width - bsize - 4, height - 2 * bsize - 8, bsize);
			b = buttonBrush;
			switch (state) {
				case ZoomButtonState.HighlightMinus:
					b = buttonBrushHighlight;
					break;
				case ZoomButtonState.PressMinus:
					b = buttonBrushPress;
					break;
			}
			PaintMinusZoomButton(g, b, width - bsize - 4, height - bsize - 4, bsize);
			g.SmoothingMode = SmoothingMode2.Default;
		}

		public static void PaintMoveUpButton(IGraphics g, Brush2 b, int x, int y, int bsize) {
			Pen2 w = new Pen2(Color2.White, 2);
			PaintRoundButton(g, b, w, x, y, bsize);
			g.FillClosedCurve(Brushes2.White,
				new[] {
					new Point2(x + bsize / 2, y + 4), new Point2(x + 5, y + bsize - 4), new Point2(x + bsize - 5, y + bsize - 4)
				});
		}

		public static void PaintMoveDownButton(IGraphics g, Brush2 b, int x, int y, int bsize) {
			Pen2 w = new Pen2(Color2.White, 2);
			PaintRoundButton(g, b, w, x, y, bsize);
			g.FillClosedCurve(Brushes2.White,
				new[] {new Point2(x + bsize / 2, y + bsize - 4), new Point2(x + 5, y + 4), new Point2(x + bsize - 5, y + 4)});
		}

		public static void PaintMoveLeftButton(IGraphics g, Brush2 b, int x, int y, int bsize) {
			Pen2 w = new Pen2(Color2.White, 2);
			PaintRoundButton(g, b, w, x, y, bsize);
			g.FillClosedCurve(Brushes2.White,
				new[] {
					new Point2(x + 4, y + bsize / 2), new Point2(x + bsize - 4, y + 5), new Point2(x + bsize - 4, y + bsize - 5)
				});
		}

		public static void PaintMoveRightButton(IGraphics g, Brush2 b, int x, int y, int bsize) {
			Pen2 w = new Pen2(Color2.White, 2);
			PaintRoundButton(g, b, w, x, y, bsize);
			g.FillClosedCurve(Brushes2.White,
				new[] {new Point2(x + bsize - 4, y + bsize / 2), new Point2(x + 4, y + 5), new Point2(x + 4, y + bsize - 5)});
		}

		public static void PaintPlusZoomButton(IGraphics g, Brush2 b, int x, int y, int bsize) {
			Pen2 w = new Pen2(Color2.White, 2);
			PaintRoundButton(g, b, w, x, y, bsize);
			g.DrawLine(w, x + 4, y + bsize / 2, x + bsize - 4, y + bsize / 2);
			g.DrawLine(w, x + bsize - bsize / 2, y + 4, x + bsize / 2, y + bsize - 4);
		}

		public static void PaintMinusZoomButton(IGraphics g, Brush2 b, int x, int y, int bsize) {
			Pen2 w = new Pen2(Color2.White, 2);
			PaintRoundButton(g, b, w, x, y, bsize);
			g.DrawLine(w, x + 4, y + bsize / 2, x + bsize - 4, y + bsize / 2);
		}

		public static void PaintRoundButton(IGraphics g, Brush2 b, Pen2 w, int x, int y, int size) {
			PaintRoundButton(g, b, w, x, y, size, false);
		}

		public static void PaintRoundButton(IGraphics g, Brush2 b, Pen2 w, int x, int y, int size, bool selected) {
			if (selected) {
				g.FillEllipse(Brushes2.Red, x - 1, y - 1, size + 2, size + 2);
			}
			g.FillEllipse(b, x, y, size, size);
			g.DrawEllipse(w, x + 2, y + 2, size - 4, size - 4);
		}

		public static Rectangle2 CalcWin(Size2 overview, SizeI2 totalSize, RectangleI2 visibleWin, float zoomFactorX,
			float zoomFactorY) {
			float winX = visibleWin.X * overview.Width / totalSize.Width;
			float winWidth = visibleWin.Width * overview.Width / totalSize.Width / zoomFactorX;
			if (winWidth > overview.Width - winX) {
				winWidth = overview.Width - winX;
			}
			float winY = visibleWin.Y * overview.Height / totalSize.Height;
			float winHeight = visibleWin.Height * overview.Height / totalSize.Height / zoomFactorY;
			if (winY < 0) {
				winY = 0;
			}
			if (winHeight > overview.Height - winY) {
				winHeight = overview.Height - winY;
			}
			return new Rectangle2(winX, winY, winWidth, winHeight);
		}

		public static void PaintOverview(IGraphics g, SizeI2 totalSize, RectangleI2 visibleWin,
			Func<int, int, Bitmap2> getOverviewBitmap, float zoomFactorX, float zoomFactorY, bool overviewTopRight) {
			if (getOverviewBitmap == null) {
				return;
			}
			Size2 overview = CalcOverviewSize(visibleWin.Width, visibleWin.Height, totalSize.Width, totalSize.Height);
			Rectangle2 win = CalcWin(overview, totalSize, visibleWin, zoomFactorX, zoomFactorY);
			float xpos = overviewTopRight ? visibleWin.Width - overview.Width - 1 : 0;
			float ypos = overviewTopRight ? 1 : visibleWin.Height - overview.Height;
			g.FillRectangle(Brushes2.White, xpos, ypos, overview.Width, overview.Height);
			Bitmap2 bm = getOverviewBitmap((int) overview.Width, (int) overview.Height);
			if (bm == null) {
				return;
			}
			g.DrawImageUnscaled(bm, xpos, ypos);
			Brush2 b = new Brush2(Color2.FromArgb(30, 0, 0, 255));
			if (win.X > 0) {
				g.FillRectangle(b, xpos, ypos, win.X, overview.Height);
			}
			if (overview.Width - win.X - win.Width > 0) {
				g.FillRectangle(b, xpos + win.X + win.Width, ypos, overview.Width - win.X - win.Width, overview.Height);
			}
			if (win.Y > 0) {
				g.FillRectangle(b, xpos + win.X, ypos, win.Width, win.Y);
			}
			if (overview.Height - win.Y - win.Height > 0) {
				g.FillRectangle(b, xpos + win.X, ypos + win.Y + win.Height - 1, win.Width, overview.Height - win.Y - win.Height);
			}
			g.DrawRectangle(Pens2.Black, xpos, ypos - 1, overview.Width, overview.Height);
			g.DrawRectangle(Pens2.Blue, xpos + win.X, ypos - 1 + win.Y, win.Width, win.Height);
			if (win.Width < 5 && win.Height < 5) {
				g.DrawLine(Pens2.Red, xpos + win.X, ypos + win.Y, xpos + win.X + 5, ypos + win.Y);
				g.DrawLine(Pens2.Red, xpos + win.X, ypos + win.Y, xpos + win.X, ypos + win.Y + 5);
				g.DrawLine(Pens2.Red, xpos + win.X, ypos + win.Y, xpos + win.X + 7, ypos + win.Y + 7);
			}
		}

		public static Size2 CalcOverviewSize(int width, int height, int totalWidth, int totalHeight) {
			int maxSize = Math.Min(Math.Min(maxOverviewSize, height), width - 20);
			Size2 x = totalWidth > totalHeight
				? new Size2(maxSize, Math.Max(1, (int) Math.Round(totalHeight / (float) totalWidth * maxSize)))
				: new Size2(Math.Max(1, (int) Math.Round(totalWidth / (float) totalHeight * maxSize)), maxSize);
			return x;
		}

		public static bool HitsAMoveButton(int x, int y, int width, int height) {
			if (HitsUpButton(x, y, width, height)) {
				return true;
			}
			if (HitsDownButton(x, y, width, height)) {
				return true;
			}
			return HitsLeftButton(x, y, width, height) || HitsRightButton(x, y, width, height);
		}

		public static bool HitsAZoomButton(int x, int y, int width, int height) {
			return HitsPlusButton(x, y, width, height) || HitsMinusButton(x, y, width, height);
		}

		public static bool HitsUpButton(int x, int y, int width, int height) {
			return HitsButton(x, y, width - 2.5f * zoomButtonSize, height - 8 - 1.5f * zoomButtonSize);
		}

		public static bool HitsDownButton(int x, int y, int width, int height) {
			return HitsButton(x, y, width - 2.5f * zoomButtonSize, height - 4 - 0.5f * zoomButtonSize);
		}

		public static bool HitsLeftButton(int x, int y, int width, int height) {
			return HitsButton(x, y, width + 2 - 3f * zoomButtonSize, height - 6 - zoomButtonSize);
		}

		public static bool HitsRightButton(int x, int y, int width, int height) {
			return HitsButton(x, y, width - 2 - 2f * zoomButtonSize, height - 6 - zoomButtonSize);
		}

		public static bool HitsPlusButton(int x, int y, int width, int height) {
			return HitsButton(x, y, width - 4 - zoomButtonSize / 2f, height - 8 - 1.5f * zoomButtonSize);
		}

		public static bool HitsMinusButton(int x, int y, int width, int height) {
			return HitsButton(x, y, width - 4 - zoomButtonSize / 2f, height - 4 - 0.5f * zoomButtonSize);
		}

		public static bool HitsButton(int x, int y, float cx, float cy) {
			return (x - cx) * (x - cx) + (y - cy) * (y - cy) <= zoomButtonSize * zoomButtonSize * 0.5 * 0.5;
		}

		public static ZoomButtonState GetNewZoomButtonState(int x, int y, int width, int height, bool press) {
			if (HitsMinusButton(x, y, width, height)) {
				return press ? ZoomButtonState.PressMinus : ZoomButtonState.HighlightMinus;
			}
			if (HitsPlusButton(x, y, width, height)) {
				return press ? ZoomButtonState.PressPlus : ZoomButtonState.HighlightPlus;
			}
			return ZoomButtonState.Neutral;
		}

		public static MoveButtonState GetNewMoveButtonState(int x, int y, int width, int height, bool press) {
			if (HitsUpButton(x, y, width, height)) {
				return press ? MoveButtonState.PressUp : MoveButtonState.HighlightUp;
			}
			if (HitsDownButton(x, y, width, height)) {
				return press ? MoveButtonState.PressDown : MoveButtonState.HighlightDown;
			}
			if (HitsLeftButton(x, y, width, height)) {
				return press ? MoveButtonState.PressLeft : MoveButtonState.HighlightLeft;
			}
			if (HitsRightButton(x, y, width, height)) {
				return press ? MoveButtonState.PressRight : MoveButtonState.HighlightRight;
			}
			return MoveButtonState.Neutral;
		}

		private static readonly Color2[] predefinedColors = {
			Color2.Blue, Color2.FromArgb(255, 144, 144), Color2.FromArgb(255, 0, 255), Color2.FromArgb(168, 156, 82),
			Color2.LightBlue, Color2.Orange, Color2.Cyan, Color2.Pink, Color2.Turquoise, Color2.LightGreen, Color2.Brown,
			Color2.DarkGoldenrod, Color2.DeepPink, Color2.LightSkyBlue, Color2.BlueViolet, Color2.Crimson
		};

		public static Font2 defaultFont = new Font2("Lucida Sans Unicode", 8F, FontStyle2.Regular);

		public static Color2 GetPredefinedColor(int index) {
			return predefinedColors[Math.Abs(index % predefinedColors.Length)];
		}

		public static void FillShadedRectangle(Bitmap2 b, int w, int h) {
			b.FillRectangle(Color2.White, 0, 0, w - 1, h - 1);
			b.SetPixel(1, 1, Color2.FromArgb(230, 238, 252));
			b.SetPixel(1, h - 3, Color2.FromArgb(219, 227, 248));
			b.SetPixel(w - 3, 1, Color2.FromArgb(220, 230, 249));
			b.SetPixel(w - 3, h - 3, Color2.FromArgb(217, 227, 246));
			b.SetPixel(w - 1, h - 3, Color2.FromArgb(174, 192, 214));
			b.SetPixel(w - 2, h - 2, Color2.FromArgb(174, 196, 219));
			b.SetPixel(0, h - 2, Color2.FromArgb(195, 212, 231));
			b.SetPixel(0, h - 1, Color2.FromArgb(237, 241, 243));
			b.SetPixel(w - 2, h - 1, Color2.FromArgb(236, 242, 247));
			b.SetPixel(w - 1, h - 1, Color2.FromArgb(255, 255, 255));
			b.SetPixel(w - 1, h - 2, Color2.FromArgb(255, 255, 255));
			int wi = w - 5;
			int he = h - 5;
			int[][] upper = InterpolateRgb(225, 234, 254, 188, 206, 250, wi);
			int[][] lower = InterpolateRgb(183, 203, 249, 174, 200, 247, wi);
			for (int i = 0; i < wi; i++) {
				int[][] pix = InterpolateRgb(upper[0][i], upper[1][i], upper[2][i], lower[0][i], lower[1][i], lower[2][i], he);
				for (int j = 0; j < he; j++) {
					b.SetPixel(i + 2, j + 2, Color2.FromArgb(pix[0][j], pix[1][j], pix[2][j]));
				}
			}
			int[][] pix2 = InterpolateRgb(208, 223, 252, 170, 192, 243, he);
			for (int j = 0; j < he; j++) {
				b.SetPixel(1, j + 2, Color2.FromArgb(pix2[0][j], pix2[1][j], pix2[2][j]));
			}
			pix2 = InterpolateRgb(185, 202, 243, 176, 197, 242, he);
			for (int j = 0; j < he; j++) {
				b.SetPixel(w - 3, j + 2, Color2.FromArgb(pix2[0][j], pix2[1][j], pix2[2][j]));
			}
			pix2 = InterpolateRgb(208, 223, 252, 175, 197, 244, wi);
			for (int i = 0; i < wi; i++) {
				b.SetPixel(i + 2, 1, Color2.FromArgb(pix2[0][i], pix2[1][i], pix2[2][i]));
			}
			pix2 = InterpolateRgb(183, 198, 241, 176, 196, 242, wi);
			for (int i = 0; i < wi; i++) {
				b.SetPixel(i + 2, h - 3, Color2.FromArgb(pix2[0][i], pix2[1][i], pix2[2][i]));
			}
			pix2 = InterpolateRgb(238, 237, 229, 160, 181, 211, he + 2);
			for (int i = 0; i < he + 2; i++) {
				b.SetPixel(w - 1, i, Color2.FromArgb(pix2[0][i], pix2[1][i], pix2[2][i]));
			}
			pix2 = InterpolateRgb(170, 192, 225, 126, 159, 211, w / 2);
			for (int i = 1; i <= w / 2; i++) {
				b.SetPixel(i, h - 1, Color2.FromArgb(pix2[0][i - 1], pix2[1][i - 1], pix2[2][i - 1]));
			}
			pix2 = InterpolateRgb(126, 159, 211, 148, 176, 221, w - 3 - w / 2);
			for (int i = w / 2 + 1; i <= w - 3; i++) {
				b.SetPixel(i, h - 1, Color2.FromArgb(pix2[0][i - w / 2 - 1], pix2[1][i - w / 2 - 1], pix2[2][i - w / 2 - 1]));
			}
		}

		public static int[][] InterpolateRgb(int start0, int start1, int start2, int end0, int end1, int end2, int n) {
			if (n == 0) {
				return new[] {new int[0], new int[0], new int[0]};
			}
			if (n == 1) {
				int r1 = (start0 + end0) / 2;
				int g1 = (start1 + end1) / 2;
				int b1 = (start2 + end2) / 2;
				return new[] {new[] {r1}, new[] {g1}, new[] {b1}};
			}
			int[] r = new int[n];
			int[] g = new int[n];
			int[] b = new int[n];
			double rstep = (end0 - start0) / (n - 1.0);
			double gstep = (end1 - start1) / (n - 1.0);
			double bstep = (end2 - start2) / (n - 1.0);
			for (int i = 0; i < n; i++) {
				r[i] = (int) Math.Round(start0 + i * rstep);
				g[i] = (int) Math.Round(start1 + i * gstep);
				b[i] = (int) Math.Round(start2 + i * bstep);
			}
			return new[] {r, g, b};
		}

		public static string[] WrapString(IGraphics g, string s, int width, Font2 font) {
			if (width < 20) {
				return new[] {s};
			}
			if (g.MeasureString(s, font).Width < width - 7) {
				return new[] {s};
			}
			s = StringUtils.ReduceWhitespace(s);
			string[] q = s.Split(' ');
			List<string> result = new List<string>();
			string current = q[0];
			for (int i = 1; i < q.Length; i++) {
				string next = current + " " + q[i];
				if (g.MeasureString(next, font).Width > width - 7) {
					result.Add(current);
					current = q[i];
				} else {
					current += " " + q[i];
				}
			}
			result.Add(current);
			return result.ToArray();
		}

		public static string GetStringValue(IGraphics g, string s, int width, Font2 font) {
			if (width < 20) {
				return "";
			}
			if (g.MeasureString(s, font).Width < width - 7) {
				return s;
			}
			StringBuilder sb = new StringBuilder();
			foreach (char t in s) {
				if (g.MeasureString(sb.ToString(), font).Width < width - 21) {
					sb.Append(t);
				} else {
					break;
				}
			}
			return sb + "...";
		}
	}
}