using System;
using BaseLibS.Num.Distrib;

namespace BaseLibS.Num.Test{
	public class PearsonCorrelationTest{
		public static void Test(double r, int n, out double bothtails, out double lefttail, out double righttail){
			if (r >= 1){
				bothtails = 0.0;
				lefttail = 1.0;
				righttail = 0.0;
				return;
			}
			if (r <= -1){
				bothtails = 0.0;
				lefttail = 0.0;
				righttail = 1.0;
				return;
			}
			if (n < 3){
				bothtails = 1.0;
				lefttail = 1.0;
				righttail = 1.0;
				return;
			}
			double t = r*Math.Sqrt((n - 2)/(1 - r*r));
			double p = StudentsT.Cumulative(n - 2, t);
			bothtails = 2*Math.Min(p, 1 - p);
			lefttail = p;
			righttail = 1 - p;
		}
	}
}