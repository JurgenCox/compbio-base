using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using BaseLibS.Util;

namespace BaseLibS.Num.Learning{
	[Serializable]
	public class ClassificationWithRanking{
		private readonly ClassificationMethod classifier;
		private readonly ClassificationFeatureRankingMethod ranker;
		private readonly int nfeatures;
		private readonly Parameters classifierParam;
		private readonly Parameters rankerParam;
		private readonly bool groupWiseSelection;
		private readonly int[] groupWiseNfeatures;

		public ClassificationWithRanking(ClassificationMethod classifier, ClassificationFeatureRankingMethod ranker,
			int nfeatures, Parameters classifierParam, Parameters rankerParam, bool groupWiseSelection, int[] groupWiseNfeatures){
			this.classifier = classifier;
			this.ranker = ranker;
			this.nfeatures = nfeatures;
			this.classifierParam = classifierParam;
			this.rankerParam = rankerParam;
			this.groupWiseSelection = groupWiseSelection;
			this.groupWiseNfeatures = groupWiseNfeatures;
		}

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, IGroupDataProvider data, int nthreads){
			return Train(x, y, ngroups, data, nthreads, null);
		}

		public ClassificationModel Train(BaseVector[] x, int[][] y, int ngroups, IGroupDataProvider data, int nthreads,
			Action<double> reportProgress){
			return groupWiseSelection
						? TrainGroupWise(x, y, ngroups, data, nthreads)
						: TrainGlobal(x, y, ngroups, data, nthreads, reportProgress);
		}

		public ClassificationModel TrainGroupWise(BaseVector[] x, int[][] y, int ngroups, IGroupDataProvider data,
			int nthreads){
			ClassificationModel[] c = new ClassificationModel[ngroups];
			ThreadDistributor td = new ThreadDistributor(nthreads, ngroups, i => { c[i] = TrainGroupWise(i, y, x, data); });
			td.Start();
			return new GroupWiseClassifier(c);
		}

		public ClassificationModel TrainGroupWise(int i, int[][] y, BaseVector[] x, IGroupDataProvider data){
			int[][] yb = ToBinaryClasses(y, i);
			if (ranker == null || groupWiseNfeatures[i] >= x[0].Length){
				return classifier.Train(x, yb, 2, classifierParam, 1);
			}
			int[] o = ranker.Rank(x, yb, 2, rankerParam, data, 1);
			int[] inds = nfeatures < o.Length ? ArrayUtils.SubArray(o, groupWiseNfeatures[i]) : o;
			return new ClassificationOnSubFeatures(classifier.Train(ExtractFeatures(x, inds), yb, 2, classifierParam, 1), inds);
		}

		public static int[][] ToBinaryClasses(IList<int[]> ints, int index){
			int[][] result = new int[ints.Count][];
			for (int i = 0; i < result.Length; i++){
				result[i] = ToBinaryClasses(ints[i], index);
			}
			return result;
		}

		private static int[] ToBinaryClasses(IList<int> ints, int index){
			if (ints.Count == 1){
				return ints[0] == index ? new[]{0} : new[]{1};
			}
			return Contains(ints, index) ? new[]{0, 1} : new[]{1};
		}

		private static bool Contains(IEnumerable<int> ints, int index){
			foreach (int i in ints){
				if (i == index){
					return true;
				}
			}
			return false;
		}

		public ClassificationModel TrainGlobal(BaseVector[] x, int[][] y, int ngroups, IGroupDataProvider data, int nthreads,
			Action<double> reportProgress){
			if (ranker == null || nfeatures >= x[0].Length){
				return classifier.Train(x, y, ngroups, classifierParam, nthreads, reportProgress);
			}
			int[] o = ranker.Rank(x, y, ngroups, rankerParam, data, nthreads);
			int[] inds = nfeatures < o.Length ? ArrayUtils.SubArray(o, nfeatures) : o;
			return
				new ClassificationOnSubFeatures(classifier.Train(ExtractFeatures(x, inds), y, ngroups, classifierParam, nthreads),
					inds);
		}

		public static BaseVector[] ExtractFeatures(IList<BaseVector> x, IList<int> inds){
			BaseVector[] result = new BaseVector[x.Count];
			for (int i = 0; i < x.Count; i++){
				result[i] = x[i].SubArray(inds);
			}
			return result;
		}
	}
}