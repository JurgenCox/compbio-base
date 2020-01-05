using System;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Classification{
	public class FisherLdaClassification : ClassificationMethod{
		public override ClassificationModel Train(BaseVector[] x, int[] nominal, int[][] y, int ngroups, Parameters param, int nthreads,
			Action<double> reportProgress) {
			x = ToOneHotEncoding(x, nominal);
			int n = x.Length;
			int p = x[0].Length;
			int[] groupCounts = new int[ngroups];
			int totalCount = 0;
			double[,] groupMeans = new double[ngroups, p];
			double[] totalMean = new double[p];
			for (int i = 0; i < n; i++){
				int groupIndex = y[i][0];
				groupCounts[groupIndex]++;
				totalCount++;
				for (int j = 0; j < p; j++){
					groupMeans[groupIndex, j] += x[i][j];
					totalMean[j] += x[i][j];
				}
			}
			for (int i = 0; i < ngroups; i++){
				for (int j = 0; j < p; j++){
					groupMeans[i, j] /= groupCounts[i];
				}
			}
			for (int j = 0; j < p; j++){
				totalMean[j] /= totalCount;
			}
			double[,] b = new double[p, p];
			for (int i = 0; i < p; i++){
				for (int j = 0; j < p; j++){
					for (int k = 0; k < ngroups; k++){
						b[i, j] += groupCounts[k]*(groupMeans[k, i] - totalMean[i])*(groupMeans[k, j] - totalMean[j]);
					}
				}
			}
			double[,] w = new double[p, p];
			for (int k = 0; k < n; k++){
				int groupIndex = y[k][0];
				for (int i = 0; i < p; i++){
					for (int j = 0; j < p; j++){
						w[i, j] += (x[k][i] - groupMeans[groupIndex, i])*(x[k][j] - groupMeans[groupIndex, j]);
					}
				}
			}
			double[,] x1;
			double[] e = NumUtils.GeneralizedEigenproblem(b, w, out x1);
			int[] order = ArrayUtils.Order(e);
			int[] indices = new int[ngroups - 1];
			for (int i = 0; i < ngroups - 1; i++){
				indices[i] = order[order.Length - 1 - i];
			}
			e = ArrayUtils.SubArray(e, indices);
			double[,] projection = ExtractColumns(x1, indices);
			double[][] projectedGroupMeans = MatrixTimesMatrix(groupMeans, projection);
			return new FisherLdaClassificationModel(projection, projectedGroupMeans, ngroups);
		}

		public static double[,] ExtractColumns(double[,] x, int[] indices){
			int n = x.GetLength(0);
			int ncol = indices.Length;
			double[,] result = new double[n, ncol];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < ncol; j++){
					result[i, j] = (float) x[i, indices[j]];
				}
			}
			return result;
		}

		public static double[][] MatrixTimesMatrix(double[,] a, double[,] b){
			double[][] result = new double[a.GetLength(0)][];
			for (int i = 0; i < result.Length; i++){
				result[i] = new double[b.GetLength(1)];
			}
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < b.GetLength(1); j++){
					for (int k = 0; k < a.GetLength(1); k++){
						result[i][j] += a[i, k]*b[k, j];
					}
				}
			}
			return result;
		}

		public static float[][] MatrixTimesMatrix(float[,] a, float[,] b){
			float[][] result = new float[a.GetLength(0)][];
			for (int i = 0; i < result.Length; i++){
				result[i] = new float[b.GetLength(1)];
			}
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < b.GetLength(1); j++){
					for (int k = 0; k < a.GetLength(1); k++){
						result[i][j] += a[i, k]*b[k, j];
					}
				}
			}
			return result;
		}

		public override Parameters Parameters => new Parameters();
		public override string Name => "Fisher LDA";
		public override string Description => "";
		public override float DisplayRank => 1;
		public override bool IsActive => true;
	}
}