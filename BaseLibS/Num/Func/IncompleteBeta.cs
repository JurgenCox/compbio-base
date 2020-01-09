using System;

namespace BaseLibS.Num.Func{
	public static class IncompleteBeta{
		/// <summary>
		/// Approximates the the incomplete beta function.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="x"></param>
		/// <returns>Returns the incomplete beta function value.</returns>
		public static double Value(double a, double b, double x){
			double bt;
			if (x < 0.0 || x > 1.0){
				throw new Exception("Bad x in routine betai");
			}
			if (x == 0.0 || x == 1.0){
				bt = 0.0;
			} else{
				bt = Math.Exp(Gamma.LnValue(a + b) - Gamma.LnValue(a) - Gamma.LnValue(b) + a*Math.Log(x) + b*Math.Log(1.0 - x));
			}
			if (x < (a + 1.0)/(a + b + 2.0)){
				return bt*Betacf(a, b, x)/a;
			}
			return 1.0 - bt*Betacf(b, a, 1.0 - x)/b;
		}

		private const int betacfMaxit = 100;
		private const double betacfEps = 3.0e-7;
		private const double betacfFpmin = 1.0e-30;

		/// <summary>
		/// Evaluates the continued fraction used for the approximation of the incomplete beta function.
		/// </summary>
		private static double Betacf(double a, double b, double x){
			int m;
			double qab = a + b;
			double qap = a + 1.0;
			double qam = a - 1.0;
			double c = 1.0;
			double d = 1.0 - qab*x/qap;
			if (Math.Abs(d) < betacfFpmin){
				d = betacfFpmin;
			}
			d = 1.0/d;
			double h = d;
			for (m = 1; m <= betacfMaxit; m++){
				int m2 = 2*m;
				double aa = m*(b - m)*x/((qam + m2)*(a + m2));
				d = 1.0 + aa*d;
				if (Math.Abs(d) < betacfFpmin){
					d = betacfFpmin;
				}
				c = 1.0 + aa/c;
				if (Math.Abs(c) < betacfFpmin){
					c = betacfFpmin;
				}
				d = 1.0/d;
				h *= d*c;
				aa = -(a + m)*(qab + m)*x/((a + m2)*(qap + m2));
				d = 1.0 + aa*d;
				if (Math.Abs(d) < betacfFpmin){
					d = betacfFpmin;
				}
				c = 1.0 + aa/c;
				if (Math.Abs(c) < betacfFpmin){
					c = betacfFpmin;
				}
				d = 1.0/d;
				double del = d*c;
				h *= del;
				if (Math.Abs(del - 1.0) < betacfEps){
					break;
				}
			}
			if (m > betacfMaxit){
				throw new Exception("a or b too big, or MAXIT too small in betacf");
			}
			return h;
		}
	}
}