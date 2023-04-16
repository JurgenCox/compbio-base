using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class AgilentQtof : QtofInstrument{
		public AgilentQtof(int index) : base(index){ }
		public override string Name => "Agilent Q-TOF";
		public override double IntensityThresholdMs1DdaDefault => 0;
		public override double IntensityThresholdMs1DiaDefault => 0;
		public override double IntensityThresholdMs2Default => 0;
		public override bool UseMs1CentroidsDefault => true;
		public override bool UseMs2CentroidsDefault => true;
		public override double DiaMinMsmsIntensityForQuantDefault => 0;
		public override int DiaTopNFragmentsForQuantDefault => 10;
	}
}