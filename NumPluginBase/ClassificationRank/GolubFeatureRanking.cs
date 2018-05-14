using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.ClassificationRank{
	public class GolubFeatureRanking : ClassificationFeatureRankingMethod{
		public override int[] Rank(BaseVector[] x, int[][] y, int ngroups, Parameters param, IGroupDataProvider data,
			int nthreads, Action<double> reportProgress){
			int nfeatures = x[0].Length;
			int[][] yy = RearrangeGroups(y, ngroups);
			double[][] score = new double[ngroups][];
			for (int k = 0; k < ngroups; k++){
				score[k] = new double[nfeatures];
			}
			for (int i = 0; i < nfeatures; i++){
				double[] xx = new double[x.Length];
				for (int j = 0; j < xx.Length; j++){
					xx[j] = x[j][i];
				}
				double[] singleFeatureScores = CalcGolubScore(xx, yy, ngroups);
				for (int c = 0; c < ngroups; ++c){
					score[c][i] = singleFeatureScores[c];
				}
			}
			return CombineRankedFeaturesLists(nfeatures, score);
		}

		private static int[][] RearrangeGroups(IList<int[]> y, int ngroups){
			List<int>[] result = new List<int>[ngroups];
			for (int i = 0; i < ngroups; i++){
				result[i] = new List<int>();
			}
			for (int i = 0; i < y.Count; i++){
				foreach (int w in y[i]){
					result[w].Add(i);
				}
			}
			int[][] r = new int[ngroups][];
			for (int i = 0; i < r.Length; i++){
				r[i] = result[i].ToArray();
			}
			return r;
		}

		/*
        * Use the association of each feature to a given class
        * to estimate its importance for the prediction (Golub)
        * 
        *     (mean_i_gr1 - mean_i_gr2)
        * w = --------------------------, in case of two groups 
        *    (stdev_i_gr1 + stdev_i_gr2)
        *    
        * ALTERNATIVELY (after Pavlidis 2000)
        * 
        *        (mean_i_gr1 - mean_i_gr2)^2
        * w = -----------------------------------, in case of two groups 
        *    ((stdev_i_gr1)^2 + (stdev_i_gr2)^2)
        */

		private static double[] CalcGolubScore(IList<double> x, IList<int[]> y, int ngroups){
			double[] means = new double[ngroups];
			double[] stdevs = new double[ngroups];
			for (int i = 0; i < ngroups; i++){
				means[i] = ArrayUtils.Mean(ArrayUtils.ExtractValidValues(ArrayUtils.SubArray(x, y[i])));
				stdevs[i] = ArrayUtils.StandardDeviation(ArrayUtils.ExtractValidValues(ArrayUtils.SubArray(x, y[i])));
			}
			double[] modelWeights = new double[ngroups];
			int modelCount = 0;
			for (int m = 0; m < ngroups; ++m){
				for (int m2 = m + 1; m2 < ngroups; ++m2){
					double w = (means[m] - means[m2])/(stdevs[m] + stdevs[m2]);
					modelWeights[modelCount++] = Math.Abs(w);
				}
			}
			return modelWeights;
		}

		private static int[] CombineRankedFeaturesLists(int nfeatures, IList<double[]> score){
			int[][] featuresLists = new int[score.Count][];
			for (int c = 0; c < score.Count; ++c){
				int[] rankedList = ArrayUtils.Order(score[c]);
				ArrayUtils.Revert(rankedList);
				featuresLists[c] = new int[nfeatures];
				featuresLists[c] = rankedList;
			}
			List<int> ranked = new List<int>();
			for (int j = 0; j < featuresLists[0].Length; ++j){
				foreach (int[] t in featuresLists){
					if (!ranked.Contains(t[j])){
						ranked.Add(t[j]);
					}
				}
			}
			return ranked.ToArray();
		}

		public override Parameters GetParameters(IGroupDataProvider data){
			return new Parameters();
		}

		public override string Name => "Golub";
		public override string Description => "";
		public override float DisplayRank => 20;
		public override bool IsActive => true;
	}
}