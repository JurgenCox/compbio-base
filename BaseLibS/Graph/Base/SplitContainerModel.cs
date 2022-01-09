using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base {
	public class SplitContainerModel : BasicControlModel {
		public int SplitterDistance{ get; set; }
		public Orientation2 Orientation { get; set; }
		public PanelModel Panel1 { get; set; }
		public PanelModel Panel2 { get; set; }
	}
}
