using System;

namespace BaseLibS.Graph{
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
		float ZoomFactor { get; }
		void Invalidate(bool p0);
		void InvalidateMainView();
		void InvalidateScrollbars();
		void InvalidateOverview();
		Tuple<int, int> GetOrigin();
		Action<BasicMouseEventArgs> OnMouseIsDownMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedMainView { get; set; }
		Action<EventArgs> OnMouseHoverMainView { get; set; }
		Action<IGraphics, int, int, int, int, bool> OnPaintMainView { get; set; }
		void ExportGraphic(string name, bool showDialog);
		float DpiScale { get; }
	}
}