using System;

namespace BaseLibS.Num.Space{
	public class Matrix3X3F{
		public float m00;
		public float m01;
		public float m02;
		public float m10;
		public float m11;
		public float m12;
		public float m20;
		public float m21;
		public float m22;
		public Matrix3X3F(){}

		public Matrix3X3F(float[] v){
			Set(v);
		}

		public Matrix3X3F(Matrix3X3F m1){
			if (m1 == null){
				SetScale(1);
				return;
			}
			m00 = m1.m00;
			m01 = m1.m01;
			m02 = m1.m02;
			m10 = m1.m10;
			m11 = m1.m11;
			m12 = m1.m12;
			m20 = m1.m20;
			m21 = m1.m21;
			m22 = m1.m22;
		}

		public void SetScale(float scale){
			ClearImpl();
			m00 = m11 = m22 = scale;
		}

		public void Set(Matrix3X3F m1){
			SetImpl(m1);
		}

		public void Set(float[] m){
			m00 = m[0];
			m01 = m[1];
			m02 = m[2];
			m10 = m[3];
			m11 = m[4];
			m12 = m[5];
			m20 = m[6];
			m21 = m[7];
			m22 = m[8];
		}

		public void SetElement(int row, int col, float v){
			switch (row){
				case 0:
					switch (col){
						case 0:
							m00 = v;
							return;
						case 1:
							m01 = v;
							return;
						case 2:
							m02 = v;
							return;
					}
					break;
				case 1:
					switch (col){
						case 0:
							m10 = v;
							return;
						case 1:
							m11 = v;
							return;
						case 2:
							m12 = v;
							return;
					}
					break;
				case 2:
					switch (col){
						case 0:
							m20 = v;
							return;
						case 1:
							m21 = v;
							return;
						case 2:
							m22 = v;
							return;
					}
					break;
			}
			Err();
		}

		public float GetElement(int row, int col){
			switch (row){
				case 0:
					switch (col){
						case 0:
							return m00;
						case 1:
							return m01;
						case 2:
							return m02;
					}
					break;
				case 1:
					switch (col){
						case 0:
							return m10;
						case 1:
							return m11;
						case 2:
							return m12;
					}
					break;
				case 2:
					switch (col){
						case 0:
							return m20;
						case 1:
							return m21;
						case 2:
							return m22;
					}
					break;
			}
			Err();
			return 0;
		}

		public void SetRow(int row, float x, float y, float z){
			switch (row){
				case 0:
					m00 = x;
					m01 = y;
					m02 = z;
					return;
				case 1:
					m10 = x;
					m11 = y;
					m12 = z;
					return;
				case 2:
					m20 = x;
					m21 = y;
					m22 = z;
					return;
				default:
					Err();
					break;
			}
		}

		public void SetRow(int row, Vector3F v){
			switch (row){
				case 0:
					m00 = v.x;
					m01 = v.y;
					m02 = v.z;
					return;
				case 1:
					m10 = v.x;
					m11 = v.y;
					m12 = v.z;
					return;
				case 2:
					m20 = v.x;
					m21 = v.y;
					m22 = v.z;
					return;
				default:
					Err();
					break;
			}
		}

		public void SetRow(int row, float[] v){
			switch (row){
				case 0:
					m00 = v[0];
					m01 = v[1];
					m02 = v[2];
					return;
				case 1:
					m10 = v[0];
					m11 = v[1];
					m12 = v[2];
					return;
				case 2:
					m20 = v[0];
					m21 = v[1];
					m22 = v[2];
					return;
				default:
					Err();
					break;
			}
		}

		public void GetRow(int row, float[] v){
			switch (row){
				case 0:
					v[0] = m00;
					v[1] = m01;
					v[2] = m02;
					return;
				case 1:
					v[0] = m10;
					v[1] = m11;
					v[2] = m12;
					return;
				case 2:
					v[0] = m20;
					v[1] = m21;
					v[2] = m22;
					return;
			}
			Err();
		}

		public void SetColumn(int column, float x, float y, float z){
			switch (column){
				case 0:
					m00 = x;
					m10 = y;
					m20 = z;
					break;
				case 1:
					m01 = x;
					m11 = y;
					m21 = z;
					break;
				case 2:
					m02 = x;
					m12 = y;
					m22 = z;
					break;
				default:
					Err();
					break;
			}
		}

		public void SetColumn(int column, Vector3F v){
			switch (column){
				case 0:
					m00 = v.x;
					m10 = v.y;
					m20 = v.z;
					break;
				case 1:
					m01 = v.x;
					m11 = v.y;
					m21 = v.z;
					break;
				case 2:
					m02 = v.x;
					m12 = v.y;
					m22 = v.z;
					break;
				default:
					Err();
					break;
			}
		}

		public void GetColumn(int column, Vector3F v){
			switch (column){
				case 0:
					v.x = m00;
					v.y = m10;
					v.z = m20;
					break;
				case 1:
					v.x = m01;
					v.y = m11;
					v.z = m21;
					break;
				case 2:
					v.x = m02;
					v.y = m12;
					v.z = m22;
					break;
				default:
					Err();
					break;
			}
		}

		public void SetColumn(int column, float[] v){
			switch (column){
				case 0:
					m00 = v[0];
					m10 = v[1];
					m20 = v[2];
					break;
				case 1:
					m01 = v[0];
					m11 = v[1];
					m21 = v[2];
					break;
				case 2:
					m02 = v[0];
					m12 = v[1];
					m22 = v[2];
					break;
				default:
					Err();
					break;
			}
		}

		public void GetColumn(int column, float[] v){
			switch (column){
				case 0:
					v[0] = m00;
					v[1] = m10;
					v[2] = m20;
					break;
				case 1:
					v[0] = m01;
					v[1] = m11;
					v[2] = m21;
					break;
				case 2:
					v[0] = m02;
					v[1] = m12;
					v[2] = m22;
					break;
				default:
					Err();
					break;
			}
		}

		public void Add(Matrix3X3F m1){
			m00 += m1.m00;
			m01 += m1.m01;
			m02 += m1.m02;
			m10 += m1.m10;
			m11 += m1.m11;
			m12 += m1.m12;
			m20 += m1.m20;
			m21 += m1.m21;
			m22 += m1.m22;
		}

		public void Subtract(Matrix3X3F m1){
			m00 -= m1.m00;
			m01 -= m1.m01;
			m02 -= m1.m02;
			m10 -= m1.m10;
			m11 -= m1.m11;
			m12 -= m1.m12;
			m20 -= m1.m20;
			m21 -= m1.m21;
			m22 -= m1.m22;
		}

		public void Transpose(){
			TransposeImpl();
		}

		public void Transpose(Matrix3X3F m1){
			SetImpl(m1);
			TransposeImpl();
		}

		public void Invert(Matrix3X3F m1){
			SetImpl(m1);
			Invert();
		}

		public void Invert(){
			double s = Determinant();
			if (s == 0.0){
				return;
			}
			s = 1/s;
			Set(m11*m22 - m12*m21, m02*m21 - m01*m22, m01*m12 - m02*m11, m12*m20 - m10*m22, m00*m22 - m02*m20, m02*m10 - m00*m12,
				m10*m21 - m11*m20, m01*m20 - m00*m21, m00*m11 - m01*m10);
			Scale((float) s);
		}

		public Matrix3X3F SetAsXRotation(float angle){
			double c = Math.Cos(angle);
			double s = Math.Sin(angle);
			m00 = 1.0f;
			m01 = 0.0f;
			m02 = 0.0f;
			m10 = 0.0f;
			m11 = (float) c;
			m12 = (float) -s;
			m20 = 0.0f;
			m21 = (float) s;
			m22 = (float) c;
			return this;
		}

		public Matrix3X3F SetAsYRotation(float angle){
			double c = Math.Cos(angle);
			double s = Math.Sin(angle);
			m00 = (float) c;
			m01 = 0.0f;
			m02 = (float) s;
			m10 = 0.0f;
			m11 = 1.0f;
			m12 = 0.0f;
			m20 = (float) -s;
			m21 = 0.0f;
			m22 = (float) c;
			return this;
		}

		public Matrix3X3F SetAsZRotation(float angle){
			double c = Math.Cos(angle);
			double s = Math.Sin(angle);
			m00 = (float) c;
			m01 = (float) -s;
			m02 = 0.0f;
			m10 = (float) s;
			m11 = (float) c;
			m12 = 0.0f;
			m20 = 0.0f;
			m21 = 0.0f;
			m22 = 1.0f;
			return this;
		}

		public void Scale(float x){
			m00 *= x;
			m01 *= x;
			m02 *= x;
			m10 *= x;
			m11 *= x;
			m12 *= x;
			m20 *= x;
			m21 *= x;
			m22 *= x;
		}

		public void Multiply(Matrix3X3F m1){
			Multiply(this, m1);
		}

		public void Multiply(Matrix3X3F m1, Matrix3X3F m2){
			Set(m1.m00*m2.m00 + m1.m01*m2.m10 + m1.m02*m2.m20, m1.m00*m2.m01 + m1.m01*m2.m11 + m1.m02*m2.m21,
				m1.m00*m2.m02 + m1.m01*m2.m12 + m1.m02*m2.m22, m1.m10*m2.m00 + m1.m11*m2.m10 + m1.m12*m2.m20,
				m1.m10*m2.m01 + m1.m11*m2.m11 + m1.m12*m2.m21, m1.m10*m2.m02 + m1.m11*m2.m12 + m1.m12*m2.m22,
				m1.m20*m2.m00 + m1.m21*m2.m10 + m1.m22*m2.m20, m1.m20*m2.m01 + m1.m21*m2.m11 + m1.m22*m2.m21,
				m1.m20*m2.m02 + m1.m21*m2.m12 + m1.m22*m2.m22);
		}

		public override bool Equals(object o){
			if (!(o is Matrix3X3F)){
				return false;
			}
			Matrix3X3F m = (Matrix3X3F) o;
			return m00 == m.m00 && m01 == m.m01 && m02 == m.m02 && m10 == m.m10 && m11 == m.m11 && m12 == m.m12 && m20 == m.m20 &&
					m21 == m.m21 && m22 == m.m22;
		}

		public override int GetHashCode(){
			unchecked{
				int hashCode = m00.GetHashCode();
				hashCode = (hashCode*397) ^ m01.GetHashCode();
				hashCode = (hashCode*397) ^ m02.GetHashCode();
				hashCode = (hashCode*397) ^ m10.GetHashCode();
				hashCode = (hashCode*397) ^ m11.GetHashCode();
				hashCode = (hashCode*397) ^ m12.GetHashCode();
				hashCode = (hashCode*397) ^ m20.GetHashCode();
				hashCode = (hashCode*397) ^ m21.GetHashCode();
				hashCode = (hashCode*397) ^ m22.GetHashCode();
				return hashCode;
			}
		}

		public void SetZero(){
			ClearImpl();
		}

		public void Set(float ma00, float ma01, float ma02, float ma10, float ma11, float ma12, float ma20, float ma21,
			float ma22){
			m00 = ma00;
			m01 = ma01;
			m02 = ma02;
			m10 = ma10;
			m11 = ma11;
			m12 = ma12;
			m20 = ma20;
			m21 = ma21;
			m22 = ma22;
		}

		public override string ToString(){
			return "[\n  [" + m00 + "\t" + m01 + "\t" + m02 + "]" + "\n  [" + m10 + "\t" + m11 + "\t" + m12 + "]" + "\n  [" + m20 +
					"\t" + m21 + "\t" + m22 + "] ]";
		}

		public Matrix3X3F Set(AxisVector3F a){
			double x = a.x;
			double y = a.y;
			double z = a.z;
			double angle = a.angle;
			// Taken from Rick's which is taken from Wertz. pg. 412
			// Bug Fixed and changed into right-handed by hiranabe
			double n = Math.Sqrt(x*x + y*y + z*z);
			// zero-div may occur
			n = 1/n;
			x *= n;
			y *= n;
			z *= n;
			double c = Math.Cos(angle);
			double s = Math.Sin(angle);
			double omc = 1.0 - c;
			m00 = (float) (c + x*x*omc);
			m11 = (float) (c + y*y*omc);
			m22 = (float) (c + z*z*omc);
			double tmp1 = x*y*omc;
			double tmp2 = z*s;
			m01 = (float) (tmp1 - tmp2);
			m10 = (float) (tmp1 + tmp2);
			tmp1 = x*z*omc;
			tmp2 = y*s;
			m02 = (float) (tmp1 + tmp2);
			m20 = (float) (tmp1 - tmp2);
			tmp1 = y*z*omc;
			tmp2 = x*s;
			m12 = (float) (tmp1 - tmp2);
			m21 = (float) (tmp1 + tmp2);
			return this;
		}

		public bool SetAsBallRotation(float responseFactor, float dx, float dy){
			float r = (float) Math.Sqrt(dx*dx + dy*dy);
			float th = r*responseFactor;
			if (th == 0){
				SetScale(1);
				return false;
			}
			float c = (float) Math.Cos(th);
			float s = (float) Math.Sin(th);
			float nx = -dy/r;
			float ny = dx/r;
			float c1 = c - 1;
			m00 = 1 + c1*nx*nx;
			m01 = m10 = c1*nx*ny;
			m20 = -(m02 = s*nx);
			m11 = 1 + c1*ny*ny;
			m21 = -(m12 = s*ny);
			m22 = c;
			return true;
		}

		public bool IsRotation(){
			return (Math.Abs(Determinant() - 1) < 0.001f);
		}

		public void Rotate(Vector3F t){
			Rotate(t, t);
		}

		public void Rotate(Vector3F t, Vector3F result){
			result.Set(m00*t.x + m01*t.y + m02*t.z, m10*t.x + m11*t.y + m12*t.z, m20*t.x + m21*t.y + m22*t.z);
		}

		public float Determinant(){
			return m00*(m11*m22 - m21*m12) - m01*(m10*m22 - m20*m12) + m02*(m10*m21 - m20*m11);
		}

		private void SetImpl(Matrix3X3F m1){
			m00 = m1.m00;
			m01 = m1.m01;
			m02 = m1.m02;
			m10 = m1.m10;
			m11 = m1.m11;
			m12 = m1.m12;
			m20 = m1.m20;
			m21 = m1.m21;
			m22 = m1.m22;
		}

		private void ClearImpl(){
			m00 = m01 = m02 = m10 = m11 = m12 = m20 = m21 = m22 = 0.0f;
		}

		private void TransposeImpl(){
			float tmp = m01;
			m01 = m10;
			m10 = tmp;
			tmp = m02;
			m02 = m20;
			m20 = tmp;
			tmp = m12;
			m12 = m21;
			m21 = tmp;
		}

		private static void Err(){
			throw new IndexOutOfRangeException("matrix column/row out of bounds");
		}
	}
}