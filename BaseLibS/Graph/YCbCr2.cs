using System;
using BaseLibS.Num.Space;

namespace BaseLibS.Graph{
	public struct YCbCr2 : IEquatable<YCbCr2>{
		private const float epsilon = 0.001F;
		private readonly Vector3F backingVector;

		public YCbCr2(float y, float cb, float cr) : this(){
			backingVector = Vector3F.Clamp(new Vector3F(y, cb, cr), Vector3F.Zero, new Vector3F(255));
		}

		public float Y => backingVector.X;
		public float Cb => backingVector.Y;
		public float Cr => backingVector.Z;

		public static implicit operator YCbCr2(Color2 color){
			float r = color.R;
			float g = color.G;
			float b = color.B;
			float y = (float) (0.299*r + 0.587*g + 0.114*b);
			float cb = 128 + (float) (-0.168736*r - 0.331264*g + 0.5*b);
			float cr = 128 + (float) (0.5*r - 0.418688*g - 0.081312*b);
			return new YCbCr2(y, cb, cr);
		}

		public static bool operator ==(YCbCr2 left, YCbCr2 right){
			return left.Equals(right);
		}

		public static bool operator !=(YCbCr2 left, YCbCr2 right){
			return !left.Equals(right);
		}

		public override int GetHashCode(){
			return backingVector.GetHashCode();
		}

		public override bool Equals(object obj){
			if (obj is YCbCr2){
				return Equals((YCbCr2) obj);
			}
			return false;
		}

		public bool Equals(YCbCr2 other){
			return AlmostEquals(other, epsilon);
		}

		public bool AlmostEquals(YCbCr2 other, float precision){
			Vector3F result = Vector3F.Abs(backingVector - other.backingVector);
			return result.X < precision && result.Y < precision && result.Z < precision;
		}
	}
}