using System;
using BaseLibS.Graph;

namespace BaseLib.Forms.Scroll{
	internal sealed class ScrollableControlRowHeaderView : ScrollComponentView{
		internal ScrollableControlRowHeaderView(CompoundScrollableControl main) : base(main) {}

		public override void OnPaint(IGraphics g, int width, int height) {
			main.OnPaintRowHeaderView?.Invoke(g, main.VisibleY, height);
		}

		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveRowHeaderView?.Invoke(e);
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			main.OnMouseIsDownRowHeaderView?.Invoke(e);
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpRowHeaderView?.Invoke(e);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			main.OnMouseDraggedRowHeaderView?.Invoke(e);
		}

		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveRowHeaderView?.Invoke(e);
		}

		public override void OnMouseClick(BasicMouseEventArgs e){
			main.OnMouseClickRowHeaderView?.Invoke(e);
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			main.OnMouseDoubleClickRowHeaderView?.Invoke(e);
		}

		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverRowHeaderView?.Invoke(e);
		}
	}
}