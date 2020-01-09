namespace BaseLibS.Graph{
	public interface IScrollableControlModel{
		void ProcessCmdKey(Keys2 keyData);
		void InvalidateBackgroundImages();
		void OnSizeChanged();
	}
}