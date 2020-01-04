using System.Collections.Generic;
using System.Linq;

namespace BaseLibS.Data{
	public class NeighbourListBipartite{
		private readonly Dictionary<int, HashSet<int>> neighborList1 = new Dictionary<int, HashSet<int>>();
		private readonly Dictionary<int, HashSet<int>> neighborList2 = new Dictionary<int, HashSet<int>>();

		public void Add(int i, int j){
			if (!neighborList1.ContainsKey(i)){
				neighborList1.Add(i, new HashSet<int>());
			}
			neighborList1[i].Add(j);
			if (!neighborList2.ContainsKey(j)){
				neighborList2.Add(j, new HashSet<int>());
			}
			neighborList2[j].Add(i);
		}

		public void GetClusterAt(int i, out int[] cluster1, out int[] cluster2){
			GetClusterAtNoRemove(i, out cluster1, out cluster2);
			RemoveCluster(cluster1, cluster2);
		}

		public void GetAllClusters(out int[][] clusters1, out int[][] clusters2){
			List<int[]> result1 = new List<int[]>();
			List<int[]> result2 = new List<int[]>();
			while (neighborList1.Count > 0){
				int[] cluster1;
				int[] cluster2;
				GetClusterAt(neighborList1.Keys.First(), out cluster1, out cluster2);
				if (cluster2.Length > 0){
					result1.Add(cluster1);
					result2.Add(cluster2);
				}
			}
			clusters1 = result1.ToArray();
			clusters2 = result2.ToArray();
		}

		public void RemoveCluster(int[] cluster1, int[] cluster2){
			foreach (int c in cluster1.Where(c => neighborList1.ContainsKey(c))){
				neighborList1.Remove(c);
			}
			foreach (int c in cluster2.Where(c => neighborList2.ContainsKey(c))){
				neighborList2.Remove(c);
			}
		}

		public void GetClusterAtNoRemove(int i, out int[] c1, out int[] c2){
			HashSet<int> cluster1 = new HashSet<int>();
			HashSet<int> cluster2 = new HashSet<int>();
			Stack<int> todo1 = new Stack<int>();
			Stack<int> todo2 = new Stack<int>();
			todo1.Push(i);
			while (todo1.Count > 0 || todo2.Count > 0){
				if (todo1.Count > 0){
					int next = todo1.Pop();
					if (!cluster1.Contains(next)){
						if (neighborList1.ContainsKey(next)){
							cluster1.Add(next);
							foreach (int x in neighborList1[next]){
								todo2.Push(x);
							}
						}
					}
				} else{
					int next = todo2.Pop();
					if (!cluster2.Contains(next)){
						if (neighborList2.ContainsKey(next)){
							cluster2.Add(next);
							foreach (int x in neighborList2[next]){
								todo1.Push(x);
							}
						}
					}
				}
			}
			c1 = cluster1.ToArray();
			c2 = cluster2.ToArray();
		}
	}
}