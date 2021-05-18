namespace BaseLibS.Num.Test.Univariate.OneSample{
	public abstract class OneSampleTest : UnivariateTest{
		public abstract void Test(double[] data, double mean, out double statistic, out double statisticS0,
			out double bothTails, out double leftTail, out double rightTail, out double difference, double s0,
			out double bothTailsS0, out double leftTailS0, out double rightTailS0);

		public void Test(double[] data, double mean, out double statistic, out double bothTails, out double leftTail,
			out double rightTail){
			const double s0 = 0;
			Test(data, mean, out statistic, out _, out bothTails, out leftTail, out rightTail, out _, s0, out _, out _,
				out _);
		}

		public abstract double[][] CalcCurve(double p2, double df, double s0, double maxD, TestSide side);
	}
}