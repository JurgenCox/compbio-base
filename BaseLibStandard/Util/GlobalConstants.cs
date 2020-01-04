using System;

namespace BaseLibS.Util{
	public static class GlobalConstants{
		public const double eCharge = 1.60217646e-19; //C 
		public const double hPlanck = 6.62606957e-34; //J*s
		public const double hBarPlanck = hPlanck / (2 * Math.PI);
		/// <summary>
		/// Prefix used for reverse protein sequences.
		/// </summary>
		public const string revPrefix = "REV__";

		/// <summary>
		/// Prefix for proteins flagged as contaminants.
		/// </summary>
		public const string conPrefix = "CON__";
	}
}