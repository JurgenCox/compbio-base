using BaseLibS.Num;
namespace NumPluginBase.RegressionRank{
	public class NegativeCorrelationFeatureRanking : CorrelationFeatureRanking{
		public override double CalcScore(double[] xx, double[] yy){
			return 1 + ArrayUtils.Correlation(xx, yy);
		}
		public override string Name => "-Pearson correlation";
		public override float DisplayRank => 2;
	}
}