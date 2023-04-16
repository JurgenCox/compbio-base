using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class SciexQtof : QtofInstrument{
		public SciexQtof(int index) : base(index){ }
		public override string Name => "Sciex Q-TOF";
		public override double IntensityThresholdMs1DdaDefault => 0;
		public override double IntensityThresholdMs1DiaDefault => 0;
		public override double IntensityThresholdMs2Default => 0;

		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;

		public override double DiaMinMsmsIntensityForQuantDefault => 0;
		public override int DiaTopNFragmentsForQuantDefault => 3;
	}
}