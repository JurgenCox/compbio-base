using System;

namespace BaseLibS.Num.Space{
	public class Vector3F{
		public static readonly Vector3F Zero = new Vector3F(0, 0, 0);
		public static readonly Vector3F One = new Vector3F(1, 1, 1);
		public float x;
		public float y;
		public float z;
		public Vector3F(){}

		public Vector3F(float x, float y, float z){
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3F(Vector3F vec){
			x = vec.x;
			y = vec.y;
			z = vec.z;
		}

		public float X{
			get => x;
			set => x = value;
		}

		public float Y{
			get => y;
			set => y = value;
		}

		public float Z{
			get => z;
			set => z = value;
		}

		public Vector3F(float a) : this(a, a, a){}

		public static Vector3F Clamp(Vector3F x, Vector3F min, Vector3F max){
			return new Vector3F(NumUtils.Clamp(x.x, min.x, max.x), NumUtils.Clamp(x.y, min.y, max.y),
				NumUtils.Clamp(x.z, min.z, max.z));
		}

		public static Vector3F Abs(Vector3F v){
			return new Vector3F(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
		}

		public static Vector3F operator +(Vector3F left, Vector3F right){
			return new Vector3F(left.x + right.x, left.y + right.y, left.z + right.z);
		}

		public static Vector3F operator -(Vector3F left, Vector3F right){
			return new Vector3F(left.x - right.x, left.y - right.y, left.z - right.z);
		}

		public static Vector3F operator *(Vector3F left, float right){
			return new Vector3F(left.x + right, left.y + right, left.z + right);
		}

		public void Set(float x1, float y1, float z1){
			x = x1;
			y = y1;
			z = z1;
		}

		public void Set(float[] t){
			x = t[0];
			y = t[1];
			z = t[2];
		}

		public void Set(Vector3F t1){
			x = t1.x;
			y = t1.y;
			z = t1.z;
		}

		public void Add(Vector3F t1, Vector3F t2){
			x = t1.x + t2.x;
			y = t1.y + t2.y;
			z = t1.z + t2.z;
		}

		public void Add(Vector3F t1){
			x += t1.x;
			y += t1.y;
			z += t1.z;
		}

		public float DistanceSquared(Vector3F p1){
			double dx = x - p1.x;
			double dy = y - p1.y;
			double dz = z - p1.z;
			return (float) (dx*dx + dy*dy + dz*dz);
		}

		public float Distance(Vector3F p1){
			return (float) Math.Sqrt(DistanceSquared(p1));
		}

		public void Subtract(Vector3F t1, Vector3F t2){
			x = t1.x - t2.x;
			y = t1.y - t2.y;
			z = t1.z - t2.z;
		}

		public static Vector3F SubtractNew(Vector3F t1, Vector3F t2){
			return new Vector3F(t1.x - t2.x, t1.y - t2.y, t1.z - t2.z);
		}

		public void Subtract(Vector3F t1){
			x -= t1.x;
			y -= t1.y;
			z -= t1.z;
		}

		public void Scale(float s){
			x *= s;
			y *= s;
			z *= s;
		}

		public void Add(float a, float b, float c){
			x += a;
			y += b;
			z += c;
		}

		public void Scale(Vector3F p){
			x *= p.x;
			y *= p.y;
			z *= p.z;
		}

		public void ScaleAdd(float s, Vector3F t1, Vector3F t2){
			x = s*t1.x + t2.x;
			y = s*t1.y + t2.y;
			z = s*t1.z + t2.z;
		}

		public void Average(Vector3F a, Vector3F b){
			x = (a.x + b.x)/2f;
			y = (a.y + b.y)/2f;
			z = (a.z + b.z)/2f;
		}

		public float Dot(Vector3F v){
			return x*v.x + y*v.y + z*v.z;
		}

		public float NormSquared(){
			return x*x + y*y + z*z;
		}

		public float Norm(){
			return (float) Math.Sqrt(NormSquared());
		}

		public void Normalize(){
			double d = Norm();
			x = (float) (x/d);
			y = (float) (y/d);
			z = (float) (z/d);
		}

		public void Cross(Vector3F v1, Vector3F v2){
			Set(v1.y*v2.z - v1.z*v2.y, v1.z*v2.x - v1.x*v2.z, v1.x*v2.y - v1.y*v2.x);
		}

		public override bool Equals(object t1){
			if (!(t1 is Vector3F)){
				return false;
			}
			Vector3F t2 = (Vector3F) t1;
			return x == t2.x && y == t2.y && z == t2.z;
		}

		protected bool Equals(Vector3F other){
			return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
		}

		public override int GetHashCode(){
			unchecked{
				int hashCode = x.GetHashCode();
				hashCode = (hashCode*397) ^ y.GetHashCode();
				hashCode = (hashCode*397) ^ z.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString(){
			return "{" + x + ", " + y + ", " + z + "}";
		}

		public float Angle(Vector3F v1){
			double xx = y*v1.z - z*v1.y;
			double yy = z*v1.x - x*v1.z;
			double zz = x*v1.y - y*v1.x;
			double cross = Math.Sqrt(xx*xx + yy*yy + zz*zz);
			return (float) Math.Abs(Math.Atan2(cross, Dot(v1)));
		}

		public string Escape(){
			return "{" + x + " " + y + " " + z + "}";
		}
	}
}