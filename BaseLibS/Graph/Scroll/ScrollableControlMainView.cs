using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Scroll{
	public sealed class ScrollableControlMainView : ScrollComponentView{
		public ScrollableControlMainView(ICompoundScrollableControl main) : base(main) {}

		public override void OnPaint(IGraphics g, int width, int height){
			main.OnPaintMainView?.Invoke(g, main.VisibleX, main.VisibleY, width, height, false);
		}

		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveMainView?.Invoke(e);
		}

		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverMainView?.Invoke(e);
		}

		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveMainView?.Invoke(e);
		}

		public override void OnMouseClick(BasicMouseEventArgs e){
			main.OnMouseClickMainView?.Invoke(e);
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			main.OnMouseDoubleClickMainView?.Invoke(e);
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			main.OnMouseIsDownMainView?.Invoke(e);
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpMainView?.Invoke(e);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			main.OnMouseDraggedMainView?.Invoke(e);
		}
	}
}