using System;
using BaseLibS.Num;
namespace NumPluginBase.RegressionRank{
	public class AbsCorrelationFeatureRanking : CorrelationFeatureRanking{
		public override double CalcScore(double[] xx, double[] yy){
			return 1 - Math.Abs(ArrayUtils.Correlation(xx, yy));
		}
		public override string Name => "Abs(Pearson correlation)";
		public override float DisplayRank => 0;
	}
}