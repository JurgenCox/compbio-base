using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Api;
using BaseLibS.Num.Matrix;

namespace BaseLibS.Num.Cluster {
	public static class KmedoidClustering {
		/// <summary>
		/// Run K-medoid clustering.
		/// </summary>
		/// <param name="data">data matrix with n rows</param>
		/// <param name="distance"></param>
		/// <param name="k">number of clusters k &lt; n</param>
		/// <returns>Array of length n. <code>assignment[i]</code> returns the index of the cluster medoid in the data matrix.</returns>
		public static int[] GenerateClusters(MatrixIndexer data, IDistance distance, int k) {
			// return GenerateClusters(data, new GenericDistanceMatrix(data, distance), k); // TODO allow calling GenericDistanceMatrix without circular dependancy
			throw new NotImplementedException("Use GenericDistanceMatrix to convert IDistance to IDistanceMatrix");
		}

		/// <summary>
		/// Run K-medoid clustering.
		/// </summary>
		/// <param name="data">data matrix with n rows</param>
		/// <param name="distance"></param>
		/// <param name="k">number of clusters k &lt; n</param>
		/// <returns>Array of length n. <code>assignment[i]</code> returns the index of the cluster medoid in the data matrix.</returns>
		public static int[] GenerateClusters(MatrixIndexer data, IDistanceMatrix distance, int k) {
			int n = data.RowCount;
			int[] medoids = SelectInitialMedoids(distance, k, n);
			int[] assignments = AssignClusters(distance, medoids, n);
			double cost = CalculateCost(distance, assignments);
			while (true) {
				medoids = SelectMedoids(distance, assignments);
				assignments = AssignClusters(distance, medoids, n);
				double newCost = CalculateCost(distance, assignments);
				if (Math.Abs(newCost - cost) < 1E-10) {
					break;
				}
				cost = newCost;
			}
			return assignments;
		}

		internal static int[] SelectMedoids(IDistanceMatrix distance, int[] assignments) {
			List<int> medoids = new List<int>();
			int[][] clusters = assignments.Select((x, i) => new {Value = x, Index = i}).GroupBy(obj => obj.Value)
				.Select(grp => grp.Select(obj => obj.Index).ToArray()).ToArray();
			foreach (int[] cluster in clusters) {
				double withinClusterDistance = double.PositiveInfinity;
				int medoid = 0;
				foreach (int newMedoid in cluster) {
					double newWithinClusterDistance = 0.0;
					foreach (int point in cluster) {
						newWithinClusterDistance += distance[newMedoid, point];
					}
					if (newWithinClusterDistance < withinClusterDistance) {
						withinClusterDistance = newWithinClusterDistance;
						medoid = newMedoid;
					}
				}
				medoids.Add(medoid);
			}
			return medoids.ToArray();
		}

		internal static double CalculateCost(IDistanceMatrix distance, int[] assignments) {
			double cost = 0.0;
			for (int i = 0; i < assignments.Length; i++) {
				cost += distance[i, assignments[i]];
			}
			return cost;
		}

		internal static int[] AssignClusters(IDistanceMatrix distance, int[] medoids, int n) {
			int[] assignments = new int[n];
			for (int point = 0; point < n; point++) {
				double d = double.PositiveInfinity;
				int assigned = 0;
				foreach (int medoid in medoids) {
					double dNew = distance[point, medoid];
					if (dNew < d) {
						d = dNew;
						assigned = medoid;
					}
				}
				assignments[point] = assigned;
			}
			return assignments;
		}

		internal static int[] SelectInitialMedoids(IDistanceMatrix distance, int k, int n) {
			double[] sumD = new double[n];
			for (int i = 0; i < n; i++) {
				double sumDil = 0.0;
				for (int l = 0; l < n; l++) {
					sumDil += distance[i, l];
				}
				sumD[i] = sumDil;
			}
			double[] v = new double[n];
			for (int j = 0; j < n; j++) {
				double vj = 0.0;
				for (int i = 0; i < n; i++) {
					vj += distance[i, j] / sumD[i];
				}
				v[j] = vj;
			}
			int[] idx = new int[n];
			for (int i = 0; i < n; i++) {
				idx[i] = i;
			}
			Array.Sort(v, idx);
			int[] medoids = new int[k];
			Array.Copy(idx, medoids, k);
			return medoids;
		}
	}
}