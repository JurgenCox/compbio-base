using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class RegressionWithRanking{
		private readonly RegressionMethod regressionMethod;
		private readonly RegressionFeatureRankingMethod ranker;
		private readonly int nfeatures;
		private readonly Parameters regressionParam;
		private readonly Parameters rankerParam;

		public RegressionWithRanking(RegressionMethod regressionMethod, RegressionFeatureRankingMethod ranker, int nfeatures,
			Parameters regressionParam, Parameters rankerParam){
			this.regressionMethod = regressionMethod;
			this.ranker = ranker;
			this.nfeatures = nfeatures;
			this.regressionParam = regressionParam;
			this.rankerParam = rankerParam;
		}

		public RegressionModel Train(BaseVector[] x, double[] y, IGroupDataProvider data){
			if (ranker == null || nfeatures >= x[0].Length){
				return regressionMethod.Train(x, y, regressionParam, 1);
			}
			int[] o = ranker.Rank(x, y, rankerParam, data, 1);
			int[] inds = nfeatures < o.Length ? ArrayUtils.SubArray(o, nfeatures) : o;
			return
				new RegressionOnSubFeatures(
					regressionMethod.Train(ClassificationWithRanking.ExtractFeatures(x, inds), y, regressionParam, 1), inds);
		}
	}
}