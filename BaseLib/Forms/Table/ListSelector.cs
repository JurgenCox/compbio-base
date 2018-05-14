using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLib.Forms.Table{
	public partial class ListSelector : UserControl{
		public event EventHandler SelectionChanged;
		private Thread downThread;
		private Thread upThread;
		private bool repeats;

		public ListSelector(){
			InitializeComponent();
			downButton.MouseDown += DownButtonMouseDown;
			downButton.MouseUp += DownButtonMouseUp;
			upButton.MouseDown += UpButtonMouseDown;
			upButton.MouseUp += UpButtonMouseUp;
			HasMoveButtons = false;
			ResizeRedraw = true;
		}

		public bool Repeats { get => repeats;
			set => repeats = value;
		}

		private void DownButtonMouseUp(object sender, MouseEventArgs e){
			downThread.Abort();
			downThread = null;
		}

		private void UpButtonMouseUp(object sender, MouseEventArgs e){
			upThread.Abort();
			upThread = null;
		}

		private void DownButtonMouseDown(object sender, MouseEventArgs e){
			downThread = new Thread(WalkDown);
			downThread.Start();
		}

		private void UpButtonMouseDown(object sender, MouseEventArgs e){
			upThread = new Thread(WalkUp);
			upThread.Start();
		}

		private void WalkDown(){
			Thread.Sleep(400);
			while (true){
				Invoke((Action) (() => DownButtonClick(null, null)));
				Thread.Sleep(150);
			}
		}

		private void WalkUp(){
			Thread.Sleep(400);
			while (true){
				Invoke((Action) (() => UpButtonClick(null, null)));
				Thread.Sleep(150);
			}
		}

		public ListBox.ObjectCollection Items => allListBox.Items;
		public ListBox.ObjectCollection SelectedItems => selectedListBox.Items;

		public string[] SelectedStrings{
			get{
				ListBox.ObjectCollection sel = SelectedItems;
				string[] result = new string[sel.Count];
				for (int i = 0; i < result.Length; i++){
					result[i] = sel[i].ToString();
				}
				return result;
			}
		}
		public int[] SelectedIndices{
			get{
				ListBox.ObjectCollection selItems = SelectedItems;
				int[] result = new int[selItems.Count];
				for (int i = 0; i < result.Length; i++){
					result[i] = GetIndexOf(selItems[i]);
				}
				return result;
			}
			set{
				selectedListBox.Items.Clear();
				foreach (int i in value){
					SetSelected(i, true);
				}
				SelectionChanged?.Invoke(this, new EventArgs());
			}
		}

		private int GetIndexOf(object o){
			ListBox.ObjectCollection items = Items;
			for (int i = 0; i < items.Count; i++){
				if (items[i].Equals(o)){
					return i;
				}
			}
			throw new IndexOutOfRangeException("Never get here.");
		}

		public void SetSelected(int index, bool value){
			if (index >= allListBox.Items.Count || index < 0){
				return;
			}
			object o = allListBox.Items[index];
			if (value){
				if (!selectedListBox.Items.Contains(o)){
					selectedListBox.Items.Add(o);
				}
			} else{
				if (selectedListBox.Items.Contains(o)){
					selectedListBox.Items.Remove(o);
				}
			}
		}

		public void SetSelected(object item, bool value){
			if (value){
				if (!selectedListBox.Items.Contains(item)){
					selectedListBox.Items.Add(item);
				}
			} else{
				if (selectedListBox.Items.Contains(item)){
					selectedListBox.Items.Remove(item);
				}
			}
		}

		private void SelectButtonClick(object sender, EventArgs e){
			foreach (object o in allListBox.SelectedItems){
				if (!selectedListBox.Items.Contains(o) || repeats){
					selectedListBox.Items.Add(o);
				}
			}
			SelectionChanged?.Invoke(this, e);
		}

		private void DeselectButtonClick(object sender, EventArgs e){
			object[] os = new object[selectedListBox.SelectedItems.Count];
			selectedListBox.SelectedItems.CopyTo(os, 0);
			foreach (object o in os){
				selectedListBox.Items.Remove(o);
			}
			SelectionChanged?.Invoke(this, e);
		}

		private void TopButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(selectedListBox);
			if (selectedIndices.Length == 0){
				return;
			}
			int n = selectedListBox.Items.Count;
			int[] unselectedIndices = ArrayUtils.Complement(selectedIndices, n);
			int[] order = ArrayUtils.Concat(selectedIndices, unselectedIndices);
			int[] selection = ArrayUtils.ConsecutiveInts(selectedIndices.Length);
			SetOrder(order, selection);
		}

		private void UpButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(selectedListBox);
			if (selectedIndices.Length == 0){
				return;
			}
			Array.Sort(selectedIndices);
			int index = -1;
			for (int i = selectedIndices.Length - 1; i >= 0; i--){
				if (i == 0 || selectedIndices[i - 1] < selectedIndices[i] - 1){
					index = i;
					break;
				}
			}
			int q = selectedIndices[index];
			if (q == 0){
				return;
			}
			int m = selectedIndices.Length - index;
			int n = selectedListBox.Items.Count;
			int[] order = new int[n];
			for (int i = 0; i < q - 1; i++){
				order[i] = i;
			}
			for (int i = 0; i < m; i++){
				order[q - 1 + i] = q + i;
			}
			order[q + m - 1] = q - 1;
			for (int i = q + m; i < n; i++){
				order[i] = i;
			}
			int[] selection = selectedIndices;
			for (int i = index; i < selection.Length; i++){
				selection[i]--;
			}
			SetOrder(order, selection);
		}

		private void DownButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(selectedListBox);
			if (selectedIndices.Length == 0){
				return;
			}
			Array.Sort(selectedIndices);
			int index = -1;
			for (int i = 0; i < selectedIndices.Length; i++){
				if (i == selectedIndices.Length - 1 || selectedIndices[i + 1] > selectedIndices[i] + 1){
					index = i;
					break;
				}
			}
			int q = selectedIndices[0];
			int n = selectedListBox.Items.Count;
			if (selectedIndices[index] == n - 1){
				return;
			}
			int m = index + 1;
			int[] order = new int[n];
			for (int i = 0; i < q; i++){
				order[i] = i;
			}
			order[q] = q + m;
			for (int i = 0; i < m; i++){
				order[q + 1 + i] = q + i;
			}
			for (int i = q + m + 1; i < n; i++){
				order[i] = i;
			}
			int[] selection = selectedIndices;
			for (int i = 0; i <= index; i++){
				selection[i]++;
			}
			SetOrder(order, selection);
		}

		private void BottomButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(selectedListBox);
			if (selectedIndices.Length == 0){
				return;
			}
			int n = selectedListBox.Items.Count;
			int[] unselectedIndices = ArrayUtils.Complement(selectedIndices, n);
			int[] order = ArrayUtils.Concat(unselectedIndices, selectedIndices);
			int[] selection = ArrayUtils.ConsecutiveInts(n - selectedIndices.Length, n);
			SetOrder(order, selection);
		}

		private void SetOrder(IList<int> order, IEnumerable<int> selection){
			object[] items = new object[selectedListBox.Items.Count];
			for (int i = 0; i < items.Length; i++){
				items[i] = selectedListBox.Items[i];
			}
			selectedListBox.Items.Clear();
			selectedListBox.ClearSelected();
			items = ArrayUtils.SubArray(items, order);
			selectedListBox.Items.AddRange(items);
			foreach (int i in selection){
				selectedListBox.SetSelected(i, true);
			}
		}

		private static int[] GetSelectedIndices(ListBox box){
			int[] result = new int[box.SelectedIndices.Count];
			for (int i = 0; i < result.Length; i++){
				result[i] = box.SelectedIndices[i];
			}
			return result;
		}

		public bool HasMoveButtons{
			get => topButton.Visible;
			set{
				topButton.Visible = value;
				upButton.Visible = value;
				downButton.Visible = value;
				bottomButton.Visible = value;
			}
		}

		public static void TrySelect(ListSelector selector, IEnumerable<string> strings){
			if (!strings.Any()){
				return;
			}
			int index = -1;
			for (int i = 0; i < selector.Items.Count; i++){
				string x = (string) selector.Items[i];
				if (StringUtils.ContainsAll(x, strings)){
					index = i;
					break;
				}
			}
			if (index != -1){
				selector.SelectedItems.Add(selector.Items[index]);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData){
			switch (keyData){
				case Keys.Control | Keys.A:
					Control c = GetChildAtPoint(Cursor.Position);
					if (c != null){
						if (c.Equals(allListBox)){
							SelectAll(allListBox);
						} else if (c.Equals(selectedListBox)){
							SelectAll(selectedListBox);
						}
					}
					Invalidate(true);
					break;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private static void SelectAll(ListBox p0){
			for (int i = 0; i < p0.Items.Count; i++){
				p0.SetSelected(i, true);
			}
		}

		public void SetDefaultSelectors(List<string> defaultSelectionNames1, List<string[]> defaultSelections1){
			if (defaultSelectionNames1.Count > 0){
				tableLayoutPanel2.RowStyles[1] = new RowStyle(SizeType.Absolute, 30F);
			}
			for (int i = 0; i < defaultSelectionNames1.Count; i++){
				Button b = new Button{
					Text = defaultSelectionNames1[i], Size = new Size(80, 23), Location = new Point(5 + i*83, 1),
					UseVisualStyleBackColor = true
				};
				defaultButtonPanel.Controls.Add(b);
				int i1 = i;
				b.Click += (sender, e) => SelectItems(defaultSelections1[i1]);
			}
		}

		private void SelectItems(string[] defaultSelection){
			selectedListBox.Items.Clear();
			selectedListBox.Items.AddRange(defaultSelection);
		}
	}
}