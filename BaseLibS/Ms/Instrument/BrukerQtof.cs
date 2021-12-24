﻿using System;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	[Serializable]
	public class BrukerQtof : QtofInstrument{
		public BrukerQtof(int index) : base(index){ }
		public override string Name => "Bruker Q-TOF";
		public override double IntensityThresholdMs1Default => 30;
		public override double IntensityThresholdMs2Default => 30;
		public override bool UseMs1CentroidsDefault => true;
		public override bool UseMs2CentroidsDefault => true;

		public override int GetMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
				case MsDataType.Peptides:
					return 3;
				case MsDataType.Metabolites:
					return 5;
				default:
					throw new Exception("Never get here.");
			}
		}

		public override int GetDiaMinPeakLengthDefault(MsDataType dataType){
			switch (dataType){
				case MsDataType.Proteins:
				case MsDataType.Peptides:
					return 2;
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