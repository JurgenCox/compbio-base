using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api {
	public abstract class ClassificationMethod : INamedListItem {
		/// <summary>
		/// Create a classification model based on the given training data x with group assignments in y.
		/// </summary>
		/// <param name="x">The training data for which the group assignment is known. <code>x.Length</code> 
		/// is the number of training instances. All <code>BaseVector</code> instances in the array must have 
		/// the same length.</param>
		/// <param name="nominal">Indicates if a feature is nominal. Has the same length as the <code>BaseVector</code> 
		/// instances in the <code>x</code> array. In case it is null, all features are assumed to be numerical. If it is
		/// not null, each array element corresponds to a feature. If the value is less than 2, the corresponding feature 
		/// is assumed to be numerical. Otherwise the feture is nominal, with the value indicating the number of possible 
		/// classes for this nominal feature. The classes are assumed to be encoded as zero-based integer values in the 
		/// corresponding positions in the BaseVector instances of the training data.</param>
		/// <param name="y">The group assignments. <code>y.Length</code> is the number of training instances.
		/// In principle each training item can be assigned to multiple groups which is why this is an
		/// array of arrays. Each item has to be assigned to at least one group.</param>
		/// <param name="ngroups">The number of groups which has to be at least two.</param>
		/// <param name="param"><code>Parameters</code> object holding the user-defined values for the parameters
		/// of the classification algorithm.</param>
		/// <param name="nthreads">Number of threads the algorithm can use in case it supports parallelization.</param>
		/// <param name="reportProgress">Call back to return a number between 0 and 1 reflecting the progress 
		/// of the calculation.</param>
		/// <returns></returns>
		public abstract ClassificationModel Train(BaseVector[] x, int[] nominal, int[][] y, int ngroups, Parameters param,
			int nthreads, Action<double> reportProgress);

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, Parameters param, int nthreads,
			Action<double> reportProgress) {
			return Train(x, null, y, ngroups, param, nthreads, reportProgress);
		}

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, Parameters param, int nthreads) {
			return Train(x, null, y, ngroups, param, nthreads, null);
		}

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, Parameters param) {
			return Train(x, null, y, ngroups, param, 1, null);
		}

		/// <summary>
		/// Gets the <code>Parameters</code> object which is to be filled with the user-defined values.
		/// </summary>
		public abstract Parameters Parameters { get; }

		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract float DisplayRank { get; }
		public abstract bool IsActive { get; }

		public static BaseVector[] ToOneHotEncoding(BaseVector[] x, int[] nominal) {
			if (nominal == null) {
				return x;
			}
			if (x.Length == 0) {
				return x;
			}
			int nfeatures = x[0].Length;
			if (nominal.Length != nfeatures) {
				throw new ArgumentException("Wrong length of 'nominal' array.");
			}
			int nominalCount;
			int[] inds = GetIndsToBeEncoded(nominal, out nominalCount);
			if (inds.Length == 0) {
				return x;
			}
			if (nominalCount == nfeatures) {
				return ToOneHotEncodingBoolean(x, nominal);
			}
			return ToOneHotEncodingMixed(x, nominal);
		}

		private static BaseVector[] ToOneHotEncodingMixed(BaseVector[] x, int[] nominal) {
			int len = CalcTotalLength(nominal);
			BaseVector[] result = new BaseVector[x.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = ToOneHotEncodingMixed(x[i], nominal, len);
			}
			return result;
		}

		private static BaseVector ToOneHotEncodingMixed(BaseVector x, int[] nominal, int len) {
			float[] r = new float[len];
			int pos = 0;
			for (int i = 0; i < x.Length; i++) {
				int nom = nominal[i];
				double w = x[i];
				if (nom <= 2) {
					r[pos] = (float) w;
					pos++;
				} else {
					r[pos + (int) Math.Round(w)] = 1;
					pos += nom;
				}
			}
			return new FloatArrayVector(r);
		}

		private static BaseVector[] ToOneHotEncodingBoolean(BaseVector[] x, int[] nominal) {
			int len = CalcTotalLength(nominal);
			BaseVector[] result = new BaseVector[x.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = ToOneHotEncodingBoolean(x[i], nominal, len);
			}
			return result;
		}

		private static BaseVector ToOneHotEncodingBoolean(BaseVector x, int[] nominal, int len) {
			bool[] r = new bool[len];
			int pos = 0;
			for (int i = 0; i < x.Length; i++) {
				int nom = nominal[i];
				int w = (int) Math.Round(x[i]);
				if (nom == 2) {
					r[pos] = w == 1;
					pos++;
				} else {
					r[pos + w] = true;
					pos += nom;
				}
			}
			return new BoolArrayVector(r);
		}

		private static int CalcTotalLength(int[] nominal) {
			int len = 0;
			foreach (int t in nominal) {
				if (t > 2) {
					len += t;
				} else {
					len += 1;
				}
			}
			return len;
		}

		private static int[] GetIndsToBeEncoded(int[] nominal, out int nominalCount) {
			List<int> result = new List<int>();
			nominalCount = 0;
			for (int i = 0; i < nominal.Length; i++) {
				if (nominal[i] > 2) {
					result.Add(i);
				}
				if (nominal[i] > 1) {
					nominalCount++;
				}
			}
			return result.ToArray();
		}
	}
}