using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph{
	public class ComboBoxModel : BasicControlModel{
		private readonly Brush2 textBrush = Brushes2.Black;
		private readonly Font2 font = new Font2("Microsoft Sans Serif", 8.25f);
		private readonly Pen2 cornerPen = new Pen2(Color2.FromArgb(99, 99, 99));
		private readonly Pen2 boxPen = new Pen2(Color2.FromArgb(214, 214, 214));
		public int OffsetX{ get; set; }
		public int OffsetY{ get; set; }
		public ComboBoxModel() : this(new string[0]){
		}
		public ComboBoxModel(string[] values){
			Values = values;
			BackColor = Color2.FromArgb(225, 225, 225);
			OffsetY = 3;
		}
		public int SelectedIndex{ get; set; }
		public string[] Values{ get; set; }
		public override void OnPaint(IGraphics g, int width, int height){
			g.DrawLine(cornerPen, width - 19, height / 2 - 2, width - 15, height / 2 + 2);
			g.DrawLine(cornerPen, width - 15, height / 2 + 2, width - 11, height / 2 - 2);
			g.DrawRectangle(boxPen, 0, 0, width - 1, height - 1);
			if (SelectedIndex < 0 || SelectedIndex >= Values.Length){
				return;
			}
			g.DrawString(Values[SelectedIndex], font, textBrush, OffsetX, OffsetY);
		}
		public override void OnMouseIsDown(BasicMouseEventArgs e){
			(int, int)? p = screenCoords?.Invoke();
			launchQuery?.Invoke(p.Value.Item1, p.Value.Item2 + e.Height, e.Width, 60);
		}
		public event EventHandler SelectedIndexChanged;
	}
}