using System;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public abstract class MsInstrument{
		protected MsInstrument(int index){
			Index = index;
		}

		public int Index{ get; }
		public abstract double IsotopeTimeCorrelationDefault { get; }
		public abstract double TheorIsotopeCorrelationDefault { get; }
		public bool RecalibrationInPpmDefault => true;
		public abstract string Name{ get; }
		public abstract double GetIsotopeMatchTolDefault(MsDataType dataType);
		public abstract bool IsotopeMatchTolInPpmDefault{ get; }
		public abstract double GetCentroidMatchTolDefault(MsDataType dataType);
		public abstract double GetValleyFactorDefault(MsDataType dataType);
		public abstract double GetIsotopeValleyFactorDefault(MsDataType dataType);
		public abstract double IntensityThresholdMs1DdaDefault { get; }
		public abstract double IntensityThresholdMs1DiaDefault { get; }
		public abstract double IntensityThresholdMs2Default { get; }
		public abstract bool IntensityDependentCalibrationDefault{ get; }
		public abstract double PrecursorToleranceFirstSearchDefault{ get; }
		public abstract double PrecursorToleranceMainSearchDefault{ get; }
		public abstract int GetMinPeakLengthDefault(MsDataType dataType);
		public abstract int GetDiaMinPeakLengthDefault(MsDataType dataType);
		public abstract int GetMaxChargeDefault(MsDataType dataType);
		public abstract bool UseMs1CentroidsDefault{ get; }
		public abstract bool UseMs2CentroidsDefault{ get; }
		public abstract double MinScoreForCalibrationDefault{ get; }
		public abstract bool GetAdvancedPeakSplittingDefault(MsDataType dataType);
		public abstract IntensityDetermination GetIntensityDeterminationDefault(MsDataType dataType);
		public abstract bool CheckMassDeficitDefault{ get; }
		public bool PrecursorToleranceUnitPpmDefault => true;
		public bool IndividualPeptideMassTolerancesDefault => true;
		public CentroidPosition CentroidPosition => CentroidPosition.Gaussian;
		public double CentroidHalfWidthDefault => 35;
		public bool CentroidHalfWidthInPpmDefault => true;
		public bool CentroidMatchTolInPpmDefault => true;
		public bool DiaAdaptiveMassAccuracyDefault => false;
		public double DiaMassWindowFactorDefault => 3.3;
		public double DiaCorrThresholdFeatureClusteringDefault => 0.85;
		public abstract double DiaInitialPrecMassTolPpmDefault{ get; }
		public abstract double DiaInitialFragMassTolPpmDefault { get; }
		public abstract bool DiaBackgroundSubtractionDefault { get; }
		public abstract double DiaBackgroundSubtractionQuantileDefault { get; }
		public abstract double DiaBackgroundSubtractionFactorDefault { get; }
		public abstract LfqRatioType DiaLfqRatioTypeDefault { get; }
		public double DiaPrecTolPpmFeatureClusteringDefault => 2;
		public int DiaScoreNDefault => 12;
		public int DiaScoreNAdditionalDefault => 5;
		public int DiaMaxTrainInstancesDefault => 1000000;
		public double DiaFragTolPpmFeatureClusteringDefault => 2;
		public double DiaMinScoreDefault => 1.99;
		public DiaQuantMethod DiaQuantMethodDefault => DiaQuantMethod.MixedLfqSplit;
		public DiaFeatureQuantMethod DiaFeatureQuantMethodDefault => DiaFeatureQuantMethod.Sum;
		public abstract int DiaTopNFragmentsForQuantDefault { get; }
		public abstract double DiaMinMsmsIntensityForQuantDefault { get; }
		public abstract double DiaTopMsmsIntensityQuantileForQuantDefault { get; }

		public DiaXgBoostLearningObjective DiaXgBoostLearningObjectiveDefault =>
			DiaXgBoostLearningObjective.Binarylogisticraw;

		public int DiaMinFragmentOverlapScoreDefault => 0;
		public double DiaMinPrecursorScoreDefault => 0.0;
		public bool DiaUseProfileCorrelationDefault => false;
		public bool DiaHardRtFilterDefault => true;
		public bool DiaConvertLibraryCharge2FragmentsDefault => false;
		public abstract bool DiaChargeNormalizationLibraryDefault { get; }
		public abstract bool DiaChargeNormalizationSampleDefault { get; }
		public double DiaMinPrecProfileCorrelationDefault => 0;
		public double DiaMinFragProfileCorrelationDefault => 0;
		public double DiaXgBoostBaseScoreDefault => 0.4;
		public double DiaXgBoostSubSampleDefault => 0.9;
		public double DiaXgBoostGammaDefault => 0.9;
		public int DiaXgBoostMaxDeltastepDefault => 3;
		public int DiaXgBoostMinChildWeightDefault => 9;
		public int DiaXgBoostMaximumTreeDepthDefault => 12;
		public int DiaXgBoostEstimatorsDefault => 580;

		public bool DiaGlobalMlDefault => true;
		public double DiaTransferQvalueDefault => 0.3;
		public bool CutPeaksDefault => true;
		public int GapScansDefault => 1;
		public bool DiaAdaptiveMlScoringDefault => false;
		public abstract float[] SmoothIntensityProfile(float[] origProfile);
		public bool DiaDeleteIntermediateResultsDefault => true;
		public abstract int DiaNumNonleadingMatchesDefault { get; }

		public override string ToString(){
			return Name;
		}
	}
}