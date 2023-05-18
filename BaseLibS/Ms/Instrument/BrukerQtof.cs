using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class BrukerQtof : QtofInstrument{
		public BrukerQtof(int index) : base(index){ }
		public override string Name => "Bruker Q-TOF";
		public override double IntensityThresholdMs1DdaDefault => 30;
		public override double IntensityThresholdMs1DiaDefault => 30;
		public override double IntensityThresholdMs2Default => 20;
		public override bool UseMs1CentroidsDefault => true;
		public override bool UseMs2CentroidsDefault => true;
		public override double DiaMinMsmsIntensityForQuantDefault => 30;
	}
}