using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph{
	public class ButtonModel : BasicControlModel{
		private readonly Brush2 textBrush = Brushes2.Black;
		private readonly Brush2 disabledTextBrush = Brushes2.Gray;
		private readonly Font2 font = new Font2("Microsoft Sans Serif", 8.25f);
		private bool highlight;
		public string Text{ get; set; } = "";
		public event EventHandler Click;

		public ButtonModel(){
			Enabled = true;
		}
		public override void OnPaint(IGraphics g, int width, int height){
			Pen2 outerPen;
			Pen2 innerPen;
			if (highlight && Enabled){
				outerPen = new Pen2(Color2.FromArgb(23, 132, 224));
				innerPen = new Pen2(Color2.FromArgb(229, 241, 251));
			} else{
				outerPen = new Pen2(Color2.FromArgb(199, 199, 199));
				innerPen = new Pen2(Color2.FromArgb(225, 225, 225));
			}
			g.DrawRectangle(outerPen, Margin.Left + 1, Margin.Top + 1,
				width - Margin.Left - Margin.Right - 3,
				height - Margin.Top - Margin.Bottom - 3);
			g.DrawRectangle(innerPen, Margin.Left + 2, Margin.Top + 2,
				width - Margin.Left - Margin.Right - 5,
				height - Margin.Top - Margin.Bottom - 5);
			g.DrawRectangle(innerPen, Margin.Left + 3, Margin.Top + 3,
				width - Margin.Left - Margin.Right - 7,
				height - Margin.Top - Margin.Bottom - 7);
			if (!string.IsNullOrEmpty(Text)){
				Size2 s = g.MeasureString(Text, font);
				int spaceX = (int) ((width - Margin.Left - Margin.Right - s.Width) * 0.5f);
				int spaceY = (int) ((height - Margin.Top - Margin.Bottom - s.Height) * 0.5f);
				g.DrawString(Text, font, Enabled ? textBrush : disabledTextBrush, Margin.Left + spaceX,
					Margin.Top + spaceY);
			}
		}
		public override void OnMouseClick(BasicMouseEventArgs e){
			if (!Enabled){
				return;
			}
			Click?.Invoke(this, EventArgs.Empty);
		}
		public override void OnMouseEnter(EventArgs e){
			highlight = true;
			Invalidate();
		}
		public override void OnMouseLeave(EventArgs e){
			highlight = false;
			Invalidate();
		}
	}
}