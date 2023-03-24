using System;
using System.Collections;
using System.Collections.Generic;
using BaseLibS.Num;

namespace PluginRawMzMl{
	/// <summary> 
	/// Copied from BrukerTdfRawFile
	/// TODO temporal solution
	/// </summary>
	public class MzGrid : IReadOnlyCollection<double>{
		public readonly double Resolution;
		public readonly double Nsigma;

		private readonly double _mz0;
		private readonly double _p;

		private double[] _grid;

		public MzGrid(double mzMin, double mzMax, double resolution, double nsigma){
			Resolution = resolution;
			Nsigma = nsigma;
			const double ff = 0.5;
			mzMin *= 1 - 0.5 * nsigma / resolution;
			mzMax *= 1 + 0.5 * nsigma / resolution;
			List<double> grid = new List<double>();
			for (double mz = mzMin; mz <= mzMax; mz += mz * 0.5 * ff / resolution){
				grid.Add(mz);
			}
			_p = 1 + 0.5 * ff / resolution;
			_mz0 = mzMin;
			_grid = grid.ToArray();
		}

		public double this[int index] => _grid[index];

		/// <summary>
		/// Closest grid index with mzGrid less than mz
		/// </summary>
		/// <param name="mz"></param>
		/// <returns></returns>
		public int ClosestIndex(double mz){
			int left = (int) Math.Log(mz / _mz0, _p);
			int right = left + 1;
			if (right > _grid.Length - 1){
				return left;
			}
			if (left < 0){
				return 0;
			}
			return mz - _grid[left] < _grid[right] - mz ? left : right;
		}

		public IEnumerator<double> GetEnumerator(){
			return (_grid as IEnumerable<double>).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

		public int Count => _grid.Length;

		/// <summary>
		/// Copied from BrukerTdfRawFile
		/// TODO temporal solution
		/// </summary>
		/// <param name="scanNumberMin"></param>
		/// <param name="scanNumberMax"></param>
		/// <param name="imsIndexMin"></param>
		/// <param name="imsIndexMax"></param>
		/// <param name="readCentroids"></param>
		/// <param name="masses"></param>
		/// <param name="intensities"></param>
		/// <param name="resolution"></param>
		/// <param name="mzMin"></param>
		/// <param name="mzMax"></param>
		public void SmoothIntensities(double[] massesIn, double[] intensitiesIn, out double[] massesOut,
			out float[] intensitiesOut){
			double mzMin = massesIn[0] * (1 - Nsigma / 2 / Resolution);
			double mzMax = massesIn[massesIn.Length - 1] * (1 + Nsigma / 2 / Resolution);
			int gridIndexMin = ClosestIndex(mzMin);
			if (gridIndexMin == 0){
				gridIndexMin = 1;
			}
			int gridIndexMax = ClosestIndex(mzMax) + 1;
			int curIndex = 0;
			intensitiesOut = new float[gridIndexMax - gridIndexMin + 3];
			massesOut = new double[gridIndexMax - gridIndexMin + 3];
			massesOut[curIndex] = _grid[gridIndexMin - 1];
			curIndex += 1;
			int lastUind = -1;
			double intensityMin = double.MaxValue;
			for (int i = 0; i < intensitiesIn.Length; i++){
				if (intensitiesIn[i] != 0.0 && intensitiesIn[i] < intensityMin){
					intensityMin = intensitiesIn[i];
				}
			}
			if (intensityMin == double.MaxValue) intensityMin = 0;
			for (int i = 0; i < massesIn.Length; i++){
				double mz = massesIn[i];
				double intensity = intensitiesIn[i] <= intensityMin ? 0 : intensitiesIn[i];
				double dm = 0.5 * mz / Resolution;
				double mzLower = mz - Nsigma * dm;
				double mzUpper = mz + Nsigma * dm;
				int lind = ClosestIndex(mzLower);
				int uind = ClosestIndex(mzUpper);
				if (i == 0){
					lastUind = uind;
				}
				if (lind - lastUind > 2){
					massesOut[curIndex] = _grid[lastUind + 1];
					curIndex += 1;
					massesOut[curIndex] = _grid[lind - 1];
					curIndex += 1;
				} else if (lind - lastUind > 1){
					massesOut[curIndex] = _grid[lastUind + 1];
					curIndex += 1;
				} else if (i != 0){
					curIndex += lind - lastUind - 1;
				}
				lastUind = uind;
				double a = _grid[lind + 1] - _grid[lind];
				for (int ind = lind; ind <= uind; ind++){
					double gridMz = _grid[ind];
					double x = (gridMz - mz) / dm;
					//TODO: 6.98
					//TODO: define constant
					double intens = intensity * Math.Exp(-0.5 * x * x) / Math.Sqrt(2 * Math.PI) / dm * a / 6.98;
					intensitiesOut[curIndex] += (float)intens;
					massesOut[curIndex] = gridMz;
					curIndex += 1;
				}
			}
			massesOut[curIndex] = _grid[gridIndexMax];
			curIndex += 1;
			massesOut = massesOut.SubArray(curIndex);
			intensitiesOut = intensitiesOut.SubArray(curIndex);
		}
	}
}