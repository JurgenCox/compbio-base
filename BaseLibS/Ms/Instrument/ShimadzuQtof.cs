using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	public class ShimadzuQtof : QtofInstrument{
		public ShimadzuQtof(int index) : base(index){ }
		public override string Name => "Shimadzu Q-TOF";
		public override double IntensityThresholdMs1DdaDefault => 10;
		public override double IntensityThresholdMs1DiaDefault => 10;
		public override double IntensityThresholdMs2Default => 10;

		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;
		public override double DiaMinMsmsIntensityForQuantDefault => 0;
	}
}