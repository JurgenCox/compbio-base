using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Scroll;
namespace BaseLibS.Graph.Base{
	public class BasicControlModel : IPrintable, IControlModel{
		public Font2 Font{ get; set; } = new Font2("Microsoft Sans Serif", 8.25f);
		public Color2 BackColor{ get; set; }
		public Color2 ForeColor{ get; set; }
		public bool Visible{ get; set; }
		public virtual bool Enabled{ get; set; } = true;
		public Action invalidate;
		public Action resetCursor;
		public Action<Cursors2> setCursor;
		public Action<int, int, int, int, IControlModel> launchQuery;
		public Func<(int, int)> screenCoords;
		public Action<string> setToolTipText;
		public Action hideToolTip;
		public Action<string, int, int> showToolTip;
		public Action<string> setToolTipTitle;
		public Func<bool> isShiftPressed;
		public Func<bool> isControlPressed;
		public Func<string, (bool, string)> saveFileDialog;
		public Func<Font2, Color2, (bool, Font2, Color2)> queryFontColor;
		public Func<string> getClipboardText;
		public Action<string> showMessage;
		public Action<object> setClipboardData;
		public Func<Tuple<int, int>> getContextMenuPosition;
		public Action addContextMenuSeparator;
		public Action<string, EventHandler> addContextMenuItem;
		public Action initContextMenu;
		public Func<Tuple<int, int>> getOrigin;
		public Action<string, bool> exportGraphic;
		public Func<int> getWidth;
		public Func<int> getHeight;
		public Padding2 Margin{ get; set; } = Padding2.Empty;
		public bool Debug{ get; set; } = false;
		public BasicControlModel(){
			BackColor = Color2.White;
			ForeColor = Color2.Black;
			Visible = true;
		}
		public void Invalidate(){
			invalidate?.Invoke();
		}
		public void ResetCursor(){
			resetCursor?.Invoke();
		}
		public Cursors2 Cursor{
			set => setCursor?.Invoke(value);
		}
		public void LaunchQuery(int x, int y, int width, int height, IControlModel model){
			launchQuery?.Invoke(x, y, width, height, model);
		}
		public (int, int) ScreenCoords() {
			return screenCoords();
		}
		public void SetToolStripText(string text) {
			setToolTipText?.Invoke(text);
		}
		public virtual void OnPaint(IGraphics g, int width, int height){
		}
		public virtual void OnPaintBackground(IGraphics g, int width, int height){
			if (!Visible) {
				return;
			}
			g.FillRectangle(new Brush2(BackColor), Margin.Left + 1, Margin.Top + 1,
				width - Margin.Left - Margin.Right - 2,
				height - Margin.Top - Margin.Bottom - 2);
		}
		public virtual void OnMouseDragged(BasicMouseEventArgs e){
		}
		public virtual void OnMouseMoved(BasicMouseEventArgs e){
		}
		public virtual void OnMouseIsUp(BasicMouseEventArgs e){
		}
		public virtual void OnMouseIsDown(BasicMouseEventArgs e){
		}
		public virtual void OnMouseLeave(EventArgs e){
		}
		public virtual void OnMouseClick(BasicMouseEventArgs e){
		}
		public virtual void OnMouseDoubleClick(BasicMouseEventArgs e){
		}
		public virtual void OnMouseHover(EventArgs e){
		}
		public virtual void OnMouseCaptureChanged(EventArgs e){
		}
		public virtual void OnMouseEnter(EventArgs e){
		}
		public virtual void OnMouseWheel(BasicMouseEventArgs e){
		}
		public virtual void OnResize(EventArgs e, int width, int height){
		}
		public virtual void Dispose(bool disposing){
		}
		public virtual void ProcessCmdKey(Keys2 keyData, int keyboardId) {
		}
		public virtual void InvalidateBackgroundImages(){
		}
		public virtual void OnSizeChanged(){
		}
		public event EventHandler Close;
		public virtual void Print(IGraphics g, int width, int height) {
			OnPaintBackground(g, width, height);
			OnPaint(g, width, height);
		}
	}
}