using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class SciexQtof : QtofInstrument{
		public SciexQtof(int index) : base(index){ }
		public override string Name => "Sciex Q-TOF";
		public override double IntensityThresholdMs1Default => 100;
		public override double IntensityThresholdMs2Default => 20;

		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;

		public override int GetMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
				case MsDataType.Peptides: return 2;
				case MsDataType.Metabolites: return 3;
				default: throw new Exception("Never get here.");
			}
		}

		public override int GetDiaMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
				case MsDataType.Peptides: return 2;
				case MsDataType.Metabolites: return 3;
				default: throw new Exception("Never get here.");
			}
		}

		public override float[] SmoothIntensityProfile(float[] origProfile){
			return ArrayUtils.SmoothMean(origProfile, 1);
		}

		public override int DiaTopNFragmentsForQuantDefault => 3;
		public override double DiaInitialPrecMassTolPpmDefault => 20;
		public override double DiaInitialFragMassTolPpmDefault => 20;
		public override bool DiaBackgroundSubtractionDefault => true;
		public override double DiaBackgroundSubtractionQuantileDefault => 0.7;
		public override double DiaBackgroundSubtractionFactorDefault => 4;
		public override LfqRatioType DiaLfqRatioTypeDefault => LfqRatioType.Median;
	}
}