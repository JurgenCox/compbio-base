using System.Collections.Generic;

namespace BaseLibS.Data{
	public class NeighbourListDirected{
		private readonly Dictionary<int, HashSet<int>> neighborList = new Dictionary<int, HashSet<int>>();
		private readonly Dictionary<int, HashSet<int>> neighborListRev = new Dictionary<int, HashSet<int>>();

		public void Add(int i, int j){
			if (!neighborList.ContainsKey(i)){
				neighborList.Add(i, new HashSet<int>());
			}
			neighborList[i].Add(j);
			if (!neighborListRev.ContainsKey(j)){
				neighborListRev.Add(j, new HashSet<int>());
			}
			neighborListRev[j].Add(i);
		}
	}
}