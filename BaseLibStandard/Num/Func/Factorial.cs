using System;

namespace BaseLibS.Num.Func{
	public static class Factorial{
		private static readonly double[] aaa = new double[15001];

		public static double LnValue(long n){
			if (n < 0){
				throw new Exception("Negative argument for factorial.");
			}
			if (n <= 1){
				return 0.0;
			}
			if (n <= 15000){
				double an = aaa[n];
				return an != 0 ? an : (aaa[n] = Gamma.LnValue(n + 1.0));
			}
			return Gamma.LnValue(n + 1.0);
		}

		public static double BinomialCoeff(long n, long k){
			return Math.Round(Math.Exp(LnValue(n) - LnValue(k) - LnValue(n - k)));
		}

		public static double Multinomial(int n, int[] partition){
			return Math.Exp(LnMultinomial(n, partition));
		}

		public static double LnMultinomial(int a, int[] bs){
			double result = Gamma.LnValue(a + 1);
			foreach (int t in bs){
				result -= Gamma.LnValue(t + 1);
			}
			return result;
		}
	}
}