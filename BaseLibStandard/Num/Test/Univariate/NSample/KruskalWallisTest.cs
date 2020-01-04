using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibS.Num.Test.Univariate.NSample{
	public class KruskalWallisTest : MultipleSampleTest{
		public override double Test(double[][] data, out double statistic, double s0, out double pvalS0, out double[] gmeans){
			pvalS0 = 1;
			gmeans = null;
			return TestImpl(data, out statistic);
		}

		public override bool HasSides => false;
		public override bool HasS0 => true;
		public override string Name => "Kruskal Wallis";

		/// <summary>
		/// Kruskal Wallis test is the extention of the Wilcoxon U test to more than two group
		/// Another words it is designed to test the equakity of medians of multiple sample
		/// </summary>
		/// <param name="data">Value of the first dimension is number of groups, second - size of the group</param>
		/// <param name="stat">H-statistic of the test</param>
		/// <returns></returns>
		public static double TestImpl(double[][] data, out double stat){
			int i, j, counter = 0;
			int[] arrN = new int[data.Length];
			List<double> x = new List<double>();
			for (i = 0; i < data.Length; i++){
				arrN[i] = data[i].Length;
				for (j = 0; j < arrN[i]; j++){
					x.Add(data[i][j]);
				}
			}
			int n = arrN.Sum();
			List<double> dataRank = ArrayUtils.Rank(x, true).ToList();
			List<int> numDuplicates = new List<int>();
			double[] dataRankSorted = dataRank.OrderBy(a => a).ToArray();
			for (i = 0; i < n; i++){
				counter++;
				if ((i == n - 1) || (dataRankSorted[i] != dataRankSorted[i + 1])){
					if (counter > 1){
						numDuplicates.Add(counter);
					}
					counter = 0;
				}
			}
			j = 0;
			double[] s = new double[data.Length];
			for (i = 0; i < data.Length; i++){
				s[i] = Math.Pow(dataRank.Skip(j).Take(arrN[i]).Select(d => d + 1).Sum(), 2)/arrN[i];
				j += arrN[i];
			}
			stat = (12*s.Sum()/(n*(n + 1)) - 3*(n + 1))/(1 - numDuplicates.Select(d => d*(d*d - 1)).Sum()/(Math.Pow(n, 3) - n));
			int df = data.Length - 1;
			return 1 - NumUtils.Gammq(stat*0.5, df*0.5);
		}
	}
}