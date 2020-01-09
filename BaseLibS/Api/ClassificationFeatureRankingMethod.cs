using System;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api{
	public abstract class ClassificationFeatureRankingMethod : INamedListItem{
		public abstract int[] Rank(BaseVector[] x, int[][] y, int ngroups, Parameters param, IGroupDataProvider data,
			int nthreads, Action<double> reportProgress);

		public int[] Rank(BaseVector[] x, int[][] y, int ngroups, Parameters param, IGroupDataProvider data, int nthreads){
			return Rank(x, y, ngroups, param, data, nthreads, null);
		}

		public int[] Rank(BaseVector[] x, int[][] y, int ngroups, Parameters param, IGroupDataProvider data){
			return Rank(x, y, ngroups, param, data, 1, null);
		}

		public abstract Parameters GetParameters(IGroupDataProvider data);
		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract float DisplayRank { get; }
		public abstract bool IsActive { get; }
	}
}