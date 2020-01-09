using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api {
	public abstract class RegressionFeatureRankingMethod : INamedListItem {
		public abstract int[] Rank(BaseVector[] x, double[] y, Parameters param, IGroupDataProvider data, int nthreads);
		public abstract Parameters GetParameters(IGroupDataProvider data);
		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract float DisplayRank { get; }
		public abstract bool IsActive { get; }
	}
}