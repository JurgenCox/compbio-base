using System.Collections.Generic;
using System.Linq;
using BaseLibS.Num;

namespace BaseLibS.Data{
    /// <summary>
    /// Graph structure based on dictionaries of node neighbors.
    /// </summary>
	public class NeighbourList{
		private readonly Dictionary<int, HashSet<int>> neighborList = new Dictionary<int, HashSet<int>>();

		public void Add(int i, int j){
			Add2(i, j);
			Add2(j, i);
		}

		public bool IsEmptyAt(int i) { return !neighborList.ContainsKey(i); }

        /// <summary>
        /// Get connected component and remove it from NeighbourList
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
		public int[] GetClusterAt(int i){
			int[] cluster = GetClusterAtNoRemove(i);
			RemoveCluster(cluster);
			return cluster;
		}

        /// <summary>
        /// Get all connected components
        /// </summary>
        /// <returns></returns>
		public int[][] GetAllClusters(){
			List<int[]> result = new List<int[]>();
			while (neighborList.Count > 0){
				result.Add(GetClusterAt(neighborList.Keys.First()));
			}
			return result.ToArray();
		}

		public void RemoveCluster(int[] cluster){
			foreach (int c in cluster){
				neighborList.Remove(c);
			}
		}

        /// <summary>
        /// Get connected component at node i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
		public int[] GetClusterAtNoRemove(int i){
			HashSet<int> cluster = new HashSet<int>();
			Stack<int> todo = new Stack<int>();
			todo.Push(i);
			while (todo.Count > 0){
				int next = todo.Pop();
				if (!cluster.Contains(next)){
					if (neighborList.ContainsKey(next)){
						cluster.Add(next);
						foreach (int x in neighborList[next]){
							todo.Push(x);
						}
					}
				}
			}
			return ArrayUtils.ToArray(cluster);
		}

		private void Add2(int i, int j){
			if (!neighborList.ContainsKey(i)){
				neighborList.Add(i, new HashSet<int>());
			}
			neighborList[i].Add(j);
		}
	}
}