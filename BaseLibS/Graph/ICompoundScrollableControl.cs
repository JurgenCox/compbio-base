using System;

namespace BaseLibS.Graph{
	public interface ICompoundScrollableControl : IScrollableControl{
		ICompoundScrollableControlModel Client { set; }
		int RowHeaderWidth { get; set; }
		int RowFooterWidth { get; set; }
		int ColumnHeaderHeight { get; set; }
		int ColumnFooterHeight { get; set; }
		bool Enabled { get; }
		Action<BasicMouseEventArgs> OnMouseDraggedRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickMainView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpRowHeaderView { get; set; }
		Action<EventArgs> OnMouseLeaveRowHeaderView { get; set; }
		Action<IGraphics, int, int> OnPaintRowHeaderView { get; set; }
		Action<IGraphics> OnPaintCornerView { get; set; }
		Action<IGraphics, int, int> OnPaintColumnHeaderView { get; set; }
		Action<IGraphics, int, int> OnPaintColumnFooterView { get; set; }
		void InvalidateColumnHeaderView();
		void InvalidateRowHeaderView();
		void InvalidateCornerView();
		void MoveDown(int rowHeight);
		void MoveUp(int rowHeight);
		void SetColumnViewToolTipTitle(string title);
		void ShowColumnViewToolTip(string text, int x, int y);
		void HideColumnViewToolTip();
	}
}