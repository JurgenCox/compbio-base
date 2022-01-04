using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Scroll;
namespace BaseLibS.Graph {
	public class SplitContainerModel : ISimpleScrollableControlModel {
		public void ProcessCmdKey(Keys2 keyData){
		}
		public void InvalidateBackgroundImages(){
		}
		public void OnSizeChanged(){
		}
		public event EventHandler Close;
		public void Register(ISimpleScrollableControl control){
		}
		public int SplitterDistance{ get; set; }
		public Orientation2 Orientation { get; set; }
		public PanelModel Panel1 { get; set; }
		public PanelModel Panel2 { get; set; }
	}
}
