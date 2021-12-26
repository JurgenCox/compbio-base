using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Scroll{
	public sealed class ScrollableControlRowFooterView : ScrollComponentView{
		public ScrollableControlRowFooterView(ICompoundScrollableControl main) : base(main) {}

		public override void OnPaint(IGraphics g, int width, int height) {
			main.OnPaintRowFooterView?.Invoke(g, main.VisibleY, height);
		}

		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveRowFooterView?.Invoke(e);
		}

		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveRowFooterView?.Invoke(e);
		}

		public override void OnMouseClick(BasicMouseEventArgs e){
			main.OnMouseClickRowFooterView?.Invoke(e);
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			main.OnMouseDoubleClickRowFooterView?.Invoke(e);
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			main.OnMouseIsDownRowFooterView?.Invoke(e);
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpRowFooterView?.Invoke(e);
		}

		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverRowFooterView?.Invoke(e);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			main.OnMouseDraggedRowFooterView?.Invoke(e);
		}
	}
}