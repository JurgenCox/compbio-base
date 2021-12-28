using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph{
	public class ComboBoxModel : BasicControlModel{
		private readonly Brush2 textBrush = Brushes2.Black;
		private readonly Font2 font = new Font2("Arial", 10F);
		public int OffsetX{ get; set; }
		public int OffsetY{ get; set; }
		public ComboBoxModel() : this(new string[0]){
		}
		public ComboBoxModel(string[] values){
			Values = values;
		}
		public int SelectedIndex{ get; set; }
		public string[] Values{ get; set; }
		public override void OnPaint(IGraphics g, int width, int height){
			if (SelectedIndex < 0 || SelectedIndex >= Values.Length){
				return;
			}
			g.DrawString(Values[SelectedIndex], font, textBrush, OffsetX, OffsetY);
		}
	}
}