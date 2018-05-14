using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class RegressionOnSubFeatures : RegressionModel{
		private readonly RegressionModel regressionModel;
		private readonly int[] featureInds;

		public RegressionOnSubFeatures(RegressionModel regressionModel, int[] featureInds){
			this.regressionModel = regressionModel;
			this.featureInds = featureInds;
		}

		public override double Predict(BaseVector x){
			return regressionModel.Predict(x.SubArray(featureInds));
		}
	}
}