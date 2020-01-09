using System;

namespace BaseLibS.Num {
	public static class NumConstants {
		public static readonly double ln2 = Math.Log(2);
		public static readonly double ln10 = Math.Log(10);
		public static readonly double radToDegree = 180.0 / Math.PI;
		public static readonly double degreeToRad = Math.PI / 180.0;

		/// <summary>
		/// Boltzmann constant
		/// </summary>
		public static readonly double kb = 1.38064852e-23; //m^2 kg /(s^2 K)

		/// <summary>
		/// elementary charge
		/// </summary>
		public static readonly double elemCharge = 1.60217662e-19; //coulombs

		public static float DegreesToRadians(float degrees) {
			return degrees * (float) degreeToRad;
		}
	}
}