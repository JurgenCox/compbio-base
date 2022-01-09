using BaseLibS.Drawing;
using BaseLibS.Graph.Scroll;
namespace BaseLibS.Graph.Base {
	public class PanelModel : BasicControlModel {
		public IControlModel ControlModel{ get; set; }
		public override void OnPaint(IGraphics g, int width, int height){
			if (ControlModel is BasicControlModel){
				BasicControlModel bcm = (BasicControlModel) ControlModel;
				bcm.OnPaint(g, width, height);
			}else if (ControlModel is ISimpleScrollableControlModel){
				ISimpleScrollableControlModel m = (ISimpleScrollableControlModel)ControlModel;
				
			}
		}
	}
}
