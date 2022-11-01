using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class ThermoFtmsInstrument : MsInstrument{
		public ThermoFtmsInstrument(int index) : base(index){ }
		public override string Name => "Orbitrap";
		public override double IsotopeTimeCorrelationDefault => 0.6;
		public override double TheorIsotopeCorrelationDefault => 0.6;
		public override double IntensityThresholdMs1Default => 0;
		public override double IntensityThresholdMs2Default => 0;
		public override bool IntensityDependentCalibrationDefault => false;
		public override bool IsotopeMatchTolInPpmDefault => true;

		public override double DiaMinMsmsIntensityForQuantDefault => 0;
		public override double DiaTopMsmsIntensityQuantileForQuantDefault => 0.85;
		public override double GetCentroidMatchTolDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
					return 8;
				case MsDataType.Proteins:
					return 11;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override double PrecursorToleranceFirstSearchDefault => 20;
		public override double PrecursorToleranceMainSearchDefault => 4.5;
		public override bool UseMs1CentroidsDefault => false;
		public override bool UseMs2CentroidsDefault => false;
		public override double MinScoreForCalibrationDefault => 70;
		public override bool CheckMassDeficitDefault => true;
		public override int DiaTopNFragmentsForQuantDefault => 10;

		public override bool GetAdvancedPeakSplittingDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
					return false;
				case MsDataType.Proteins:
					return true;
				default:
					throw new Exception("Never get here.");
			}
		}


		public override double GetIsotopeMatchTolDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
					return 2;
				case MsDataType.Proteins:
					return 10;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override IntensityDetermination GetIntensityDeterminationDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
					return IntensityDetermination.Maximum;
				case MsDataType.Metabolites:
				case MsDataType.Proteins:
					return IntensityDetermination.Maximum;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override int GetMaxChargeDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
					return 7;
				case MsDataType.Proteins:
					return 25;
				case MsDataType.Metabolites:
					return 2;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override double GetValleyFactorDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
					return 1.1;
				case MsDataType.Peptides:
					return 1.4;
				case MsDataType.Metabolites:
					return 1.4;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override double GetIsotopeValleyFactorDefault(MsDataType dataType){
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

		public override int GetMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
					return 2;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override int GetDiaMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
				case MsDataType.Peptides:
				case MsDataType.Metabolites:
					return 1;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override float[] SmoothIntensityProfile(float[] origProfile){
			float[] result = ArrayUtils.SmoothMedian(origProfile, 1);
			return ArrayUtils.SmoothMean(result, 2);
		}
		public override double DiaInitialPrecMassTolPpmDefault => 20;
		public override double DiaInitialFragMassTolPpmDefault => 20;
		public override bool DiaBackgroundSubtractionDefault => false;
		public override double DiaBackgroundSubtractionQuantileDefault => 0.5;
		public override double DiaBackgroundSubtractionFactorDefault => 4;
		public override LfqRatioType DiaLfqRatioTypeDefault => LfqRatioType.Median;
		public override bool DiaChargeNormalizationLibraryDefault => true;
		public override bool DiaChargeNormalizationSampleDefault => true;
	}
}