using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph{
	public class ComboBoxModel : BasicControlModel{
		private readonly Brush2 textBrush = Brushes2.Black;
		private readonly Pen2 cornerPen = new Pen2(Color2.FromArgb(99, 99, 99));
		private Pen2 boxPen = new Pen2(Color2.FromArgb(214, 214, 214));
		public int OffsetX{ get; set; }
		public int OffsetY{ get; set; }
		public ComboBoxModel() : this(new string[0]){
		}
		public ComboBoxModel(string[] values){
			Values = values;
			BackColor = Color2.FromArgb(225, 225, 225);
			OffsetY = 3;
			SelectedIndexChanged += (sender, args) => { Invalidate(); };
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
			g.DrawString(Values[SelectedIndex], Font, textBrush, OffsetX, OffsetY);
		}
		public override void OnMouseIsDown(BasicMouseEventArgs e){
			TextFieldModel tfm = new TextFieldModel(Values){
				MultiLine = true,
				Selectable = true,
				Editable = false,
				SelectionMode = TextFieldSelectionMode.SingleLines,
				Font = new Font2("Microsoft Sans Serif", 7.1f),
				SelectedLine = SelectedIndex
			};
			tfm.SelectionChanged += (sender, args) => {
				SelectedIndex = tfm.SelectedLine;
				SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
				tfm.CloseMe();
			};
			(int, int) p = screenCoords.Invoke();
			launchQuery?.Invoke(p.Item1, p.Item2 + e.Height, e.Width, Values.Length * tfm.LineHeight, tfm);
		}
		public override void OnMouseEnter(EventArgs e){
			BackColor = Color2.FromArgb(229, 241, 251);
			boxPen = new Pen2(Color2.FromArgb(128, 187, 235));
			Invalidate();
		}
		public override void OnMouseLeave(EventArgs e){
			BackColor = Color2.FromArgb(225, 225, 225);
			boxPen = new Pen2(Color2.FromArgb(214, 214, 214));
			Invalidate();
		}
		public event EventHandler SelectedIndexChanged;
	}
}