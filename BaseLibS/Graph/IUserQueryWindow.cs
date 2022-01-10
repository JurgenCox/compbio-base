using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph{
	public interface IUserQueryWindow{
		void InitContextMenu();
		void AddContextMenuItem(string text, EventHandler action);
		void AddContextMenuSeparator();
		Tuple<int, int> GetContextMenuPosition();
		void SetClipboardData(object data);
		void ShowMessage(string text);
		string GetClipboardText();
		(bool, Font2, Color2) QueryFontColor(Font2 fontIn, Color2 colorIn);
		(bool, string) SaveFileDialog(string filter);
		bool IsControlPressed();
		bool IsShiftPressed();
		void SetCursor(Cursors2 cursor);
	}
}