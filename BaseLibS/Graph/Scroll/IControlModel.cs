using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Scroll{
	public interface IControlModel{
		void ProcessCmdKey(Keys2 keyData);
		void InvalidateBackgroundImages();
		void OnSizeChanged();
		event EventHandler Close;
	}
}