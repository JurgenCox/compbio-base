using System;

namespace BaseLibS.Num.Space{
	public class Quat{
		public float q0, q1, q2, q3;
		private Matrix3X3F mat;
		private static readonly Vector4F qZero = new Vector4F();
		public const double radPerDeg = Math.PI/180;

		public Quat(){
			q0 = 1;
		}

		public Quat(Quat q){
			Set(q);
		}

		public Quat(Vector3F v, float theta){
			Set(v, theta);
		}

		public Quat(Matrix3X3F mat){
			Set(new Matrix3X3F(mat));
		}

		public Quat(AxisVector3F a){
			Set(a);
		}

		public Quat(Vector4F pt){
			Set(pt);
		}

		public Quat(float qx1, float qx2, float qx3, float qx0){
			q0 = qx0;
			q1 = qx1;
			q2 = qx2;
			q3 = qx3;
		}

		public void Set(Quat q){
			q0 = q.q0;
			q1 = q.q1;
			q2 = q.q2;
			q3 = q.q3;
		}

		private void Set(Vector4F pt){
			float factor = pt?.Distance(qZero) ?? 0;
			if (factor == 0){
				q0 = 1;
				return;
			}
			q0 = pt.W/factor;
			q1 = pt.X/factor;
			q2 = pt.Y/factor;
			q3 = pt.Z/factor;
		}

		public void Set(Vector3F pt, float theta){
			if (pt.x == 0 && pt.y == 0 && pt.z == 0){
				q0 = 1;
				return;
			}
			double fact = Math.Sin(theta/2*radPerDeg)/Math.Sqrt(pt.x*pt.x + pt.y*pt.y + pt.z*pt.z);
			q0 = (float) Math.Cos(theta/2*radPerDeg);
			q1 = (float) (pt.x*fact);
			q2 = (float) (pt.y*fact);
			q3 = (float) (pt.z*fact);
		}

		public void Set(AxisVector3F a){
			AxisVector3F aa = new AxisVector3F(a);
			if (aa.angle == 0){
				aa.y = 1;
			}
			Set(new Matrix3X3F().Set(aa));
		}

		private void Set(Matrix3X3F mat1){
			mat = mat1;
			double trace = mat1.m00 + mat1.m11 + mat1.m22;
			double temp;
			double w, x, y, z;
			if (trace >= 0.5){
				w = Math.Sqrt(1.0 + trace);
				x = (mat1.m21 - mat1.m12)/w;
				y = (mat1.m02 - mat1.m20)/w;
				z = (mat1.m10 - mat1.m01)/w;
			} else if ((temp = mat1.m00 + mat1.m00 - trace) >= 0.5){
				x = Math.Sqrt(1.0 + temp);
				w = (mat1.m21 - mat1.m12)/x;
				y = (mat1.m10 + mat1.m01)/x;
				z = (mat1.m20 + mat1.m02)/x;
			} else if ((temp = mat1.m11 + mat1.m11 - trace) >= 0.5 || mat1.m11 > mat1.m22){
				y = Math.Sqrt(1.0 + temp);
				w = (mat1.m02 - mat1.m20)/y;
				x = (mat1.m10 + mat1.m01)/y;
				z = (mat1.m21 + mat1.m12)/y;
			} else{
				z = Math.Sqrt(1.0 + mat1.m22 + mat1.m22 - trace);
				w = (mat1.m10 - mat1.m01)/z;
				x = (mat1.m20 + mat1.m02)/z; // was -
				y = (mat1.m21 + mat1.m12)/z; // was -
			}
			q0 = (float) (w*0.5);
			q1 = (float) (x*0.5);
			q2 = (float) (y*0.5);
			q3 = (float) (z*0.5);
		}

		public void SetRef(Quat qref){
			if (qref == null){
				Multiply(GetFixFactor());
				return;
			}
			if (Dot(qref) >= 0){
				return;
			}
			q0 *= -1;
			q1 *= -1;
			q2 *= -1;
			q3 *= -1;
		}

		public static Quat GetQuaternionFrame(Vector3F center, Vector3F x, Vector3F xy){
			Vector3F vA = new Vector3F(x);
			Vector3F vB = new Vector3F(xy);
			if (center != null){
				vA.Subtract(center);
				vB.Subtract(center);
			}
			return GetQuaternionFrameV(vA, vB, null, false);
		}

		public static Quat GetQuaternionFrameV(Vector3F vA, Vector3F vB, Vector3F vC, bool yBased){
			if (vC == null){
				vC = new Vector3F();
				vC.Cross(vA, vB);
				if (yBased){
					vA.Cross(vB, vC);
				}
			}
			Vector3F vBprime = new Vector3F();
			vBprime.Cross(vC, vA);
			vA.Normalize();
			vBprime.Normalize();
			vC.Normalize();
			Matrix3X3F mat = new Matrix3X3F();
			mat.SetColumn(0, vA);
			mat.SetColumn(1, vBprime);
			mat.SetColumn(2, vC);
			Quat q = new Quat(mat);
			return q;
		}

		public Matrix3X3F GetMatrix(){
			if (mat == null){
				SetMatrix();
			}
			return mat;
		}

		private void SetMatrix(){
			mat = new Matrix3X3F{
				m00 = q0*q0 + q1*q1 - q2*q2 - q3*q3,
				m01 = 2*q1*q2 - 2*q0*q3,
				m02 = 2*q1*q3 + 2*q0*q2,
				m10 = 2*q1*q2 + 2*q0*q3,
				m11 = q0*q0 - q1*q1 + q2*q2 - q3*q3,
				m12 = 2*q2*q3 - 2*q0*q1,
				m20 = 2*q1*q3 - 2*q0*q2,
				m21 = 2*q2*q3 + 2*q0*q1,
				m22 = q0*q0 - q1*q1 - q2*q2 + q3*q3
			};
		}

		public Quat Add(float x){
			return new Quat(GetNormal(), GetTheta() + x);
		}

		public Quat Multiply(float x){
			return x == 1 ? new Quat(q1, q2, q3, q0) : new Quat(GetNormal(), GetTheta()*x);
		}

		public Quat Multiply(Quat p){
			return new Quat(q0*p.q1 + q1*p.q0 + q2*p.q3 - q3*p.q2, q0*p.q2 + q2*p.q0 + q3*p.q1 - q1*p.q3,
				q0*p.q3 + q3*p.q0 + q1*p.q2 - q2*p.q1, q0*p.q0 - q1*p.q1 - q2*p.q2 - q3*p.q3);
		}

		public Quat Divide(Quat p){
			return Multiply(p.Invert());
		}

		public Quat DivideLeft(Quat p){
			return Invert().Multiply(p);
		}

		public float Dot(Quat q){
			return q0*q.q0 + q1*q.q1 + q2*q.q2 + q3*q.q3;
		}

		public Quat Invert(){
			return new Quat(-q1, -q2, -q3, q0);
		}

		public Quat Negate(){
			return new Quat(-q1, -q2, -q3, -q0);
		}

		private float GetFixFactor(){
			return q0 < 0 || q0 == 0 && (q1 < 0 || q1 == 0 && (q2 < 0 || q2 == 0 && q3 < 0)) ? -1 : 1;
		}

		public Vector3F GetVector(int i){
			return GetVectorScaled(i, 1f);
		}

		public Vector3F GetVectorScaled(int i, float scale){
			if (i == -1){
				scale *= GetFixFactor();
				return new Vector3F(q1*scale, q2*scale, q3*scale);
			}
			if (mat == null){
				SetMatrix();
			}
			Vector3F v = new Vector3F();
			mat.GetColumn(i, v);
			if (scale != 1f){
				v.Scale(scale);
			}
			return v;
		}

		public Vector3F GetNormal(){
			Vector3F v = GetRawNormal(this);
			v.Scale(GetFixFactor());
			return v;
		}

		private static Vector3F GetRawNormal(Quat q){
			Vector3F v = new Vector3F(q.q1, q.q2, q.q3);
			if (v.Norm() == 0){
				return new Vector3F(0, 0, 1);
			}
			v.Normalize();
			return v;
		}

		public float GetTheta(){
			return (float) (Math.Acos(Math.Abs(q0))*2*180/Math.PI);
		}

		public float GetThetaRadians(){
			return (float) (Math.Acos(Math.Abs(q0))*2);
		}

		public Vector3F GetNormalDirected(Vector3F v0){
			Vector3F v = GetNormal();
			if (v.x*v0.x + v.y*v0.y + v.z*v0.z < 0){
				v.Scale(-1);
			}
			return v;
		}

		public Vector3F Get3DProjection(Vector3F v3D){
			v3D.Set(q1, q2, q3);
			return v3D;
		}

		public Vector4F GetThetaDirected(Vector4F axisAngle){
			float theta = GetTheta();
			Vector3F v = GetNormal();
			if (axisAngle.X*q1 + axisAngle.Y*q2 + axisAngle.Z*q3 < 0){
				v.Scale(-1);
				theta = -theta;
			}
			axisAngle.Set(v.x, v.y, v.z, theta);
			return axisAngle;
		}

		public float GetThetaDirected(Vector3F vector){
			float theta = GetTheta();
			Vector3F v = GetNormal();
			if (vector.x*q1 + vector.y*q2 + vector.z*q3 < 0){
				v.Scale(-1);
				theta = -theta;
			}
			return theta;
		}

		public Vector4F ToPoint(){
			return new Vector4F(q1, q2, q3, q0);
		}

		public AxisVector3F ToAxisAngle(){
			double theta = 2*Math.Acos(Math.Abs(q0));
			double sinTheta2 = Math.Sin(theta/2);
			Vector3F v = GetNormal();
			if (sinTheta2 < 0){
				v.Scale(-1);
				theta = Math.PI - theta;
			}
			return new AxisVector3F(v, (float) theta);
		}

		public Vector3F Transform(Vector3F pt, Vector3F ptNew){
			if (mat == null){
				SetMatrix();
			}
			mat.Rotate(pt, ptNew);
			return ptNew;
		}

		public Quat LeftDifference(Quat qx2){
			Quat q2Adjusted = (Dot(qx2) < 0 ? qx2.Negate() : qx2);
			return Invert().Multiply(q2Adjusted);
		}

		public Quat RightDifference(Quat qx2){
			Quat q2Adjusted = (Dot(qx2) < 0 ? qx2.Negate() : qx2);
			return Multiply(q2Adjusted.Invert());
		}

		public override string ToString(){
			return "{" + q1 + " " + q2 + " " + q3 + " " + q0 + "}";
		}

		public static Quat[] Divide(Quat[] data1, Quat[] data2, int nMax, bool isRelative){
			int n;
			if (data1 == null || data2 == null || (n = Math.Min(data1.Length, data2.Length)) == 0){
				return null;
			}
			if (nMax > 0 && n > nMax){
				n = nMax;
			}
			Quat[] dqs = new Quat[n];
			for (int i = 0; i < n; i++){
				if (data1[i] == null || data2[i] == null){
					return null;
				}
				dqs[i] = (isRelative ? data1[i].DivideLeft(data2[i]) : data1[i].Divide(data2[i]));
			}
			return dqs;
		}

		public static Quat SphereMean(Quat[] data, float[] retStddev, float criterion){
			if (data == null || data.Length == 0){
				return new Quat();
			}
			if (retStddev == null){
				retStddev = new float[1];
			}
			if (data.Length == 1){
				retStddev[0] = 0;
				return new Quat(data[0]);
			}
			float diff = float.MaxValue;
			float lastStddev = float.MaxValue;
			Quat qMean = SimpleAverage(data);
			int maxIter = 100; // typically goes about 5 iterations
			int iter = 0;
			while (diff > criterion && lastStddev != 0 && iter < maxIter){
				qMean = NewMean(data, qMean);
				retStddev[0] = StdDev(data, qMean);
				diff = Math.Abs(retStddev[0] - lastStddev);
				lastStddev = retStddev[0];
			}
			return qMean;
		}

		private static Quat SimpleAverage(Quat[] ndata){
			Vector3F mean = new Vector3F(0, 0, 1);
			// using the directed normal ensures that the mean is 
			// continually added to and never subtracted from 
			Vector3F v = ndata[0].GetNormal();
			mean.Add(v);
			for (int i = ndata.Length; --i >= 0;){
				mean.Add(ndata[i].GetNormalDirected(mean));
			}
			mean.Subtract(v);
			mean.Normalize();
			float f = 0;
			// the 3D projection of the quaternion is [sin(theta/2)]*n
			// so dotted with the normalized mean gets us an approximate average for sin(theta/2)
			for (int i = ndata.Length; --i >= 0;){
				f += Math.Abs(ndata[i].Get3DProjection(v).Dot(mean));
			}
			if (f != 0){
				mean.Scale(f/ndata.Length);
			}
			// now convert f to the corresponding cosine instead of sine
			f = (float) Math.Sqrt(1 - mean.NormSquared());
			if (float.IsNaN(f)){
				f = 0;
			}
			return new Quat(new Vector4F(mean.x, mean.y, mean.z, f));
		}

		private static Quat NewMean(Quat[] data, Quat mean){
			Vector3F sum = new Vector3F();
			for (int i = data.Length; --i >= 0;){
				Quat q = data[i];
				Quat dq = q.Divide(mean);
				Vector3F v = dq.GetNormal();
				v.Scale(dq.GetTheta());
				sum.Add(v);
			}
			sum.Scale(1f/data.Length);
			Quat dqMean = new Quat(sum, sum.Norm());
			return dqMean.Multiply(mean);
		}

		private static float StdDev(Quat[] data, Quat mean){
			double sum2 = 0;
			int n = data.Length;
			for (int i = n; --i >= 0;){
				float theta = data[i].Divide(mean).GetTheta();
				sum2 += theta*theta;
			}
			return (float) Math.Sqrt(sum2/n);
		}

		public float[] GetEulerZyz(){
			if (q1 == 0 && q2 == 0){
				float theta = GetTheta();
				return new[]{q3 < 0 ? -theta : theta, 0, 0};
			}
			double rA = Math.Atan2(2*(q2*q3 + q0*q1), 2*(-q1*q3 + q0*q2));
			double rB = Math.Acos(q3*q3 - q2*q2 - q1*q1 + q0*q0);
			double rG = Math.Atan2(2*(q2*q3 - q0*q1), 2*(q0*q2 + q1*q3));
			return new[]{(float) (rA/radPerDeg), (float) (rB/radPerDeg), (float) (rG/radPerDeg)};
		}

		public float[] GetEulerZxz(){
			if (q1 == 0 && q2 == 0){
				float theta = GetTheta();
				return new[]{q3 < 0 ? -theta : theta, 0, 0};
			}
			double rA = Math.Atan2(2*(q1*q3 - q0*q2), 2*(q0*q1 + q2*q3));
			double rB = Math.Acos(q3*q3 - q2*q2 - q1*q1 + q0*q0);
			double rG = Math.Atan2(2*(q1*q3 + q0*q2), 2*(-q2*q3 + q0*q1));
			return new[]{(float) (rA/radPerDeg), (float) (rB/radPerDeg), (float) (rG/radPerDeg)};
		}
	}
}