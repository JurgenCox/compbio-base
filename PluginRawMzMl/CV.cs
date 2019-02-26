// ReSharper disable InconsistentNaming
namespace PluginRawMzMl
{
	/// <summary>
	/// Controlled vocabulary https://raw.githubusercontent.com/HUPO-PSI/psi-ms-CV/master/psi-ms.obo
	/// </summary>
	public static class CV
	{
		// spectrum
		public const string MS_LEVEL = "MS:1000511";
		public const string POSITIVE_SCAN = "MS:1000130";
		public const string BASE_PEAK_INTENSITY = "MS:1000505";
		public const string TOTAL_ION_CURRENT = "MS:1000285";
		public const string PROFILE_SPECTRUM = "MS:1000128";
		public const string CENTROID_SPECTRUM = "MS:1000127";
		public const string LOWEST_OBSERVED_M_Z = "MS:1000528";
		public const string HIGHEST_OBSERVED_M_Z = "MS:1000527";

		// scanList
		public const string NO_COMBINATION = "MS:1000795";

		// scanList/scan
		public const string SCAN_START_TIME = "MS:1000016";

		// analyzer
		public const string TIME_OF_FLIGHT = "MS:1000084";
		public const string FOURIER_TRANSFORM_ION_CYCLOTRON_RESONANCE_MASS_SPECTROMETER = "MS:1000079";
		public const string RADIAL_EJECTION_LINEAR_ION_TRAP = "MS:1000078";

		// precursor / activation
		public const string COLLISION_INDUCED_DISSOCIATION = "MS:1000133";
		public const string BEAM_TYPE_COLLISION_INDUCED_DISSOCIATION = "MS:1000422";
		public const string COLLISION_ENERGY = "MS:1000045";

		// precursor / isolation window
		public static string ISOLATION_WINDOW_TARGET_M_Z = "MS:1000827";
		public static string ISOLATION_WINDOW_LOWER_OFFSET = "MS:1000828";
		public static string ISOLATION_WINDOW_UPPER_OFFSET = "MS:1000829";

		// binaryDataArray
		public const string NO_COMPRESSION = "MS:1000576";
		public const string ZLIB_COMPRESSION = "MS:1000574";
		public const string INTEGER_64_BIT = "MS:1000522";
		public const string FLOAT_64_BIT = "MS:1000523";
		public const string FLOAT_32_BIT = "MS:1000521";
		public const string M_Z_ARRAY = "MS:1000514";
		public const string INTENSITY_ARRAY = "MS:1000515";

		// NUMPRESS compression
		public const string NUMPRESS_LINEAR = "MS:1002312";
		public const string NUMPRESS_PIC = "MS:1002313";
		public const string NUMPRESS_SLOF = "MS:1002314";
		public static string[] NUMPRESS_ALL = { NUMPRESS_LINEAR, NUMPRESS_PIC, NUMPRESS_SLOF };
	}
}