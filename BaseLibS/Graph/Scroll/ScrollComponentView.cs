using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph.Scroll{
	public class ScrollComponentView : BasicView{
		protected readonly ICompoundScrollableControl main;

		protected ScrollComponentView(ICompoundScrollableControl main){
			this.main = main;
		}

		public sealed override void OnPaintBackground(IGraphics g, int width, int height){
			if (main == null || main.BackColor2 == Color2.Transparent) {
				return;
			}
			Brush2 b = new Brush2(main.BackColor2.IsEmpty ? Color2.White : main.BackColor2);
			g.FillRectangle(b, 0, 0, width, height);
		}
	}
}