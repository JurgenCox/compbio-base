using System;
using System.Collections.Generic;
using BaseLibS.Num;
using BaseLibS.Num.Func;

namespace BaseLibS.Mol {
	public class ChemElement {
		public int Z { get; }
		public int[] MainValences { get; }
		public string Name { get; }
		public ChemElementType Type { get; }
		public string Symbol { get; }
		public string CasRegistryId { get; }
		public double AtomicWeight { get; }
		public double MonoIsotopicMass { get; }
		public bool IsIsotopicLabel { get; }
		public int NaturalVersion { get; internal set; }
		public int MaxNumDefault { get; }
		private readonly double[] composition;
		private readonly double[] masses;
		private readonly Dictionary<int, double[][]> store = new Dictionary<int, double[][]>();

		internal ChemElement(int z, string symbol, string name, double[] masses, double[] composition, double atomicWeight,
			ChemElementType type, int[] mainValences = null, string casRegistryId = "", bool isotopicLabel = false,
			int maxNumDefault = 10) {
			Symbol = symbol;
			this.masses = masses;
			Z = z;
			Name = name;
			Type = type;
			CasRegistryId = casRegistryId;
			MonoIsotopicMass = masses[ArrayUtils.MaxInd(composition)];
			this.composition = composition;
			for (int i = 0; i < this.composition.Length; i++) {
				this.composition[i] *= 0.01;
			}
			AtomicWeight = atomicWeight;
			IsIsotopicLabel = isotopicLabel;
			MaxNumDefault = maxNumDefault;
			MainValences = mainValences;
		}

		public bool OddValence => MainValences != null && MainValences[0] % 2 != 0;
		public int Valence => MainValences != null ? Math.Abs(MainValences[0]) : 0;

		public double[][] GetIsotopeDistribution(int n) {
			if (store.ContainsKey(n)) {
				return store[n];
			}
			double[][] dist = GetIsotopeDistribution(n, masses, composition);
			if (n <= 300) {
				store.Add(n, dist);
			}
			return dist;
		}

		public double[][] GetIsotopeDistributionAlteredIsotopeContribution(int n, int index, double delta) {
			double[] c2 = new double[composition.Length];
			for (int i = 0; i < c2.Length; i++) {
				c2[i] = composition[i];
				if (i == index) {
					c2[i] += delta;
				}
			}
			c2[0] -= delta;
			return GetIsotopeDistribution(n, masses, c2);
		}

		public static double[][] GetIsotopeDistribution(int n, double[] masses, double[] composition) {
			int len = masses.Length;
			int[][] partitions = NumUtils.GetPartitions(n, len);
			double[] ms = new double[partitions.Length];
			double[] weights = new double[partitions.Length];
			for (int i = 0; i < partitions.Length; i++) {
				weights[i] = 1;
				int[] partition = partitions[i];
				for (int j = 0; j < len; j++) {
					ms[i] += partition[j] * masses[j];
					for (int k = 0; k < partition[j]; k++) {
						weights[i] *= composition[j];
					}
				}
				weights[i] *= Factorial.Multinomial(n, partition);
			}
			int[] o = ArrayUtils.Order(ms);
			ms = ArrayUtils.SubArray(ms, o);
			weights = ArrayUtils.SubArray(weights, o);
			double[][] x = FilterWeights(ms, weights, 1e-6);
			ms = x[0];
			weights = x[1];
			x = FilterMasses(ms, weights, 0.2);
			ms = x[0];
			weights = x[1];
			return new[] {ms, weights};
		}

		public static double[][] FilterMasses(double[] masses, double[] weights, double massPrec) {
			int pos = 0;
			double[] newMasses = new double[masses.Length];
			double[] newWeights = new double[weights.Length];
			for (int i = 0; i < masses.Length; i++) {
				if (i == masses.Length - 1 || masses[i + 1] - masses[i] >= massPrec) {
					newMasses[pos] = masses[i];
					newWeights[pos] = weights[i];
					pos++;
				} else {
					int start = i;
					while (i < masses.Length - 1 && masses[i + 1] - masses[i] < massPrec) {
						i++;
					}
					double nm = 0;
					double nw = 0;
					for (int j = start; j <= i; j++) {
						nw += weights[j];
						nm += weights[j] * masses[j];
					}
					nm /= nw;
					newMasses[pos] = nm;
					newWeights[pos] = nw;
					pos++;
				}
			}
			Array.Resize(ref newMasses, pos);
			Array.Resize(ref newWeights, pos);
			return new[] {newMasses, newWeights};
		}

		public static double[][] FilterWeights(double[] masses, double[] weights, double weightCut) {
			List<int> v = new List<int>();
			int maxInd = ArrayUtils.MaxInd(weights);
			for (int i = 0; i < weights.Length; i++) {
				if ((i <= maxInd && weights[i] > 0) || weights[i] >= weightCut) {
					v.Add(i);
				}
			}
			int[] valids = v.ToArray();
			return new[] {ArrayUtils.SubArray(masses, valids), ArrayUtils.SubArray(weights, valids)};
		}

		public int GetNominalMass() {
			int ind = ArrayUtils.MaxInd(composition);
			return (int) Math.Round(masses[ind]);
		}

		public bool IsHydrogen() {
			return Z == 1;
		}

		public override string ToString() {
			return Symbol;
		}
	}
}