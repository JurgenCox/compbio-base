using System;
using System.Collections.Generic;

namespace BaseLibS.Num.Vector{
	[Serializable]
	public class FloatArrayVector : BaseVector{
		internal float[] values;

		public FloatArrayVector(float[] values){
			this.values = values;
		}

		public override BaseVector Minus(BaseVector other){
			if (other is DoubleArrayVector){
				double[] result = new double[values.Length];
				for (int i = 0; i < result.Length; i++){
					result[i] = values[i] - other[i];
				}
				return new DoubleArrayVector(result);
			}
			float[] result1 = (float[]) values.Clone();
			for (int i = 0; i < other.Length; i++){
				result1[i] -= (float) other[i];
			}
			return new FloatArrayVector(result1);
		}

		public override BaseVector Plus(BaseVector other){
			if (other is DoubleArrayVector){
				double[] result = new double[values.Length];
				for (int i = 0; i < result.Length; i++){
					result[i] = values[i] + other[i];
				}
				return new DoubleArrayVector(result);
			}
			float[] result1 = (float[]) values.Clone();
			for (int i = 0; i < other.Length; i++){
				result1[i] += (float) other[i];
			}
			return new FloatArrayVector(result1);
		}

		public override int Length => values.Length;

		public override BaseVector Mult(double d){
			float[] result = (float[]) values.Clone();
			for (int i = 0; i < result.Length; i++){
				result[i] *= (float) d;
			}
			return new FloatArrayVector(result);
		}

		public override BaseVector Copy(){
			float[] newValues = new float[Length];
			Array.Copy(values, newValues, Length);
			return new FloatArrayVector(newValues);
		}

		public override double this[int i]{
			get => values[i];
			set => values[i] = (float) value;
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
			if (y is BoolArrayVector){
				return BoolArrayVector.Dot((BoolArrayVector) y, this);
			}
			return Dot(this, (FloatArrayVector) y);
		}

		public override double SumSquaredDiffs(BaseVector y){
			if (y is SparseFloatVector){
				return SparseFloatVector.SumSquaredDiffs(this, (SparseFloatVector) y);
			}
			if (y is DoubleArrayVector){
				return SumSquaredDiffs(this, (DoubleArrayVector) y);
			}
			if (y is BoolArrayVector){
				return BoolArrayVector.SumSquaredDiffs((BoolArrayVector) y, this);
			}
			return SumSquaredDiffs(this, (FloatArrayVector) y);
		}

		public override BaseVector SubArray(IList<int> inds){
			return new FloatArrayVector(ArrayUtils.SubArray(values, inds));
		}

		public override IEnumerator<double> GetEnumerator(){
			foreach (float foo in values){
				yield return foo;
			}
		}

		internal static double Dot(FloatArrayVector x, FloatArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				sum += x.values[i]*y.values[i];
			}
			return sum;
		}

		internal static double Dot(FloatArrayVector x, DoubleArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				sum += x.values[i]*y.values[i];
			}
			return sum;
		}

		internal static double SumSquaredDiffs(FloatArrayVector x, FloatArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				double d = x.values[i] - y.values[i];
				sum += d*d;
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

		public override bool ContainsNaNOrInf(){
			foreach (float value in values){
				if (float.IsNaN(value) || float.IsInfinity(value)){
					return true;
				}
			}
			return false;
		}

		public override double[] Unpack(){
			return ArrayUtils.ToDoubles(values);
		}

		public override void Dispose(){
			values = null;
		}

		public override bool IsNaNOrInf(){
			foreach (float value in values){
				if (!float.IsNaN(value) && !float.IsInfinity(value)){
					return false;
				}
			}
			return true;
		}
	}
}