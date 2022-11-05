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
		public override double GetCentroidMatchTolDefault(MsDataType dataType) {
			return 15;
		}

		public override float[] SmoothIntensityProfile(float[] origProfile) {
			float[] result = ArrayUtils.SmoothMedian(origProfile, 1);
			return ArrayUtils.SmoothMean(result, 2);
		}

		public override int DiaTopNFragmentsForQuantDefault => 3;
		public override double DiaInitialPrecMassTolPpmDefault => 20;
		public override double DiaInitialFragMassTolPpmDefault => 20;
		public override bool DiaBackgroundSubtractionDefault => false;
		public override double DiaBackgroundSubtractionQuantileDefault => 0.7;
		public override double DiaBackgroundSubtractionFactorDefault => 4;
		public override LfqRatioType DiaLfqRatioTypeDefault => LfqRatioType.Median;
	}
}