using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BaseLibS.Num;
using BaseLibS.Param;

namespace BaseLib.Forms{
	public partial class SubSelectionControl : UserControl{
		private Thread downThread;
		private Thread upThread;
		private Func<string[], Parameters> parameterFuncs;
		internal readonly List<Parameters> parameters = new List<Parameters>();
		internal MultiListSelectorControl MultiListSelectorControl { get; set; }

		internal Func<string[], Parameters> ParameterFuncs{
			get => parameterFuncs;
			set{
				parameterButton.Visible = true;
				parameterFuncs = value;
			}
		}

		public SubSelectionControl(){
			InitializeComponent();
			topButton.Click += TopButtonClick;
			upButton.Click += UpButtonClick;
			downButton.Click += DownButtonClick;
			bottomButton.Click += BottomButtonClick;
			selectButton.Click += Select_OnClick;
			deselectButton.Click += Deselect_OnClick;
			parameterButton.Click += ParameterButtonClick;
			downButton.MouseDown += DownButtonMouseDown;
			downButton.MouseUp += DownButtonMouseUp;
			upButton.MouseDown += UpButtonMouseDown;
			upButton.MouseUp += UpButtonMouseUp;
		}

		public string Text1{
			set => titleLabel.Text = value;
			get => titleLabel.Text;
		}

		internal ListBox.ObjectCollection SelectedItems => listBox1.Items;

		internal string[] SelectedStrings{
			get{
				ListBox.ObjectCollection sel = SelectedItems;
				string[] result = new string[sel.Count];
				for (int i = 0; i < result.Length; i++){
					result[i] = sel[i].ToString();
				}
				return result;
			}
		}

		private static int[] GetSelectedIndices(ListBox box){
			int[] result = new int[box.SelectedItems.Count];
			for (int i = 0; i < result.Length; i++){
				result[i] = box.Items.IndexOf(box.SelectedItems[i]);
			}
			return result;
		}

		private void SetOrder(IList<int> order, IEnumerable<int> selection){
			object[] items = new object[listBox1.Items.Count];
			for (int i = 0; i < items.Length; i++){
				items[i] = listBox1.Items[i];
			}
			listBox1.Items.Clear();
			//listBox1.UnselectAll();
			items = ArrayUtils.SubArray(items, order);
			foreach (object item in items){
				listBox1.Items.Add(item);
			}
			foreach (int i in selection){
				listBox1.SelectedItems.Add(items[i]);
			}
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
				Invoke(new Action(() => DownButtonClick(null, null)));
				Thread.Sleep(150);
			}
			// ReSharper disable FunctionNeverReturns
		}

		// ReSharper restore FunctionNeverReturns
		private void WalkUp(){
			Thread.Sleep(400);
			while (true){
				Invoke(new Action(() => UpButtonClick(null, null)));
				Thread.Sleep(150);
			}
			// ReSharper disable FunctionNeverReturns
		}

		// ReSharper restore FunctionNeverReturns
		private void TopButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(listBox1);
			if (selectedIndices.Length == 0){
				return;
			}
			int n = listBox1.Items.Count;
			int[] unselectedIndices = ArrayUtils.Complement(selectedIndices, n);
			int[] order = ArrayUtils.Concat(selectedIndices, unselectedIndices);
			int[] selection = ArrayUtils.ConsecutiveInts(selectedIndices.Length);
			SetOrder(order, selection);
			parameters.Clear();
		}

		private void UpButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(listBox1);
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
			int n = listBox1.Items.Count;
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
			parameters.Clear();
		}

		private void DownButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(listBox1);
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
			int n = listBox1.Items.Count;
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
			parameters.Clear();
		}

		private void BottomButtonClick(object sender, EventArgs e){
			int[] selectedIndices = GetSelectedIndices(listBox1);
			if (selectedIndices.Length == 0){
				return;
			}
			int n = listBox1.Items.Count;
			int[] unselectedIndices = ArrayUtils.Complement(selectedIndices, n);
			int[] order = ArrayUtils.Concat(unselectedIndices, selectedIndices);
			int[] selection = ArrayUtils.ConsecutiveInts(n - selectedIndices.Length, n);
			SetOrder(order, selection);
			parameters.Clear();
		}

		private void ParameterButtonClick(object sender, EventArgs e){
			if (SelectedStrings.Length == 0){
				return;
			}
			Point p = parameterButton.PointToScreen(new Point(0, 0));
			FilterForm fw = new FilterForm(this){Top = p.Y, Left = p.X - 895, Text = "Filter " + Text.ToLower() + " columns"};
			fw.ShowDialog();
		}

		private void Select_OnClick(object sender, EventArgs e){
			parameters.Clear();
			object[] os = new object[MultiListSelectorControl.AllListBox.SelectedItems.Count];
			MultiListSelectorControl.AllListBox.SelectedItems.CopyTo(os, 0);
			foreach (object o in os){
				if (!listBox1.Items.Contains(o)){
					listBox1.Items.Add(o);
				}
			}
			foreach (object o in os){
				MultiListSelectorControl.AllListBox.Items.Remove(o);
			}
			MultiListSelectorControl.SelectionHasChanged(this, e);
		}

		private void Deselect_OnClick(object sender, EventArgs e){
			parameters.Clear();
			object[] os = new object[listBox1.SelectedItems.Count];
			listBox1.SelectedItems.CopyTo(os, 0);
			foreach (object o in os){
				listBox1.Items.Remove(o);
			}
			foreach (object o in os){
				MultiListSelectorControl.AllListBox.Items.Add(o);
			}
			MultiListSelectorControl.SelectionHasChanged(this, e);
		}

		public Parameters GetParameters(){
			return parameterFuncs(SelectedStrings);
		}
	}
}