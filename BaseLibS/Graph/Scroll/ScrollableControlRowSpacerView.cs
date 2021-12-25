using System;
namespace BaseLibS.Graph.Scroll{
	public sealed class ScrollableControlRowSpacerView : ScrollComponentView{
		public ScrollableControlRowSpacerView(ICompoundScrollableControl main) : base(main) {}

		public override void OnPaint(IGraphics g, int width, int height) {
			main.OnPaintRowSpacerView?.Invoke(g);
		}

		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveRowSpacerView?.Invoke(e);
		}

		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveRowSpacerView?.Invoke(e);
		}

		public override void OnMouseClick(BasicMouseEventArgs e){
			main.OnMouseClickRowSpacerView?.Invoke(e);
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			main.OnMouseDoubleClickRowSpacerView?.Invoke(e);
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			main.OnMouseIsDownRowSpacerView?.Invoke(e);
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpRowSpacerView?.Invoke(e);
		}

		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverRowSpacerView?.Invoke(e);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			main.OnMouseDraggedRowSpacerView?.Invoke(e);
		}
	}
}