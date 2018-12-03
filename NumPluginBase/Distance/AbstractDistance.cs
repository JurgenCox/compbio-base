using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	[Serializable]
	public abstract class AbstractDistance : IDistance {
		public abstract object Clone();
		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract float DisplayRank { get; }
		public abstract bool IsActive { get; }
		public abstract Parameters Parameters { get; set; }
		public abstract double Get(IList<float> x, IList<float> y);
		public abstract double Get(IList<double> x, IList<double> y);
		public abstract double Get(BaseVector x, BaseVector y);
		public abstract bool IsAngular { get; }

		public virtual double Get(float[,] data1, float[,] data2, int index1, int index2, MatrixAccess access1,
			MatrixAccess access2) {
			int n = data1.GetLength(access1 == MatrixAccess.Rows ? 1 : 0);
			float[] x = new float[n];
			float[] y = new float[n];
			for (int i = 0; i < n; i++) {
				x[i] = access1 == MatrixAccess.Rows ? data1[index1, i] : data1[i, index1];
				y[i] = access2 == MatrixAccess.Rows ? data2[index2, i] : data2[i, index2];
			}
			return Get(x, y);
		}

		public virtual double Get(double[,] data1, double[,] data2, int index1, int index2, MatrixAccess access1,
			MatrixAccess access2) {
			int n = data1.GetLength(access1 == MatrixAccess.Rows ? 1 : 0);
			double[] x = new double[n];
			double[] y = new double[n];
			for (int i = 0; i < n; i++) {
				x[i] = access1 == MatrixAccess.Rows ? data1[index1, i] : data1[i, index1];
				y[i] = access2 == MatrixAccess.Rows ? data2[index2, i] : data2[i, index2];
			}
			return Get(x, y);
		}
	}
}