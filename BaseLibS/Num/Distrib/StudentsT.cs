using System;
using BaseLibS.Num.Func;

namespace BaseLibS.Num.Distrib{
	public static class StudentsT{
		private const double epsilon = 5E-16;
/*************************************************************************
Student's t distribution

Computes the integral from minus infinity to t of the Student
t distribution with integer k > 0 degrees of freedom:

									 t
									 -
									| |
			 -                      |         2   -(k+1)/2
			| ( (k+1)/2 )           |  (     x   )
	  ----------------------        |  ( 1 + --- )        dx
					-               |  (      k  )
	  sqrt( k pi ) | ( k/2 )        |
								  | |
								   -
								  -inf.

Relation to incomplete beta integral:

	   1 - stdtr(k,t) = 0.5 * incbet( k/2, 1/2, z )
where
	   z = k/(k + t**2).

For t < -2, this is the method of computation.  For higher t,
a direct method is derived from integration by parts.
Since the function is symmetric about t=0, the area under the
right tail of the density is found by calling the function
with -t instead of t.

ACCURACY:

Tested at random 1 <= k <= 25.  The "domain" refers to t.
					 Relative error:
arithmetic   domain     # trials      peak         rms
   IEEE     -100,-2      50000       5.9e-15     1.4e-15
   IEEE     -2,100      500000       2.7e-15     4.9e-17

Cephes Math Library Release 2.8:  June, 2000
Copyright 1984, 1987, 1995, 2000 by Stephen L. Moshier
*************************************************************************/

		public static double Cumulative(int k, double t){
			if (k <= 0){
				throw new Exception("Negative value for k in Student's distribution");
			}
			if (t == 0){
				return 0.5;
			}
			double rk;
			double z;
			if (t < -2.0){
				rk = k;
				z = rk/(rk + t*t);
				return 0.5*IncompleteBeta.Value(0.5*rk, 0.5, z);
			}
			double x;
			if (t < 0){
				x = -t;
			} else{
				x = t;
			}
			rk = k;
			z = 1.0 + x*x/rk;
			double p;
			if (k%2 != 0){
				double xsqk = x/Math.Sqrt(rk);
				p = Math.Atan(xsqk);
				if (k > 1){
					double f = 1.0;
					double tz = 1.0;
					int j = 3;
					while (j <= k - 2 & tz/f > epsilon){
						tz = tz*((j - 1)/(z*j));
						f = f + tz;
						j = j + 2;
					}
					p = p + f*xsqk/z;
				}
				p = p*2.0/Math.PI;
			} else{
				double f = 1.0;
				double tz = 1.0;
				int j = 2;
				while (j <= k - 2 & tz/f > epsilon){
					tz = tz*((j - 1)/(z*j));
					f = f + tz;
					j = j + 2;
				}
				p = f*x/Math.Sqrt(z*rk);
			}
			if (t < 0){
				p = -p;
			}
			return 0.5 + 0.5*p;
		}
	}
}