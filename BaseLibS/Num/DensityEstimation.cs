using System;
using System.Collections.Generic;

namespace BaseLibS.Num {
	public static class DensityEstimation {
		public static (double[] dvals, double[] pvals) CalcDensitiesAtData(double[] xvals, double[] yvals, int points,
			DensityEstimationType type) {
			(double[,] values, double[] xmat, double[] ymat) = CalcDensityOnGrid(xvals, yvals, points, type);
			double[,] percvalues = CalcExcludedPercentage(values);
			double[] dvals = new double[xvals.Length];
			double[] pvals = new double[xvals.Length];
			for (int i = 0; i < dvals.Length; i++) {
				double xx = xvals[i];
				double yy = yvals[i];
				if (!double.IsNaN(xx) && !double.IsNaN(yy)) {
					int xind = ArrayUtils.ClosestIndex(xmat, xx);
					int yind = ArrayUtils.ClosestIndex(ymat, yy);
					dvals[i] = values[xind, yind];
					pvals[i] = percvalues[xind, yind];
				} else {
					dvals[i] = double.NaN;
					pvals[i] = double.NaN;
				}
			}
			return (dvals, pvals);
		}

		public static (double[,], double[], double[]) CalcDensityOnGrid(double[] xvals, double[] yvals, int points,
			DensityEstimationType type) {
			GetValidPairs(xvals, yvals, out double[] xvals1, out double[] yvals1);
			CalcRanges(xvals1, yvals1, out double xmin, out double xmax, out double ymin, out double ymax);
			double[,] values = GetValuesOnGrid(xvals1, xmin, (xmax - xmin) / points, points, yvals1, ymin,
				(ymax - ymin) / points, points);
			if (type == DensityEstimationType.DivideByX) {
				MakeConditional1(values);
			}
			if (type == DensityEstimationType.DivideByY) {
				MakeConditional2(values);
			}
			if (type == DensityEstimationType.DivideByXY) {
				MakeConditional3(values);
			}
			DivideByMaximum(values);
			double[] xmat = new double[points];
			for (int i = 0; i < points; i++) {
				xmat[i] = xmin + i * (xmax - xmin) / points;
			}
			double[] ymat = new double[points];
			for (int i = 0; i < points; i++) {
				ymat[i] = ymin + i * (ymax - ymin) / points;
			}
			return (values, xmat, ymat);
		}

		public static void CalcRanges(IList<double> xvals, IList<double> yvals, out double xmin, out double xmax,
			out double ymin, out double ymax) {
			xmin = double.MaxValue;
			xmax = double.MinValue;
			ymin = double.MaxValue;
			ymax = double.MinValue;
			for (int i = 0; i < xvals.Count; i++) {
				if (double.IsInfinity(xvals[i]) || double.IsInfinity(yvals[i]) || double.IsNaN(xvals[i]) ||
				    double.IsNaN(yvals[i])) {
					continue;
				}
				if (xvals[i] < xmin) {
					xmin = xvals[i];
				}
				if (xvals[i] > xmax) {
					xmax = xvals[i];
				}
				if (yvals[i] < ymin) {
					ymin = yvals[i];
				}
				if (yvals[i] > ymax) {
					ymax = yvals[i];
				}
			}
			double dx = xmax - xmin;
			double dy = ymax - ymin;
			xmin -= 0.05 * dx;
			xmax += 0.05 * dx;
			ymin -= 0.05 * dy;
			ymax += 0.05 * dy;
		}

        public static void TimeCalcRanges(IList<double> xvals, out double xmin, out double xmax)
        {
            xmin = double.MaxValue;
            xmax = double.MinValue;

            for (int i = 0; i < xvals.Count; i++)
            {
                if (double.IsInfinity(xvals[i])  || double.IsNaN(xvals[i]) )
                {  continue; }
                if (xvals[i] < xmin)
                {  xmin = xvals[i]; }
                if (xvals[i] > xmax)
                {   xmax = xvals[i];  }
            }
            double dx = xmax - xmin;
            xmin -= 0.01 * dx;
            xmax += 0.01 * dx;
        }

        public static void CalcRanges(IList<double> xvals, IList<double> yvals, IList<double> zvals, out double xmin,
			out double xmax, out double ymin, out double ymax, out double zmin, out double zmax) {
			xmin = double.MaxValue;
			xmax = double.MinValue;
			ymin = double.MaxValue;
			ymax = double.MinValue;
			zmin = double.MaxValue;
			zmax = double.MinValue;
			for (int i = 0; i < xvals.Count; i++) {
				if (double.IsInfinity(xvals[i]) || double.IsInfinity(yvals[i]) || double.IsInfinity(zvals[i]) ||
				    double.IsNaN(xvals[i]) || double.IsNaN(yvals[i]) || double.IsNaN(zvals[i])) {
					continue;
				}
				if (xvals[i] < xmin) {
					xmin = xvals[i];
				}
				if (xvals[i] > xmax) {
					xmax = xvals[i];
				}
				if (yvals[i] < ymin) {
					ymin = yvals[i];
				}
				if (yvals[i] > ymax) {
					ymax = yvals[i];
				}
				if (zvals[i] < zmin) {
					zmin = zvals[i];
				}
				if (zvals[i] > zmax) {
					zmax = zvals[i];
				}
			}
			double dx = xmax - xmin;
			double dy = ymax - ymin;
			double dz = zmax - zmin;
			xmin -= 0.05 * dx;
			xmax += 0.05 * dx;
			ymin -= 0.05 * dy;
			ymax += 0.05 * dy;
			zmin -= 0.05 * dz;
			zmax += 0.05 * dz;
		}

		public static double[,] GetValuesOnGrid(IList<double> xvals, double minx, double xStep, int xCount,
			IList<double> yvals, double miny, double yStep, int yCount) {
			double[,] vals = new double[xCount, yCount];
			if (xvals == null || yvals == null) {
				return vals;
			}
			int n = xvals.Count;
			double[,] cov = NumUtils.CalcCovariance(new[] {xvals, yvals});
			double fact = Math.Pow(n, 1.0 / 6.0);
			double[,] hinv = NumUtils.ApplyFunction(cov, w => fact / Math.Sqrt(w));
			hinv[0, 0] *= xStep;
			hinv[1, 0] *= xStep;
			hinv[0, 1] *= yStep;
			hinv[1, 1] *= yStep;
			int dx = (int) (1.0 / hinv[0, 0] * 5);
			int dy = (int) (1.0 / hinv[1, 1] * 5);
			for (int i = 0; i < xvals.Count; i++) {
				double xval = xvals[i];
				if (double.IsNaN(xval)) {
					continue;
				}
				int xind = (int) Math.Floor((xval - minx) / xStep);
				double yval = yvals[i];
				if (double.IsNaN(yval)) {
					continue;
				}
				int yind = (int) Math.Floor((yval - miny) / yStep);
				for (int ii = Math.Max(xind - dx, 0); ii <= Math.Min(xind + dx, xCount - 1); ii++) {
					for (int jj = Math.Max(yind - dy, 0); jj <= Math.Min(yind + dy, yCount - 1); jj++) {
						double[] w = {ii - xind, jj - yind};
						double[] b = NumUtils.MatrixTimesVector(hinv, w);
						vals[ii, jj] += NumUtils.StandardGaussian(b);
					}
				}
			}
			return vals;
		}

		public static double[,,] GetValuesOnGrid(IList<double> xvals, double minx, double xStep, int xCount,
			IList<double> yvals, double miny, double yStep, int yCount, IList<double> zvals, double minz, double zStep,
			int zCount) {
			double[,,] vals = new double[xCount, yCount, zCount];
			if (xvals == null || yvals == null || zvals == null) {
				return vals;
			}
			int n = xvals.Count;
			double[,] cov = NumUtils.CalcCovariance(new[] {xvals, yvals, zvals});
			double fact = Math.Pow(n, 1.0 / 6.0);
			double[,] hinv = NumUtils.ApplyFunction(cov, w => fact / Math.Sqrt(w));
			hinv[0, 0] *= xStep;
			hinv[1, 0] *= xStep;
			hinv[2, 0] *= xStep;
			hinv[0, 1] *= yStep;
			hinv[1, 1] *= yStep;
			hinv[2, 1] *= yStep;
			hinv[0, 2] *= zStep;
			hinv[1, 2] *= zStep;
			hinv[2, 2] *= zStep;
			int dx = (int) (1.0 / hinv[0, 0] * 5);
			int dy = (int) (1.0 / hinv[1, 1] * 5);
			int dz = (int) (1.0 / hinv[2, 2] * 5);
			for (int i = 0; i < xvals.Count; i++) {
				double xval = xvals[i];
				if (double.IsNaN(xval)) {
					continue;
				}
				int xind = (int) Math.Floor((xval - minx) / xStep);
				double yval = yvals[i];
				if (double.IsNaN(yval)) {
					continue;
				}
				int yind = (int) Math.Floor((yval - miny) / yStep);
				double zval = zvals[i];
				if (double.IsNaN(zval)) {
					continue;
				}
				int zind = (int) Math.Floor((zval - minz) / zStep);
				for (int ii = Math.Max(xind - dx, 0); ii <= Math.Min(xind + dx, xCount - 1); ii++) {
					for (int jj = Math.Max(yind - dy, 0); jj <= Math.Min(yind + dy, yCount - 1); jj++) {
						for (int kk = Math.Max(zind - dz, 0); kk <= Math.Min(zind + dz, zCount - 1); kk++) {
							double[] w = {ii - xind, jj - yind, kk - zind};
							double[] b = NumUtils.MatrixTimesVector(hinv, w);
							vals[ii, jj, kk] += NumUtils.StandardGaussian(b);
						}
					}
				}
			}
			return vals;
		}

		public static void DivideByMaximum(double[,] m) {
			double max = 0;
			for (int i = 0; i < m.GetLength(0); i++) {
				for (int j = 0; j < m.GetLength(1); j++) {
					if (m[i, j] > max) {
						max = m[i, j];
					}
				}
			}
			for (int i = 0; i < m.GetLength(0); i++) {
				for (int j = 0; j < m.GetLength(1); j++) {
					m[i, j] /= max;
				}
			}
		}

		public static void MakeConditional1(double[,] values) {
			double[] m = new double[values.GetLength(0)];
			for (int i = 0; i < m.Length; i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					m[i] += values[i, j];
				}
			}
			for (int i = 0; i < m.Length; i++) {
				if (m[i] == 0) {
					continue;
				}
				for (int j = 0; j < values.GetLength(1); j++) {
					values[i, j] /= m[i];
				}
			}
		}

		public static void MakeConditional2(double[,] values) {
			double[] m = new double[values.GetLength(1)];
			for (int i = 0; i < m.Length; i++) {
				for (int j = 0; j < values.GetLength(0); j++) {
					m[i] += values[j, i];
				}
			}
			for (int i = 0; i < m.Length; i++) {
				if (m[i] == 0) {
					continue;
				}
				for (int j = 0; j < values.GetLength(0); j++) {
					values[j, i] /= m[i];
				}
			}
		}

		public static void MakeConditional3(double[,] values) {
			double[] m1 = new double[values.GetLength(0)];
			double[] m2 = new double[values.GetLength(1)];
			for (int i = 0; i < m1.Length; i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					m1[i] += values[i, j];
					m2[j] += values[i, j];
				}
			}
			for (int i = 0; i < m1.Length; i++) {
				if (m1[i] == 0) {
					continue;
				}
				for (int j = 0; j < values.GetLength(1); j++) {
					if (m2[j] == 0) {
						continue;
					}
					values[i, j] /= m1[i] * m2[j];
				}
			}
		}

		public static double[,] CalcExcludedPercentage(double[,] values) {
			int n0 = values.GetLength(0);
			int n1 = values.GetLength(1);
			double[] v = new double[n0 * n1];
			int[] ind0 = new int[n0 * n1];
			int[] ind1 = new int[n0 * n1];
			int count = 0;
			for (int i0 = 0; i0 < n0; i0++) {
				for (int i1 = 0; i1 < n1; i1++) {
					v[count] = values[i0, i1];
					ind0[count] = i0;
					ind1[count] = i1;
					count++;
				}
			}
			int[] o = ArrayUtils.Order(v);
			v = ArrayUtils.SubArray(v, o);
			ind0 = ArrayUtils.SubArray(ind0, o);
			ind1 = ArrayUtils.SubArray(ind1, o);
			double total = 0;
			foreach (double t in v) {
				total += t;
			}
			double[,] result = new double[n0, n1];
			double sum = 0;
			for (int i = 0; i < v.Length; i++) {
				result[ind0[i], ind1[i]] = sum / total;
				sum += v[i];
			}
			return result;
		}

		public static void GetValidPairs(IList<double> x, IList<double> y, out double[] x1, out double[] y1) {
			List<double> x2 = new List<double>();
			List<double> y2 = new List<double>();
			for (int i = 0; i < x.Count; i++) {
				if (!double.IsNaN(x[i]) && !double.IsInfinity(x[i]) && !double.IsNaN(y[i]) &&
				    !double.IsInfinity(y[i])) {
					x2.Add(x[i]);
					y2.Add(y[i]);
				}
			}
			x1 = x2.ToArray();
			y1 = y2.ToArray();
		}

		public static (double[], double[], double[], double[]) Regression(double[] xvals, double[] yvals)
		{
			const int numPoints = 300;
			
			(double[,] values, double[] xmat, double[] ymat) =
				CalcDensityOnGrid(xvals, yvals, numPoints, DensityEstimationType.DivideByX);
			double[] yfit = new double[numPoints];
			double[] yupper = new double[numPoints];
			double[] ylower = new double[numPoints];
			for (int i = 0; i < xmat.Length; i++) {
				int maxInd = -1;
				double maxVal = double.MinValue;
				for (int j = 0; j < ymat.Length; j++) {
					if (values[i, j] > maxVal) {
						maxVal = values[i, j];
						maxInd = j;
					}
				}
				yfit[i] = ymat[maxInd];
				int upperInd = ymat.Length;
				for (int j = maxInd; j < ymat.Length; j++) {
					if (values[i, j] < 0.5 * maxVal) {
						upperInd = j;
						break;
					}
				}
				yupper[i] = upperInd == ymat.Length ? ymat[upperInd - 1] : ymat[upperInd];
				int lowerInd = -1;
				for (int j = maxInd; j >= 0; j--) {
					if (values[i, j] < 0.5 * maxVal) {
						lowerInd = j;
						break;
					}
				}
				ylower[i] = lowerInd == -1 ? ymat[0] : ymat[lowerInd];
			}
			return (xmat, yfit, ylower, yupper);
		}
	}
}