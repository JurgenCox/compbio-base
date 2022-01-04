using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph{
	public class CheckBoxModel : BasicControlModel {
		private readonly Pen2 boxPen = new Pen2(Color2.FromArgb(112, 112, 112));
		private readonly Pen2 checkPen = new Pen2(Color2.FromArgb(72, 72, 72));
		public bool Checked{ get; set; }
		public event EventHandler CheckedChanged;

		public override void OnPaint(IGraphics g, int width, int height) {
			g.DrawRectangle(boxPen, 3, 3, 11, 11);
			if (Checked){
				g.DrawLine(checkPen, 4, 9, 7, 12);
				g.DrawLine(checkPen, 7, 12, 14, 6);
			}
		}
		public override void OnMouseClick(BasicMouseEventArgs e){
			Checked = !Checked;
			CheckedChanged?.Invoke(this, EventArgs.Empty);
			Invalidate();
		}
	}
}