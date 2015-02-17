﻿using System;
using System.Collections.Generic;
using BaseLibS.Api;

namespace BaseLibS.Num.Vector{
	[Serializable]
	public class SparseBoolVector : BaseVector{
		/// <summary>
		/// Indices of nonzero elements. Indices are sorted.
		/// </summary>
		private readonly int[] indices;

		/// <summary>
		/// Total length of the vector.
		/// </summary>
		private readonly int length;

		public SparseBoolVector(IList<bool> values){
			List<int> newIndices = new List<int>();
			for (int i = 0; i < values.Count; i++){
				if (values[i]){
					newIndices.Add(i);
				}
			}
			indices = newIndices.ToArray();
			length = values.Count;
		}

		public SparseBoolVector(int[] indices, int length){
			this.indices = indices;
			this.length = length;
		}

		public override int Length { get { return length; } }

		public override BaseVector Copy(){
			int[] newIndices = new int[indices.Length];
			Array.Copy(indices, newIndices, indices.Length);
			return new SparseBoolVector(newIndices, length);
		}

		public override BaseVector SubArray(IList<int> inds){
			List<int> newIndices = new List<int>();
			for (int i = 0; i < inds.Count; i++){
				int x = Array.BinarySearch(indices, inds[i]);
				if (x >= 0){
					newIndices.Add(i);
				}
			}
			return new SparseBoolVector(newIndices.ToArray(), inds.Count);
		}

		public override IEnumerator<double> GetEnumerator() { throw new NotImplementedException(); }
		public override bool ContainsNaNOrInfinity() { return false; }

		public override double this[int i]{
			get{
				int ind = Array.BinarySearch(indices, i);
				return ind < 0 ? 0 : 1;
			}
		}

		public override double Dot(BaseVector y){
			if (y is SparseBoolVector){
				return Dot((SparseBoolVector) y, this);
			}
			if (y is FloatArrayVector){
				return Dot((FloatArrayVector) y, this);
			}
			if (y is DoubleArrayVector){
				return Dot((DoubleArrayVector) y, this);
			}
			if (y is BoolArrayVector){
				return Dot((BoolArrayVector) y, this);
			}
			return Dot(this, (SparseFloatVector) y);
		}

		public override double SumSquaredDiffs(BaseVector y){
			if (y is SparseBoolVector){
				return SumSquaredDiffs((SparseBoolVector) y, this);
			}
			if (y is FloatArrayVector){
				return SumSquaredDiffs((FloatArrayVector) y, this);
			}
			if (y is DoubleArrayVector){
				return SumSquaredDiffs((DoubleArrayVector) y, this);
			}
			if (y is BoolArrayVector){
				return SumSquaredDiffs((BoolArrayVector) y, this);
			}
			return SumSquaredDiffs(this, (SparseFloatVector) y);
		}

		internal static double Dot(SparseBoolVector x, SparseFloatVector y){
			double sum = 0;
			int xlen = x.indices.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (x.indices[i] == y.indices[j]){
					sum += y.values[j++];
					i++;
				} else{
					if (x.indices[i] > y.indices[j]){
						++j;
					} else{
						++i;
					}
				}
			}
			return sum;
		}

		internal static double Dot(SparseBoolVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.indices.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (x.indices[i] == y.indices[j]){
					i++;
					j++;
					sum += 1;
				} else{
					if (x.indices[i] > y.indices[j]){
						++j;
					} else{
						++i;
					}
				}
			}
			return sum;
		}

		internal static double Dot(FloatArrayVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (i == y.indices[j]){
					j++;
					sum += x.values[i++];
				} else{
					if (i > y.indices[j]){
						++j;
					} else{
						++i;
					}
				}
			}
			return sum;
		}

		internal static double Dot(BoolArrayVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (i == y.indices[j]){
					if (x.values[i]){
						sum++;
					}
					i++;
					j++;
				} else{
					if (i > y.indices[j]){
						++j;
					} else{
						++i;
					}
				}
			}
			return sum;
		}

		internal static double Dot(DoubleArrayVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (i == y.indices[j]){
					j++;
					sum += x.values[i++];
				} else{
					if (i > y.indices[j]){
						++j;
					} else{
						++i;
					}
				}
			}
			return sum;
		}

		internal static double SumSquaredDiffs(SparseBoolVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.indices.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (x.indices[i] == y.indices[j]){
					i++;
					j++;
				} else if (x.indices[i] > y.indices[j]){
					sum++;
					j++;
				} else{
					sum++;
					i++;
				}
			}
			while (i < xlen){
				sum ++;
				i++;
			}
			while (j < ylen){
				sum++;
				j++;
			}
			return sum;
		}

		internal static double SumSquaredDiffs(FloatArrayVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (i == y.indices[j]){
					double d = x.values[i++] - 1;
					j++;
					sum += d*d;
				} else if (i > y.indices[j]){
					sum += 1;
					++j;
				} else{
					sum += x.values[i]*x.values[i];
					++i;
				}
			}
			while (i < xlen){
				sum += x.values[i]*x.values[i];
				++i;
			}
			while (j < ylen){
				sum += 1;
				++j;
			}
			return sum;
		}

		internal static double SumSquaredDiffs(DoubleArrayVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (i == y.indices[j]){
					double d = x.values[i++] - 1;
					j++;
					sum += d*d;
				} else if (i > y.indices[j]){
					sum += 1;
					++j;
				} else{
					sum += x.values[i]*x.values[i];
					++i;
				}
			}
			while (i < xlen){
				sum += x.values[i]*x.values[i];
				++i;
			}
			while (j < ylen){
				sum += 1;
				++j;
			}
			return sum;
		}

		internal static double SumSquaredDiffs(BoolArrayVector x, SparseBoolVector y){
			double sum = 0;
			int xlen = x.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (i == y.indices[j]){
					double d = 1;
					j++;
					if (x.values[i++]){
						d -= 1;
					}
					sum += d*d;
				} else if (i > y.indices[j]){
					sum += 1;
					++j;
				} else{
					if (x.values[i]){
						sum += 1;
					}
					++i;
				}
			}
			while (i < xlen){
				if (x.values[i]){
					sum += 1;
				}
				++i;
			}
			while (j < ylen){
				sum += 1;
				++j;
			}
			return sum;
		}

		internal static double SumSquaredDiffs(SparseBoolVector x, SparseFloatVector y){
			double sum = 0;
			int xlen = x.indices.Length;
			int ylen = y.indices.Length;
			int i = 0;
			int j = 0;
			while (i < xlen && j < ylen){
				if (x.indices[i] == y.indices[j]){
					double d = 1 - y.values[j++];
					i++;
					sum += d*d;
				} else if (x.indices[i] > y.indices[j]){
					sum += y.values[j]*y.values[j];
					++j;
				} else{
					sum += 1;
					++i;
				}
			}
			while (i < xlen){
				sum += 1;
				++i;
			}
			while (j < ylen){
				sum += y.values[j]*y.values[j];
				++j;
			}
			return sum;
		}
	}
}