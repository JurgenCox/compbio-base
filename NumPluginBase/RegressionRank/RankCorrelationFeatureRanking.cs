using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.RegressionRank{
	public abstract class RankCorrelationFeatureRanking : RegressionFeatureRankingMethod{
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
			return s.Order();
		}

		public abstract double CalcScore(float[] xx, float[] yy);
		public override Parameters GetParameters(IGroupDataProvider data) { return new Parameters(); }
		public override string Description => "";
		public override bool IsActive => true;
	}
}