using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Scroll;
namespace BaseLibS.Graph.Base{
	public class PanelModel : BasicControlModel{
		private BasicControlModel controlModel;
		private IGenericControl parent;
		public IControlModel ControlModel{
			get => controlModel;
			set{
				if (value is BasicControlModel){
					controlModel = (BasicControlModel) value;
				} else if (value is ISimpleScrollableControlModel){
					parent = new PanelParentAdapter(this);
					controlModel = new SimpleScrollableControlModel(parent, (ISimpleScrollableControlModel) value);
				} else if (value is ICompoundScrollableControlModel){
					parent = new PanelParentAdapter(this);
					controlModel = new CompoundScrollableControlModel(parent, (ICompoundScrollableControlModel) value);
				} else{
					throw new ArgumentException("Illegal type.");
				}
			}
		}
		public override void OnPaint(IGraphics g, int width, int height){
			controlModel.OnPaint(g, width, height);
		}
		public override void OnMouseDragged(BasicMouseEventArgs e){
			controlModel.OnMouseDragged(e);
		}
		public override void OnMouseMoved(BasicMouseEventArgs e){
			controlModel.OnMouseMoved(e);
		}
		public override void OnMouseIsUp(BasicMouseEventArgs e){
			controlModel.OnMouseIsUp(e);
		}
		public override void OnMouseIsDown(BasicMouseEventArgs e){
			controlModel.OnMouseIsDown(e);
		}
		public override void OnMouseLeave(EventArgs e){
			controlModel.OnMouseLeave(e);
		}
		public override void OnMouseClick(BasicMouseEventArgs e){
			controlModel.OnMouseClick(e);
		}
		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			controlModel.OnMouseDoubleClick(e);
		}
		public override void OnMouseHover(EventArgs e){
			controlModel.OnMouseHover(e);
		}
		public override void OnMouseCaptureChanged(EventArgs e){
			controlModel.OnMouseCaptureChanged(e);
		}
		public override void OnMouseEnter(EventArgs e){
			controlModel.OnMouseEnter(e);
		}
		public override void OnMouseWheel(BasicMouseEventArgs e){
			controlModel.OnMouseWheel(e);
		}
		public override void OnResize(EventArgs e, int width, int height){
			controlModel.OnResize(e, width, height);
		}
		public override void Dispose(bool disposing){
			controlModel.Dispose(disposing);
		}
		public override void ProcessCmdKey(Keys2 keyData, int keyboardId) {
			controlModel.ProcessCmdKey(keyData, keyboardId);
		}
		public override void InvalidateBackgroundImages(){
			controlModel.InvalidateBackgroundImages();
		}
		public override void OnSizeChanged(){
			controlModel.OnSizeChanged();
		}
	}
}