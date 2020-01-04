using System;
using System.IO;

namespace BaseLibS.Num.Cluster {
	[Serializable]
	public class HierarchicalClusterNode {
		/// <summary>
		/// The distance between the two children.
		/// </summary>
		public double distance;

		/// <summary>
		/// The id of the left child. If it is >=0, it means the left child is an original 
		/// data point and the value is its index. When negative the left child is the <c>-left-1</c>
		/// entry in the cluster node list.
		/// </summary>
		public int left;

		/// <summary>
		/// The id of the right child. If it is >=0, it means the right child is an original 
		/// data point and the value is its index. When negative the right child is the <c>-right-1</c>
		/// entry in the cluster node list.
		/// </summary>
		public int right;

		public HierarchicalClusterNode(BinaryReader reader) {
			distance = reader.ReadDouble();
			left = reader.ReadInt32();
			right = reader.ReadInt32();
		}

		public HierarchicalClusterNode() { }

		public void Write(BinaryWriter writer) {
			writer.Write(distance);
			writer.Write(left);
			writer.Write(right);
		}

		public void Flip() {
			int tmp = left;
			left = right;
			right = tmp;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType()) return false;
			HierarchicalClusterNode other = (HierarchicalClusterNode) obj;
			bool sameOrFlipped = ((left == other.left) && (right == other.right)) ||
			                     ((right == other.left) && (left == other.right));
			return sameOrFlipped && (Math.Abs(distance - other.distance) < 0.0001);
		}

		/// <summary>
		/// Utility format for reading clustering results from R.
		/// </summary>
		/// <param name="left">first column of <code>hclust$merge</code></param>
		/// <param name="right">second column of <code>hclust$merge</code></param>
		/// <param name="distance"><code>hclust$height</code></param>
		/// <returns></returns>
		public static HierarchicalClusterNode[] FromRFormat(int[] left, int[] right, double[] distance) {
			int n = distance.Length;
			HierarchicalClusterNode[] nodes = new HierarchicalClusterNode[n];
			for (int i = 0; i < n; i++) {
				nodes[i] = new HierarchicalClusterNode {
					distance = distance[i],
					left = left[i] < 0 ? -left[i] - 1 : -left[i],
					right = right[i] < 0 ? -right[i] - 1 : -right[i]
				};
			}
			return nodes;
		}
	}
}