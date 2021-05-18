namespace BaseLibS.Num.Test.Univariate.NSample{
	public abstract class MultipleSampleTest : UnivariateTest{
		public abstract double Test(double[][] data, out double statistic, double s0, out double pvalS0, out double[] gmeans);

		public double Test(double[][] data, out double statistic, double s0, out double pvalS0){
			return Test(data, out statistic, s0, out pvalS0, out _);
		}

		public double Test(double[][] data, out double statistic){
			return Test(data, out statistic, 0.0, out _);
		}
	}
}