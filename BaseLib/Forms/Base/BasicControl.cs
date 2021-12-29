using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Graphic;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
using BaseLibS.Graph.Scroll;
namespace BaseLib.Forms.Base{
	public class BasicControl : GenericControl{
		private ToolTip toolTip;
		private bool mouseDown;
		public BasicControlModel view;
		public BasicControl(){
			DoubleBuffered = true;
			ResizeRedraw = true;
			Margin = new Padding(0);
			Dock = DockStyle.Fill;
		}
		public void Activate(BasicControlModel view1){
			view = view1;
			view.invalidate = Invalidate;
			view.resetCursor = ResetCursor;
			view.setCursor = SetCursor;
			view.launchQuery = LaunchQuery;
			view.screenCoords = ScreenCoords;
			view.setToolTipText = SetToolTipText;
		}
		public void SetToolTipText(string text){
			if (toolTip == null){
				toolTip = new ToolTip();
			}
			toolTip.SetToolTip(this, text);
		}
		public (int, int) ScreenCoords(){
			Point p = PointToScreen(new Point(0, 0));
			return (p.X, p.Y);
		}
		private void LaunchQuery(int x, int y, int width, int height, IControlModel visual){
			Form f = new Form{
				StartPosition = FormStartPosition.Manual,
				Location = new Point(x, y),
				Width = width,
				Height = height,
				MinimizeBox = false,
				MaximizeBox = false,
				ControlBox = false,
				SizeGripStyle = SizeGripStyle.Hide,
				FormBorderStyle = FormBorderStyle.FixedToolWindow,
				ShowInTaskbar = false
			};
			visual.Close += (sender, args) => { f.Close(); };
			Control c = FormUtil.GetControl(visual);
			f.Controls.Add(c);
			f.ShowDialog(this);
		}
		public void Print(IGraphics g){
			view?.OnPaint(g, Width, Height);
		}
		protected sealed override void OnPaint(PaintEventArgs e){
			base.OnPaint(e);
			if (view != null){
				if (view.Debug){
					view.OnPaint(new CGraphics(e.Graphics), Width, Height);
				} else{
					try{
						view.OnPaint(new CGraphics(e.Graphics), Width, Height);
					} catch (Exception e1){
						MessageBox.Show(e1.Message + "\n" + e1.StackTrace);
					}
				}
			}
		}
		protected sealed override void OnPaintBackground(PaintEventArgs e){
			base.OnPaintBackground(e);
			view?.OnPaintBackground(new CGraphics(e.Graphics), Width, Height);
		}
		protected sealed override void OnMouseDown(MouseEventArgs e){
			base.OnMouseDown(e);
			mouseDown = true;
			view.OnMouseIsDown(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
				() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
		}
		protected sealed override void OnMouseUp(MouseEventArgs e){
			base.OnMouseUp(e);
			mouseDown = false;
			view.OnMouseIsUp(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
				() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
		}
		protected sealed override void OnMouseMove(MouseEventArgs e){
			SetStyle(ControlStyles.Selectable, true);
			base.OnMouseMove(e);
			if (mouseDown){
				view.OnMouseDragged(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
					() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
			} else{
				view.OnMouseMoved(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
					() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
			}
		}
		protected sealed override void OnMouseLeave(EventArgs e){
			base.OnMouseLeave(e);
			view?.OnMouseLeave(e);
		}
		protected sealed override void OnMouseClick(MouseEventArgs e){
			base.OnMouseClick(e);
			view?.OnMouseClick(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
				() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
		}
		protected sealed override void OnMouseDoubleClick(MouseEventArgs e){
			base.OnMouseDoubleClick(e);
			view?.OnMouseDoubleClick(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
				() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
		}
		protected sealed override void OnMouseHover(EventArgs e){
			base.OnMouseHover(e);
			view?.OnMouseHover(e);
		}
		protected sealed override void OnMouseCaptureChanged(EventArgs e){
			base.OnMouseCaptureChanged(e);
			view?.OnMouseCaptureChanged(e);
		}
		protected sealed override void OnMouseEnter(EventArgs e){
			Focus();
			base.OnMouseEnter(e);
			view?.OnMouseEnter(e);
		}
		protected sealed override void OnMouseWheel(MouseEventArgs e){
			base.OnMouseWheel(e);
			view?.OnMouseWheel(new BasicMouseEventArgs(e.X, e.Y, e.Button == MouseButtons.Left, Width, Height,
				() => (ModifierKeys & Keys.Control) == Keys.Control, ViewToolTip));
		}
		protected sealed override void OnResize(EventArgs e){
			base.OnResize(e);
			view?.OnResize(e, Width, Height);
		}
		protected sealed override void Dispose(bool disposing){
			base.Dispose(disposing);
			view?.Dispose(disposing);
		}
		private void ViewToolTip(string text, int x, int y, int duration){
			if (toolTip == null){
				toolTip = new ToolTip();
			}
			toolTip.Show(text, this, x, y, duration);
		}
		public static BasicControl CreateControl(BasicControlModel view){
			BasicControl c = new BasicControl();
			c.Activate(view);
			return c;
		}
	}
}