using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base {
	public class PanelParentAdapter : IGenericControl{
		private readonly PanelModel panelModel;
		public PanelParentAdapter(PanelModel panelModel){
			this.panelModel = panelModel;
		}

		public Tuple<int, int> GetOrigin(){
			return panelModel.getOrigin();
		}
		public void ExportGraphic(string name, bool showDialog){
			panelModel.exportGraphic?.Invoke(name, showDialog);
		}
		public void SetModel(BasicControlModel createModel){
			throw new NotImplementedException();
		}
		public Action<int> onMouseWheel{ get; set; }
		public Action onResize{ get; set; }
		public Action onSizeChanged{ get; set; }
		public Action<Keys2, int> processCmdKey{ get; set; }
		public int Width1 => panelModel.getWidth();
		public int Height1 => panelModel.getHeight();
		public bool Enabled => panelModel.Enabled;
		public void Invalidate(bool p0){
			panelModel.invalidate?.Invoke();
		}
		public Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight, int totalWidth, int totalHeight,
			Action<IGraphics, int, int, int, int, bool> onPaintMainView){
			//TODO
			throw new NotImplementedException();
		}
		public void InitContextMenu(){
			panelModel.initContextMenu?.Invoke();
		}
		public void AddContextMenuItem(string text, EventHandler action){
			panelModel.addContextMenuItem?.Invoke(text, action);
		}
		public void AddContextMenuSeparator(){
			panelModel.addContextMenuSeparator?.Invoke();
		}
		public Tuple<int, int> GetContextMenuPosition(){
			return panelModel.getContextMenuPosition();
		}
		public void SetClipboardData(object data){
			panelModel.setClipboardData?.Invoke(data);
		}
		public void ShowMessage(string text){
			panelModel.showMessage?.Invoke(text);
		}
		public string GetClipboardText(){
			return panelModel.getClipboardText();
		}
		public (bool, Font2, Color2) QueryFontColor(Font2 fontIn, Color2 colorIn){
			return panelModel.queryFontColor(fontIn, colorIn);
		}
		public (bool, string) SaveFileDialog(string filter){
			return panelModel.saveFileDialog(filter);
		}
		public bool IsControlPressed(){
			return panelModel.isControlPressed();
		}
		public bool IsShiftPressed(){
			return panelModel.isShiftPressed();
		}
		public void SetCursor(Cursors2 cursor){
			panelModel.setCursor?.Invoke(cursor);
		}
		public void SetToolTipTitle(string title){
			panelModel.setToolTipTitle?.Invoke(title);
		}
		public void ShowToolTip(string text, int i, int i1){
			panelModel.showToolTip?.Invoke(text, i, i1);
		}
		public void HideToolTip(){
			panelModel.hideToolTip?.Invoke();
		}
	}
}
