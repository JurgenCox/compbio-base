using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BaseLibS.Graph;
using BaseLibS.Num;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms.Table {
	internal partial class FindForm : Form {
		private const int expandedHeight = 700;
		private readonly TableViewControlModel tableViewWf;
		private readonly ICompoundScrollableControl tableView;
		private readonly ITableModel tableModel;
		private long searchRowIndView = -1;
		private int[] multipleColumns = new int[0];
		private readonly TableViewControlModel searchResultsTableView;
		private StatusStrip statusStrip1;
		private TableLayoutPanel tableLayoutPanel1;
		private Panel panel1;
		private Button cancelButton;
		private CheckBox matchCaseCheckBox;
		private TextBox expressionTextBox;
		private Label label1;
		private Button findAllButton;
		private Button findNextButton;
		private Scroll.CompoundScrollableControl tableView1;
		private CheckBox matchWholeWordCheckBox;
		private ComboBox wildcardsComboBox;
		private ComboBox lookInComboBox;
		private Label label2;
		private CheckBox useCheckBox;
		private CheckBox searchUpCheckBox;
		private Button columnSelectButton;
		private Button helpButton;
		private ToolStripStatusLabel toolStripStatusLabel1;

		public FindForm(TableViewControlModel tableViewWf, ICompoundScrollableControl tableView) {
			InitializeComponent();
			InitializeComponent2();
			searchResultsTableView = new TableViewControlModel(null);
			tableView1.Client = searchResultsTableView;
			searchResultsTableView.origColumnHeaderHeight = 26;
			searchResultsTableView.HasHelp = true;
			searchResultsTableView.MultiSelect = true;
			searchResultsTableView.Sortable = true;
			searchResultsTableView.TableModel = null;
			this.tableViewWf = tableViewWf;
			this.tableView = tableView;
			tableModel = tableViewWf.TableModel;
			wildcardsComboBox.SelectedIndex = 0;
			wildcardsComboBox.Enabled = false;
			helpButton.Enabled = false;
			columnSelectButton.Enabled = false;
			lookInComboBox.Items.Add("Whole table");
			for (int i = 0; i < tableModel.ColumnCount; i++) {
				lookInComboBox.Items.Add(tableModel.GetColumnName(i));
			}
			lookInComboBox.Items.Add("Multiple columns");
			lookInComboBox.SelectedIndex = 0;
			lookInComboBox.SelectedIndexChanged += LookInComboBoxSelectedIndexChanged;
			useCheckBox.Visible = false;
			wildcardsComboBox.Visible = false;
			helpButton.Visible = false;
			searchResultsTableView.SelectionChanged += SearchResultsTableSelectionChanged;
			expressionTextBox.TextChanged += (sender, args) => { searchRowIndView = -1; };
			expressionTextBox.Focus();
			expressionTextBox.SelectAll();
		}

		private void InitializeComponent2() {
			components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FindForm));
			statusStrip1 = new StatusStrip();
			toolStripStatusLabel1 = new ToolStripStatusLabel();
			tableLayoutPanel1 = new TableLayoutPanel();
			panel1 = new Panel();
			columnSelectButton = new Button();
			helpButton = new Button();
			wildcardsComboBox = new ComboBox();
			lookInComboBox = new ComboBox();
			label2 = new Label();
			useCheckBox = new CheckBox();
			searchUpCheckBox = new CheckBox();
			matchWholeWordCheckBox = new CheckBox();
			findAllButton = new Button();
			findNextButton = new Button();
			cancelButton = new Button();
			matchCaseCheckBox = new CheckBox();
			expressionTextBox = new TextBox();
			label1 = new Label();
			tableView1 = new Scroll.CompoundScrollableControl();
			statusStrip1.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] {toolStripStatusLabel1});
			statusStrip1.Location = new Point(0, 196);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(336, 22);
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new Size(0, 17);
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(panel1, 0, 0);
			tableLayoutPanel1.Controls.Add(tableView1, 0, 1);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 196F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Size = new Size(336, 196);
			tableLayoutPanel1.TabIndex = 1;
			// 
			// panel1
			// 
			panel1.Controls.Add(columnSelectButton);
			panel1.Controls.Add(helpButton);
			panel1.Controls.Add(wildcardsComboBox);
			panel1.Controls.Add(lookInComboBox);
			panel1.Controls.Add(label2);
			panel1.Controls.Add(useCheckBox);
			panel1.Controls.Add(searchUpCheckBox);
			panel1.Controls.Add(matchWholeWordCheckBox);
			panel1.Controls.Add(findAllButton);
			panel1.Controls.Add(findNextButton);
			panel1.Controls.Add(cancelButton);
			panel1.Controls.Add(matchCaseCheckBox);
			panel1.Controls.Add(expressionTextBox);
			panel1.Controls.Add(label1);
			panel1.Dock = DockStyle.Fill;
			panel1.Location = new Point(3, 3);
			panel1.Name = "panel1";
			panel1.Size = new Size(330, 190);
			panel1.TabIndex = 0;
			// 
			// columnSelectButton
			// 
			columnSelectButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			columnSelectButton.Location = new Point(299, 36);
			columnSelectButton.Name = "columnSelectButton";
			columnSelectButton.Size = new Size(22, 21);
			columnSelectButton.TabIndex = 14;
			columnSelectButton.Text = ">";
			columnSelectButton.UseVisualStyleBackColor = true;
			columnSelectButton.Click += ColumnSelectButtonClick;
			// 
			// helpButton
			// 
			helpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			helpButton.Location = new Point(299, 6);
			helpButton.Name = "helpButton";
			helpButton.Size = new Size(22, 21);
			helpButton.TabIndex = 13;
			helpButton.Text = ">";
			helpButton.UseVisualStyleBackColor = true;
			// 
			// wildcardsComboBox
			// 
			wildcardsComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			wildcardsComboBox.FormattingEnabled = true;
			wildcardsComboBox.Items.AddRange(new object[] {"Regular expressions", "Wildcards"});
			wildcardsComboBox.Location = new Point(72, 134);
			wildcardsComboBox.Name = "wildcardsComboBox";
			wildcardsComboBox.Size = new Size(249, 21);
			wildcardsComboBox.TabIndex = 12;
			// 
			// lookInComboBox
			// 
			lookInComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			lookInComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			lookInComboBox.FormattingEnabled = true;
			lookInComboBox.Location = new Point(72, 36);
			lookInComboBox.Name = "lookInComboBox";
			lookInComboBox.Size = new Size(221, 21);
			lookInComboBox.TabIndex = 11;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(10, 39);
			label2.Name = "label2";
			label2.Size = new Size(45, 13);
			label2.TabIndex = 10;
			label2.Text = "Look in:";
			// 
			// useCheckBox
			// 
			useCheckBox.AutoSize = true;
			useCheckBox.Location = new Point(15, 136);
			useCheckBox.Name = "useCheckBox";
			useCheckBox.Size = new Size(45, 17);
			useCheckBox.TabIndex = 9;
			useCheckBox.Text = "Use";
			useCheckBox.UseVisualStyleBackColor = true;
			useCheckBox.CheckedChanged += UseCheckBoxCheckedChanged;
			// 
			// searchUpCheckBox
			// 
			searchUpCheckBox.AutoSize = true;
			searchUpCheckBox.Location = new Point(15, 113);
			searchUpCheckBox.Name = "searchUpCheckBox";
			searchUpCheckBox.Size = new Size(75, 17);
			searchUpCheckBox.TabIndex = 8;
			searchUpCheckBox.Text = "Search up";
			searchUpCheckBox.UseVisualStyleBackColor = true;
			// 
			// matchWholeWordCheckBox
			// 
			matchWholeWordCheckBox.AutoSize = true;
			matchWholeWordCheckBox.Location = new Point(15, 90);
			matchWholeWordCheckBox.Name = "matchWholeWordCheckBox";
			matchWholeWordCheckBox.Size = new Size(113, 17);
			matchWholeWordCheckBox.TabIndex = 7;
			matchWholeWordCheckBox.Text = "Match whole word";
			matchWholeWordCheckBox.UseVisualStyleBackColor = true;
			// 
			// findAllButton
			// 
			findAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			findAllButton.Location = new Point(165, 161);
			findAllButton.Name = "findAllButton";
			findAllButton.Size = new Size(75, 23);
			findAllButton.TabIndex = 5;
			findAllButton.Text = "Find all";
			findAllButton.UseVisualStyleBackColor = true;
			findAllButton.Click += FindAllButtonClick;
			// 
			// findNextButton
			// 
			findNextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			findNextButton.Location = new Point(84, 161);
			findNextButton.Name = "findNextButton";
			findNextButton.Size = new Size(75, 23);
			findNextButton.TabIndex = 4;
			findNextButton.Text = "Find next";
			findNextButton.UseVisualStyleBackColor = true;
			findNextButton.Click += FindNextButtonClick;
			// 
			// cancelButton
			// 
			cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Location = new Point(246, 161);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(75, 23);
			cancelButton.TabIndex = 3;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			cancelButton.Click += CancelButtonClick;
			// 
			// matchCaseCheckBox
			// 
			matchCaseCheckBox.AutoSize = true;
			matchCaseCheckBox.Location = new Point(15, 67);
			matchCaseCheckBox.Name = "matchCaseCheckBox";
			matchCaseCheckBox.Size = new Size(82, 17);
			matchCaseCheckBox.TabIndex = 2;
			matchCaseCheckBox.Text = "Match case";
			matchCaseCheckBox.UseVisualStyleBackColor = true;
			// 
			// expressionTextBox
			// 
			expressionTextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right;
			expressionTextBox.Location = new Point(72, 7);
			expressionTextBox.Name = "expressionTextBox";
			expressionTextBox.Size = new Size(221, 20);
			expressionTextBox.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(10, 10);
			label1.Name = "label1";
			label1.Size = new Size(56, 13);
			label1.TabIndex = 0;
			label1.Text = "Find what:";
			// 
			// tableView1
			// 
			tableView1.ColumnHeaderHeight = 26;
			tableView1.Dock = DockStyle.Fill;
			tableView1.Location = new Point(3, 199);
			tableView1.Name = "tableView1";
			tableView1.RowHeaderWidth = 70;
			tableView1.Size = new Size(330, 1);
			tableView1.TabIndex = 1;
			tableView1.VisibleX = 0;
			tableView1.VisibleY = 0;
			// 
			// FindForm
			// 
			AutoScaleDimensions = new SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = cancelButton;
			ClientSize = new Size(336, 218);
			Controls.Add(tableLayoutPanel1);
			Controls.Add(statusStrip1);
			Icon = ((Icon) (resources.GetObject("$this.Icon")));
			MaximizeBox = false;
			MinimizeBox = false;
			MinimumSize = new Size(276, 256);
			Name = "FindForm";
			Text = Loc.Find;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		protected override void OnLoad(EventArgs e) {
			expressionTextBox.Select();
		}

		protected override void OnClosing(CancelEventArgs e) {
			e.Cancel = true;
			Visible = false;
		}

		private void SearchResultsTableSelectionChanged(object sender, EventArgs e) {
			tableViewWf.ClearSelection();
			int[] rows = searchResultsTableView.GetSelectedRows();
			if (rows.Length > 0) {
				int row = rows[0];
				int ind = (int) searchResultsTableView.GetEntry(row, 0) - 1;
				string cname = (string) searchResultsTableView.GetEntry(row, 1);
				tableViewWf.SetSelectedViewIndex(ind);
				for (int i = 1; i < rows.Length; i++) {
					int row1 = rows[i];
					int ind1 = (int)searchResultsTableView.GetEntry(row1, 0) - 1;
					tableViewWf.SetSelectedViewIndex(ind1);
				}
				tableViewWf.ScrollToRow(ind);
				int colInd = tableModel.GetColumnIndex(cname);
				tableViewWf.ScrollToColumn(colInd);
				tableViewWf.tableView.SwitchOnTextBox();
				object entry = tableViewWf.GetEntry(ind, colInd);
				if (entry != null && !(entry is DBNull)) {
					tableViewWf.tableView.SetAuxText(entry.ToString());
				}
			}
			tableView.Invalidate(true);
		}

		private void LookInComboBoxSelectedIndexChanged(object sender, EventArgs e) {
			columnSelectButton.Enabled = lookInComboBox.SelectedIndex == tableModel.ColumnCount + 1;
		}

		private IEnumerable<int> GetColumnIndices() {
			int ind = lookInComboBox.SelectedIndex;
			if (ind == 0) {
				return ArrayUtils.ConsecutiveInts(0, tableModel.ColumnCount);
			}
			return ind <= tableModel.ColumnCount ? new[] {ind - 1} : multipleColumns;
		}

		private bool MatchCase => matchCaseCheckBox.Checked;
		private bool MatchWholeWord => matchWholeWordCheckBox.Checked;
		private bool SearchUp => searchUpCheckBox.Checked;
		private string SearchString => expressionTextBox.Text;

		private void UseCheckBoxCheckedChanged(object sender, EventArgs e) {
			wildcardsComboBox.Enabled = useCheckBox.Checked;
			helpButton.Enabled = useCheckBox.Checked;
		}

		private void FindAllButtonClick(object sender, EventArgs e) {
			toolStripStatusLabel1.Text = "";
			if (lookInComboBox.SelectedIndex > tableModel.ColumnCount) {
				if (multipleColumns.Length == 0) {
					MessageBox.Show("Please select columns.");
					return;
				}
			}
			bool matchCase = MatchCase;
			bool matchWholeWord = MatchWholeWord;
			string searchString = SearchString;
			if (string.IsNullOrEmpty(searchString)) {
				MessageBox.Show("Please enter a search string.");
				return;
			}
			IEnumerable<int> colInds = GetColumnIndices();
			if (!matchCase) {
				searchString = searchString.ToLower();
			}
			int[][] matchingCols;
			int[] searchInds = FindAll(matchCase, matchWholeWord, searchString, colInds, out matchingCols);
			if (searchInds.Length == 0) {
				toolStripStatusLabel1.Text = "Search string not found.";
			}
			Height = expandedHeight;
			searchResultsTableView.TableModel = CreateSearchResultsTable(searchInds, matchingCols);
			tableView1.VisibleY = 0;
			tableView1.Invalidate(true);
		}

		private ITableModel CreateSearchResultsTable(IList<int> searchInds, IList<int[]> matchingCols) {
			DataTable2 table = new DataTable2("Search results", "Search results");
			table.AddColumn("Row", 100, ColumnType.Integer, "");
			table.AddColumn("Column", 80, ColumnType.Text, "");
			for (int index = 0; index < searchInds.Count; index++) {
				int searchInd = searchInds[index];
				for (int i = 0; i < matchingCols[index].Length; i++) {
					string colName = tableModel.GetColumnName(matchingCols[index][i]);
					DataRow2 row = table.NewRow();
					row["Row"] = searchInd + 1;
					row["Column"] = colName;
					table.AddRow(row);
				}
			}
			return table;
		}

		private int[] FindAll(bool matchCase, bool matchWholeWord, string searchString, IEnumerable<int> colInds,
			out int[][] matchingCols) {
			List<int> result = new List<int>();
			List<int[]> matchingCols2 = new List<int[]>();
			for (int i = 0; i < tableModel.RowCount; i++) {
				int modelInd = tableViewWf.GetModelIndex(i);
				int[] matchingCols1;
				if (MatchRow(modelInd, colInds, matchCase, matchWholeWord, searchString, out matchingCols1)) {
					result.Add(i);
					matchingCols2.Add(matchingCols1);
				}
			}
			matchingCols = matchingCols2.ToArray();
			return result.ToArray();
		}

		private void FindNextButtonClick(object sender, EventArgs e) {
			if (lookInComboBox.SelectedIndex > tableModel.ColumnCount) {
				if (multipleColumns.Length == 0) {
					MessageBox.Show("Please select columns.");
					return;
				}
			}
			bool matchCase = MatchCase;
			bool matchWholeWord = MatchWholeWord;
			bool searchUp = SearchUp;
			string searchString = SearchString;
			if (string.IsNullOrEmpty(searchString)) {
				MessageBox.Show("Please enter a search string.");
				return;
			}
			IEnumerable<int> colInds = GetColumnIndices();
			if (!matchCase) {
				searchString = searchString.ToLower();
			}
			if (searchUp) {
				if (searchRowIndView < 0) {
					searchRowIndView = tableModel.RowCount;
				}
				FindUp(matchCase, matchWholeWord, searchString, colInds);
			} else {
				FindDown(matchCase, matchWholeWord, searchString, colInds);
			}
		}

		private void FindUp(bool matchCase, bool matchWholeWord, string searchString, IEnumerable<int> colInds) {
			toolStripStatusLabel1.Text = "";
			if (searchRowIndView == -1) {
				searchRowIndView = tableViewWf.RowCount;
			}
			searchRowIndView--;
			while (searchRowIndView >= 0) {
				int modelInd = tableViewWf.GetModelIndex((int) searchRowIndView);
				if (MatchRow(modelInd, colInds, matchCase, matchWholeWord, searchString, out int[] matchingCols)) {
					tableViewWf.ClearSelection();
					tableViewWf.SetSelectedViewIndex((int) searchRowIndView);
					tableViewWf.ScrollToRow((int) searchRowIndView);
					int colInd = matchingCols[0];
					tableViewWf.ScrollToColumn(colInd);
					tableViewWf.tableView.SwitchOnTextBox();
					object entry = tableViewWf.GetEntry(modelInd, colInd);
					if (entry != null && !(entry is DBNull)) {
						tableViewWf.tableView.SetAuxText(entry.ToString());
					}
					return;
				}
				searchRowIndView--;
			}
			toolStripStatusLabel1.Text = "Search string not found.";
			searchRowIndView = -1;
		}

		private void FindDown(bool matchCase, bool matchWholeWord, string searchString, IEnumerable<int> colInds) {
			toolStripStatusLabel1.Text = "";
			searchRowIndView++;
			while (searchRowIndView < tableModel.RowCount) {
				int modelInd = tableViewWf.GetModelIndex((int) searchRowIndView);
				if (MatchRow(modelInd, colInds, matchCase, matchWholeWord, searchString, out int[] matchingCols)) {
					tableViewWf.ClearSelection();
					tableViewWf.SetSelectedViewIndex((int) searchRowIndView);
					tableViewWf.ScrollToRow((int) searchRowIndView);
					int colInd = matchingCols[0];
					tableViewWf.ScrollToColumn(colInd);
					tableViewWf.tableView.SwitchOnTextBox();
					object entry = tableViewWf.GetEntry((int) searchRowIndView, colInd);
					if (entry != null && !(entry is DBNull)) {
						tableViewWf.tableView.SetAuxText(entry.ToString());
					}
					return;
				}
				searchRowIndView++;
			}
			toolStripStatusLabel1.Text = "Search string not found.";
			searchRowIndView = -1;
		}

		private bool MatchRow(int rowInd, IEnumerable<int> columnIndices, bool matchCase, bool matchWholeWord,
			string searchString, out int[] matchingCols) {
			List<int> matchingCols1 = new List<int>();
			foreach (int columnIndex in columnIndices) {
				object e = tableModel.GetEntry(rowInd, columnIndex);
				if (e == null) {
					continue;
				}
				string val = e.ToString();
				if (!matchCase) {
					val = val.ToLower();
				}
				if (MatchCell(val, matchWholeWord, searchString)) {
					matchingCols1.Add(columnIndex);
				}
			}
			matchingCols = matchingCols1.Count > 0 ? matchingCols1.ToArray() : null;
			return matchingCols1.Count > 0;
		}

		private static bool MatchCell(string val, bool matchWholeWord, string searchString) {
			if (!matchWholeWord) {
				return val.Contains(searchString);
			}
			val = StringUtils.ReduceWhitespace(val);
			string[] vals = val.Split(' ', ';');
			foreach (string s in vals) {
				if (s.Equals(searchString)) {
					return true;
				}
			}
			return false;
		}

		private void CancelButtonClick(object sender, EventArgs e) {
			Close();
		}

		private void ColumnSelectButtonClick(object sender, EventArgs e) {
			string[] names = new string[tableModel.ColumnCount];
			for (int i = 0; i < names.Length; i++) {
				names[i] = tableModel.GetColumnName(i);
			}
			SelectColumnsForm scf = new SelectColumnsForm(names, multipleColumns);
			scf.ShowDialog();
			multipleColumns = scf.SelectedIndices;
		}

		public void FocusInputField() {
			expressionTextBox.Focus();
			expressionTextBox.SelectAll();
		}
	}
}