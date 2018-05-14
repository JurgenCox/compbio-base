using System;
using System.Collections.Generic;
using BaseLibS.Num;

namespace BaseLibS.Ms {
	/// <summary>
	/// A simple object containing a vector of masses and corresponding intensities, 
	/// with simple manipulation methods.
	/// </summary>
	public class Spectrum : IDisposable {
		public double[] Masses { get; set; }
		public double[] Intensities { get; set; }

		public Spectrum(double[] masses, double[] intensities) {
			Masses = masses;
			Intensities = intensities;
		}

		/// <summary>
		/// Number of mass/intensity pairs.
		/// </summary>
		public int Count => Masses?.Length ?? 0;

		public double MinMass => GetMass(0);
		public double MaxMass => GetMass(Count - 1);

		/// <summary>
		/// smallest index of masses for which masses[index] is greater than or equal to mass
		/// </summary>
		public int GetCeilIndex(double mass) {
			if (double.IsNaN(mass) || double.IsInfinity(mass)) {
				return -1;
			}
			return ArrayUtils.CeilIndex(Masses, mass);
		}

		/// <summary>
		/// largest index of masses for which masses[index] is less than or equal to mass
		/// </summary>
		public int GetFloorIndex(double mass) {
			if (double.IsNaN(mass) || double.IsInfinity(mass)) {
				return -1;
			}
			return ArrayUtils.FloorIndex(Masses, mass);
		}

		public int GetClosestIndex(double mass, bool outOfRangeIsInvalid) {
			if (double.IsNaN(mass) || double.IsInfinity(mass)) {
				return -1;
			}
			if (mass <= MinMass) {
				return outOfRangeIsInvalid ? -1 : 0;
			}
			if (mass >= MaxMass) {
				return outOfRangeIsInvalid ? -1 : Count - 1;
			}
			int index = Array.BinarySearch(Masses, mass); //TODO: invalid operation
			if (index >= 0) {
				return index;
			}
			index = -2 - index;
			if (Math.Abs(GetMass(index) - mass) < Math.Abs(GetMass(index + 1) - mass)) {
				return index;
			}
			return index + 1;
		}

		public double GetMass(int index) {
			return Masses.Length > 0 ? Masses[index] : double.NaN;
		}

		/// <summary>
		/// Get intensity as a function of index.
		/// </summary>
		public double GetIntensity(int index) {
			return index >= Intensities.Length ? 0 : Intensities[index];
		}

		public double GetIntensityFromMass(double mass, double dm) {
			int ind = GetClosestIndex(mass, true);
			if (ind == -1) {
				return 0;
			}
			return Math.Abs(mass - GetMass(ind)) > dm ? 0 : GetIntensity(ind);
		}

		public double InterpolateIntensity(double mass) {
			int indf = GetFloorIndex(mass);
			if (indf == -1) {
				int ind1 = GetClosestIndex(mass, true);
				return ind1 == -1 ? 0 : GetIntensity(ind1);
			}
			int indc = indf + 1;
			return indc >= Count
				? GetIntensity(indf)
				: InterpolateIntensity(mass, Masses[indf], Masses[indc], Intensities[indf], Intensities[indc]);
		}

		/// <summary>
		/// Is a itself local maximum or the right-most of two points making a local maximum.
		/// </summary>
		/// <param name="x">Intensity at the index in question.</param>
		/// <param name="m1">Intensity at that index minus 1.</param>
		/// <param name="p1">Intensity at that index plus 1.</param>
		/// <param name="m2">Intensity at that index minus 2.</param>
		/// <returns></returns>
		public bool IsMax(double x, double m1, double p1, double m2) {
			if (x > m1 && x > p1) {
				// intensity absolutely greater than immediate neighbors
				return true;
			}
			if (x > m2 && x == m1 && x > p1) {
				// intensity equal to neighbor on left but greater than neighbors outside that
				return true;
			}
			return false;
		}

		/// <summary>
		/// Move to the left as long as the next point is strictly lower than the current one but not zero.
		/// </summary>
		public int CalcMinPeakIndex(int ind) {
			while (ind > 0 && Intensities[ind - 1] != 0 && Intensities[ind - 1] < Intensities[ind]) {
				ind--;
			}
			return ind;
		}

		/// <summary>
		/// Move to the right as long as the next point is strictly lower than the current one but not zero.
		/// </summary>
		public int CalcMaxPeakIndex(int ind) {
			while (ind < Count - 1 && Intensities[ind + 1] != 0 && Intensities[ind + 1] < Intensities[ind]) {
				ind++;
			}
			return ind;
		}

		public virtual void Dispose() {
			Masses = null;
			Intensities = null;
		}

		public double[] CopyMasses() {
			double[] result = new double[Masses.Length];
			for (int i = 0; i < Masses.Length; i++) {
				result[i] = Masses[i];
			}
			return result;
		}

		public double[] CopyIntensities() {
			double[] result = new double[Count];
			for (int i = 0; i < Count; i++) {
				result[i] = GetIntensity(i);
			}
			return result;
		}

		private static double InterpolateIntensity(double mass, double massL, double massH, double intL, double intH) {
			double wL = ((massH - mass) / (massH - massL));
			double wH =  ((mass - massL) / (massH - massL));
			return intL * wL + intH * wH;
		}

		private const int pointsPerSigma = 7;
		private const int sigmasPerPeak = 4;

		public Spectrum Smooth(double resolution, bool inMda) {
			if (Masses.Length == 0) {
				return this;
			}
			double[] intensities;
			double[] masses = CalcSpec(resolution, inMda, out intensities);
			int[] vailds = GetZeroRegions(intensities);
			return new Spectrum(ArrayUtils.SubArray(masses, vailds), ArrayUtils.SubArray(intensities, vailds));
		}

		private static int[] GetZeroRegions(double[] intensities) {
			bool inRegion = false;
			int currentStart = -1;
			bool[] mask = new bool[intensities.Length];
			for (int i = 0; i < intensities.Length; i++) {
				if (intensities[i] == 0) {
					if (!inRegion) {
						currentStart = i;
						inRegion = true;
					}
				} else {
					if (inRegion) {
						if (i - currentStart > 2) {
							for (int j = currentStart + 1; j < i - 1; j++) {
								mask[j] = true;
							}
						}
						currentStart = -1;
						inRegion = false;
					}
				}
			}
			if (inRegion) {
				for (int j = currentStart + 1; j < mask.Length; j++) {
					mask[j] = true;
				}
			}
			List<int> valids = new List<int>();
			for (int i = 0; i < mask.Length; i++) {
				if (!mask[i]) {
					valids.Add(i);
				}
			}
			return valids.ToArray();
		}

		private double[] CalcSpec(double resolution, bool inMda, out double[] intensities) {
			double[] masses = GetMasses(resolution, inMda);
			intensities = new double[masses.Length];
			for (int i = 0; i < Masses.Length; i++) {
				double m = Masses[i];
				double sigma = inMda ? resolution / 1000 : m / resolution;
				int centerInd = ArrayUtils.ClosestIndex(masses, m);
				for (int ind = centerInd - pointsPerSigma * sigmasPerPeak;
					ind <= centerInd + pointsPerSigma * sigmasPerPeak;
					ind++) {
					double dm = masses[ind] - m;
					double ex = -dm * dm / sigma * sigma * 0.5;
					intensities[ind] += Math.Exp(ex) * Intensities[i];
				}
			}
			return masses;
		}

		private double[] GetMasses(double resolution, bool inMda) {
			double mstart = Masses[0];
			double mend = Masses[Masses.Length - 1];
			double sigmaStart = inMda ? resolution / 1000 : mstart / resolution;
			double sigmaEnd = inMda ? resolution / 1000 : mend / resolution;
			mstart -= 2 * sigmasPerPeak * sigmaStart;
			mend += 2 * sigmasPerPeak * sigmaEnd;
			List<double> masses1 = new List<double>();
			double m = mstart;
			while (m <= mend) {
				masses1.Add(m);
				double dm = (inMda ? resolution / 1000 : m / resolution) / pointsPerSigma;
				m += dm;
			}
			return masses1.ToArray();
		}

		public Spectrum SuppressZeroes() {
			bool[] supress = new bool[Masses.Length];
			int x1 = GetLeadingZeroesInd(Intensities);
			for (int i = 0; i < x1; i++) {
				supress[i] = true;
			}
			int x2 = GetTrailingZeroesInd(Intensities);
			for (int i = x2 + 1; i < supress.Length; i++) {
				supress[i] = true;
			}
			bool inRegion = false;
			int start = -1;
			for (int i = x1 + 1; i < x2; i++) {
				if (Intensities[i] == 0 && !inRegion) {
					inRegion = true;
					start = i;
				}
				if (Intensities[i] > 0 && inRegion) {
					int stop = i;
					if (stop - start > 2) {
						for (int j = start + 1; j < stop - 1; j++) {
							supress[j] = true;
						}
					}
					inRegion = false;
					start = -1;
				}
			}
			List<int> valids = new List<int>();
			for (int i = 0; i < supress.Length; i++) {
				if (!supress[i]) {
					valids.Add(i);
				}
			}
			return new Spectrum(ArrayUtils.SubArray(Masses, valids), ArrayUtils.SubArray(Intensities, valids));
		}

		private static int GetLeadingZeroesInd(double[] intensities) {
			int ind = -1;
			while (ind < intensities.Length - 1 && intensities[ind + 1] == 0) {
				ind++;
			}
			return ind;
		}

		private static int GetTrailingZeroesInd(double[] intensities) {
			int ind = intensities.Length;
			while (ind > 0 && intensities[ind - 1] == 0) {
				ind--;
			}
			return ind;
		}
	}
}