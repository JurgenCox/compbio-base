using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class ClassificationOnSubFeatures : ClassificationModel{
		private readonly ClassificationModel classifier;
		private readonly int[] featureInds;

		public ClassificationOnSubFeatures(ClassificationModel classifier, int[] featureInds){
			this.classifier = classifier;
			this.featureInds = featureInds;
		}

		public override double[] PredictStrength(BaseVector x){
			return classifier.PredictStrength(x.SubArray(featureInds));
		}
	}
}