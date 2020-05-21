using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Mol;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using BaseLibS.Util;

namespace NumPluginBase.Classification{
	public static class ClassificationUtil{
		public static IntParam GetNumberOfThreadsParam(int n){
			return new IntParam("Number of threads", n){
				Help = "Specify here the number of logical processors that should be used by this activity."
			};
		}

		public static bool CheckIfOverlapping(IEnumerable<int[]> ints){
			foreach (int[] i in ints){
				if (i.Length > 1){
					return true;
				}
			}
			return false;
		}

		public static string[][] FilterMinGroupSize(IList<string[]> groups, int minGroupSize){
			string[] allGroups = ArrayUtils.UniqueValues(ArrayUtils.Concat(groups));
			int[] groupCounts = new int[allGroups.Length];
			foreach (string[] g in groups){
				foreach (string s in g){
					int index = Array.BinarySearch(allGroups, s);
					groupCounts[index]++;
				}
			}
			HashSet<string> toBeRemoved = new HashSet<string>();
			for (int i = 0; i < groupCounts.Length; i++){
				if (groupCounts[i] < minGroupSize){
					toBeRemoved.Add(allGroups[i]);
				}
			}
			string[][] result = new string[groups.Count][];
			for (int i = 0; i < result.Length; i++){
				List<string> a = new List<string>();
				foreach (string s in groups[i]){
					if (!toBeRemoved.Contains(s)){
						a.Add(s);
					}
				}
				result[i] = a.ToArray();
			}
			return result;
		}

		public static int[][] TransformSubGroups(IList<int> subY, int length){
			if (subY == null){
				int[][] result = new int[length][];
				for (int i = 0; i < result.Length; i++){
					result[i] = new[]{i};
				}
				return result;
			}
			int n = ArrayUtils.UniqueValues(subY).Length;
			List<int>[] r = new List<int>[n];
			for (int i = 0; i < n; i++){
				r[i] = new List<int>();
			}
			for (int i = 0; i < subY.Count; i++){
				r[subY[i]].Add(i);
			}
			int[][] rx = new int[r.Length][];
			for (int i = 0; i < r.Length; i++){
				rx[i] = r[i].ToArray();
			}
			return rx;
		}

		public static void GetRandomSamplingSubsets(int testSetSize, int totalSize, out int[][] trainingSets,
			out int[][] testSets, int numberOfRepeats, IList<int[]> y, int ngroups){
			Random2 rand = new Random2(7);
			List<int[]> train = new List<int[]>();
			List<int[]> test = new List<int[]>();
			int[] testCount = new int[totalSize];
			int i = 0;
			for (;;){
				int[] candidateTestSet = i % 3 == 0 && 2 * i >= numberOfRepeats
					? GetSubset(testSetSize, testCount, rand)
					: GetRandomSubset(testSetSize, totalSize, rand);
				Array.Sort(candidateTestSet);
				int[] trainSet = ArrayUtils.Complement(candidateTestSet, totalSize);
				if (y != null && !ValidTrainSet(y.SubArray(trainSet), ngroups)){
					continue;
				}
				test.Add(candidateTestSet);
				train.Add(trainSet);
				if (test.Count >= numberOfRepeats){
					break;
				}
				foreach (int i1 in candidateTestSet){
					testCount[i1]++;
				}
				i++;
			}
			trainingSets = train.ToArray();
			testSets = test.ToArray();
		}

		private static bool ValidTrainSet(IList<int[]> y, int ngroups){
			int[] vals = ArrayUtils.UniqueValues(ArrayUtils.Concat(y));
			return vals.Length == ngroups;
		}

		private static int[] GetSubset(int testSetSize, IList<int> testCount, Random2 rand){
			int[] o = testCount.Order();
			int biggestTestCount = testCount[o[testSetSize - 1]];
			List<int> v = new List<int>();
			for (int i = 0; i < testCount.Count; i++){
				if (testCount[i] <= biggestTestCount){
					v.Add(i);
				}
			}
			return v.Count == testSetSize
				? v.ToArray()
				: v.SubArray(rand.NextPermutation(v.Count)).SubArray(testSetSize);
		}

		private static int[] GetRandomSubset(int trainSetSize, int totalSize, Random2 rand){
			int[] x = rand.NextPermutation(totalSize);
			return x.SubArray(trainSetSize);
		}

		public static int[][] GetNfoldSubGroups(int nitems, int n){
			return GetNfoldSubGroups(nitems, n, out Dictionary<int, int> _);
		}

		public static int[][] GetNfoldSubGroups(int nitems, int n, out Dictionary<int, int> index2TestGroup){
			Random2 rand = new Random2(7);
			index2TestGroup = new Dictionary<int, int>();
			int[] perm = rand.NextPermutation(nitems);
			n = Math.Min(n, nitems);
			List<int>[] g = new List<int>[n];
			for (int i = 0; i < n; i++){
				g[i] = new List<int>();
			}
			for (int i = 0; i < perm.Length; i++){
				g[i % n].Add(perm[i]);
				index2TestGroup.Add(perm[i], i % n);
			}
			int[][] gg = new int[g.Length][];
			for (int i = 0; i < gg.Length; i++){
				gg[i] = g[i].ToArray();
			}
			return gg;
		}

		public static string CheckSubY(IList<int> subY, int nsub, IList<int[]> y){
			List<int>[] x = new List<int>[nsub];
			for (int i = 0; i < nsub; i++){
				x[i] = new List<int>();
			}
			for (int i = 0; i < subY.Count; i++){
				x[subY[i]].Add(i);
			}
			for (int i = 0; i < nsub; i++){
				int[][] y1 = y.SubArray(x[i]);
				if (!AllEqual(y1)){
					return "Sub groups are split between groups";
				}
			}
			return null;
		}

		private static bool AllEqual(IList<int[]> y1){
			if (y1.Count < 2){
				return true;
			}
			int[] first = y1[0];
			for (int i = 1; i < y1.Count; i++){
				if (!ArrayUtils.EqualArrays(first, y1[i])){
					return false;
				}
			}
			return true;
		}

		public static int[] GetIntSubGroups(string[] allSubGroups, IList<string[]> trainSubGroups){
			int[][] y = GetIntGroups(allSubGroups, trainSubGroups);
			int[] result = new int[y.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = y[i][0];
			}
			return result;
		}

		public static int[][] GetIntGroups(string[] allGroups, IList<string[]> trainGroups){
			int[][] y = new int[trainGroups.Count][];
			for (int i = 0; i < trainGroups.Count; i++){
				string[] gg = trainGroups[i];
				int[] y1 = new int[gg.Length];
				for (int j = 0; j < gg.Length; j++){
					y1[j] = Array.IndexOf(allGroups, gg[j]);
				}
				y[i] = ArrayUtils.UniqueValues(y1);
			}
			return y;
		}

		public static string CheckSubGroups(IEnumerable<string[]> trainSubGroups){
			if (trainSubGroups == null){
				return null;
			}
			foreach (string[] s in trainSubGroups){
				if (s.Length == 0){
					return "Each assigned item has to be in a sub group";
				}
				if (s.Length > 1){
					return "Sub groups have to be uniquely assigned.";
				}
			}
			return null;
		}

		public static SingleChoiceWithSubParams GetCrossValidationParam(){
			return new SingleChoiceWithSubParams("Cross-validation type"){
				Values = new[]{"Leave one out", "n-fold", "Random sampling"},
				SubParams =
					new[]{
						new Parameters(),
						new Parameters(new IntParam("n", 4){
							Help = "Number of splits of the items into training and test sets."
						}),
						new Parameters(
							new IntParam("Test set percentage", 15){
								Help =
									"The percentage of items taken out to form the test set and not used for building the predictor."
							},
							new IntParam("Number of repeats", 250){
								Help =
									"Number of times the items are split into training and test sets and a predictor is constructed."
							})
					},
				Value = 1,
				ParamNameWidth = 136,
				TotalWidth = 580,
				Help =
					"Three types of cross validation are offered. a) 'Leave one out': As many predictors are built as there are items. For each " +
					"predictor one item is left out and the remaining items form the training set. The predictor is evaluated on the left out item. " +
					"b) 'n-fold': The items are split into n equally sized chunks. n predictors will be generated. In each of these prediction models " +
					"the union of n-1 of these chunks are taken as the training set and the remaining chunk is the test set. c: 'Random sampling': Here the " +
					"number of predictors is specified by the 'Number of repeats' parameter. The number of items taken out to form the test set " +
					"(and not used for building the predictor) is specified by the 'Test set percentage' parameter."
			};
		}

		public static double[] SequenceRegressionCrossValidation(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, SequenceRegressionMethod cme, Parameters param, int nthreads,
			AllModifications allMods){
			const int nfolds = 5;
			int[][] nfoldSubGroups = ClassificationUtil.GetNfoldSubGroups(sequences.Length, nfolds, out _);
			double[] result = new double[sequences.Length];
			int nToplevelThreads = Math.Min(nthreads, nfolds);
			ThreadDistributor td = new ThreadDistributor(nToplevelThreads, nfolds, i => {
				int[] testInds = nfoldSubGroups[i];
				int[] trainInds = ArrayUtils.Concat(ArrayUtils.RemoveAtIndex(nfoldSubGroups, i));
				string[] trainSeq = sequences.SubArray(trainInds);
				PeptideModificationState[] trainMod = modifications.SubArray(trainInds);
				BaseVector[] trainMetadata = metadata?.SubArray(trainInds);
				double[] trainY = y.SubArray(trainInds);
				SequenceRegressionModel cm = cme.Train(trainSeq, trainMod, trainMetadata, trainY, allMods, param, 1);
				foreach (int testInd in testInds){
					result[testInd] = cm.Predict(sequences[testInd], modifications[testInd], metadata?[testInd]);
				}
			});
			td.Start();
			return result;
		}
	}
}