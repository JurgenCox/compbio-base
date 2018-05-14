using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaseLibS.Num;
using BaseLibS.Param;

namespace BaseLib.Forms{
	public partial class MultiListSelectorControl : UserControl{
		public event EventHandler SelectionChanged;
		public IList<string> items;
		internal ListBox AllListBox { get; }
		private SubSelectionControl[] subSelection;
		private readonly TableLayoutPanel tableLayoutPanel1;

		public MultiListSelectorControl(){
			InitializeComponent();
			AllListBox = new ListBox();
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			tableLayoutPanel1.Controls.Add(AllListBox, 0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			Controls.Add(tableLayoutPanel1);
		}

		public void Init(IList<string> items1){
			items = items1;
			ClearSelection();
			AllListBox.Items.Clear();
			foreach (string s in items1){
				AllListBox.Items.Add(s);
			}
		}

		public int[][] SelectedIndices{
			get{
				int[][] result = new int[subSelection.Length][];
				for (int i = 0; i < result.Length; i++){
					result[i] = GetSelectedIndices(i);
				}
				return result;
			}
			set{
				ClearSelection();
				for (int i = 0; i < value.Length; i++){
					foreach (int x in value[i]){
						SetSelected(i, x, true);
					}
				}
			}
		}

		private void ClearSelection(){
			foreach (SubSelectionControl t in subSelection){
				foreach (object x in t.listBox1.Items){
					AllListBox.Items.Add(x);
				}
				t.listBox1.Items.Clear();
			}
		}

		public void Init(IList<string> items1, IList<string> selectorNames){
			Init(items1, selectorNames, new Func<string[], Parameters>[0]);
		}

		public void Init(IList<string> items1, IList<string> selectorNames, IList<Func<string[], Parameters>> subParams){
			items = items1;
			foreach (string s in items1){
				AllListBox.Items.Add(s);
			}
			AllListBox.Dock = DockStyle.Fill;
			AllListBox.Margin = new Padding(0);
			AllListBox.SelectionMode = SelectionMode.MultiExtended;
			int n = selectorNames.Count;
			TableLayoutPanel tableLayoutPanel2 = new TableLayoutPanel();
			subSelection = new SubSelectionControl[n];
			for (int i = 0; i < n; i++){
				subSelection[i] = new SubSelectionControl{
					MultiListSelectorControl = this,
					Text1 = selectorNames[i],
					Dock = DockStyle.Fill,
					Margin = new Padding(0)
				};
				if (subParams != null && i < subParams.Count && subParams[i] != null){
					subSelection[i].ParameterFuncs = subParams[i];
				}
			}
			tableLayoutPanel2.Margin = new Padding(0);
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			for (int i = 0; i < n; i++){
				tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
			}
			for (int i = 0; i < n; i++){
				tableLayoutPanel2.Controls.Add(subSelection[i], 0, i);
			}
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
		}

		internal void SelectionHasChanged(SubSelectionControl sender, EventArgs e){
			SelectionChanged?.Invoke(this, e);
		}

		private HashSet<string> GetSubSelection(int selectorInd){
			return new HashSet<string>(subSelection[selectorInd].SelectedStrings);
		}

		public void SetSelected(int selectorInd, int itemInd, bool b){
			HashSet<string> x = GetSubSelection(selectorInd);
			if (x.Contains(items[itemInd])){
				return;
			}
			if (!AllListBox.Items.Contains(items[itemInd])){
				for (int i = 0; i < subSelection.Length; i++){
					if (i == selectorInd){
						continue;
					}
					if (subSelection[i].listBox1.Items.Contains(items[itemInd])){
						subSelection[i].listBox1.Items.Remove(items[itemInd]);
						AllListBox.Items.Add(items[itemInd]);
						break;
					}
				}
			}
			AllListBox.Items.Remove(items[itemInd]);
			subSelection[selectorInd].listBox1.Items.Add(items[itemInd]);
		}

		public int[] GetSelectedIndices(int selectorInd){
			string[] sel = subSelection[selectorInd].SelectedStrings;
			int[] result = new int[sel.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = ArrayUtils.IndexOf(items, sel[i]);
			}
			return result;
		}

		public IList<Parameters[]> GetSubParameterValues(){
			Parameters[][] result = new Parameters[subSelection.Length][];
			for (int i = 0; i < result.Length; i++){
				if (subSelection[i].ParameterFuncs == null){
					continue;
				}
				result[i] = subSelection[i].parameters.ToArray();
			}
			return result;
		}
	}
}