using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph {
	public class LabelModel : BasicControlModel{
		private readonly Brush2 textBrush = Brushes2.Black;
		private string text;
		public int OffsetX { get; set; }
		public int OffsetY { get; set; }
		public LabelModel() : this("") {
		}
		public LabelModel(string text) {
			Text = text;
			BackColor = Color2.Transparent;
		}
		public string Text{
			get => text;
			set{
				text = value;
				Invalidate();
			}
		}
		public override void OnPaint(IGraphics g, int width, int height) {
			g.DrawString(text, Font, textBrush, OffsetX, OffsetY);
		}
	}
}
