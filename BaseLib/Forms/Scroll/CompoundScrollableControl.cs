using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLib.Graphic;
using BaseLibS.Graph;
using BaseLibS.Graph.Base;

namespace BaseLib.Forms.Scroll{
	public sealed class CompoundScrollableControl : UserControl, ICompoundScrollableControl{
		private int rowHeaderWidth = 40;
		private int rowFooterWidth;
		private int columnHeaderHeight = 40;
		private int columnFooterHeight;
		private int visibleX;
		private int visibleY;
		private BasicTableLayoutView tableLayoutPanel1;
		private BasicControl tableLayoutControl;
		private BasicTableLayoutView tableLayoutPanel2;
		private BasicView horizontalScrollBarView;
		private BasicView verticalScrollBarView;
		private BasicView mainView;
		private BasicView rowHeaderView;
		private BasicView rowFooterView;
		private BasicView rowSpacerView;
		private BasicView columnHeaderView;
		private BasicView columnFooterView;
		private BasicView columnSpacerView;
		private BasicView cornerView;
		private BasicView smallCornerView;
		private BasicView middleCornerView;
		public Action<BasicMouseEventArgs> OnMouseClickMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedMainView { get; set; }
		public Action<EventArgs> OnMouseHoverMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpMainView { get; set; }
		public Action<EventArgs> OnMouseLeaveMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickRowHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickRowHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedRowHeaderView { get; set; }
		public Action<EventArgs> OnMouseHoverRowHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownRowHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpRowHeaderView { get; set; }
		public Action<EventArgs> OnMouseLeaveRowHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveRowHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickRowFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickRowFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedRowFooterView { get; set; }
		public Action<EventArgs> OnMouseHoverRowFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownRowFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpRowFooterView { get; set; }
		public Action<EventArgs> OnMouseLeaveRowFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveRowFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickRowSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickRowSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedRowSpacerView { get; set; }
		public Action<EventArgs> OnMouseHoverRowSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownRowSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpRowSpacerView { get; set; }
		public Action<EventArgs> OnMouseLeaveRowSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveRowSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickColumnHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickColumnHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedColumnHeaderView { get; set; }
		public Action<EventArgs> OnMouseHoverColumnHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownColumnHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpColumnHeaderView { get; set; }
		public Action<EventArgs> OnMouseLeaveColumnHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveColumnHeaderView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickColumnFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickColumnFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedColumnFooterView { get; set; }
		public Action<EventArgs> OnMouseHoverColumnFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownColumnFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpColumnFooterView { get; set; }
		public Action<EventArgs> OnMouseLeaveColumnFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveColumnFooterView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickColumnSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickColumnSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedColumnSpacerView { get; set; }
		public Action<EventArgs> OnMouseHoverColumnSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownColumnSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpColumnSpacerView { get; set; }
		public Action<EventArgs> OnMouseLeaveColumnSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveColumnSpacerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedCornerView { get; set; }
		public Action<EventArgs> OnMouseHoverCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpCornerView { get; set; }
		public Action<EventArgs> OnMouseLeaveCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickMiddleCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickMiddleCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedMiddleCornerView { get; set; }
		public Action<EventArgs> OnMouseHoverMiddleCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownMiddleCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpMiddleCornerView { get; set; }
		public Action<EventArgs> OnMouseLeaveMiddleCornerView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveMiddleCornerView { get; set; }
		public Action<IGraphics, int, int, int, int, bool> OnPaintMainView { get; set; }

		public void ExportGraphic(string name, bool showDialog){
			ExportGraphics.ExportGraphic(this, name, showDialog);
		}

		public void AddContextMenuItem(string text, EventHandler action){
			ToolStripMenuItem menuItem = new ToolStripMenuItem{Size = new Size(209, 22), Text = text};
			menuItem.Click += action;
			ContextMenuStrip.Items.Add(menuItem);
		}

		public void AddContextMenuSeparator(){
			ContextMenuStrip.Items.Add(new ToolStripSeparator());
		}

		public void InitContextMenu(){
			ContextMenuStrip = new ContextMenuStrip();
		}

		public Tuple<int, int> GetContextMenuPosition(){
			Point p = ContextMenuStrip.PointToScreen(new Point(0, 0));
			return new Tuple<int, int>(p.X, p.Y);
		}

		public void SetClipboardData(object data){
			Clipboard.Clear();
			Clipboard.SetDataObject(data);
		}

		public void ShowMessage(string text){
			MessageBox.Show(text);
		}

		public string GetClipboardText(){
			return Clipboard.GetText();
		}

		public bool QueryFontColor(Font2 fontIn, Color2 colorIn, out Font2 font, out Color2 color){
			font = null;
			color = Color2.Empty;
			FontDialog fontDialog = new FontDialog{
				ShowColor = true,
				Font = GraphUtils.ToFont(fontIn),
				Color = GraphUtils.ToColor(colorIn)
			};
			bool ok = fontDialog.ShowDialog() == DialogResult.OK;
			if (ok){
				font = GraphUtils.ToFont2(fontDialog.Font);
				color = GraphUtils.ToColor2(fontDialog.Color);
			}
			fontDialog.Dispose();
			return ok;
		}

		public int Width1 => Width;
		public int Height1 => Height;
		public Action<IGraphics, int, int> OnPaintRowHeaderView { get; set; }
		public Action<IGraphics, int, int> OnPaintRowFooterView { get; set; }
		public Action<IGraphics, int, int> OnPaintColumnHeaderView { get; set; }
		public Action<IGraphics, int, int> OnPaintColumnFooterView { get; set; }
		public Action<IGraphics> OnPaintColumnSpacerView { get; set; }
		public Action<IGraphics> OnPaintRowSpacerView { get; set; }
		public Action<IGraphics> OnPaintCornerView { get; set; }
		public Action<IGraphics> OnPaintMiddleCornerView { get; set; }
		private readonly ToolTip columnViewToolTip = new ToolTip();
		private readonly float sfx;
		public CompoundScrollableControl(){
			sfx = FormUtils.GetDpiScale(CreateGraphics());
			InitializeComponent2();
			ResizeRedraw = true;
			DoubleBuffered = true;
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
			DeltaX = () => (Width - RowHeaderWidth)/20;
			DeltaY = () => (Height - ColumnHeaderHeight)/20;
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

		public void InvalidateOverview(){}

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

		public void InvalidateMainView(){
			mainView.Invalidate();
		}

		public int RowHeaderWidth{
			get => rowHeaderWidth;
			set{
				rowHeaderWidth = value;
				tableLayoutPanel2.ColumnStyles[0] = new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, value *sfx * client?.UserSf ?? 1);
			}
		}

		public int RowFooterWidth{
			get => rowFooterWidth;
			set{
				rowFooterWidth = value;
				tableLayoutPanel2.ColumnStyles[2] = new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, value * sfx * client?.UserSf ?? 1);
			}
		}

		public int ColumnHeaderHeight{
			get => columnHeaderHeight;
			set{
				columnHeaderHeight = value;
				tableLayoutPanel2.RowStyles[0] = new BasicRowStyle(BasicSizeType.AbsoluteResizeable, value * sfx * client?.UserSf ?? 1);
			}
		}

		public int ColumnFooterHeight{
			get => columnFooterHeight;
			set{
				columnFooterHeight = value;
				tableLayoutPanel2.RowStyles[2] = new BasicRowStyle(BasicSizeType.AbsoluteResizeable, value * sfx * client?.UserSf ?? 1);
			}
		}

		public Func<int> TotalWidth { get; set; }
		public Func<int> TotalHeight { get; set; }
		public Func<int> DeltaX { get; set; }
		public Func<int> DeltaY { get; set; }
		public Func<int> DeltaUpToSelection { get; set; }
		public Func<int> DeltaDownToSelection { get; set; }
		public int VisibleWidth => Width - RowHeaderWidth - RowFooterWidth - (int)(sfx * GraphUtil.scrollBarWidth);
		public int VisibleHeight => Height - ColumnHeaderHeight - ColumnFooterHeight - (int)(sfx *GraphUtil.scrollBarWidth);
		public int TotalClientWidth => TotalWidth() + RowHeaderWidth + RowFooterWidth;
		public int TotalClientHeight => TotalHeight() + ColumnHeaderHeight + ColumnFooterHeight;
		public float ZoomFactor => 1f;
		private ICompoundScrollableControlModel client;

		public ICompoundScrollableControlModel Client{
			set{
				client = value;
				value.Register(this, sfx);
			}
		}

		protected override void OnResize(EventArgs e){
			if (TotalWidth == null || TotalHeight == null){
				return;
			}
			VisibleX = Math.Max(0, Math.Min(VisibleX, TotalWidth() - VisibleWidth - 1));
			VisibleY = Math.Max(0, Math.Min(VisibleY, TotalHeight() - VisibleHeight - 1));
			InvalidateBackgroundImages();
			base.OnResize(e);
		}

		public void MoveUp(int delta){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Max(0, VisibleY - delta);
		}

		public Tuple<int, int> GetOrigin(){
			Point q = PointToScreen(new Point(0, 0));
			return new Tuple<int, int>(q.X, q.Y);
		}

		public void MoveDown(int delta){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Min(TotalHeight() - VisibleHeight, VisibleY + delta);
		}

		private void InitializeComponent2(){
			tableLayoutPanel1 = new BasicTableLayoutView();
			tableLayoutPanel2 = new BasicTableLayoutView();
			mainView = new ScrollableControlMainView(this);
			rowHeaderView = new ScrollableControlRowHeaderView(this);
			rowFooterView = new ScrollableControlRowFooterView(this);
			rowSpacerView = new ScrollableControlRowSpacerView(this);
			columnHeaderView = new ScrollableControlColumnHeaderView(this);
			columnFooterView = new ScrollableControlColumnFooterView(this);
			columnSpacerView = new ScrollableControlColumnSpacerView(this);
			horizontalScrollBarView = new HorizontalScrollBarView(this, sfx);
			verticalScrollBarView = new VerticalScrollBarView(this, sfx);
			cornerView = new ScrollableControlCornerView(this);
			middleCornerView = new ScrollableControlMiddleCornerView(this);
			smallCornerView = new ScrollableControlSmallCornerView();
			SuspendLayout();
			tableLayoutPanel1.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Absolute, sfx * GraphUtil.scrollBarWidth));
			tableLayoutPanel1.Add(tableLayoutPanel2, 0, 0);
			tableLayoutPanel1.Add(horizontalScrollBarView, 0, 1);
			tableLayoutPanel1.Add(verticalScrollBarView, 1, 0);
			tableLayoutPanel1.Add(smallCornerView, 1, 1);
			tableLayoutPanel1.RowStyles.Add(new BasicRowStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new BasicRowStyle(BasicSizeType.Absolute, sfx * GraphUtil.scrollBarWidth));
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, sfx * rowHeaderWidth));
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.AbsoluteResizeable, sfx * rowFooterWidth));
			tableLayoutPanel2.Add(mainView, 1, 1);
			tableLayoutPanel2.Add(rowHeaderView, 0, 1);
			tableLayoutPanel2.Add(rowFooterView, 2, 1);
			tableLayoutPanel2.Add(rowSpacerView, 0, 2);
			tableLayoutPanel2.Add(columnHeaderView, 1, 0);
			tableLayoutPanel2.Add(columnFooterView, 1, 2);
			tableLayoutPanel2.Add(columnSpacerView, 2, 0);
			tableLayoutPanel2.Add(cornerView, 0, 0);
			tableLayoutPanel2.Add(middleCornerView, 2, 2);
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.AbsoluteResizeable, sfx * columnHeaderHeight));
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.AbsoluteResizeable, sfx * columnFooterHeight));
			AutoScaleDimensions = new SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			tableLayoutControl = BasicControl.CreateControl(tableLayoutPanel1);
			Controls.Add(tableLayoutControl);
			Name = "ScrollableControl2";
			Size = new Size(409, 390);
			ResumeLayout(false);
		}

		protected override void OnMouseWheel(MouseEventArgs e){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Min(Math.Max(0, VisibleY - (int) Math.Round(VisibleHeight*0.001*e.Delta)),
				TotalHeight() - VisibleHeight);
			verticalScrollBarView.Invalidate();
			base.OnMouseWheel(e);
		}

		public void Print(IGraphics g, int width, int height){
			tableLayoutPanel2.InvalidateSizes();
			tableLayoutPanel2.Print(g, width, height);
			tableLayoutPanel2.InvalidateSizes();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData){
			client?.ProcessCmdKey((Keys2) keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnSizeChanged(EventArgs e){
			base.OnSizeChanged(e);
			client?.OnSizeChanged();
		}

		public void HideColumnViewToolTip(){
			columnViewToolTip.Hide(this);
		}

		public void ShowColumnViewToolTip(string text, int x, int y){
			columnViewToolTip.Show(text, this, x, y);
		}

		public void SetColumnViewToolTipTitle(string title){
			columnViewToolTip.ToolTipTitle = title;
		}

		public bool SaveFileDialog(out string fileName, string filter){
			SaveFileDialog ofd = new SaveFileDialog{Filter = filter};
			if (ofd.ShowDialog() == DialogResult.OK){
				fileName = ofd.FileName;
				return true;
			}
			fileName = null;
			return false;
		}

		public bool IsControlPressed(){
			return (ModifierKeys & Keys.Control) == Keys.Control;
		}

		public bool IsShiftPressed(){
			return (ModifierKeys & Keys.Shift) == Keys.Shift;
		}

		public void SetCursor(Cursors2 cursor){
			Cursor.Current = GraphUtils.ToCursor(cursor);
		}

		public float DpiScale => FormUtils.GetDpiScale(CreateGraphics());
	}
}