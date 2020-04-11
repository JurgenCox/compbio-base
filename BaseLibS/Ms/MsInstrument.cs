using System;

namespace BaseLibS.Ms{
	[Serializable]
	public abstract class MsInstrument{
		protected MsInstrument(int index){
			Index = index;
		}

		public int Index{ get; }
		public abstract string Name{ get; }
		public abstract double GetIsotopeMatchTolDefault(MsDataType dataType);
		public abstract bool IsotopeMatchTolInPpmDefault{ get; }
		public abstract bool RecalibrationInPpmDefault{ get; }
		public abstract double GetCentroidMatchTolDefault(MsDataType dataType);
		public abstract bool CentroidMatchTolInPpmDefault{ get; }
		public abstract double CentroidHalfWidthDefault{ get; }
		public abstract bool CentroidHalfWidthInPpmDefault{ get; }
		public abstract double GetValleyFactorDefault(MsDataType dataType);
		public abstract double GetIsotopeValleyFactorDefault(MsDataType dataType);
		public abstract double IsotopeTimeCorrelationDefault{ get; }
		public abstract double TheorIsotopeCorrelationDefault{ get; }
		public abstract float[] SmoothIntensityProfile(float[] origProfile);
		public abstract double IntensityThresholdDefault{ get; }
		public abstract bool IntensityDependentCalibrationDefault{ get; }
		public abstract double PrecursorToleranceFirstSearchDefault{ get; }
		public abstract double PrecursorToleranceMainSearchDefault{ get; }
		public abstract bool PrecursorToleranceUnitPpmDefault{ get; }
		public abstract int GetMinPeakLengthDefault(MsDataType dataType);
		public abstract int GetDiaMinPeakLengthDefault(MsDataType dataType);
		public abstract int GetMaxChargeDefault(MsDataType dataType);
		public abstract bool IndividualPeptideMassTolerancesDefault{ get; }
		public abstract bool UseMs1CentroidsDefault{ get; }
		public abstract bool UseMs2CentroidsDefault{ get; }
		public abstract double MinScoreForCalibrationDefault{ get; }
		public abstract bool GetAdvancedPeakSplittingDefault(MsDataType dataType);
		public abstract IntensityDetermination GetIntensityDeterminationDefault(MsDataType dataType);
		public abstract bool CutPeaksDefault{ get; }
		public abstract int GapScansDefault{ get; }
		public abstract bool CheckMassDeficitDefault{ get; }
		public abstract CentroidPosition CentroidPosition{ get; }
		public double DiaCorrThresholdFeatureClusteringDefault => 0.85;
		public double DiaInitialPrecMassTolPpmDefault => 20;
		public double DiaPrecTolPpmFeatureClusteringDefault => 2;
		public int DiaScoreNDefault => 7;
		public double DiaInitialFragMassTolPpmDefault => 20;
		public double DiaFragTolPpmFeatureClusteringDefault => 2;
		public double DiaMinScoreDefault => 1.99;
		public double DiaXgBoostBaseScoreDefault => 0.4;
		public double DiaXgBoostSubSampleDefault => 0.65;
		public DiaQuantMethod DiaQuantMethodDefault => DiaQuantMethod.MixedLfqSplit;
		public int DiaTopNFragmentsForQuantDefault => 10;
		public double DiaMinMsmsIntensityForQuantDefault => 0;
		public double DiaTopMsmsIntensityQuantileForQuantDefault => 0.85;
		public PrecursorFilterType DiaPrecursorFilterTypeDefault => PrecursorFilterType.None;

		public DiaXgBoostLearningObjective DiaXgBoostLearningObjectiveDefault =>
			DiaXgBoostLearningObjective.Binarylogisticraw;

		public int DiaMinFragmentOverlapScoreDefault => 1;
		public double DiaMinPrecursorScoreDefault => 0.5;
		public double DiaMinProfileCorrelationDefault => 0;
		public int DiaXgBoostMinChildWeightDefault => 3;
		public int DiaXgBoostMaximumTreeDepthDefault => 25;
		public int DiaXgBoostEstimatorsDefault => 260;
		public bool DiaGlobalMlDefault => true;

		public override string ToString(){
			return Name;
		}
	}
}