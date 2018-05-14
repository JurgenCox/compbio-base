using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Scroll;
using BaseLibS.Num;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms.Table {
	public partial class TableView : UserControl {
		internal static readonly List<ITableSelectionAgent> selectionAgents = new List<ITableSelectionAgent>();
		public event EventHandler SelectionChanged;
		private readonly CompoundScrollableControl tableView;
		private readonly TableViewControlModel tableViewWf;
		private bool hasSelectionAgent;
		private ITableSelectionAgent selectionAgent;
		private int selectionAgentColInd = -1;
		private double[] selectionAgentColVals;
		private readonly TextBox auxTextBox;
		private SplitContainer splitContainer;
		private TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
		private Button selectionAgentButton;
		private Button textButton;
		private Label itemsLabel;
		private Label selectedLabel;
		private Panel mainPanel;
		private ComboBox scaleFactorComboBox;
		public float sfx;
		public bool TextBoxIsVisible { get; private set; }

		public TableView() {
			InitializeComponent();
			sfx = FormUtils.GetDpiScale(CreateGraphics());
			InitializeComponent2();
			scaleFactorComboBox.SelectedIndex = 3;
			tableView = new CompoundScrollableControl {Dock = DockStyle.Fill, Margin = new Padding(0)};
			tableViewWf = new TableViewControlModel(this);
			tableView.Client = tableViewWf;
			tableViewWf.SelectionChanged += (sender, args) => {
				SelectionChanged?.Invoke(sender, args);
				SetCounts();
			};
			mainPanel.Controls.Add(tableView);
			textButton.Click += TextButton_OnClick;
			selectionAgentButton.Click += SelectionAgentButton_OnClick;
			KeyDown += (sender, args) => tableView.Focus();
			auxTextBox = new TextBox {Dock = DockStyle.Fill, Padding = new Padding(0), Multiline = true, ReadOnly = true};
			scaleFactorComboBox.SelectedIndexChanged += (sender, args) => {
				switch (scaleFactorComboBox.SelectedIndex) {
					case 0:
						tableViewWf.UserSf = 0.25f;
						break;
					case 1:
						tableViewWf.UserSf = 0.5f;
						break;
					case 2:
						tableViewWf.UserSf = 0.7f;
						break;
					case 3:
						tableViewWf.UserSf = 1f;
						break;
					case 4:
						tableViewWf.UserSf = 1.5f;
						break;
					case 5:
						tableViewWf.UserSf = 2f;
						break;
					case 6:
						tableViewWf.UserSf = 4f;
						break;
				}
				tableViewWf.UpdateScaling();
				tableView.Invalidate(true);
			};
		}

		public void SetCounts() {
			if (tableViewWf == null) {
				return;
			}
			long c = tableViewWf.SelectedCount;
			long t = tableViewWf.RowCount;
			selectedLabel.Text = c > 0 && MultiSelect ? StringUtils.WithDecimalSeparators(c) + " " + Loc.Selected : "";
			itemsLabel.Text = "" + StringUtils.WithDecimalSeparators(t) + @" " + (t == 1 ? Loc.Item : Loc.Items);
		}

		private void InitializeComponent2() {
			bool isUnix = FileUtils.IsUnix();
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel2 = new TableLayoutPanel();
			selectionAgentButton = new Button();
			textButton = new Button();
			itemsLabel = new Label();
			selectedLabel = new Label();
			mainPanel = new Panel();
			scaleFactorComboBox = new ComboBox();
			tableLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
			tableLayoutPanel1.Controls.Add(mainPanel, 0, 0);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F * sfx));
			tableLayoutPanel1.Size = new Size(523, 538);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			tableLayoutPanel2.ColumnCount = 6;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F * sfx));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F * sfx));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F * sfx));
			tableLayoutPanel2.Controls.Add(selectionAgentButton, 3, 0);
			tableLayoutPanel2.Controls.Add(textButton, 5, 0);
			tableLayoutPanel2.Controls.Add(itemsLabel, 0, 0);
			tableLayoutPanel2.Controls.Add(selectedLabel, 1, 0);
			tableLayoutPanel2.Controls.Add(scaleFactorComboBox, 4, 0);
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new Point(0, 518);
			tableLayoutPanel2.Margin = new Padding(0);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Size = new Size(523, 20);
			tableLayoutPanel2.TabIndex = 0;
			// 
			// selectionAgentButton
			// 
			selectionAgentButton.Dock = DockStyle.Fill;
			selectionAgentButton.Location = new Point(433, 0);
			selectionAgentButton.Margin = new Padding(0);
			selectionAgentButton.Name = "selectionAgentButton";
			selectionAgentButton.Size = new Size(20, 20);
			selectionAgentButton.TabIndex = 0;
			selectionAgentButton.UseVisualStyleBackColor = true;
			selectionAgentButton.Visible = false;
			// 
			// textButton
			// 
			textButton.Dock = DockStyle.Fill;
			textButton.Location = new Point(503, 0);
			textButton.Margin = new Padding(0);
			textButton.Name = "textButton";
			textButton.Text = @"↑";
			textButton.Font = new Font("Microsoft Sans Serif", 7.1F * sfx);
			textButton.Size = new Size(20, 20);
			textButton.TabIndex = 1;
			textButton.UseVisualStyleBackColor = true;
			// 
			// itemsLabel
			// 
			itemsLabel.AutoSize = true;
			itemsLabel.Dock = DockStyle.Fill;
			itemsLabel.Location = new Point(3, 0);
			itemsLabel.Name = "itemsLabel";
			itemsLabel.Size = new Size(1, 20);
			itemsLabel.TabIndex = 2;
			itemsLabel.Font = new Font("Microsoft Sans Serif", 8.1F * sfx);
			// 
			// selectedLabel
			// 
			selectedLabel.AutoSize = true;
			selectedLabel.Dock = DockStyle.Fill;
			selectedLabel.Location = new Point(9, 0);
			selectedLabel.Name = "selectedLabel";
			selectedLabel.Size = new Size(1, (int) (20 * sfx));
			selectedLabel.TabIndex = 3;
			selectedLabel.Font = new Font("Microsoft Sans Serif", 8.1F * sfx);
			// 
			// mainPanel
			// 
			mainPanel.Dock = DockStyle.Fill;
			mainPanel.Location = new Point(0, 0);
			mainPanel.Margin = new Padding(0);
			mainPanel.Name = "mainPanel";
			mainPanel.Size = new Size(523, 518);
			mainPanel.TabIndex = 1;
			// 
			// scaleFactorComboBox
			// 
			scaleFactorComboBox.Dock = DockStyle.Fill;
			scaleFactorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			scaleFactorComboBox.Font = new Font("Microsoft Sans Serif", (isUnix ? 4 : 7) * sfx);
			scaleFactorComboBox.FormattingEnabled = true;
			scaleFactorComboBox.Items.AddRange(new object[] {"25 %", "50 %", "70 %", "100 %", "150 %", "200 %", "400 %"});
			scaleFactorComboBox.Location = new Point(453, 0);
			scaleFactorComboBox.Margin = new Padding(0);
			scaleFactorComboBox.Name = "scaleFactorComboBox";
			scaleFactorComboBox.Size = new Size(60, 20);
			scaleFactorComboBox.TabIndex = 4;
			// 
			// TableView
			// 
			AutoScaleDimensions = new SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(tableLayoutPanel1);
			Name = "TableView";
			Size = new Size(523, 538);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel2.ResumeLayout(false);
			tableLayoutPanel2.PerformLayout();
			ResumeLayout(false);
		}

		public void SelectTime(double timeMs) {
			if (selectionAgentColInd < 0) {
				return;
			}
			int ind = ArrayUtils.ClosestIndex(selectionAgentColVals, timeMs);
			ClearSelection();
			SetSelectedIndex(ind);
		}

		public static void RegisterSelectionAgent(ITableSelectionAgent agent) {
			selectionAgents.Add(agent);
		}

		public static void UnregisterSelectionAgent(ITableSelectionAgent agent) {
			selectionAgents.Remove(agent);
		}

		public bool HasSelectionAgent {
			get => hasSelectionAgent;
			set {
				hasSelectionAgent = value;
				if (hasSelectionAgent && selectionAgents.Count > 0) {
					selectionAgentButton.Visible = true;
				}
			}
		}

		/// <summary>
		/// Get the table model.
		/// Use <code>Dispatcher.Invoke(() => view.TableModel ... )</code> to access this property for a non-GUI thread
		/// </summary>
		public ITableModel TableModel {
			get => tableViewWf.TableModel;
			set {
				tableViewWf.TableModel = value;
				SetCounts();
			}
		}

		public void SwitchOnTextBox() {
			if (TextBoxIsVisible) {
				return;
			}
			textButton.Text = @"↓";
			tableViewWf.SetCellText = s => auxTextBox.Text = s;
			mainPanel.Controls.Remove(tableView);
			splitContainer = new SplitContainer();
			splitContainer.Panel1.Controls.Add(tableView);
			splitContainer.Panel2.Controls.Add(auxTextBox);
			splitContainer.SplitterDistance = 90;
			splitContainer.Margin = new Padding(0);
			splitContainer.Dock = DockStyle.Fill;
			splitContainer.Orientation = Orientation.Horizontal;
			mainPanel.Controls.Add(splitContainer);
			TextBoxIsVisible = true;
		}

		public void SwitchOffTextBox() {
			if (!TextBoxIsVisible) {
				return;
			}
			textButton.Text = @"↑";
			auxTextBox.Text = "";
			tableViewWf.SetCellText = null;
			mainPanel.Controls.Remove(splitContainer);
			splitContainer.Panel1.Controls.Remove(tableView);
			splitContainer.Panel2.Controls.Remove(auxTextBox);
			splitContainer = null;
			mainPanel.Controls.Add(tableView);
			TextBoxIsVisible = false;
		}

		public void SetAuxText(string text) {
			auxTextBox.Text = text;
		}

		public bool MultiSelect {
			get => tableViewWf.MultiSelect;
			set => tableViewWf.MultiSelect = value;
		}

		public bool Sortable {
			get => tableViewWf.Sortable;
			set => tableViewWf.Sortable = value;
		}

		public int RowCount => tableViewWf.RowCount;

		public int RowHeaderWidth {
			set => tableView.RowHeaderWidth = value;
			get => tableView.RowHeaderWidth;
		}

		public int ColumnHeaderHeight {
			set {
				tableViewWf.origColumnHeaderHeight = value;
				tableView.ColumnHeaderHeight = value;
			}
			get => tableView.ColumnHeaderHeight;
		}

		public int VisibleX {
			get => tableView.VisibleX;
			set => tableView.VisibleX = value;
		}

		public int VisibleY {
			get => tableView.VisibleY;
			set => tableView.VisibleY = value;
		}

		public void SetSelectedRow(int row) {
			tableViewWf.SetSelectedRow(row);
		}

		public void SetSelectedRow(int row, bool add, bool fire) {
			tableViewWf.SetSelectedRow(row, add, fire);
		}

		public bool HasSelectedRows() {
			return tableViewWf.HasSelectedRows();
		}

		public void SetSelectedRows(IList<int> rows) {
			tableViewWf.SetSelectedRows(rows);
		}

		public void SetSelectedRows(IList<int> rows, bool add, bool fire) {
			tableViewWf.SetSelectedRows(rows, add, fire);
		}

		public void SetSelectedRowAndMove(int row) {
			tableViewWf.SetSelectedRowAndMove(row);
		}

		public void SetSelectedRowsAndMove(IList<int> rows) {
			tableViewWf.SetSelectedRowsAndMove(rows);
		}

		public int[] GetSelectedRows() {
			return tableViewWf.GetSelectedRows();
		}

		public int GetSelectedRow() {
			return tableViewWf.GetSelectedRow();
		}

		public void ScrollToRow(int row) {
			tableViewWf.ScrollToRow(row);
		}

		public void BringSelectionToTop() {
			tableViewWf.BringSelectionToTop();
		}

		public void FireSelectionChange() {
			tableViewWf.FireSelectionChange();
		}

		public bool ModelRowIsSelected(int row) {
			return tableViewWf.ModelRowIsSelected(row);
		}

		public void ClearSelection() {
			tableViewWf.ClearSelection();
		}

		public void SelectAll() {
			tableViewWf.SelectAll();
		}

		public void SetSelection(bool[] selection) {
			tableViewWf.SetSelection(selection);
		}

		public void SetSelectedIndex(int index) {
			tableViewWf.SetSelectedIndex(index);
		}

		public void SetSelectedViewIndex(int index) {
			tableViewWf.SetSelectedViewIndex(index);
		}

		public void SetSelectedIndex(int index, object sender) {
			tableViewWf.SetSelectedIndex(index, sender);
		}

		public object GetEntry(int row, int col) {
			return tableViewWf.GetEntry(row, col);
		}

		private void TextButton_OnClick(object sender, EventArgs e) {
			if (TextBoxIsVisible) {
				SwitchOffTextBox();
			} else {
				SwitchOnTextBox();
			}
		}

		public void ClearSelectionFire() {
			tableViewWf.ClearSelectionFire();
		}

		private void SelectionAgentButton_OnClick(object sender, EventArgs e) {
			Point p = selectionAgentButton.PointToScreen(new Point(0, 0));
			TableViewSelectionAgentForm w = new TableViewSelectionAgentForm(TableModel) {Top = p.Y - 125, Left = p.X - 300};
			if (w.ShowDialog() == DialogResult.OK) {
				int ind1 = w.sourceBox.SelectedIndex;
				int ind2 = w.columnBox.SelectedIndex;
				if (ind1 >= 0 && ind2 >= 0) {
					selectionAgent = selectionAgents[ind1];
					selectionAgentColInd = ind2;
					selectionAgentColVals = GetTimeVals(ind2);
					selectionAgent.AddTable(this);
				} else {
					selectionAgent = null;
					selectionAgentColInd = -1;
					selectionAgentColVals = null;
					selectionAgent.RemoveTable(this);
				}
			}
		}

		private double[] GetTimeVals(int ind2) {
			double[] result = new double[TableModel.RowCount];
			for (int i = 0; i < result.Length; i++) {
				result[i] = (double) TableModel.GetEntry(i, ind2);
			}
			return result;
		}
	}
}