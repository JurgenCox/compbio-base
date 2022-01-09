using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base {
	public class SplitContainerParentAdapter : IGenericControl {
		public Tuple<int, int> GetOrigin(){
			throw new NotImplementedException();
		}
		public void ExportGraphic(string name, bool showDialog){
			throw new NotImplementedException();
		}
		public void SetModel(BasicControlModel createModel){
			throw new NotImplementedException();
		}
		public Action<int> onMouseWheel{ get; set; }
		public Action onResize{ get; set; }
		public Action onSizeChanged{ get; set; }
		public Action<Keys2> processCmdKey{ get; set; }
		public int Width1{ get; }
		public int Height1{ get; }
		public bool Enabled{ get; }
		public void Invalidate(bool p0){
			throw new NotImplementedException();
		}
		public Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight, int totalWidth, int totalHeight,
			Action<IGraphics, int, int, int, int, bool> onPaintMainView){
			throw new NotImplementedException();
		}
		public void InitContextMenu(){
			throw new NotImplementedException();
		}
		public void AddContextMenuItem(string text, EventHandler action){
			throw new NotImplementedException();
		}
		public void AddContextMenuSeparator(){
			throw new NotImplementedException();
		}
		public Tuple<int, int> GetContextMenuPosition(){
			throw new NotImplementedException();
		}
		public void SetClipboardData(object data){
			throw new NotImplementedException();
		}
		public void ShowMessage(string text){
			throw new NotImplementedException();
		}
		public string GetClipboardText(){
			throw new NotImplementedException();
		}
		public bool QueryFontColor(Font2 fontIn, Color2 colorIn, out Font2 font, out Color2 color){
			throw new NotImplementedException();
		}
		public bool SaveFileDialog(out string fileName, string filter){
			throw new NotImplementedException();
		}
		public bool IsControlPressed(){
			throw new NotImplementedException();
		}
		public bool IsShiftPressed(){
			throw new NotImplementedException();
		}
		public void SetCursor(Cursors2 cursor){
			throw new NotImplementedException();
		}
		public void SetToolTipTitle(string title){
			throw new NotImplementedException();
		}
		public void ShowToolTip(string text, int i, int i1){
			throw new NotImplementedException();
		}
		public void HideToolTip(){
			throw new NotImplementedException();
		}
	}
}
