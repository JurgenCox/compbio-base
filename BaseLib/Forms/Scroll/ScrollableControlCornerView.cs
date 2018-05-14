using System;
using BaseLibS.Graph;

namespace BaseLib.Forms.Scroll{
	internal sealed class ScrollableControlCornerView : ScrollComponentView{
		internal ScrollableControlCornerView(CompoundScrollableControl main) : base(main) {}

		public override void OnPaint(IGraphics g, int width, int height) {
			main.OnPaintCornerView?.Invoke(g);
		}

		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveCornerView?.Invoke(e);
		}

		public override void OnMouseClick(BasicMouseEventArgs e){
			main.OnMouseClickCornerView?.Invoke(e);
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			main.OnMouseDoubleClickCornerView?.Invoke(e);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			main.OnMouseDraggedCornerView?.Invoke(e);
		}

		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverCornerView?.Invoke(e);
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			main.OnMouseIsDownCornerView?.Invoke(e);
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpCornerView?.Invoke(e);
		}

		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveCornerView?.Invoke(e);
		}
	}
}