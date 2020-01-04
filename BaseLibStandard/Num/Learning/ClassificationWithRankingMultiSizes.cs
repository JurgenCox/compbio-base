using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class ClassificationWithRankingMultiSizes{
		private readonly ClassificationMethod classifier;
		private readonly ClassificationFeatureRankingMethod ranker;
		private readonly double reductionFactor;
		private readonly int maxFeatures;
		private readonly Parameters classifierParam;
		private readonly Parameters rankerParam;

		public ClassificationWithRankingMultiSizes(ClassificationMethod classifier,
			ClassificationFeatureRankingMethod ranker, double reductionFactor, int maxFeatures, Parameters classifierParam,
			Parameters rankerParam){
			this.classifier = classifier;
			this.ranker = ranker;
			this.reductionFactor = reductionFactor;
			this.maxFeatures = maxFeatures;
			this.classifierParam = classifierParam;
			this.rankerParam = rankerParam;
		}

		public ClassificationModel[] Train(BaseVector[] x, int[][] y, int ngroups, int[] rankedFeatures,
			IGroupDataProvider data){
			int[] o = ranker.Rank(x, y, ngroups, rankerParam, data, 1); //TODO
			Array.Copy(o, rankedFeatures, o.Length);
			int[] sizes = GetSizes(x[0].Length, reductionFactor, maxFeatures);
			ClassificationModel[] result = new ClassificationModel[sizes.Length];
			for (int i = 0; i < result.Length; i++){
				if (i == 0 && sizes[0] == x[0].Length){
					result[0] = classifier.Train(x, y, ngroups, classifierParam, 1);
				} else{
					int[] inds = ArrayUtils.SubArray(o, sizes[i]);
					result[i] =
						new ClassificationOnSubFeatures(
							classifier.Train(ClassificationWithRanking.ExtractFeatures(x, inds), y, ngroups, classifierParam, 1), inds);
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