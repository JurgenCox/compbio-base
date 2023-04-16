using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class BrukerTims : QtofInstrument{
		public BrukerTims(int index) : base(index){ }
		public override string Name => "Bruker TIMS";
		public override double IntensityThresholdMs1DdaDefault => 30;
		public override double IntensityThresholdMs1DiaDefault => 70;
		public override double IntensityThresholdMs2Default => 30;
		public override double DiaMinMsmsIntensityForQuantDefault => 30;
		public override int DiaTopNFragmentsForQuantDefault => 4;
		public sealed override bool UseMs1CentroidsDefault => false;
		public sealed override bool UseMs2CentroidsDefault => false;
	}
}