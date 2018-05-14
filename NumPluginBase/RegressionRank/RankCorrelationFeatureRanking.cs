using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.RegressionRank{
	public class RankCorrelationFeatureRanking : RegressionFeatureRankingMethod{
		public override int[] Rank(BaseVector[] x, double[] y, Parameters param, IGroupDataProvider data, int nthreads){
			int nfeatures = x[0].Length;
			float[] yr = ArrayUtils.RankF(y);
			double[] s = new double[nfeatures];
			for (int i = 0; i < nfeatures; i++){
				float[] xx = new float[x.Length];
				for (int j = 0; j < xx.Length; j++){
					xx[j] = (float) x[j][i];
				}
				float[] xxr = ArrayUtils.RankF(xx);
				s[i] = CalcScore(xxr, yr);
			}
			return ArrayUtils.Order(s);
		}

		private static double CalcScore(IList<float> xx, IList<float> yy) { return 1 - Math.Abs(ArrayUtils.Correlation(xx, yy)); }
		public override Parameters GetParameters(IGroupDataProvider data) { return new Parameters(); }
		public override string Name => "Abs(Spearman correlation)";
		public override string Description => "";
		public override float DisplayRank => 1;
		public override bool IsActive => true;
	}
}