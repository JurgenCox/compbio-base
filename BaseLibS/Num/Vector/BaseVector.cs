using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Num.Vector{
	[Serializable]
	public abstract class BaseVector : ICloneable, IDisposable, IEnumerable<double>{
		/// <summary>
		/// Determines the scalar product of this vector with another one passed as the argument. 
		/// </summary>
		public abstract double Dot(BaseVector vec);

		/// <summary>
		/// Multiplication with a scalar.
		/// </summary>
		public abstract BaseVector Mult(double d);

		/// <summary>
		/// Produces a deep copy of this vector.
		/// </summary>
		public abstract BaseVector Copy();

		/// <summary>
		/// Calculates this vector minus the other.
		/// </summary>
		public abstract BaseVector Minus(BaseVector other);

		/// <summary>
		/// Calculates this vector plus the other.
		/// </summary>
		public abstract BaseVector Plus(BaseVector other);

		/// <summary>
		/// Number of elements in this vector.
		/// </summary>
		public abstract int Length{ get; }

		/// <summary>
		/// Indexer to the elements of this vector.
		/// </summary>
		public abstract double this[int index]{ get; set; }

		/// <summary>
		/// Determines the sum of squared differences of this vector with another one passed as the argument. 
		/// </summary>
		public abstract double SumSquaredDiffs(BaseVector y1);

		/// <summary>
		/// Creates a new vector containing the elements that are indexed by the input array.
		/// </summary>
		public abstract BaseVector SubArray(IList<int> inds);

		/// <summary>
		/// True if at least one entry is NaN or Infinity.
		/// </summary>
		public abstract bool ContainsNaNOrInf();

		/// <summary>
		/// True if all entries are NaN or Infinity.
		/// </summary>
		public abstract bool IsNaNOrInf();

		/// <summary>
		/// Unpack the vector elements into a double array.
		/// </summary>
		public abstract double[] Unpack();

		/// <summary>
		/// Performs tasks associated with freeing, releasing, or resetting resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public abstract IEnumerator<double> GetEnumerator();

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

		/// <summary>
		/// Produces a deep copy of this vector.
		/// </summary>
		public object Clone(){
			return Copy();
		}

		public abstract void Read(BinaryReader reader);

		public abstract void Write(BinaryWriter writer);

		public abstract VectorType GetVectorType();

		public void Read(string path){
			BinaryReader reader = FileUtils.GetBinaryReader(path);
			Read(reader);
			reader.Close();
		}

		public void Write(string path){
			BinaryWriter writer = FileUtils.GetBinaryWriter(path);
			Write(writer);
			writer.Close();
		}

		public static BaseVector ReadbaseVector(VectorType type, BinaryReader reader){
			BaseVector result;
			switch (type){
				case VectorType.BoolArray:
					result = new BoolArrayVector();
					break;
				case VectorType.SoarseBool:
					result = new SparseBoolVector();
					break;
				case VectorType.DoubleArray:
					result = new DoubleArrayVector();
					break;
				case VectorType.SparseFloat:
					result = new SparseFloatVector();
					break;
				case VectorType.FloatArray:
					result = new FloatArrayVector();
					break;
				default:
					throw new Exception("Never get here.");
			}
			result.Read(reader);
			return result;
		}
	}
}