using System;
using System.Collections.Generic;

namespace BaseLibS.Num {
	public class BayesianInversion1D {
		private readonly double[] xout;
		private readonly double[] zout;
		private readonly double[] forward;
		private readonly double[] reverse;
		public BayesianInversion1D(IList<double> xdata, IList<bool> correct) : this(xdata, correct, false) { }

		public BayesianInversion1D(IList<double> xdata, IList<bool> correct, bool debug) {
			Invert(xdata, correct, out xout, out zout, debug, out forward, out reverse);
		}

		public double GetValue(double x) {
			return GetValue(x, xout, zout);
		}

		public double GetForwardHist(double x) {
			return GetValue(x, xout, forward);
		}

		public double GetReverseHist(double x) {
			return GetValue(x, xout, reverse);
		}

		private static void Invert(IList<double> xdata, IList<bool> correct, out double[] xout, out double[] zout, bool debug,
			out double[] forwardOut, out double[] reverseOut) {
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
				zout = null;
				forwardOut = null;
				reverseOut = null;
				return;
			}
			double[] falseData = new double[nfalse];
			double[] trueData = new double[ntrue];
			int iFalse = 0;
			int iTrue = 0;
			for (int i = 0; i < n; i++) {
				if (correct[i]) {
					trueData[iTrue] = xdata[i];
					iTrue++;
				} else {
					falseData[iFalse] = xdata[i];
					iFalse++;
				}
			}
			Invert(falseData, trueData, out xout, out zout, out forwardOut, out reverseOut, debug);
		}

		private static double GetValue(double x, double[] xvals, IList<double> zvals) {
			if (xvals == null) {
				return double.NaN;
			}
			if (xvals.Length == 0) {
				return double.NaN;
			}
			if (x <= xvals[0]) {
				return zvals[0];
			}
			if (x >= xvals[xvals.Length - 1]) {
				return zvals[xvals.Length - 1];
			}
			int ax = Array.BinarySearch(xvals, x);
			if (ax >= 0) {
				return zvals[ax];
			}
			int i1 = -2 - ax;
			int i2 = i1 + 1;
			if (i1 < 0) {
				return double.NaN;
			}
			if (i2 >= xvals.Length) {
				return double.NaN;
			}
			double x1 = xvals[i1];
			double x2 = xvals[i2];
			double z11 = zvals[i1];
			double z21 = zvals[i2];
			double w1 = (z11 * (x2 - x) + z21 * (x - x1)) / (x2 - x1);
			return Math.Min(1, w1);
		}

		private static void Invert(double[] falseData, double[] trueData, out double[] xRes, out double[] zRes,
			out double[] forwardOut, out double[] reverseOut, bool debug) {
			double xmin = double.MaxValue;
			double xmax = -double.MaxValue;
			for (int i = 0; i < falseData.GetLength(0); i++) {
				double xx = falseData[i];
				if (xx < xmin) {
					xmin = xx;
				}
				if (xx > xmax) {
					xmax = xx;
				}
			}
			for (int i = 0; i < trueData.GetLength(0); i++) {
				double xx = trueData[i];
				if (xx < xmin) {
					xmin = xx;
				}
				if (xx > xmax) {
					xmax = xx;
				}
			}
			double dx = xmax - xmin;
			xmin -= 0.1 * dx;
			xmax += 0.1 * dx;
			double[] falseX;
			double[] falseZ;
			int n = trueData.GetLength(0);
			double cov = ArrayUtils.CalcCovariance(trueData);
			double fact = Math.Pow(n, 1.0 / 5.0);
			double hinv = fact / Math.Sqrt(cov);
			if (hinv == 0) {
				xRes = null;
				zRes = null;
				forwardOut = null;
				reverseOut = null;
				return;
			}
			EstimateBivariateDensity(falseData, out falseX, out falseZ, xmin, xmax, hinv);
			double[] trueX;
			double[] trueZ;
			EstimateBivariateDensity(trueData, out trueX, out trueZ, xmin, xmax, hinv);
			double[] x = UnifySupport(falseX, trueX);
			falseZ = Interpolate(x, falseX, falseZ);
			trueZ = Interpolate(x, trueX, trueZ);
			double[] inverse = new double[x.Length];
			for (int i = 0; i < x.Length; i++) {
				inverse[i] = falseZ[i] <= 0 ? double.Epsilon : Math.Max((falseZ[i] * 0.5) / trueZ[i], double.Epsilon);
			}
			double maxVal = double.MinValue;
			int maxInd = -1;
			for (int i = 0; i < x.Length; i++) {
				if (inverse[i] > maxVal) {
					maxVal = inverse[i];
					maxInd = i;
				}
			}
			for (int i = 0; i < maxInd; i++) {
				inverse[i] = maxVal;
			}
			xRes = x;
			zRes = inverse;
			if (debug) {
				forwardOut = trueZ;
				reverseOut = falseZ;
			} else {
				forwardOut = null;
				reverseOut = null;
			}
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

		private static double[] Interpolate(IList<double> newX, double[] xvals, IList<double> zvals) {
			double[] newZ = new double[newX.Count];
			for (int i = 0; i < newX.Count; i++) {
				newZ[i] = GetValue(newX[i], xvals, zvals);
			}
			return newZ;
		}

		private static void EstimateBivariateDensity(double[] data, out double[] xvals, out double[] zvals, double xmin,
			double xmax, double hinv) {
			double bandWidthX = 1.0 / hinv;
			List<double> xv = new List<double>();
			for (double val = xmin; val <= xmax; val += bandWidthX) {
				xv.Add(val);
			}
			xvals = xv.ToArray();
			zvals = new double[xvals.Length];
			for (int i = 0; i < xvals.Length; i++) {
				zvals[i] = EstimateDensity(xvals[i], data, hinv);
			}
		}

		private static double EstimateDensity(double x, double[] data, double hinv) {
			double result = 0;
			for (int i = 0; i < data.GetLength(0); i++) {
				double w = x - data[i];
				double[] b = new[] {hinv * w};
				result += NumUtils.StandardGaussian(b);
			}
			result *= hinv / data.Length;
			return result;
		}

		public bool IsValid() {
			return zout != null;
		}
	}
}