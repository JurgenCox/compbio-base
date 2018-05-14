using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BaseLib.Forms {
	public partial class CheckedListControl : UserControl {
		private readonly List<CheckBox> checkBoxes = new List<CheckBox>();
		private TableLayoutPanel tableLayoutPanel1;
		public event EventHandler<ItemCheckEventArgs> ItemCheck;

		public CheckedListControl() {
			InitializeComponent();
		}

		public void AddRange(string[] text) {
			//TODO
			float sfx = 1;
			//float sfx = FormUtils.GetDpiScale(CreateGraphics());
			Controls.Clear();
			checkBoxes.Clear();
			tableLayoutPanel1 = new TableLayoutPanel {ColumnCount = 1};
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = text.Length;
			for (int i = 0; i < text.Length; i++) {
				tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, sfx * 23F));
			}
			tableLayoutPanel1.Size = new Size((int)(sfx * 260), (int)(sfx * 23 * text.Length));
			tableLayoutPanel1.TabIndex = 0;
			tableLayoutPanel1.AutoScroll = true;
			tableLayoutPanel1.VerticalScroll.Visible = true;
			// 
			// CheckedListControl
			// 
			AutoScaleDimensions = new SizeF(16F, 31F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(tableLayoutPanel1);
			Name = "CheckedListControl";
			Size = new Size((int)(sfx * 260), (int)(sfx * 150));
			for (int index = 0; index < text.Length; index++) {
				string s = text[index];
				CheckBox cb = new CheckBox {Text = s, Margin = new Padding(0), Dock = DockStyle.Fill};
				checkBoxes.Add(cb);
				tableLayoutPanel1.Controls.Add(cb, 0, index);
			}
		}

		public int[] CheckedIndices {
			get {
				List<int> result = new List<int>();
				for (int i = 0; i < checkBoxes.Count; i++) {
					if (checkBoxes[i].Checked) {
						result.Add(i);
					}
				}
				return result.ToArray();
			}
		}

		public string[] CheckedItems {
			get {
				List<string> result = new List<string>();
				foreach (CheckBox t in checkBoxes) {
					if (t.Checked) {
						result.Add(t.Text);
					}
				}
				return result.ToArray();
			}
		}

		public int Count => checkBoxes.Count;

		public void SetItemChecked(int i, bool b) {
			checkBoxes[i].Checked = b;
		}
	}
}