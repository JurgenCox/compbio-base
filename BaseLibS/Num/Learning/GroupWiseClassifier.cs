using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class GroupWiseClassifier : ClassificationModel{
		private readonly ClassificationModel[] classifiers;

		public GroupWiseClassifier(ClassificationModel[] classifiers){
			this.classifiers = classifiers;
		}

		public override double[] PredictStrength(BaseVector x) {
			double[] result = new double[classifiers.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = classifiers[i].PredictStrength(x)[0];
			}
			return result;
		}
	}
}