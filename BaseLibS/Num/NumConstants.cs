using System;

namespace BaseLibS.Num{
	public static class NumConstants{
		public static readonly double ln2 = Math.Log(2);
		public static readonly double ln10 = Math.Log(10);
		public static readonly double radToDegree = 180.0/Math.PI;
		public static readonly double degreeToRad = Math.PI/180.0;

		public static float DegreesToRadians(float degrees){
			return degrees*(float) degreeToRad;
		}
	}
}