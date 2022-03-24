using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;
namespace BaseLibS.Graph.Scroll{
	public class SimpleScrollableControlModel : BasicControlModel, ISimpleScrollableControl{
		public IGenericControl Parent{ get; set; }
		private int visibleX;
		private int visibleY;
		private BasicControlModel horizontalScrollBar;
		private BasicControlModel verticalScrollBar;
		private BasicControlModel mainView;
		private BasicControlModel smallCornerView;
		private TableLayoutModel tableLayoutPanel1;
		public Action<IGraphics, int, int, int, int, bool> OnPaintMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedMainView{ get; set; }
		public Action<EventArgs> OnMouseHoverMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpMainView{ get; set; }
		public Action<EventArgs> OnMouseLeaveMainView{ get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveMainView{ get; set; }
		public Tuple<int, int> GetOrigin() {
			return Parent.GetOrigin();
		}
		public float ZoomFactor{ get; set; } = 1;
		public void ExportGraphic(string name, bool showDialog){
			Parent.ExportGraphic(name, showDialog);
		}
		public bool HasOverview{ get; set; } = true;
		private ScrollBarMode horizontalScrollbarMode = ScrollBarMode.Always;
		public ScrollBarMode HorizontalScrollbarMode{
			get => horizontalScrollbarMode;
			set{
				if (horizontalScrollbarMode == value){
					return;
				}
				switch (value){
					case ScrollBarMode.Never:
						tableLayoutPanel1.RowStyles[1] = new BasicRowStyle(BasicSizeType.Absolute, 0);
						break;
					case ScrollBarMode.Always:
						tableLayoutPanel1.RowStyles[1] =
							new BasicRowStyle(BasicSizeType.Absolute, GraphUtil.scrollBarWidth);
						break;
					case ScrollBarMode.Auto:
						//TODO
						break;
				}
				horizontalScrollbarMode = value;
			}
		}
		private ScrollBarMode verticalScrollbarMode = ScrollBarMode.Always;
		public ScrollBarMode VerticalScrollbarMode{
			get => verticalScrollbarMode;
			set{
				if (verticalScrollbarMode == value){
					return;
				}
				switch (value){
					case ScrollBarMode.Never:
						tableLayoutPanel1.ColumnStyles[1] = new BasicColumnStyle(BasicSizeType.Absolute, 0);
						break;
					case ScrollBarMode.Always:
						tableLayoutPanel1.ColumnStyles[1] =
							new BasicColumnStyle(BasicSizeType.Absolute, GraphUtil.scrollBarWidth);
						break;
					case ScrollBarMode.Auto:
						//TODO
						break;
				}
				verticalScrollbarMode = value;
			}
		}
		public override bool Enabled => Parent.Enabled;
		public bool HasZoomButtons{ get; set; } = true;
		public Bitmap2 OverviewBitmap{ get; set; }
		public event Action OnZoomChanged;
		public SimpleScrollableControlModel(IGenericControl parent, ISimpleScrollableControlModel model){
			Client = model;
			Parent = parent;
			CreateModel();
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
				verticalScrollBar.Invalidate();
			};
			Parent.onSizeChanged = () => { client?.OnSizeChanged(); };
			Parent.processCmdKey = (keyData, keyboardId) => { client?.ProcessCmdKey(keyData, keyboardId); };
			OnPaintMainView = (g, x, y, width, height, isOverview) => { };
			TotalWidth = () => 200;
			TotalHeight = () => 200;
			DeltaX = () => Width1 / 20;
			DeltaY = () => Height1 / 20;
			DeltaUpToSelection = () => 0;
			DeltaDownToSelection = () => 0;
		}
		public override void InvalidateBackgroundImages(){
			client?.InvalidateBackgroundImages();
		}
		public void InvalidateScrollbars(){
			horizontalScrollBar.Invalidate();
			verticalScrollBar.Invalidate();
		}
		public void InvalidateOverview(){
			OverviewBitmap = null;
		}
		public void EnableContent(){
			mainView.Enabled = true;
		}
		public void DisableContent(){
			mainView.Enabled = false;
		}
		public int VisibleX{
			get => visibleX;
			set{
				visibleX = value;
				InvalidateBackgroundImages();
				mainView.Invalidate();
				horizontalScrollBar.Invalidate();
			}
		}
		public int VisibleY{
			get => visibleY;
			set{
				visibleY = value;
				InvalidateBackgroundImages();
				mainView.Invalidate();
				verticalScrollBar.Invalidate();
			}
		}
		public void Invalidate(bool p0){
			invalidate();
		}
		public void InvalidateMainView(){
			mainView.Invalidate();
		}
		public RectangleI2 VisibleWin => new RectangleI2(visibleX, visibleY, VisibleWidth, VisibleHeight);
		public int Width1 => Parent.Width1;
		public int Height1 => Parent.Height1;
		public Func<int> TotalWidth{ get; set; }
		public Func<int> TotalHeight{ get; set; }
		public Func<int> DeltaX{ get; set; }
		public Func<int> DeltaY{ get; set; }
		public Func<int> DeltaUpToSelection{ get; set; }
		public Func<int> DeltaDownToSelection{ get; set; }
		public int TotalClientWidth => TotalWidth();
		public int TotalClientHeight => TotalHeight();
		public int VisibleWidth => Width1 - GraphUtil.scrollBarWidth;
		public int VisibleHeight => Height1 - GraphUtil.scrollBarWidth;
		private ISimpleScrollableControlModel client;
		public ISimpleScrollableControlModel Client{
			set{
				client = value;
				value.Register(this);
			}
		}
		public SizeI2 TotalSize => new SizeI2(TotalWidth(), TotalHeight());
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
		public BasicControlModel CreateModel(){
			tableLayoutPanel1 = new TableLayoutModel();
			mainView = new SimpleScrollableControlMainView(this);
			horizontalScrollBar = new HorizontalScrollBarView(this);
			verticalScrollBar = new VerticalScrollBarView(this);
			smallCornerView = new ScrollableControlSmallCornerView();
			tableLayoutPanel1.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Absolute, GraphUtil.scrollBarWidth));
			tableLayoutPanel1.Add(mainView, 0, 0);
			tableLayoutPanel1.Add(horizontalScrollBar, 0, 1);
			tableLayoutPanel1.Add(verticalScrollBar, 1, 0);
			tableLayoutPanel1.Add(smallCornerView, 1, 1);
			tableLayoutPanel1.RowStyles.Add(new BasicRowStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new BasicRowStyle(BasicSizeType.Absolute, GraphUtil.scrollBarWidth));
			return tableLayoutPanel1;
		}
		public override void Print(IGraphics g, int width, int height){
			mainView.Print(g, width, height);
		}
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
			setCursor(cursor);
		}
		public override void OnPaint(IGraphics g, int width, int height){
			tableLayoutPanel1.OnPaint(g, width, height);
		}
		public override void OnMouseDragged(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseDragged(e);
		}
		public override void OnMouseMoved(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseMoved(e);
		}
		public override void OnMouseIsUp(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseIsUp(e);
		}
		public override void OnMouseIsDown(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseIsDown(e);
		}
		public override void OnMouseLeave(EventArgs e){
			tableLayoutPanel1.OnMouseLeave(e);
		}
		public override void OnMouseClick(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseClick(e);
		}
		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseDoubleClick(e);
		}
		public override void OnMouseHover(EventArgs e){
			tableLayoutPanel1.OnMouseHover(e);
		}
		public override void OnMouseCaptureChanged(EventArgs e){
			tableLayoutPanel1.OnMouseCaptureChanged(e);
		}
		public override void OnMouseEnter(EventArgs e){
			tableLayoutPanel1.OnMouseEnter(e);
		}
		public override void OnMouseWheel(BasicMouseEventArgs e){
			tableLayoutPanel1.OnMouseWheel(e);
		}
		public override void OnResize(EventArgs e, int width, int height){
			tableLayoutPanel1.OnResize(e, width, height);
		}
		public override void Dispose(bool disposing){
			tableLayoutPanel1.Dispose(disposing);
		}
		public override void ProcessCmdKey(Keys2 keyData, int keyboardId) {
			tableLayoutPanel1.ProcessCmdKey(keyData, keyboardId);
		}
		public override void OnSizeChanged(){
			tableLayoutPanel1.OnSizeChanged();
		}
	}
}