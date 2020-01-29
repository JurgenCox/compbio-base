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
		public abstract float[] SmoothIntensityProfile(float[] origProfile);
		public abstract CentroidApproach Centroiding { get; }
		public abstract double IntensityThresholdDefault { get; }
		public abstract bool IntensityDependentCalibrationDefault { get; }
		public abstract double PrecursorToleranceFirstSearchDefault { get; }
		public abstract double PrecursorToleranceMainSearchDefault { get; }
		public abstract bool PrecursorToleranceUnitPpmDefault { get; }
		public abstract int GetMinPeakLengthDefault(MsDataType dataType);
		public abstract int GetDiaMinPeakLengthDefault(MsDataType dataType);
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
		public abstract CentroidPosition CentroidPosition { get; }
		public abstract double DiaCorrThresholdFeatureClusteringDefault { get; }
		public abstract double DiaInitialPrecMassTolPpmDefault { get; }
		public abstract double DiaPrecTolPpmFeatureClusteringDefault { get; }
		public abstract int DiaScoreNDefault { get; }
		public abstract double DiaInitialFragMassTolPpmDefault { get; }
		public abstract double DiaFragTolPpmFeatureClusteringDefault { get; }
		public abstract double DiaMinScoreDefault { get; }
		public abstract DiaQuantMethod DiaQuantMethodDefault { get; }
		public abstract int DiaTopNFragmentsForQuantDefault { get; }
		public abstract PrecursorFilterType DiaPrecursorFilterTypeDefault { get; }
        //public abstract DiaXgBoostBoosterType DiaXgBoostBoosterTypeDefault(MsDataType dataType);
        public abstract DiaXgBoostLearningObjective DiaXgBoostLearningObjectiveDefault { get; }
		public abstract int DiaMinOverlapDefault { get; }
		public abstract double DiaMinPrecursorScoreDefault { get; }
		public abstract double DiaXgBoostBaseScoreDefault { get; }
		public abstract double DiaXgBoostSubSampleDefault { get; }
		public abstract double DiaMinProfileCorrelationDefault { get; }
		public abstract bool DiaGlobalMlDefault { get; }
	    public abstract int DiaXgBoostMinChildWeightDefault { get; }
	    public abstract int DiaXgBoostMaximumTreeDepthDefault { get; }
	    public abstract int DiaXgBoostEstimatorsDefault { get; }

        public override string ToString() {
			return Name;
		}
	}
}