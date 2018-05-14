using System;
using System.Windows.Forms;

namespace BaseLib.Forms{
	public partial class SliderControl : UserControl{
		public event EventHandler ValueChanged;
		public double Minimum { get; set; }
		public double Maximum { get; set; }
		public SliderControl(){
			InitializeComponent();
			trackBar1.ValueChanged += (sender, args) => { ValueChanged?.Invoke(sender, args); };
		}
		public double Value{
			get{
				double a = 0;
				if (InvokeRequired){
					Invoke((Action) (() => a = (trackBar1.Value - trackBar1.Minimum)/(double) (trackBar1.Maximum - trackBar1.Minimum)));
				} else{
					a = (trackBar1.Value - trackBar1.Minimum)/(double) (trackBar1.Maximum - trackBar1.Minimum);
				}
				return Minimum + a*(Maximum - Minimum);
			}
			set{
				double a = (value - Minimum)/(Maximum - Minimum);
				if (double.IsNaN(a) || double.IsInfinity(a)){
					return;
				}
				trackBar1.Value = (int) Math.Round(trackBar1.Minimum + a*(trackBar1.Maximum - trackBar1.Minimum));
			}
		}
		public TickStyle TickStyle{
			get => trackBar1.TickStyle;
			set => trackBar1.TickStyle = value;
		}
		public int TickFrequency{
			get => trackBar1.TickFrequency;
			set => trackBar1.TickFrequency = value;
		}
		public Orientation Orientation{
			get => trackBar1.Orientation;
			set => trackBar1.Orientation = value;
		}
	}
}