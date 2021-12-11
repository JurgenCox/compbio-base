﻿using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class AgilentQtof : QtofInstrument{
		public AgilentQtof(int index) : base(index){ }
		public override string Name => "Agilent Q-TOF";
		public override double IntensityThresholdMs1Default => 0;
		public override double IntensityThresholdMs2Default => 0;
		public override bool UseMs1CentroidsDefault => true;
		public override bool UseMs2CentroidsDefault => true;

		public override int GetDiaMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Proteins:
					return 2;
				case MsDataType.Metabolites:
					return 5;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override int GetMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Peptides:
				case MsDataType.Proteins:
					return 3;
				case MsDataType.Metabolites:
					return 5;
				default:
					throw new Exception("Never get here.");
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