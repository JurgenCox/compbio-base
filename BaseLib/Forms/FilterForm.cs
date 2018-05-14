using System;
using System.Windows.Forms;
using BaseLib.Graphic;
using BaseLib.Param;
using BaseLibS.Graph;
using BaseLibS.Param;

namespace BaseLib.Forms{
	public partial class FilterForm : Form{
		private readonly SubSelectionControl subSelectionControl;

		public FilterForm(SubSelectionControl subSelectionControl){
			InitializeComponent();
			this.subSelectionControl = subSelectionControl;
			addButton.Image = GraphUtils.ToBitmap(Bitmap2.GetImage("plus1.bmp"));
			removeButton.Image = GraphUtils.ToBitmap(Bitmap2.GetImage("minus1.bmp"));
			//Icon = GraphUtils.ToBitmap(Bitmap2.GetImage("Perseus.jpg"));
			addButton.Click += AddButton_OnClick;
			removeButton.Click += RemoveButton_OnClick;
			RebuildGui();
		}

		private void AddButton_OnClick(object sender, EventArgs e){
			Parameters p = subSelectionControl.GetParameters();
			subSelectionControl.parameters.Add(p);
			RebuildGui();
		}

		private void RebuildGui(){
			TableLayoutPanel g = new TableLayoutPanel();
			foreach (Parameters t in subSelectionControl.parameters){
				t.SetValuesFromControl();
			}
			for (int i = 0; i < subSelectionControl.parameters.Count; i++){
				g.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100));
			}
			for (int i = 0; i < subSelectionControl.parameters.Count; i++){
				ParameterPanel tb = new ParameterPanel();
				tb.Init(subSelectionControl.parameters[i]);
				g.Controls.Add(tb, 0, i);
			}
			Controls.Add(g);
		}

		private void RemoveButton_OnClick(object sender, EventArgs e){
			if (subSelectionControl.parameters.Count == 0){
				return;
			}
			subSelectionControl.parameters.RemoveAt(subSelectionControl.parameters.Count - 1);
			RebuildGui();
		}

		protected override void OnClosed(EventArgs e){
			foreach (Parameters t in subSelectionControl.parameters){
				t.SetValuesFromControl();
			}
			base.OnClosed(e);
		}
	}
}