using System;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class WatersQtof : QtofInstrument{
		public WatersQtof(int index) : base(index){ }
		public override string Name => "Waters Q-TOF";
		public override double IntensityThresholdMs1DdaDefault => 15;
		public override double IntensityThresholdMs1DiaDefault => 15;
		public override double IntensityThresholdMs2Default => 10;
		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;
		public override double DiaMinMsmsIntensityForQuantDefault => 0;
	}
}