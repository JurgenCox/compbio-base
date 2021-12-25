using System;

namespace BaseLibS.Graph.Scroll{
	public interface IScrollableControl : IUserQueryWindow, IPrintable{
		int Width1 { get; }
		int Height1 { get; }
		Func<int> TotalWidth { get; set; }
		Func<int> TotalHeight { get; set; }
		Func<int> DeltaX { get; set; }
		Func<int> DeltaY { get; set; }
		Func<int> DeltaUpToSelection { get; set; }
		Func<int> DeltaDownToSelection { get; set; }
		int VisibleX { get; set; }
		int VisibleY { get; set; }
		int VisibleWidth { get; }
		int VisibleHeight { get; }
		int TotalClientWidth { get; }
		int TotalClientHeight { get; }
		float ZoomFactor { get; set; }
		void Invalidate(bool p0);
		void InvalidateMainView();
		void InvalidateScrollbars();
		void InvalidateOverview();
		Tuple<int, int> GetOrigin();
		Action<BasicMouseEventArgs> OnMouseDoubleClickMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedMainView { get; set; }
		Action<EventArgs> OnMouseHoverMainView { get; set; }
		Action<EventArgs> OnMouseLeaveMainView { get; set; }
		Action<IGraphics, int, int, int, int, bool> OnPaintMainView { get; set; }
		void ExportGraphic(string name, bool showDialog);
		bool HasOverview { get; set; }
		bool HasZoomButtons { get; set; }
		ScrollBarMode HorizontalScrollbarMode { get; set; }
		ScrollBarMode VerticalScrollbarMode { get; set; }
		Color2 BackColor2 { get; set; }
		SizeI2 TotalSize { get; }
		RectangleI2 VisibleWin { get; }
		void UpdateZoom();
		Bitmap2 OverviewBitmap { get; set; }
		Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight);
	}
}