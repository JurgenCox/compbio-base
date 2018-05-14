using System;

namespace BaseLibS.Ms {
	[Serializable]
	public abstract class MsInstrument {
		protected MsInstrument(int index) {
			Index = index;
		}
		public int Index { get; }
		public abstract string Name { get; }
		public abstract double GetIsotopeMatchTolDefault(MsDataType dataType);
		public abstract bool IsotopeMatchTolInPpmDefault { get; }
		public abstract bool RecalibrationInPpmDefault { get; }
		public abstract double GetCentroidMatchTolDefault(MsDataType dataType);
		public abstract bool CentroidMatchTolInPpmDefault { get; }
		public abstract double CentroidHalfWidthDefault { get; }
		public abstract bool CentroidHalfWidthInPpmDefault { get; }
		public abstract double GetValleyFactorDefault(MsDataType dataType);
		public abstract double GetIsotopeValleyFactorDefault(MsDataType dataType);
		public abstract double IsotopeTimeCorrelationDefault { get; }
		public abstract double TheorIsotopeCorrelationDefault { get; }
		public abstract double[] SmoothIntensityProfile(double[] origProfile);
		public abstract CentroidApproach Centroiding { get; }
		public abstract double IntensityThresholdDefault { get; }
		public abstract bool IntensityDependentCalibrationDefault { get; }
		public abstract double PrecursorToleranceFirstSearchDefault { get; }
		public abstract double PrecursorToleranceMainSearchDefault { get; }
		public abstract bool PrecursorToleranceUnitPpmDefault { get; }
		public abstract int GetMinPeakLengthDefault(MsDataType dataType);
		public abstract int GetMaxChargeDefault(MsDataType dataType);
		public abstract bool IndividualPeptideMassTolerancesDefault { get; }
		public abstract bool UseMs1CentroidsDefault { get; }
		public abstract bool UseMs2CentroidsDefault { get; }
		public abstract double MinScoreForCalibrationDefault { get; }
		public abstract bool GetAdvancedPeakSplittingDefault(MsDataType dataType);
		public abstract IntensityDetermination GetIntensityDeterminationDefault(MsDataType dataType);
		public abstract bool CutPeaksDefault { get; }
		public abstract int GapScansDefault { get; }
		public abstract bool CheckMassDeficitDefault { get; }

		public override string ToString() {
			return Name;
		}
	}
}