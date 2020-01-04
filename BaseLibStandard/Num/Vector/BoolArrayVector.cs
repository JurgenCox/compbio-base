using System;
using System.Collections.Generic;

namespace BaseLibS.Num.Vector{
	[Serializable]
	public class BoolArrayVector : BaseVector{
		/// <summary>
		/// These boolean values are encoding 0 and 1 values.
		/// </summary>
		internal bool[] values;

		public BoolArrayVector(bool[] values){
			this.values = values;
		}

		public override BaseVector Minus(BaseVector other){
			if (other is DoubleArrayVector){
				double[] result = new double[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] -= other[i];
				}
				return new DoubleArrayVector(result);
			}
			if (other is FloatArrayVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] -= (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			if (other is BoolArrayVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] -= (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			if (other is SparseFloatVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] -= (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			if (other is SparseBoolVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] -= (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			throw new Exception("Never get here.");
		}

		public override BaseVector Plus(BaseVector other){
			if (other is DoubleArrayVector){
				double[] result = new double[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] += other[i];
				}
				return new DoubleArrayVector(result);
			}
			if (other is FloatArrayVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] += (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			if (other is BoolArrayVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] += (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			if (other is SparseFloatVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] += (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			if (other is SparseBoolVector){
				float[] result = new float[Length];
				for (int i = 0; i < Length; i++){
					if (values[i]){
						result[i] = 1;
					}
					result[i] += (float) other[i];
				}
				return new FloatArrayVector(result);
			}
			throw new Exception("Never get here.");
		}

		public override int Length => values.Length;

		public override BaseVector Mult(double d){
			float[] result = new float[Length];
			for (int i = 0; i < Length; i++){
				if (values[i]){
					result[i] = (float) d;
				}
			}
			return new FloatArrayVector(result);
		}

		public override BaseVector Copy(){
			bool[] newValues = new bool[Length];
			Array.Copy(values, newValues, Length);
			return new BoolArrayVector(newValues);
		}

		public override double this[int i]{
			get => values[i] ? 1 : 0;
			set{
				if (value != 1 && value != 0){
					throw new Exception("Illegal value.");
				}
				values[i] = value == 1;
			}
		}

		public override double Dot(BaseVector y){
			if (y is SparseFloatVector){
				return SparseFloatVector.Dot(this, (SparseFloatVector) y);
			}
			if (y is SparseBoolVector){
				return SparseBoolVector.Dot(this, (SparseBoolVector) y);
			}
			if (y is DoubleArrayVector){
				return Dot(this, (DoubleArrayVector) y);
			}
			if (y is FloatArrayVector){
				return Dot(this, (FloatArrayVector) y);
			}
			return Dot(this, (BoolArrayVector) y);
		}

		public override double SumSquaredDiffs(BaseVector y){
			if (y is SparseFloatVector){
				return SparseFloatVector.SumSquaredDiffs(this, (SparseFloatVector) y);
			}
			if (y is DoubleArrayVector){
				return SumSquaredDiffs(this, (DoubleArrayVector) y);
			}
			if (y is FloatArrayVector){
				return SumSquaredDiffs(this, (FloatArrayVector) y);
			}
			return SumSquaredDiffs(this, (BoolArrayVector) y);
		}

		public override BaseVector SubArray(IList<int> inds){
			return new BoolArrayVector(ArrayUtils.SubArray(values, inds));
		}

		public override IEnumerator<double> GetEnumerator(){
			foreach (bool foo in values){
				yield return foo ? 1 : 0;
			}
		}

		internal static double Dot(BoolArrayVector x, BoolArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				if (x.values[i] && y.values[i]){
					sum ++;
				}
			}
			return sum;
		}

		internal static double Dot(BoolArrayVector x, DoubleArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				if (x.values[i]){
					sum += y.values[i];
				}
			}
			return sum;
		}

		internal static double Dot(BoolArrayVector x, FloatArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				if (x.values[i]){
					sum += y.values[i];
				}
			}
			return sum;
		}

		internal static double SumSquaredDiffs(BoolArrayVector x, BoolArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				if (x.values[i] != y.values[i]){
					sum ++;
				}
			}
			return sum;
		}

		internal static double SumSquaredDiffs(FloatArrayVector x, DoubleArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				double d = x.values[i] - y.values[i];
				sum += d*d;
			}
			return sum;
		}

		internal static double SumSquaredDiffs(BoolArrayVector x, DoubleArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				double d = y.values[i];
				if (x.values[i]){
					d -= 1;
				}
				sum += d*d;
			}
			return sum;
		}

		internal static double SumSquaredDiffs(BoolArrayVector x, FloatArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				double d = y.values[i];
				if (x.values[i]){
					d -= 1;
				}
				sum += d*d;
			}
			return sum;
		}

		public override bool ContainsNaNOrInf(){
			return false;
		}

		public override double[] Unpack(){
			return ArrayUtils.ToDoubles(this);
		}

		public override void Dispose(){
			values = null;
		}

		public override bool IsNaNOrInf(){
			return false;
		}
	}
}