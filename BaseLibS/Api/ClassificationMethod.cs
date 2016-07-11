using System;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api{
	public abstract class ClassificationMethod : INamedListItem{
		/// <summary>
		/// Create a classification model based on the given training data x with group assignments in y..
		/// </summary>
		/// <param name="x">The training data for which the group assignment is known. <code>x.Length</code> 
		/// is the number of training instances.</param>
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
		public abstract ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, Parameters param, int nthreads,
			Action<double> reportProgress);

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, Parameters param, int nthreads){
			return Train(x, y, ngroups, param, nthreads, null);
		}

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, Parameters param){
			return Train(x, y, ngroups, param, 1, null);
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