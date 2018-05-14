using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Data;
using BaseLibS.Num.Func;
using BaseLibS.Num.Test;
using BaseLibS.Util;

namespace BaseLibS.Num{
	public static class NumUtils{
		/// <summary>
		/// Creates all partitions of exactly <code>nItems</code> items into <code>nClasses</code> classes. 
		/// </summary>
		/// <param name="nItems">Number of items to be distributed into the classes.</param>
		/// <param name="nClasses">Number of classes</param>
		/// <returns></returns>
		public static int[][] GetPartitions(int nItems, int nClasses){
			return GetPartitions(nItems, nClasses, null, null);
		}

		/// <summary>
		/// Creates all partitions of exactly <code>nItems</code> items into <code>nClasses</code> classes. 
		/// </summary>
		/// <param name="nItems">Number of items to be distributed into the classes</param>
		/// <param name="nClasses">Number of classes</param>
		/// <param name="validPartition">Here you can add a criterion for the partition to be valid.</param>
		/// <param name="task">If <code>task != null</code> this action will be performed on all valid partitions. 
		/// In that case the return value will be <code>null</code>.</param>
		/// <returns></returns>
		public static int[][] GetPartitions(int nItems, int nClasses, Func<int[], bool> validPartition, Action<int[]> task){
			List<int[]> partitions = new List<int[]>();
			Partition(new TmpPartition(nItems), partitions, nClasses, validPartition, task);
			return task == null ? partitions.ToArray() : null;
		}

		private static void Partition(TmpPartition x, ICollection<int[]> allPartitions, int len,
			Func<int[], bool> validPartition, Action<int[]> task){
			if (x.remainder == 0 && x.partition.Count == len){
				int[] part = x.partition.ToArray();
				if (validPartition == null || validPartition(part)){
					if (task != null){
						task(part);
					} else{
						allPartitions.Add(part);
					}
				}
				return;
			}
			if (x.partition.Count == len){
				return;
			}
			for (int i = 0; i <= x.remainder; i++){
				Partition(x.Add(i), allPartitions, len, validPartition, task);
			}
		}

		public static int[][] GetCombinations(int n, int k, int max, out bool incomplete){
			List<int[]> result = new List<int[]>();
			if (k == 0){
				incomplete = false;
				result.Add(new int[0]);
				return result.ToArray();
			}
			Combination comb = new Combination(n, k);
			result.Add(comb.Data);
			incomplete = false;
			int count1 = 1;
			while ((comb = comb.Successor) != null){
				result.Add(comb.Data);
				count1++;
				if (count1 >= max){
					incomplete = true;
					break;
				}
			}
			return result.ToArray();
		}

		private class TmpPartition{
			public List<int> partition;
			public int remainder;
			private TmpPartition(){}

			internal TmpPartition(int n){
				remainder = n;
				partition = new List<int>(n);
			}

			internal TmpPartition Add(int a){
				TmpPartition result = new TmpPartition{remainder = remainder - a, partition = new List<int>()};
				result.partition.AddRange(partition);
				result.partition.Add(a);
				return result;
			}
		}

		public static int Fit(double[] theorWeights, double[] weights, out double maxCorr){
			maxCorr = -double.MaxValue;
			int maxCorrInd = int.MinValue;
			for (int i = -theorWeights.Length + 1; i < weights.Length - 1; i++){
				int start = Math.Min(i, 0);
				int end = Math.Max(i + theorWeights.Length, weights.Length);
				int len = end - start;
				double[] p1 = new double[len];
				double[] p2 = new double[len];
				for (int j = 0; j < theorWeights.Length; j++){
					p1[i + j - start] = theorWeights[j];
				}
				for (int j = 0; j < weights.Length; j++){
					p2[j - start] = weights[j];
				}
				double corr = ArrayUtils.Cosine(p1, p2);
				if (corr > maxCorr){
					maxCorr = corr;
					maxCorrInd = i;
				}
			}
			return -maxCorrInd;
		}

		public static double RoundSignificantDigits(double x, int n){
			if (x == 0){
				return 0;
			}
			if (double.IsNaN(x) || double.IsInfinity(x)){
				return x;
			}
			try{
				int sign = x > 0 ? 1 : -1;
				x = Math.Abs(x);
				int w = (int) Math.Ceiling(Math.Log(x)/Math.Log(10));
				if (w - n > 0){
					double fact = Math.Round(Math.Pow(10, w - n));
					x = Math.Round(x/fact)/Math.Pow(10, n - w);
					return x*sign;
				}
				if (w - n < 0){
					double fact = Math.Round(Math.Pow(10, n - w));
					if (double.IsInfinity(fact)){
						return 0;
					}
					x = Math.Round(x*fact)/Math.Pow(10, n - w);
					return x*sign;
				}
				x = Math.Round(x);
				return x*sign;
			} catch (OverflowException){
				return x;
			}
		}

		public static string RoundSignificantDigits2(double x, int n, double max){
			if (Math.Abs(x) < Math.Abs(max)*1e-8){
				return "0";
			}
			if (double.IsNaN(x) || double.IsInfinity(x)){
				return Parser.ToString(x);
			}
			try{
				string prefix = x < 0 ? "-" : "";
				x = Math.Abs(x);
				int w = (int) Math.Ceiling(Math.Log(x)/Math.Log(10));
				if (w - n > 0){
					double fact = Math.Round(Math.Pow(10, w - n));
					string s = Shift((long) Math.Round(x/fact), n - w);
					return prefix + s;
				}
				if (w - n < 0){
					double fact = Math.Round(Math.Pow(10, n - w));
					if (double.IsInfinity(fact)){
						return "0";
					}
					string s = Shift((long) Math.Round(x*fact), n - w);
					return prefix + s;
				}
				return prefix + Math.Round(x);
			} catch (OverflowException){
				return "" + x;
			}
		}

		private static string Shift(long l, int m){
			string s = "" + l;
			if (l == 0){
				return s;
			}
			if (m >= s.Length){
				string w = "0.";
				for (int i = 0; i < m - s.Length; i++){
					w += "0";
				}
				w += Remove0(s);
				return w;
			}
			if (m <= 0){
				for (int i = 0; i < -m; i++){
					s += "0";
				}
				return s;
			}
			string q1 = s.Substring(0, s.Length - m);
			string q2 = Remove0(s.Substring(s.Length - m, m));
			if (q2.Length == 0){
				return q1;
			}
			return q1 + "." + q2;
		}

		private static string Remove0(string s){
			int end = s.Length;
			while (end > 0 && s[end - 1] == '0'){
				end--;
			}
			return s.Substring(0, end);
		}

		public static void PolynomialFit(double[] x, double[] y, int degree, out double[] a){
			a = new double[degree + 1];
			LinFit2(x, y, a, delegate(double x1, double[] a1){
				double p = 1;
				for (int i = 0; i < degree + 1; i++){
					a1[i] = p;
					p *= x1;
				}
			});
		}

		public static void FitNonlin(double[] x, double[] y, double[] sig, double[] a, double[] amin, double[] amax,
			out double chisq, Func<double, double[], double> func, double epsilon, int nthreads){
			Func<double, double[], double[], int, double> f = (x1, a1, dyda, na) =>{
				double y1 = func(x1, a1);
				for (int i = 0; i < na; i++){
					dyda[i] = Dyda(x1, a1, func, i, epsilon);
				}
				return y1;
			};
			FitNonlin(x, y, sig, a, amin, amax, out chisq, f, nthreads);
		}

		public static double Dyda(double x, IList<double> a, Func<double, double[], double> func, int ind, double epsilon){
			double[] a1 = new double[a.Count];
			double[] a2 = new double[a.Count];
			for (int i = 0; i < a.Count; i++){
				a1[i] = a[i];
				a2[i] = a[i];
			}
			a1[ind] += epsilon/2.0;
			a2[ind] -= epsilon/2.0;
			return (func(x, a1) - func(x, a2))/epsilon;
		}

		public static void FitNonlin(double[] x, double[] y, double[] sig, double[] a, double[] amin, double[] amax,
			out double chisq, Func<double, double[], double[], int, double> func, int nthreads){
			int ndata = x.Length;
			if (sig == null){
				sig = new double[ndata];
				for (int i = 0; i < sig.Length; i++){
					sig[i] = 1;
				}
			}
			int ma = a.Length;
			double[,] covar = new double[ma, ma];
			double[,] alpha = new double[ma, ma];
			double alamda = -1;
			double ochisq = 0;
			double[,] oneda = null;
			int mfit = 0;
			double[] atry = null;
			double[] beta = null;
			double[] da = null;
			NumRecipes.Mrqmin(x, y, sig, ndata, a, amin, amax, covar, alpha, out chisq, func, ref alamda, ref ochisq, ref oneda,
				ref mfit, ref atry, ref beta, ref da, nthreads);
			int count1 = 0;
			while (alamda > 1e-60 && alamda < 1e60 && count1 < 500){
				NumRecipes.Mrqmin(x, y, sig, ndata, a, amin, amax, covar, alpha, out chisq, func, ref alamda, ref ochisq, ref oneda,
					ref mfit, ref atry, ref beta, ref da, nthreads);
				count1++;
			}
			alamda = 0;
			NumRecipes.Mrqmin(x, y, sig, ndata, a, amin, amax, covar, alpha, out chisq, func, ref alamda, ref ochisq, ref oneda,
				ref mfit, ref atry, ref beta, ref da, nthreads);
		}

		public static void LinFit2(double[] x, double[] y, double[] a, Action<double, double[]> funcs){
			double chisq;
			LinFit2(x, y, null, a, out chisq, funcs);
		}

		public static void LinFit2(double[] x, double[] y, double[] sig, double[] a, out double chisq,
			Action<double, double[]> funcs){
			double[,] covar;
			if (sig == null){
				sig = new double[x.Length];
				for (int i = 0; i < sig.Length; i++){
					sig[i] = 1E-2;
				}
			}
			Lfit(x, y, sig, a, out covar, out chisq, funcs);
		}

		public static void Lfit(double[] x, double[] y, double[] sig, double[] a, out double[,] covar, out double chisq,
			Action<double, double[]> funcs){
			int ndat = x.Length;
			int i, j;
			int l;
			int ma = a.Length;
			int mfit = a.Length;
			double[,] beta = new double[ma, 1];
			double[] afunc = new double[ma];
			covar = new double[ma, ma];
			for (i = 0; i < ndat; i++){
				funcs(x[i], afunc);
				double ym = y[i];
				double sig2I = 1.0/(sig[i]*sig[i]);
				for (l = 0; l < ma; l++){
					double wt = afunc[l]*sig2I;
					for (int m = 0; m <= l; m++){
						covar[l, m] += wt*afunc[m];
					}
					beta[l, 0] += ym*wt;
				}
			}
			for (j = 1; j < mfit; j++){
				int k;
				for (k = 0; k < j; k++){
					covar[k, j] = covar[j, k];
				}
			}
			Gaussj(covar, mfit, beta, 1);
			for (l = 0; l < ma; l++){
				a[l] = beta[l, 0];
			}
			chisq = 0.0;
			for (i = 0; i < ndat; i++){
				funcs(x[i], afunc);
				double sum = 0.0;
				for (j = 0; j < ma; j++){
					sum += a[j]*afunc[j];
				}
				chisq += ((y[i] - sum)/sig[i])*((y[i] - sum)/sig[i]);
			}
			Covsrt(covar);
		}

		public static void Gaussj(double[,] a, int n, double[,] b, int m){
			int icol = -1, irow = -1, j, k, l;
			double temp;
			int[] indxc = new int[n];
			int[] indxr = new int[n];
			int[] ipiv = new int[n];
			for (j = 0; j < n; j++){
				ipiv[j] = 0;
			}
			for (int i = 0; i < n; i++){
				double big = 0.0;
				for (j = 0; j < n; j++){
					if (ipiv[j] != 1){
						for (k = 0; k < n; k++){
							if (ipiv[k] == 0){
								if (Math.Abs(a[j, k]) >= big){
									big = Math.Abs(a[j, k]);
									irow = j;
									icol = k;
								}
							} else if (ipiv[k] > 1){
								throw new Exception("gaussj: Singular Matrix-1");
							}
						}
					}
				}
				++(ipiv[icol]);
				if (irow != icol){
					for (l = 0; l < n; l++){
						temp = a[irow, l];
						a[irow, l] = a[icol, l];
						a[icol, l] = temp;
					}
					for (l = 0; l < m; l++){
						temp = b[irow, l];
						b[irow, l] = b[icol, l];
						b[icol, l] = temp;
					}
				}
				indxr[i] = irow;
				indxc[i] = icol;
				if (a[icol, icol] == 0.0){
					throw new Exception("gaussj: Singular Matrix-2");
				}
				double pivinv = 1.0/a[icol, icol];
				a[icol, icol] = 1.0;
				for (l = 0; l < n; l++){
					a[icol, l] *= pivinv;
				}
				for (l = 0; l < m; l++){
					b[icol, l] *= pivinv;
				}
				int ll;
				for (ll = 0; ll < n; ll++){
					if (ll != icol){
						double dum = a[ll, icol];
						a[ll, icol] = 0.0;
						for (l = 0; l < n; l++){
							a[ll, l] -= a[icol, l]*dum;
						}
						for (l = 0; l < m; l++){
							b[ll, l] -= b[icol, l]*dum;
						}
					}
				}
			}
			for (l = n - 1; l >= 0; l--){
				if (indxr[l] != indxc[l]){
					for (k = 0; k < n; k++){
						temp = a[k, indxr[l]];
						a[k, indxr[l]] = a[k, indxc[l]];
						a[k, indxc[l]] = temp;
					}
				}
			}
		}

		public static void Covsrt(double[,] covar){
			int ma = covar.GetLength(0);
			int mfit = ma;
			for (int i = mfit; i < ma; i++){
				for (int j = 0; j <= i; j++){
					covar[i, j] = 0.0;
					covar[j, i] = 0.0;
				}
			}
			int k = mfit - 1;
			for (int j = ma - 1; j >= 0; j--){
				double swap;
				for (int i = 0; i < ma; i++){
					swap = covar[i, k];
					covar[i, k] = covar[i, j];
					covar[i, j] = swap;
				}
				for (int i = 0; i < ma; i++){
					swap = covar[k, i];
					covar[k, i] = covar[j, i];
					covar[j, i] = swap;
				}
				k--;
			}
		}

		public static void LinearFit(double[] x, double[] y, bool bIs0, bool median, out double a, out double b){
			if (median){
				if (bIs0){
					a = MedfitOrigin(x, y);
					b = 0;
				} else{
					double abdev;
					Medfit(x, y, out b, out a, out abdev);
				}
			} else{
				if (bIs0){
					double[] ap = new double[1];
					LinFit2(x, y, ap, delegate(double x1, double[] a1) { a1[0] = x1; });
					a = ap[0];
					b = 0;
				} else{
					double[] ap = new double[2];
					LinFit2(x, y, ap, delegate(double x1, double[] a1){
						a1[0] = 1;
						a1[1] = x1;
					});
					a = ap[1];
					b = ap[0];
				}
			}
		}

		/// <summary>
		/// Fits y = a + b * x by the criterion of least absolute deviations.
		/// </summary>
		/// <param name="x">The input x values.</param>
		/// <param name="y">The input y values.</param>
		/// <param name="a">Fitted offset parameter.</param>
		/// <param name="b">Fitted slope parameter.</param>
		/// <param name="abdev">absolute deviation in y of 
		/// the experimental points from the fitted line.</param>
		public static void Medfit(double[] x, double[] y, out double a, out double b, out double abdev){
			int ndata = x.Length;
			double sx = 0.0, sy = 0.0, sxy = 0.0, sxx = 0.0, chisq = 0.0;
			int ndatat = ndata;
			double[] xt = x;
			double[] yt = y;
			for (int j = 0; j < ndata; j++){
				sx += x[j];
				sy += y[j];
				sxy += x[j]*y[j];
				sxx += x[j]*x[j];
			}
			double del = ndata*sxx - sx*sx;
			double aa = (sxx*sy - sx*sxy)/del;
			double bb = (ndata*sxy - sx*sy)/del;
			for (int j = 0; j < ndata; j++){
				double temp = y[j] - (aa + bb*x[j]);
				chisq += (temp*temp);
			}
			double sigb = Math.Sqrt(chisq/del);
			double b1 = bb;
			double abdevt;
			double f1 = Rofunc(b1, ndatat, xt, yt, out aa, out abdevt);
			double sign = f1 >= 0.0 ? Math.Abs(3.0*sigb) : -Math.Abs(3.0*sigb);
			double b2 = bb + sign;
			double f2 = Rofunc(b2, ndatat, xt, yt, out aa, out abdevt);
			while (f1*f2 > 0.0){
				bb = 2.0*b2 - b1;
				b1 = b2;
				f1 = f2;
				b2 = bb;
				f2 = Rofunc(b2, ndatat, xt, yt, out aa, out abdevt);
			}
			sigb = 0.01*sigb;
			while (Math.Abs(b2 - b1) > sigb){
				bb = 0.5*(b1 + b2);
				if (bb == b1 || bb == b2){
					break;
				}
				double f = Rofunc(bb, ndatat, xt, yt, out aa, out abdevt);
				if (f*f1 >= 0.0){
					f1 = f;
					b1 = bb;
				} else{
					b2 = bb;
				}
			}
			a = aa;
			b = bb;
			abdev = abdevt/ndata;
		}

		private const double rofuncEps = 1.0e-7;

		public static double Rofunc(double b, int ndatat, double[] xt, double[] yt, out double aa, out double abdevt){
			double[] arr = new double[ndatat];
			for (int j = 0; j < ndatat; j++){
				arr[j] = yt[j] - b*xt[j];
			}
			if (ndatat%2 == 1){
				aa = Select((ulong) (ndatat/2), (ulong) ndatat, arr);
			} else{
				int j = ndatat/2;
				aa = 0.5*(Select((ulong) (j - 1), (ulong) ndatat, arr) + Select((ulong) j, (ulong) ndatat, arr));
			}
			abdevt = 0.0;
			double sum = 0.0;
			for (int j = 0; j < ndatat; j++){
				double d = yt[j] - (b*xt[j] + aa);
				abdevt += Math.Abs(d);
				if (yt[j] != 0.0){
					d /= Math.Abs(yt[j]);
				}
				if (Math.Abs(d) > rofuncEps){
					sum += d >= 0.0 ? xt[j] : -xt[j];
				}
			}
			return sum;
		}

		public static double Select(ulong k, ulong n, double[] arr){
			ulong l = 0;
			ulong ir = n - 1;
			for (;;){
				double temp;
				if (ir <= l + 1){
					if (ir == l + 1 && arr[ir] < arr[l]){
						temp = arr[l];
						arr[l] = arr[ir];
						arr[ir] = temp;
					}
					return arr[k];
				}
				ulong mid = (l + ir) >> 1;
				temp = arr[mid];
				arr[mid] = arr[l + 1];
				arr[l + 1] = temp;
				if (arr[l + 1] > arr[ir]){
					temp = arr[l + 1];
					arr[l + 1] = arr[ir];
					arr[ir] = temp;
				}
				if (arr[l] > arr[ir]){
					temp = arr[l];
					arr[l] = arr[ir];
					arr[ir] = temp;
				}
				if (arr[l + 1] > arr[l]){
					temp = arr[l + 1];
					arr[l + 1] = arr[l];
					arr[l] = temp;
				}
				ulong i = l + 1;
				ulong j = ir;
				double d = arr[l];
				for (;;){
					do i++; while (arr[i] < d);
					do j--; while (arr[j] > d);
					if (j < i){
						break;
					}
					temp = arr[i];
					arr[i] = arr[j];
					arr[j] = temp;
				}
				arr[l] = arr[j];
				arr[j] = d;
				if (j >= k){
					ir = j - 1;
				}
				if (j <= k){
					l = i;
				}
			}
		}

		public static double MedfitOrigin(double[] x, double[] y){
			double abdev;
			double r = MedfitOriginImpl(x, y, out abdev);
			double r2 = MedfitOriginImpl(y, x, out abdev);
			if (r <= 0 || double.IsInfinity(r) || double.IsNaN(r)){
				return 1/r2;
			}
			if (r2 <= 0 || double.IsInfinity(r2) || double.IsNaN(r2)){
				return r;
			}
			return Math.Sqrt(r/r2);
		}

		public static double MedfitOriginImpl(double[] x, double[] y, out double abdev){
			int ndata = x.Length;
			int j;
			double sx = 0.0, sy = 0.0, sxy = 0.0, sxx = 0.0, chisq = 0.0;
			int ndatat = ndata;
			double[] xt = x;
			double[] yt = y;
			for (j = 0; j < ndata; j++){
				sx += x[j];
				sy += y[j];
				sxy += x[j]*y[j];
				sxx += x[j]*x[j];
			}
			double del = ndata*sxx - sx*sx;
			double bb = (ndata*sxy - sx*sy)/del;
			for (j = 0; j < ndata; j++){
				double temp = y[j] - (bb*x[j]);
				chisq += temp*temp;
			}
			double sigb = Math.Sqrt(chisq/del);
			double b1 = bb;
			double abdevt;
			double f1 = Rofunc(b1, ndatat, xt, yt, out abdevt);
			double sign = f1 >= 0.0 ? Math.Abs(3.0*sigb) : -Math.Abs(3.0*sigb);
			double b2 = bb + sign;
			double f2 = Rofunc(b2, ndatat, xt, yt, out abdevt);
			while (f1*f2 > 0.0){
				bb = 2.0*b2 - b1;
				b1 = b2;
				f1 = f2;
				b2 = bb;
				f2 = Rofunc(b2, ndatat, xt, yt, out abdevt);
			}
			sigb = 0.01*sigb;
			while (Math.Abs(b2 - b1) > sigb){
				bb = 0.5*(b1 + b2);
				if (bb == b1 || bb == b2){
					break;
				}
				double f = Rofunc(bb, ndatat, xt, yt, out abdevt);
				if (f*f1 >= 0.0){
					f1 = f;
					b1 = bb;
				} else{
					b2 = bb;
				}
			}
			abdev = abdevt/ndata;
			return bb;
		}

		private const double eps = 1.0e-7;

		public static double Rofunc(double b, int ndatat, double[] xt, double[] yt, out double abdevt){
			int j;
			double sum = 0.0;
			double[] arr = new double[ndatat];
			for (j = 0; j < ndatat; j++){
				arr[j] = yt[j] - b*xt[j];
			}
			abdevt = 0.0;
			for (j = 0; j < ndatat; j++){
				double d = yt[j] - (b*xt[j]);
				abdevt += Math.Abs(d);
				if (yt[j] != 0.0){
					d /= Math.Abs(yt[j]);
				}
				if (Math.Abs(d) > eps){
					sum += (d >= 0.0 ? xt[j] : -xt[j]);
				}
			}
			return sum;
		}

		public static double[] MatrixTimesVector(double[,] x, double[] v){
			double[] result = new double[x.GetLength(0)];
			for (int i = 0; i < x.GetLength(0); i++){
				for (int k = 0; k < x.GetLength(1); k++){
					result[i] += x[i, k]*v[k];
				}
			}
			return result;
		}

		public static double StandardGaussian(double[] x){
			double sum = 0;
			foreach (double t in x){
				sum += t*t;
			}
			return Math.Exp(-0.5*sum)/Math.Pow(2*Math.PI, 0.5*x.Length);
		}

		public static double[,] CalcCovariance(double[,] data){
			int n = data.GetLength(0);
			int p = data.GetLength(1);
			double[] means = new double[p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j < n; j++){
					means[i] += data[j, i];
				}
				means[i] /= n;
			}
			double[,] cov = new double[p, p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j <= i; j++){
					for (int k = 0; k < n; k++){
						cov[i, j] += (data[k, i] - means[i])*(data[k, j] - means[j]);
					}
					cov[i, j] /= n;
					cov[j, i] = cov[i, j];
				}
			}
			return cov;
		}

		public static double[,] CalcCovariance(IList<double>[] data){
			int n = data[0].Count;
			int p = data.Length;
			double[] means = new double[p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j < n; j++){
					means[i] += data[i][j];
				}
				means[i] /= n;
			}
			double[,] cov = new double[p, p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j <= i; j++){
					for (int k = 0; k < n; k++){
						cov[i, j] += (data[i][k] - means[i])*(data[j][k] - means[j]);
					}
					cov[i, j] /= n;
					cov[j, i] = cov[i, j];
				}
			}
			return cov;
		}

		public static double[,] CalcCovariance(double[][] data){
			int n = data[0].Length;
			int p = data.Length;
			double[] means = new double[p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j < n; j++){
					means[i] += data[i][j];
				}
				means[i] /= n;
			}
			double[,] cov = new double[p, p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j <= i; j++){
					for (int k = 0; k < n; k++){
						cov[i, j] += (data[i][k] - means[i])*(data[j][k] - means[j]);
					}
					cov[i, j] /= n;
					cov[j, i] = cov[i, j];
				}
			}
			return cov;
		}

		public static double[,] ApplyFunction(double[,] m, Func<double, double> func){
			int n = m.GetLength(0);
			double[,] v;
			double[] e = DiagonalizeSymmMatrix(m, out v);
			for (int i = 0; i < n; i++){
				e[i] = func(e[i]);
			}
			double[,] result = new double[n, n];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < n; j++){
					for (int k = 0; k < n; k++){
						result[i, j] += v[i, k]*e[k]*v[j, k];
					}
				}
			}
			return result;
		}

		/// <param name="m">Symmetrical input matrix.</param>
		/// <param name="evec">The matrix of eigenvectors. The second index iterates through the different eigenvectors.</param>
		/// <returns>Vector of eigenvalues in no particular order.</returns>
		public static double[] DiagonalizeSymmMatrix(double[,] m, out double[,] evec){
			double[] d;
			double[] e;
			evec = (double[,]) m.Clone();
			Tred2(evec, out d, out e);
			Tqli(d, e, evec);
			return d;
		}

		public static void Tqli(double[] d, double[] e, double[,] z){
			int n = d.Length;
			int l;
			int i;
			for (i = 1; i < n; i++){
				e[i - 1] = e[i];
			}
			e[n - 1] = 0.0;
			for (l = 0; l < n; l++){
				int iter = 0;
				int m;
				do{
					for (m = l; m < n - 1; m++){
						double dd = Math.Abs(d[m]) + Math.Abs(d[m + 1]);
						if ((Math.Abs(e[m]) + dd) == dd){
							break;
						}
					}
					if (m != l){
						if (iter++ == 30){
							throw new Exception("Too many iterations in tqli");
						}
						double g = (d[l + 1] - d[l])/(2.0*e[l]);
						double r = Pythag(g, 1.0);
						g = d[m] - d[l] + e[l]/(g + ((g) >= 0.0 ? Math.Abs(r) : -Math.Abs(r)));
						double c;
						double s = c = 1.0;
						double p = 0.0;
						for (i = m - 1; i >= l; i--){
							double f = s*e[i];
							double b = c*e[i];
							e[i + 1] = (r = Pythag(f, g));
							if (r == 0.0){
								d[i + 1] -= p;
								e[m] = 0.0;
								break;
							}
							s = f/r;
							c = g/r;
							g = d[i + 1] - p;
							r = (d[i] - g)*s + 2.0*c*b;
							d[i + 1] = g + (p = s*r);
							g = c*r - b;
							int k;
							for (k = 0; k < n; k++){
								f = z[k, i + 1];
								z[k, i + 1] = s*z[k, i] + c*f;
								z[k, i] = c*z[k, i] - s*f;
							}
						}
						if (r == 0.0 && i >= l){
							continue;
						}
						d[l] -= p;
						e[l] = g;
						e[m] = 0.0;
					}
				} while (m != l);
			}
		}

		/// <summary>
		/// Computes (a^2+b^2)^1/2 without underflow or overflow.
		/// </summary>
		public static double Pythag(double a, double b){
			double absa = Math.Abs(a);
			double absb = Math.Abs(b);
			if (absa > absb){
				return absa*Math.Sqrt(1.0 + (absb/absa)*(absb/absa));
			}
			return (absb == 0.0 ? 0.0 : absb*Math.Sqrt(1.0 + (absa/absb)*(absa/absb)));
		}

		public static void Tred2(double[,] a, out double[] d, out double[] e){
			int n = a.GetLength(0);
			d = new double[n];
			e = new double[n];
			int l, k, j, i;
			double g;
			for (i = n - 1; i >= 1; i--){
				l = i - 1;
				double scale;
				double h = scale = 0.0;
				if (l > 0){
					for (k = 0; k <= l; k++){
						scale += Math.Abs(a[i, k]);
					}
					if (scale == 0.0){
						e[i] = a[i, l];
					} else{
						for (k = 0; k <= l; k++){
							a[i, k] /= scale;
							h += a[i, k]*a[i, k];
						}
						double f = a[i, l];
						g = (f >= 0.0 ? -Math.Sqrt(h) : Math.Sqrt(h));
						e[i] = scale*g;
						h -= f*g;
						a[i, l] = f - g;
						f = 0.0;
						for (j = 0; j <= l; j++){
							a[j, i] = a[i, j]/h;
							g = 0.0;
							for (k = 0; k <= j; k++){
								g += a[j, k]*a[i, k];
							}
							for (k = j + 1; k <= l; k++){
								g += a[k, j]*a[i, k];
							}
							e[j] = g/h;
							f += e[j]*a[i, j];
						}
						double hh = f/(h + h);
						for (j = 0; j <= l; j++){
							f = a[i, j];
							e[j] = g = e[j] - hh*f;
							for (k = 0; k <= j; k++){
								a[j, k] -= (f*e[k] + g*a[i, k]);
							}
						}
					}
				} else{
					e[i] = a[i, l];
				}
				d[i] = h;
			}
			d[0] = 0.0;
			e[0] = 0.0;
			/* Contents of this loop can be omitted if eigenvectors not
			wanted except for statement d[i]=a[i][i]; */
			for (i = 0; i < n; i++){
				l = i - 1;
				if (d[i] != 0){
					for (j = 0; j <= l; j++){
						g = 0.0;
						for (k = 0; k <= l; k++){
							g += a[i, k]*a[k, j];
						}
						for (k = 0; k <= l; k++){
							a[k, j] -= g*a[k, i];
						}
					}
				}
				d[i] = a[i, i];
				a[i, i] = 1.0;
				for (j = 0; j <= l; j++){
					a[j, i] = a[i, j] = 0.0;
				}
			}
		}

		public static void CalcIntersect(float[] x, float[] y, out float[] x1, out float[] y1){
			List<float> x2 = new List<float>();
			List<float> y2 = new List<float>();
			for (int i = 0; i < x.Length; i++){
				float x3 = x[i];
				float y3 = y[i];
				if (!float.IsNaN(x3) && !float.IsNaN(y3) && !float.IsInfinity(x3) && !float.IsInfinity(y3)){
					x2.Add(x3);
					y2.Add(y3);
				}
			}
			x1 = x2.ToArray();
			y1 = y2.ToArray();
		}

		/// <summary>
		/// Returns the error function erf(x)
		/// </summary>
		public static double Erff(double x){
			return x < 0.0 ? -Gammp(0.5, x*x) : Gammp(0.5, x*x);
		}

		/// <summary>
		/// Returns the complementary error function erfc(x) = 1 - erf(x)
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static double Erffc(double x){
			return x < 0.0 ? 1.0 + Gammp(0.5, x*x) : Gammq(0.5, x*x);
		}

		/// <summary>
		/// Returns the incomplete gamma function P(a,x)
		/// </summary>
		public static double Gammp(double a, double x){
			double gamser = double.NaN;
			double gammcf = double.NaN;
			double gln;
			if (x < 0.0 || a <= 0.0){
				throw new Exception("Invalid arguments in routine gammq");
			}
			if (x < a + 1.0){
				Gser(ref gamser, a, x, out gln);
				return gamser;
			}
			Gcf(ref gammcf, a, x, out gln);
			return 1.0 - gammcf;
		}

		/// <summary>
		/// Returns the incomplete gamma function Q(a,x) = 1 - P(a,x)
		/// </summary>
		public static double Gammq(double a, double x){
			if (x < 0.0 || a <= 0.0){
				throw new Exception("Invalid arguments in routine gammq");
			}
			if (x < (a + 1.0)){
				double gamser = double.NaN;
				double gln;
				Gser(ref gamser, a, x, out gln);
				return 1.0 - gamser;
			} else{
				double gammcf = double.NaN;
				double gln;
				Gcf(ref gammcf, a, x, out gln);
				return gammcf;
			}
		}

		private const double gcfEps = 3.0e-7;
		private const double gcfFpmin = 1.0e-30;
		private const double gcfItmax = 100;
		private const double gserEps = 3.0e-7;
		private const double gserItmax = 100;

		public static void Gcf(ref double gammcf, double a, double x, out double gln){
			int i;
			gln = Gamma.LnValue(a);
			double b = x + 1.0 - a;
			double c = 1.0/gcfFpmin;
			double d = 1.0/b;
			double h = d;
			for (i = 1; i <= gcfItmax; i++){
				double an = -i*(i - a);
				b += 2.0;
				d = an*d + b;
				if (Math.Abs(d) < gcfFpmin){
					d = gcfFpmin;
				}
				c = b + an/c;
				if (Math.Abs(c) < gcfFpmin){
					c = gcfFpmin;
				}
				d = 1.0/d;
				double del = d*c;
				h *= del;
				if (Math.Abs(del - 1.0) < gcfEps){
					break;
				}
			}
			if (i > gcfItmax){
				throw new Exception("a too large, ITMAX too small in gcf");
			}
			gammcf = Math.Exp(-x + a*Math.Log(x) - gln)*h;
		}

		public static void Gser(ref double gamser, double a, double x, out double gln){
			gln = Gamma.LnValue(a);
			if (x <= 0.0){
				if (x < 0.0){
					throw new Exception("x less than 0 in routine gser");
				}
				gamser = 0.0;
				return;
			}
			double ap = a;
			double sum = 1.0/a;
			double del = sum;
			for (int n = 1; n <= gserItmax; n++){
				++ap;
				del *= x/ap;
				sum += del;
				if (Math.Abs(del) < Math.Abs(sum)*gserEps){
					gamser = sum*Math.Exp(-x + a*Math.Log(x) - (gln));
					return;
				}
			}
			throw new Exception("a too large, ITMAX too small in routine gser");
		}

		public static float Clamp(float x, float min, float max){
			return Math.Min(Math.Max(x, min), max);
		}

		public static int[][] CalcCollapse(string[] names){
			string[][] names2 = new string[names.Length][];
			for (int i = 0; i < names.Length; i++){
				string s = names[i].Trim();
				names2[i] = s.Length > 0 ? s.Split(';') : new string[0];
				Array.Sort(names2[i]);
			}
			NeighbourList neighbourList = new NeighbourList();
			for (int i = 0; i < names2.Length; i++){
				for (int j = i + 1; j < names2.Length; j++){
					if (CommonElement(names2[i], names2[j])){
						neighbourList.Add(i, j);
					}
				}
			}
			List<int[]> clusterList = new List<int[]>();
			for (int i = 0; i < names2.Length; i++){
				if (neighbourList.IsEmptyAt(i)){
					if (names2[i].Length > 0){
						clusterList.Add(new[]{i});
					}
				}
			}
			for (int i = 0; i < names2.Length; i++){
				if (!neighbourList.IsEmptyAt(i)){
					int[] cc = neighbourList.GetClusterAt(i);
					Array.Sort(cc);
					clusterList.Add(cc);
				}
			}
			return clusterList.ToArray();
		}

		private static bool CommonElement(IEnumerable<string> s1, string[] s2){
			foreach (string s in s1){
				if (Array.BinarySearch(s2, s) >= 0){
					return true;
				}
			}
			return false;
		}

		public static double[] MovingBoxPlot(double[] values, double[] controlValues, int nbins){
			return MovingBoxPlot(values, controlValues, nbins, TestSide.Both);
		}

		public static double[] MovingBoxPlot(double[] values, double[] controlValues, int nbins, TestSide type){
			double[] lowerQuart;
			double[] median;
			double[] upperQuart;
			double[] binBoundaries;
			return MovingBoxPlot(values, controlValues, nbins, out lowerQuart, out median, out upperQuart, out binBoundaries,
				type);
		}

		public static readonly int minBinsize = 500;

		public static double[] MovingBoxPlot(double[] values, double[] controlValues, int nbins, out double[] lowerQuart,
			out double[] median, out double[] upperQuart, out double[] binBoundaries, TestSide type){
			int n = values.Length;
			if (n == 0){
				lowerQuart = new double[0];
				median = new double[0];
				upperQuart = new double[0];
				binBoundaries = new double[0];
				return new double[0];
			}
			nbins = nbins < 0 ? Math.Max(1, (int) Math.Round(n/(double) minBinsize)) : Math.Min(nbins, 1 + n/4);
			int[] pos = new int[nbins + 1];
			for (int i = 0; i < nbins + 1; i++){
				pos[i] = (int) Math.Round(i/(double) nbins*n);
			}
			double[] result = new double[n];
			lowerQuart = new double[nbins];
			median = new double[nbins];
			upperQuart = new double[nbins];
			binBoundaries = new double[nbins];
			int[] o = ArrayUtils.Order(controlValues);
			int[][] indices = new int[nbins][];
			for (int i = 0; i < nbins; i++){
				indices[i] = new int[pos[i + 1] - pos[i]];
				Array.Copy(o, pos[i], indices[i], 0, pos[i + 1] - pos[i]);
			}
			for (int i = 0; i < nbins; i++){
				double[] r = ArrayUtils.SubArray(values, indices[i]);
				double[] c = ArrayUtils.SubArray(controlValues, indices[i]);
				int[] o1 = ArrayUtils.Order(r);
				double rlow = r[o1[(int) Math.Round(0.1587*(r.Length - 1))]];
				double rmed = r[o1[(int) Math.Round(0.5*(r.Length - 1))]];
				double rhigh = r[o1[(int) Math.Round(0.8413*(r.Length - 1))]];
				lowerQuart[i] = rlow;
				median[i] = rmed;
				upperQuart[i] = rhigh;
				binBoundaries[i] = ArrayUtils.Min(c);
				if (indices[i].Length > 2){
					for (int j = 0; j < indices[i].Length; j++){
						double ratio = r[j];
						switch (type){
							case TestSide.Right:
								if (rhigh == rmed){
									result[indices[i][j]] = 1;
								} else{
									double z = (ratio - rmed)/(rhigh - rmed);
									result[indices[i][j]] = Errfunc(z);
								}
								break;
							case TestSide.Left:
								if (rlow == rmed){
									result[indices[i][j]] = 1;
								} else{
									double z = (ratio - rmed)/(rlow - rmed);
									result[indices[i][j]] = Errfunc(z);
								}
								break;
							default:
								if (ratio >= rmed){
									if (rhigh == rmed){
										result[indices[i][j]] = 1;
									} else{
										double z = (ratio - rmed)/(rhigh - rmed);
										result[indices[i][j]] = 2*Errfunc(z);
									}
								} else{
									if (rlow == rmed){
										result[indices[i][j]] = 1;
									} else{
										double z = (ratio - rmed)/(rlow - rmed);
										result[indices[i][j]] = 2*Errfunc(z);
									}
								}
								break;
						}
					}
				} else{
					for (int j = 0; j < indices[i].Length; j++){
						result[indices[i][j]] = 1;
					}
				}
			}
			return result;
		}

		public static double Errfunc(double z){
			try{
				return (Erffc(z/Math.Sqrt(2.0)))/2.0;
			} catch (Exception){
				return 1;
			}
		}

		public static double[,] InvertSymmMatrix(double[,] m){
			int n = m.GetLength(0);
			double[,] v;
			double[] e = DiagonalizeSymmMatrix(m, out v);
			for (int i = 0; i < n; i++){
				e[i] = 1.0/e[i];
			}
			double[,] result = new double[n, n];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < n; j++){
					for (int k = 0; k < n; k++){
						result[i, j] += v[i, k]*e[k]*v[j, k];
					}
				}
			}
			return result;
		}

		public static double[] GeneralizedEigenproblem(double[,] b, double[,] w, out double[,] x){
			int n = b.GetLength(0);
			double[,] winv = InvertSymmMatrix(w);
			double[,] v;
			double[] e = DiagonalizeSymmMatrix(b, out v);
			double[,] sqrtB = new double[n, n];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < n; j++){
					for (int k = 0; k < n; k++){
						if (e[k] > 1e-7){
							sqrtB[i, j] += v[i, k]*Math.Sqrt(e[k])*v[j, k];
						}
					}
				}
			}
			double[,] sqrtBinv = new double[n, n];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < n; j++){
					for (int k = 0; k < n; k++){
						if (e[k] > 1e-7){
							sqrtBinv[i, j] += v[i, k]*1.0/Math.Sqrt(e[k])*v[j, k];
						}
					}
				}
			}
			double[,] q = MatrixUtils.MatrixTimesMatrix(MatrixUtils.MatrixTimesMatrix(sqrtB, winv), sqrtB);
			double[] f = DiagonalizeSymmMatrix(q, out x);
			x = MatrixUtils.MatrixTimesMatrix(sqrtBinv, x);
			return f;
		}

		/// <summary>
		/// Returns the sample ranks of the values in a array. Ties (i.e., equal values) are handled . in "average" way.
		/// </summary>
		/// <param name="x">Numeric List object</param>
		/// <param name="sumDuplicates">Arrays of number elements in each ties group. Important for test correction</param>
		/// <returns>Rank array</returns>
		private static double[] Rank(List<double> x, out List<int> sumDuplicates){
			sumDuplicates = new List<int>();
			List<KeyValuePair<double, int>> xx = x.Select((a, b) => new KeyValuePair<double, int>(a, b)).OrderBy(a => a.Key).ToList();
			int[] xSortedIndex = xx.Select(a => a.Value).ToArray();
			double[] xSorted = xx.Select(a => a.Key).ToArray();
			int n = xSorted.Length;
			double[] result = new double[n];
			int duplicates = 0, sumRank = 0, i;
			for (i = 0; i < n; i++){
				sumRank += i;
				duplicates++;
				if ((i == n - 1) || (xSorted[i] != xSorted[i + 1])){
					int j;
					for (j = i - duplicates + 1; j < i + 1; j++){
						result[xSortedIndex[j]] = 1 + sumRank*1.0/duplicates;
					}
					if (duplicates > 1){
						sumDuplicates.Add(duplicates);
					}
					duplicates = 0;
					sumRank = 0;
				}
			}
			return result;
		}

		public static double Gaussian(double x, double sigma){
			return  Math.Exp(-x*x/(2*sigma*sigma))/ (Math.Sqrt(2*Math.PI)*sigma);
		}

		public static double SinC(double x){
			const double epsilon = .00001F;
			if (Math.Abs(x) > epsilon){
				x *= Math.PI;
				return Clean(Math.Sin(x)/x);
			}
			return 1.0f;
		}

		private static double Clean(double x) {
			const double epsilon = .00001F;
			return Math.Abs(x) < epsilon ? 0F : x;
		}

		public static int Clamp(int value, int min, int max){
			if (value > max){
				return max;
			}
			return value < min ? min : value;
		}

		public static double Determinant2X2(double[,] m) {
			return m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
		}
	}
}