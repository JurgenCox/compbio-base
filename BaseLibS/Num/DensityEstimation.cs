using System;
using System.Collections.Generic;

namespace BaseLibS.Num{
	public static class DensityEstimation{
		public static void CalcRanges(IList<double> xvals, IList<double> yvals, out double xmin, out double xmax,
			out double ymin, out double ymax){
			xmin = double.MaxValue;
			xmax = double.MinValue;
			ymin = double.MaxValue;
			ymax = double.MinValue;
			for (int i = 0; i < xvals.Count; i++){
				if (double.IsInfinity(xvals[i]) || double.IsInfinity(yvals[i]) || double.IsNaN(xvals[i]) || double.IsNaN(yvals[i])){
					continue;
				}
				if (xvals[i] < xmin){
					xmin = xvals[i];
				}
				if (xvals[i] > xmax){
					xmax = xvals[i];
				}
				if (yvals[i] < ymin){
					ymin = yvals[i];
				}
				if (yvals[i] > ymax){
					ymax = yvals[i];
				}
			}
			double dx = xmax - xmin;
			double dy = ymax - ymin;
			xmin -= 0.05*dx;
			xmax += 0.05*dx;
			ymin -= 0.05*dy;
			ymax += 0.05*dy;
		}

		public static void CalcRanges(IList<double> xvals, IList<double> yvals, IList<double> zvals, out double xmin,
			out double xmax, out double ymin, out double ymax, out double zmin, out double zmax){
			xmin = double.MaxValue;
			xmax = double.MinValue;
			ymin = double.MaxValue;
			ymax = double.MinValue;
			zmin = double.MaxValue;
			zmax = double.MinValue;
			for (int i = 0; i < xvals.Count; i++){
				if (double.IsInfinity(xvals[i]) || double.IsInfinity(yvals[i]) || double.IsInfinity(zvals[i]) ||
					double.IsNaN(xvals[i]) || double.IsNaN(yvals[i]) || double.IsNaN(zvals[i])){
					continue;
				}
				if (xvals[i] < xmin){
					xmin = xvals[i];
				}
				if (xvals[i] > xmax){
					xmax = xvals[i];
				}
				if (yvals[i] < ymin){
					ymin = yvals[i];
				}
				if (yvals[i] > ymax){
					ymax = yvals[i];
				}
				if (zvals[i] < zmin){
					zmin = zvals[i];
				}
				if (zvals[i] > zmax){
					zmax = zvals[i];
				}
			}
			double dx = xmax - xmin;
			double dy = ymax - ymin;
			double dz = zmax - zmin;
			xmin -= 0.05*dx;
			xmax += 0.05*dx;
			ymin -= 0.05*dy;
			ymax += 0.05*dy;
			zmin -= 0.05*dz;
			zmax += 0.05*dz;
		}

		public static double[,] GetValuesOnGrid(IList<double> xvals, double minx, double xStep, int xCount, IList<double> yvals,
			double miny, double yStep, int yCount){
			double[,] vals = new double[xCount,yCount];
			if (xvals == null || yvals == null){
				return vals;
			}
			int n = xvals.Count;
			double[,] cov = NumUtils.CalcCovariance(new[]{xvals, yvals});
			double fact = Math.Pow(n, 1.0/6.0);
			double[,] hinv = NumUtils.ApplyFunction(cov, w => fact/Math.Sqrt(w));
			hinv[0, 0] *= xStep;
			hinv[1, 0] *= xStep;
			hinv[0, 1] *= yStep;
			hinv[1, 1] *= yStep;
			int dx = (int) (1.0/hinv[0, 0]*5);
			int dy = (int) (1.0/hinv[1, 1]*5);
			for (int i = 0; i < xvals.Count; i++){
				double xval = xvals[i];
				if (double.IsNaN(xval)){
					continue;
				}
				int xind = (int) Math.Floor((xval - minx)/xStep);
				double yval = yvals[i];
				if (double.IsNaN(yval)){
					continue;
				}
				int yind = (int) Math.Floor((yval - miny)/yStep);
				for (int ii = Math.Max(xind - dx, 0); ii <= Math.Min(xind + dx, xCount - 1); ii++){
					for (int jj = Math.Max(yind - dy, 0); jj <= Math.Min(yind + dy, yCount - 1); jj++){
						double[] w = {ii - xind, jj - yind};
						double[] b = NumUtils.MatrixTimesVector(hinv, w);
						vals[ii, jj] += NumUtils.StandardGaussian(b);
					}
				}
			}
			return vals;
		}

		public static double[,,] GetValuesOnGrid(IList<double> xvals, double minx, double xStep, int xCount, IList<double> yvals,
			double miny, double yStep, int yCount, IList<double> zvals, double minz, double zStep, int zCount){
			double[,,] vals = new double[xCount,yCount,zCount];
			if (xvals == null || yvals == null || zvals == null){
				return vals;
			}
			int n = xvals.Count;
			double[,] cov = NumUtils.CalcCovariance(new[]{xvals, yvals, zvals});
			double fact = Math.Pow(n, 1.0/6.0);
			double[,] hinv = NumUtils.ApplyFunction(cov, w => fact/Math.Sqrt(w));
			hinv[0, 0] *= xStep;
			hinv[1, 0] *= xStep;
			hinv[2, 0] *= xStep;
			hinv[0, 1] *= yStep;
			hinv[1, 1] *= yStep;
			hinv[2, 1] *= yStep;
			hinv[0, 2] *= zStep;
			hinv[1, 2] *= zStep;
			hinv[2, 2] *= zStep;
			int dx = (int) (1.0/hinv[0, 0]*5);
			int dy = (int) (1.0/hinv[1, 1]*5);
			int dz = (int) (1.0/hinv[2, 2]*5);
			for (int i = 0; i < xvals.Count; i++){
				double xval = xvals[i];
				if (double.IsNaN(xval)){
					continue;
				}
				int xind = (int) Math.Floor((xval - minx)/xStep);
				double yval = yvals[i];
				if (double.IsNaN(yval)){
					continue;
				}
				int yind = (int) Math.Floor((yval - miny)/yStep);
				double zval = zvals[i];
				if (double.IsNaN(zval)){
					continue;
				}
				int zind = (int) Math.Floor((zval - minz)/zStep);
				for (int ii = Math.Max(xind - dx, 0); ii <= Math.Min(xind + dx, xCount - 1); ii++){
					for (int jj = Math.Max(yind - dy, 0); jj <= Math.Min(yind + dy, yCount - 1); jj++){
						for (int kk = Math.Max(zind - dz, 0); kk <= Math.Min(zind + dz, zCount - 1); kk++){
							double[] w = {ii - xind, jj - yind, kk - zind};
							double[] b = NumUtils.MatrixTimesVector(hinv, w);
							vals[ii, jj, kk] += NumUtils.StandardGaussian(b);
						}
					}
				}
			}
			return vals;
		}

		public static void DivideByMaximum(double[,] m){
			double max = 0;
			for (int i = 0; i < m.GetLength(0); i++){
				for (int j = 0; j < m.GetLength(1); j++){
					if (m[i, j] > max){
						max = m[i, j];
					}
				}
			}
			for (int i = 0; i < m.GetLength(0); i++){
				for (int j = 0; j < m.GetLength(1); j++){
					m[i, j] /= max;
				}
			}
		}
	}
}