namespace BaseLibS.Graph.Image{
	public interface IImageBase {
		Color2[] Pixels { get; }
		void SetPixels(int width, int height, Color2[] pixels);
		void ClonePixels(int width, int height, Color2[] pixels);
		IPixelAccessor Lock();
		RectangleI2 Bounds { get; }
		int Quality { get; set; }
		int FrameDelay { get; set; }
		int MaxWidth { get; set; }
		int MaxHeight { get; set; }
		int Width { get; }
		int Height { get; }
		double PixelRatio { get; }
	}
}