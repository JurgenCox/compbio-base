﻿using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Graph;

namespace BaseLibS.Num.Cluster{
	/// <summary>
	/// Contains all information on a row/column clustering.
	///
	/// The dendrogram is represented by the list of <see cref="nodes"/>.
	/// A clustering of (n+1) items is represented by n <see cref="nodes"/>.
	/// </summary>
	[Serializable]
	public class HierarchicalClusterData{
		public readonly HierarchicalClusterNode[] nodes;
		private int[] itemOrder;
		private int[] itemOrderInv;

		/// <summary>
		/// Number of data points under each <see cref="nodes"/>
		/// </summary>
		private int[] sizes;

		private int[] start;
		private int[] end;

		public Dictionary<string, Color2> cluster2Color;

		/// <summary>
		/// Clusters are a list of nodes according to <see cref="HierarchicalClusterNode"/> format.
		/// Multiple nodes per cluster enable the representation of non-contiguous clusters in the dendrogram.
		///
		/// Every node can be member of at most one cluster.
		/// </summary>
		private int[][] clusters;

		public bool drawHang = true;
		public int colorBarSize = 15;
		public int treeLineWidth = 2;

		public HierarchicalClusterData(HierarchicalClusterNode[] nodes, int[][] clusters, Dictionary<string, Color2> colorMap){
			this.nodes = nodes;
			HierarchicalClustering.CalcTree(nodes, out sizes, out start, out end, out itemOrder, out itemOrderInv);
			Clusters = clusters;
			cluster2Color = colorMap;
		}

		public HierarchicalClusterData(HierarchicalClusterNode[] nodes) : this(nodes, new int[0][],
			new Dictionary<string, Color2>()){ }

		private int[][] Clusters{
			get => clusters;
			set => clusters = value;
		}

		/// <summary>
		/// Unique id for every cluster.
		/// </summary>
		public string[] ClusterIds{
			get{
				string[] result = new string[Clusters.Length];
				for (int i = 0; i < result.Length; i++){
					result[i] = string.Join(";", Clusters[i]);
				}
				return result;
			}
		}

		public int[] ItemOrder => itemOrder;
		public int[] ItemOrderInv => itemOrderInv;
		public int[] Sizes => sizes;
		public int[] Start => start;
		public int[] End => end;

		/// <summary>
		/// Flip the dendrogram around a node.
		/// </summary>
		/// <param name="i"></param>
		public void Flip(int i){
			nodes[i].Flip();
			HierarchicalClustering.CalcTree(nodes, out sizes, out start, out end, out itemOrder, out itemOrderInv);
		}

		/// <summary>
		/// Returns the color of each leaf.
		/// </summary>
		public Color2[] LeafColorsByClusters(Color2 unselectedColor){
			Color2[] result = Enumerable.Repeat(unselectedColor, nodes.Length + 1).ToArray();
			foreach ((int[] t, string id) in Clusters.Zip(ClusterIds, (t, id) => (t, id))){
				if (!cluster2Color.ContainsKey(id)){
					cluster2Color.Add(id, GraphUtil.GetPredefinedColor(t[0]));
				}
				foreach (int cl in t){
					if (cl < 0){
						for (int j = Start[-1 - cl]; j < End[-1 - cl]; j++){
							result[ItemOrderInv[j]] = cluster2Color[id];
						}
					} else{
						result[cl] = cluster2Color[id];
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Cut the dendrogram at the defined distance and return the highest nodes in the <see cref="HierarchicalClusterNode"/>
		/// index format.
		/// </summary>
		public void DefineClusters(double dist){
			void Remove(ICollection<int> nodes1, int i){
				nodes1.Remove(i);
				if (i < 0){
					nodes1.Remove(i);
					Remove(nodes1, nodes[-1 - i].left);
					Remove(nodes1, nodes[-1 - i].right);
				}
			}

			HashSet<int> clusterNodes = new HashSet<int>();
			for (int i = 0; i <= nodes.Length; i++){
				clusterNodes.Add(i);
			}
			for (int i = 0; i < nodes.Length; i++){
				if (nodes[i].distance > dist){
					break;
				}
				clusterNodes.Add(-1 - i);
				Remove(clusterNodes, nodes[i].left);
				Remove(clusterNodes, nodes[i].right);
			}
			Clusters = clusterNodes.Select(i => new[]{i}).ToArray();
			foreach (string clusterId in ClusterIds){
				if (!cluster2Color.ContainsKey(clusterId)){
					cluster2Color.Add(clusterId, GraphUtil.GetPredefinedColor(cluster2Color.Count));
				}
			}
		}

		/// <summary>
		/// Creates a color map based on the cluster definitions, maps indices of <see cref="nodes"/> to colors.
		/// </summary>
		public IDictionary<int, Color2> NodeIndexToColor(){
			Dictionary<int, Color2> result = new Dictionary<int, Color2>();

			void AddChildren(int c, string id, int value){
				if (!cluster2Color.ContainsKey(id)){
					cluster2Color.Add(id, GraphUtil.GetPredefinedColor(value));
				}
				result.Add(-1 - c, cluster2Color[id]);
				HierarchicalClusterNode node = nodes[-1 - c];
				if (node.left < 0){
					AddChildren(node.left, id, value);
				}
				if (node.right < 0){
					AddChildren(node.right, id, value);
				}
			}

			foreach ((int[] t, string id) in Clusters.Zip(ClusterIds, (t, id) => (t, id))){
				foreach (int cl in t){
					if (cl < 0){
						AddChildren(cl, id, t[0]);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Remove the parents of <param name="clusterId">cl</param> from <see cref="Clusters"/>.
		/// </summary>
		public void RemoveParents(string clusterId){
			int[] nodes1 = NodesFromClusterId(clusterId);
			IEnumerable<int> parents = GetParents(nodes1);
			Clusters = Clusters.Select(cluster => cluster.Except(parents).ToArray())
				.Where(cluster => cluster.Length > 0).ToArray();
		}

		/// <summary>
		/// Get all parent nodes indices of <param name="nodes1">nodes</param>
		/// </summary>
		private IEnumerable<int> GetParents(params int[] nodes1){
			List<int> parents = new List<int>();
			foreach (int cl in nodes1){
				for (int i = 0; i < this.nodes.Length; i++){
					if (IsParent(cl, -1 - i)){
						parents.Add(-1 - i);
					}
				}
			}
			return parents;
		}

		/// <summary>
		/// Check if <param name="child">child</param> is a parent of <param name="parent">Nodes[-1 - parent]</param>.
		/// </summary>
		private bool IsParent(int child, int parent){
			HierarchicalClusterNode node = nodes[-1 - parent];
			if (node.left == child){
				return true;
			}
			if (node.right == child){
				return true;
			}
			if (node.left >= 0 && node.right >= 0){
				return false;
			}
			if (node.left < 0 && node.right < 0){
				return IsParent(child, node.left) || IsParent(child, node.right);
			}
			return IsParent(child, node.left < 0 ? node.left : node.right);
		}

		/// <summary>
		/// Remove the children of <param name="clusterId">cl</param> from <see cref="Clusters"/>.
		/// </summary>
		public void RemoveChildren(string clusterId){
			int[] nodes1 = NodesFromClusterId(clusterId);
			IEnumerable<int> children = GetChildren(nodes1);
			Clusters = Clusters.Select(cluster => cluster.Except(children).ToArray())
				.Where(cluster => cluster.Length > 0).ToArray();
		}

		/// <summary>
		/// Get children nodes of cluster <param name="cl">cl</param>.
		/// </summary>
		private int[] GetChildren(params int[] nodes1){
			List<int> children = new List<int>();
			foreach (int cl in nodes1){
				if (cl >= 0){
					continue;
				}
				HierarchicalClusterNode node = this.nodes[-1 - cl];
				children.Add(node.left);
				children.Add(node.right);
				children.AddRange(GetChildren(node.left).Concat(GetChildren(node.right)));
			}
			return children.ToArray();
		}

		/// <summary>
		/// Remove the cluster <param name="clusterId">cl</param> from <see cref="Clusters"/>.
		/// </summary>
		public void RemoveCluster(string clusterId){
			int[] nodes1 = NodesFromClusterId(clusterId);
			Clusters = Clusters.Select(cluster => cluster.Except(nodes1).ToArray()).Where(cluster => cluster.Length > 0)
				.ToArray();
		}

		/// <summary>
		/// Create a cluster is from the actual nodes. See <see cref="NodesFromClusterId"/> for inverse.
		/// </summary>
		public static string CreateClusterId(params int[] nodes){
			return string.Join(";", nodes);
		}

		/// <summary>
		/// Extract nodes from cluster id. See <see cref="CreateClusterId"/> for inverse.
		/// </summary>
		public static int[] NodesFromClusterId(string clusterId){
			return clusterId.Split(';').Select(s => Convert.ToInt32(s)).ToArray();
		}

		/// <summary>
		/// Add the cluster <param name="cl">cl</param> to <see cref="Clusters"/> and return its index.
		/// </summary>
		public int AddCluster(string clusterId){
			int[] nodes1 = NodesFromClusterId(clusterId);
			(bool contained, int index) = Clusters
				.Select((cluster, i) => (contained: nodes1.All(cluster.Contains), index: i))
				.SingleOrDefault(cluster => cluster.contained);
			if (!contained){
				index = Clusters.Length;
				Clusters = Clusters.Concat(new[]{nodes1}).ToArray();
			}
			return index;
		}

		// TODO Functions very similar/identical to GetLeaves?!
		/// <summary>
		/// Original data indices for the cluster.
		/// </summary>
		public int[] DataIndicesFromCluster(string clusterId){
			List<int> result = new List<int>();
			foreach (int cl in clusterId.Split(';').Select(s => Convert.ToInt32(s))){
				if (cl < 0){
					for (int j = Start[-1 - cl]; j < End[-1 - cl]; j++){
						result.Add(ItemOrderInv[j]);
					}
				} else{
					result.Add(cl);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Returns a boolean for each original data point which indicates whether the
		/// data point is contained in any of the <param name="clusterIds">clusterIds</param>
		/// </summary>
		public bool[] AreDataInCluster(string[] clusterIds){
			bool[] result = new bool[nodes.Length + 1];
			foreach (int t in clusterIds.SelectMany(id => id.Split(';').Select(s => Convert.ToInt32(s)))){
				if (t < 0){
					for (int j = Start[-1 - t]; j < End[-1 - t]; j++){
						result[j] = true;
					}
				} else{
					result[ItemOrder[t]] = true;
				}
			}
			return result;
		}

		/// <summary>
		/// Cluster size for each <see cref="ClusterIds"/>.
		/// </summary>
		public IEnumerable<int> ClusterSizes(){
			IEnumerable<int[]> clusterNodes = ClusterIds.Select(NodesFromClusterId);
			return clusterNodes.Select(nodes1 => nodes1.Sum(node => node < 0 ? Sizes[-1 - node] : 1));
		}

		/// <summary>
		/// Returns cluster information describing the clusters.
		/// Mainly used for showing clusters in the GUI cluster table.
		/// </summary>
		/// <returns></returns>
		public (string, int, string)[] GetInformation(){
			(string id, int size, string colorName)[] clusters1 = ClusterIds.Zip(ClusterSizes(), (id, size) => {
				string colorName = "";
				if (cluster2Color.TryGetValue(id, out Color2 color)){
					colorName = color.Name;
				}
				return (id, size, colorName);
			}).ToArray();
			return clusters1;
		}

		public bool HasSubClusterGeThan(int size){
			for (int i = sizes.Length - 2; i >= 0; i--){
				if (sizes[i] >= size){
					return true;
				}
			}
			return false;
		}
	}
}