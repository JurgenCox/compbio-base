using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLibS.Num.NdArray.Slice;

namespace BaseLibS.Num.NdArray{
	[Serializable]
	public class NdArray<T> : IEnumerable<T>{
		public T[] Data;
		public int[] Shape;
		public int[] Stride;
		public int Dim => Shape.Length;
		public int Count => Shape.Aggregate(1, (res, dim) => res * dim);

		/// <summary>
		/// True if all values are stored consecutively in the <see cref="Data"/> array.
		/// </summary>
		public bool IsDense => Stride.SequenceEqual(StridesFromShape(Shape));

		public int Offset;

		public NdArray(T[] data, int offset = 0) : this(data, new[]{data.Length}, offset: offset){ }

		public NdArray(T[] data, int[] shape, int offset = 0) : this(data, shape, StridesFromShape(shape),
			offset: offset){ }

		public NdArray(T[] data, int[] shape, int[] stride, int offset = 0){
			Data = data;
			Shape = shape;
			Stride = stride;
			Offset = offset;
		}

		internal static int[] StridesFromShape(int[] shape){
			List<int> reverseStrides = new List<int>();
			int s = 1;
			foreach (int n in shape.AsEnumerable().Reverse()){
				reverseStrides.Add(s);
				s = s * n;
			}
			return reverseStrides.AsEnumerable().Reverse().ToArray();
		}

		public NdArray<T> this[params ASlice[] slices]{
			get{
				int offset = Offset;
				List<int> stride = new List<int>();
				List<int> shape = new List<int>();
				for (int i = 0; i < slices.Length; i++){
					int i1 = i;
					slices[i].Match(slice => {
						offset += slice.Start * Stride[i1];
						int relativeStride = Stride[i1] * slice.Stride;
						stride.Add(relativeStride);
						int relativeShape = (int) Math.Ceiling((slice.End - slice.Start) / (double) slice.Stride);
						shape.Add(relativeShape);
					}, () => {
						stride.Add(Stride[i1]);
						shape.Add(Shape[i1]);
					});
				}
				return new NdArray<T>(Data, shape.ToArray(), stride.ToArray(), offset);
			}
		}

		public T this[params int[] idxs] => Data[GetRawIndex(idxs)];

		public int GetRawIndex(params int[] idxs){
			int idx = Offset;
			for (int i = 0; i < idxs.Length; i++){
				idx += Stride[i] * idxs[i];
			}
			return idx;
		}


		public IEnumerator<T> GetEnumerator(){
			return new RowMajorEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

		public bool Equals(NdArray<T> other, Func<T, T, bool> equal){
			bool shapeAndStride = Shape.SequenceEqual(other.Shape) && Stride.SequenceEqual(other.Stride);
			if (!shapeAndStride){
				return false;
			}
			for (int i = 0; i < Data.Length; i++){
				if (!equal(Data[i], other.Data[i])){
					return false;
				}
			}
			return true;
		}

		public override string ToString(){
			switch (Shape.Length){
				case 1:
					return string.Join(",", this);
				case 2:
					StringBuilder builder = new StringBuilder();
					for (int i = 0; i < Shape[0]; i++){
						for (int j = 0; j < Shape[1]; j++){
							builder.Append(this[i, j]);
							builder.Append(", ");
						}
						builder.AppendLine();
					}
					return builder.ToString();
				default:
					return base.ToString();
			}
		}
	}
}