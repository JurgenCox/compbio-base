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

		private void EditButtonOnClick(object sender, EventArgs eventArgs) { }

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
				tableLayoutPanel2.Controls.Add(CreateDefaultButton(defaults[i]), 6 + 2*i, 0);
			}


			//tableLayoutPanel2.Controls.Add(identifierRuleButton, 6, 0);
			//tableLayoutPanel2.Controls.Add(descriptionRuleButton, 8, 0);
			//tableLayoutPanel2.Controls.Add(taxonomyRuleButton, 10, 0);
			//tableLayoutPanel2.Controls.Add(taxonomyIdButton, 12, 0);
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

		private static Control CreateDefaultButton(IsobaricLabelingDefault def) {
			Button button = new Button {
				Dock = DockStyle.Fill,
				Location = new System.Drawing.Point(230, 0),
				Margin = new Padding(0),
				Name = "removeButton",
				Size = new System.Drawing.Size(220, 50),
				TabIndex = 1,
				Text = def.Name,
				UseVisualStyleBackColor = true
			};
			return button;
		}

		private DataTable2 CreateTable() {
			table = new DataTable2("isobaric labels table");
			table.AddColumn("Internal label", 120, ColumnType.Text, "");
			table.AddColumn("Terminal label", 120, ColumnType.Text, "");
			table.AddColumn("Correction factor -2 [%]", 100, ColumnType.Numeric);
			table.AddColumn("Correction factor -1 [%]", 100, ColumnType.Numeric);
			table.AddColumn("Correction factor +1 [%]", 100, ColumnType.Numeric);
			table.AddColumn("Correction factor +2 [%]", 100, ColumnType.Numeric);
			table.AddColumn("TMT like", 60, ColumnType.Boolean);
			return table;
		}

		public string[][] Value { get; set; }
	}
}