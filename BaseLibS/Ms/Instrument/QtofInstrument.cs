using BaseLibS.Num;
using System;
namespace BaseLibS.Ms.Instrument{
	public abstract class QtofInstrument : MsInstrument{
		protected QtofInstrument(int index) : base(index){ }

		public sealed override double IsotopeTimeCorrelationDefault => 0.6;
		public sealed override double TheorIsotopeCorrelationDefault => 0.6;
		public sealed override bool IntensityDependentCalibrationDefault => true;

		public sealed override double GetIsotopeMatchTolDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
				case MsDataType.Proteins:
					return 0.005;
				default:
					throw new Exception("Never get here.");
			}
		}

		public sealed override bool IsotopeMatchTolInPpmDefault => false;

		public sealed override int GetMaxChargeDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
					return 4;
				case MsDataType.Proteins:
					return 20;
				case MsDataType.Metabolites:
					return 2;
				default:
					throw new Exception("Never get here.");
			}
		}

		public sealed override double GetValleyFactorDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Proteins:
					return 1.1;
				case MsDataType.Metabolites:
					return 1.5;
				default:
					throw new Exception("Never get here.");
			}
		}

		public sealed override double GetIsotopeValleyFactorDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
					return 1.5;
				case MsDataType.Peptides:
					return 1.2;
				case MsDataType.Metabolites:
					return 1.5;
				default:
					throw new Exception("Never get here.");
			}
		}
		public sealed override int GetMinPeakLengthDefault(MsDataType dataType) {
			switch (dataType) {
				case MsDataType.Peptides:
				case MsDataType.Proteins:
				case MsDataType.Metabolites:
					return 2;
				default:
					throw new Exception("Never get here.");
			}
		}

		public sealed override int GetDiaMinPeakLengthDefault(MsDataType dataType) {
			switch (dataType) {
				case MsDataType.Peptides:
				case MsDataType.Proteins:
				case MsDataType.Metabolites:
					return 1;
				default:
					throw new Exception("Never get here.");
			}
		}


		public sealed override double PrecursorToleranceFirstSearchDefault => 20;
		public sealed override double PrecursorToleranceMainSearchDefault => 10;
		public sealed override bool CheckMassDeficitDefault => false;
		public sealed override double MinScoreForCalibrationDefault => 40;

		public sealed override bool GetAdvancedPeakSplittingDefault(MsDataType dataType){
			return true;
		}

		public sealed override int DiaNumNonleadingMatchesDefault => 1;
		public sealed override bool DiaChargeNormalizationLibraryDefault => false;
		public sealed override bool DiaChargeNormalizationSampleDefault => false;
		public sealed override IntensityDetermination GetIntensityDeterminationDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
				case MsDataType.Proteins:
					return IntensityDetermination.SumTotal;
				default:
					throw new Exception("Never get here.");
			}
		}
		public sealed override bool DiaBackgroundSubtractionDefault => false;
		public sealed override double DiaBackgroundSubtractionQuantileDefault => 0.5;
		public sealed override double DiaBackgroundSubtractionFactorDefault => 4;
		public sealed override double DiaInitialPrecMassTolPpmDefault => 20;
		public sealed override double DiaInitialFragMassTolPpmDefault => 25;
		public sealed override LfqRatioType DiaLfqRatioTypeDefault => LfqRatioType.Median;
		public sealed override double GetCentroidMatchTolDefault(MsDataType dataType) {
			return 10;
		}

		public sealed override float[] SmoothIntensityProfile(float[] origProfile) {
			return ArrayUtils.SmoothMean(origProfile, 1);
		}
		public sealed override double DiaTopMsmsIntensityQuantileForQuantDefault => 0.85;
	}
}