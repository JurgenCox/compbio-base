using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using NumPluginBase.Kernel;
using NumPluginSvm.Svm;

namespace NumPluginSvm {
	public class LinearSvmRfeFeatureRanking : ClassificationFeatureRankingMethod {
		public override Parameters GetParameters(IGroupDataProvider data) {
			return new Parameters(new DoubleParam("C", 100) {Help = SvmClassification.cHelp},
				new DoubleParam("Reduction factor", 1.414) {
					Help = "The feature set will be recursively reduced in size by this factor."
				});
		}

		public override string Name => "RFE-SVM";
		public override string Description => "";
		public override float DisplayRank => 0;
		public override bool IsActive => true;

		public override int[] Rank(BaseVector[] x, int[][] y, int ngroups, Parameters param, IGroupDataProvider data,
			int nthreads, Action<double> reportProgress) {
			SvmParameter sp = new SvmParameter {
				kernelFunction = new LinearKernelFunction(),
				svmType = SvmType.CSvc,
				c = param.GetParam<double>("C").Value
			};
			double redfactor = param.GetParam<double>("Reduction factor").Value;
			bool[] invert;
			SvmProblem[] problems = CreateProblems(x, y, ngroups, out invert);
			int[][] rankedSets = new int[problems.Length][];
			for (int i = 0; i < problems.Length; ++i) {
				rankedSets[i] = RankBinary(problems[i], sp, redfactor);
			}
			return CombineRankedFeaturesLists(rankedSets);
		}

		private static int[] RankBinary(SvmProblem prob, SvmParameter param, double redfactor) {
			int nfeatures = prob.x[0].Length;
			int[] result = new int[nfeatures];
			int[] survivingFeatures = ArrayUtils.ConsecutiveInts(nfeatures);
			SvmProblem problem = prob.Copy();
			int p = nfeatures - 1;
			int[] indices = ArrayUtils.ConsecutiveInts(nfeatures);
			while (survivingFeatures.Length > 0) {
				problem = problem.ExtractFeatures(survivingFeatures);
				indices = ArrayUtils.SubArray(indices, survivingFeatures);
				int nfeatures2 = survivingFeatures.Length;
				double[] criteria = ComputeRankingCriteria(SvmMain.SvmTrain(problem, param)
					.ComputeBinaryClassifierWeights(nfeatures2));
				int[] order = ArrayUtils.Order(criteria);
				int numFeaturesToRemove = Math.Max((int) Math.Round(nfeatures2 / redfactor), 1);
				for (int i = 0; i < numFeaturesToRemove; ++i) {
					result[p--] = indices[order[i]];
				}
				survivingFeatures = ArrayUtils.SubArray(order, numFeaturesToRemove, nfeatures2);
			}
			return result;
		}

		private static double[] ComputeRankingCriteria(IList<double> weights) {
			double[] rankCriteria = new double[weights.Count];
			for (int i = 0; i < weights.Count; i++) {
				rankCriteria[i] = weights[i] * weights[i];
			}
			return rankCriteria;
		}

		private static int[] CombineRankedFeaturesLists(IList<int[]> featuresLists) {
			List<int> ranked = new List<int>();
			for (int j = 0; j < featuresLists[0].Length; ++j) {
				foreach (int[] t in featuresLists) {
					if (!ranked.Contains(t[j])) {
						ranked.Add(t[j]);
					}
				}
			}
			return ranked.ToArray();
		}

		private static SvmProblem[] CreateProblems(IList<BaseVector> x, IList<int[]> y, int ngroups, out bool[] invert) {
			if (ngroups == 2) {
				invert = new bool[1];
				return new[] {CreateProblem(x, y, 0, out invert[0])};
			}
			SvmProblem[] result = new SvmProblem[ngroups];
			invert = new bool[ngroups];
			for (int i = 0; i < ngroups; i++) {
				result[i] = CreateProblem(x, y, i, out invert[i]);
			}
			return result;
		}

		private static SvmProblem CreateProblem(IList<BaseVector> x, IList<int[]> y, int index, out bool invert) {
			double[] y1 = new double[y.Count];
			for (int i = 0; i < y.Count; i++) {
				if (Array.BinarySearch(y[i], index) >= 0) {
					y1[i] = 1;
				} else {
					y1[i] = 0;
				}
			}
			invert = Array.BinarySearch(y[0], index) < 0;
			return new SvmProblem(x, y1);
		}
	}
}