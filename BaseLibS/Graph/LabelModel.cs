using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph {
	public class LabelModel : BasicControlModel{
		private readonly Brush2 textBrush = Brushes2.Black;
		private Pen2 boxPen = new Pen2(Color2.FromArgb(214, 214, 214));
		public int OffsetX { get; set; }
		public int OffsetY { get; set; }
		public LabelModel() : this("") {
		}
		public LabelModel(string text) {
			Text = text;
			BackColor = Color2.FromArgb(225, 225, 225);
		}
		public string Text { get; set; }
		public override void OnPaint(IGraphics g, int width, int height) {
			g.DrawString(Text, Font, textBrush, OffsetX, OffsetY);
		}
	}
}
