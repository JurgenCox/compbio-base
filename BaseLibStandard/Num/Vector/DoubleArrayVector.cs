using System;
using System.Collections.Generic;

namespace BaseLibS.Num.Vector{
	[Serializable]
	public class DoubleArrayVector : BaseVector{
		internal double[] values;

		public DoubleArrayVector(double[] values){
			this.values = values;
		}

		public override BaseVector Minus(BaseVector other){
			double[] result = (double[]) values.Clone();
			for (int i = 0; i < other.Length; i++){
				result[i] -= other[i];
			}
			return new DoubleArrayVector(result);
		}

		public override BaseVector Plus(BaseVector other){
			double[] result = (double[]) values.Clone();
			for (int i = 0; i < other.Length; i++){
				result[i] += other[i];
			}
			return new DoubleArrayVector(result);
		}

		public override int Length => values.Length;

		public override BaseVector Mult(double d){
			double[] result = (double[]) values.Clone();
			for (int i = 0; i < result.Length; i++){
				result[i] *= d;
			}
			return new DoubleArrayVector(result);
		}

		public override BaseVector Copy(){
			double[] newValues = new double[Length];
			Array.Copy(values, newValues, Length);
			return new DoubleArrayVector(newValues);
		}

		public override double this[int i]{
			get => values[i];
			set => values[i] = value;
		}

		public override double Dot(BaseVector y){
			if (y is SparseFloatVector){
				return SparseFloatVector.Dot(this, (SparseFloatVector) y);
			}
			if (y is SparseBoolVector){
				return SparseBoolVector.Dot(this, (SparseBoolVector) y);
			}
			if (y is FloatArrayVector){
				return FloatArrayVector.Dot((FloatArrayVector) y, this);
			}
			if (y is BoolArrayVector){
				return BoolArrayVector.Dot((BoolArrayVector) y, this);
			}
			return Dot(this, (DoubleArrayVector) y);
		}

		public override double SumSquaredDiffs(BaseVector y){
			if (y is SparseFloatVector){
				return SparseFloatVector.SumSquaredDiffs(this, (SparseFloatVector) y);
			}
			if (y is FloatArrayVector){
				return FloatArrayVector.SumSquaredDiffs((FloatArrayVector) y, this);
			}
			if (y is BoolArrayVector){
				return BoolArrayVector.SumSquaredDiffs((BoolArrayVector) y, this);
			}
			return SumSquaredDiffs(this, (DoubleArrayVector) y);
		}

		public override BaseVector SubArray(IList<int> inds){
			return new DoubleArrayVector(ArrayUtils.SubArray(values, inds));
		}

		public override IEnumerator<double> GetEnumerator(){
			foreach (double foo in values){
				yield return foo;
			}
		}

		internal static double Dot(DoubleArrayVector x, DoubleArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				sum += x.values[i]*y.values[i];
			}
			return sum;
		}

		internal static double SumSquaredDiffs(DoubleArrayVector x, DoubleArrayVector y){
			double sum = 0;
			for (int i = 0; i < x.Length; i++){
				double d = x.values[i] - y.values[i];
				sum += d*d;
			}
			return sum;
		}

		public override bool ContainsNaNOrInf(){
			foreach (double value in values){
				if (double.IsNaN(value) || double.IsInfinity(value)){
					return true;
				}
			}
			return false;
		}

		public override bool IsNaNOrInf(){
			foreach (double value in values){
				if (!double.IsNaN(value) && !double.IsInfinity(value)){
					return false;
				}
			}
			return true;
		}

		public override double[] Unpack(){
			return values;
		}

		public override void Dispose(){
			values = null;
		}
	}
}