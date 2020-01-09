using System;

namespace BaseLibS.Graph{
	public interface IUserQueryWindow{
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
	}
}