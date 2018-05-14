using System;

namespace BaseLibS.Num.Space{
	public class Vector3D{
		public double x;
		public double y;
		public double z;

		public void Set(double x1, double y1, double z1){
			x = x1;
			y = y1;
			z = z1;
		}

		public void Set(double[] t){
			x = t[0];
			y = t[1];
			z = t[2];
		}

		public void Set(Vector3D t1){
			x = t1.x;
			y = t1.y;
			z = t1.z;
		}

		public void Add(Vector3D t1, Vector3D t2){
			x = t1.x + t2.x;
			y = t1.y + t2.y;
			z = t1.z + t2.z;
		}

		public void Add(Vector3D t1){
			x += t1.x;
			y += t1.y;
			z += t1.z;
		}

		public void Subtract(Vector3D t1, Vector3D t2){
			x = t1.x - t2.x;
			y = t1.y - t2.y;
			z = t1.z - t2.z;
		}

		public void Subtract(Vector3D t1){
			x -= t1.x;
			y -= t1.y;
			z -= t1.z;
		}

		public void Scale(double s){
			x *= s;
			y *= s;
			z *= s;
		}

		public void ScaleAdd(double s, Vector3D t1, Vector3D t2){
			x = s*t1.x + t2.x;
			y = s*t1.y + t2.y;
			z = s*t1.z + t2.z;
		}

		public void Cross(Vector3D v1, Vector3D v2){
			Set(v1.y*v2.z - v1.z*v2.y, v1.z*v2.x - v1.x*v2.z, v1.x*v2.y - v1.y*v2.x);
		}

		public void Normalize(){
			double d = Norm();
			x /= d;
			y /= d;
			z /= d;
		}

		public double Dot(Vector3D v){
			return x*v.x + y*v.y + z*v.z;
		}

		public double NormSquared(){
			return x*x + y*y + z*z;
		}

		public double Norm(){
			return Math.Sqrt(NormSquared());
		}

		public override bool Equals(object t1){
			if (!(t1 is Vector3D)){
				return false;
			}
			Vector3D t2 = (Vector3D) t1;
			return (x == t2.x && y == t2.y && z == t2.z);
		}

		protected bool Equals(Vector3D other){
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
	}
}