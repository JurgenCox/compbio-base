using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Graphic;
using BaseLibS.Graph;
using BaseLibS.Graph.Base;

namespace BaseLib.Forms{
	public partial class PlanePropertiesForm : Form{
		public PlanePropertiesForm(Color2 horizontalGridColor, Color2 verticalGridColor, GridType horizontalGrid,
			GridType verticalGrid, int horizontalGridWidth, int verticalGridWidth, Color2 borderColor, Color2 backgroundColor,
			Color2 axisColor, int majorTickLength, float majorTickLineWidth, int minorTickLength, float minorTickLineWidth,
			bool topAxisVisible, bool rightAxisVisible, float axisLineWidth, int numbersFontSize, bool numbersFontBold,
			int titleFontSize, bool titleFontBold, Color2 horizontalZeroColor, Color2 verticalZeroColor, int horizontalZeroWidth,
			int verticalZeroWidth, bool horizontalZeroVisible, bool verticalZeroVisible){
			InitializeComponent();
			numbersFontSizeNumericUpDown.Value = numbersFontSize;
			numbersFontBoldCheckBox.Checked = numbersFontBold;
			titlesFontSizeNumericUpDown.Value = titleFontSize;
			titlesFontBoldCheckBox.Checked = titleFontBold;
			horizontalGridColorButton.BackColor = GraphUtils.ToColor(horizontalGridColor);
			verticalGridColorButton.BackColor = GraphUtils.ToColor(verticalGridColor);
			lineWidthNumericUpDown.Value = (decimal) axisLineWidth;
			switch (horizontalGrid){
				case GridType.None:
					horizontalGridComboBox.SelectedIndex = 0;
					break;
				case GridType.Major:
					horizontalGridComboBox.SelectedIndex = 1;
					break;
				case GridType.All:
					horizontalGridComboBox.SelectedIndex = 2;
					break;
			}
			switch (verticalGrid){
				case GridType.None:
					verticalGridComboBox.SelectedIndex = 0;
					break;
				case GridType.Major:
					verticalGridComboBox.SelectedIndex = 1;
					break;
				case GridType.All:
					verticalGridComboBox.SelectedIndex = 2;
					break;
			}
			showTopAxisCheckBox.Checked = topAxisVisible;
			showRightAxisCheckBox.Checked = rightAxisVisible;
			horizontalGridNumericUpDown.Value = horizontalGridWidth;
			verticalGridNumericUpDown.Value = verticalGridWidth;
			fillColorButton.BackColor = GraphUtils.ToColor(borderColor);
			backgroundColorButton.BackColor = GraphUtils.ToColor(backgroundColor);
			lineColorButton.BackColor = GraphUtils.ToColor(axisColor);
			majorTicLengthNumericUpDown.Value = majorTickLength;
			minorTicLengthNumericUpDown.Value = minorTickLength;
			majorTicLineWidthNumericUpDown.Value = (decimal) majorTickLineWidth;
			minorTicLineWidthNumericUpDown.Value = (decimal) minorTickLineWidth;
			horizontalZeroColorButton.BackColor = GraphUtils.ToColor(horizontalZeroColor);
			verticalZeroColorButton.BackColor = GraphUtils.ToColor(verticalZeroColor);
			horizontalZeroNumericUpDown.Value = horizontalZeroWidth;
			verticalZeroNumericUpDown.Value = verticalZeroWidth;
			horizontalZeroCheckBox.Checked = horizontalZeroVisible;
			verticalZeroCheckBox.Checked = verticalZeroVisible;
		}

		public bool Ok { get; private set; }
		public Color FillColor => fillColorButton.BackColor;
		public bool TopAxesVisible => showTopAxisCheckBox.Checked;
		public bool RightAxesVisible => showRightAxisCheckBox.Checked;
		public int MajorTickLength => (int) majorTicLengthNumericUpDown.Value;
		public int MajorTickLineWidth => (int) majorTicLineWidthNumericUpDown.Value;
		public int MinorTickLength => (int) minorTicLengthNumericUpDown.Value;
		public int MinorTickLineWidth => (int) minorTicLineWidthNumericUpDown.Value;
		public Color2 LineColor => GraphUtils.ToColor2(lineColorButton.BackColor) ;
		public Color2 BackgroundColor => GraphUtils.ToColor2(backgroundColorButton.BackColor);
		public Color2 HorizontalGridColor => GraphUtils.ToColor2(horizontalGridColorButton.BackColor);
		public Color2 VerticalGridColor => GraphUtils.ToColor2(verticalGridColorButton.BackColor);
		public Color2 HorizontalZeroColor => GraphUtils.ToColor2(horizontalZeroColorButton.BackColor);
		public Color2 VerticalZeroColor => GraphUtils.ToColor2(verticalZeroColorButton.BackColor);

		public GridType HorizontalGrid{
			get{
				int ind = horizontalGridComboBox.SelectedIndex;
				if (ind == 0){
					return GridType.None;
				}
				return ind == 1 ? GridType.Major : GridType.All;
			}
		}

		public GridType VerticalGrid{
			get{
				int ind = verticalGridComboBox.SelectedIndex;
				if (ind == 0){
					return GridType.None;
				}
				return ind == 1 ? GridType.Major : GridType.All;
			}
		}

		public int HorizontalGridWidth => (int) horizontalGridNumericUpDown.Value;
		public int VerticalGridWidth => (int) verticalGridNumericUpDown.Value;
		public int HorizontalZeroWidth => (int) horizontalZeroNumericUpDown.Value;
		public int VerticalZeroWidth => (int) verticalZeroNumericUpDown.Value;
		public bool HorizontalZeroVisible => horizontalZeroCheckBox.Checked;
		public bool VerticalZeroVisible => verticalZeroCheckBox.Checked;
		public int LineWidth => (int) lineWidthNumericUpDown.Value;
		public int NumbersFontSize => (int) numbersFontSizeNumericUpDown.Value;
		public bool NumbersFontBold => numbersFontBoldCheckBox.Checked;
		public int TitleFontSize => (int) titlesFontSizeNumericUpDown.Value;
		public bool TitleFontBold => titlesFontBoldCheckBox.Checked;

		private void HorizontalGridColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = horizontalGridColorButton.BackColor};
			cd.ShowDialog();
			horizontalGridColorButton.BackColor = cd.Color;
			cd.Dispose();
		}

		private void VerticalGridColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = verticalGridColorButton.BackColor};
			cd.ShowDialog();
			verticalGridColorButton.BackColor = cd.Color;
			cd.Dispose();
		}

		private void BorderColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = fillColorButton.BackColor};
			cd.ShowDialog();
			fillColorButton.BackColor = cd.Color;
			cd.Dispose();
		}

		private void CancelButtonClick(object sender, EventArgs e){
			Close();
		}

		private void OkButtonClick(object sender, EventArgs e){
			Ok = true;
			Close();
		}

		private void BackgroundColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = backgroundColorButton.BackColor};
			cd.ShowDialog();
			backgroundColorButton.BackColor = cd.Color;
			cd.Dispose();
		}

		private void AxisColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = lineColorButton.BackColor};
			cd.ShowDialog();
			lineColorButton.BackColor = cd.Color;
			cd.Dispose();
		}

		private void HorizontalZeroColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = horizontalZeroColorButton.BackColor};
			cd.ShowDialog();
			horizontalZeroColorButton.BackColor = cd.Color;
			cd.Dispose();
		}

		private void VerticalZeroColorButtonClick(object sender, EventArgs e){
			ColorDialog cd = new ColorDialog{Color = verticalZeroColorButton.BackColor};
			cd.ShowDialog();
			verticalZeroColorButton.BackColor = cd.Color;
			cd.Dispose();
		}
	}
}