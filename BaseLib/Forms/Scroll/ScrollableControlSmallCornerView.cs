using BaseLibS.Graph;
using BaseLibS.Graph.Base;

namespace BaseLib.Forms.Scroll{
	internal sealed class ScrollableControlSmallCornerView : BasicView{
		public override void OnPaintBackground(IGraphics g, int width, int height){
			Brush2 b = new Brush2(Color2.FromArgb(236, 233, 216));
			g.FillRectangle(b, 0, 0, width, height);
			Pen2 p = new Pen2(Color2.FromArgb(172, 168, 153));
			g.DrawLine(p, 0, height - 1, width, height - 1);
			g.DrawLine(p, width - 1, 0, width - 1, height);
		}
	}
}