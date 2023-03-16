using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public partial class IsobaricLabelsParamControl : UserControl{
		private readonly IsobaricLabelingDefault[] defaults = {
			new IsobaricLabelingDefault("4plex iTRAQ",
				new[]{"iTRAQ4plex-Lys114", "iTRAQ4plex-Lys115", "iTRAQ4plex-Lys116", "iTRAQ4plex-Lys117"},
				new[]{"iTRAQ4plex-Nter114", "iTRAQ4plex-Nter115", "iTRAQ4plex-Nter116", "iTRAQ4plex-Nter117"}, 
				false),
			new IsobaricLabelingDefault("8plex iTRAQ",
				new[]{
					"iTRAQ8plex-Lys113", "iTRAQ8plex-Lys114", "iTRAQ8plex-Lys115", "iTRAQ8plex-Lys116",
					"iTRAQ8plex-Lys117", "iTRAQ8plex-Lys118", "iTRAQ8plex-Lys119", "iTRAQ8plex-Lys121"
				},
				new[]{
					"iTRAQ8plex-Nter113", "iTRAQ8plex-Nter114", "iTRAQ8plex-Nter115", "iTRAQ8plex-Nter116",
					"iTRAQ8plex-Nter117", "iTRAQ8plex-Nter118", "iTRAQ8plex-Nter119", "iTRAQ8plex-Nter121"
				}, false),
			new IsobaricLabelingDefault("2plex TMT", new[]{"TMT2plex-Lys126", "TMT2plex-Lys127"},
				new[]{"TMT2plex-Nter126", "TMT2plex-Nter127"}, false),
			new IsobaricLabelingDefault("6plex TMT",
				new[]{
					"TMT6plex-Lys126", "TMT6plex-Lys127", "TMT6plex-Lys128", "TMT6plex-Lys129", "TMT6plex-Lys130",
					"TMT6plex-Lys131"
				},
				new[]{
					"TMT6plex-Nter126", "TMT6plex-Nter127", "TMT6plex-Nter128", "TMT6plex-Nter129", "TMT6plex-Nter130",
					"TMT6plex-Nter131"
				}, false),
			new IsobaricLabelingDefault("8plex TMT",
				new[]{
					"TMT8plex-Lys126C", "TMT8plex-Lys127N", "TMT8plex-Lys127C", "TMT8plex-Lys128C", "TMT8plex-Lys129N",
					"TMT8plex-Lys129C", "TMT8plex-Lys130C", "TMT8plex-Lys131N"
				},
				new[]{
					"TMT8plex-Nter126C", "TMT8plex-Nter127N", "TMT8plex-Nter127C", "TMT8plex-Nter128C",
					"TMT8plex-Nter129N", "TMT8plex-Nter129C", "TMT8plex-Nter130C", "TMT8plex-Nter131N"
				}, false),
			new IsobaricLabelingDefault("10plex TMT",
				new[]{
					"TMT10plex-Lys126C", "TMT10plex-Lys127N", "TMT10plex-Lys127C", "TMT10plex-Lys128N",
					"TMT10plex-Lys128C", "TMT10plex-Lys129N", "TMT10plex-Lys129C", "TMT10plex-Lys130N",
					"TMT10plex-Lys130C", "TMT10plex-Lys131N"
				},
				new[]{
					"TMT10plex-Nter126C", "TMT10plex-Nter127N", "TMT10plex-Nter127C", "TMT10plex-Nter128N",
					"TMT10plex-Nter128C", "TMT10plex-Nter129N", "TMT10plex-Nter129C", "TMT10plex-Nter130N",
					"TMT10plex-Nter130C", "TMT10plex-Nter131N"
				}, false),
			new IsobaricLabelingDefault("11plex TMT",
				new[]{
					"TMT10plex-Lys126C", "TMT10plex-Lys127N", "TMT10plex-Lys127C", "TMT10plex-Lys128N",
					"TMT10plex-Lys128C", "TMT10plex-Lys129N", "TMT10plex-Lys129C", "TMT10plex-Lys130N",
					"TMT10plex-Lys130C", "TMT10plex-Lys131N", "TMT11plex-Lys131C"
				},
				new[]{
					"TMT10plex-Nter126C", "TMT10plex-Nter127N", "TMT10plex-Nter127C", "TMT10plex-Nter128N",
					"TMT10plex-Nter128C", "TMT10plex-Nter129N", "TMT10plex-Nter129C", "TMT10plex-Nter130N",
					"TMT10plex-Nter130C", "TMT10plex-Nter131N", "TMT11plex-Nter131C"
				}, false),
			new IsobaricLabelingDefault("16plex TMTpro",
				new[]{
					"TMTpro16plex-Lys126C", "TMTpro16plex-Lys127N", "TMTpro16plex-Lys127C", "TMTpro16plex-Lys128N",
					"TMTpro16plex-Lys128C", "TMTpro16plex-Lys129N", "TMTpro16plex-Lys129C", "TMTpro16plex-Lys130N",
					"TMTpro16plex-Lys130C", "TMTpro16plex-Lys131N", "TMTpro16plex-Lys131C", "TMTpro16plex-Lys132N",
					"TMTpro16plex-Lys132C", "TMTpro16plex-Lys133N", "TMTpro16plex-Lys133C", "TMTpro16plex-Lys134N"
				},
				new[]{
					"TMTpro16plex-Nter126C", "TMTpro16plex-Nter127N", "TMTpro16plex-Nter127C", "TMTpro16plex-Nter128N",
					"TMTpro16plex-Nter128C", "TMTpro16plex-Nter129N", "TMTpro16plex-Nter129C", "TMTpro16plex-Nter130N",
					"TMTpro16plex-Nter130C", "TMTpro16plex-Nter131N", "TMTpro16plex-Nter131C", "TMTpro16plex-Nter132N",
					"TMTpro16plex-Nter132C", "TMTpro16plex-Nter133N", "TMTpro16plex-Nter133C", "TMTpro16plex-Nter134N"
				}, true),
						new IsobaricLabelingDefault("18plex TMTpro",
				new[]{
					"TMTpro16plex-Lys126C", "TMTpro16plex-Lys127N", "TMTpro16plex-Lys127C", "TMTpro16plex-Lys128N",
					"TMTpro16plex-Lys128C", "TMTpro16plex-Lys129N", "TMTpro16plex-Lys129C", "TMTpro16plex-Lys130N",
					"TMTpro16plex-Lys130C", "TMTpro16plex-Lys131N", "TMTpro16plex-Lys131C", "TMTpro16plex-Lys132N",
					"TMTpro16plex-Lys132C", "TMTpro16plex-Lys133N", "TMTpro16plex-Lys133C", "TMTpro16plex-Lys134N",
					"TMTpro18plex-Lys134C", "TMTpro18plex-Lys135N"
				},
				new[]{
					"TMTpro16plex-Nter126C", "TMTpro16plex-Nter127N", "TMTpro16plex-Nter127C", "TMTpro16plex-Nter128N",
					"TMTpro16plex-Nter128C", "TMTpro16plex-Nter129N", "TMTpro16plex-Nter129C", "TMTpro16plex-Nter130N",
					"TMTpro16plex-Nter130C", "TMTpro16plex-Nter131N", "TMTpro16plex-Nter131C", "TMTpro16plex-Nter132N",
					"TMTpro16plex-Nter132C", "TMTpro16plex-Nter133N", "TMTpro16plex-Nter133C", "TMTpro16plex-Nter134N",
                    "TMTpro18plex-Nter134C", "TMTpro18plex-Nter135N"
                }, true),
			new IsobaricLabelingDefault("iodo6plexTMT",
				new[]{
					"iodoTMT6plex-Cys126", "iodoTMT6plex-Cys127", "iodoTMT6plex-Cys128", "iodoTMT6plex-Cys129",
					"iodoTMT6plex-Cys130", "iodoTMT6plex-Cys131"
				}, new string[]{ }, false),
		};
		private TableLayoutPanel tableLayoutPanel1;
		private Table.TableView tableViewSimple;
		private Table.TableView tableViewComplex;
		private DataTable2 tableSimple;
		private DataTable2 tableComplex;
		public bool ComplexTableIsVisible => false;

		public IsobaricLabelsParamControl(){
			InitializeComponent();
			InitializeComponent1();
			InitializeComponent2();
		}

		private static readonly string[] headerSimple = {
			"Internal label", "Terminal label", "CF -2 [%]", "CF -1 [%]", "CF +1 [%]", "CF +2 [%]", "TMT-like"
		};

		private static readonly string[] headerComplex = {
			"Internal label", "Terminal label", "CF -2x13C [%]", "CF -13C-15N [%]", "CF -13C [%]", "CF -15N [%]", 
			"CF +15N [%]", "CF +13C [%]", "CF +15N+13C [%]", "CF +2x13C [%]", "TMT-like"
		};

		private void AddButtonOnClick(object sender, EventArgs eventArgs){
			if (ComplexTableIsVisible){
				DataRow2 row = tableComplex.NewRow();
				row[headerComplex[0]] = "";
				row[headerComplex[1]] = "";
				row[headerComplex[2]] = 0d;
				row[headerComplex[3]] = 0d;
				row[headerComplex[4]] = 0d;
				row[headerComplex[5]] = 0d;
				row[headerComplex[6]] = 0d;
				row[headerComplex[7]] = 0d;
				row[headerComplex[8]] = 0d;
				row[headerComplex[9]] = 0d;
				row[headerComplex[10]] = true;
				tableComplex.AddRow(row);
				tableViewComplex.Invalidate(true);
			} else {
				DataRow2 row = tableSimple.NewRow();
				row[headerSimple[0]] = "";
				row[headerSimple[1]] = "";
				row[headerSimple[2]] = 0d;
				row[headerSimple[3]] = 0d;
				row[headerSimple[4]] = 0d;
				row[headerSimple[5]] = 0d;
				row[headerSimple[6]] = true;
				tableSimple.AddRow(row);
				tableViewSimple.Invalidate(true);
			}
		}

		private void RemoveButtonOnClick(object sender, EventArgs eventArgs){
			if (ComplexTableIsVisible){
				int[] sel = tableViewComplex.GetSelectedRows();
				if (sel.Length == 0) {
					MessageBox.Show(Loc.PleaseSelectSomeRows);
					return;
				}
				tableComplex.RemoveRows(sel);
				tableViewComplex.Invalidate(true);
			} else {
				int[] sel = tableViewSimple.GetSelectedRows();
				if (sel.Length == 0) {
					MessageBox.Show(Loc.PleaseSelectSomeRows);
					return;
				}
				tableSimple.RemoveRows(sel);
				tableViewSimple.Invalidate(true);
			}
		}

		private void ImportButtonOnClick(object sender, EventArgs eventArgs){
			OpenFileDialog ofd = new OpenFileDialog{
				Multiselect = false,
				Title = @"Open a isobaric label tab-separated file",
				FileName = @"Select a isobaric label tab-separated file",
				Filter = @"Text file (*.txt)|*.txt",
			};
			if (ofd.ShowDialog() == DialogResult.OK){
				ImportLabelFile(ofd.FileName);
			}
		}

		private void ImportLabelFile(string fileName){
			using (StreamReader sr = new StreamReader(fileName)){
				string line = sr.ReadLine();
				if (string.IsNullOrEmpty(line)) return;
				string[] hh = line.Split('\t');
				if (hh.Length != headerSimple.Length && hh.Length != headerComplex.Length) {
					return;
				}
				bool isSimple = hh.Length == headerSimple.Length;
				if (isSimple){
					for (int i = 0; i < headerSimple.Length; i++) {
						if (!hh[i].Equals(headerSimple[i]) ) {
							return;
						}
					}
				} else {
					for (int i = 0; i < headerComplex.Length; i++) {
						if (!hh[i].Equals(headerComplex[i])) {
							return;
						}
					}
				}
				List<string[]> buf = new List<string[]>();
				while (!string.IsNullOrEmpty(line = sr.ReadLine())){
					string[] ll = line.Split('\t');
					if ((isSimple && ll.Length != headerSimple.Length) || (!isSimple && ll.Length != headerComplex.Length)) {
						return;
					}
					buf.Add(ll);
				}
				if (buf.Count == 0){
					return;
				}
				if (isSimple == ComplexTableIsVisible){
					FlipTable();
				}
				ClearVisibleTable();
				foreach (string[] b in buf){
					AddLabel(b);
				}
			}
			InvalidateVisibleTable();
		}
		private void ClearVisibleTable() {
			if (ComplexTableIsVisible) {
				tableComplex.Clear();
			} else {
				tableSimple.Clear();
			}
		}
		private void InvalidateVisibleTable() {
			if (ComplexTableIsVisible) {
				tableViewComplex.Invalidate(true);
			} else {
				tableViewSimple.Invalidate(true);
			}
		}
		private void FlipTable(){
			//TODO
		}
		private void ExportButtonOnClick(object sender, EventArgs eventArgs) {
			SaveFileDialog sfd = new SaveFileDialog {
				Title = @"Save a isobaric label tab-separated file", Filter = @"Text file (*.txt)|*.txt"
			};
			if (sfd.ShowDialog() == DialogResult.OK) {
				if (File.Exists(sfd.FileName)){
					File.Delete(sfd.FileName);
				}
				ExportLabelFile(sfd.FileName);
			}
		}

		private void TableTypeButtonOnClick(object sender, EventArgs eventArgs) {
		}

		private void ExportLabelFile(string fileName){
			string[][] value = Value;
			using (StreamWriter sw = new StreamWriter(fileName)){
				sw.WriteLine(string.Join("\t", headerSimple));
				foreach (string[] t in value){
					sw.WriteLine(string.Join("\t", t));
				}
			}
		}

		private void EditButtonOnClick(object sender, EventArgs eventArgs) {
			if (ComplexTableIsVisible){
				EditComplex();
			} else {
				EditSimple();
			}
		}

		private void EditSimple() {
			int[] sel = tableViewSimple.GetSelectedRows();
			if (sel.Length != 1) {
				MessageBox.Show("Please select exactly one row.");
				return;
			}
			DataRow2 row = tableSimple.GetRow(sel[0]);
			IsobaricLabelsSimpleEditForm f = new IsobaricLabelsSimpleEditForm(new IsobaricLabelInfoSimple((string)row[0],
				(string)row[1], (double)row[2], (double)row[3], (double)row[4], (double)row[5], (bool)row[6]));
			f.ShowDialog();
			if (f.DialogResult != DialogResult.OK) {
				return;
			}
			IsobaricLabelInfoSimple info = f.Info;
			row[0] = info.internalLabel;
			row[1] = info.terminalLabel;
			row[2] = info.correctionFactorM2;
			row[3] = info.correctionFactorM1;
			row[4] = info.correctionFactorP1;
			row[5] = info.correctionFactorP2;
			row[6] = info.tmtLike;
			tableViewSimple.Invalidate(true);
		}

		private void EditComplex() {
			int[] sel = tableViewComplex.GetSelectedRows();
			if (sel.Length != 1) {
				MessageBox.Show("Please select exactly one row.");
				return;
			}
			DataRow2 row = tableComplex.GetRow(sel[0]);
			IsobaricLabelsComplexEditForm f = new IsobaricLabelsComplexEditForm(
				new IsobaricLabelInfoComplex((string)row[0], (string)row[1], (double)row[2], 
					(double)row[3], (double)row[4], (double)row[5],
					(double)row[6], (double)row[7], (double)row[8], 
					(double)row[9], (bool)row[10]));
			f.ShowDialog();
			if (f.DialogResult != DialogResult.OK) {
				return;
			}
			IsobaricLabelInfoSimple info = f.Info;
			row[0] = info.internalLabel;
			row[1] = info.terminalLabel;
			row[2] = info.correctionFactorM2;
			row[3] = info.correctionFactorM1;
			row[4] = info.correctionFactorP1;
			row[5] = info.correctionFactorP2;
			row[6] = info.tmtLike;
			tableViewSimple.Invalidate(true);
		}

		private void InitializeComponent1() {
			tableLayoutPanel1 = new TableLayoutPanel();
			tableViewSimple = new Table.TableView();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(tableViewSimple, 0, 2);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 3;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Size = new System.Drawing.Size(801, 326);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// tableViewSimple
			// 
			tableViewSimple.ColumnHeaderHeight = 26;
			tableViewSimple.Dock = DockStyle.Fill;
			tableViewSimple.Location = new System.Drawing.Point(0, 42);
			tableViewSimple.Margin = new Padding(0);
			tableViewSimple.MultiSelect = true;
			tableViewSimple.Name = "tableViewSimple";
			tableViewSimple.RowHeaderWidth = 70;
			tableViewSimple.Size = new System.Drawing.Size(801, 284);
			tableViewSimple.Sortable = true;
			tableViewSimple.TabIndex = 0;
			tableViewSimple.TableModel = null;
			// 
			// IsobaricLabelsParamControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(tableLayoutPanel1);
			Margin = new Padding(1, 1, 1, 1);
			Name = "IsobaricLabelsParamControl";
			Size = new System.Drawing.Size(801, 326);
			tableLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);

		}

		private void InitializeComponent2(){
			TableLayoutPanel tableLayoutPanel2 = new TableLayoutPanel();
			TableLayoutPanel tableLayoutPanel3 = new TableLayoutPanel();
			Button addButton = new Button();
			Button removeButton = new Button();
			Button editButton = new Button();
			Button importButton = new Button();
			Button exportButton = new Button();
			Button tableTypeButton = new Button();
			tableLayoutPanel2.SuspendLayout();
			tableLayoutPanel3.SuspendLayout();
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
			int firstRowButtons = 2;
			int nbuttons2 = 6 + firstRowButtons;
			int nbuttons3 = defaults.Length - firstRowButtons;
			tableLayoutPanel2.ColumnCount = 2 * nbuttons2;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
			for (int i = 0; i < nbuttons2 - 1; i++){
				tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
				tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
			}
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
			tableLayoutPanel2.Controls.Add(addButton, 0, 0);
			tableLayoutPanel2.Controls.Add(removeButton, 2, 0);
			tableLayoutPanel2.Controls.Add(editButton, 4, 0);
			tableLayoutPanel2.Controls.Add(importButton, 6, 0);
			tableLayoutPanel2.Controls.Add(exportButton, 8, 0);
			tableLayoutPanel2.Controls.Add(tableTypeButton, 10, 0);
			for (int i = 0; i < firstRowButtons; i++){
				tableLayoutPanel2.Controls.Add(CreateDefaultButton(defaults[i]), 12 + 2 * i, 0);
			}
			tableLayoutPanel3.ColumnCount = 2 * nbuttons3;
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
			for (int i = 0; i < nbuttons3 - 1; i++){
				tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
				tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
			}
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
			for (int i = firstRowButtons; i < defaults.Length; i++){
				tableLayoutPanel3.Controls.Add(CreateDefaultButton(defaults[i]), 2 * (i - firstRowButtons), 0);
			}
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel2.Margin = new Padding(0);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Size = new System.Drawing.Size(2135, 50);
			tableLayoutPanel2.TabIndex = 2;
			tableLayoutPanel3.Dock = DockStyle.Fill;
			tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel3.Margin = new Padding(0);
			tableLayoutPanel3.Name = "tableLayoutPanel2";
			tableLayoutPanel3.RowCount = 1;
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel3.Size = new System.Drawing.Size(2135, 50);
			tableLayoutPanel3.TabIndex = 2;
			// 
			// addButton
			// 
			addButton.Dock = DockStyle.Fill;
			addButton.Location = new System.Drawing.Point(0, 0);
			addButton.Margin = new Padding(0);
			addButton.Name = "addButton";
			addButton.Size = new System.Drawing.Size(220, 50);
			addButton.TabIndex = 0;
			addButton.Text = @"Add";
			addButton.UseVisualStyleBackColor = true;
			// 
			// removeButton
			// 
			removeButton.Dock = DockStyle.Fill;
			removeButton.Location = new System.Drawing.Point(230, 0);
			removeButton.Margin = new Padding(0);
			removeButton.Name = "removeButton";
			removeButton.Size = new System.Drawing.Size(220, 50);
			removeButton.TabIndex = 1;
			removeButton.Text = @"Remove";
			removeButton.UseVisualStyleBackColor = true;
			// 
			// editButton
			// 
			editButton.Dock = DockStyle.Fill;
			editButton.Location = new System.Drawing.Point(460, 0);
			editButton.Margin = new Padding(0);
			editButton.Name = "editButton";
			editButton.Size = new System.Drawing.Size(220, 50);
			editButton.TabIndex = 1;
			editButton.Text = @"Edit";
			editButton.UseVisualStyleBackColor = true;
			// 
			// importButton
			// 
			importButton.Dock = DockStyle.Fill;
			importButton.Location = new System.Drawing.Point(690, 0);
			importButton.Margin = new Padding(0);
			importButton.Name = "importButton";
			importButton.Size = new System.Drawing.Size(220, 50);
			importButton.TabIndex = 0;
			importButton.Text = @"Import";
			importButton.UseVisualStyleBackColor = true;
			// 
			// exportButton
			// 
			exportButton.Dock = DockStyle.Fill;
			exportButton.Location = new System.Drawing.Point(920, 0);
			exportButton.Margin = new Padding(0);
			exportButton.Name = "exportButton";
			exportButton.Size = new System.Drawing.Size(220, 50);
			exportButton.TabIndex = 0;
			exportButton.Text = @"Export";
			exportButton.UseVisualStyleBackColor = true;
			// 
			// tableTypeButton
			// 
			tableTypeButton.Dock = DockStyle.Fill;
			tableTypeButton.Location = new System.Drawing.Point(920, 0);
			tableTypeButton.Margin = new Padding(0);
			tableTypeButton.Name = "tableTypeButton";
			tableTypeButton.Size = new System.Drawing.Size(220, 50);
			tableTypeButton.TabIndex = 0;
			tableTypeButton.Text = @"Change type";
			tableTypeButton.UseVisualStyleBackColor = true;
			tableLayoutPanel2.ResumeLayout(false);
			tableLayoutPanel3.ResumeLayout(false);
			tableViewSimple.TableModel = CreateTable();
			addButton.Click += AddButtonOnClick;
			removeButton.Click += RemoveButtonOnClick;
			editButton.Click += EditButtonOnClick;
			importButton.Click += ImportButtonOnClick;
			exportButton.Click += ExportButtonOnClick;
			tableTypeButton.Click += TableTypeButtonOnClick;
		}

		private Control CreateDefaultButton(IsobaricLabelingDefault def){
			Button button = new Button{
				Dock = DockStyle.Fill,
				Location = new System.Drawing.Point(230, 0),
				Margin = new Padding(0),
				Name = "button",
				Size = new System.Drawing.Size(220, 50),
				TabIndex = 1,
				Text = def.Name,
				UseVisualStyleBackColor = true
			};
			button.Click += (sender, args) => { SetDefaults(def); };
			return button;
		}

		private void SetDefaults(IsobaricLabelingDefault def){
			tableSimple.Clear();
			for (int i = 0; i < def.Count; i++){
				DataRow2 row = tableSimple.NewRow();
				row[0] = def.GetInternalLabel(i);
				row[1] = def.GetTerminalLabel(i);
				row[2] = 0d;
				row[3] = 0d;
				row[4] = 0d;
				row[5] = 0d;
				row[6] = def.IsLikelyTmtLike(i);
				tableSimple.AddRow(row);
			}
			tableViewSimple.Invalidate(true);
		}

		private DataTable2 CreateTable(){
			tableSimple = new DataTable2("isobaric labels table");
			tableSimple.AddColumn(headerSimple[0], 130, ColumnType.Text, "");
			tableSimple.AddColumn(headerSimple[1], 130, ColumnType.Text, "");
			tableSimple.AddColumn(headerSimple[2], 80, ColumnType.Numeric);
			tableSimple.AddColumn(headerSimple[3], 80, ColumnType.Numeric);
			tableSimple.AddColumn(headerSimple[4], 80, ColumnType.Numeric);
			tableSimple.AddColumn(headerSimple[5], 80, ColumnType.Numeric);
			tableSimple.AddColumn(headerSimple[6], 60, ColumnType.Boolean);
			return tableSimple;
		}

		public string[][] Value{
			get{
				string[][] result = new string[tableSimple.RowCount][];
				for (int i = 0; i < result.Length; i++){
					result[i] = new[]{
						(string) tableSimple.GetEntry(i, headerSimple[0]), (string) tableSimple.GetEntry(i, headerSimple[1]),
						((double) tableSimple.GetEntry(i, headerSimple[2])).ToString(CultureInfo.InvariantCulture),
						((double) tableSimple.GetEntry(i, headerSimple[3])).ToString(CultureInfo.InvariantCulture),
						((double) tableSimple.GetEntry(i, headerSimple[4])).ToString(CultureInfo.InvariantCulture),
						((double) tableSimple.GetEntry(i, headerSimple[5])).ToString(CultureInfo.InvariantCulture),
						((bool) tableSimple.GetEntry(i, headerSimple[6])).ToString()
					};
				}
				return result;
			}
			set{
				ClearVisibleTable();
				foreach (string[] t in value){
					AddLabel(t);
				}
			}
		}
		private void AddLabel(string[] t) {
			if (t.Length == headerSimple.Length){
				AddLabelSimple(t);
			} else{
				AddLabelComplex(t);
			}
		}
		private void AddLabelSimple(string[] t) {
			DataRow2 row = tableSimple.NewRow();
			row[0] = t[0];
			row[1] = t[1];
			row[2] = Parser.Double(t[2]);
			row[3] = Parser.Double(t[3]);
			row[4] = Parser.Double(t[4]);
			row[5] = Parser.Double(t[5]);
			row[6] = Parser.Bool(t[6]);
			tableSimple.AddRow(row);
		}
		private void AddLabelComplex(string[] t) {
			DataRow2 row = tableComplex.NewRow();
			row[0] = t[0];
			row[1] = t[1];
			row[2] = Parser.Double(t[2]);
			row[3] = Parser.Double(t[3]);
			row[4] = Parser.Double(t[4]);
			row[5] = Parser.Double(t[5]);
			row[6] = Parser.Double(t[6]);
			row[7] = Parser.Double(t[7]);
			row[8] = Parser.Double(t[8]);
			row[9] = Parser.Double(t[9]);
			row[10] = Parser.Bool(t[10]);
			tableComplex.AddRow(row);
		}
	}
}