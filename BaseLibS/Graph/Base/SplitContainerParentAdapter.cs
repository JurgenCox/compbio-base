using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base {
	public class SplitContainerParentAdapter : IGenericControl {
		private readonly SplitContainerModel splitContainerModel;
		public SplitContainerParentAdapter(SplitContainerModel splitContainerModel) {
			this.splitContainerModel = splitContainerModel;
		}
		public Tuple<int, int> GetOrigin() {
			return splitContainerModel.getOrigin();
		}
		public void ExportGraphic(string name, bool showDialog) {
			splitContainerModel.exportGraphic?.Invoke(name, showDialog);
		}
		public void SetModel(BasicControlModel createModel) {
			throw new NotImplementedException();
		}
		public Action<int> onMouseWheel { get; set; }
		public Action onResize { get; set; }
		public Action onSizeChanged { get; set; }
		public Action<Keys2> processCmdKey { get; set; }
		public int Width1 => splitContainerModel.getWidth();
		public int Height1 => splitContainerModel.getHeight();
		public bool Enabled => splitContainerModel.Enabled;
		public void Invalidate(bool p0) {
			splitContainerModel.invalidate?.Invoke();
		}
		public Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight, int totalWidth, int totalHeight,
			Action<IGraphics, int, int, int, int, bool> onPaintMainView) {
			//TODO
			throw new NotImplementedException();
		}
		public void InitContextMenu() {
			splitContainerModel.initContextMenu?.Invoke();
		}
		public void AddContextMenuItem(string text, EventHandler action) {
			splitContainerModel.addContextMenuItem?.Invoke(text, action);
		}
		public void AddContextMenuSeparator() {
			splitContainerModel.addContextMenuSeparator?.Invoke();
		}
		public Tuple<int, int> GetContextMenuPosition() {
			return splitContainerModel.getContextMenuPosition();
		}
		public void SetClipboardData(object data) {
			splitContainerModel.setClipboardData?.Invoke(data);
		}
		public void ShowMessage(string text) {
			splitContainerModel.showMessage?.Invoke(text);
		}
		public string GetClipboardText() {
			return splitContainerModel.getClipboardText();
		}
		public (bool, Font2, Color2) QueryFontColor(Font2 fontIn, Color2 colorIn) {
			return splitContainerModel.queryFontColor(fontIn, colorIn);
		}
		public (bool, string) SaveFileDialog(string filter) {
			return splitContainerModel.saveFileDialog(filter);
		}
		public bool IsControlPressed() {
			return splitContainerModel.isControlPressed();
		}
		public bool IsShiftPressed() {
			return splitContainerModel.isShiftPressed();
		}
		public void SetCursor(Cursors2 cursor) {
			splitContainerModel.setCursor?.Invoke(cursor);
		}
		public void SetToolTipTitle(string title) {
			splitContainerModel.setToolTipTitle?.Invoke(title);
		}
		public void ShowToolTip(string text, int i, int i1) {
			splitContainerModel.showToolTip?.Invoke(text, i, i1);
		}
		public void HideToolTip() {
			splitContainerModel.hideToolTip?.Invoke();
		}
	}
}
