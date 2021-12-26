using BaseLibS.Drawing;
using BaseLibS.Graph.Scroll;
namespace BaseLibS.Graph {
	public class ComboBoxModel : ISimpleScrollableControlModel {
		private readonly Brush2 textBrush = Brushes2.Black;
		private readonly Font2 font = new Font2("Arial", 10F);

		public ComboBoxModel() : this(new string[0]){
		}
		public ComboBoxModel(string[] values) {
			Values = values;
		}

		public string[] Values{ get; set; }
		public void ProcessCmdKey(Keys2 keyData){
		}
		public void InvalidateBackgroundImages(){
		}
		public void OnSizeChanged(){
		}
		public void Register(ISimpleScrollableControl control){
		}
	}
}
