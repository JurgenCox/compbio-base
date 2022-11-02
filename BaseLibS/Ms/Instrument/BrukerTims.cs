using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class BrukerTims : QtofInstrument{
		public BrukerTims(int index) : base(index){ }
		public override string Name => "Bruker TIMS";
		public override double IntensityThresholdMs1Default => 17;
		public override double IntensityThresholdMs2Default => 11;
		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;
		public override double DiaMinMsmsIntensityForQuantDefault => 30;
		public override double DiaTopMsmsIntensityQuantileForQuantDefault => 0.85;

		public override int DiaTopNFragmentsForQuantDefault => 4;
		public override int GetMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Proteins:
				case MsDataType.Metabolites:
					return 2;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override int GetDiaMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Proteins:
				case MsDataType.Metabolites:
					return 1;
				default:
					throw new Exception("Never get here.");
			}
		}
		public override double GetCentroidMatchTolDefault(MsDataType dataType) {
			return 10;
		}

		public override float[] SmoothIntensityProfile(float[] origProfile){
			return ArrayUtils.SmoothMean(origProfile, 1);
		}
		public override double DiaInitialPrecMassTolPpmDefault => 20;
		public override double DiaInitialFragMassTolPpmDefault => 25;
		public override bool DiaBackgroundSubtractionDefault => false;
		public override double DiaBackgroundSubtractionQuantileDefault => 0.5;
		public override double DiaBackgroundSubtractionFactorDefault => 4;
		public override LfqRatioType DiaLfqRatioTypeDefault => LfqRatioType.Median;
	}
}