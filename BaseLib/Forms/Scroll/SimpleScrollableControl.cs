using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLib.Graphic;
using BaseLibS.Graph;
using BaseLibS.Graph.Base;

namespace BaseLib.Forms.Scroll{
	public delegate void ZoomChangeHandler2();
	public sealed class SimpleScrollableControl : UserControl, ISimpleScrollableControl{
		private int visibleX;
		private int visibleY;
		private BasicView horizontalScrollBar;
		private BasicView verticalScrollBar;
		private BasicControl mainControl;
		private BasicView mainView;
		private BasicView smallCornerView;
		public Action<IGraphics, int, int, int, int, bool> OnPaintMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseClickMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDoubleClickMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseDraggedMainView { get; set; }
		public Action<EventArgs> OnMouseHoverMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsDownMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseIsUpMainView { get; set; }
		public Action<EventArgs> OnMouseLeaveMainView { get; set; }
		public Action<BasicMouseEventArgs> OnMouseMoveMainView { get; set; }
		public float ZoomFactor { get; set; } = 1;
		internal Bitmap2 overviewBitmap;
		public event ZoomChangeHandler2 OnZoomChanged;
		private readonly float sfx;
		public SimpleScrollableControl(){
			sfx = FormUtils.GetDpiScale(CreateGraphics());
			InitializeComponent2();
			ResizeRedraw = true;
			DoubleBuffered = true;
			OnPaintMainView = (g, x, y, width, height, isOverview) => { };
			TotalWidth = () => ClientSize.Width;
			TotalHeight = () => ClientSize.Height;
			DeltaX = () => Width/20;
			DeltaY = () => Height/20;
			DeltaUpToSelection = () => 0;
			DeltaDownToSelection = () => 0;
		}

		public void InvalidateBackgroundImages(){
			client?.InvalidateBackgroundImages();
		}

		public void InvalidateScrollbars(){
			horizontalScrollBar.Invalidate();
			verticalScrollBar.Invalidate();
		}

		public void InvalidateOverview(){
			overviewBitmap = null;
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

		public void InvalidateMainView(){
			mainView.Invalidate();
		}

		public RectangleI2 VisibleWin => new RectangleI2(visibleX, visibleY, mainControl.Width, mainControl.Height);
		public int Width1 => Width;
		public int Height1 => Height;
		public Func<int> TotalWidth { get; set; }
		public Func<int> TotalHeight { get; set; }
		public Func<int> DeltaX { get; set; }
		public Func<int> DeltaY { get; set; }
		public Func<int> DeltaUpToSelection { get; set; }
		public Func<int> DeltaDownToSelection { get; set; }
		public int TotalClientWidth => TotalWidth();
		public int TotalClientHeight => TotalHeight();
		public int VisibleWidth => mainControl.Width;
		public int VisibleHeight => mainControl.Height;
		private ISimpleScrollableControlModel client;

		public void AddContextMenuItem(string text, EventHandler action){
			ToolStripMenuItem menuItem = new ToolStripMenuItem{Size = new Size(209, 22), Text = text};
			menuItem.Click += action;
			ContextMenuStrip.Items.Add(menuItem);
		}

		public void InitContextMenu(){
			ContextMenuStrip = new ContextMenuStrip();
		}

		public void AddContextMenuSeparator(){
			ContextMenuStrip.Items.Add(new ToolStripSeparator());
		}

		public void SetClipboardData(object data){
			Clipboard.Clear();
			Clipboard.SetDataObject(data);
		}

		public ISimpleScrollableControlModel Client{
			set{
				client = value;
				value.Register(this);
			}
		}

		public SizeI2 TotalSize => new SizeI2(TotalWidth(), TotalHeight());

		public Tuple<int, int> GetContextMenuPosition(){
			Point p = ContextMenuStrip.PointToScreen(new Point(0, 0));
			return new Tuple<int, int>(p.X, p.Y);
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

		public void MoveDown(int delta){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Min(TotalHeight() - VisibleHeight, VisibleY + delta);
		}

		private void InitializeComponent2(){
			TableLayoutPanel tableLayoutPanel1 = new TableLayoutPanel();
			mainView = new SimpleScrollableControlMainView(this);
			horizontalScrollBar = new HorizontalScrollBarView(this, sfx);
			verticalScrollBar = new VerticalScrollBarView(this, sfx);
			smallCornerView = new ScrollableControlSmallCornerView();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sfx*GraphUtil.scrollBarWidth));
			mainControl = BasicControl.CreateControl(mainView);
			tableLayoutPanel1.Controls.Add(mainControl, 0, 0);
			tableLayoutPanel1.Controls.Add(BasicControl.CreateControl(horizontalScrollBar), 0, 1);
			tableLayoutPanel1.Controls.Add(BasicControl.CreateControl(verticalScrollBar), 1, 0);
			tableLayoutPanel1.Controls.Add(BasicControl.CreateControl(smallCornerView), 1, 1);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, sfx*GraphUtil.scrollBarWidth));
			tableLayoutPanel1.Size = new Size(409, 390);
			tableLayoutPanel1.TabIndex = 0;
			AutoScaleDimensions = new SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(tableLayoutPanel1);
			Name = "ScrollableControl2";
			Size = new Size(409, 390);
			tableLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		protected override void OnMouseWheel(MouseEventArgs e){
			if (TotalHeight() <= VisibleHeight){
				return;
			}
			VisibleY = Math.Min(Math.Max(0, VisibleY - (int) Math.Round(VisibleHeight*0.001*e.Delta)),
				TotalHeight() - VisibleHeight);
			verticalScrollBar.Invalidate();
			base.OnMouseWheel(e);
		}

		public void Print(IGraphics g, int width, int height){
			mainView.Print(g, width, height);
		}

		public void ExportGraphic(string name, bool showDialog){
			ExportGraphics.ExportGraphic(this, name, showDialog);
		}

		public float DpiScale => FormUtils.GetDpiScale(CreateGraphics());

		public Tuple<int, int> GetOrigin(){
			Point q = PointToScreen(new Point(0, 0));
			return new Tuple<int, int>(q.X, q.Y);
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

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData){
			client?.ProcessCmdKey((Keys2) keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}

		public string GetClipboardText(){
			return Clipboard.GetText();
		}

		protected override void OnSizeChanged(EventArgs e){
			base.OnSizeChanged(e);
			client?.OnSizeChanged();
		}

		public void ShowMessage(string text){
			MessageBox.Show(text);
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

		public void UpdateZoom(){
			OnZoomChanged?.Invoke();
		}
	}
}