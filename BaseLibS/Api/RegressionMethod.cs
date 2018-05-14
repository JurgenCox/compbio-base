using System;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api {
	public abstract class RegressionMethod : INamedListItem {
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
		/// <param name="y">The output variable. <code>y.Length</code> is the number of training instances.
		/// In principle each training item can be assigned to multiple groups which is why this is an
		/// array of arrays. Each item has to be assigned to at least one group.</param>
		/// <param name="param"><code>Parameters</code> object holding the user-defined values for the parameters
		/// of the classification algorithm.</param>
		/// <param name="nthreads">Number of threads the algorithm can use in case it supports parallelization.</param>
		/// <param name="reportProgress">Call back to return a number between 0 and 1 reflecting the progress 
		/// of the calculation.</param>
		/// <returns></returns>
		public abstract RegressionModel Train(BaseVector[] x, int[] nominal, double[] y, Parameters param,
			int nthreads, Action<double> reportProgress);

		public RegressionModel Train(BaseVector[] x, double[] y, Parameters param, int nthreads,
			Action<double> reportProgress) {
			return Train(x, null, y, param, nthreads, reportProgress);
		}

		public RegressionModel Train(BaseVector[] x, double[] y, Parameters param, int nthreads) {
			return Train(x, null, y, param, nthreads, null);
		}

		public RegressionModel Train(BaseVector[] x, double[] y, Parameters param) {
			return Train(x, null, y, param, 1, null);
		}

		/// <summary>
		/// Gets the <code>Parameters</code> object which is to be filled with the user-defined values.
		/// </summary>
		public abstract Parameters Parameters { get; }

		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract float DisplayRank { get; }
		public abstract bool IsActive { get; }
	}
}