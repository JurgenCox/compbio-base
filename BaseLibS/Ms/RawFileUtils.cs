using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Num;

namespace BaseLibS.Ms {
	/// <summary>
	/// A collection of static methods to manipulate RawFiles.
	/// </summary>
	public static class RawFileUtils {
		public static FragmentationTypeEnum IntToFragmentationTypeEnum(int i) {
			switch (i) {
				case 0: return FragmentationTypeEnum.Cid;
				case 1: return FragmentationTypeEnum.Hcd;
				case 2: return FragmentationTypeEnum.Etd;
				case 3: return FragmentationTypeEnum.Pqd;
				case 4: return FragmentationTypeEnum.Ethcd;
				case 5: return FragmentationTypeEnum.Etcid;
				case 6: return FragmentationTypeEnum.Uvpd;
				case 7: return FragmentationTypeEnum.Unknown;
				default: throw new Exception("Never get here.");
			}
		}
		public static int FragmentationTypeEnumToInt(FragmentationTypeEnum e) {
			switch (e) {
				case FragmentationTypeEnum.Cid: return 0;
				case FragmentationTypeEnum.Hcd: return 1;
				case FragmentationTypeEnum.Etd: return 2;
				case FragmentationTypeEnum.Pqd: return 3;
				case FragmentationTypeEnum.Ethcd: return 4;
				case FragmentationTypeEnum.Etcid: return 5;
				case FragmentationTypeEnum.Uvpd: return 6;
				case FragmentationTypeEnum.Unknown: return 7;
				default: throw new Exception("Never get here.");
			}
		}
		public static MassAnalyzerEnum IntToMassAnalyzerEnum(int i) {
			switch (i) {
				case 0: return MassAnalyzerEnum.Ftms;
				case 1: return MassAnalyzerEnum.Itms;
				case 2: return MassAnalyzerEnum.Tof;
				case 3: return MassAnalyzerEnum.Unknown;
				default: throw new Exception("Never get here.");
			}
		}
		public static int MassAnalyzerEnumToInt(MassAnalyzerEnum e) {
			switch (e) {
				case MassAnalyzerEnum.Ftms: return 0;
				case MassAnalyzerEnum.Itms: return 1;
				case MassAnalyzerEnum.Tof: return 2;
				case MassAnalyzerEnum.Unknown: return 3;
				default: throw new Exception("Never get here.");
			}
		}
		public static bool IsMonotoneIncreasing(double[] c, out int startPos) {
			for (int i = 0; i < c.Length - 1; i++) {
				if (c[i] > c[i + 1]) {
					startPos = i + 1;
					return false;
				}
			}
			startPos = 0;
			return true;
		}
		internal static void Interpolate(ref int[] indices, ref double[] values) {
			List<int> inds = new List<int>();
			List<double> vals = new List<double>();
			int pos = 0;
			inds.Add(indices[0]);
			vals.Add(values[0]);
			while (pos < values.Length - 1) {
				pos++;
				if (indices[pos] > inds[inds.Count - 1] + 1) { } else {
					inds.Add(indices[pos]);
					vals.Add(values[pos]);
				}
			}
		}
		internal static double[] MakeMonotone(IDictionary<int, double> mins, out int[] indices) {
			int[] keys = mins.Keys.ToArray();
			Array.Sort(keys);
			double[] values = new double[keys.Length];
			for (int i = 0; i < keys.Length; i++) {
				values[i] = mins[keys[i]];
			}
			while (true) {
				bool changed = IncreaseMonotonicity(ref keys, ref values);
				if (!changed) {
					break;
				}
			}
			indices = keys;
			return values;
		}
		private static bool IncreaseMonotonicity(ref int[] keys, ref double[] values) {
			List<int> toBeRemoved = new List<int>();
			for (int i = 0; i < keys.Length - 1; i++) {
				if (values[i] > values[i + 1]) {
					toBeRemoved.Add(i);
					i++;
				}
			}
			if (toBeRemoved.Count > 0) {
				int[] valids = ArrayUtils.Complement(toBeRemoved, keys.Length);
				keys = keys.SubArray(valids);
				values = values.SubArray(valids);
				return true;
			}
			return false;
		}
		internal static void InitMassGrid(RawFileLayer layer, IDictionary<int, double> mins) {
			for (int i = 0; i < layer.Ms1Count; i++) {
				layer.GetMs1SpectrumArray(i, false, out double[] masses, out float[] _);
				for (int j = 0; j < masses.Length - 1; j++) {
					double m1 = masses[j];
					double m2 = masses[j + 1];
					double dm = m2 - m1;
					int intMass = (int) ((m1 + m2) / 2);
					if (mins.ContainsKey(intMass)) {
						mins[intMass] = Math.Min(mins[intMass], dm);
					} else {
						mins.Add(intMass, dm);
					}
				}
			}
		}
		public static int SignalTypeToInt(SignalType signalType) {
			switch (signalType) {
				case SignalType.Centroid: return 0;
				case SignalType.Profile: return 1;
			}
			throw new Exception("Never get here.");
		}
		public static SignalType IntToSignalType(int x) {
			switch (x) {
				case 0: return SignalType.Centroid;
				case 1: return SignalType.Profile;
			}
			throw new Exception("Never get here.");
		}
		public static double[] GetMzGrid(double minMz, double maxMz, double resolution, double nsigma) {
			if (resolution == 0) {
				resolution = 30000;
			}
			const double ff = 0.5;
			double dm1 = 0.5 * minMz / resolution;
			double dm2 = 0.5 * maxMz / resolution;
			double mzLower = minMz - nsigma * dm1;
			double mzUpper = maxMz + nsigma * dm2;
			List<double> result = new List<double>();
			for (double mx = mzLower; mx <= mzUpper; mx += 0.5 * mx / resolution * ff) {
				result.Add(mx);
			}
			return result.ToArray();
		}

		public static (double[], float[]) CalcMergedSpectrum(double[][] masses, float[][] intensities)
		{
			double peakMergeThreshold = GetPeakMergeThreshold(masses);
			// collect all peaks within one long list and order by mz values 
			List<(double mass, float intensity)> allPeaks = new List<(double mass, float intensity)>();
			for (int k1 = 0; k1 < masses.Length; k1++)
			{
				for (int i1 = 0; i1 < masses[k1].Length; i1++)
				{
					allPeaks.Add((masses[k1][i1], intensities[k1][i1]));
				}
			}
			List<(double mass, float intensity)> sortedPeaks = allPeaks.OrderBy(t => t.mass).ToList();
			int sortedPeaksCount = sortedPeaks.Count;
			List<double> mergedMasses = new List<double>();
			List<float> mergedIntensities = new List<float>();

			// merge peaks in the list which lie less than the threshold distance away from each other
			// final mz value is the intensity-weighted average 
			int i = 0;
			while (i < sortedPeaksCount)
			{
				(double mass, float intensity) p = sortedPeaks[i];
				double wavgMass = p.mass * p.intensity;
				float intensity = p.intensity;
				while (i < sortedPeaksCount - 1)
				{
					(double mass, float intensity) pj = sortedPeaks[++i];
					if ((pj.mass - sortedPeaks[i - 1].mass) / pj.mass * 1e6 > peakMergeThreshold) break;
					wavgMass += pj.mass * pj.intensity;
					intensity += pj.intensity;
				}
				if (i == sortedPeaksCount - 1) i++;
				mergedMasses.Add(wavgMass / intensity);
				mergedIntensities.Add(intensity);
			}

			return (mergedMasses.ToArray(), mergedIntensities.ToArray());
		}
		private static double GetPeakMergeThreshold(double[][] masses1)
		{
			// the threshold to merge is half the minimum peak distance within the spectra
			// but maximum 7 ppm 
			double peakMergeThreshold = 7;
			foreach (double[] m1 in masses1)
			{
				List<double> masses = m1.OrderBy(m => m).ToList();
				for (int c = 0; c < masses.Count() - 1; c++)
				{
					double d = (masses[c + 1] - masses[c]) / masses[c] * 1e6 * 0.5;
					if (peakMergeThreshold > d) peakMergeThreshold = d;
				}

			}
			return peakMergeThreshold;
		}


	}
}