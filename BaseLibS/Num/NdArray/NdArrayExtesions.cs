using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BaseLibS.Num.NdArray.Slice;

namespace BaseLibS.Num.NdArray{
	public static class NdArrayExtesions{
		/// <summary>
		/// Convert NdArray into standard 2D array
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static T[,] To2D<T>(this NdArray<T> array){
			if (array.Dim != 2){
				throw new ArgumentException($"Array dimension must be 2, was {array.Dim}");
			}
			int n = array.Shape[0];
			int m = array.Shape[1];
			T[,] result = new T[n, m];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < m; j++){
					result[i, j] = array[i, j];
				}
			}
			return result;
		}

		/// <summary>
		/// Reshape array in to new shape. Won't make a copy of the data in simple cases.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="newShape"></param>
		/// <returns></returns>
		public static NdArray<T> Reshape<T>(this NdArray<T> array, params int[] newShape){
			if (array.IsDense) // no need to copy data, could be more involved
			{
				return new NdArray<T>(array.Data, newShape, offset: array.Offset);
			}
			return new NdArray<T>(array.Select(t => t).ToArray(), newShape);
		}

		/// <summary>
		/// Flatten the array. Won't make a copy of the data in simple cases.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static NdArray<T> Flatten<T>(this NdArray<T> array){
			return Reshape(array, array.Count);
		}

		/// <summary>
		/// Returns the diagonal of the provided array
		/// </summary>
		/// <param name="array"></param>
		/// <returns>diagonal array with dim - 1</returns>
		public static NdArrayDouble Diagonal(this NdArray<double> array){
			if (array.Dim < 2){
				throw new ArgumentException($"Cannot take diagonal of array with dimension {array.Dim}");
			}
			int[] reverseShape = array.Shape.Reverse().ToArray();
			int n = reverseShape.First();
			int[] shape = reverseShape.Skip(1).Reverse().ToArray();
			if (shape.Last() != n){
				throw new ArgumentException($"Cannot take diagonal of non-square {array.Shape[1]}x{n} matrix");
			}
			int[] reverseStride = array.Stride.Reverse().ToArray();
			reverseStride[1] = reverseStride[1] + reverseStride[0];
			int[] stride = reverseStride.Skip(1).Reverse().ToArray();
			return new NdArrayDouble(array.Data, shape, stride, array.Offset);
		}

		/// <summary>
		/// Converts generic NdArray to NdArrayDouble
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static NdArrayDouble AsNdArrayDouble(this NdArray<double> array){
			return new NdArrayDouble(array);
		}

		/// <summary>
		/// Removes size 1 dimensions
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static NdArray<T> Squeeze<T>(this NdArray<T> array){
			if (array.Dim == 1){
				return array;
			}
			List<int> stride = new List<int>();
			List<int> shape = new List<int>();
			for (int i = 0; i < array.Dim; i++){
				if (array.Shape[i] != 1){
					shape.Add(array.Shape[i]);
					stride.Add(array.Stride[i]);
				}
			}
			return new NdArray<T>(array.Data, shape.ToArray(), stride.ToArray(), array.Offset);
		}

		/// <summary>
		/// Transpose the array without copying data
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static NdArray<T> Transpose<T>(this NdArray<T> array){
			return new NdArray<T>(array.Data, array.Shape.Reverse().ToArray(), array.Stride.Reverse().ToArray(),
				array.Offset);
		}

		/// <summary>
		/// Transpose the array without copying data
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static NdArrayDouble Transpose(this NdArrayDouble array){
			return new NdArrayDouble(array.Data, array.Shape.Reverse().ToArray(), array.Stride.Reverse().ToArray(),
				array.Offset);
		}

		/// <summary>
		/// Extract <code>rows</code> from the array
		/// </summary>
		/// <param name="array"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		public static NdArrayDouble Loc(this NdArrayDouble array, params int[] rows){
			List<double> data = new List<double>();
			foreach (int row in rows){
				Debug.Assert(row < array.Shape[0]);
				data.AddRange(array[new Slice.Slice(row, row + 1), new Everything()].ToArray());
			}
			return new NdArrayDouble(data.ToArray(), new[]{rows.Length, array.Shape[1]});
		}
	}
}