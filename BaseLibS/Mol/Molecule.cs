using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Mol {
	public class Molecule {
		public static readonly Molecule empty = new Molecule();
		public static readonly decimal massProtonM = 1.0072764666m;
		public static readonly double massProton = 1.0072764666;
		public static readonly double massElectron = 5.4857990943e-4;
		public static readonly double massC = CalcMonoMass("C");
		public static readonly double massH = CalcMonoMass("H");
		public static readonly double massD = CalcMonoMass("Hx");
		public static readonly double massN = CalcMonoMass("N");
		public static readonly double massO = CalcMonoMass("O");
		public static readonly double massP = CalcMonoMass("P");
		public static readonly double massS = CalcMonoMass("S");
		public static readonly double weightC = CalcWeight("C");
		public static readonly double weightH = CalcWeight("H");
		public static readonly double weightN = CalcWeight("N");
		public static readonly double weightO = CalcWeight("O");
		public static readonly double weightP = CalcWeight("P");
		public static readonly double weightS = CalcWeight("S");
		public static readonly double massWater = CalcMonoMass("H2O");
		public static readonly double massAmmonia = CalcMonoMass("NH3");
		public static readonly double s34S32Diff = CalcMonoMass("Sx") - massS;
		public static readonly double c13C12Diff = CalcMonoMass("Cx") - massC;
		public static readonly double sulphurShift = 2 * c13C12Diff - s34S32Diff;
		public int[] AtomType { get; set; }
		public int[] AtomCount { get; set; }
		public double MonoIsotopicMass { get; set; }
		public double MolecularWeight { get; set; }
		public string Name { get; set; }
		private double mostLikelyMass = double.NaN;

		public Molecule(string empiricalFormula) {
			if (empiricalFormula.Contains(".")) {
				string[] q = empiricalFormula.Split('.');
				Molecule[] m = new Molecule[q.Length];
				int[] w = new int[q.Length];
				for (int i = 0; i < q.Length; i++) {
					m[i] = new Molecule(q[i]);
					w[i] = 1;
				}
				Molecule x = Sum(m, w);
				AtomType = x.AtomType;
				AtomCount = x.AtomCount;
				CalcMasses();
			} else {
				int[][] tc = ProcessEmpiricalFormula(empiricalFormula);
				AtomType = tc[0];
				AtomCount = tc[1];
				CalcMasses();
			}
		}

		public Molecule(int[] atomType, int[] atomCount) {
			AtomType = atomType;
			AtomCount = atomCount;
			CalcMasses();
		}

		public Molecule() : this(new int[0], new int[0]) { }

		public Molecule(BinaryReader reader) {
			AtomCount = FileUtils.ReadInt32Array(reader);
			AtomType = FileUtils.ReadInt32Array(reader);
			MolecularWeight = reader.ReadDouble();
			MonoIsotopicMass = reader.ReadDouble();
			mostLikelyMass = reader.ReadDouble();
		}

		public void Write(BinaryWriter writer) {
			FileUtils.Write(AtomCount, writer);
			FileUtils.Write(AtomType, writer);
			writer.Write(MolecularWeight);
			writer.Write(MonoIsotopicMass);
			writer.Write(mostLikelyMass);
		}

		public string GetEmpiricalFormula() {
			return GetEmpiricalFormula(true);
		}

		public int GetOCount() {
			int ind = ChemElements.IndexO;
			int x = Array.BinarySearch(AtomType, ind);
			return x < 0 ? 0 : AtomCount[x];
		}

		public int GetSCount() {
			int ind = ChemElements.IndexS;
			int x = Array.BinarySearch(AtomType, ind);
			return x < 0 ? 0 : AtomCount[x];
		}

		public int GetClCount() {
			int ind = ChemElements.IndexCl;
			int x = Array.BinarySearch(AtomType, ind);
			return x < 0 ? 0 : AtomCount[x];
		}

		public int GetHCount() {
			int ind = ChemElements.IndexH;
			int x = Array.BinarySearch(AtomType, ind);
			return x < 0 ? 0 : AtomCount[x];
		}

		public int GetCCount() {
			int ind = ChemElements.IndexC;
			int x = Array.BinarySearch(AtomType, ind);
			return x < 0 ? 0 : AtomCount[x];
		}

		public string GetEmpiricalFormula(bool oneForSingleAtoms) {
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < AtomType.Length; i++) {
				int count = AtomCount[i];
				if (count != 0) {
					string ac = (count != 1 || oneForSingleAtoms) ? "" + count : "";
					result.Append(ChemElements.Elements[AtomType[i]].Symbol + ac);
				}
			}
			return result.ToString();
		}

		public int NominalMass {
			get {
				int nm = 0;
				for (int i = 0; i < AtomType.Length; i++) {
					int count = AtomCount[i];
					int type = AtomType[i];
					int aw = ChemElements.Elements[type].GetNominalMass();
					nm += count * aw;
				}
				return nm;
			}
		}

		public double GetMostLikelyMass(double massPrecision) {
			if (double.IsNaN(mostLikelyMass)) {
				if (AtomType.Length == 0) {
					mostLikelyMass = 0;
				} else {
					double[][] distrib = GetIsotopeDistribution(massPrecision);
					double[] masses = distrib[0];
					double[] weights = distrib[1];
					double max = 0;
					int maxind = -1;
					for (int i = 0; i < weights.Length; i++) {
						if (weights[i] > max) {
							max = weights[i];
							maxind = i;
						}
					}
					mostLikelyMass = masses[maxind];
				}
			}
			return mostLikelyMass;
		}

		private void CalcMasses() {
			MonoIsotopicMass = 0;
			MolecularWeight = 0;
			for (int i = 0; i < AtomType.Length; i++) {
				int count = AtomCount[i];
				int type = AtomType[i];
				double mim = ChemElements.Elements[type].MonoIsotopicMass;
				double aw = ChemElements.Elements[type].AtomicWeight;
				MonoIsotopicMass += count * mim;
				MolecularWeight += count * aw;
			}
		}

		private static int[][] ProcessEmpiricalFormula(string formula) {
			formula = StringUtils.RemoveWhitespace(formula);
			return formula.Length == 0 ? new[] {new int[0], new int[0]} : ProcessEmpiricalFormulaImpl(formula);
		}

		private static int[][] ProcessEmpiricalFormulaImpl(string formula) {
			int factor = 1;
			if (formula[0] >= '0' && formula[0] <= '9') {
				int index = 0;
				while (formula[index] >= '0' && formula[index] <= '9') {
					index++;
				}
				factor = Parser.Int(formula.Substring(0, index));
				formula = formula.Substring(index);
			}
			int[] counts = new int[ChemElements.Elements.Length];
			while (formula.Length > 0) {
				formula = ProcessFirst(formula, counts);
			}
			List<int> atomTypes = new List<int>();
			List<int> atomCounts = new List<int>();
			for (int i = 0; i < counts.Length; i++) {
				if (counts[i] != 0) {
					atomTypes.Add(i);
					atomCounts.Add(factor * counts[i]);
				}
			}
			return new[] {atomTypes.ToArray(), atomCounts.ToArray()};
		}

		private static string ProcessFirst(string formula, IList<int> counts) {
			for (int i = ChemElements.Elements.Length - 1; i >= 0; i--) {
				string symbol = ChemElements.Elements[i].Symbol;
				if (formula.StartsWith(symbol)) {
					return Calc(formula, counts, symbol, i);
				}
			}
			throw new Exception("Cannot process " + formula);
		}

		private static string Calc(string formula, IList<int> counts, string symbol, int i) {
			formula = formula.Substring(symbol.Length);
			int index = 0;
			while (index < formula.Length && ((formula[index] >= '0' && formula[index] <= '9') || formula[index] == '-')) {
				index++;
			}
			int amount;
			if (index == 0) {
				amount = 1;
			} else {
				amount = Parser.Int(formula.Substring(0, index));
				formula = formula.Substring(index);
			}
			counts[i] += amount;
			return formula;
		}

		public bool IsIsotopicLabel {
			get {
				foreach (int t in AtomType) {
					if (ChemElements.Elements[t].IsIsotopicLabel) {
						return true;
					}
				}
				return false;
			}
		}

		public Molecule GetUnlabeledVersion() {
			Dictionary<int, int> w = new Dictionary<int, int>();
			for (int i = 0; i < AtomType.Length; i++) {
				w.Add(AtomType[i], AtomCount[i]);
			}
			foreach (int t in AtomType) {
				ChemElement el = ChemElements.Elements[t];
				if (el.IsIsotopicLabel) {
					int c = w[t];
					w.Remove(t);
					int n = el.NaturalVersion;
					if (!w.ContainsKey(n)) {
						w.Add(n, 0);
					}
					w[n] += c;
				}
			}
			int[] newTypes = w.Keys.ToArray();
			Array.Sort(newTypes);
			int[] newCounts = new int[newTypes.Length];
			for (int i = 0; i < newCounts.Length; i++) {
				newCounts[i] = w[newTypes[i]];
			}
			return new Molecule(newTypes, newCounts);
		}

		public bool IsEmpty {
			get {
				if (AtomType.Length == 0) {
					return true;
				}
				foreach (int i in AtomCount) {
					if (i > 0) {
						return false;
					}
				}
				return true;
			}
		}

		public double[][] GetIsotopeDistribution(double massPrecision) {
			if (AtomType.Length == 0) {
				return new[] {new double[] {0}, new double[] {1}};
			}
			ChemElement element = ChemElements.Elements[AtomType[0]];
			double[][] distrib = element.GetIsotopeDistribution(AtomCount[0]);
			for (int i = 1; i < AtomType.Length; i++) {
				element = ChemElements.Elements[AtomType[i]];
				distrib = Convolute(distrib, element.GetIsotopeDistribution(AtomCount[i]), massPrecision, 1e-6);
			}
			return distrib;
		}

		public double[][] GetIsotopeDistributionAlteredDeuterium(double deltaDeuterium, double massPrecision) {
			if (AtomType.Length == 0) {
				return new[] {new double[] {0}, new double[] {1}};
			}
			ChemElement element = ChemElements.Elements[AtomType[0]];
			double[][] distrib = element.IsHydrogen()
				? element.GetIsotopeDistributionAlteredIsotopeContribution(AtomCount[0], 1, deltaDeuterium)
				: element.GetIsotopeDistribution(AtomCount[0]);
			for (int i = 1; i < AtomType.Length; i++) {
				element = ChemElements.Elements[AtomType[i]];
				distrib = Convolute(distrib, element.GetIsotopeDistribution(AtomCount[i]), massPrecision, 1e-6);
			}
			return distrib;
		}

		public double[][] GetIsotopeSpectrum(double resolution, int charge) {
			double m = MonoIsotopicMass;
			double sigma = m / Math.Abs(charge) / resolution * 0.5 / Math.Sqrt(2 * Math.Log(2));
			double[][] x = GetIsotopeSpectrum(sigma, 12, 0.0001);
			double dm = charge > 0 ? -massElectron : massElectron;
			for (int i = 0; i < x[0].Length; i++) {
				x[0][i] += dm;
				x[0][i] /= Math.Abs(charge);
			}
			double max = ArrayUtils.Max(x[1]) * 0.01;
			for (int i = 0; i < x[1].Length; i++) {
				x[1][i] /= max;
			}
			return x;
		}

		private double[][] GetIsotopeSpectrum(double sigma, int pointsPerSigma, double massPrecision) {
			double spacing = sigma / pointsPerSigma;
			double[][] distrib = GetIsotopeDistribution(massPrecision);
			double[] masses = distrib[0];
			double[] weights = distrib[1];
			double start = masses[0] - 5 * sigma;
			double end = masses[masses.Length - 1] + 5 * sigma;
			double len = end - start;
			int n = (int) Math.Round(len / spacing);
			double[] newMasses = new double[n];
			double[] newWeights = new double[n];
			for (int i = 0; i < n; i++) {
				double mass = start + i * spacing;
				newMasses[i] = mass;
				for (int j = 0; j < masses.Length; j++) {
					newWeights[i] += weights[j] * Math.Exp(-(mass - masses[j]) * (mass - masses[j]) / 2 / sigma / sigma);
				}
			}
			return new[] {newMasses, newWeights};
		}

		public static double[][] Convolute(double[][] distrib1, double[][] distrib2, double massPrecision,
			double weightCutoff) {
			double[] masses1 = distrib1[0];
			double[] masses2 = distrib2[0];
			double[] weights1 = distrib1[1];
			double[] weights2 = distrib2[1];
			double[] masses = new double[masses1.Length * masses2.Length];
			double[] weights = new double[masses1.Length * masses2.Length];
			int count = 0;
			for (int i = 0; i < masses1.Length; i++) {
				for (int j = 0; j < masses2.Length; j++) {
					masses[count] = masses1[i] + masses2[j];
					weights[count] = weights1[i] * weights2[j];
					count++;
				}
			}
			int[] o = ArrayUtils.Order(masses);
			masses = ArrayUtils.SubArray(masses, o);
			weights = ArrayUtils.SubArray(weights, o);
			double[][] x = ChemElement.FilterMasses(masses, weights, massPrecision);
			masses = x[0];
			weights = x[1];
			if (!double.IsNaN(weightCutoff)) {
				x = ChemElement.FilterWeights(masses, weights, weightCutoff);
				masses = x[0];
				weights = x[1];
			}
			return new[] {masses, weights};
		}

		public static void ConvoluteWithoutErrors(double[] masses1, double[] weights1, double[] masses2, double[] weights2,
			double massCutoff, double weightCutoff, out double[] masses, out double[] weights) {
			double minMass = masses1[0] + masses2[0];
			double maxMass = masses1[masses1.Length - 1] + masses2[masses2.Length - 1];
			int nbins = (int) Math.Ceiling((maxMass - minMass) / massCutoff) + 1;
			double[] m = new double[nbins];
			double[] w = new double[nbins];
			for (int i = 0; i < masses1.Length; i++) {
				for (int j = 0; j < masses2.Length; j++) {
					double mass = masses1[i] + masses2[j];
					double weight = weights1[i] * weights2[j];
					int bin = (int) ((mass - minMass) / massCutoff + 0.5);
					m[bin] += mass * weight;
					w[bin] += weight;
				}
			}
			int size = 0;
			masses = new double[nbins];
			weights = new double[nbins];
			for (int i = 0; i < nbins; i++) {
				if (m[i] != 0 && w[i] >= weightCutoff) {
					masses[size] = m[i] / w[i];
					weights[size] = w[i];
					size++;
				}
			}
			Array.Resize(ref masses, size);
			Array.Resize(ref weights, size);
		}

		public static Tuple<Molecule, Molecule> GetDifferences(string composition1, string composition2) {
			Molecule c11;
			Molecule c12;
			if (composition1.Contains("-")) {
				int index = composition1.IndexOf("-", StringComparison.InvariantCulture);
				c11 = new Molecule(composition1.Substring(0, index));
				c12 = new Molecule(composition1.Substring(index + 1));
			} else {
				c11 = new Molecule(composition1);
				c12 = new Molecule();
			}
			Molecule c21;
			Molecule c22;
			if (composition2.Contains("-")) {
				int index = composition2.IndexOf("-", StringComparison.InvariantCulture);
				c21 = new Molecule(composition2.Substring(0, index));
				c22 = new Molecule(composition2.Substring(index + 1));
			} else {
				c21 = new Molecule(composition2);
				c22 = new Molecule();
			}
			return GetDifferences(Sum(c11, c22), Sum(c21, c12));
		}

		public static Tuple<Molecule, Molecule> GetDifferences(Molecule molecule1, Molecule molecule2) {
			int[] counts1 = new int[ChemElements.Elements.Length];
			for (int j = 0; j < molecule1.AtomType.Length; j++) {
				counts1[molecule1.AtomType[j]] = molecule1.AtomCount[j];
			}
			int[] counts2 = new int[ChemElements.Elements.Length];
			for (int j = 0; j < molecule2.AtomType.Length; j++) {
				counts2[molecule2.AtomType[j]] = molecule2.AtomCount[j];
			}
			for (int i = 0; i < counts1.Length; i++) {
				if (counts1[i] > counts2[i]) {
					counts1[i] -= counts2[i];
					counts2[i] = 0;
				} else {
					counts2[i] -= counts1[i];
					counts1[i] = 0;
				}
			}
			int[] types1 = new int[counts1.Length];
			int count1 = 0;
			for (int i = 0; i < counts1.Length; i++) {
				if (counts1[i] > 0) {
					types1[count1++] = i;
				}
			}
			Array.Resize(ref types1, count1);
			Molecule diff1 = new Molecule(types1, ArrayUtils.SubArray(counts1, types1));
			int[] types2 = new int[counts2.Length];
			int count2 = 0;
			for (int i = 0; i < counts2.Length; i++) {
				if (counts2[i] > 0) {
					types2[count2++] = i;
				}
			}
			Array.Resize(ref types2, count2);
			Molecule diff2 = new Molecule(types2, ArrayUtils.SubArray(counts2, types2));
			return new Tuple<Molecule, Molecule>(diff1, diff2);
		}

		public static Molecule Sum(Molecule molecule1, Molecule molecule2) {
			return Sum(new[] {molecule1, molecule2});
		}

		public static Molecule Subtract(Molecule molecule1, Molecule molecule2) {
			int[] counts = new int[ChemElements.Elements.Length];
			for (int j = 0; j < molecule1.AtomType.Length; j++) {
				counts[molecule1.AtomType[j]] += molecule1.AtomCount[j];
			}
			for (int j = 0; j < molecule2.AtomType.Length; j++) {
				counts[molecule2.AtomType[j]] -= molecule2.AtomCount[j];
			}
			int[] types = new int[counts.Length];
			int count = 0;
			for (int i = 0; i < counts.Length; i++) {
				if (counts[i] > 0) {
					types[count++] = i;
				}
			}
			Array.Resize(ref types, count);
			return new Molecule(types, ArrayUtils.SubArray(counts, types));
		}

		public static Molecule Sum(IList<Molecule> molecules) {
			int[] n = new int[molecules.Count];
			for (int i = 0; i < n.Length; i++) {
				n[i] = 1;
			}
			return Sum(molecules, n);
		}

		//TODO: can probably be optimized. (not necessary to produce the full counts arrays.)
		public bool Contains(Molecule other) {
			int[] counts = ToCountArray();
			int[] otherCounts = other.ToCountArray();
			for (int i = 0; i < counts.Length; i++) {
				if (otherCounts[i] > counts[i]) {
					return false;
				}
			}
			return true;
		}

		private int[] ToCountArray() {
			int[] counts = new int[ChemElements.Elements.Length];
			for (int j = 0; j < AtomType.Length; j++) {
				counts[AtomType[j]] += AtomCount[j];
			}
			return counts;
		}

		public static Molecule Sum(IList<Molecule> molecules, IList<ushort> n) {
			return Sum(molecules, ArrayUtils.ToInts(n));
		}

		public static Molecule Sum(IList<Molecule> molecules, IList<int> n) {
			int[] counts = new int[ChemElements.Elements.Length];
			for (int i = 0; i < molecules.Count; i++) {
				Molecule mol = molecules[i];
				for (int j = 0; j < mol.AtomType.Length; j++) {
					counts[mol.AtomType[j]] += n[i] * mol.AtomCount[j];
				}
			}
			int[] types = new int[counts.Length];
			int count = 0;
			for (int i = 0; i < counts.Length; i++) {
				if (counts[i] != 0) {
					types[count++] = i;
				}
			}
			Array.Resize(ref types, count);
			return new Molecule(types, ArrayUtils.SubArray(counts, types));
		}

		public static Molecule Max(Molecule x, Molecule y) {
			int[] counts1 = new int[ChemElements.Elements.Length];
			for (int j = 0; j < x.AtomType.Length; j++) {
				counts1[x.AtomType[j]] = x.AtomCount[j];
			}
			int[] counts2 = new int[ChemElements.Elements.Length];
			for (int j = 0; j < y.AtomType.Length; j++) {
				counts2[y.AtomType[j]] = y.AtomCount[j];
			}
			int[] counts = new int[ChemElements.Elements.Length];
			for (int i = 0; i < ChemElements.Elements.Length; i++) {
				counts[i] = Math.Max(counts1[i], counts2[i]);
			}
			int[] types = new int[counts.Length];
			int count = 0;
			for (int i = 0; i < counts.Length; i++) {
				if (counts[i] > 0) {
					types[count++] = i;
				}
			}
			Array.Resize(ref types, count);
			return new Molecule(types, ArrayUtils.SubArray(counts, types));
		}

		public bool ContainsOnly(string[] elems) {
			HashSet<int> types = new HashSet<int>();
			string[] symbols = ChemElements.GetSymbols();
			foreach (string elem in elems) {
				int ind = ArrayUtils.IndexOf(symbols, elem);
				if (ind < 0) {
					throw new Exception("Element is not contained: " + elem);
				}
				types.Add(ind);
			}
			foreach (int atomType in AtomType) {
				if (!types.Contains(atomType)) {
					return false;
				}
			}
			return true;
		}

		public int CountAtoms(string elem) {
			Dictionary<string, int> d = ChemElements.ElementIndex;
			if (!d.ContainsKey(elem)) {
				return 0;
			}
			int index = d[elem];
			int c = Array.BinarySearch(AtomType, index);
			return c < 0 ? 0 : AtomCount[c];
		}

		public static double CalcMonoMass(string formula) {
			return new Molecule(formula).MonoIsotopicMass;
		}

		public static double CalcWeight(string formula) {
			return new Molecule(formula).MolecularWeight;
		}

		public static double ConvertToMass(double mz, int charge) {
			return mz * charge - massProton * charge;
		}

		public static double ConvertToMz(double mass, int charge) {
			return (mass + charge * massProton) / charge;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			}
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			if (obj.GetType() != GetType()) {
				return false;
			}
			return Equals((Molecule) obj);
		}

		protected bool Equals(Molecule other) {
			return ArrayUtils.EqualArrays(AtomType, other.AtomType) && ArrayUtils.EqualArrays(AtomCount, other.AtomCount);
		}

		public override int GetHashCode() {
			unchecked {
				return ((AtomType != null ? ArrayUtils.GetArrayHashCode(AtomType) : 0) * 397) ^
				       (AtomCount != null ? ArrayUtils.GetArrayHashCode(AtomCount) : 0);
			}
		}
	}
}