using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Ms {
	/// <summary>
	/// A "layer" is the information of a raw data file for only the positive or only the negative ions.
	/// </summary>
	public class RawFileLayer : IDisposable {
		private RawFile rawFile;
		private readonly bool positive;
		public double Ms1MassMin { get; protected internal set; }
		public double Ms1MassMax { get; protected internal set; }
		public int Ms1MaxNumIms { get; protected internal set; }
		public int[] Ms1ScanNumbers { get; protected internal set; }
		public int[] Ms1NumIms { get; protected internal set; }
		public double[] Ms1Rt { get; protected internal set; }
		public double[] Ms1IonInjectionTimes { get; protected internal set; }
		public double[] Ms1BasepeakIntensities { get; protected internal set; }
		public double[] Ms1ElapsedTimes { get; protected internal set; }
		public double[] Ms1Tic { get; protected internal set; }
		protected internal bool[] ms1HasCentroid;
		protected internal bool[] ms1HasProfile;
		protected internal MassAnalyzerEnum[] ms1Analyzer;
		protected internal double[] ms1MinMass;
		protected internal double[] ms1MaxMass;
		public double[] Ms1Resolution { get; protected internal set; }
		public bool[] Ms1IsSim { get; protected internal set; }
		public double[] Ms1ResolutionValues { get; protected internal set; }
		public double[] Ms1IntenseCompFactor { get; protected internal set; }
		public double[] Ms1EmIntenseComp { get; protected internal set; }
		public double[] Ms1RawOvFtT { get; protected internal set; }
		public double[] Ms1AgcFill { get; protected internal set; }
		public double Ms2MassMin { get; protected internal set; }
		public double Ms2MassMax { get; protected internal set; }
		public int Ms2MaxNumIms { get; protected internal set; }
		public int[] Ms2PrevMs1Index { get; protected internal set; }
		public int[][] Ms2DependentMs3Inds { get; protected internal set; }
		public int[] Ms2ScanNumbers { get; protected internal set; }
		public int[] Ms2NumIms { get; protected internal set; }
		public double[] Ms2Rt { get; protected internal set; }
		public double[] Ms2Mz { get; protected internal set; }
		public double[] Ms2IsolationMzMin { get; protected internal set; }
		public double[] Ms2IsolationMzMax { get; protected internal set; }
		public FragmentationTypeEnum[] ms2FragmentationTypes;
		public double[] Ms2IonInjectionTimes { get; protected internal set; }
		public double[] Ms2BasepeakIntensities { get; protected internal set; }
		public double[] Ms2ElapsedTimes { get; protected internal set; }
		public double[] Ms2Energies { get; protected internal set; }
		public double[] Ms2Summations { get; protected internal set; }
		public double[] Ms2MonoisotopicMz { get; protected internal set; }
		public double[] Ms2Tic { get; protected internal set; }
		protected internal bool[] ms2HasCentroid;
		protected internal bool[] ms2HasProfile;
		protected internal MassAnalyzerEnum[] ms2Analyzer;
		protected internal double[] ms2MinMass;
		protected internal double[] ms2MaxMass;
		public double[] Ms2Resolution { get; protected internal set; }
		public double[] Ms2IntenseCompFactor { get; protected internal set; }
		public double[] Ms2EmIntenseComp { get; protected internal set; }
		public double[] Ms2RawOvFtT { get; protected internal set; }
		public double[] Ms2AgcFill { get; protected internal set; }
		public double Ms3MassMin { get; protected internal set; }
		public double Ms3MassMax { get; protected internal set; }
		public int[] Ms3PrevMs2Index { get; protected internal set; }
		public int[] Ms3ScanNumbers { get; protected internal set; }
		public double[] Ms3Rt { get; protected internal set; }
		public double[] Ms3Mz1 { get; protected internal set; }
		public double[] Ms3Mz2 { get; protected internal set; }
		public double[] Ms3IsolationMzMin { get; protected internal set; }
		public double[] Ms3IsolationMzMax { get; protected internal set; }
		public FragmentationTypeEnum[] ms3FragmentationTypes;
		public double[] Ms3IonInjectionTimes { get; protected internal set; }
		public double[] Ms3BasepeakIntensities { get; protected internal set; }
		public double[] Ms3ElapsedTimes { get; protected internal set; }
		public double[] Ms3Energies { get; protected internal set; }
		public double[] Ms3Summations { get; protected internal set; }
		public double[] Ms3MonoisotopicMz { get; protected internal set; }
		public double[] Ms3Tic { get; protected internal set; }
		protected internal bool[] ms3HasCentroid;
		protected internal bool[] ms3HasProfile;
		protected internal MassAnalyzerEnum[] ms3Analyzer;
		protected internal double[] ms3MinMass;
		protected internal double[] ms3MaxMass;
		public double[] Ms3Resolution { get; protected internal set; }
		public double[] Ms3IntenseCompFactor { get; protected internal set; }
		public double[] Ms3EmIntenseComp { get; protected internal set; }
		public double[] Ms3RawOvFtT { get; protected internal set; }
		public double[] Ms3AgcFill { get; protected internal set; }
		public int Ms1MassRangeCount { get; protected internal set; }
		public double MaxIntensity { get; protected internal set; }
		public double[] massRangesMin;

		protected internal RawFileLayer(RawFile rawFile, bool positive) {
			this.rawFile = rawFile;
			this.positive = positive;
		}

		protected internal RawFileLayer(BinaryReader reader, RawFile rawFile, bool positive) {
			this.rawFile = rawFile;
			this.positive = positive;
			Ms1ScanNumbers = FileUtils.ReadInt32Array(reader);
			Ms1NumIms = FileUtils.ReadInt32Array(reader);
			Ms2ScanNumbers = FileUtils.ReadInt32Array(reader);
			Ms2NumIms = FileUtils.ReadInt32Array(reader);
			Ms2PrevMs1Index = FileUtils.ReadInt32Array(reader);
			Ms1Rt = FileUtils.ReadDoubleArray(reader);
			Ms2Rt = FileUtils.ReadDoubleArray(reader);
			Ms1IonInjectionTimes = FileUtils.ReadDoubleArray(reader);
			Ms1BasepeakIntensities = FileUtils.ReadDoubleArray(reader);
			Ms2IonInjectionTimes = FileUtils.ReadDoubleArray(reader);
			Ms2BasepeakIntensities = FileUtils.ReadDoubleArray(reader);
			Ms1ElapsedTimes = FileUtils.ReadDoubleArray(reader);
			Ms2ElapsedTimes = FileUtils.ReadDoubleArray(reader);
			Ms2Energies = FileUtils.ReadDoubleArray(reader);
			Ms2Summations = FileUtils.ReadDoubleArray(reader);
			Ms2Mz = FileUtils.ReadDoubleArray(reader);
			Ms2IsolationMzMin = FileUtils.ReadDoubleArray(reader);
			Ms2IsolationMzMax = FileUtils.ReadDoubleArray(reader);
			Ms2MonoisotopicMz = FileUtils.ReadDoubleArray(reader);
			massRangesMin = FileUtils.ReadDoubleArray(reader);
			Ms1Tic = FileUtils.ReadDoubleArray(reader);
			Ms2Tic = FileUtils.ReadDoubleArray(reader);
			ms1MinMass = FileUtils.ReadDoubleArray(reader);
			ms1MaxMass = FileUtils.ReadDoubleArray(reader);
			ms2MinMass = FileUtils.ReadDoubleArray(reader);
			ms2MaxMass = FileUtils.ReadDoubleArray(reader);
			Ms1IsSim = FileUtils.ReadBooleanArray(reader);
			Ms1Resolution = FileUtils.ReadDoubleArray(reader);
			Ms1ResolutionValues = FileUtils.ReadDoubleArray(reader);
			Ms2Resolution = FileUtils.ReadDoubleArray(reader);
			Ms1IntenseCompFactor = FileUtils.ReadDoubleArray(reader);
			Ms2IntenseCompFactor = FileUtils.ReadDoubleArray(reader);
			Ms1EmIntenseComp = FileUtils.ReadDoubleArray(reader);
			Ms2EmIntenseComp = FileUtils.ReadDoubleArray(reader);
			Ms1RawOvFtT = FileUtils.ReadDoubleArray(reader);
			Ms2RawOvFtT = FileUtils.ReadDoubleArray(reader);
			Ms1AgcFill = FileUtils.ReadDoubleArray(reader);
			Ms2AgcFill = FileUtils.ReadDoubleArray(reader);
			Ms1MassMin = reader.ReadDouble();
			Ms1MassMax = reader.ReadDouble();
			Ms1MaxNumIms = reader.ReadInt32();
			Ms2MassMin = reader.ReadDouble();
			Ms2MassMax = reader.ReadDouble();
			Ms2MaxNumIms = reader.ReadInt32();
			Ms1MassRangeCount = reader.ReadInt32();
			MaxIntensity = reader.ReadDouble();
			int len = reader.ReadInt32();
			ms2FragmentationTypes = new FragmentationTypeEnum[len];
			for (int i = 0; i < len; i++) {
				int x = reader.ReadInt32();
				ms2FragmentationTypes[i] = RawFileUtils.IntToFragmentationTypeEnum(x);
			}
			ms1HasCentroid = FileUtils.ReadBooleanArray(reader);
			ms1HasProfile = FileUtils.ReadBooleanArray(reader);
			ms2HasCentroid = FileUtils.ReadBooleanArray(reader);
			ms2HasProfile = FileUtils.ReadBooleanArray(reader);
			len = reader.ReadInt32();
			ms1Analyzer = new MassAnalyzerEnum[len];
			for (int i = 0; i < len; i++) {
				int x = reader.ReadInt32();
				ms1Analyzer[i] = RawFileUtils.IntToMassAnalyzerEnum(x);
			}
			len = reader.ReadInt32();
			ms2Analyzer = new MassAnalyzerEnum[len];
			for (int i = 0; i < len; i++) {
				int x = reader.ReadInt32();
				ms2Analyzer[i] = RawFileUtils.IntToMassAnalyzerEnum(x);
			}
			Ms3MassMin = reader.ReadDouble();
			Ms3MassMax = reader.ReadDouble();
			Ms3PrevMs2Index = FileUtils.ReadInt32Array(reader);
			Ms3ScanNumbers = FileUtils.ReadInt32Array(reader);
			Ms3Rt = FileUtils.ReadDoubleArray(reader);
			Ms3Mz1 = FileUtils.ReadDoubleArray(reader);
			Ms3Mz2 = FileUtils.ReadDoubleArray(reader);
			Ms3IsolationMzMin = FileUtils.ReadDoubleArray(reader);
			Ms3IsolationMzMax = FileUtils.ReadDoubleArray(reader);
			Ms3IonInjectionTimes = FileUtils.ReadDoubleArray(reader);
			Ms3BasepeakIntensities = FileUtils.ReadDoubleArray(reader);
			Ms3ElapsedTimes = FileUtils.ReadDoubleArray(reader);
			Ms3Energies = FileUtils.ReadDoubleArray(reader);
			Ms3Summations = FileUtils.ReadDoubleArray(reader);
			Ms3MonoisotopicMz = FileUtils.ReadDoubleArray(reader);
			Ms3Tic = FileUtils.ReadDoubleArray(reader);
			ms3HasCentroid = FileUtils.ReadBooleanArray(reader);
			ms3HasProfile = FileUtils.ReadBooleanArray(reader);
			ms3MinMass = FileUtils.ReadDoubleArray(reader);
			ms3MaxMass = FileUtils.ReadDoubleArray(reader);
			Ms3Resolution = FileUtils.ReadDoubleArray(reader);
			Ms3IntenseCompFactor = FileUtils.ReadDoubleArray(reader);
			Ms3EmIntenseComp = FileUtils.ReadDoubleArray(reader);
			Ms3RawOvFtT = FileUtils.ReadDoubleArray(reader);
			Ms3AgcFill = FileUtils.ReadDoubleArray(reader);
			len = reader.ReadInt32();
			ms3FragmentationTypes = new FragmentationTypeEnum[len];
			for (int i = 0; i < len; i++) {
				int x = reader.ReadInt32();
				ms3FragmentationTypes[i] = RawFileUtils.IntToFragmentationTypeEnum(x);
			}
			len = reader.ReadInt32();
			ms3Analyzer = new MassAnalyzerEnum[len];
			for (int i = 0; i < len; i++) {
				int x = reader.ReadInt32();
				ms3Analyzer[i] = RawFileUtils.IntToMassAnalyzerEnum(x);
			}
			Ms2DependentMs3Inds = FileUtils.Read2DInt32Array(reader);
		}

		public int Ms1Count => Ms1ScanNumbers.Length;
		public int Ms2Count => Ms2ScanNumbers.Length;
		public int Ms3Count => Ms3ScanNumbers.Length;
		public double StartTime => Math.Min(StartTimeMs1, StartTimeMs2);
		public double StartTimeMs1 => Ms1Rt.Length < 1 ? 0 : 1.5f * GetMs1Time(0) - 0.5f * GetMs1Time(1);
		public double StartTimeMs2 => Ms2Rt.Length < 1 ? 0 : 1.5f * GetMs2Time(0) - 0.5f * GetMs2Time(1);
		public double EndTime => Math.Max(EndTimeMs1, EndTimeMs2);
		public double EndTimeMs1 => Ms1Rt.Length < 1 ? 1 : 1.5f * GetMs1Time(Ms1Count - 1) - 0.5f * GetMs1Time(Ms1Count - 2);
		public double EndTimeMs2 => Ms2Rt.Length < 1 ? 1 : 1.5f * GetMs2Time(Ms2Count - 1) - 0.5f * GetMs2Time(Ms2Count - 2);

		public int[] Ms2FragTypes {
			get {
				int[] result = new int[ms2FragmentationTypes.Length];
				for (int i = 0; i < result.Length; i++) {
					result[i] = RawFileUtils.FragmentationTypeEnumToInt(ms2FragmentationTypes[i]);
				}
				return result;
			}
		}

		public int[] Ms2MassAnalyzers {
			get {
				int[] result = new int[ms2Analyzer.Length];
				for (int i = 0; i < result.Length; i++) {
					result[i] = RawFileUtils.MassAnalyzerEnumToInt(ms2Analyzer[i]);
				}
				return result;
			}
		}

		public int[] Ms2ScanEventNumbers {
			get {
				int[] result = new int[ms2FragmentationTypes.Length];
				for (int i = 0; i < result.Length; i++) {
					result[i] = GetScanEventNumberForMs2Ind(i);
				}
				return result;
			}
		}

		private int GetMs1CeilIndexFromRt(double rt) {
			if (rt <= Ms1Rt[0]) {
				return 0;
			}
			if (rt >= Ms1Rt[Ms1Rt.Length - 1]) {
				return -1;
			}
			int a = Array.BinarySearch(Ms1Rt, rt);
			if (a >= 0) {
				return a;
			}
			return -a - 1;
		}

		private int GetMs1FloorIndexFromRt(double rt) {
			if (rt <= Ms1Rt[0]) {
				return -1;
			}
			if (rt >= Ms1Rt[Ms1Rt.Length - 1]) {
				return Ms1Rt.Length - 1;
			}
			int a = Array.BinarySearch(Ms1Rt, rt);
			if (a >= 0) {
				return a;
			}
			return -a - 2;
		}

		private int GetMs2CeilIndexFromRt(double rt) {
			if (rt <= Ms2Rt[0]) {
				return 0;
			}
			if (rt >= Ms2Rt[Ms2Rt.Length - 1]) {
				return -1;
			}
			int a = Array.BinarySearch(Ms2Rt, rt);
			if (a >= 0) {
				return a;
			}
			return -a - 1;
		}

		private int GetMs2FloorIndexFromRt(double rt) {
			if (rt <= Ms2Rt[0]) {
				return -1;
			}
			if (rt >= Ms2Rt[Ms2Rt.Length - 1]) {
				return Ms2Rt.Length - 1;
			}
			int a = Array.BinarySearch(Ms2Rt, rt);
			if (a >= 0) {
				return a;
			}
			return -a - 2;
		}

		public double GetMs1Time(int index) {
			if (index < 0 || index >= Ms1Rt.Length) {
				return double.NaN;
			}
			return Ms1Rt[index];
		}

		public double GetMs2Time(int index) {
			if (index < 0 || index >= Ms2Rt.Length) {
				return double.NaN;
			}
			return Ms2Rt[index];
		}

		public double GetMs2IsolationMax(int index) {
			if (index >= Ms2IsolationMzMax.Length || index < 0) {
				return double.NaN;
			}
			return Ms2IsolationMzMax[index];
		}

		public double GetMs2IsolationMin(int index) {
			if (index >= Ms2IsolationMzMin.Length || index < 0) {
				return double.NaN;
			}
			return Ms2IsolationMzMin[index];
		}

		public double GetMaxMs2IsolationWidth() {
			if (Ms2IsolationMzMin == null) {
				return double.NaN;
			}
			double max = 0;
			for (int i = 0; i < Ms2IsolationMzMin.Length; i++) {
				double t = Ms2IsolationMzMax[i] - Ms2IsolationMzMin[i];
				if (t > max) {
					max = t;
				}
			}
			return max;
		}

		public int GetMasterMs1Index(int index) {
			return Math.Max(0, ArrayUtils.FloorIndex(Ms1ScanNumbers, Ms2ScanNumbers[index]));
		}

		public double[] GetMs1TimeSpan(int index) {
			if (index < 0) {
				return new double[] {0, 0};
			}
			double min = index <= 0 ? StartTimeMs1 : 0.5f * (Ms1Rt[index] + Ms1Rt[index - 1]);
			double max = index >= Ms1Rt.Length - 1 ? EndTimeMs1 : 0.5f * (Ms1Rt[index] + Ms1Rt[index + 1]);
			return new[] {min, max};
		}

		public double[] GetMs2TimeSpan(int index) {
			if (index < 0) {
				return new double[] {0, 0};
			}
			double min = index <= 0 ? StartTimeMs2 : 0.5f * (Ms2Rt[index] + Ms2Rt[index - 1]);
			double max = index >= Ms2Count - 1 ? EndTimeMs2 : 0.5f * (Ms2Rt[index] + Ms2Rt[index + 1]);
			return new[] {min, max};
		}

		public int GetMs1IndexFromRt(double rt) {
			if (double.IsNaN(rt) || double.IsInfinity(rt)) {
				return -1;
			}
			if (Ms1Rt.Length == 0) {
				return -1;
			}
			if (rt < StartTimeMs1 || rt > EndTimeMs1) {
				return -1;
			}
			if (rt <= Ms1Rt[0]) {
				return 0;
			}
			if (rt >= Ms1Rt[Ms1Rt.Length - 1]) {
				return Ms1Rt.Length - 1;
			}
			int a = Array.BinarySearch(Ms1Rt, rt);
			if (a >= 0) {
				return a;
			}
			if (2 * rt < Ms1Rt[-a - 1] + Ms1Rt[-a - 2]) {
				return -a - 2;
			}
			return -a - 1;
		}

		public int GetMs2IndexFromRt(double rt) {
			if (double.IsNaN(rt) || double.IsInfinity(rt)) {
				return -1;
			}
			if (Ms2Rt.Length == 0) {
				return -1;
			}
			if (rt < StartTimeMs2 || rt > EndTimeMs2) {
				return -1;
			}
			if (rt <= Ms2Rt[0]) {
				return 0;
			}
			if (rt >= Ms2Rt[Ms2Rt.Length - 1]) {
				return Ms2Rt.Length - 1;
			}
			int a = Array.BinarySearch(Ms2Rt, rt);
			if (a >= 0) {
				return a;
			}
			if (2 * rt < Ms2Rt[-a - 1] + Ms2Rt[-a - 2]) {
				return -a - 2;
			}
			return -a - 1;
		}

		public int GetScanNumberFromMs1Index(int ms1Index) {
			return Ms1ScanNumbers[ms1Index];
		}

		public int GetScanNumberFromMs2Index(int ms2Index) {
			return Ms2ScanNumbers[ms2Index];
		}

		public double GetMs2ParentMz(int index, double ms2PrecShift) {
			if (index < 0 || index >= Ms2Mz.Length) {
				return double.NaN;
			}
			return Ms2Mz[index] + ms2PrecShift;
		}

		public double GetMs3ParentMz1(int index) {
			if (index < 0 || index >= Ms3Mz1.Length) {
				return double.NaN;
			}
			return Ms3Mz1[index];
		}

		public double GetMs3ParentMz2(int index) {
			if (index < 0 || index >= Ms3Mz2.Length) {
				return double.NaN;
			}
			return Ms3Mz2[index];
		}

		public double GetMs2IsolationMzMin(int index) {
			if (index < 0 || index >= Ms2IsolationMzMin.Length) {
				return double.NaN;
			}
			return Ms2IsolationMzMin[index];
		}

		public double GetMs2IsolationMzMax(int index) {
			if (index < 0 || index >= Ms2IsolationMzMax.Length) {
				return double.NaN;
			}
			return Ms2IsolationMzMax[index];
		}

		public int GetMs2IndexFromScanNumber(int scanNumber) {
			int ind = Array.BinarySearch(Ms2ScanNumbers, scanNumber);
			if (ind < 0) {
				return -1;
			}
			return ind;
		}

		public int GetClosestMs1IndexFromScanNumber(int scanNumber) {
			if (scanNumber <= Ms1ScanNumbers[0]) {
				return 0;
			}
			if (scanNumber >= Ms1ScanNumbers[Ms1ScanNumbers.Length - 1]) {
				return Ms1ScanNumbers.Length - 1;
			}
			int ind = Array.BinarySearch(Ms1ScanNumbers, scanNumber);
			if (ind >= 0) {
				return ind;
			}
			if (2 * scanNumber <= Ms1ScanNumbers[-2 - ind] + Ms1ScanNumbers[-1 - ind]) {
				return -2 - ind;
			}
			return -1 - ind;
		}

		public void GetMs2SpectrumArray(int index, bool readCentroids, out double[] masses, out double[] intensities) {
			rawFile.GetSpectrum(Ms2ScanNumbers[index], readCentroids, out masses, out intensities);
		}

		public void GetMs2SpectrumArray(int index, int imsIndMin, int imsIndMax, bool readCentroids, out double[] masses,
			out double[] intensities, double resolution, double mzMin, double mzMax) {
			rawFile.GetSpectrum(Ms2ScanNumbers[index], Ms2ScanNumbers[index], imsIndMin, imsIndMax, readCentroids, out masses,
				out intensities, resolution, mzMin, mzMax);
		}

		public void GetMs3SpectrumArray(int index, bool readCentroids, out double[] masses, out double[] intensities) {
			rawFile.GetSpectrum(Ms3ScanNumbers[index], readCentroids, out masses, out intensities);
		}

		public void GetMs1SpectrumArray(int index, bool readCentroids, out double[] masses, out double[] intensities) {
			if (index >= Ms1ScanNumbers.Length) {
				masses = null;
				intensities = null;
				return;
			}
			rawFile.GetSpectrum(Ms1ScanNumbers[index], readCentroids, out masses, out intensities);
		}

		public void GetMs1SpectrumArray(int index, int imsIndexMin, int imsIndexMax, bool readCentroids, out double[] masses,
			out double[] intensities, double resolution, double mzMin, double mzMax) {
			if (index >= Ms1ScanNumbers.Length) {
				masses = null;
				intensities = null;
				return;
			}
			rawFile.GetSpectrum(Ms1ScanNumbers[index], Ms1ScanNumbers[index], imsIndexMin, imsIndexMax, readCentroids,
				out masses, out intensities, resolution, mzMin, mzMax);
		}

		public void GetMs1SpectrumArray(int indexMin, int indexMax, int imsIndexMin, int imsIndexMax, bool readCentroids,
			out double[] masses, out double[] intensities, double resolution, double mzMin, double mzMax) {
			if (indexMax >= Ms1ScanNumbers.Length || indexMax < 0) {
				masses = null;
				intensities = null;
				return;
			}
			rawFile.GetSpectrum(Ms1ScanNumbers[indexMin], Ms1ScanNumbers[indexMax], imsIndexMin, imsIndexMax, readCentroids,
				out masses, out intensities, resolution, mzMin, mzMax);
		}

		public void GetMs2SpectrumArray(int indexMin, int indexMax, int imsIndexMin, int imsIndexMax, bool readCentroids,
			out double[] masses, out double[] intensities, double resolution, double mzMin, double mzMax) {
			if (indexMax >= Ms2ScanNumbers.Length || indexMax < 0) {
				masses = null;
				intensities = null;
				return;
			}
			rawFile.GetSpectrum(Ms2ScanNumbers[indexMin], Ms2ScanNumbers[indexMax], imsIndexMin, imsIndexMax, readCentroids,
				out masses, out intensities, resolution, mzMin, mzMax);
		}

		// no usages
		public bool HasMs2Centroid(int index) {
			return ms2HasCentroid[index];
		}

		public bool HasMs2Profile(int index) {
			return ms2HasProfile[index];
		}

		public bool HasMs3Profile(int index) {
			return ms3HasProfile[index];
		}

		// no usages
		public bool HasMs1Centroid(int index) {
			return ms1HasCentroid[index];
		}

		public bool HasMs1Profile(int index) {
			return ms1HasProfile[index];
		}

		public MassAnalyzerEnum GetMs2MassAnalyzer(int index) {
			return ms2Analyzer[index];
		}

		public MassAnalyzerEnum GetMs3MassAnalyzer(int index) {
			return ms3Analyzer[index];
		}

		private MassAnalyzerEnum GetMs1MassAnalyzer(int index) {
			if (index >= ms1Analyzer.Length) {
				return MassAnalyzerEnum.Unknown;
			}
			return ms1Analyzer[index];
		}

		public void GetMs1MassRange(int index, out double minMass, out double maxMass) {
			if (index < 0 || index >= ms1MinMass.Length) {
				minMass = 200;
				maxMass = 300;
				return;
			}
			minMass = ms1MinMass[index];
			maxMass = ms1MaxMass[index];
		}

		public void GetMs2MassRange(int index, out double minMass, out double maxMass) {
			if (index < 0 || index >= ms2MinMass.Length) {
				minMass = 200;
				maxMass = 300;
				return;
			}
			minMass = ms2MinMass[index];
			maxMass = ms2MaxMass[index];
		}

		public FragmentationTypeEnum GetMs2FragmentationType(int index) {
			if (index >= ms2FragmentationTypes.Length || index < 0) {
				return FragmentationTypeEnum.Unknown;
			}
			return ms2FragmentationTypes[index];
		}

		public FragmentationTypeEnum GetMs3FragmentationType(int index) {
			if (index >= ms3FragmentationTypes.Length || index < 0) {
				return FragmentationTypeEnum.Unknown;
			}
			return ms3FragmentationTypes[index];
		}

		public double GetMs2MonoisotopicMz(int index) {
			if (index >= Ms2MonoisotopicMz.Length || index < 0) {
				return 0;
			}
			return Ms2MonoisotopicMz[index];
		}

		public byte GetMs1MassRangeIndex(int index) {
			if (!ms1HasProfile[index]) {
				return 0;
			}
			GetMs1MassRange(index, out double min, out double _);
			for (int i = 0; i < massRangesMin.Length; i++) {
				if (min == massRangesMin[i]) {
					return (byte) i;
				}
			}
			return byte.MaxValue;
		}

		public List<int> GetMs2InRectangle(double minMz, double maxMz, double minRt, double maxRt, double ms2PrecShift) {
			List<int> result = new List<int>();
			for (int i = 0; i < Ms2Count; i++) {
				double t = GetMs2Time(i);
				if (t < minRt || t > maxRt) {
					continue;
				}
				double ms = GetMs2ParentMz(i, ms2PrecShift);
				double m = ms;
				if (m >= minMz && m <= maxMz) {
					result.Add(i);
				}
			}
			return result;
		}

		public double[][] GetMs1SpectrumOnGrid(int index, bool readCentroids, double min, double max, int count,
			int imsIndexMin, int imsIndexMax, double resolution) {
			Spectrum s;
			if (imsIndexMin < 0 || imsIndexMax < 0) {
				s = GetMs1Spectrum(index, readCentroids);
			} else {
				s = GetMs1Spectrum(index, imsIndexMin, imsIndexMax, readCentroids, resolution, min, max);
			}
			return s.Masses == null ? null : CalcBinMinMaxInSpectrum(min, max, count, s);
		}

		private static double[][] CalcBinMinMaxInSpectrum(double minMass, double maxMass, int mzCount, Spectrum s) {
			double[] minVals = new double[mzCount];
			double[] maxVals = new double[mzCount];
			double massStep = (maxMass - minMass) / mzCount;
			int firstIndex = s.GetCeilIndex(minMass);
			if (firstIndex < 0) {
				for (int i = 0; i < mzCount; i++) {
					minVals[i] = double.NaN;
					maxVals[i] = double.NaN;
				}
				return new[] {minVals, maxVals};
			}
			int p = firstIndex;
			for (int index = 0; index < mzCount; index++) {
				double rightBorder = minMass + (index + 1) * massStep;
				double max = double.MinValue;
				double min = double.MaxValue;
				while (p < s.Count && s.GetMass(p) < rightBorder) {
					double yVal = s.GetIntensity(p++);
					if (yVal > max) {
						max = yVal;
					}
					if (yVal < min) {
						min = yVal;
					}
					if (p == s.Count) {
						break;
					}
				}
				if (max == double.MinValue) {
					minVals[index] = double.NaN;
					maxVals[index] = double.NaN;
				} else {
					minVals[index] = min;
					maxVals[index] = max;
				}
			}
			return new[] {minVals, maxVals};
		}

		public double[][] GetTicOnGrid(double min, double max, int count) {
			return CalcBinMinMaxInSpectrum(min, max, count, Ms1Rt, Ms1Tic);
		}

		public static double[][] CalcBinMinMaxInSpectrum(double minX, double maxX, int xCount, double[] sx, double[] sy) {
			if (sx == null) {
				return null;
			}
			double[] minVals = new double[xCount];
			double[] maxVals = new double[xCount];
			double massStep = (maxX - minX) / xCount;
			int firstIndex = ArrayUtils.CeilIndex(sx, minX);
			if (firstIndex < 0) {
				for (int i = 0; i < xCount; i++) {
					minVals[i] = double.NaN;
					maxVals[i] = double.NaN;
				}
				return new[] {minVals, maxVals};
			}
			int p = firstIndex;
			for (int index = 0; index < xCount; index++) {
				double rightBorder = minX + (index + 1) * massStep;
				double max = double.MinValue;
				double min = double.MaxValue;
				while (p < sx.Length && sx[p] < rightBorder) {
					double yVal = sy[p++];
					if (yVal > max) {
						max = yVal;
					}
					if (yVal < min) {
						min = yVal;
					}
					if (p == sx.Length) {
						break;
					}
				}
				if (max == double.MinValue) {
					minVals[index] = double.NaN;
					maxVals[index] = double.NaN;
				} else {
					minVals[index] = min;
					maxVals[index] = max;
				}
			}
			return new[] {minVals, maxVals};
		}

		public Spectrum GetMs1Spectrum(int index, bool readCentroids) {
			GetMs1SpectrumArray(index, readCentroids, out double[] masses, out double[] intensities);
			return new Spectrum(masses, intensities);
		}

		public Spectrum GetMs1Spectrum(int index, int imsIndexMin, int imsIndexMax, bool readCentroids, double resolution,
			double mzMin, double mzMax) {
			GetMs1SpectrumArray(index, imsIndexMin, imsIndexMax, readCentroids, out double[] masses, out double[] intensities,
				resolution, mzMin, mzMax);
			return new Spectrum(masses, intensities);
		}

		public Spectrum GetMs1Spectrum(int indexMin, int indexMax, int imsIndexMin, int imsIndexMax, bool readCentroids,
			double resolution, double mzMin, double mzMax) {
			GetMs1SpectrumArray(indexMin, indexMax, imsIndexMin, imsIndexMax, readCentroids, out double[] masses,
				out double[] intensities, resolution, mzMin, mzMax);
			return new Spectrum(masses, intensities);
		}

		public Spectrum GetMs2Spectrum(int index, bool readCentroids) {
			GetMs2SpectrumArray(index, readCentroids, out double[] masses, out double[] intensities);
			return new Spectrum(masses, intensities);
		}

		public Spectrum GetMs2Spectrum(int index, int imsIndexMin, int imsIndexMax, bool readCentroids, double resoluion,
			double mzMin, double mzMax) {
			GetMs2SpectrumArray(index, imsIndexMin, imsIndexMax, readCentroids, out double[] masses, out double[] intensities,
				resoluion, mzMin, mzMax);
			return new Spectrum(masses, intensities);
		}

		public Spectrum GetMs2Spectrum(int indexMin, int indexMax, int imsIndexMin, int imsIndexMax, bool readCentroids,
			double resolution, double mzMin, double mzMax) {
			GetMs2SpectrumArray(indexMin, indexMax, imsIndexMin, imsIndexMax, readCentroids, out double[] masses,
				out double[] intensities, resolution, mzMin, mzMax);
			return new Spectrum(masses, intensities);
		}

		public Spectrum GetMs3Spectrum(int index, bool readCentroids) {
			GetMs3SpectrumArray(index, readCentroids, out double[] masses, out double[] intensities);
			return new Spectrum(masses, intensities);
		}

		public int[] GetMs2IndToMs1Ind(int scanOffset) {
			int[] ms1ScanNumbers = Ms1ScanNumbers;
			int[] ms2ScanNumbers = Ms2ScanNumbers;
			int[] ms2IndToMs1Ind = new int[ms2ScanNumbers.Length];
			for (int i = 0; i < ms2ScanNumbers.Length; i++) {
				int sn = ms2ScanNumbers[i];
				ms2IndToMs1Ind[i] = Math.Max(0, ArrayUtils.FloorIndex(ms1ScanNumbers, sn) - scanOffset);
			}
			return ms2IndToMs1Ind;
		}

		public int GetClosestMs1IndForMs2Ind(int i) {
			int prevInd = Ms2PrevMs1Index[i];
			if (prevInd == Ms1Count - 1) {
				return prevInd;
			}
			int nextInd = prevInd + 1;
			double rtprev = Ms1Rt[prevInd];
			double rtnext = Ms1Rt[nextInd];
			double rt = Ms2Rt[i];
			return Math.Abs(rtprev - rt) <= Math.Abs(rtnext - rt) ? prevInd : nextInd;
		}

		public int GetPreviousMs1IndForMs2Ind(int i) {
			return Ms2PrevMs1Index[i];
		}

		public int GetScanEventNumberForMs2Ind(int ms2Ind) {
			int ms1Ind = Ms2PrevMs1Index[ms2Ind];
			int ms2ScanNumber = Ms2ScanNumbers[ms2Ind];
			if (ms1Ind >= 0 && ms1Ind < Ms1ScanNumbers.Length) {
				int ms1ScanNumber = Ms1ScanNumbers[ms1Ind];
				return ms2ScanNumber - ms1ScanNumber;
			}
			return -1;
		}

		public void GetMs1DiagnosticData(int index, out double intenseCompFactor, out double emIntenseComp,
			out double rawOvFtT, out double agcFill) {
			intenseCompFactor = index >= 0 && index < Ms1IntenseCompFactor.Length ? Ms1IntenseCompFactor[index] : double.NaN;
			emIntenseComp = index >= 0 && index < Ms1EmIntenseComp.Length ? Ms1EmIntenseComp[index] : double.NaN;
			rawOvFtT = index >= 0 && index < Ms1RawOvFtT.Length ? Ms1RawOvFtT[index] : double.NaN;
			agcFill = index >= 0 && index < Ms1AgcFill.Length ? Ms1AgcFill[index] : double.NaN;
		}

		public void GetMs2DiagnosticData(int index, out double intenseCompFactor, out double emIntenseComp,
			out double rawOvFtT, out double agcFill) {
			intenseCompFactor = index >= 0 && index < Ms2IntenseCompFactor.Length ? Ms2IntenseCompFactor[index] : double.NaN;
			emIntenseComp = index >= 0 && index < Ms2EmIntenseComp.Length ? Ms2EmIntenseComp[index] : double.NaN;
			rawOvFtT = index >= 0 && index < Ms2RawOvFtT.Length ? Ms2RawOvFtT[index] : double.NaN;
			agcFill = index >= 0 && index < Ms2AgcFill.Length ? Ms2AgcFill[index] : double.NaN;
		}

		internal void Write(BinaryWriter writer) {
			FileUtils.Write(Ms1ScanNumbers, writer);
			FileUtils.Write(Ms1NumIms, writer);
			FileUtils.Write(Ms2ScanNumbers, writer);
			FileUtils.Write(Ms2NumIms, writer);
			FileUtils.Write(Ms2PrevMs1Index, writer);
			FileUtils.Write(Ms1Rt, writer);
			FileUtils.Write(Ms2Rt, writer);
			FileUtils.Write(Ms1IonInjectionTimes, writer);
			FileUtils.Write(Ms1BasepeakIntensities, writer);
			FileUtils.Write(Ms2IonInjectionTimes, writer);
			FileUtils.Write(Ms2BasepeakIntensities, writer);
			FileUtils.Write(Ms1ElapsedTimes, writer);
			FileUtils.Write(Ms2ElapsedTimes, writer);
			FileUtils.Write(Ms2Energies, writer);
			FileUtils.Write(Ms2Summations, writer);
			FileUtils.Write(Ms2Mz, writer);
			FileUtils.Write(Ms2IsolationMzMin, writer);
			FileUtils.Write(Ms2IsolationMzMax, writer);
			FileUtils.Write(Ms2MonoisotopicMz, writer);
			FileUtils.Write(massRangesMin, writer);
			FileUtils.Write(Ms1Tic, writer);
			FileUtils.Write(Ms2Tic, writer);
			FileUtils.Write(ms1MinMass, writer);
			FileUtils.Write(ms1MaxMass, writer);
			FileUtils.Write(ms2MinMass, writer);
			FileUtils.Write(ms2MaxMass, writer);
			FileUtils.Write(Ms1IsSim, writer);
			FileUtils.Write(Ms1Resolution, writer);
			FileUtils.Write(Ms1ResolutionValues, writer);
			FileUtils.Write(Ms2Resolution, writer);
			FileUtils.Write(Ms1IntenseCompFactor, writer);
			FileUtils.Write(Ms2IntenseCompFactor, writer);
			FileUtils.Write(Ms1EmIntenseComp, writer);
			FileUtils.Write(Ms2EmIntenseComp, writer);
			FileUtils.Write(Ms1RawOvFtT, writer);
			FileUtils.Write(Ms2RawOvFtT, writer);
			FileUtils.Write(Ms1AgcFill, writer);
			FileUtils.Write(Ms2AgcFill, writer);
			writer.Write(Ms1MassMin);
			writer.Write(Ms1MassMax);
			writer.Write(Ms1MaxNumIms);
			writer.Write(Ms2MassMin);
			writer.Write(Ms2MassMax);
			writer.Write(Ms2MaxNumIms);
			writer.Write(Ms1MassRangeCount);
			writer.Write(MaxIntensity);
			writer.Write(ms2FragmentationTypes.Length);
			foreach (FragmentationTypeEnum t in ms2FragmentationTypes) {
				writer.Write(RawFileUtils.FragmentationTypeEnumToInt(t));
			}
			FileUtils.Write(ms1HasCentroid, writer);
			FileUtils.Write(ms1HasProfile, writer);
			FileUtils.Write(ms2HasCentroid, writer);
			FileUtils.Write(ms2HasProfile, writer);
			writer.Write(ms1Analyzer.Length);
			foreach (MassAnalyzerEnum t in ms1Analyzer) {
				writer.Write(RawFileUtils.MassAnalyzerEnumToInt(t));
			}
			writer.Write(ms2Analyzer.Length);
			foreach (MassAnalyzerEnum t in ms2Analyzer) {
				writer.Write(RawFileUtils.MassAnalyzerEnumToInt(t));
			}
			writer.Write(Ms3MassMin);
			writer.Write(Ms3MassMax);
			FileUtils.Write(Ms3PrevMs2Index, writer);
			FileUtils.Write(Ms3ScanNumbers, writer);
			FileUtils.Write(Ms3Rt, writer);
			FileUtils.Write(Ms3Mz1, writer);
			FileUtils.Write(Ms3Mz2, writer);
			FileUtils.Write(Ms3IsolationMzMin, writer);
			FileUtils.Write(Ms3IsolationMzMax, writer);
			FileUtils.Write(Ms3IonInjectionTimes, writer);
			FileUtils.Write(Ms3BasepeakIntensities, writer);
			FileUtils.Write(Ms3ElapsedTimes, writer);
			FileUtils.Write(Ms3Energies, writer);
			FileUtils.Write(Ms3Summations, writer);
			FileUtils.Write(Ms3MonoisotopicMz, writer);
			FileUtils.Write(Ms3Tic, writer);
			FileUtils.Write(ms3HasCentroid, writer);
			FileUtils.Write(ms3HasProfile, writer);
			FileUtils.Write(ms3MinMass, writer);
			FileUtils.Write(ms3MaxMass, writer);
			FileUtils.Write(Ms3Resolution, writer);
			FileUtils.Write(Ms3IntenseCompFactor, writer);
			FileUtils.Write(Ms3EmIntenseComp, writer);
			FileUtils.Write(Ms3RawOvFtT, writer);
			FileUtils.Write(Ms3AgcFill, writer);
			writer.Write(ms3FragmentationTypes.Length);
			foreach (FragmentationTypeEnum t in ms3FragmentationTypes) {
				writer.Write(RawFileUtils.FragmentationTypeEnumToInt(t));
			}
			writer.Write(ms3Analyzer.Length);
			foreach (MassAnalyzerEnum t in ms3Analyzer) {
				writer.Write(RawFileUtils.MassAnalyzerEnumToInt(t));
			}
			FileUtils.Write(Ms2DependentMs3Inds, writer);
		}

		public void Dispose() {
			Ms1ScanNumbers = null;
			Ms1NumIms = null;
			Ms2PrevMs1Index = null;
			Ms2ScanNumbers = null;
			Ms1Rt = null;
			Ms2Rt = null;
			Ms1IonInjectionTimes = null;
			Ms1ElapsedTimes = null;
			Ms2Mz = null;
			Ms2IsolationMzMin = null;
			Ms2IsolationMzMax = null;
			Ms2IonInjectionTimes = null;
			Ms2ElapsedTimes = null;
			Ms2Energies = null;
			Ms2Summations = null;
			Ms2MonoisotopicMz = null;
			massRangesMin = null;
			ms2FragmentationTypes = null;
			Ms1Tic = null;
			Ms2Tic = null;
			ms1HasCentroid = null;
			ms1HasProfile = null;
			ms2HasCentroid = null;
			ms2HasProfile = null;
			ms1MinMass = null;
			ms1MaxMass = null;
			ms2MinMass = null;
			ms2MaxMass = null;
			Ms1IsSim = null;
			Ms1Resolution = null;
			Ms1ResolutionValues = null;
			Ms2Resolution = null;
			rawFile = null;
		}

		private void SetMs1Data(Ms1Lists ms1Lists) {
			// scalars
			Ms1MassMin = ms1Lists.massMin;
			Ms1MassMax = ms1Lists.massMax;
			Ms1MaxNumIms = Math.Max(1, ms1Lists.maxNumIms);
			// arrays
			Ms1ScanNumbers = ms1Lists.scans.ToArray();
			Ms1NumIms = ms1Lists.nImsScans.ToArray();
			Ms1Rt = ms1Lists.rts.ToArray();
			Ms1IonInjectionTimes = ms1Lists.ionInjectionTimes.ToArray();
			Ms1BasepeakIntensities = ms1Lists.basepeakIntensities.ToArray();
			Ms1ElapsedTimes = ms1Lists.elapsedTimes.ToArray();
			Ms1Tic = ms1Lists.tics.ToArray();
			ms1HasCentroid = ms1Lists.hasCentroids.ToArray();
			ms1HasProfile = ms1Lists.hasProfiles.ToArray();
			ms1Analyzer = ms1Lists.massAnalyzer.ToArray();
			ms1MinMass = ms1Lists.minMasses.ToArray();
			ms1MaxMass = ms1Lists.maxMasses.ToArray();
			Ms1IsSim = ms1Lists.isSim.ToArray();
			Ms1Resolution = ms1Lists.resolutions.ToArray();
			Ms1ResolutionValues = ArrayUtils.UniqueValues(Ms1Resolution);
			if (Ms1ResolutionValues.Length > 2) {
				Ms1ResolutionValues = GetMainResolutionValues(Ms1Resolution);
			}
			Ms1IntenseCompFactor = ms1Lists.intenseCompFactors.ToArray();
			Ms1EmIntenseComp = ms1Lists.emIntenseComp.ToArray();
			Ms1RawOvFtT = ms1Lists.rawOvFtT.ToArray();
			Ms1AgcFill = ms1Lists.agcFillList.ToArray();
			// truncate arrays to portion that is monotonically increasing
			if (!RawFileUtils.IsMonotoneIncreasing(Ms1Rt, out int startPos)) {
				Ms1ScanNumbers = ArrayUtils.SubArrayFrom(Ms1ScanNumbers, startPos);
				Ms1Rt = ArrayUtils.SubArrayFrom(Ms1Rt, startPos);
				Ms1IonInjectionTimes = ArrayUtils.SubArrayFrom(Ms1IonInjectionTimes, startPos);
				Ms1BasepeakIntensities = ArrayUtils.SubArrayFrom(Ms1BasepeakIntensities, startPos);
				Ms1ElapsedTimes = ArrayUtils.SubArrayFrom(Ms1ElapsedTimes, startPos);
				Ms1Tic = ArrayUtils.SubArrayFrom(Ms1Tic, startPos);
				ms1HasCentroid = ArrayUtils.SubArrayFrom(ms1HasCentroid, startPos);
				ms1HasProfile = ArrayUtils.SubArrayFrom(ms1HasProfile, startPos);
				ms1Analyzer = ArrayUtils.SubArrayFrom(ms1Analyzer, startPos);
				ms1MinMass = ArrayUtils.SubArrayFrom(ms1MinMass, startPos);
				ms1MaxMass = ArrayUtils.SubArrayFrom(ms1MaxMass, startPos);
				Ms1Resolution = ArrayUtils.SubArrayFrom(Ms1Resolution, startPos);
				Ms1IntenseCompFactor = ArrayUtils.SubArrayFrom(Ms1IntenseCompFactor, startPos);
				Ms1EmIntenseComp = ArrayUtils.SubArrayFrom(Ms1EmIntenseComp, startPos);
				Ms1RawOvFtT = ArrayUtils.SubArrayFrom(Ms1RawOvFtT, startPos);
				Ms1AgcFill = ArrayUtils.SubArray(Ms1AgcFill, startPos);
			}
		}

		private static double[] GetMainResolutionValues(double[] ms1Resolution) {
			Dictionary<double, int> counts = new Dictionary<double, int>();
			foreach (double f in ms1Resolution) {
				if (!counts.ContainsKey(f)) {
					counts.Add(f, 0);
				}
				counts[f]++;
			}
			double[] keys = counts.Keys.ToArray();
			int[] values = new int[keys.Length];
			for (int i = 0; i < keys.Length; i++) {
				values[i] = counts[keys[i]];
			}
			int[] o = ArrayUtils.Order(values);
			Array.Reverse(o);
			double[] result = {keys[o[0]], keys[o[1]]};
			Array.Sort(result);
			return result;
		}

		private void SetMs2Data(Ms2Lists ms2Lists) {
			Ms2MassMin = ms2Lists.massMin;
			Ms2MassMax = ms2Lists.massMax;
			Ms2MaxNumIms = Math.Max(1, ms2Lists.maxNumIms);
			Ms2PrevMs1Index = ms2Lists.prevMs1IndexList.ToArray();
			Ms2ScanNumbers = ms2Lists.scansList.ToArray();
			Ms2NumIms = ms2Lists.nImsScans.ToArray();
			Ms2Rt = ms2Lists.rtList.ToArray();
			Ms2Mz = ms2Lists.mzList.ToArray();
			Ms2IsolationMzMin = ms2Lists.isolationMzMinList.ToArray();
			Ms2IsolationMzMax = ms2Lists.isolationMzMaxList.ToArray();
			ms2FragmentationTypes = ms2Lists.fragmentTypeList.ToArray();
			Ms2IonInjectionTimes = ms2Lists.ionInjectionTimesList.ToArray();
			Ms2BasepeakIntensities = ms2Lists.basepeakIntensityList.ToArray();
			Ms2ElapsedTimes = ms2Lists.elapsedTimesList.ToArray();
			Ms2Energies = ms2Lists.energiesList.ToArray();
			Ms2Summations = ms2Lists.summationsList.ToArray();
			Ms2MonoisotopicMz = ms2Lists.monoisotopicMzList.ToArray();
			Ms2Tic = ms2Lists.ticList.ToArray();
			ms2HasCentroid = ms2Lists.hasCentroidList.ToArray();
			ms2HasProfile = ms2Lists.hasProfileList.ToArray();
			ms2Analyzer = ms2Lists.analyzerList.ToArray();
			ms2MinMass = ms2Lists.minMassList.ToArray();
			ms2MaxMass = ms2Lists.maxMassList.ToArray();
			Ms2Resolution = ms2Lists.resolutionList.ToArray();
			Ms2IntenseCompFactor = ms2Lists.intenseCompFactor.ToArray();
			Ms2EmIntenseComp = ms2Lists.emIntenseComp.ToArray();
			Ms2RawOvFtT = ms2Lists.rawOvFtT.ToArray();
			Ms2AgcFill = ms2Lists.agcFillList.ToArray();
		}

		private void SetMs3Data(Ms3Lists ms3Lists) {
			Ms3MassMin = ms3Lists.massMin;
			Ms3MassMax = ms3Lists.massMax;
			Ms3PrevMs2Index = ms3Lists.prevMs2IndexList.ToArray();
			Ms2DependentMs3Inds = CreateMs2DependentMs3Inds(Ms3PrevMs2Index, Ms2Mz.Length);
			Ms3ScanNumbers = ms3Lists.scansList.ToArray();
			Ms3Rt = ms3Lists.rtList.ToArray();
			Ms3Mz1 = ms3Lists.mz1List.ToArray();
			Ms3Mz2 = ms3Lists.mz2List.ToArray();
			Ms3IsolationMzMin = ms3Lists.isolationMzMinList.ToArray();
			Ms3IsolationMzMax = ms3Lists.isolationMzMaxList.ToArray();
			ms3FragmentationTypes = ms3Lists.fragmentTypeList.ToArray();
			Ms3IonInjectionTimes = ms3Lists.ionInjectionTimesList.ToArray();
			Ms3BasepeakIntensities = ms3Lists.basepeakIntensityList.ToArray();
			Ms3ElapsedTimes = ms3Lists.elapsedTimesList.ToArray();
			Ms3Energies = ms3Lists.energiesList.ToArray();
			Ms3Summations = ms3Lists.summationsList.ToArray();
			Ms3MonoisotopicMz = ms3Lists.monoisotopicMzList.ToArray();
			Ms3Tic = ms3Lists.ticList.ToArray();
			ms3HasCentroid = ms3Lists.hasCentroidList.ToArray();
			ms3HasProfile = ms3Lists.hasProfileList.ToArray();
			ms3Analyzer = ms3Lists.analyzerList.ToArray();
			ms3MinMass = ms3Lists.minMassList.ToArray();
			ms3MaxMass = ms3Lists.maxMassList.ToArray();
			Ms3Resolution = ms3Lists.resolutionList.ToArray();
			Ms3IntenseCompFactor = ms3Lists.intenseCompFactor.ToArray();
			Ms3EmIntenseComp = ms3Lists.emIntenseComp.ToArray();
			Ms3RawOvFtT = ms3Lists.rawOvFtT.ToArray();
			Ms3AgcFill = ms3Lists.agcFillList.ToArray();
		}

		private static int[][] CreateMs2DependentMs3Inds(int[] ms3PrevMs2Index, int n2) {
			if (ms3PrevMs2Index.Length == 0) {
				int[][] result = new int[n2][];
				for (int i = 0; i < n2; i++) {
					result[i] = new int[0];
				}
				return result;
			}
			List<int>[] x = new List<int>[n2];
			for (int i = 0; i < x.Length; i++) {
				x[i] = new List<int>();
			}
			for (int i = 0; i < ms3PrevMs2Index.Length; i++) {
				if (ms3PrevMs2Index[i] >= 0) {
					x[ms3PrevMs2Index[i]].Add(i);
				}
			}
			int[][] res = new int[x.Length][];
			for (int i = 0; i < res.Length; i++) {
				res[i] = x[i].ToArray();
			}
			return res;
		}

		internal void SetData(InfoLists infoLists, double maxIntensity) {
			SetMs1Data(infoLists.ms1Lists);
			SetMs2Data(infoLists.ms2Lists);
			SetMs3Data(infoLists.ms3Lists);
			Ms1MassRangeCount = infoLists.allMassRanges.Count;
			MaxIntensity = maxIntensity;
			massRangesMin = new double[infoLists.allMassRanges.Count];
			infoLists.allMassRanges.Keys.CopyTo(massRangesMin, 0);
			Array.Sort(massRangesMin);
		}

		public bool[] GetMs1SelectionOnGrid(double minRt, double rtStep, int rtCount, int selectedMs1Index) {
			bool[] values = new bool[rtCount];
			for (int i = 0; i < rtCount; i++) {
				double rt = minRt + rtStep * i;
				int index = GetMs1IndexFromRt(rt);
				if (index == -1) {
					values[i] = false;
				} else {
					int i1 = GetMs1CeilIndexFromRt(rt);
					int i2 = GetMs1FloorIndexFromRt((rt + rtStep));
					if (i1 >= i2) {
						values[i] = index == selectedMs1Index;
					} else {
						bool result = false;
						for (int ii = i1; ii <= i2; ii++) {
							result = result || ii == selectedMs1Index;
						}
						values[i] = result;
					}
				}
			}
			return values;
		}

		public bool[] GetMs2SelectionOnGrid(double minRt, double rtStep, int rtCount, int selectedMs2Index) {
			bool[] values = new bool[rtCount];
			for (int i = 0; i < rtCount; i++) {
				double rt = minRt + rtStep * i;
				int index = GetMs2IndexFromRt(rt);
				if (index == -1) {
					values[i] = false;
				} else {
					int i1 = GetMs2CeilIndexFromRt(rt);
					int i2 = GetMs2FloorIndexFromRt((rt + rtStep));
					if (i1 >= i2) {
						values[i] = index == selectedMs2Index;
					} else {
						bool result = false;
						for (int ii = i1; ii <= i2; ii++) {
							result = result || ii == selectedMs2Index;
						}
						values[i] = result;
					}
				}
			}
			return values;
		}

		public MassAnalyzerEnum GetMasterMassAnalyzer(int index) {
			int ms1Index = Math.Max(0, ArrayUtils.FloorIndex(Ms1ScanNumbers, Ms2ScanNumbers[index]));
			return GetMs1MassAnalyzer(ms1Index);
		}

		public int[] GetMs1IndsForResolution(int resInd) {
			if (Ms1ResolutionValues.Length == 0) {
				return new int[0];
			}
			double res = Ms1ResolutionValues[resInd];
			List<int> result = new List<int>();
			for (int i = 0; i < Ms1Resolution.Length; i++) {
				if (Ms1Resolution[i] == res) {
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public int[][] GetConsecutiveSimScans() {
			List<int[]> result = new List<int[]>();
			bool inRegion = false;
			int start = -1;
			for (int i = 0; i < Ms1IsSim.Length; i++) {
				if (Ms1IsSim[i]) {
					if (!inRegion) {
						start = i;
						inRegion = true;
					}
				} else {
					if (inRegion) {
						result.Add(ArrayUtils.ConsecutiveInts(start, i));
						inRegion = false;
					}
				}
			}
			if (inRegion) {
				result.Add(ArrayUtils.ConsecutiveInts(start, Ms1IsSim.Length));
			}
			return result.ToArray();
		}

		public int PasefMsmsCount => positive ? rawFile.PasefMsmsCount : 0;

		public PasefFrameMsMsInfo GetPasefMsmsInfo(int index) {
			return positive ? rawFile.GetPasefMsmsInfo(index) : null;
		}

		public int PasefPrecursorCount => positive ? rawFile.PasefPrecursorCount : 0;

		public PasefPrecursorInfo GetPasefPrecursorInfo(int index) {
			return positive ? rawFile.GetPasefPrecursorInfo(index) : null;
		}

		public void GetDiaRanges(out double[] mzMin, out double[] mzMax) {
			int[][] inds = GetDiaMs2Indices();
			mzMin = new double[inds.Length];
			mzMax = new double[inds.Length];
			for (int i = 0; i < inds.Length; i++) {
				mzMin[i] = Ms2IsolationMzMin[inds[i][0]];
				mzMax[i] = Ms2IsolationMzMax[inds[i][0]];
			}
		}

		public int[][] GetDiaMs2Indices() {
			if (Ms2Count == 0) {
				return new int[0][];
			}
			double d = Ms2IsolationMzMin[0];
			int len = 1;
			while (Ms2IsolationMzMin[len] != d) {
				len++;
			}
			int[][] result = new int[len][];
			int len2 = Ms2Count / len;
			for (int i = 0; i < len; i++) {
				result[i] = new int[len2];
				for (int j = 0; j < len2; j++) {
					result[i][j] = i + len * j;
				}
			}
			return result;
		}
	}
}