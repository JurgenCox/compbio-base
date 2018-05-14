using System;
using System.Windows.Forms;
using BaseLibS.Graph;

namespace BaseLib.Forms.Base{
	[Obsolete]
	public class BasicUserControl : UserControl {
		private bool mouseDown;

		protected override void OnMouseDown(MouseEventArgs e){
			base.OnMouseDown(e);
			mouseDown = true;
			OnMouseIsDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e){
			base.OnMouseUp(e);
			mouseDown = false;
			OnMouseIsUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e){
			SetStyle(ControlStyles.Selectable, true);
			Focus();
			base.OnMouseMove(e);
			if (mouseDown){
				OnMouseDragged(e);
			} else{
				OnMouseMoved(e);
			}
		}

		public virtual void DoPaint(IGraphics g) {}

		public virtual void DoPaintBackground(IGraphics g){
			g.FillRectangle(new Brush2(Color2.FromArgb(BackColor.A, BackColor.R, BackColor.G, BackColor.B) ), 0, 0, Width, Height);
		}

		protected virtual void OnMouseDragged(MouseEventArgs e) {}
		protected virtual void OnMouseMoved(MouseEventArgs e) {}
		protected virtual void OnMouseIsUp(MouseEventArgs e) {}
		protected virtual void OnMouseIsDown(MouseEventArgs e) {}
	}
}