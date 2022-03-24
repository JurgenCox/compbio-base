using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Graphic;
using BaseLibS.Drawing;
using BaseLibS.Graph;
using BaseLibS.Graph.Base;
namespace BaseLib.Forms.Base{
	public class GenericControl : Control, IGenericControl{
		public Action onResize { get; set; }
		public Action<int> onMouseWheel { get; set; }
		public Action onSizeChanged { get; set; }
		public Action<Keys2, int> processCmdKey { get; set; }
		private ToolTip toolTip;
		public int Width1 => Width;
		public int Height1 => Height;
		public GenericControl(){
			Dock = DockStyle.Fill;
			DoubleBuffered = true;
			ResizeRedraw = true;
			Margin = new Padding(0);
		}
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
		public Tuple<int, int> GetContextMenuPosition(){
			Point p = ContextMenuStrip.PointToScreen(new Point(0, 0));
			return new Tuple<int, int>(p.X, p.Y);
		}
		public bool IsControlPressed(){
			return (ModifierKeys & Keys.Control) == Keys.Control;
		}
		public bool IsShiftPressed(){
			return (ModifierKeys & Keys.Shift) == Keys.Shift;
		}
		public Tuple<int, int> GetOrigin(){
			Point q = PointToScreen(new Point(0, 0));
			return new Tuple<int, int>(q.X, q.Y);
		}
		public void SetClipboardData(object data){
			Clipboard.Clear();
			Clipboard.SetDataObject(data);
		}
		public (bool, Font2, Color2) QueryFontColor(Font2 fontIn, Color2 colorIn){
			Font2 font = null;
			Color2 color = Color2.Empty;
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
			return (ok, font, color);
		}
		public string GetClipboardText(){
			return Clipboard.GetText();
		}
		public (bool, string) SaveFileDialog(string filter){
			SaveFileDialog ofd = new SaveFileDialog{Filter = filter};
			if (ofd.ShowDialog() == DialogResult.OK){
				return (true, ofd.FileName);
			}
			return (false, null);
		}
		public void SetCursor(Cursors2 cursor){
			Cursor.Current = GraphUtils.ToCursor(cursor);
		}
		public void ShowMessage(string text){
			MessageBox.Show(text);
		}
		protected override void OnMouseWheel(MouseEventArgs e){
			onMouseWheel?.Invoke(e.Delta);
			base.OnMouseWheel(e);
		}

		private int keyboardId = -1;

		private int KeyboardId{
			get{
				keyboardId = InputLanguage.CurrentInputLanguage.Culture.KeyboardLayoutId;
				return keyboardId;
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData){
			processCmdKey?.Invoke((Keys2) keyData, KeyboardId);
			return base.ProcessCmdKey(ref msg, keyData);
		}
		public void HideToolTip(){
			if (toolTip == null) {
				toolTip = new ToolTip();
			}
			toolTip.Hide(this);
		}
		public void ShowToolTip(string text, int x, int y){
			if (toolTip == null) {
				toolTip = new ToolTip();
			}
			toolTip.Show(text, this, x, y);
		}
		public void SetToolTipTitle(string title){
			if (toolTip == null) {
				toolTip = new ToolTip();
			}
			toolTip.ToolTipTitle = title;
		}
		public void ExportGraphic(string name, bool showDialog){
			ExportGraphics.ExportGraphic(this, name, showDialog);
		}
		public Bitmap2 CreateOverviewBitmap(int overviewWidth, int overviewHeight, int totalWidth, int totalHeight,
			Action<IGraphics, int, int, int, int, bool> onPaintMainView){
			BitmapGraphics bg =
				new BitmapGraphics(Math.Min(totalWidth, 15000), Math.Min(totalHeight, 15000));
			onPaintMainView?.Invoke(bg, 0, 0, totalWidth, totalHeight, true);
			try{
				return GraphUtils.ToBitmap2(GraphUtils.ResizeImage(bg.Bitmap, overviewWidth, overviewHeight));
			} catch (Exception){
				return GraphUtils.ToBitmap2(bg.Bitmap);
			}
		}
		public void SetModel(BasicControlModel model) {
			Controls.Add(BasicControl.CreateControl(model));
		}
		protected override void OnResize(EventArgs e) {
			onResize?.Invoke();
			base.OnResize(e);
		}
		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			onSizeChanged?.Invoke();
		}
	}
}