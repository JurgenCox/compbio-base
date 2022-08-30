using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	public class ShimadzuQtof : QtofInstrument{
		public ShimadzuQtof(int index) : base(index){ }
		public override string Name => "Shimadzu Q-TOF";
		public override double IntensityThresholdMs1Default => 10;
		public override double IntensityThresholdMs2Default => 10;

		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;
		public override double DiaMinMsmsIntensityForQuantDefault => 0;
		public override double DiaTopMsmsIntensityQuantileForQuantDefault => 0.85;

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

		public override int DiaTopNFragmentsForQuantDefault => 10;

		public override double DiaInitialPrecMassTolPpmDefault => 20;
		public override double DiaInitialFragMassTolPpmDefault => 20;
		public override bool DiaBackgroundSubtractionDefault => false;
		public override double DiaBackgroundSubtractionQuantileDefault => 0.5;
		public override double DiaBackgroundSubtractionFactorDefault => 4;
		public override LfqRatioType DiaLfqRatioTypeDefault => LfqRatioType.Median;
	}
}