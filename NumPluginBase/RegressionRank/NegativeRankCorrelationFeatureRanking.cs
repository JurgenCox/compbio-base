using BaseLibS.Num;

namespace NumPluginBase.RegressionRank{
	public class NegativeRankCorrelationFeatureRanking : RankCorrelationFeatureRanking {
		public override double CalcScore(float[] xx, float[] yy) {
			return 1 + ArrayUtils.Correlation(xx, yy);
		}
		public override string Name => "Spearman correlation";
		public override float DisplayRank => 4;
	}
}