using System;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Num{
	[Serializable]
	public class LinearInterpolatorMixed{
		public double[] xvals;
		private float[] yvals;

		public LinearInterpolatorMixed(double[] xvals, float[] yvals){
			this.xvals = xvals;
			this.yvals = yvals;
		}

		public LinearInterpolatorMixed(float val){
			xvals = new[]{0.0, 1.0};
			yvals = new[]{val, val};
		}

		public LinearInterpolatorMixed(BinaryReader reader){
			int len = reader.ReadInt32();
			xvals = new double[len];
			yvals = new float[len];
			for (int i = 0; i < len; i++){
				xvals[i] = reader.ReadDouble();
				yvals[i] = reader.ReadSingle();
			}
		}

		public void Write(BinaryWriter writer){
			writer.Write(xvals.Length);
			for (int i = 0; i < xvals.Length; i++){
				writer.Write(xvals[i]);
				writer.Write(yvals[i]);
			}
		}

		public void AddConstant(float c){
			for (int i = 0; i < yvals.Length; i++){
				yvals[i] += c;
			}
		}

		public void Multiply(float c){
			for (int i = 0; i < yvals.Length; i++){
				yvals[i] *= c;
			}
		}

		public double MinX => xvals[0];
		public double MaxX => xvals[xvals.Length - 1];

		public LinearInterpolator Scale(double mean, double width, double height){
			double[] newX = new double[xvals.Length];
			double[] newY = new double[xvals.Length];
			for (int i = 0; i < newX.Length; i++){
				newX[i] = xvals[i] * width + mean;
				newY[i] = yvals[i] * height;
			}
			return new LinearInterpolator(newX, newY);
		}

		public float Get(double x){
			return Get(x, xvals, yvals);
		}

		public float Get(double x, out double dydx){
			return Get(x, out dydx, xvals, yvals);
		}

		public static float Get(double x, out double dydx, double[] xvals, float[] yvals){
			if (double.IsNaN(x) || double.IsInfinity(x)){
				dydx = double.NaN;
				return float.NaN;
			}
			if (xvals.Length == 0){
				dydx = double.NaN;
				return float.NaN;
			}
			if (xvals.Length == 1){
				dydx = 0;
				return yvals[0];
			}
			int i;
			if (x <= xvals[0]){
				for (i = 1; i < xvals.Length; i++){
					if (xvals[i] != xvals[0]){
						return Interpolate(xvals[0], xvals[i], yvals[0], yvals[i], x, out dydx);
					}
				}
				throw new Exception("Values are the same");
			}
			if (x >= xvals[xvals.Length - 1]){
				for (i = xvals.Length - 2; i >= 0; i--){
					if (xvals[i] != xvals[xvals.Length - 1]){
						return Interpolate(xvals[i], xvals[xvals.Length - 1], yvals[i], yvals[xvals.Length - 1], x,
							out dydx);
					}
				}
				throw new Exception("Values are the same");
			}
			int a = Array.BinarySearch(xvals, x);
			if (a >= 0){
				Interpolate(xvals[-2 - a], xvals[-1 - a], yvals[-2 - a], yvals[-1 - a], x, out dydx);
				return yvals[a];
			}
			return Interpolate(xvals[-2 - a], xvals[-1 - a], yvals[-2 - a], yvals[-1 - a], x, out dydx);
		}

		public static float Get(double x, double[] xvals, float[] yvals){
			if (double.IsNaN(x) || double.IsInfinity(x)){
				return float.NaN;
			}
			if (xvals.Length == 0){
				return float.NaN;
			}
			if (xvals.Length == 1){
				return yvals[0];
			}
			int i;
			if (x <= xvals[0]){
				for (i = 1; i < xvals.Length; i++){
					if (xvals[i] != xvals[0]){
						return Interpolate(xvals[0], xvals[i], yvals[0], yvals[i], x);
					}
				}
				throw new Exception("Values are the same");
			}
			if (x >= xvals[xvals.Length - 1]){
				for (i = xvals.Length - 2; i >= 0; i--){
					if (xvals[i] != xvals[xvals.Length - 1]){
						return Interpolate(xvals[i], xvals[xvals.Length - 1], yvals[i], yvals[xvals.Length - 1], x);
					}
				}
				throw new Exception("Values are the same");
			}
			int a = Array.BinarySearch(xvals, x);
			return a >= 0 ? yvals[a] : Interpolate(xvals[-2 - a], xvals[-1 - a], yvals[-2 - a], yvals[-1 - a], x);
		}

		public double Get(double x, out double[] dyda){
			return Get(x, out dyda, xvals, yvals);
		}

		public static double Get(double x, out double[] dyda, double[] xvals, float[] yvals){
			dyda = new double[xvals.Length];
			if (double.IsNaN(x) || double.IsInfinity(x)){
				return double.NaN;
			}
			if (xvals.Length == 0){
				return double.NaN;
			}
			if (xvals.Length == 1){
				dyda[0] = 1;
				return yvals[0];
			}
			int i;
			if (x <= xvals[0]){
				for (i = 1; i < xvals.Length; i++){
					if (xvals[i] != xvals[0]){
						dyda[0] = Dy1(xvals[0], xvals[i], x);
						dyda[i] = Dy2(xvals[0], xvals[i], x);
						return Interpolate(xvals[0], xvals[i], yvals[0], yvals[i], x);
					}
				}
				throw new Exception("Values are the same");
			}
			if (x >= xvals[xvals.Length - 1]){
				for (i = xvals.Length - 2; i >= 0; i--){
					if (xvals[i] != xvals[xvals.Length - 1]){
						dyda[i] = Dy1(i, xvals[xvals.Length - 1], x);
						dyda[xvals.Length - 1] = Dy2(xvals[i], xvals[xvals.Length - 1], x);
						return Interpolate(xvals[i], xvals[xvals.Length - 1], yvals[i], yvals[xvals.Length - 1], x);
					}
				}
				throw new Exception("Values are the same");
			}
			int a = Array.BinarySearch(xvals, x);
			if (a >= 0){
				dyda[a] = 1;
				return yvals[a];
			}
			dyda[-2 - a] = Dy1(xvals[-2 - a], xvals[-1 - a], x);
			dyda[-1 - a] = Dy2(xvals[-2 - a], xvals[-1 - a], x);
			return Interpolate(xvals[-2 - a], xvals[-1 - a], yvals[-2 - a], yvals[-1 - a], x);
		}

		private static double Dy2(double x1, double x2, double x){
			return (x - x1) / (x2 - x1);
		}

		private static double Dy1(double x1, double x2, double x){
			return (x2 - x) / (x2 - x1);
		}

		private static float Interpolate(double x1, double x2, float y1, float y2, double x){
			return (float) ((y2 - y1) / (x2 - x1) * (x - x1) + y1);
		}

		private static float Interpolate(double x1, double x2, float y1, float y2, double x, out double dydx){
			dydx = (y2 - y1) / (x2 - x1);
			return (float) (dydx * (x - x1) + y1);
		}

		public void Dispose(){
			xvals = null;
			yvals = null;
		}

		public void FlattenEnds(){
			if (xvals.Length == 0){
				xvals = new[]{0.0, 1.0};
				yvals = new[]{0.0f, 0.0f};
				return;
			}
			if (xvals.Length == 1){
				xvals = new[]{0.0, 1.0};
				yvals = new[]{yvals[0], yvals[0]};
				return;
			}
			double[] newx = new double[xvals.Length + 2];
			float[] newy = new float[xvals.Length + 2];
			newx[0] = 2 * xvals[0] - xvals[1];
			newy[0] = yvals[0];
			for (int i = 0; i < xvals.Length; i++){
				newx[i + 1] = xvals[i];
				newy[i + 1] = yvals[i];
			}
			newx[xvals.Length + 1] = 2 * xvals[xvals.Length - 1] - xvals[xvals.Length - 2];
			newy[xvals.Length + 1] = yvals[xvals.Length - 1];
			xvals = newx;
			yvals = newy;
		}

		public void Write(string filename){
			StreamWriter writer = new StreamWriter(filename);
			writer.WriteLine("x" + "\t" + "y");
			for (int i = 0; i < xvals.Length; i++){
				writer.WriteLine(Parser.ToString(xvals[i]) + "\t" + Parser.ToString(yvals[i]));
			}
			writer.Close();
		}

		public LinearInterpolator Minus(){
			return new LinearInterpolator((double[]) xvals.Clone(), Negate((double[]) yvals.Clone()));
		}

		private double[] Negate(double[] doubles){
			for (int i = 0; i < doubles.Length; i++){
				doubles[i] *= -1;
			}
			return doubles;
		}
	}
}