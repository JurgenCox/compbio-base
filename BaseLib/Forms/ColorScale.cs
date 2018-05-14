using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLibS.Graph;
using BaseLibS.Graph.Axis;

namespace BaseLib.Forms{
	public delegate void ColorChangeHandler();

	public partial class ColorScale : UserControl{
		public event Action OnColorChange;
		private readonly NumericAxisView axis;
		public bool Locked { get; set; }
		public ColorStripView ColorStrip { get; }
		private readonly BasicControl colorStripControl;

		public ColorScale(){
			InitializeComponent();
			ColorStrip = new ColorStripView();
			axis = new NumericAxisView();
			SuspendLayout();
			axis.ForeColor = Color2.Black;
			AutoScaleDimensions = new SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			colorStripControl = BasicControl.CreateControl(ColorStrip);
			colorStripControl.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right;
			colorStripControl.Location = new Point(0, 59);
			colorStripControl.Margin = new Padding(0);
			colorStripControl.Name = "colorStrip";
			colorStripControl.Size = new Size(749, 31);
			Controls.Add(colorStripControl);
			BasicControl axisControl = BasicControl.CreateControl(axis);
			axisControl.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
			axisControl.ForeColor = Color.Black;
			axisControl.Location = new Point(0, 0);
			axisControl.Margin = new Padding(0);
			axisControl.Name = "axis";
			axisControl.Size = new Size(749, 59);
			Controls.Add(axisControl);
			Name = "ColorScale";
			Size = new Size(749, 90);
			ResumeLayout(false);
			ColorStrip.Arrow = Arrows.Second;
			ColorStrip.StartupColorMax = Color2.Red;
			ColorStrip.StartupColorMin = Color2.White;
			ColorStrip.StripWidth = 10;
			ColorStrip.Vertical = false;
			ColorStrip.Weight1 = 1F;
			ColorStrip.Weight2 = 0F;
			axis.Configurable = true;
			axis.IndicatorColor = Color2.Transparent;
			axis.IsLogarithmic = false;
			axis.LineWidth = 0.5F;
			axis.MajorTickLength = 6;
			axis.MajorTickLineWidth = 0.5F;
			axis.MinorTickLength = 3;
			axis.MinorTickLineWidth = 0.5F;
			axis.MouseMode = AxisMouseMode.Zoom;
			axis.Positioning = AxisPositioning.Top;
			axis.Reverse = false;
			axis.TotalMax = 1;
			axis.TotalMin = 0;
			axis.ZeroPoint = double.NaN;
			axis.ZoomMax = 1;
			axis.ZoomMin = 0;
			axis.ZoomType = AxisZoomType.Zoom;
			ColorStrip.OnColorChange += UpdateAxis;
			axis.OnZoomChange += UpdateColor;
		}

		private void UpdateAxis(){
			FireColorChanged();
		}

		private void UpdateColor(object source, double min, double max){
			Locked = true;
			FireColorChanged();
		}

		private void FireColorChanged(){
			OnColorChange?.Invoke();
		}

		//TODO: why is this not visible?
		public override string Text{
			get => axis.Text;
			set{
				axis.Text = value;
				axis.Invalidate();
			}
		}

		public void InitColors(Color2[] colors, double[] positions){
			ColorStrip.InitColors(colors, positions);
		}

		public AxisPositioning Positioning{
			get => axis.Positioning;
			set => axis.Positioning = value;
		}

		public bool Reverse{
			get => axis.Reverse;
			set => axis.Reverse = value;
		}

		public bool IsLogarithmic{
			get => axis.IsLogarithmic;
			set{
				bool oldValue = axis.IsLogarithmic;
				if (oldValue != value){
					if (value){
						axis.TotalMin = Math.Log(axis.TotalMin);
						axis.TotalMax = Math.Log(axis.TotalMax);
					} else{
						axis.TotalMin = Math.Exp(axis.TotalMin);
						axis.TotalMax = Math.Exp(axis.TotalMax);
					}
					axis.ZoomMin = axis.TotalMin;
					axis.ZoomMax = axis.TotalMax;
				}
				axis.IsLogarithmic = value;
				FireColorChanged();
			}
		}

		public double Min{
			get => IsLogarithmic ? Math.Exp(axis.ZoomMin) : axis.ZoomMin;
			set{
				double v = IsLogarithmic ? Math.Log(value) : value;
				axis.TotalMin = v;
				axis.ZoomMin = v;
				axis.Invalidate();
			}
		}

		public double Max{
			get => IsLogarithmic ? Math.Exp(axis.ZoomMax) : axis.ZoomMax;
			set{
				double v = IsLogarithmic ? Math.Log(value) : value;
				axis.TotalMax = v;
				axis.ZoomMax = v;
				axis.Invalidate();
			}
		}

		public Pen2 GetPen(double value){
			return ColorStrip.GetPenAt(GetScaledValue(value), colorStripControl.Width, colorStripControl.Height);
		}

		public Color2 GetColor(double value){
			return ColorStrip.GetColorAt(GetScaledValue(value), colorStripControl.Width, colorStripControl.Height);
		}

		public double GetScaledValue(double unscaledValue){
			if (unscaledValue <= Min){
				return 0;
			}
			if (unscaledValue >= Max){
				return 1;
			}
			if (IsLogarithmic){
				unscaledValue = Math.Log(unscaledValue);
			}
			return (unscaledValue - axis.ZoomMin)/(axis.ZoomMax - axis.ZoomMin);
		}

		public void WidenRange(double min, double max, bool fullZoom){
			axis.WidenRange(min, max, fullZoom);
			FireColorChanged();
		}

		public void AdjustMax(double[,] values){
			if (values == null) {
				return;
			}
		    double max = 0;
			for (int i = 0; i < values.GetLength(0); i++){
				for (int j = 0; j < values.GetLength(1); j++){
				    double v = IsLogarithmic ? Math.Log(Math.Max(1, values[i, j])) : values[i, j];
					if (v > max){
						max = v;
					}
				}
			}
			Max = max;
		}

		public void Adjust(double[,] values){
			if (values == null) {
				return;
			}
		    double min = double.MaxValue;
		    double max = -double.MaxValue;
			for (int i = 0; i < values.GetLength(0); i++){
				for (int j = 0; j < values.GetLength(1); j++){
				    double v = IsLogarithmic ? Math.Log(Math.Max(1, values[i, j])) : values[i, j];
					if (v < min){
						min = v;
					}
					if (v > max){
						max = v;
					}
				}
			}
			axis.SetZoomNoFire(min, max);
		}

		public void SetLogarithmic(bool isLogarithmic){
			axis.IsLogarithmic = isLogarithmic;
		}

		public override Color BackColor{
			get => base.BackColor;
			set{
				base.BackColor = value;
				axis.BackColor = Color2.FromArgb(value.A, value.R, value.G, value.B);
			}
		}
	}
}