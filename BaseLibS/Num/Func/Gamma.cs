using System;

namespace BaseLibS.Num.Func{
	public static class Gamma{
		/// <summary>
		/// Approximation to the natural logarithm of the gamma function.
		/// </summary>
		/// <param name="xx">The argument is required to be positive.</param>
		/// <returns>Ln of the gamma function value.</returns>
		public static double LnValue(double xx){
			double x;
			int j;
			double y = x = xx;
			double tmp = x + 5.5;
			tmp -= (x + 0.5)*Math.Log(tmp);
			double ser = 1.000000000190015;
			for (j = 0; j <= 5; j++){
				ser += gammlnCof[j]/++y;
			}
			return -tmp + Math.Log(2.5066282746310005*ser/x);
		}

		private static readonly double[] gammlnCof ={
			76.18009172947146, -86.50532032941677, 24.01409824083091,
			-1.231739572450155, 0.1208650973866179e-2, -0.5395239384953e-5
		};
	}
}