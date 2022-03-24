using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Scroll{
	public interface IControlModel{
		void ProcessCmdKey(Keys2 keyData, int keyboardId);
		void InvalidateBackgroundImages();
		void OnSizeChanged();
		event EventHandler Close;
	}
}