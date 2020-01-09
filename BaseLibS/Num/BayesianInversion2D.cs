using System;
using System.Collections.Generic;
using System.Threading;

namespace BaseLibS.Num {
	public class BayesianInversion2D {
		private readonly double[] xout;
		private readonly double[] yout;
		private readonly double[,] zout;
		private readonly double[,] forward;
		private readonly double[,] reverse;

		public BayesianInversion2D(IList<double> xdata, IList<double> ydata, IList<bool> correct, int nthreads, int ndata,
			double[,] covIn, bool debug) {
			Invert(xdata, ydata, correct, out xout, out yout, out zout, out forward, out reverse, nthreads, ndata, covIn, debug);
		}

		public double GetValue(double x, double y) {
			return GetValue(x, y, xout, yout, zout);
		}

		public double GetForwardHist(double x, double y) {
			return GetValue(x, y, xout, yout, forward);
		}

		public double GetReverseHist(double x, double y) {
			return GetValue(x, y, xout, yout, reverse);
		}

		private static void Invert(IList<double> xdata, IList<double> ydata, IList<bool> correct, out double[] xout,
			out double[] yout, out double[,] zout, out double[,] forwardOut, out double[,] reverseOut, int nthreads, int ndata,
			double[,] covIn, bool debug) {
			int n = correct.Count;
			int ntrue = 0;
			for (int i = 0; i < n; i++) {
				if (correct[i]) {
					ntrue++;
				}
			}
			int nfalse = n - ntrue;
			if (ntrue < 3 || nfalse < 3) {
				xout = null;
				yout = null;
				zout = null;
				forwardOut = null;
				reverseOut = null;
				return;
			}
			double[,] falseData = new double[nfalse, 2];
			double[,] trueData = new double[ntrue, 2];
			int iFalse = 0;
			int iTrue = 0;
			for (int i = 0; i < n; i++) {
				if (correct[i]) {
					trueData[iTrue, 0] = xdata[i];
					trueData[iTrue, 1] = ydata[i];
					iTrue++;
				} else {
					falseData[iFalse, 0] = xdata[i];
					falseData[iFalse, 1] = ydata[i];
					iFalse++;
				}
			}
			Invert(falseData, trueData, out xout, out yout, out zout, out forwardOut, out reverseOut, nthreads,
				ndata <= 0 ? ntrue : ndata, covIn, debug);
		}

		private static void Invert(double[,] falseData, double[,] trueData, out double[] xRes, out double[] yRes,
			out double[,] zRes, out double[,] forwardOut, out double[,] reverseOut, int nthreads, int ndata, double[,] covIn,
			bool debug) {
			double xmin = double.MaxValue;
			double xmax = double.MinValue;
			double ymin = double.MaxValue;
			double ymax = double.MinValue;
			for (int i = 0; i < falseData.GetLength(0); i++) {
				double xx = falseData[i, 0];
				double yy = falseData[i, 1];
				if (xx < xmin) {
					xmin = xx;
				}
				if (xx > xmax) {
					xmax = xx;
				}
				if (yy < ymin) {
					ymin = yy;
				}
				if (yy > ymax) {
					ymax = yy;
				}
			}
			for (int i = 0; i < trueData.GetLength(0); i++) {
				double xx = trueData[i, 0];
				double yy = trueData[i, 1];
				if (xx < xmin) {
					xmin = xx;
				}
				if (xx > xmax) {
					xmax = xx;
				}
				if (yy < ymin) {
					ymin = yy;
				}
				if (yy > ymax) {
					ymax = yy;
				}
			}
			double dx = xmax - xmin;
			xmin -= 0.1 * dx;
			xmax += 0.1 * dx;
			double dy = ymax - ymin;
			ymin -= 0.1 * dy;
			ymax += 0.1 * dy;
			double[] falseX;
			double[] falseY;
			double[,] falseZ;
			double[,] cov = covIn ?? NumUtils.CalcCovariance(trueData);
			double fact = Math.Pow(ndata, 1.0 / 6.0);
			double[,] hinv = null;
			try {
				hinv = NumUtils.ApplyFunction(cov, w => fact / Math.Sqrt(w));
			} catch { }
			if (hinv == null || !IsValidMatrix(hinv)) {
				xRes = null;
				yRes = null;
				zRes = null;
				forwardOut = null;
				reverseOut = null;
				return;
			}
			try {
				EstimateBivariateDensity(falseData, out falseX, out falseY, out falseZ, xmin, xmax, ymin, ymax, hinv, nthreads);
			} catch (Exception) {
				xRes = null;
				yRes = null;
				zRes = null;
				forwardOut = null;
				reverseOut = null;
				return;
			}
			double[] trueX;
			double[] trueY;
			double[,] trueZ;
			try {
				EstimateBivariateDensity(trueData, out trueX, out trueY, out trueZ, xmin, xmax, ymin, ymax, hinv, nthreads);
			} catch (Exception) {
				xRes = null;
				yRes = null;
				zRes = null;
				forwardOut = null;
				reverseOut = null;
				return;
			}
			double[] x = UnifySupport(falseX, trueX);
			double[] y = UnifySupport(falseY, trueY);
			falseZ = Interpolate(x, y, falseX, falseY, falseZ);
			trueZ = Interpolate(x, y, trueX, trueY, trueZ);
			double[,] inverse = new double[x.Length, y.Length];
			for (int i = 0; i < x.Length; i++) {
				for (int j = 0; j < y.Length; j++) {
					inverse[i, j] = falseZ[i, j] <= 0 ? double.Epsilon : Math.Max((falseZ[i, j] * 0.5) / trueZ[i, j], double.Epsilon);
				}
			}
			for (int j = 0; j < y.Length; j++) {
				double maxVal = double.MinValue;
				int maxInd = -1;
				for (int i = 0; i < x.Length; i++) {
					if (inverse[i, j] > maxVal) {
						maxVal = inverse[i, j];
						maxInd = i;
					}
				}
				for (int i = 0; i < maxInd; i++) {
					inverse[i, j] = maxVal;
				}
			}
			xRes = x;
			yRes = y;
			zRes = inverse;
			if (debug) {
				forwardOut = trueZ;
				reverseOut = falseZ;
			} else {
				forwardOut = null;
				reverseOut = null;
			}
		}

		private static double GetValue(double x, double y, double[] xvals, double[] yvals, double[,] zvals) {
			double z = GetValueImpl(x, y, xvals, yvals, zvals);
			return double.IsNaN(z) ? 10 : z;
		}

		private static double GetValueImpl(double x, double y, double[] xvals, double[] yvals, double[,] zvals) {
			if (xvals == null || yvals == null) {
				return double.NaN;
			}
			if (xvals.Length == 0 || yvals.Length == 0) {
				return double.NaN;
			}
			if (x <= xvals[0]) {
				return InterpolateExactX(0, y, yvals, zvals);
			}
			if (x >= xvals[xvals.Length - 1]) {
				return InterpolateExactX(xvals.Length - 1, y, yvals, zvals);
			}
			int ax = Array.BinarySearch(xvals, x);
			if (ax >= 0) {
				return InterpolateExactX(ax, y, yvals, zvals);
			}
			if (y <= yvals[0]) {
				return InterpolateExactY(0, x, xvals, zvals);
			}
			if (y >= yvals[yvals.Length - 1]) {
				return InterpolateExactY(yvals.Length - 1, x, xvals, zvals);
			}
			int ay = Array.BinarySearch(yvals, y);
			if (ay >= 0) {
				return InterpolateExactY(ay, x, xvals, zvals);
			}
			int i1 = -2 - ax;
			int i2 = i1 + 1;
			int j1 = -2 - ay;
			int j2 = j1 + 1;
			if (i1 < 0 || j1 < 0) {
				return double.NaN;
			}
			if (i2 >= xvals.Length || j2 >= yvals.Length) {
				return double.NaN;
			}
			double x1 = xvals[i1];
			double x2 = xvals[i2];
			double y1 = yvals[j1];
			double y2 = yvals[j2];
			double z11 = zvals[i1, j1];
			double z12 = zvals[i1, j2];
			double z21 = zvals[i2, j1];
			double z22 = zvals[i2, j2];
			double w1 = (z11 * (x2 - x) + z21 * (x - x1)) / (x2 - x1);
			double w2 = (z12 * (x2 - x) + z22 * (x - x1)) / (x2 - x1);
			return (w1 * (y2 - y) + w2 * (y - y1)) / (y2 - y1);
		}

		private static bool IsValidMatrix(double[,] x) {
			for (int i = 0; i < x.GetLength(0); i++) {
				for (int j = 0; j < x.GetLength(1); j++) {
					double y = x[i, j];
					if (double.IsNaN(y) || double.IsInfinity(y)) {
						return false;
					}
				}
			}
			return true;
		}

		private static double[] UnifySupport(double[] x1, double[] x2) {
			if (x1.Length == 0) {
				return x2;
			}
			if (x2.Length == 0) {
				return x1;
			}
			double avDiff1 = 0;
			for (int i = 0; i < x1.Length - 1; i++) {
				avDiff1 += x1[i + 1] - x1[i];
			}
			avDiff1 /= x1.Length - 1;
			double avDiff2 = 0;
			for (int i = 0; i < x2.Length - 1; i++) {
				avDiff2 += x2[i + 1] - x2[i];
			}
			avDiff2 /= x2.Length - 1;
			double diff = Math.Min(avDiff1, avDiff2);
			double min = Math.Min(x1[0], x2[0]);
			double max = Math.Max(x1[x1.Length - 1], x2[x2.Length - 1]);
			List<double> xvals = new List<double>();
			for (double val = min; val <= max; val += diff) {
				xvals.Add(val);
			}
			return xvals.ToArray();
		}

		private static double[,] Interpolate(IList<double> newX, IList<double> newY, double[] xvals, double[] yvals,
			double[,] zvals) {
			double[,] newZ = new double[newX.Count, newY.Count];
			for (int i = 0; i < newX.Count; i++) {
				for (int j = 0; j < newY.Count; j++) {
					newZ[i, j] = GetValue(newX[i], newY[j], xvals, yvals, zvals);
				}
			}
			return newZ;
		}

		private static double InterpolateExactY(int yind, double x, double[] xvals, double[,] zvals) {
			if (x <= xvals[0]) {
				return zvals[0, yind];
			}
			if (x >= xvals[xvals.Length - 1]) {
				return zvals[xvals.Length - 1, yind];
			}
			int ax = Array.BinarySearch(xvals, x);
			if (ax >= 0) {
				return zvals[ax, yind];
			}
			int i1 = -2 - ax;
			int i2 = i1 + 1;
			double x1 = xvals[i1];
			double x2 = xvals[i2];
			double w1 = zvals[i1, yind];
			double w2 = zvals[i2, yind];
			return (w1 * (x2 - x) + w2 * (x - x1)) / (x2 - x1);
		}

		private static double InterpolateExactX(int xind, double y, double[] yvals, double[,] zvals) {
			if (y <= yvals[0]) {
				return zvals[xind, 0];
			}
			if (y >= yvals[yvals.Length - 1]) {
				return zvals[xind, yvals.Length - 1];
			}
			int ay = Array.BinarySearch(yvals, y);
			if (ay >= 0) {
				return zvals[xind, ay];
			}
			int j1 = -2 - ay;
			int j2 = j1 + 1;
			double y1 = yvals[j1];
			double y2 = yvals[j2];
			double w1 = zvals[xind, j1];
			double w2 = zvals[xind, j2];
			return (w1 * (y2 - y) + w2 * (y - y1)) / (y2 - y1);
		}

		private const int nmax = 600;

		private static void EstimateBivariateDensity(double[,] data, out double[] xvalsO, out double[] yvalsO,
			out double[,] zvalsO, double xmin, double xmax, double ymin, double ymax, double[,] hinv, int nthreads) {
			double bandWidthX = 2.0 / hinv[0, 0];
			double bandWidthY = 2.0 / hinv[1, 1];
			bandWidthX = Math.Max(bandWidthX, (xmax - xmin) / nmax);
			bandWidthY = Math.Max(bandWidthY, (ymax - ymin) / nmax);
			List<double> xv = new List<double>();
			for (double val = xmin; val <= xmax; val += bandWidthX) {
				xv.Add(val);
			}
			double[] xvals = xv.ToArray();
			List<double> yv = new List<double>();
			for (double val = ymin; val <= ymax; val += bandWidthY) {
				yv.Add(val);
			}
			double[] yvals = yv.ToArray();
			int nx = xvals.Length;
			int ny = yvals.Length;
			double[,] zvals = new double[nx, ny];
			nthreads = Math.Min(nthreads, nx);
			int[] inds = new int[nthreads + 1];
			for (int i = 0; i < nthreads + 1; i++) {
				inds[i] = (int) Math.Round(i / (double) (nthreads) * (nx));
			}
			Thread[] t = new Thread[nthreads];
			for (int i = 0; i < nthreads; i++) {
				int i0 = i;
				t[i] = new Thread(new ThreadStart(delegate {
					for (int i1 = inds[i0]; i1 < inds[i0 + 1]; i1++) {
						for (int j = 0; j < ny; j++) {
							zvals[i1, j] = EstimateDensity(xvals[i1], yvals[j], data, hinv);
						}
					}
				}));
				t[i].Start();
			}
			for (int i = 0; i < nthreads; i++) {
				t[i].Join();
			}
			xvalsO = xvals;
			yvalsO = yvals;
			zvalsO = zvals;
		}

		private static double EstimateDensity(double x, double y, double[,] data, double[,] hinv) {
			double result = 0;
			for (int i = 0; i < data.GetLength(0); i++) {
				double[] w = {x - data[i, 0], y - data[i, 1]};
				double[] b = NumUtils.MatrixTimesVector(hinv, w);
				result += StandardGaussian(b);
			}
			result *= NumUtils.Determinant2X2(hinv) / data.Length;
			return result;
		}

		public static double StandardGaussian2(double[] x) {
			double sum = 0;
			foreach (double t in x) {
				sum += t * t;
			}
			sum = Math.Sqrt(sum);
			return Math.Exp(-0.5 * sum) / Math.Pow(2 * Math.PI, 0.5 * x.Length);
		}

		public static double StandardGaussian(double[] x) {
			double sum = 0;
			foreach (double t in x) {
				sum += t * t;
			}
			return Math.Exp(-0.5 * sum) / Math.Pow(2 * Math.PI, 0.5 * x.Length);
		}

		public bool IsValid() {
			return zout != null;
		}
	}
}