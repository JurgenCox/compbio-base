using System;
using BaseLibS.Ms;

namespace PluginRawMzXml {
	/// <summary>
	/// Implementation of the <see cref="RawFile"/> interface for mz-xml files.
	/// </summary>
	public class MzXmlRawFile : RawFile {
		private MzXml mzXml;
		private double maxIntensity;
		public override string Suffix => ".mzxml";
		public override string Name => "MzXml file";
		public override MsInstrument DefaultInstrument => null;
		public override bool IsInstalled => true;
		public override bool IsFolderBased => false;
		public override bool NeedsGrid => false;
		public override string InstallMessage => "Something went wrong with MzXmlRawFile. " +
												"Should never be accessed since IsInstalled is always true.";
		public override bool HasIms => false;
		protected override void PreInit() {
			preInitialized = false;
//			try {
				// open the link to the File
				mzXml = new MzXml(Path);
				// retrieve basic information
				maxIntensity = 0;
				for (int scanNumber = mzXml.GetFirstSpectrumNumber(); scanNumber <= mzXml.GetLastSpectrumNumber(); ++scanNumber) {
					maxIntensity = Math.Max(maxIntensity, mzXml.GetScanHeader(scanNumber).BasePeakIntensity);
				}
				preInitialized = true;
//			} catch (Exception e) {
//				Console.WriteLine(e.Message);
//				Console.WriteLine(e.StackTrace);
//				throw;
//			}
		}
		public override int FirstScanNumber {
			get {
				if (!preInitialized) {
					PreInit();
				}
				return mzXml.GetFirstSpectrumNumber();
			}
		}
		public override int LastScanNumber {
			get {
				if (!preInitialized) {
					PreInit();
				}
				return mzXml.GetLastSpectrumNumber();
			}
		}
		protected override double MaximumIntensity => maxIntensity;
		protected override void GetSpectrum(int scanNumberMin, int scanNumberMax, int imsIndexMin, int imsIndexMax,
			bool readCentroids, out double[] masses, out double[] intensities, double resolution, double mzMin, double mzMax) {
			if (!preInitialized) {
				PreInit();
			}
			double[,] spectrum = mzXml.GetMassListFromScanNum(scanNumberMin);
			int length = spectrum.Length / 2;
			masses = new double[length];
			intensities = new double[length];
			for (int i = 0; i < length; ++i) {
				masses[i] = spectrum[0, i];
				intensities[i] = spectrum[1, i];
			}
		}
		protected override ScanInfo GetInfoForScanNumber(int scanNumber) {
			if (!preInitialized) {
				PreInit();
			}
			if (scanNumber < mzXml.GetFirstSpectrumNumber() || scanNumber > mzXml.GetLastSpectrumNumber()) {
				return null;
			}
			MzXml.MzxmlHeader header = mzXml.GetHeader();
			MzXml.ScanHeader scanHeader = mzXml.GetScanHeader(scanNumber);
			// todo
			ScanInfo scanInfo = new ScanInfo {
				intenseCompFactor = double.NaN,
				emIntenseComp = double.NaN,
				rawOvFtT = double.NaN,
				positiveIonMode = true,
				basepeakIntensity = 0,
				msLevel = scanHeader.MsLevel == 1 ? MsLevel.Ms1 : MsLevel.Ms2,
				min = scanHeader.LowMz, // TODO low resolution value...
				max = scanHeader.HighMz, // TODO low resolution value...
				ionInjectionTime = -1, // TODO defaulting to -1 (field not existing)
				elapsedTime = -1, // TODO defaulting to -1 (field not existing)
				tic = scanHeader.TotalIonCurrent,
				rt = scanHeader.RetentionTime,
				resolution = header.Resolution == -1 ? 30000 : header.Resolution
			};
			if (scanInfo.msLevel == MsLevel.Ms2) {
				scanInfo.ms2ParentMz = scanHeader.PrecursorMz;
				scanInfo.ms2IsolationMin = scanHeader.PrecursorMz - 1;
				scanInfo.ms2IsolationMax = scanHeader.PrecursorMz + 1;
				scanInfo.ms2MonoMz = double.NaN;
				scanInfo.ms2FragType = scanHeader.FragmentationType;
				//scanInfo.ms2IsolationWidth = 4; // TODO defaulting to 4 (field not existing)
				scanInfo.energy = scanHeader.CollisionEnergy;
			} else {
				scanInfo.ms2FragType = FragmentationTypeEnum.Unknown;
				scanInfo.ms2ParentMz = 0;
				//scanInfo.ms2IsolationWidth = 0;
				scanInfo.energy = 0;
			}
			// thermo specific
			if (header.MachineManufacturer != null && (header.MachineManufacturer.Equals("Thermo Scientific") ||
														header.MachineManufacturer.Equals("Thermo Finnigan"))) {
				string filter = scanHeader.FilterLine;
				if (filter != null) {
					scanInfo.isSim = filter.Contains("SIM");
					scanInfo.analyzer = filter.Contains("ITMS")
						? MassAnalyzerEnum.Itms
						: (filter.Contains("FTMS") ? MassAnalyzerEnum.Ftms : MassAnalyzerEnum.Unknown);
					if (filter.Contains(" p ")) {
						scanInfo.hasProfile = true;
						scanInfo.hasCentroid = false;
					} else {
						scanInfo.hasProfile = false;
						scanInfo.hasCentroid = true;
					}
				} else {
					scanInfo.isSim = scanHeader.ScanType == "SIM";
					scanInfo.analyzer = MassAnalyzerEnum.Ftms; // information unkown, we assume high resolution
					if (scanHeader.Centroided == "0") {
						scanInfo.hasProfile = true;
						scanInfo.hasCentroid = false;
					} else {
						scanInfo.hasProfile = false;
						scanInfo.hasCentroid = true;
					}
				}
			} else {
				scanInfo.isSim = false;
				scanInfo.analyzer = MassAnalyzerEnum.Unknown;
				if (header.SignalType == SignalType.Profile) {
					scanInfo.hasProfile = true;
					scanInfo.hasCentroid = false;
				}
			}
			// did the processing screw up the data
			if (header.SignalType != SignalType.Profile) {
				scanInfo.hasProfile = false;
				scanInfo.hasCentroid = true;
			}
			return scanInfo;
		}
	}
}