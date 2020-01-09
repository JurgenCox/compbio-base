using System;

namespace BaseLibS.Num.Space{
	public class Vector4F{
		public Vector4F(float x, float y, float z, float w){
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float W { get; set; }
		public static readonly Vector4F Zero = new Vector4F(0, 0, 0, 0);
		public static readonly Vector4F One = new Vector4F(1, 1, 1, 1);
		public Vector4F(float a) : this(a, a, a, a){}

		public Vector4F(Vector4F value){
			Set(value.X, value.Y, value.Z, value.W);
		}

		public Vector4F(Vector3F v, float w){
			X = v.X;
			Y = v.Y;
			Z = v.Z;
			W = w;
		}

		public Vector4F(){}

		public static Vector4F Clamp(Vector4F x, Vector4F min, Vector4F max){
			return new Vector4F(NumUtils.Clamp(x.X, min.X, max.X), NumUtils.Clamp(x.Y, min.Y, max.Y),
				NumUtils.Clamp(x.Z, min.Z, max.Z), NumUtils.Clamp(x.W, min.W, max.W));
		}

		public static Vector4F operator +(Vector4F left, Vector4F right){
			return new Vector4F(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}

		public static Vector4F operator *(Vector4F left, Vector4F right){
			return new Vector4F(left.X*right.X, left.Y*right.Y, left.Z*right.Z, left.W*right.W);
		}

		public static Vector4F operator *(Vector4F left, float right){
			return new Vector4F(left.X + right, left.Y + right, left.Z + right, left.W + right);
		}

		public static Vector4F operator -(Vector4F left, Vector4F right){
			return new Vector4F(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}

		public static Vector4F Abs(Vector4F v){
			return new Vector4F(Math.Abs(v.X), Math.Abs(v.Y), Math.Abs(v.Z), Math.Abs(v.W));
		}

		public static Vector4F LinearInterpolate(Vector4F a, Vector4F b, float b1){
			float a1 = 1 - b1;
			return new Vector4F(a1*a.X + b1*b.X, a1*a.Y + b1*b.Y, a1*a.Z + b1*b.Z, a1*a.W + b1*b.W);
		}

		public void Set(float x1, float y1, float z1, float w1){
			X = x1;
			Y = y1;
			Z = z1;
			W = w1;
		}

		public void Scale(float s){
			X *= s;
			Y *= s;
			Z *= s;
			W *= s;
		}

		public float Distance(Vector4F p1){
			double dx = X - p1.X;
			double dy = Y - p1.Y;
			double dz = Z - p1.Z;
			double dw = W - p1.W;
			return (float) Math.Sqrt(dx*dx + dy*dy + dz*dz + dw*dw);
		}

		public string Escape(){
			return "{" + X + " " + Y + " " + Z + " " + W + "}";
		}
	}
}