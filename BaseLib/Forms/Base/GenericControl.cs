using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Graphic;
using BaseLibS.Drawing;
namespace BaseLib.Forms.Base{
	public class GenericControl : Control{
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
		public string GetClipboardText(){
			return Clipboard.GetText();
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
		public void SetCursor(Cursors2 cursor){
			Cursor.Current = GraphUtils.ToCursor(cursor);
		}
		public void ShowMessage(string text) {
			MessageBox.Show(text);
		}
	}
}