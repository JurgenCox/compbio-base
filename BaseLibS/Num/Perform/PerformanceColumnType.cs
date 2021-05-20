using System;
using System.Collections.Generic;

namespace BaseLibS.Num.Perform{
	public abstract class PerformanceColumnType{
		public static PerformanceColumnType precision = new PerformanceColumnTypePrecision();
		public static PerformanceColumnType recall = new PerformanceColumnTypeRecall();
		public static PerformanceColumnType sensitivity = new PerformanceColumnTypeSensitivity();
		public static PerformanceColumnType specificity = new PerformanceColumnTypeSpecificity();
		public static PerformanceColumnType tpFraction = new PerformanceColumnTypeTpFraction();
		public static PerformanceColumnType tnFraction = new PerformanceColumnTypeTnFraction();
		public static PerformanceColumnType fpFraction = new PerformanceColumnTypeFpFraction();
		public static PerformanceColumnType fnFraction = new PerformanceColumnTypeFnFraction();

		public static PerformanceColumnType[] allTypes = {
			precision, recall, new PerformanceColumnTypeFpTp(), new PerformanceColumnTypeTpNp(), sensitivity,
			specificity, tpFraction, tnFraction, fpFraction, fnFraction
		};

		public static string[] AllTypeNames{
			get{
				string[] names = new string[allTypes.Length];
				for (int i = 0; i < names.Length; i++){
					names[i] = allTypes[i].Name;
				}
				return names;
			}
		}

		public abstract string Name{ get; }
		public abstract double Calculate(double tp, double tn, double fp, double fn, double np, double nn);

		public static (double[][] curves, int[] order) CalcCurves(bool[] indicatorCol, bool falseAreIndicated, double[] vals,
			bool largeIsGood, PerformanceColumnType[] types){
			if (falseAreIndicated){
				indicatorCol = ArrayUtils.Invert(indicatorCol);
			}
			int[] order = GetOrder(vals, largeIsGood);
			bool[] x = indicatorCol.SubArray(order);
			double[][] columns = new double[types.Length][];
			for (int i = 0; i < types.Length; i++){
				columns[i] = new double[x.Length + 1];
			}
			int np = 0;
			int nn = 0;
			foreach (bool t in x){
				if (t){
					np++;
				} else{
					nn++;
				}
			}
			double tp = 0;
			double fp = 0;
			double tn = nn;
			double fn = np;
			for (int j = 0; j < types.Length; j++){
				columns[j][0] = types[j].Calculate(tp, tn, fp, fn, np, nn);
			}
			for (int i = 0; i < x.Length; i++){
				if (x[i]){
					tp++;
					fn--;
				} else{
					fp++;
					tn--;
				}
				for (int j = 0; j < types.Length; j++){
					columns[j][i + 1] = types[j].Calculate(tp, tn, fp, fn, np, nn);
				}
			}
			return (columns, order);
		}

		private static int[] GetOrder(double[] vals, bool largeIsGood){
			List<int> valids = new List<int>();
			List<int> invalids = new List<int>();
			for (int i = 0; i < vals.Length; i++){
				if (double.IsNaN(vals[i])){
					invalids.Add(i);
				} else{
					valids.Add(i);
				}
			}
			vals = vals.SubArray(valids);
			int[] o = OrderValues(vals);
			o = valids.SubArray(o);
			if (largeIsGood){
				ArrayUtils.Revert(o);
			}
			return ArrayUtils.Concat(o, invalids.ToArray());
		}

		private static int[] OrderValues(IList<double> vals){
			int[] o = vals.Order();
			RandomizeConstantRegions(o, vals);
			return o;
		}

		private static void RandomizeConstantRegions(int[] o, IList<double> vals){
			int startInd = 0;
			for (int i = 1; i < o.Length; i++){
				if (vals[o[i]] != vals[o[startInd]]){
					if (i - startInd > 1){
						RandomizeConstantRegion(o, startInd, i);
					}
					startInd = i;
				}
			}
		}

		private static void RandomizeConstantRegion(int[] o, int startInd, int endInd){
			int len = endInd - startInd;
			Random2 r = new Random2(7);
			int[] p = r.NextPermutation(len);
			int[] permuted = new int[len];
			for (int i = 0; i < len; i++){
				permuted[i] = o[startInd + p[i]];
			}
			Array.Copy(permuted, 0, o, startInd, len);
		}
	}
}