using System;
using BaseLibS.Num.Func;

namespace BaseLibS.Num.Test{
	public class FisherExactTest{

        /// <summary>
        /// P-value calculation from q_xy of the contingency table. See <see cref="GetLogFisherP"/> for log p-value
        /// and <see cref="CalcContingency"/> for calculating the contingency table.
        /// </summary>
        /// <param name="q00"></param>
        /// <param name="q01"></param>
        /// <param name="q10"></param>
        /// <param name="q11"></param>
        /// <returns></returns>
		public static double Test(int q00, int q01, int q10, int q11){
			return Math.Exp(GetLogFisherP(q00, q01, q10, q11));
		}

        /// <summary>
        /// Calculate contingency table and p-value, see <see cref="CalcContingency"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
		public static double Test(bool[] x, bool[] y){
            CalcContingency(x, y, out int q00, out int q01, out int q10, out int q11);
            return Test(q00, q01, q10, q11);
		}

        /// <summary>
        /// Log p-value calculation from q_xy of the contingency table.
        /// </summary>
        /// <param name="q00"></param>
        /// <param name="q01"></param>
        /// <param name="q10"></param>
        /// <param name="q11"></param>
        /// <returns></returns>
		public static double GetLogFisherP(int q00, int q01, int q10, int q11){
			int rowSum0 = q00 + q01;
			int rowSum1 = q10 + q11;
			int colSum0 = q00 + q10;
			int colSum1 = q01 + q11;
			int total = rowSum0 + rowSum1;
			return Factorial.LnValue(rowSum0) + Factorial.LnValue(rowSum1) + Factorial.LnValue(colSum0) +
					Factorial.LnValue(colSum1) - Factorial.LnValue(q00) - Factorial.LnValue(q01) - Factorial.LnValue(q10) -
					Factorial.LnValue(q11) - Factorial.LnValue(total);
		}

        /// <summary>
        /// Calculate all 4 counts of the 2x2 contingency table.
        /// Count for (x,y) in (xs, ys):
        ///         |y=True |y=False|
        /// |x=True |  r00  |  r10  |
        /// |x=False|  r01  |  r11  |
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        /// <param name="r00"></param>
        /// <param name="r01"></param>
        /// <param name="r10"></param>
        /// <param name="r11"></param>
		public static void CalcContingency(bool[] xs, bool[] ys, out int r00, out int r01, out int r10, out int r11){
            var table = CalcContingencyTable(xs, ys);
            r00 = table[0, 0];
            r01 = table[0, 1];
            r10 = table[1, 0];
            r11 = table[1, 1];
        }

        /// <summary>
        /// Calculate all 4 counts of the 2x2 contingency table.
        /// Count for (x,y) in (xs, ys):
        ///         |y=True |y=False|
        /// |x=True |  r00  |  r10  |
        /// |x=False|  r01  |  r11  |
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        public static int[,] CalcContingencyTable(bool[] xs, bool[] ys)
		{
		    var table = new int[2, 2];
			for (int i = 0; i < xs.Length; i++)
			{
			    int row = xs[i] ? 0 : 1;
			    int col = ys[i] ? 0 : 1;
			    table[row, col]++;
			}
		    return table;
		}
	}
}