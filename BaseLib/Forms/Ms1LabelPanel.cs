using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Num;

namespace BaseLib.Forms{
	public partial class Ms1LabelPanel : UserControl{
		/// <summary>
		/// Number of label states. Set by input argument to constructor.
		/// </summary>
		private readonly int n;

		/// <summary>
		/// Set in constructor based on the list of labels given as an argument.
		/// </summary>
		private Dictionary<int, int[]> deselectionMap;

		/// <summary>
		/// One CheckedListBoxControl object for each label state, 
		/// each containing the full list of labels given as an argument to the constructor.
		/// Set by InitializeComponent1(), called by constructor.
		/// </summary>
		private CheckedListControl[] labelsListBoxes;

		/// <summary>
		/// Display names of the label states, e.g., "Light labels" and "Heavy labels". 
		/// Set by InitializeComponent1(), called by constructor, depending on number of label states.
		/// </summary>
		private Label[] textLabels;

		/// <summary>
		/// List of potential biochemical labels, e.g. Arg6 or Lys8. 
		/// Set by input argument to constructor.
		/// </summary>
		private readonly string[] labels;

		public Ms1LabelPanel(int n, string[] labels){
			InitializeComponent();
			this.n = n;
			this.labels = labels;
			InitializeComponent1();
			LabelModification[] x = ToLabelMods(labels);
			deselectionMap = CreateDeselectionMap(x);
			foreach (CheckedListControl box in labelsListBoxes.Where(box => box != null)){
				box.ItemCheck += LabelsListBoxItemCheck;
			}
		}

		/// <summary>
		/// Creates list of LabelModification objects based on a list of names.
		/// </summary>
		/// <param name="names"></param>
		/// <returns></returns>
		private static LabelModification[] ToLabelMods(IList<string> names){
			LabelModification[] result = new LabelModification[names.Count];
			for (int i = 0; i < result.Length; i++){
				result[i] = LabelModification.GetLabelByName(names[i]);
			}
			return result;
		}

		/// <summary>
		/// Initializes an Ms1LabelPanel instance. Called only by constructor.
		/// Sets textLabels and labelsListBoxes.
		/// </summary>
		private void InitializeComponent1(){
			labelsListBoxes = new CheckedListControl[n];
			textLabels = new Label[n];
			for (int i = 0; i < n; i++){
				labelsListBoxes[i] = new CheckedListControl();
				labelsListBoxes[i].AddRange(labels);
				textLabels[i] = new Label{Text = GetLabelText(i, n)};
			}
			// grid1 has one row to show "Light labels", "Medium labels", etc., and one row for the boxes 
			// to select the relevant labels. It has one column for each label state.
			TableLayoutPanel grid1 = new TableLayoutPanel();
			grid1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
			grid1.RowStyles.Add(new RowStyle(SizeType.Absolute, 126));
			for (int i = 0; i < n; i++){
				grid1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
			}
			for (int i = 0; i < n; i++){
				grid1.Controls.Add(textLabels[i], i, 0);
				grid1.Controls.Add(labelsListBoxes[i], i, 1);
			}
			grid1.AutoScroll = true;
			grid1.VerticalScroll.Enabled = false;
			grid1.Dock = DockStyle.Top;
			grid1.Width = 540;
			grid1.Height = 300;
			Controls.Add(grid1);
			Name = "Ms1LabelPanel";
			Width = 540;
			Height = 150;
			ResumeLayout(true);
		}

		/// <summary>
		/// Returns a text to use in the panel, depending on the index of the label state 
		/// and the total number of label states. Called only from InitializeComponent1.
		/// </summary>
		/// <param name="i">Index of label state (e.g., 0 for light, 1 for medium, etc.)
		/// Should be between 0 and m-1.</param>
		/// <param name="m">Number of label states. Should be greater than 0.</param>
		/// <returns></returns>
		private static string GetLabelText(int i, int m){
			if (i >= m){ // actually an error state which should be caught, as should i<0 and m<=1
				return "";
			}
			if (m == 1){ // only one label state
				return "";
			}
			if (m == 2){ // two lable states: light and heavy
				return i == 0 ? "Light labels" : "Heavy labels";
			}
			if (m == 3){ // three lable states: light, medium, and heavy
				switch (i){
					case 0:
						return "Light labels";
					case 1:
						return "Medium labels";
					case 2:
						return "Heavy labels";
				}
			}
			// For more than 3 label states, number them.
			return "Labels-" + i;
		}

		public string[] GetLabels(int ind){
			return LabelsFromBox(labelsListBoxes[ind]);
		}

		private static string[] LabelsFromBox(CheckedListControl box){
			List<string> result = new List<string>();
			foreach (string x in box.CheckedItems){
				LabelModification sl = LabelModification.GetLabelByName(x);
				if (sl == null){
					throw new Exception("Label " + x + " does not exist.");
				}
				result.Add(sl.Name);
			}
			return result.ToArray();
		}

		public void SetLabels(LabelModification[] sls){
			deselectionMap = CreateDeselectionMap(sls);
			foreach (CheckedListControl box in labelsListBoxes){
				box.AddRange(ToS(sls));
			}
		}

		private static string[] ToS(LabelModification[] sls) {
			string[] result = new string[sls.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = sls[i].Name;
			}
			return result;
		}

		private static Dictionary<int, int[]> CreateDeselectionMap(IList<LabelModification> sls){
			Dictionary<char, List<int>> aa2Inds = new Dictionary<char, List<int>>();
			for (int i = 0; i < sls.Count; i++){
				LabelModification sl = sls[i];
				if (sl is AminoAcidLabel){
					char aa = ((AminoAcidLabel) sl).Aa;
					if (!aa2Inds.ContainsKey(aa)){
						aa2Inds.Add(aa, new List<int>());
					}
					aa2Inds[aa].Add(i);
				} else{
					bool nt = ((TerminalLabel) sl).IsNterm;
					char aa = nt ? '<' : '>';
					if (!aa2Inds.ContainsKey(aa)){
						aa2Inds.Add(aa, new List<int>());
					}
					aa2Inds[aa].Add(i);
				}
			}
			Dictionary<int, int[]> result = new Dictionary<int, int[]>();
			foreach (List<int> w in aa2Inds.Values){
				int n = w.Count;
				if (n < 2){
					continue;
				}
				int[] q = w.ToArray();
				for (int i = 0; i < n; i++){
					int b = q[i];
					int[] r = ArrayUtils.RemoveAtIndex(q, i);
					result.Add(b, r);
				}
			}
			return result;
		}

		public int[][] SelectedIndices{
			get{
				int[][] result = new int[n][];
				for (int i = 0; i < n; i++){
					result[i] = ToInt(labelsListBoxes[i].CheckedIndices);
				}
				return result;
			}
			set{
				for (int i = 0; i < n; i++){
					if (labelsListBoxes[i] == null){
						continue;
					}
					for (int j = 0; j < labelsListBoxes[i].Count; j++){
						labelsListBoxes[i].SetItemChecked(j, ArrayUtils.Contains(value[i], j));
					}
				}
			}
		}

		private static int[] ToInt(ICollection selectedIndices){
			int[] result = new int[selectedIndices.Count];
			int c = 0;
			foreach (int x in selectedIndices){
				result[c++] = x;
			}
			return result;
		}

		private void LabelsListBoxItemCheck(object sender, ItemCheckEventArgs e){
			CheckedListControl box = (CheckedListControl) sender;
			if (e.NewValue != CheckState.Checked){
				return;
			}
			if (deselectionMap.ContainsKey(e.Index)){
				foreach (int i in deselectionMap[e.Index]){
					box.SetItemChecked(i, false);
				}
			}
		}
	}
}