using System;
using System.Windows.Forms;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms {
	public partial class IsobaricLabelsParamControl : UserControl {
		private readonly IsobaricLabelingDefault[] defaults = {
			new IsobaricLabelingDefault("4plex iTRAQ",
				new[] {"iTRAQ4plex-Lys114", "iTRAQ4plex-Lys115", "iTRAQ4plex-Lys116", "iTRAQ4plex-Lys117"},
				new[] {"iTRAQ4plex-Nter114", "iTRAQ4plex-Nter115", "iTRAQ4plex-Nter116", "iTRAQ4plex-Nter117"}),
			new IsobaricLabelingDefault("8plex iTRAQ",
				new[] {
					"iTRAQ8plex-Lys113", "iTRAQ8plex-Lys114", "iTRAQ8plex-Lys115", "iTRAQ8plex-Lys116", "iTRAQ8plex-Lys117",
					"iTRAQ8plex-Lys118", "iTRAQ8plex-Lys119", "iTRAQ8plex-Lys121"
				},
				new[] {
					"iTRAQ8plex-Nter113", "iTRAQ8plex-Nter114", "iTRAQ8plex-Nter115", "iTRAQ8plex-Nter116", "iTRAQ8plex-Nter117",
					"iTRAQ8plex-Nter118", "iTRAQ8plex-Nter119", "iTRAQ8plex-Nter121"
				}),
			new IsobaricLabelingDefault("2plex TMT", new[] {"TMT2plex-Lys126", "TMT2plex-Lys127"},
				new[] {"TMT2plex-Nter126", "TMT2plex-Nter127"}),
			new IsobaricLabelingDefault("6plex TMT",
				new[] {
					"TMT6plex-Lys126", "TMT6plex-Lys127", "TMT6plex-Lys128", "TMT6plex-Lys129", "TMT6plex-Lys130", "TMT6plex-Lys131"
				},
				new[] {
					"TMT6plex-Nter126", "TMT6plex-Nter127", "TMT6plex-Nter128", "TMT6plex-Nter129", "TMT6plex-Nter130",
					"TMT6plex-Nter131"
				}),
			new IsobaricLabelingDefault("8plex TMT",
				new[] {
					"TMT8plex-Lys126C", "TMT8plex-Lys127N", "TMT8plex-Lys127C", "TMT8plex-Lys128C", "TMT8plex-Lys129N",
					"TMT8plex-Lys129C", "TMT8plex-Lys130C", "TMT8plex-Lys131N"
				},
				new[] {
					"TMT8plex-Nter126C", "TMT8plex-Nter127N", "TMT8plex-Nter127C", "TMT8plex-Nter128C", "TMT8plex-Nter129N",
					"TMT8plex-Nter129C", "TMT8plex-Nter130C", "TMT8plex-Nter131N"
				}),
			new IsobaricLabelingDefault("10plex TMT",
				new[] {
					"TMT10plex-Lys126C", "TMT10plex-Lys127N", "TMT10plex-Lys127C", "TMT10plex-Lys128N", "TMT10plex-Lys128C",
					"TMT10plex-Lys129N", "TMT10plex-Lys129C", "TMT10plex-Lys130N", "TMT10plex-Lys130C", "TMT10plex-Lys131N"
				},
				new[] {
					"TMT10plex-Nter126C", "TMT10plex-Nter127N", "TMT10plex-Nter127C", "TMT10plex-Nter128N", "TMT10plex-Nter128C",
					"TMT10plex-Nter129N", "TMT10plex-Nter129C", "TMT10plex-Nter130N", "TMT10plex-Nter130C", "TMT10plex-Nter131N"
				}),
			new IsobaricLabelingDefault("11plex TMT",
				new[] {
					"TMT10plex-Lys126C", "TMT10plex-Lys127N", "TMT10plex-Lys127C", "TMT10plex-Lys128N", "TMT10plex-Lys128C",
					"TMT10plex-Lys129N", "TMT10plex-Lys129C", "TMT10plex-Lys130N", "TMT10plex-Lys130C", "TMT10plex-Lys131N",
					"TMT11plex-Lys131C"
				},
				new[] {
					"TMT10plex-Nter126C", "TMT10plex-Nter127N", "TMT10plex-Nter127C", "TMT10plex-Nter128N", "TMT10plex-Nter128C",
					"TMT10plex-Nter129N", "TMT10plex-Nter129C", "TMT10plex-Nter130N", "TMT10plex-Nter130C", "TMT10plex-Nter131N",
					"TMT11plex-Nter131C"
				}),
			new IsobaricLabelingDefault("iodo6plexTMT",
				new[] {
					"iodoTMT6plex-Cys126", "iodoTMT6plex-Cys127", "iodoTMT6plex-Cys128", "iodoTMT6plex-Cys129", "iodoTMT6plex-Cys130",
					"iodoTMT6plex-Cys131"
				}, new string[] { }),
		};

		private DataTable2 table;
		private TableLayoutPanel tableLayoutPanel2;
		private Button addButton;
		private Button removeButton;
		private Button editButton;

		public IsobaricLabelsParamControl() {
			InitializeComponent();
			InitializeComponent2();
			tableView1.TableModel = CreateTable();
			addButton.Click += AddButtonOnClick;
			removeButton.Click += RemoveButtonOnClick;
			editButton.Click += EditButtonOnClick;
		}

		private void AddButtonOnClick(object sender, EventArgs eventArgs) {
			DataRow2 row = table.NewRow();
			row["Internal label"] = "";
			row["Terminal label"] = "";
			row["Correction factor -2 [%]"] = 0d;
			row["Correction factor -1 [%]"] = 0d;
			row["Correction factor +1 [%]"] = 0d;
			row["Correction factor +2 [%]"] = 0d;
			row["TMT like"] = true;
			table.AddRow(row);
			tableView1.Invalidate(true);
		}

		private void RemoveButtonOnClick(object sender, EventArgs eventArgs) {
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0) {
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			table.RemoveRows(sel);
			tableView1.Invalidate(true);
		}

		private void EditButtonOnClick(object sender, EventArgs eventArgs) {
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length != 1) {
				MessageBox.Show("Please select exactly one row.");
				return;
			}
			DataRow2 row = table.GetRow(sel[0]);
			IsobaricLabelsEditForm f = new IsobaricLabelsEditForm((string) row[0], (string) row[1], (double) row[2],
				(double) row[3], (double) row[4], (double) row[5], (bool) row[6]);
			f.ShowDialog();
			if (f.DialogResult != DialogResult.OK) {
				return;
			}
			row[0] = f.InternalLabel;
			row[1] = f.TerminalLabel;
			row[2] = f.CorrectionFactorM2;
			row[3] = f.CorrectionFactorM1;
			row[4] = f.CorrectionFactorP1;
			row[5] = f.CorrectionFactorP2;
			row[6] = f.TmtLike;
			tableView1.Invalidate(true);
		}

		private void InitializeComponent2() {
			tableLayoutPanel2 = new TableLayoutPanel();
			addButton = new Button();
			removeButton = new Button();
			editButton = new Button();
			tableLayoutPanel2.SuspendLayout();
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
			int nbuttons = 10;
			tableLayoutPanel2.ColumnCount = 2 * nbuttons;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
			for (int i = 0; i < nbuttons - 1; i++) {
				tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
				tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
			}
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
			tableLayoutPanel2.Controls.Add(addButton, 0, 0);
			tableLayoutPanel2.Controls.Add(removeButton, 2, 0);
			tableLayoutPanel2.Controls.Add(editButton, 4, 0);
			for (int i = 0; i < defaults.Length; i++) {
				tableLayoutPanel2.Controls.Add(CreateDefaultButton(defaults[i]), 6 + 2 * i, 0);
			}
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel2.Margin = new Padding(0);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Size = new System.Drawing.Size(2135, 50);
			tableLayoutPanel2.TabIndex = 2;
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
			editButton.Location = new System.Drawing.Point(230, 0);
			editButton.Margin = new Padding(0);
			editButton.Name = "editButton";
			editButton.Size = new System.Drawing.Size(220, 50);
			editButton.TabIndex = 1;
			editButton.Text = @"Edit";
			editButton.UseVisualStyleBackColor = true;
			tableLayoutPanel2.ResumeLayout(false);
		}

		private Control CreateDefaultButton(IsobaricLabelingDefault def) {
			Button button = new Button {
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

		private void SetDefaults(IsobaricLabelingDefault def) {
			table.Clear();
			for (int i = 0; i < def.Count; i++) {
				DataRow2 row = table.NewRow();
				row[0] = def.GetInternalLabel(i);
				row[1] = def.GetTerminalLabel(i);
				row[2] = 0d;
				row[3] = 0d;
				row[4] = 0d;
				row[5] = 0d;
				row[6] = def.IsLikelyTmtLike(i);
				table.AddRow(row);
			}
			tableView1.Invalidate(true);
		}

		private DataTable2 CreateTable() {
			table = new DataTable2("isobaric labels table");
			table.AddColumn("Internal label", 130, ColumnType.Text, "");
			table.AddColumn("Terminal label", 130, ColumnType.Text, "");
			table.AddColumn("Correction factor -2 [%]", 80, ColumnType.Numeric);
			table.AddColumn("Correction factor -1 [%]", 80, ColumnType.Numeric);
			table.AddColumn("Correction factor +1 [%]", 80, ColumnType.Numeric);
			table.AddColumn("Correction factor +2 [%]", 80, ColumnType.Numeric);
			table.AddColumn("TMT like", 60, ColumnType.Boolean);
			return table;
		}

		public string[][] Value {
			get {
				string[][] result = new string[table.RowCount][];
				for (int i = 0; i < result.Length; i++) {
					result[i] = new[] {
						(string) table.GetEntry(i, "Internal label"), (string) table.GetEntry(i, "Terminal label"),
						((double) table.GetEntry(i, "Correction factor -2 [%]")).ToString(),
						((double) table.GetEntry(i, "Correction factor -1 [%]")).ToString(),
						((double) table.GetEntry(i, "Correction factor +1 [%]")).ToString(),
						((double) table.GetEntry(i, "Correction factor +2 [%]")).ToString(),
						((bool) table.GetEntry(i, "TMT like")).ToString()
					};
				}
				return result;
			}
			set {
				table.Clear();
				foreach (string[] t in value) {
					AddLabel(t[0], t[1], t[2], t[3], t[4], t[5], t[6]);
				}
			}
		}

		private void AddLabel(string internalLabel, string terminalLabel, string correctionFactorM2,
			string correctionFactorM1, string correctionFactorP1, string correctionFactorP2, string tmtLike) {
			DataRow2 row = table.NewRow();
			row[0] = internalLabel;
			row[1] = terminalLabel;
			row[2] = Parser.Double(correctionFactorM2);
			row[3] = Parser.Double(correctionFactorM1);
			row[4] = Parser.Double(correctionFactorP1);
			row[5] = Parser.Double(correctionFactorP2);
			row[6] = Parser.Bool(tmtLike);
			table.AddRow(row);
		}
	}
}