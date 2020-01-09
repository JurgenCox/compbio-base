using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class RegressionWithRankingMultiSizes{
		private readonly RegressionMethod classifier;
		private readonly RegressionFeatureRankingMethod ranker;
		private readonly double reductionFactor;
		private readonly int maxFeatures;
		private readonly Parameters classifierParam;
		private readonly Parameters rankerParam;

		public RegressionWithRankingMultiSizes(RegressionMethod classifier, RegressionFeatureRankingMethod ranker,
			double reductionFactor, int maxFeatures, Parameters classifierParam, Parameters rankerParam){
			this.classifier = classifier;
			this.ranker = ranker;
			this.reductionFactor = reductionFactor;
			this.maxFeatures = maxFeatures;
			this.classifierParam = classifierParam;
			this.rankerParam = rankerParam;
		}

		public RegressionModel[] Train(BaseVector[] x, double[] y, IGroupDataProvider data){
			int[] o = ranker.Rank(x, y, rankerParam, data, 1);
			int[] sizes = GetSizes(x[0].Length, reductionFactor, maxFeatures);
			RegressionModel[] result = new RegressionModel[sizes.Length];
			for (int i = 0; i < result.Length; i++){
				if (i == 0 && sizes[0] == x[0].Length){
					result[0] = classifier.Train(x, y, classifierParam, 1);
				} else{
					int[] inds = ArrayUtils.SubArray(o, sizes[i]);
					result[i] =
						new RegressionOnSubFeatures(
							classifier.Train(ClassificationWithRanking.ExtractFeatures(x, inds), y, classifierParam, 1), inds);
				}
			}
			return result;
		}

		private static int[] GetSizes(int n, double reductionFactor, int maxFeatures){
			List<int> result = new List<int>();
			int current = Math.Min(n, maxFeatures);
			while (true){
				result.Add(current);
				current = Math.Min((int) Math.Round(current/reductionFactor), current - 1);
				if (current < 1){
					break;
				}
			}
			return result.ToArray();
		}

		public int[] GetSizes(int n){
			return GetSizes(n, reductionFactor, maxFeatures);
		}
	}
}