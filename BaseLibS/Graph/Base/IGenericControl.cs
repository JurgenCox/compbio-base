using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Base {
	public interface IGenericControl {
		Tuple<int, int> GetOrigin();
		void ExportGraphic(string name, bool showDialog);
		void SetModel(BasicControlModel createModel);
		Action<int> onMouseWheel{ get; set; }
		Action onResize{ get; set; }
		Action onSizeChanged{ get; set; }
		Action<Keys2> processCmdKey{ get; set; }
		int Width1{ get; }
		int Height1 { get; }
		bool Enabled { get; }
		void Invalidate(bool p0);
		Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight, int totalWidth, int totalHeight, Action<IGraphics, int, int, int, int, bool> onPaintMainView);
		void InitContextMenu();
		void AddContextMenuItem(string text, EventHandler action);
		void AddContextMenuSeparator();
		Tuple<int, int> GetContextMenuPosition();
		void SetClipboardData(object data);
		void ShowMessage(string text);
		string GetClipboardText();
		bool QueryFontColor(Font2 fontIn, Color2 colorIn, out Font2 font, out Color2 color);
		bool SaveFileDialog(out string fileName, string filter);
		bool IsControlPressed();
		bool IsShiftPressed();
		void SetCursor(Cursors2 cursor);
		void SetToolTipTitle(string title);
		void ShowToolTip(string text, int i, int i1);
		void HideToolTip();
	}
}
