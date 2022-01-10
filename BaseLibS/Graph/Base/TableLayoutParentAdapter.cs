using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base{
	public class TableLayoutParentAdapter : IGenericControl{
		private readonly TableLayoutModel tableLayoutModel;
		public TableLayoutParentAdapter(TableLayoutModel tableLayoutModel) {
			this.tableLayoutModel = tableLayoutModel;
		}
		public Tuple<int, int> GetOrigin() {
			return tableLayoutModel.getOrigin();
		}
		public void ExportGraphic(string name, bool showDialog) {
			tableLayoutModel.exportGraphic?.Invoke(name, showDialog);
		}
		public void SetModel(BasicControlModel createModel) {
			throw new NotImplementedException();
		}
		public Action<int> onMouseWheel { get; set; }
		public Action onResize { get; set; }
		public Action onSizeChanged { get; set; }
		public Action<Keys2> processCmdKey { get; set; }
		public int Width1 => tableLayoutModel.getWidth();
		public int Height1 => tableLayoutModel.getHeight();
		public bool Enabled => tableLayoutModel.Enabled;
		public void Invalidate(bool p0) {
			tableLayoutModel.invalidate?.Invoke();
		}
		public Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight, int totalWidth, int totalHeight,
			Action<IGraphics, int, int, int, int, bool> onPaintMainView) {
			//TODO
			throw new NotImplementedException();
		}
		public void InitContextMenu() {
			tableLayoutModel.initContextMenu?.Invoke();
		}
		public void AddContextMenuItem(string text, EventHandler action) {
			tableLayoutModel.addContextMenuItem?.Invoke(text, action);
		}
		public void AddContextMenuSeparator() {
			tableLayoutModel.addContextMenuSeparator?.Invoke();
		}
		public Tuple<int, int> GetContextMenuPosition() {
			return tableLayoutModel.getContextMenuPosition();
		}
		public void SetClipboardData(object data) {
			tableLayoutModel.setClipboardData?.Invoke(data);
		}
		public void ShowMessage(string text) {
			tableLayoutModel.showMessage?.Invoke(text);
		}
		public string GetClipboardText() {
			return tableLayoutModel.getClipboardText();
		}
		public (bool, Font2, Color2) QueryFontColor(Font2 fontIn, Color2 colorIn) {
			return tableLayoutModel.queryFontColor(fontIn, colorIn);
		}
		public (bool, string) SaveFileDialog(string filter) {
			return tableLayoutModel.saveFileDialog(filter);
		}
		public bool IsControlPressed() {
			return tableLayoutModel.isControlPressed();
		}
		public bool IsShiftPressed() {
			return tableLayoutModel.isShiftPressed();
		}
		public void SetCursor(Cursors2 cursor) {
			tableLayoutModel.setCursor?.Invoke(cursor);
		}
		public void SetToolTipTitle(string title) {
			tableLayoutModel.setToolTipTitle?.Invoke(title);
		}
		public void ShowToolTip(string text, int i, int i1) {
			tableLayoutModel.showToolTip?.Invoke(text, i, i1);
		}
		public void HideToolTip() {
			tableLayoutModel.hideToolTip?.Invoke();
		}
	}
}