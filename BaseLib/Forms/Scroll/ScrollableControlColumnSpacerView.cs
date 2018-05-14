using System;
using BaseLibS.Graph;

namespace BaseLib.Forms.Scroll{
	internal sealed class ScrollableControlColumnSpacerView : ScrollComponentView{
		internal ScrollableControlColumnSpacerView(CompoundScrollableControl main) : base(main){}

		public override void OnPaint(IGraphics g, int width, int height){
			main.OnPaintColumnSpacerView?.Invoke(g);
		}

		public override void OnMouseMoved(BasicMouseEventArgs e){
			main.OnMouseMoveColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseLeave(EventArgs e){
			main.OnMouseLeaveColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseClick(BasicMouseEventArgs e){
			main.OnMouseClickColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			main.OnMouseDoubleClickColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			main.OnMouseIsDownColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			main.OnMouseIsUpColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseHover(EventArgs e){
			main.OnMouseHoverColumnSpacerView?.Invoke(e);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			main.OnMouseDraggedColumnSpacerView?.Invoke(e);
		}
	}
}