using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph.Scroll{
	public sealed class SimpleScrollableControlMainView : BasicControlModel{
		private ZoomButtonState state = ZoomButtonState.Neutral;
		private readonly ISimpleScrollableControl main;
		private readonly NavigatorData navigatorData = new NavigatorData();
		public SimpleScrollableControlMainView(ISimpleScrollableControl main){
			this.main = main;
		}
		public override void OnPaint(IGraphics g, int width, int height){
			IGraphics g1 = main.ZoomFactor == 1 ? g : new ScaledGraphics(g, main.ZoomFactor);
			main.OnPaintMainView?.Invoke(g1, main.VisibleX, main.VisibleY, width, height, false);
			if (main.HasZoomButtons){
				GraphUtil.PaintZoomButtons(g, width, height, state);
			}
			if (main.HasOverview){
				GraphUtil.PaintOverview(g, main.TotalSize, main.VisibleWin,
					(overviewWidth, overviewHeight) => main.OverviewBitmap ??
					                                   (main.OverviewBitmap =
						                                   main.CreateOverviewBitmap(overviewWidth, overviewHeight)),
					main.ZoomFactor, main.ZoomFactor, false);
			}
		}
		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveMainView?.Invoke(e.Scale(main.ZoomFactor));
			ZoomButtonState newState = GraphUtil.GetNewZoomButtonState(e.X, e.Y, e.Width, e.Height, false);
			if (newState != state){
				state = newState;
				Invalidate();
			}
		}
		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverMainView?.Invoke(e);
		}
		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveMainView?.Invoke(e);
			state = ZoomButtonState.Neutral;
			Invalidate();
		}
		public override void OnMouseClick(BasicMouseEventArgs e){
			Size2 overview = GraphUtil.CalcOverviewSize(e.Width, e.Height, main.TotalWidth(), main.TotalHeight());
			if (e.X < overview.Width && e.Y > e.Height - overview.Height){
				return;
			}
			if (GraphUtil.HitsAZoomButton(e.X, e.Y, e.Width, e.Height)){
				return;
			}
			main.OnMouseClickMainView?.Invoke(e.Scale(main.ZoomFactor));
		}
		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			Size2 overview = GraphUtil.CalcOverviewSize(e.Width, e.Height, main.TotalWidth(), main.TotalHeight());
			if (e.X < overview.Width && e.Y > e.Height - overview.Height){
				return;
			}
			if (GraphUtil.HitsAZoomButton(e.X, e.Y, e.Width, e.Height)){
				return;
			}
			main.OnMouseDoubleClickMainView?.Invoke(e.Scale(main.ZoomFactor));
		}
		public override void OnMouseIsDown(BasicMouseEventArgs e){
			if (main.HasOverview){
				Size2 overview = GraphUtil.CalcOverviewSize(e.Width, e.Height, main.TotalWidth(), main.TotalHeight());
				if (e.X < overview.Width && e.Y > e.Height - overview.Height) {
					OnMouseIsDownOverview(e.X, (int)(e.Y - e.Height + overview.Height), e.Width, e.Height);
					return;
				}
			}
			if (main.HasZoomButtons){
				ZoomButtonState newState = GraphUtil.GetNewZoomButtonState(e.X, e.Y, e.Width, e.Height, true);
				switch (newState) {
					case ZoomButtonState.PressMinus:
						main.ZoomFactor /= GraphUtil.zoomStep;
						invalidate();
						main.InvalidateScrollbars();
						main.UpdateZoom();
						break;
					case ZoomButtonState.PressPlus:
						main.ZoomFactor *= GraphUtil.zoomStep;
						invalidate();
						main.InvalidateScrollbars();
						main.UpdateZoom();
						break;
					default:
						if (newState != state) {
							invalidate();
						}
						break;
				}
				state = newState;
				if (newState == ZoomButtonState.PressMinus || newState == ZoomButtonState.PressPlus) {
					return;
				}
			}
			main.OnMouseIsDownMainView?.Invoke(e.Scale(main.ZoomFactor));
		}
		private void OnMouseIsDownOverview(int x, int y, int width, int height){
			Size2 overview = GraphUtil.CalcOverviewSize(width, height, main.TotalWidth(), main.TotalHeight());
			Rectangle2 win = GraphUtil.CalcWin(overview, main.TotalSize, main.VisibleWin, main.ZoomFactor,
				main.ZoomFactor);
			if (win.Contains(x, y)){
				navigatorData.Start(x, y, main.VisibleX, main.VisibleY);
			} else{
				float x1 = x - win.Width / 2;
				float y1 = y - win.Height / 2;
				int newX = (int) Math.Round(x1 * main.TotalWidth() / overview.Width);
				int newY = (int) Math.Round(y1 * main.TotalHeight() / overview.Height);
				newX = (int) Math.Min(Math.Max(newX, 0), main.TotalWidth() - main.VisibleWidth / main.ZoomFactor);
				main.VisibleX = newX;
				newY = (int) Math.Min(Math.Max(newY, 0), main.TotalHeight() - main.VisibleHeight / main.ZoomFactor);
				main.VisibleY = newY;
				invalidate();
			}
		}
		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpMainView?.Invoke(e.Scale(main.ZoomFactor));
			navigatorData.Reset();
			state = ZoomButtonState.Neutral;
			Invalidate();
		}
		public override void OnMouseDragged(BasicMouseEventArgs e){
			if (navigatorData.IsMoving()){
				PointI2 newXy = navigatorData.Dragging(e.X, e.Y, e.Width, e.Height, main.TotalWidth(),
					main.TotalHeight(),
					main.VisibleWidth, main.VisibleHeight, main.ZoomFactor);
				main.VisibleX = newXy.X;
				main.VisibleY = newXy.Y;
				invalidate();
				return;
			}
			main.OnMouseDraggedMainView?.Invoke(e.Scale(main.ZoomFactor));
		}
	}
}