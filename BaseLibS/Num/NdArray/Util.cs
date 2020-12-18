using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibS.Num.NdArray{
	public static class Util{
		public static NdArrayDouble Ones(int count){
			return Repeat(1.0, count);
		}

		public static NdArrayDouble Repeat(double ele, int count){
			return new NdArrayDouble(Enumerable.Repeat(ele, count).ToArray());
		}

		public static NdArray<T> Repeat<T>(T ele, int count){
			return new NdArray<T>(Enumerable.Repeat(ele, count).ToArray());
		}

		public static NdArrayDouble Range(int start, int stop, int step = 1){
			if ((step == 0) || (start < stop && step < 0) || (start > stop && step > 0)){
				throw new ArgumentException($"cannot create range [{start},{stop}) with step size {step}");
			}
			IEnumerable<int> data = start < stop
				? Enumerable.Range(start, Math.Abs(stop - start))
				: Enumerable.Range(-start, Math.Abs(start - stop)).Select(x => -x);
			return new NdArrayDouble(data.Where(i => i % step == 0).Select(Convert.ToDouble).ToArray());
		}

		public static NdArray<T> From2D<T>(this T[,] array){
			int rows = array.GetLength(0);
			int cols = array.GetLength(1);
			T[] data = array.Cast<T>().ToArray();
			return new NdArray<T>(data, new[]{rows, cols});
		}

		public static NdArray<double> Stack(params NdArray<double>[] arrs){
			List<double> data = new List<double>();
			NdArray<double> first = arrs.First();
			data.AddRange(first);
			int size = first.Count;
			int newDim = 1;
			foreach (NdArray<double> arr in arrs.Skip(1)){
				if (arr.Count != size){
					throw new ArgumentException($"Array sizes do not match, had {first.Shape}, found {arr.Shape}");
				}
				data.AddRange(arr);
				newDim++;
			}
			List<int> shape = new List<int>(){newDim};
			shape.AddRange(first.Shape);
			return new NdArray<double>(data.ToArray(), shape.ToArray());
		}

		public static NdArray<TResult> Map<T, TResult>(this NdArray<T> array, Func<T, TResult> func){
			TResult[] data;
			if (array.Dim == 1 || array.IsDense) // fast track
			{
				data = new TResult[array.Count];
				int s = array.Stride.Last();
				for (int i = 0; i < array.Count; i++){
					data[i] = func(array.Data[(i * s) + array.Offset]);
				}
			} else{
				data = array.Select(func).ToArray();
			}
			return new NdArray<TResult>(data, array.Shape);
		}

		public static NdArray<TResult> Broadcast<T, TResult>(Func<T, T, TResult> func, T a, NdArray<T> b){
			return Broadcast((y, x) => func(x, y), b, a);
		}

		public static NdArray<TResult> Broadcast<T, TResult>(Func<T, T, TResult> func, NdArray<T> a, T b){
			if (a.IsDense) // fast track
			{
				T[] data = a.Data;
				TResult[] newData = new TResult[a.Count];
				for (int i = 0; i < a.Count; i++){
					newData[i] = func(data[i + a.Offset], b);
				}
				return new NdArray<TResult>(newData, a.Shape);
			}
			return Broadcast(func, a, new NdArray<T>(new[]{b}, new[]{1}, new[]{0}));
		}

		public static NdArray<TResult> Broadcast<T, TResult>(Func<T, T, TResult> func, NdArray<T> a, NdArray<T> b){
			if (a.IsDense & b.IsDense){
				T[] dataA = a.Data;
				T[] dataB = b.Data;
				int n = dataA.Length;
				int m = dataB.Length;
				if (a.Shape.SequenceEqual(b.Shape)){
					TResult[] newData = new TResult[m];
					for (int i = 0; i < n; i++){
						newData[i] = func(dataA[i], dataB[i]);
					}
					return new NdArray<TResult>(newData, a.Shape);
				}
				if (a.Dim == 1 && b.Dim == 2){
					TResult[] newData = new TResult[m];
					if (n == b.Shape[1]){
						for (int i = 0; i < m; i++){
							newData[i] = func(dataA[i % n], dataB[i]); // indices -> 0,1,2,0,1,2,0,1,2,...
						}
					} else // n == b.Shape[0]
					{
						for (int i = 0; i < m; i++){
							newData[i] = func(dataA[i / n], dataB[i]); // indices -> 0,0,0,1,1,1,2,2,2...
						}
					}
					return new NdArray<TResult>(newData, b.Shape);
				}
				if (b.Dim == 1 && a.Dim == 2){
					return Broadcast((y, x) => func(x, y), b, a);
				}
			}
			int[] outputShape;
			List<NdArray<T>> extendedDim = ExtendDimensions(new[]{a, b}, out outputShape);
			NdArray<T> aBroad = extendedDim[0];
			NdArray<T> bBroad = extendedDim[1];
			TResult[] data = RowMajorIndexEnumerator.GetPerm(outputShape).Select(idx => func(aBroad[idx], bBroad[idx]))
				.ToArray();
			return new NdArray<TResult>(data, outputShape);
		}

		public static NdArray<TResult> Broadcast<T, TResult>(Func<IEnumerable<T>, TResult> func,
			params NdArray<T>[] arrs){
			int[] outputShape;
			List<NdArray<T>> extendedDim = ExtendDimensions(arrs, out outputShape);
			TResult[] data = RowMajorIndexEnumerator.GetPerm(outputShape)
				.Select(idx => func(extendedDim.Select(arr => arr[idx]))).ToArray();
			return new NdArray<TResult>(data, outputShape);
		}

		private static List<NdArray<T>> ExtendDimensions<T>(NdArray<T>[] arrs, out int[] outputShape){
			IOrderedEnumerable<NdArray<T>> sorted = arrs.OrderByDescending(arr => arr.Dim);
			int maxDim = sorted.First().Dim;
			List<NdArray<T>> extendedDim = arrs.Select(arr => {
				int missingDim = maxDim - arr.Dim;
				IEnumerable<int> shape = Enumerable.Repeat(1, missingDim).Concat(arr.Shape);
				IEnumerable<int> stride = Enumerable.Repeat(0, missingDim).Concat(arr.Stride);
				return new NdArray<T>(arr.Data, shape.ToArray(), stride.ToArray(), arr.Offset);
			}).ToList();
			outputShape = Enumerable.Range(0, maxDim).Select(i => extendedDim.Max(arr => arr.Shape[i])).ToArray();
			foreach (NdArray<T> arr in extendedDim){
				if (outputShape.Zip(arr.Shape, (nRes, nArr) => nArr != 1 && nArr != nRes).Any(i => i)){
					throw new ArgumentException(
						$"Cannot broadcast shape ({string.Join(",", arr.Shape)}) onto ({string.Join(",", outputShape)})");
				}
			}
			return extendedDim;
		}
	}
}