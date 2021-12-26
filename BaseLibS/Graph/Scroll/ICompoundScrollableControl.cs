using System;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Scroll{
	public interface ICompoundScrollableControl : IScrollableControl{
		ICompoundScrollableControlModel Client { set; }
		int RowHeaderWidth { get; set; }
		int RowFooterWidth { get; set; }
		int ColumnHeaderHeight { get; set; }
		int ColumnFooterHeight { get; set; }
		bool Enabled { get; }
		Action<BasicMouseEventArgs> OnMouseDraggedRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedColumnHeaderView { get; set; }
		
		Action<BasicMouseEventArgs> OnMouseIsDownRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownColumnHeaderView { get; set; }

		Action<BasicMouseEventArgs> OnMouseMoveColumnHeaderView { get; set; }

		Action<EventArgs> OnMouseLeaveColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpColumnHeaderView { get; set; }
		Action<EventArgs> OnMouseHoverColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickColumnHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpRowHeaderView { get; set; }
		Action<EventArgs> OnMouseLeaveRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveRowFooterView { get; set; }
		Action<EventArgs> OnMouseLeaveRowFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickRowFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickRowFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownRowFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpRowFooterView { get; set; }
		Action<EventArgs> OnMouseHoverRowFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedRowFooterView { get; set; }
		Action<EventArgs> OnMouseHoverRowHeaderView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveColumnFooterView { get; set; }
		Action<EventArgs> OnMouseLeaveColumnFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickColumnFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickColumnFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownColumnFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpColumnFooterView { get; set; }
		Action<EventArgs> OnMouseHoverColumnFooterView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedColumnFooterView { get; set; }
		Action<IGraphics, int, int> OnPaintRowHeaderView { get; set; }
		Action<IGraphics> OnPaintCornerView { get; set; }
		Action<IGraphics, int, int> OnPaintColumnHeaderView { get; set; }
		Action<IGraphics, int, int> OnPaintColumnFooterView { get; set; }
		Action<IGraphics, int, int> OnPaintRowFooterView { get; set; }
		Action<IGraphics> OnPaintColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveColumnSpacerView { get; set; }
		Action<EventArgs> OnMouseLeaveColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpColumnSpacerView { get; set; }
		Action<EventArgs> OnMouseHoverColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedColumnSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedCornerView { get; set; }
		Action<EventArgs> OnMouseHoverCornerView { get; set; }
		Action<EventArgs> OnMouseLeaveCornerView { get; set; }
		Action<EventArgs> OnMouseLeaveMainView { get; set; }
		Action<IGraphics> OnPaintMiddleCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveMiddleCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickMiddleCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickMiddleCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedMiddleCornerView { get; set; }
		Action<EventArgs> OnMouseHoverMiddleCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownMiddleCornerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpMiddleCornerView { get; set; }
		Action<EventArgs> OnMouseLeaveMiddleCornerView { get; set; }
		Action<IGraphics> OnPaintRowSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseMoveRowSpacerView { get; set; }
		Action<EventArgs> OnMouseLeaveRowSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseClickRowSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDoubleClickRowSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsDownRowSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseIsUpRowSpacerView { get; set; }
		Action<EventArgs> OnMouseHoverRowSpacerView { get; set; }
		Action<BasicMouseEventArgs> OnMouseDraggedRowSpacerView { get; set; }
		void InvalidateColumnHeaderView();
		void InvalidateRowHeaderView();
		void InvalidateCornerView();
		void MoveDown(int delta);
		void MoveUp(int delta);
		void MoveLeft(int delta);
		void MoveRight(int delta);
		void SetColumnViewToolTipTitle(string title);
		void ShowColumnViewToolTip(string text, int x, int y);
		void HideColumnViewToolTip();
	}
}