using System;
using System.Threading;
using BaseLibS.Graph;

namespace BaseLib.Forms.Scroll {
	internal sealed class VerticalScrollBarView : ScrollBarView {
		private Thread downThread;
		private Thread upThread;
		internal VerticalScrollBarView(IScrollableControl main, float sfx) : base(main, sfx) { }

		public override void OnPaintBackground(IGraphics g, int width, int height) {
			Pen2 p = new Pen2(Color2.FromArgb(172, 168, 153));
			g.DrawLine(p, width - 1, 0, width - 1, height);
			int[][] rgbs = GraphUtil.InterpolateRgb(243, 241, 236, 254, 254, 251, ScrollBarWidth - 2);
			for (int i = 0; i < ScrollBarWidth - 2; i++) {
				p = new Pen2(Color2.FromArgb(rgbs[0][i], rgbs[1][i], rgbs[2][i]));
				g.DrawLine(p, i + 1, 0, i + 1, height);
			}
			p = new Pen2(Color2.FromArgb(238, 237, 229));
			g.DrawLine(p, 0, 0, 0, height);
			g.DrawLine(p, width - 2, 0, width - 2, height);
		}

		public override void OnPaint(IGraphics g, int width, int height) {
			PaintFirstMark(g, ScrollBarWidth);
			PaintSecondMark(g, ScrollBarWidth, height);
			PaintBar(g, ScrollBarWidth, height);
		}

		private void PaintBar(IGraphics g, int scrollBarWid, int height) {
			if (!HasBar) {
				return;
			}
			if (bar != null && CalcBarSize(height) != bar.Height) {
				bar = null;
			}
			if (bar == null) {
				CreateBar(scrollBarWid, height);
			}
			int h = CalcBarStart(height);
			switch (state) {
				case ScrollBarState.HighlightBar:
					g.DrawImageUnscaled(barHighlight, 1, h);
					break;
				case ScrollBarState.PressBar:
					g.DrawImageUnscaled(barPress, 1, h);
					break;
				default:
					g.DrawImageUnscaled(bar, 1, h);
					break;
			}
		}

		private void PaintFirstMark(IGraphics g, int scrollBarWid) {
			if (firstMark == null) {
				CreateFirstMark(scrollBarWid);
			}
			switch (state) {
				case ScrollBarState.HighlightFirstBox:
					g.DrawImageUnscaled(firstMarkHighlight, 1, 0);
					break;
				case ScrollBarState.PressFirstBox:
					g.DrawImageUnscaled(firstMarkPress, 1, 0);
					break;
				default:
					g.DrawImageUnscaled(firstMark, 1, 0);
					break;
			}
		}

		private void PaintSecondMark(IGraphics g, int scrollBarWid, int height) {
			if (secondMark == null) {
				CreateSecondMark(scrollBarWid);
			}
			int h = scrollBarWid - 1;
			switch (state) {
				case ScrollBarState.HighlightSecondBox:
					g.DrawImageUnscaled(secondMarkHighlight, 1, height - h);
					break;
				case ScrollBarState.PressSecondBox:
					g.DrawImageUnscaled(secondMarkPress, 1, height - h);
					break;
				default:
					g.DrawImageUnscaled(secondMark, 1, height - h);
					break;
			}
		}

		private void CreateBar(int scrollBarWid, int height) {
			int w = scrollBarWid - 2;
			int h = CalcBarSize(height);
			Bitmap2 b = new Bitmap2(w, h);
			GraphUtil.FillShadedRectangle(b, w, h);
			Bitmap2 bh = b.Lighter();
			Bitmap2 bp = b.Darker();
			bar = b;
			barHighlight = bh;
			barPress = bp;
		}

		private int CalcBarStart(int height) {
			int hx = height - 2 * ScrollBarWidth + 2;
			if (hx <= 0) {
				return ScrollBarWidth - 1;
			}
			int w = (int) Math.Round(hx * (main.VisibleY / (double) (main.TotalHeight() - 1)));
			w = Math.Min(w, hx - MinBarSize);
			return Math.Max(0, w) + ScrollBarWidth - 1;
		}

		private int CalcBarEnd(int height) {
			int hx = height - 2 * ScrollBarWidth + 2;
			if (hx <= 0) {
				return ScrollBarWidth - 1;
			}
			return Math.Min(hx,
				       (int) Math.Round(hx * ((main.VisibleY + main.VisibleHeight / main.ZoomFactor) /
				                              (double) (main.TotalHeight() - 1)))) + ScrollBarWidth - 1;
		}

		private int CalcBarSize(int height) {
			int hx = height - 2 * ScrollBarWidth + 2;
			return hx <= 0 ? MinBarSize : Math.Max(MinBarSize, CalcBarEnd(height) - CalcBarStart(height));
		}

		private void CreateFirstMark(int scrollBarWid) {
			int w = scrollBarWid - 2;
			int h = scrollBarWid - 1;
			Bitmap2 b = new Bitmap2(w, h);
			GraphUtil.FillShadedRectangle(b, w, h);
			Bitmap2 bh = b.Lighter();
			Bitmap2 bp = b.Darker();
			firstMark = b;
			firstMarkHighlight = bh;
			firstMarkPress = bp;
		}

		private void CreateSecondMark(int scrollBarWid) {
			int w = scrollBarWid - 2;
			int h = scrollBarWid - 1;
			Bitmap2 b = new Bitmap2(w, h);
			GraphUtil.FillShadedRectangle(b, w, h);
			Bitmap2 bh = b.Lighter();
			Bitmap2 bp = b.Darker();
			secondMark = b;
			secondMarkHighlight = bh;
			secondMarkPress = bp;
		}

		public override void OnMouseMoved(BasicMouseEventArgs e) {
			ScrollBarState newState = ScrollBarState.Neutral;
			if (e.Y < ScrollBarWidth - 1) {
				newState = ScrollBarState.HighlightFirstBox;
			} else if (e.Y > e.Height - ScrollBarWidth) {
				newState = ScrollBarState.HighlightSecondBox;
			} else if (HasBar) {
				int s = CalcBarStart(e.Height);
				int l = CalcBarSize(e.Height);
				if (e.Y >= s && e.Y <= s + l) {
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
			bool ctrl = e.ControlPressed;
			if (e.Y < ScrollBarWidth - 1) {
				if (ctrl) {
					newState = ScrollBarState.PressFirstBox;
					MoveUp(main.DeltaUpToSelection());
				} else {
					newState = ScrollBarState.PressFirstBox;
					MoveUp(main.DeltaY());
					upThread = new Thread(() => WalkUp(main.DeltaY()));
					upThread.Start();
				}
			} else if (e.Y > e.Height - ScrollBarWidth) {
				if (ctrl) {
					newState = ScrollBarState.PressSecondBox;
					MoveDown(main.DeltaDownToSelection());
				} else {
					newState = ScrollBarState.PressSecondBox;
					MoveDown(main.DeltaY());
					downThread = new Thread(() => WalkDown(main.DeltaY()));
					downThread.Start();
				}
			} else if (HasBar) {
				int s = CalcBarStart(e.Height);
				int l = CalcBarSize(e.Height);
				if (e.Y >= s && e.Y <= s + l) {
					newState = ScrollBarState.PressBar;
					dragStart = e.Y;
					visibleDragStart = main.VisibleY;
				} else if (e.Y < s) {
					MoveUp((int) (main.VisibleHeight / main.ZoomFactor));
					upThread = new Thread(() => WalkUp((int) (main.VisibleHeight / main.ZoomFactor)));
					upThread.Start();
				} else {
					MoveDown((int) (main.VisibleHeight / main.ZoomFactor));
					downThread = new Thread(() => WalkDown((int) (main.VisibleHeight / main.ZoomFactor)));
					downThread.Start();
				}
			}
			if (newState != state) {
				state = newState;
				Invalidate();
			}
		}

		private void MoveUp(int delta) {
			main.VisibleY = Math.Max(0, main.VisibleY - delta);
		}

		private void MoveDown(int delta) {
			main.VisibleY = (int) Math.Min(main.TotalHeight() - main.VisibleHeight / main.ZoomFactor, main.VisibleY + delta);
		}

		private void WalkDown(int delta) {
			Thread.Sleep(400);
			while (true) {
				MoveDown(delta);
				Invalidate();
				Thread.Sleep(150);
			}
			// ReSharper disable FunctionNeverReturns
		}

		// ReSharper restore FunctionNeverReturns

		private void WalkUp(int delta) {
			Thread.Sleep(400);
			while (true) {
				MoveUp(delta);
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
			int hx = e.Height - 2 * ScrollBarWidth + 2;
			int y = visibleDragStart + (int) Math.Round((e.Y - dragStart) / (double) hx * main.TotalHeight());
			y = Math.Max(0, y);
			y = (int) Math.Min(main.TotalHeight() - main.VisibleHeight / main.ZoomFactor, y);
			main.VisibleY = y;
			Invalidate();
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e) {
			if (upThread != null) {
				upThread.Abort();
				upThread = null;
			}
			if (downThread != null) {
				downThread.Abort();
				downThread = null;
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

		private bool HasBar => main.VisibleHeight / main.ZoomFactor < main.TotalHeight();
	}
}