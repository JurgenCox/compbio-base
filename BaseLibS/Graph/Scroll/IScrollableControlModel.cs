namespace BaseLibS.Graph.Scroll{
	public interface IScrollableControlModel{
		void ProcessCmdKey(Keys2 keyData);
		void InvalidateBackgroundImages();
		void OnSizeChanged();
	}
}