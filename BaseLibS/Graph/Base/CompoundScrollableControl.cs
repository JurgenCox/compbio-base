﻿using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Scroll;
namespace BaseLibS.Graph.Base{
	public sealed class CompoundScrollableControl : ICompoundScrollableControl{
		public IGenericControl Parent{ get; set; }
		private int rowHeaderWidth = 40;
		private int rowFooterWidth;
		private int columnHeaderHeight = 40;
		private int columnFooterHeight;
		private int visibleX;
		private int visibleY;
		public event Action OnZoomChanged;
		public Bitmap2 OverviewBitmap{ get; set; }
		private TableLayoutModel tableLayoutPanel1;
		private TableLayoutModel tableLayoutPanel2;
		private BasicControlModel horizontalScrollBarView;
		private BasicControlModel verticalScrollBarView;
		private BasicControlModel mainView;
		private BasicControlModel rowHeaderView;
		private BasicControlModel rowFooterView;
		private BasicControlModel rowSpacerView;
		private BasicControlModel columnHeaderView;
		private BasicControlModel columnFooterView;
		private BasicControlModel columnSpacerView;
		private BasicControlModel cornerView;
		private BasicControlModel smallCornerView;
		private BasicControlModel middleCornerView;
		public Action<BasicMouseEventArgs> OnMouseClickMainView{ get; set; }
		public Tuple<int, int> GetOrigin(){
			return Parent.GetOrigin();
		}
		public Action<BasicMouseEventArgs> OnMouseDoubleClickMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedMainView{ get; set; }
		public Action<EventArgs> OnMouseHoverMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpMainView{ get; set; }
		public Action<EventArgs> OnMouseLeaveMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickRowHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickRowHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedRowHeaderView{ get; set; }
		public Action<EventArgs> OnMouseHoverRowHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownRowHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpRowHeaderView{ get; set; }
		public Action<EventArgs> OnMouseLeaveRowHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveRowHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickRowFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickRowFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedRowFooterView{ get; set; }
		public Action<EventArgs> OnMouseHoverRowFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownRowFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpRowFooterView{ get; set; }
		public Action<EventArgs> OnMouseLeaveRowFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveRowFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickRowSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickRowSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedRowSpacerView{ get; set; }
		public Action<EventArgs> OnMouseHoverRowSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownRowSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpRowSpacerView{ get; set; }
		public Action<EventArgs> OnMouseLeaveRowSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveRowSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickColumnHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickColumnHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedColumnHeaderView{ get; set; }
		public Action<EventArgs> OnMouseHoverColumnHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownColumnHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpColumnHeaderView{ get; set; }
		public Action<EventArgs> OnMouseLeaveColumnHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveColumnHeaderView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickColumnFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickColumnFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedColumnFooterView{ get; set; }
		public Action<EventArgs> OnMouseHoverColumnFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownColumnFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpColumnFooterView{ get; set; }
		public Action<EventArgs> OnMouseLeaveColumnFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveColumnFooterView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickColumnSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickColumnSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedColumnSpacerView{ get; set; }
		public Action<EventArgs> OnMouseHoverColumnSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownColumnSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpColumnSpacerView{ get; set; }
		public Action<EventArgs> OnMouseLeaveColumnSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveColumnSpacerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedCornerView{ get; set; }
		public Action<EventArgs> OnMouseHoverCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpCornerView{ get; set; }
		public Action<EventArgs> OnMouseLeaveCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickMiddleCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickMiddleCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedMiddleCornerView{ get; set; }
		public Action<EventArgs> OnMouseHoverMiddleCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownMiddleCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpMiddleCornerView{ get; set; }
		public Action<EventArgs> OnMouseLeaveMiddleCornerView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveMiddleCornerView{ get; set; }
		public Action<IGraphics, int, int, int, int, bool> OnPaintMainView{ get; set; }
		public Action<IGraphics, int, int> OnPaintRowHeaderView{ get; set; }
		public Action<IGraphics, int, int> OnPaintRowFooterView{ get; set; }
		public Action<IGraphics, int, int> OnPaintColumnHeaderView{ get; set; }
		public Action<IGraphics, int, int> OnPaintColumnFooterView{ get; set; }
		public Action<IGraphics> OnPaintColumnSpacerView{ get; set; }
		public Action<IGraphics> OnPaintRowSpacerView{ get; set; }
		public Action<IGraphics> OnPaintCornerView{ get; set; }
		public Action<IGraphics> OnPaintMiddleCornerView{ get; set; }
		public void ExportGraphic(string name, bool showDialog){
			Parent.ExportGraphic(name, showDialog);
		}
		public bool HasOverview{ get; set; } = true;
		public bool HasZoomButtons{ get; set; } = true;
		public bool Enabled => Parent.Enabled;
		public ScrollBarMode HorizontalScrollbarMode{ get; set; } = ScrollBarMode.Always;
		public ScrollBarMode VerticalScrollbarMode{ get; set; } = ScrollBarMode.Always;
		public CompoundScrollableControl(IGenericControl parent){
			Parent = parent;
			Parent.SetModel(CreateModel());
			Parent.onResize = () => {
				if (TotalWidth == null || TotalHeight == null){
					return;
				}
				VisibleX = Math.Max(0, Math.Min(VisibleX, TotalWidth() - VisibleWidth - 1));
				VisibleY = Math.Max(0, Math.Min(VisibleY, TotalHeight() - VisibleHeight - 1));
				InvalidateBackgroundImages();
			};
			Parent.onMouseWheel = delta => {
				if (TotalHeight() <= VisibleHeight){
					return;
				}
				VisibleY = Math.Min(Math.Max(0, VisibleY - (int) Math.Round(VisibleHeight * 0.001 * delta)),
					TotalHeight() - VisibleHeight);
				verticalScrollBarView.Invalidate();
			};
			Parent.onSizeChanged = () => { client?.OnSizeChanged(); };
			Parent.processCmdKey = (keyData, keyboardId) => { client?.ProcessCmdKey(keyData, keyboardId); };
			OnPaintMainView = (g, x, y, width, height, isOverview) => { };
			OnPaintRowHeaderView = (g, y, height) => { };
			OnPaintRowFooterView = (g, y, height) => { };
			OnPaintColumnHeaderView = (g, x, width) => { };
			OnPaintColumnFooterView = (g, x, width) => { };
			OnPaintColumnSpacerView = g => { };
			OnPaintRowSpacerView = g => { };
			OnPaintCornerView = g => { };
			OnPaintMiddleCornerView = g => { };
			TotalWidth = () => 200;
			TotalHeight = () => 200;
			DeltaX = () => (Parent.Width1 - RowHeaderWidth - RowFooterWidth) / 20;
			DeltaY = () => (Parent.Height1 - ColumnHeaderHeight - ColumnFooterHeight) / 20;
			DeltaUpToSelection = () => 0;
			DeltaDownToSelection = () => 0;
		}
		public void InvalidateBackgroundImages(){
			client?.InvalidateBackgroundImages();
		}
		public void InvalidateScrollbars(){
			horizontalScrollBarView.Invalidate();
			verticalScrollBarView.Invalidate();
		}
		public void InvalidateOverview(){
		}
		public void EnableContent(){
			mainView.Enabled = true;
			rowHeaderView.Enabled = true;
			rowFooterView.Enabled = true;
			rowSpacerView.Enabled = true;
			columnHeaderView.Enabled = true;
			columnFooterView.Enabled = true;
			columnSpacerView.Enabled = true;
			cornerView.Enabled = true;
			middleCornerView.Enabled = true;
		}
		public void DisableContent(){
			mainView.Enabled = false;
			rowHeaderView.Enabled = false;
			rowFooterView.Enabled = false;
			rowSpacerView.Enabled = false;
			columnHeaderView.Enabled = false;
			columnFooterView.Enabled = false;
			columnSpacerView.Enabled = false;
			cornerView.Enabled = false;
			middleCornerView.Enabled = false;
		}
		public int VisibleX{
			get => visibleX;
			set{
				visibleX = value;
				InvalidateBackgroundImages();
				mainView.Invalidate();
				columnHeaderView.Invalidate();
				columnFooterView.Invalidate();
				horizontalScrollBarView.Invalidate();
			}
		}
		public int VisibleY{
			get => visibleY;
			set{
				visibleY = value;
				InvalidateBackgroundImages();
				mainView.Invalidate();
				rowHeaderView.Invalidate();
				rowFooterView.Invalidate();
				verticalScrollBarView.Invalidate();
			}
		}
		public void InvalidateColumnHeaderView(){
			columnHeaderView.Invalidate();
			horizontalScrollBarView.Invalidate();
		}
		public void InvalidateCornerView(){
			cornerView.Invalidate();
		}
		public void InvalidateRowHeaderView(){
			rowHeaderView.Invalidate();
			verticalScrollBarView.Invalidate();
		}
		public void Invalidate(bool p0){
			Parent.Invalidate(p0);
		}
		public void InvalidateMainView(){
			mainView.Invalidate();
		}
		public int RowHeaderWidth{
			get => rowHeaderWidth;
			set{
				rowHeaderWidth = value;
				tableLayoutPanel2.ColumnStyles[0] =
					new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, value * client?.UserSf ?? 1);
			}
		}
		public int RowFooterWidth{
			get => rowFooterWidth;
			set{
				rowFooterWidth = value;
				tableLayoutPanel2.ColumnStyles[2] =
					new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, value * client?.UserSf ?? 1);
			}
		}
		public int ColumnHeaderHeight{
			get => columnHeaderHeight;
			set{
				columnHeaderHeight = value;
				tableLayoutPanel2.RowStyles[0] =
					new BasicRowStyle(BasicSizeType.AbsoluteResizeable, value * client?.UserSf ?? 1);
			}
		}
		public int ColumnFooterHeight{
			get => columnFooterHeight;
			set{
				columnFooterHeight = value;
				tableLayoutPanel2.RowStyles[2] =
					new BasicRowStyle(BasicSizeType.AbsoluteResizeable, value * client?.UserSf ?? 1);
			}
		}
		public int Width1 => Parent.Width1;
		public int Height1 => Parent.Height1;
		public Func<int> TotalWidth{ get; set; }
		public Func<int> TotalHeight{ get; set; }
		public Func<int> DeltaX{ get; set; }
		public Func<int> DeltaY{ get; set; }
		public Func<int> DeltaUpToSelection{ get; set; }
		public Func<int> DeltaDownToSelection{ get; set; }
		public int VisibleWidth => Parent.Width1 - RowHeaderWidth - RowFooterWidth - (GraphUtil.scrollBarWidth);
		public int VisibleHeight =>
			Parent.Height1 - ColumnHeaderHeight - ColumnFooterHeight - (GraphUtil.scrollBarWidth);
		public int TotalClientWidth => TotalWidth() + RowHeaderWidth + RowFooterWidth;
		public int TotalClientHeight => TotalHeight() + ColumnHeaderHeight + ColumnFooterHeight;
		public float ZoomFactor{ get; set; } = 1;
		private ICompoundScrollableControlModel client;
		public ICompoundScrollableControlModel Client{
			set{
				client = value;
				value.Register(this);
			}
		}
		public void MoveUp(int delta){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Max(0, VisibleY - delta);
		}
		public void MoveDown(int delta){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Min(TotalHeight() - VisibleHeight, VisibleY + delta);
		}
		public void MoveLeft(int delta){
			if (TotalWidth() <= VisibleWidth){
				return;
			}
			VisibleX = Math.Max(0, VisibleX - delta);
		}
		public void MoveRight(int delta){
			if (TotalWidth() <= VisibleWidth){
				return;
			}
			VisibleX = Math.Min(TotalWidth() - VisibleWidth, VisibleX + delta);
		}
		public void SetToolTipTitle(string title){
			Parent.SetToolTipTitle(title);
		}
		public void ShowToolTip(string text, int x, int y){
			Parent.ShowToolTip(text, x, y);
		}
		public void HideToolTip(){
			Parent.HideToolTip();
		}
		public BasicControlModel CreateModel(){
			tableLayoutPanel1 = new TableLayoutModel();
			tableLayoutPanel2 = new TableLayoutModel();
			mainView = new ScrollableControlMainView(this);
			rowHeaderView = new ScrollableControlRowHeaderView(this);
			rowFooterView = new ScrollableControlRowFooterView(this);
			rowSpacerView = new ScrollableControlRowSpacerView(this);
			columnHeaderView = new ScrollableControlColumnHeaderView(this);
			columnFooterView = new ScrollableControlColumnFooterView(this);
			columnSpacerView = new ScrollableControlColumnSpacerView(this);
			horizontalScrollBarView = new HorizontalScrollBarView(this);
			verticalScrollBarView = new VerticalScrollBarView(this);
			cornerView = new ScrollableControlCornerView(this);
			middleCornerView = new ScrollableControlMiddleCornerView(this);
			smallCornerView = new ScrollableControlSmallCornerView();
			tableLayoutPanel1.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Absolute, GraphUtil.scrollBarWidth));
			tableLayoutPanel1.Add(tableLayoutPanel2, 0, 0);
			tableLayoutPanel1.Add(horizontalScrollBarView, 0, 1);
			tableLayoutPanel1.Add(verticalScrollBarView, 1, 0);
			tableLayoutPanel1.Add(smallCornerView, 1, 1);
			tableLayoutPanel1.RowStyles.Add(new BasicRowStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new BasicRowStyle(BasicSizeType.Absolute, GraphUtil.scrollBarWidth));
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, rowHeaderWidth));
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, rowFooterWidth));
			tableLayoutPanel2.Add(mainView, 1, 1);
			tableLayoutPanel2.Add(rowHeaderView, 0, 1);
			tableLayoutPanel2.Add(rowFooterView, 2, 1);
			tableLayoutPanel2.Add(rowSpacerView, 0, 2);
			tableLayoutPanel2.Add(columnHeaderView, 1, 0);
			tableLayoutPanel2.Add(columnFooterView, 1, 2);
			tableLayoutPanel2.Add(columnSpacerView, 2, 0);
			tableLayoutPanel2.Add(cornerView, 0, 0);
			tableLayoutPanel2.Add(middleCornerView, 2, 2);
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.AbsoluteResizeable, columnHeaderHeight));
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.AbsoluteResizeable, columnFooterHeight));
			return tableLayoutPanel1;
		}
		public void Print(IGraphics g, int width, int height){
			tableLayoutPanel2.InvalidateSizes();
			tableLayoutPanel2.Print(g, width, height);
			tableLayoutPanel2.InvalidateSizes();
		}
		public SizeI2 TotalSize => new SizeI2(TotalWidth(), TotalHeight());
		public RectangleI2 VisibleWin => new RectangleI2(visibleX, visibleY, VisibleWidth, VisibleHeight);
		public void UpdateZoom(){
			OnZoomChanged?.Invoke();
		}
		public Color2 BackColor2{ get; set; }
		public Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight){
			return Parent.CreateOverviewBitmap(overviewWidth, overviewHeight, TotalWidth(), TotalHeight(),
				OnPaintMainView);
		}
		public void InitContextMenu(){
			Parent.InitContextMenu();
		}
		public void AddContextMenuItem(string text, EventHandler action){
			Parent.AddContextMenuItem(text, action);
		}
		public void AddContextMenuSeparator(){
			Parent.AddContextMenuSeparator();
		}
		public Tuple<int, int> GetContextMenuPosition(){
			return Parent.GetContextMenuPosition();
		}
		public void SetClipboardData(object data){
			Parent.SetClipboardData(data);
		}
		public void ShowMessage(string text){
			Parent.ShowMessage(text);
		}
		public string GetClipboardText(){
			return Parent.GetClipboardText();
		}
		public (bool, Font2, Color2) QueryFontColor(Font2 fontIn, Color2 colorIn){
			return Parent.QueryFontColor(fontIn, colorIn);
		}
		public (bool, string) SaveFileDialog(string filter){
			return Parent.SaveFileDialog(filter);
		}
		public bool IsControlPressed(){
			return Parent.IsControlPressed();
		}
		public bool IsShiftPressed(){
			return Parent.IsShiftPressed();
		}
		public void SetCursor(Cursors2 cursor){
			Parent.SetCursor(cursor);
		}
	}
}