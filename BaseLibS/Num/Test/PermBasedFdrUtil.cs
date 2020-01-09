using System.Collections.Generic;

namespace BaseLibS.Num.Test {
	public static class PermBasedFdrUtil {
		/// <summary>
		/// Creates permutations of group memberships of data points while preserving subgroup structures. 
		/// </summary>
		public static void BalancedPermutationsSubgroups(int[][][] inds, out int[][] indsOut, Random2 r2) {
			//Permute indices within groups
			int[][][] inds1 = new int[inds.Length][][];
			for (int i = 0; i < inds.Length; i++) {
				inds1[i] = ArrayUtils.SubArray(inds[i], r2.NextPermutation(inds[i].Length));
			}
			//Permute order of groups
			inds1 = ArrayUtils.SubArray(inds1, r2.NextPermutation(inds1.Length));
			List<int[]>[] newInds = new List<int[]>[inds1.Length];
			for (int i = 0; i < inds1.Length; i++) {
				newInds[i] = new List<int[]>();
			}
			int index = 0;
			for (int i = inds1.Length - 1; i >= 0; i--) {
				int[][] ig = inds1[i];
				foreach (int[] ind in ig) {
					//It has to be inds[index] here since otherwise for sided two-sample tests the group size will be swapped
					while (newInds[index].Count >= inds[index].Length) {
						index = (index + 1) % inds1.Length;
					}
					newInds[index].Add(ind);
					index = (index + 1) % inds1.Length;
				}
			}
			indsOut = new int[inds1.Length][];
			for (int i = 0; i < inds1.Length; i++) {
				indsOut[i] = ArrayUtils.Concat(newInds[i]);
			}
		}

		/// <summary>
		/// Creates permutations of group memberships of data points. 
		/// </summary>
		/// <param name="inds">Original group memberships. Length = number of groups. Each sub-array contains indices 
		/// of original group members.</param>
		/// <param name="indsOut">Scrambled groups</param>
		/// <param name="r2">Random number generator used for creating the permutations</param>
		public static void BalancedPermutations(int[][] inds, out int[][] indsOut, Random2 r2) {
			//Permute indices within groups
			int[][] inds1 = new int[inds.Length][];
			for (int i = 0; i < inds.Length; i++) {
				inds1[i] = ArrayUtils.SubArray(inds[i], r2.NextPermutation(inds[i].Length));
			}
			//Permute order of groups
			inds1 = ArrayUtils.SubArray(inds1, r2.NextPermutation(inds1.Length));
			List<int>[] newInds = new List<int>[inds1.Length];
			for (int i = 0; i < inds1.Length; i++) {
				newInds[i] = new List<int>();
			}
			int index = 0;
			foreach (int[] ig in inds1) {
				foreach (int ind in ig) {
					//It has to be inds[index] here since otherwise for sided two-sample tests the group size will be swapped
					while (newInds[index].Count >= inds[index].Length) {
						index = (index + 1) % inds1.Length;
					}
					newInds[index].Add(ind);
					index = (index + 1) % inds1.Length;
				}
			}
			indsOut = new int[inds1.Length][];
			for (int i = 0; i < inds1.Length; i++) {
				indsOut[i] = newInds[i].ToArray();
			}
		}
	}
}