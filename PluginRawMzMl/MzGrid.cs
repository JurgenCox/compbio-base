using System;
using System.Collections.Generic;
using BaseLibS.Ms;
using BaseLibS.Num;

namespace PluginRawMzMl{
	/// <summary> 
	/// Copied from BrukerTdfRawFile
	/// TODO temporary solution
	/// </summary>
	public class MzGrid {
		private static readonly double factor = 1.0 / Math.Sqrt(2 * Math.PI) / 6.98;
		public readonly double resolution;
		public double MzMax { get; }
		public double MzMin { get; }
		private double mz0;
		private readonly double p;
		public readonly double nsigma;
		private double[] grid;
		private float[] tmpBufIntensities;
		public MzGrid(double mzMin, double mzMax, double resolution, double nsigma, double gridSpacing) {
			this.nsigma = nsigma;
			MzMin = mzMin;
			MzMax = mzMax;
			this.resolution = resolution;
			p = 1 + 0.5 * gridSpacing / resolution;
			grid = CreateGrid(mzMin, mzMax, nsigma, resolution, gridSpacing);
			if (grid.Length > 0) {
				mz0 = grid[0];
			}
			tmpBufIntensities = new float[grid.Length];
		}
		public void ExpandIfNeeded(double mzMin, double mzMax) {
			double gridMzMin = MzMin;
			double gridMzMax = MzMax;
			double spectrumMzMin = Math.Min(gridMzMin, mzMin);
			double spectrumMzMax = Math.Max(gridMzMax, mzMax * 1.1);
			if (spectrumMzMax > gridMzMax || spectrumMzMin < gridMzMin) {
				grid = CreateGrid(spectrumMzMin, spectrumMzMax, nsigma, resolution, gridMzMax);
				if (grid.Length > 0) {
					mz0 = grid[0];
				}
				tmpBufIntensities = new float[grid.Length];
			}
		}
		public void SmoothIntensities(double[] masses, float[] intensities, out double[] smoothedMasses,
			out float[] smoothedIntensities) {
			if (masses[0] <= mz0 + 1) {
				int ind = 0;
				while (masses[ind] <= mz0 + 1) {
					ind++;
					if (ind >= masses.Length) {
						break;
					}
				}
				if (ind > 0) {
					masses = masses.SubArray(ind, masses.Length - ind);
					intensities = intensities.SubArray(ind, intensities.Length - ind);
				}
			}
			double mzMin = masses[0] * (1 - nsigma / 2 / resolution);
			double mzMax = masses[masses.Length - 1] * (1 + nsigma / 2 / resolution);
			int gridIndexMin = ClosestIndex(mzMin);
			int gridIndexMax = ClosestIndex(mzMax) + 1;
			int curIndex = 0;
			smoothedMasses = new double[gridIndexMax - gridIndexMin + 3];
			smoothedIntensities = new float[gridIndexMax - gridIndexMin + 3];
			smoothedMasses[curIndex] = grid[gridIndexMin - 1];
			curIndex += 1;
			int lastUind = -1;
			for (int i = 0; i < masses.Length; i++) {
				double mz = masses[i];
				float intensity = intensities[i];
				double dm = 0.5 * mz / resolution;
				double mzLower = mz - nsigma * dm;
				double mzUpper = mz + nsigma * dm;
				int lind = ClosestIndex(mzLower);
				int uind = ClosestIndex(mzUpper);
				if (i == 0) {
					lastUind = uind;
				}
				if (lind >= grid.Length) {
					continue;
				}
				if (lind - lastUind > 2) {
					smoothedMasses[curIndex] = grid[lastUind + 1];
					curIndex += 1;
					smoothedMasses[curIndex] = grid[lind - 1];
					curIndex += 1;
				} else if (lind - lastUind > 1) {
					smoothedMasses[curIndex] = grid[lastUind + 1];
					curIndex += 1;
				} else if (i != 0) {
					curIndex += lind - lastUind - 1;
				}
				lastUind = uind;
				if (lind >= grid.Length - 1) {
					continue;
				}
				double a = grid[lind + 1] - grid[lind];
				double f = a * factor / dm;
				for (int ind = lind; ind <= uind; ind++) {
					if (ind >= grid.Length) {
						break;
					}
					double gridMz = grid[ind];
					double x = (gridMz - mz) / dm;
					smoothedIntensities[curIndex] += (float)(intensity * Math.Exp(-0.5 * x * x) * f);
					smoothedMasses[curIndex] = gridMz;
					curIndex += 1;
				}
			}
			if (gridIndexMax < grid.Length) {
				smoothedMasses[curIndex] = grid[gridIndexMax];
				curIndex += 1;
			}
			smoothedMasses = smoothedMasses.SubArray(curIndex);
			smoothedIntensities = smoothedIntensities.SubArray(curIndex);
		}
		public double this[int index] => grid[index];
		public Spectrum Combine(Spectrum[] spectra) {
			int gridLength = grid.Length;
			Array.Clear(tmpBufIntensities, 0, tmpBufIntensities.Length);
			int lowerIndex = int.MaxValue;
			int upperIndex = int.MinValue;
			foreach (Spectrum spectrum in spectra) {
				double[] masses = spectrum.Masses;
				float[] intensities = spectrum.Intensities;
				if (masses.Length == 0) {
					continue;
				}
				double prevMass = masses[0];
				float prevIntensity = intensities[0];
				// TODO: Spectrum.Count == 1
				for (int i = 1; i < masses.Length; i++) {
					double mass = masses[i];
					float intensity = intensities[i];
					int index = (int)Math.Log(mass / mz0, p);
					if (index < 0 || index > gridLength) {
						continue;
					}
					lowerIndex = Math.Min(lowerIndex, index);
					upperIndex = Math.Max(upperIndex, index);
					tmpBufIntensities[index] += Interpolate(grid[index], prevMass, mass, prevIntensity, intensity);
					prevMass = mass;
					prevIntensity = intensity;
				}
			}
			if (lowerIndex == int.MaxValue) {
				return new Spectrum(new double[0], new float[0]);
			}
			lowerIndex = Math.Max(0, lowerIndex - 1);
			upperIndex = Math.Min(gridLength - 1, upperIndex + 1);
			int resultLen = upperIndex - lowerIndex + 1;
			double[] resultMasses = new double[resultLen];
			float[] resultIntensities = new float[resultLen];

			// TODO: suppress zeroes performance
			Array.Copy(grid, lowerIndex, resultMasses, 0, resultLen);
			Array.Copy(tmpBufIntensities, lowerIndex, resultIntensities, 0, resultLen);
			return new Spectrum(resultMasses, resultIntensities);
		}
		private static float Interpolate(double val, double xMin, double xMax, float yMin, float yMax) {
			double num1 = (xMax - val) / (xMax - xMin);
			double num2 = (val - xMin) / (xMax - xMin);
			//return (float)(yMin);
			return (float)(yMin * num1 + yMax * num2);
		}
		private static double[] CreateGrid(double mzMin, double mzMax, double nsigma, double resolution,
			double gridSpacing) {
			double d = 0.5 * nsigma / resolution;
			mzMin *= 1 - d;
			mzMax *= 1 + d;
			List<double> grid = new List<double>();
			double g = 0.5 * gridSpacing / resolution;
			for (double mz = mzMin; mz <= mzMax; mz += mz * g) {
				grid.Add(mz);
			}
			return grid.ToArray();
		}
		/// <summary>
		/// Closest grid index with mzGrid less than mz
		/// </summary>
		/// <param name="mz"></param>
		/// <returns></returns>
		public int ClosestIndex(double mz) {
			return (int)Math.Log(mz / mz0, p);
		}
		public static Spectrum SmoothIntensities(MzGrid mzGrid, double[] expVals, double gridExp, double[] masses1,
			float[] intensities1, double resolution1) {
			if (masses1.Length == 0) {
				return new Spectrum(masses1, intensities1);
			}
			double mzMin = masses1[0] * (1 - mzGrid.nsigma / 2 / resolution1);
			double mzMax = masses1[masses1.Length - 1] * (1 + mzGrid.nsigma / 2 / resolution1);
			int gridIndexMin = mzGrid.ClosestIndex(mzMin);
			int gridIndexMax = mzGrid.ClosestIndex(mzMax) + 1;
			int curIndex = 0;
			int c = (gridIndexMax - gridIndexMin) / 50 + 3;
			int intensDataLen = Math.Max(c, 16);
			float[] intensData = new float[intensDataLen];
			GrowableArrayDouble mzs = new GrowableArrayDouble(c);
			double[] grid = mzGrid.grid;
			if (gridIndexMin <= grid.Length) {
				mzs[curIndex] = grid[gridIndexMin - 1];
				curIndex += 1;
			}
			int lastUind = -1;
			for (int i = 0; i < masses1.Length; i++) {
				double mz = masses1[i];
				double dm = 0.5 * mz / resolution1;
				double dmx = mzGrid.nsigma * dm;
				int lind = mzGrid.ClosestIndex(mz - dmx);
				int uind = mzGrid.ClosestIndex(mz + dmx);
				if (i == 0) {
					lastUind = uind;
				}
				if (lind >= grid.Length) {
					continue;
				}
				if (lind - lastUind > 2) {
					mzs[curIndex] = grid[lastUind + 1];
					curIndex += 1;
					mzs[curIndex] = grid[lind - 1];
					curIndex += 1;
				} else if (lind - lastUind > 1) {
					mzs[curIndex] = grid[lastUind + 1];
					curIndex += 1;
				} else if (i != 0) {
					curIndex += lind - lastUind - 1;
				}
				lastUind = uind;
				if (lind >= grid.Length - 1) {
					continue;
				}
				double f = intensities1[i] * (grid[lind + 1] - grid[lind]) * factor / dm;
				if (expVals != null) {
					for (int ind = lind; ind <= uind; ind++) {
						if (ind >= grid.Length) {
							break;
						}
						double gridMz = grid[ind];
						double x = Math.Abs(gridMz - mz) / dm;
						if (curIndex >= intensDataLen) {
							intensData = Ensure(curIndex + 1, intensData);
							intensDataLen = intensData.Length;
						}
						intensData[curIndex] += (float)(f * expVals[(int)(x / gridExp)]);
						mzs[curIndex] = gridMz;
						curIndex += 1;
					}
				} else {
					for (int ind = lind; ind <= uind; ind++) {
						if (ind >= grid.Length) {
							break;
						}
						double gridMz = grid[ind];
						double x = (gridMz - mz) / dm;
						if (curIndex >= intensDataLen) {
							intensData = Ensure(curIndex + 1, intensData);
							intensDataLen = intensData.Length;
						}
						intensData[curIndex] += (float)(f * Math.Exp(-0.5 * x * x));
						mzs[curIndex] = gridMz;
						curIndex += 1;
					}
				}
			}
			if (gridIndexMax < grid.Length) {
				mzs[curIndex] = grid[gridIndexMax];
				curIndex += 1;
			}
			return new Spectrum(mzs.ToArray(curIndex), ToArray(curIndex, intensData));
		}
		public static Dictionary<int, float> GetMap(MzGrid mzGrid, double[] expVals, double gridExp, double[] masses1,
			float[] intensities1, double resolution1) {
			Dictionary<int, float> result = new Dictionary<int, float>();
			double[] grid = mzGrid.grid;
			for (int i = 0; i < masses1.Length; i++) {
				double mz = masses1[i];
				double dm = 0.5 * mz / resolution1;
				double dmx = mzGrid.nsigma * dm;
				int lind = mzGrid.ClosestIndex(mz - dmx);
				int uind = mzGrid.ClosestIndex(mz + dmx);
				double f = intensities1[i] * (grid[lind + 1] - grid[lind]) * factor / dm;
				for (int ind = lind; ind <= uind; ind++) {
					double gridMz = grid[ind];
					double x = Math.Abs(gridMz - mz) / dm;
					float q = (float)(f * expVals[(int)(x / gridExp)]);
					if (result.ContainsKey(ind)) {
						result[ind] += q;
					} else {
						result.Add(ind, q);
					}
				}
				if (!result.ContainsKey(lind - 1)) {
					result.Add(lind - 1, 0);
				}
				if (!result.ContainsKey(uind + 1)) {
					result.Add(uind + 1, 0);
				}
			}
			return result;
		}
		public static Spectrum GetSpectrum(Dictionary<int, float>[] maps, double[] grid) {
			Dictionary<int, float> sum = new Dictionary<int, float>();
			foreach (Dictionary<int, float> map in maps) {
				foreach (KeyValuePair<int, float> p in map) {
					if (sum.ContainsKey(p.Key)) {
						sum[p.Key] += p.Value;
					} else {
						sum.Add(p.Key, p.Value);
					}
				}
			}
			List<double> masses = new List<double>();
			List<float> intensities = new List<float>();
			for (int i = 0; i < grid.Length; i++) {
				if (sum.ContainsKey(i)) {
					masses.Add(grid[i]);
					intensities.Add(sum[i]);
				}
			}
			return new Spectrum(masses.ToArray(), intensities.ToArray());
		}
		public static float[] ToArray(int length, float[] intensData) {
			float[] result = new float[length];
			Array.Copy(intensData, result, Math.Min(result.Length, intensData.Length));
			return result;
		}
		private static float[] Ensure(int size, float[] data) {
			int newSize = 2 * data.Length;
			newSize = Math.Max(size, newSize);
			float[] newData = new float[newSize];
			Array.Copy(data, newData, data.Length);
			return newData;
		}
	}
}