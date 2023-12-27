using System;
using BaseLibS.Num;

namespace NumPluginBase.RegressionRank{
	public class AbsRankCorrelationFeatureRanking : RankCorrelationFeatureRanking {
		public override double CalcScore(float[] xx, float[] yy) {
			return 1 - Math.Abs(ArrayUtils.Correlation(xx, yy));
		}
		public override string Name => "Abs(Spearman correlation)";
		public override float DisplayRank => 3;
	}
}