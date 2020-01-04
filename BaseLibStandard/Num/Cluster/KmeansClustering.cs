using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Data;
using BaseLibS.Num.Matrix;

namespace BaseLibS.Num.Cluster {
	public class KmeansClustering {
		private static readonly Random2 randy = new Random2(7);

		/// <summary>
		/// Run k-means clustering
		/// </summary>
		/// <param name="data"></param>
		/// <param name="k">number of clusters</param>
		/// <param name="maxIter">maximal number of iterations, if not converging</param>
		/// <param name="restarts"></param>
		/// <param name="progress"></param>
		/// <param name="clusterCenters"></param>
		/// <param name="clusterIndices"></param>
		public static void GenerateClusters(MatrixIndexer data, int k, int maxIter, int restarts, Action<int> progress,
			out float[,] clusterCenters, out int[] clusterIndices) {
		    ExtractUniqueRows(data, out var rowIndexMap, out var reducedData);
		    GenerateClustersImpl(reducedData, k, maxIter, restarts, progress, out clusterCenters, out var uniqueClusterIndices);
			clusterIndices = new int[data.RowCount];
			RestoreRowIndices(rowIndexMap, uniqueClusterIndices, clusterIndices);
		}

		private static void RestoreRowIndices(Dictionary<EquatableArray<double>, List<int>> rowIndexMap,
			int[] uniqueClusterIndices, int[] clusterIndices) {
			List<int>[] duplicates = rowIndexMap.Values.ToArray();
			for (int newIndex = 0; newIndex < uniqueClusterIndices.Length; newIndex++) {
				List<int> duplicate = duplicates[newIndex];
				foreach (int oldIndex in duplicate) {
					clusterIndices[oldIndex] = uniqueClusterIndices[newIndex];
				}
			}
		}

		private static void ExtractUniqueRows(MatrixIndexer data,
			out Dictionary<EquatableArray<double>, List<int>> rowIndexMap, out double[][] reducedData) {
			List<double[]> uniqueRows = new List<double[]>();
			rowIndexMap = new Dictionary<EquatableArray<double>, List<int>>();
			for (int row = 0; row < data.RowCount; row++) {
				double[] rowArray = data.GetRow(row).ToArray();
				EquatableArray<double> rowEqArray = new EquatableArray<double>(rowArray);
				if (!rowIndexMap.ContainsKey(rowEqArray)) {
					rowIndexMap.Add(rowEqArray, new List<int>());
					uniqueRows.Add(rowArray);
				}
				rowIndexMap[rowEqArray].Add(row);
			}
			reducedData = uniqueRows.ToArray();
		}

		public static void GenerateClustersImpl(double[][] data, int k, int maxIter, int restarts, Action<int> progress,
			out float[,] clusterCenters, out int[] clusterIndices) {
			int npoints = data.Length;
			int nvars = data.First().Length;
			if ((k < 1) || restarts < 1) {
				throw new Exception("Invalid cluster parameters.");
			}
			if (npoints <= k) {
				clusterCenters = new float[npoints, nvars];
				for (int i = 0; i < npoints; i++) {
					double[] rowData = data[i];
					for (int j = 0; j < nvars; j++) {
						clusterCenters[i, j] = (float) rowData[j];
					}
				}
				clusterIndices = ArrayUtils.ConsecutiveInts(npoints);
				return;
			}
			if (nvars < 1) {
				clusterCenters = new float[k, 0];
				clusterIndices = ArrayUtils.ConsecutiveInts(k);
				return;
			}
			//
			// Multiple passes of k-means algorithm
			//
			var bestPass = Enumerable.Range(0, restarts).AsParallel().Select(pass => {
				double[,] ct = SelectInitialCenters(data, npoints, nvars, k);
				int[] localClusterIndices = UpdateCenterPositions(data, k, maxIter, restarts, progress, npoints, pass, nvars, ct);
				double e = CalculateE(data, localClusterIndices, npoints, nvars, ct);
				return new {ct, localClusterIndices, e};
			}).Aggregate((best, current) => best.e > current.e ? best : current);
			clusterCenters = ArrayUtils.ToFloats(bestPass.ct);
			clusterIndices = bestPass.localClusterIndices;
			progress(100);
		}

		private static double CalculateE(double[][] data, int[] clusterIndices, int npoints, int nvars, double[,] ct) {
			double e = 0;
			for (int i = 0; i < npoints; i++) {
				double[] rowData = data[i];
				double v = 0.0;
				double c = 0;
				for (int l = 0; l < nvars; l++) {
					double temp = rowData[l] - ct[clusterIndices[i], l];
					if (!double.IsNaN(temp)) {
						v += temp * temp;
						c++;
					}
				}
				v *= nvars / c;
				e = e + v;
			}
			return e;
		}

		private static int[] UpdateCenterPositions(double[][] data, int k, int maxIter, int restarts, Action<int> progress,
			int npoints, int pass, int nvars, double[,] ct) {
			bool[] cbusy = new bool[k];
			for (int i = 0; i < k; i++) {
				cbusy[i] = false;
			}
			int[] clusterIndices = new int[npoints];
			for (int i = 0; i < npoints; i++) {
				clusterIndices[i] = -1;
			}
			for (int iter = 0; iter < maxIter; iter++) {
				progress(100 * pass / restarts * (1 + iter / maxIter));
				// assign items to clusters
				bool wereChanges = false;
				for (int i = 0; i < npoints; i++) {
					double[] rowData = data[i];
					int cclosest = -1;
					double dclosest = double.MaxValue;
					for (int j = 0; j < k; j++) {
						double v = 0.0;
						double c = 0;
						for (int l = 0; l < nvars; l++) {
							double temp = rowData[l] - ct[j, l];
							if (!double.IsNaN(temp)) {
								v += temp * temp;
								c++;
							}
						}
						v *= nvars / c;
						if (v < dclosest) {
							cclosest = j;
							dclosest = v;
						}
					}
					if (clusterIndices[i] != cclosest) {
						wereChanges = true;
					}
					clusterIndices[i] = cclosest;
				}
				// Update centers
				int[] csizes = new int[k];
				for (int i = 0; i < k; i++) {
					for (int j = 0; j < nvars; j++) {
						ct[i, j] = 0;
					}
				}
				int[,] counts = new int[k, nvars];
				for (int i = 0; i < npoints; i++) {
					double[] rowData = data[i];
					int cind = clusterIndices[i];
					if (cind < 0) {
						continue;
					}
					csizes[cind] = csizes[cind] + 1;
					for (int l = 0; l < nvars; l++) {
						if (!double.IsNaN(rowData[l])) {
							ct[cind, l] = ct[cind, l] + rowData[l];
							counts[cind, l]++;
						}
					}
				}
				for (int j = 0; j < k; j++) {
					for (int l = 0; l < nvars; l++) {
						ct[j, l] = counts[j, l] > 0 ? ct[j, l] / counts[j, l] : double.NaN;
					}
				}
				bool zerosizeclusters = false;
				for (int i = 0; i < k; i++) {
					cbusy[i] = csizes[i] != 0;
					zerosizeclusters = zerosizeclusters || csizes[i] == 0;
				}
				if (zerosizeclusters) {
					//
					// Some clusters have zero size - rare, but possible.
					// We'll choose new centers for such clusters
					// and restart algorithm
					//
					SelectCenter(data, npoints, nvars, ct, cbusy, k);
					continue;
				}
				//
				// if nothing has changed during iteration
				//
				if (!wereChanges) {
					break;
				}
			}
			return clusterIndices;
		}

		private static double[,] SelectInitialCenters(double[][] xy, int npoints, int nvars, int k) {
			bool[] busycenters = new bool[npoints];
			for (int i = 0; i < k; i++) {
				busycenters[i] = false;
			}
			double[,] centers = new double[k, nvars];
			return SelectCenter(xy, npoints, nvars, centers, busycenters, k);
		}

		private static double[,] SelectCenter(double[][] xy, int npoints, int nvars, double[,] centers, bool[] busycenters,
			int k) {
			int[] perm = randy.NextPermutation(npoints);
			for (int cc = 0; cc < k; cc++) {
				double[] row = xy[perm[cc]];
				if (!busycenters[cc]) {
					for (int i = 0; i < nvars; i++) {
						centers[cc, i] = row[i];
					}
				}
			}
			return centers;
		}
	}
}