using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLibS.Drawing;
using BaseLibS.Graph;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms.Table{
	public partial class TableView : UserControl{
		public event EventHandler SelectionChanged;
		private readonly CompoundScrollableControl tableView;
		private readonly TableViewControlModel tableViewWf;
		private readonly TextBox auxTextBox;
		private SplitContainer splitContainer;
		private TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
		private ButtonModel textButton;
		private Label itemsLabel;
		private Label selectedLabel;
		private Panel mainPanel;
		private ComboBoxModel scaleFactorComboBox;
		public bool TextBoxIsVisible{ get; private set; }
		public  EventHandler<int> DoubleClickOnRow;

		public TableView(){
			InitializeComponent();
			InitializeComponent2();
			scaleFactorComboBox.SelectedIndex = 3;
			tableView = new CompoundScrollableControl{Dock = DockStyle.Fill, Margin = new Padding(0)};
			tableViewWf = new TableViewControlModel(this);
			tableView.Client = tableViewWf;
			tableViewWf.SelectionChanged += (sender, args) => {
				SelectionChanged?.Invoke(sender, args);
				SetCounts();
			};

			tableViewWf.DoubleClickOnRow += (sender, i) => DoubleClickOnRow?.Invoke(this, i);
			mainPanel.Controls.Add(tableView);
			textButton.Click += TextButton_OnClick;
			KeyDown += (sender, args) => tableView.Focus();
			auxTextBox = new TextBox{
				Dock = DockStyle.Fill, Padding = new Padding(0), Multiline = true, ReadOnly = true
			};
			scaleFactorComboBox.SelectedIndexChanged += (sender, args) => {
				switch (scaleFactorComboBox.SelectedIndex){
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

		public void SetCounts(){
			if (tableViewWf == null){
				return;
			}
			long c = tableViewWf.SelectedCount;
			long t = tableViewWf.RowCount;
			selectedLabel.Text = c > 0 && MultiSelect ? StringUtils.WithDecimalSeparators(c) + " " + Loc.Selected : "";
			itemsLabel.Text = "" + StringUtils.WithDecimalSeparators(t) + @" " + (t == 1 ? Loc.Item : Loc.Items);
		}

		private void InitializeComponent2(){
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel2 = new TableLayoutPanel();
			textButton = new ButtonModel();
			itemsLabel = new Label();
			selectedLabel = new Label();
			mainPanel = new Panel();
			scaleFactorComboBox = new ComboBoxModel();
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
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
			tableLayoutPanel1.Size = new Size(523, 538);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			tableLayoutPanel2.ColumnCount = 5;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
			tableLayoutPanel2.Controls.Add(BasicControl.CreateControl(textButton), 4, 0);
			tableLayoutPanel2.Controls.Add(itemsLabel, 0, 0);
			tableLayoutPanel2.Controls.Add(selectedLabel, 1, 0);
			tableLayoutPanel2.Controls.Add(BasicControl.CreateControl(scaleFactorComboBox), 3, 0);
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new Point(0, 518);
			tableLayoutPanel2.Margin = new Padding(0);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Size = new Size(523, 20);
			tableLayoutPanel2.TabIndex = 0;
			// 
			// textButton
			// 
			textButton.Margin = new Padding2(0);
			textButton.Text = @"↑";
			textButton.Font = new Font2("Microsoft Sans Serif", 7.1F);
			// 
			// itemsLabel
			// 
			itemsLabel.AutoSize = true;
			itemsLabel.Dock = DockStyle.Fill;
			itemsLabel.Location = new Point(3, 0);
			itemsLabel.Name = "itemsLabel";
			itemsLabel.Size = new Size(1, 20);
			itemsLabel.TabIndex = 2;
			itemsLabel.Font = new Font("Microsoft Sans Serif", 8.1F);
			// 
			// selectedLabel
			// 
			selectedLabel.AutoSize = true;
			selectedLabel.Dock = DockStyle.Fill;
			selectedLabel.Location = new Point(9, 0);
			selectedLabel.Name = "selectedLabel";
			selectedLabel.Size = new Size(1,  20);
			selectedLabel.TabIndex = 3;
			selectedLabel.Font = new Font("Microsoft Sans Serif", 8.1F);
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
			scaleFactorComboBox.Font = new Font2("Microsoft Sans Serif", 7.1f);
			scaleFactorComboBox.Values=new []{
				"25 %", "50 %", "70 %", "100 %", "150 %", "200 %", "400 %"
			};
			scaleFactorComboBox.Margin = new Padding2(0);
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

		/// <summary>
		/// Get the table model.
		/// Use <code>Dispatcher.Invoke(() => view.TableModel ... )</code> to access this property for a non-GUI thread
		/// </summary>
		public ITableModel TableModel{
			get => tableViewWf.TableModel;
			set{
				tableViewWf.TableModel = value;
				SetCounts();
			}
		}

		public void SwitchOnTextBox(){
			if (TextBoxIsVisible){
				return;
			}
			textButton.Text = @"↓";
			tableViewWf.SetCellText = SetAuxText;
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

		public void SwitchOffTextBox(){
			if (!TextBoxIsVisible){
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

		private static bool lineBreakAtSemicolon = true;
		public void SetAuxText(string text){
			if (lineBreakAtSemicolon && text.Contains(";")){
				text = text.Replace(";", "\r\n");
			}
			auxTextBox.Text = text;
		}

		public bool MultiSelect{
			get => tableViewWf.MultiSelect;
			set => tableViewWf.MultiSelect = value;
		}

		public bool Sortable{
			get => tableViewWf.Sortable;
			set => tableViewWf.Sortable = value;
		}

		public int RowCount => tableViewWf.RowCount;

		public int RowHeaderWidth{
			set => tableView.RowHeaderWidth = value;
			get => tableView.RowHeaderWidth;
		}

		public int ColumnHeaderHeight{
			set{
				tableViewWf.origColumnHeaderHeight = value;
				tableView.ColumnHeaderHeight = value;
			}
			get => tableView.ColumnHeaderHeight;
		}

		public int VisibleX{
			get => tableView.VisibleX;
			set => tableView.VisibleX = value;
		}

		public int VisibleY{
			get => tableView.VisibleY;
			set => tableView.VisibleY = value;
		}

		public void SetSelectedRow(int row){
			tableViewWf.SetSelectedRow(row);
		}

		public void SetSelectedRow(int row, bool add, bool fire){
			tableViewWf.SetSelectedRow(row, add, fire);
		}

		public bool HasSelectedRows(){
			return tableViewWf.HasSelectedRows();
		}

		public void SetSelectedRows(IList<int> rows){
			tableViewWf.SetSelectedRows(rows);
		}

		public void SetSelectedRows(IList<int> rows, bool add, bool fire){
			tableViewWf.SetSelectedRows(rows, add, fire);
		}

		public void SetSelectedRowAndMove(int row){
			tableViewWf.SetSelectedRowAndMove(row);
		}

		public void SetSelectedRowsAndMove(IList<int> rows){
			tableViewWf.SetSelectedRowsAndMove(rows);
		}

		public int[] GetSelectedRows(){
			return tableViewWf.GetSelectedRows();
		}

		public int GetSelectedRow(){
			return tableViewWf.GetSelectedRow();
		}

		public int[] GetSelectedAll(){
			return tableViewWf.GetSelectedAll();
		}

		public List<int> GetSelectedAllList(){
			return tableViewWf.GetSelectedAllList();
		}

		public string[] GetColumnNames(){
			return tableViewWf.GetColumnNames();
		}

		public StreamWriter ExportMatrixPolygon(DataTable2 model, StreamWriter writer){
			return tableViewWf.ExportMatrixPolygon(model, writer);
		}

		public int GetSelectedAll1(){
			return tableViewWf.GetSelectedAll1();
		}

		public void ScrollToRow(int row){
			tableViewWf.ScrollToRow(row);
		}

		public void BringSelectionToTop(){
			tableViewWf.BringSelectionToTop();
		}

		public void FireSelectionChange(){
			tableViewWf.FireSelectionChange();
		}

		public bool ModelRowIsSelected(int row){
			return tableViewWf.ModelRowIsSelected(row);
		}

		public void ClearSelection(){
			tableViewWf.ClearSelection();
		}

		public void SelectAll(){
			tableViewWf.SelectAll();
		}

		public void SetSelection(bool[] selection){
			tableViewWf.SetSelection(selection);
		}

		public void SetSelectedIndex(int index){
			tableViewWf.SetSelectedIndex(index);
		}

		public void SetSelectedViewIndex(int index){
			tableViewWf.SetSelectedViewIndex(index);
		}

		public void SetSelectedIndex(int index, object sender){
			tableViewWf.SetSelectedIndex(index, sender);
		}

		public object GetEntry(int row, int col){
			return tableViewWf.GetEntry(row, col);
		}

		private void TextButton_OnClick(object sender, EventArgs e){
			if (TextBoxIsVisible){
				SwitchOffTextBox();
			} else{
				SwitchOnTextBox();
			}
		}

		public void ClearSelectionFire(){
			tableViewWf.ClearSelectionFire();
		}

		private double[] GetTimeVals(int ind2){
			double[] result = new double[TableModel.RowCount];
			for (int i = 0; i < result.Length; i++){
				result[i] = (double) TableModel.GetEntry(i, ind2);
			}
			return result;
		}
	}
}