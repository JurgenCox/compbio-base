using System;
using System.Threading;
using BaseLibS.Graph;

namespace BaseLib.Forms.Scroll {
	internal sealed class HorizontalScrollBarView : ScrollBarView {
		private Thread leftThread;
		private Thread rightThread;
		internal HorizontalScrollBarView(IScrollableControl main, float sfx) : base(main, sfx) { }

		public override void OnPaintBackground(IGraphics g, int width, int height) {
			Pen2 p = new Pen2(Color2.FromArgb(172, 168, 153));
			g.DrawLine(p, 0, height - 1, width, height - 1);
			int[][] rgbs = GraphUtil.InterpolateRgb(243, 241, 236, 254, 254, 251, ScrollBarWidth - 2);
			for (int i = 0; i < ScrollBarWidth - 2; i++) {
				p = new Pen2(Color2.FromArgb(rgbs[0][i], rgbs[1][i], rgbs[2][i]));
				g.DrawLine(p, 0, i + 1, width, i + 1);
			}
			p = new Pen2(Color2.FromArgb(238, 237, 229));
			g.DrawLine(p, 0, 0, width, 0);
			g.DrawLine(p, 0, height - 2, width, height - 2);
		}

		public override void OnPaint(IGraphics g, int width, int height) {
			PaintFirstMark(g, ScrollBarWidth);
			PaintSecondMark(g, ScrollBarWidth, width);
			PaintBar(g, ScrollBarWidth, width);
		}

		private void PaintBar(IGraphics g, int scrollBarWid, int width) {
			if (!HasBar) {
				return;
			}
			if (bar != null && CalcBarSize(width) != bar.Width) {
				bar = null;
			}
			if (bar == null) {
				CreateBar(scrollBarWid, width);
			}
			int h = CalcBarStart(width);
			switch (state) {
				case ScrollBarState.HighlightBar:
					g.DrawImageUnscaled(barHighlight, h, 1);
					break;
				case ScrollBarState.PressBar:
					g.DrawImageUnscaled(barPress, h, 1);
					break;
				default:
					g.DrawImageUnscaled(bar, h, 1);
					break;
			}
		}

		private bool HasBar => main.VisibleWidth / main.ZoomFactor < main.TotalWidth();

		private void PaintFirstMark(IGraphics g, int scrollBarWid) {
			if (firstMark == null) {
				CreateFirstMark(scrollBarWid);
			}
			switch (state) {
				case ScrollBarState.HighlightFirstBox:
					g.DrawImageUnscaled(firstMarkHighlight, 0, 1);
					break;
				case ScrollBarState.PressFirstBox:
					g.DrawImageUnscaled(firstMarkPress, 0, 1);
					break;
				default:
					g.DrawImageUnscaled(firstMark, 0, 1);
					break;
			}
		}

		private void PaintSecondMark(IGraphics g, int scrollBarWid, int width) {
			if (secondMark == null) {
				CreateSecondMark(scrollBarWid);
			}
			int h = scrollBarWid - 1;
			switch (state) {
				case ScrollBarState.HighlightSecondBox:
					g.DrawImageUnscaled(secondMarkHighlight, width - h, 1);
					break;
				case ScrollBarState.PressSecondBox:
					g.DrawImageUnscaled(secondMarkPress, width - h, 1);
					break;
				default:
					g.DrawImageUnscaled(secondMark, width - h, 1);
					break;
			}
		}

		private void CreateBar(int scrollBarWid, int width) {
			int w = CalcBarSize(width);
			int h = scrollBarWid - 2;
			Bitmap2 b = new Bitmap2(w, h);
			GraphUtil.FillShadedRectangle(b, w, h);
			bar = b;
			barHighlight = b.Lighter();
			barPress = b.Darker();
		}

		private int CalcBarStart(int width) {
			int hx = width - 2 * ScrollBarWidth + 2;
			if (hx <= 0) {
				return ScrollBarWidth - 1;
			}
			int w = (int) Math.Round(hx * (main.VisibleX / (double) (main.TotalWidth() - 1)));
			w = Math.Min(w, hx - MinBarSize);
			return Math.Max(0, w) + ScrollBarWidth - 1;
		}

		private int CalcBarEnd(int width) {
			int hx = width - 2 * ScrollBarWidth + 2;
			if (hx <= 0) {
				return ScrollBarWidth - 1;
			}
			return Math.Min(hx,
				       (int) Math.Round(hx * ((main.VisibleX + main.VisibleWidth / main.ZoomFactor) /
				                              (double) (main.TotalWidth() - 1)))) + ScrollBarWidth - 1;
		}

		private int CalcBarSize(int width) {
			int hx = width - 2 * ScrollBarWidth + 2;
			return hx <= 0 ? MinBarSize : Math.Max(MinBarSize, CalcBarEnd(width) - CalcBarStart(width));
		}

		private void CreateFirstMark(int scrollBarWid) {
			int w = scrollBarWid - 1;
			int h = scrollBarWid - 2;
			Bitmap2 b = new Bitmap2(w, h);
			GraphUtil.FillShadedRectangle(b, w, h);
			firstMark = b;
			firstMarkHighlight = b.Lighter();
			firstMarkPress = b.Darker();
		}

		private void CreateSecondMark(int scrollBarWid) {
			int w = scrollBarWid - 1;
			int h = scrollBarWid - 2;
			Bitmap2 b = new Bitmap2(w, h);
			GraphUtil.FillShadedRectangle(b, w, h);
			secondMark = b;
			secondMarkHighlight = b.Lighter();
			secondMarkPress = b.Darker();
		}

		public override void OnMouseMoved(BasicMouseEventArgs e) {
			ScrollBarState newState = ScrollBarState.Neutral;
			if (e.X < ScrollBarWidth - 1) {
				newState = ScrollBarState.HighlightFirstBox;
			} else if (e.X > e.Width - ScrollBarWidth) {
				newState = ScrollBarState.HighlightSecondBox;
			} else if (HasBar) {
				int s = CalcBarStart(e.Width);
				int l = CalcBarSize(e.Width);
				if (e.X >= s && e.X <= s + l) {
					newState = ScrollBarState.HighlightBar;
				}
			}
			if (newState != state) {
				state = newState;
				Invalidate();
			}
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e) {
			ScrollBarState newState = ScrollBarState.Neutral;
			if (e.X < ScrollBarWidth - 1) {
				newState = ScrollBarState.PressFirstBox;
				MoveLeft(main.DeltaX());
				leftThread = new Thread(() => WalkLeft(main.DeltaX()));
				leftThread.Start();
			} else if (e.X > e.Width - ScrollBarWidth) {
				newState = ScrollBarState.PressSecondBox;
				MoveRight(main.DeltaX());
				rightThread = new Thread(() => WalkRight(main.DeltaX()));
				rightThread.Start();
			} else if (HasBar) {
				int s = CalcBarStart(e.Width);
				int l = CalcBarSize(e.Width);
				if (e.X >= s && e.X <= s + l) {
					newState = ScrollBarState.PressBar;
					dragStart = e.X;
					visibleDragStart = main.VisibleX;
				} else if (e.X < s) {
					MoveLeft((int) (main.VisibleWidth / main.ZoomFactor));
					leftThread = new Thread(() => WalkLeft((int) (main.VisibleWidth / main.ZoomFactor)));
					leftThread.Start();
				} else {
					MoveRight((int) (main.VisibleWidth / main.ZoomFactor));
					rightThread = new Thread(() => WalkRight((int) (main.VisibleWidth / main.ZoomFactor)));
					rightThread.Start();
				}
			}
			if (newState != state) {
				state = newState;
				Invalidate();
			}
		}

		private void MoveLeft(int delta) {
			main.VisibleX = Math.Max(0, main.VisibleX - delta);
		}

		private void MoveRight(int delta) {
			main.VisibleX = (int) Math.Min(main.TotalWidth() - main.VisibleWidth / main.ZoomFactor, main.VisibleX + delta);
		}

		private void WalkLeft(int delta) {
			Thread.Sleep(400);
			while (true) {
				MoveLeft(delta);
				Invalidate();
				Thread.Sleep(150);
			}
			// ReSharper disable FunctionNeverReturns
		}

		// ReSharper restore FunctionNeverReturns

		private void WalkRight(int delta) {
			Thread.Sleep(400);
			while (true) {
				MoveRight(delta);
				Invalidate();
				Thread.Sleep(150);
			}
			// ReSharper disable FunctionNeverReturns
		}

		// ReSharper restore FunctionNeverReturns

		public override void OnMouseDragged(BasicMouseEventArgs e) {
			if (state != ScrollBarState.PressBar) {
				return;
			}
			int hx = e.Width - 2 * ScrollBarWidth + 2;
			int x = visibleDragStart + (int) Math.Round((e.X - dragStart) / (double) hx * main.TotalWidth());
			x = Math.Max(0, x);
			x = (int) Math.Min(main.TotalWidth() - main.VisibleWidth / main.ZoomFactor, x);
			main.VisibleX = x;
			Invalidate();
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e) {
			if (leftThread != null) {
				leftThread.Abort();
				leftThread = null;
			}
			if (rightThread != null) {
				rightThread.Abort();
				rightThread = null;
			}
			const ScrollBarState newState = ScrollBarState.Neutral;
			OnMouseMoved(e);
			state = newState;
			Invalidate();
		}

		public override void OnMouseLeave(EventArgs e) {
			if (state == ScrollBarState.PressBar) {
				return;
			}
			state = ScrollBarState.Neutral;
			Invalidate();
		}
	}
}