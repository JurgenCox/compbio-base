﻿using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base{
	public class BasicControlModel : IPrintable{
		public Color2 BackColor { get; set; }
		public Color2 ForeColor { get; set; }
		public bool Visible { get; set; }
		public Font2 Font { get; set; }
		public bool Enabled { get; set; }
		public Action invalidate;
		public Action resetCursor;
		public Action<Cursors2> setCursor;
		public Action<int, int, int, int> launchQuery;
		public Func<(int, int)> screenCoords;
		public bool Debug { get; set; } = false;

		public BasicControlModel(){
			BackColor = Color2.White;
			ForeColor = Color2.Black;
			Visible = true;
			Font = new Font2("Microsoft Sans Serif", 8.25f, FontStyle2.Regular);
		}

		public void Activate(BasicControlModel view) {
			invalidate = view.Invalidate;
			resetCursor = view.ResetCursor;
			setCursor = c => view.Cursor = c;
		}

		public void Invalidate(){
			invalidate?.Invoke();
		}

		public void ResetCursor(){
			resetCursor?.Invoke();
		}

		public Cursors2 Cursor{
			set => setCursor?.Invoke(value);
		}

		public virtual void OnPaint(IGraphics g, int width, int height){}

		public virtual void OnPaintBackground(IGraphics g, int width, int height){
			g.FillRectangle(new Brush2(BackColor), 0, 0, width, height);
		}

		public virtual void OnMouseDragged(BasicMouseEventArgs e){}
		public virtual void OnMouseMoved(BasicMouseEventArgs e){}
		public virtual void OnMouseIsUp(BasicMouseEventArgs e){}
		public virtual void OnMouseIsDown(BasicMouseEventArgs e){}
		public virtual void OnMouseLeave(EventArgs e){}
		public virtual void OnMouseClick(BasicMouseEventArgs e){}
		public virtual void OnMouseDoubleClick(BasicMouseEventArgs e){}
		public virtual void OnMouseHover(EventArgs e){}
		public virtual void OnMouseCaptureChanged(EventArgs e){}
		public virtual void OnMouseEnter(EventArgs e){}
		public virtual void OnMouseWheel(BasicMouseEventArgs e){}
		public virtual void OnResize(EventArgs e, int width, int height){}
		public virtual void Dispose(bool disposing){}

		public void Print(IGraphics g, int width, int height){
			OnPaintBackground(g, width, height);
			OnPaint(g, width, height);
		}
	}
}