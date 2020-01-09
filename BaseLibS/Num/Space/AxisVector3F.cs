using System;

namespace BaseLibS.Num.Space{
	public class AxisVector3F{
		public float x;
		public float y;
		public float z;
		public float angle;

		public AxisVector3F(){
			z = 1.0f;
		}

		public AxisVector3F(float x, float y, float z, float angle){
			Set(x, y, z, angle);
		}

		public AxisVector3F(AxisVector3F a1){
			Set(a1.x, a1.y, a1.z, a1.angle);
		}

		public AxisVector3F(Vector3F axis, float angle){
			Set(axis, angle);
		}

		public void Set(Vector3F axis, float angle1){
			x = axis.x;
			y = axis.y;
			z = axis.z;
			angle = angle1;
		}

		public void Set(float x1, float y1, float z1, float angle1){
			x = x1;
			y = y1;
			z = z1;
			angle = angle1;
		}

		public void Set(AxisVector3F a){
			x = a.x;
			y = a.y;
			z = a.z;
			angle = a.angle;
		}

		public void Set(Matrix3X3F m1){
			SetFromMat(m1.m00, m1.m01, m1.m02, m1.m10, m1.m11, m1.m12, m1.m20, m1.m21, m1.m22);
		}

		private void SetFromMat(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21,
			double m22){
			double cos = (m00 + m11 + m22 - 1.0)*0.5;
			x = (float) (m21 - m12);
			y = (float) (m02 - m20);
			z = (float) (m10 - m01);
			double sin = 0.5*Math.Sqrt(x*x + y*y + z*z);
			if (sin == 0 && cos == 1){
				x = y = 0;
				z = 1;
				angle = 0;
			} else{
				angle = (float) Math.Atan2(sin, cos);
			}
		}

		public override bool Equals(object o){
			if (!(o is AxisVector3F)){
				return false;
			}
			AxisVector3F a1 = (AxisVector3F) o;
			return x == a1.x && y == a1.y && z == a1.z && angle == a1.angle;
		}

		protected bool Equals(AxisVector3F other){
			return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && angle.Equals(other.angle);
		}

		public override int GetHashCode(){
			unchecked{
				int hashCode = x.GetHashCode();
				hashCode = (hashCode*397) ^ y.GetHashCode();
				hashCode = (hashCode*397) ^ z.GetHashCode();
				hashCode = (hashCode*397) ^ angle.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString(){
			return "(" + x + ", " + y + ", " + z + ", " + angle + ")";
		}
	}
}