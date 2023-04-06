using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using BaseLibS.Ms;
using BaseLibS.Ms.Instrument;
using BaseLibS.Num;
using BaseLibS.Util;
using zlib;

namespace PluginRawMzMl{
	/// <summary>
	/// Implementation of the indexed .mzml raw file format. Call <see cref="Dispose"/> when
	/// not used anymore!
	/// </summary>
	/// <remarks>
	/// The implementation relies on the automatically generated mzML1_1_1_idx.cs code
	/// and tries to avoid manual xml parsing.
	///
	/// The aim of the implementation was to have a low memory footprint, therefore
	/// not the entire mzml is parsed at once but rather only parts are parsed
	/// and spectra are read from file on demand.
	/// </remarks>
	public class MzMLRawFile : RawFile{
		private const float nsigma = 3;
		private MzGrid mzGrid;
		private OffsetType[] offset;
		private Dictionary<string, InstrumentConfigurationType> instrumentConfigurations;
		private string defaultInstrumentConfigurationRef;

		/// <summary>
		/// Open reader to <see cref="RawFile.Path"/>.
		/// Should not be closed manually!
		/// </summary>
		private StreamReader reader;

		public override string Suffix => ".mzML";
		public override bool IsFolderBased => false;
		public override string Name => "mzML";
		public override bool NeedsIsolationWindow => false;
		public override bool NeedsBackgroundSubtraction => false;
		public override MsInstrument DefaultInstrument => MsInstruments.watersTof;
		public override bool IsInstalled => true;
		public override bool NeedsGrid => false;
		public override bool HasIms => false;
		public override string InstallMessage => "No installation necessary";

		/// <remarks>
		///	There seems to be no 'scan number' in mzML.
		/// Scan numbers are therefore assigned by indexing the <see cref="RunType.spectrumList"/>.
		/// This only makes sense if there is only a single scan in <see cref="SpectrumType.scanList"/>.
		/// </remarks>
		public override int FirstScanNumber => 0;

		public override int LastScanNumber{
			get{
				if (!preInitialized){
					PreInit();
				}
				return offset.Length - 1;
			}
		}

		/// <summary>
		/// Pre-initialize the raw file. Should be called before accessing any fields, properties or methods.
		/// 
		/// 1. Look for <code>indexListOffset</code> tag
		/// 2. Seek indexList
		/// 3. Deserialize indexList
		/// </summary>
		protected override void PreInit(){
			preInitialized = true;
			reader = File.OpenText(Path);
			PreInitOffset();
			reader.DiscardBufferedData();
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			using (XmlReader xml = XmlReader.Create(reader)){
				// This might take longer than expected if <instrumentConfigurationList> is at the end of file rather than beginning.
				xml.ReadToDescendant(Xml.InstrumentConfigurationListElementName);
				InstrumentConfigurationListType instrumentConfigurationList =
					(InstrumentConfigurationListType) Xml.InstrumentConfigurationListSerializer.Deserialize(xml);
				instrumentConfigurations =
					instrumentConfigurationList.instrumentConfiguration.ToDictionary(config => config.id,
						config => config);
				xml.ReadToFollowing(Xml.RunElementName);
				defaultInstrumentConfigurationRef =
					xml.GetAttribute(Xml.DefaultInstrumentConfigurationRefAttributeName);
			}
		}

		public override void Dispose(){
			base.Dispose();
			reader?.Dispose();
		}

		/// <summary>
		/// Initialize <see cref="offset"/>.
		/// </summary>
		private void PreInitOffset(){
			FileInfo offsetIndexFile = new FileInfo(Path + ".offsets");
			if (offsetIndexFile.Exists){
				using (StreamReader sr = new StreamReader(offsetIndexFile.OpenRead())){
					IndexType indexType = (IndexType) Xml.IndexTypeSerializer.Deserialize(sr);
					offset = indexType.offset;
				}
			} else{
				IndexListType indexList;
				// Try unsafe
				long indexListOffset = FindIndexListOffsetUnsafe();
				if (indexListOffset >= 0){
					indexList = ReadIndexList(indexListOffset);
				} else // Fall back to safe
				{
					indexListOffset = FindIndexListOffsetSafe();
					indexList = ReadIndexList(indexListOffset);
				}
				OffsetType[] preOffset = indexList.index?.SingleOrDefault(index => index.name == IndexTypeName.spectrum)
					?.offset;
				if (preOffset == null){
					throw new Exception(
						$"mzML rawfile {System.IO.Path.GetFileNameWithoutExtension(Path)} did not contain {nameof(IndexTypeName.spectrum)} index.");
				}
				offset = preOffset;
				//offset = FilterOffset(preOffset);
				using (StreamWriter sw = new StreamWriter(offsetIndexFile.OpenWrite())){
					Xml.IndexTypeSerializer.Serialize(sw,
						new IndexType(){name = IndexTypeName.spectrum, offset = offset});
				}
			}
		}

		/// <summary>
		/// Align DIA ranges
		/// TODO this is temporary solution
		/// </summary>
		/// <param name="preOffset"></param>
		/// <returns></returns>
		private OffsetType[] FilterOffset(OffsetType[] preOffset){
			List<int> ms1Scans = new List<int>();
			List<int> ms2Scans = new List<int>();
			List<int> ms2Toms1 = new List<int>();
			List<double> ms2IsolationMins = new List<double>();
			List<double> ms2IsolationMaxs = new List<double>();
			for (int i = 0; i < preOffset.Length; i++){
				SpectrumType spectrum = DeserializeSpectrum(preOffset[i]);
				Dictionary<string, string> parameters = Parameters(spectrum);
				MsLevel msLevel = MsLevel(parameters);
				switch (msLevel){
					case BaseLibS.Ms.MsLevel.Ms1:
						ms1Scans.Add(i);
						break;
					case BaseLibS.Ms.MsLevel.Ms2:
						ms2Scans.Add(i);
						ms2Toms1.Add(ms1Scans.Count - 1);
						if (spectrum.precursorList != null){
							PrecursorType precursor = spectrum.precursorList.precursor.Single();
							Dictionary<string, string> isolationWindowParameters = Parameters(precursor.isolationWindow);
							double ms2ParentMz = Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_TARGET_M_Z]);
							double dm = isolationWindowParameters.ContainsKey(CV.ISOLATION_WINDOW_LOWER_OFFSET) ? 
								Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_LOWER_OFFSET]) : 1.5;
							ms2IsolationMins.Add(ms2ParentMz - dm);
							double dp = isolationWindowParameters.ContainsKey(CV.ISOLATION_WINDOW_UPPER_OFFSET) ? 
								Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_UPPER_OFFSET]) : 1.5;
							ms2IsolationMaxs.Add(ms2ParentMz + dp);
						}
						break;
					case BaseLibS.Ms.MsLevel.Ms3:
						throw new NotImplementedException();
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			Dictionary<Tuple<double, double>, int> cnt = new Dictionary<Tuple<double, double>, int>();
			for (int i = 0; i < ms2Scans.Count; i++){
				Tuple<double, double> key = new Tuple<double, double>(ms2IsolationMins[i], ms2IsolationMaxs[i]);
				if (cnt.ContainsKey(key)){
					cnt[key]++;
				} else{
					cnt.Add(key, 1);
				}
			}
			int n = ms1Scans.Count;
			int m = 0;
			Dictionary<Tuple<double, double>, int> rangeIndex = new Dictionary<Tuple<double, double>, int>();
			foreach (KeyValuePair<Tuple<double, double>, int> kv in cnt.OrderBy(x => x.Key)){
				if (kv.Value >= ms1Scans.Count){
					rangeIndex[kv.Key] = m++;
				}
			}
			int[,] indexes = new int[n, m];
			for (int i = 0; i < ms2Scans.Count; i++){
				Tuple<double, double> key = new Tuple<double, double>(ms2IsolationMins[i], ms2IsolationMaxs[i]);
				if (cnt[key] >= ms1Scans.Count){
					indexes[ms2Toms1[i], rangeIndex[key]] = ms2Scans[i];
				}
			}

			// If one is missing, we are copying the fake one from neighbor
			// This is the best from worst decision!
			for (int i = 0; i < n; i++){
				for (int j = 0; j < m; j++){
					if (indexes[i, j] == 0){
						int d = 1;
						while (i - d >= 0 || i + d < n){
							if (i - d >= 0 && indexes[i - d, j] != 0){
								indexes[i, j] = indexes[i - d, j];
								break;
							}
							if (i + d < n && indexes[i + d, j] != 0){
								indexes[i, j] = indexes[i + d, j];
								break;
							}
							d++;
						}
						if (i - d < 0 && i + d >= n)
							throw new Exception("Hopeless data - no way to patch it");
					}
				}
			}
			OffsetType[] offset1 = new OffsetType[n * m + ms1Scans.Count];
			int ims1 = 0;
			int ims2 = 0;
			while ((ims1 + ims2) != offset1.Length){
				offset1[ims1 + ims2] = preOffset[ms1Scans[ims1]];
				ims1++;
				for (int i = 0; i < m; i++, ims2++){
					offset1[ims1 + ims2] = preOffset[indexes[ims1 - 1, i]];
				}
			}
			return offset1;
		}

		/// <summary>
		/// Seek the location of the indexList according to the offset and deserialize the list.
		/// </summary>
		private IndexListType ReadIndexList(long indexListOffset){
			reader.DiscardBufferedData();
			reader.BaseStream.Seek(indexListOffset, SeekOrigin.Begin);
			using (XmlReader xml = MzmlXmlReader(reader)){
				xml.Read();
				IndexListType indexList = (IndexListType) Xml.IndexListSerializer.Deserialize(xml);
				return indexList;
			}
		}

		/// <summary>
		/// Create XmlReader suitable for reading from the middle of the file.
		/// The mzml namespace is added manually.
		/// </summary>
		private XmlReader MzmlXmlReader(TextReader reader1){
			XmlReaderSettings settings = new XmlReaderSettings{NameTable = new NameTable()};
			XmlNamespaceManager xmlns = new XmlNamespaceManager(settings.NameTable);
			xmlns.AddNamespace("", "http://psi.hupo.org/ms/mzml");
			XmlParserContext context = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
			return XmlReader.Create(reader1, settings, context);
		}

		/// <summary>
		/// Find index list offset in an unsafe way by reading from the end of the file
		/// and manually parsing the xml tags.
		///
		/// This is hopefully faster than <see cref="FindIndexListOffsetSafe"/>.
		/// </summary>
		private long FindIndexListOffsetUnsafe(){
			Regex target = new Regex(@"<indexListOffset>(?<indexListOffset>\d+)</indexListOffset>");
			Regex wentTooFar = new Regex("</indexList>");
			using (StreamBackwardReader reader1 = new StreamBackwardReader(Path)){
				foreach (string line in reader1.ReadLines()){
					Match match = target.Match(line);
					if (match.Success){
						return long.TryParse(match.Groups["indexListOffset"].Value, out long indexListOffset)
							? indexListOffset
							: -1;
					}
					if (wentTooFar.IsMatch(line)){
						return -1;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Find index list offset in a safe manner using <see cref="XmlReader"/>.
		///
		/// This is hopefully safer than <see cref="FindIndexListOffsetUnsafe"/>.
		/// </summary>
		private long FindIndexListOffsetSafe(){
			long indexListOffset;
			reader.DiscardBufferedData();
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			using (XmlReader xml = XmlReader.Create(reader)){
				bool isIndexedMzML = xml.ReadToFollowing(Xml.IndexedmzMLElementName);
				if (!isIndexedMzML){
					throw new ArgumentException(
						$"mzML without index not supported. Could not find 'indexedmzML' tag in {Path}.");
				}
				bool hasOffset = xml.ReadToDescendant(Xml.IndexListOffsetElementName);
				if (!hasOffset){
					throw new ArgumentException(
						$"Could not find indexListOffset tag required by indexed mzML standard in {Path}.");
				}
				XmlReader indexListReader = xml.ReadSubtree();
				long? maybeIndexList = (long?) Xml.IndexListOffsetSerializer.Deserialize(indexListReader);
				if (!maybeIndexList.HasValue){
					throw new ArgumentException(
						$"Could not parse indexListOffset required by indexed mzML standard in {Path}.");
				}
				indexListOffset = maybeIndexList.Value;
			}
			return indexListOffset;
		}
		private static readonly bool isSpecial = true;
		public override void GetSpectrum(int scanNumberMin, int scanNumberMax, int imsIndexMin, int imsIndexMax,
			bool readCentroids, out double[] masses, out float[] intensities, double resolution, double gridSpacing, double mzMin,
			double mzMax, bool isMs1){
			if (!preInitialized){
				PreInit();
			}
			if (scanNumberMin != scanNumberMax){
				throw new NotImplementedException("TODO: No idea what to do for a range of scan numbers.");
			}
			int scanNumber = scanNumberMin;
			SpectrumType spectrum = DeserializeSpectrum(scanNumber);
			if (int.TryParse(spectrum.scanList?.count, out int scanListCount) && scanListCount > 1){
				throw new ArgumentException(
					$"Unsupported mzml feature: Found more than one {nameof(ScanListType)} in {nameof(SpectrumType)} {spectrum.id}.");
			}
			BinaryDataArrayType[] binary = spectrum.binaryDataArrayList.binaryDataArray;
			List<Dictionary<string, string>> binaryParameters = binary.Select(Parameters).ToList();
			double[] massesIn = FromBinaryArray(CV.M_Z_ARRAY, binary, binaryParameters);
			double[] intensitiesIn;
			if (isSpecial){
				intensitiesIn = FromBinaryArrayIntensities(CV.INTENSITY_ARRAY, binary, binaryParameters);
			} else {
				intensitiesIn = FromBinaryArray(CV.INTENSITY_ARRAY, binary, binaryParameters);
			}
			resolution = 25000;
			if (massesIn.Length == 0){
				masses = new double[0];
				intensities = new float[0];
				return;
			}
			if (mzGrid == null){
				mzGrid = new MzGrid(massesIn.Min() - 1, massesIn.Max() + 1, resolution, nsigma, 0.5);
			} else{
				if (mzGrid.resolution != resolution){
					throw new Exception($"Invalid grid resolution: {resolution}, " +
					                    $"grid is already initialized with resolution: {mzGrid.resolution}");
				}
			}
			mzGrid.SmoothIntensities(massesIn, ArrayUtils.ToFloats(intensitiesIn) , out masses, out intensities);
		}
		
		public override IntSpectrum[] GetSpectrum(int scanNumberMin, int scanNumberMax, int[] imsIndexMin, int[] imsIndexMax,
			bool readCentroids){
			throw new NotImplementedException();
		}

		public override Spectrum GetSpectrum(IntSpectrum s, double resolution, double gridSpacing){
			throw new NotImplementedException();
		}

		private SpectrumType DeserializeSpectrum(int scanNumber){
			return DeserializeSpectrum(offset[scanNumber]);
		}

		private SpectrumType DeserializeSpectrum(OffsetType offset){
			reader.DiscardBufferedData();
			reader.BaseStream.Seek(offset.Value, SeekOrigin.Begin);
			using (XmlReader xml = MzmlXmlReader(reader)){
				xml.Read();
				SpectrumType spectrum = (SpectrumType) Xml.SpectrumSerializer.Deserialize(xml);
				Debug.Assert(offset.idRef.Equals(spectrum.id));
				return spectrum;
			}
		}

		/// <summary>
		/// Check if the parameter dictionary contains at least on CV term associated with ms-numpress <see cref="CV.NUMPRESS_ALL"/>.
		/// </summary>
		private static bool TryGetNumpressCompressionType(Dictionary<string, string> parameters,
			out string compressionType){
			compressionType = CV.NUMPRESS_ALL.SingleOrDefault(parameters.ContainsKey);
			return !string.IsNullOrEmpty(compressionType);
		}
		/// <summary>
		/// Decompress binary array with the given name using the appropriate compression algorithm.
		/// </summary>
		private static double[] FromBinaryArray(string name, BinaryDataArrayType[] binary,
			List<Dictionary<string, string>> binaryParameters) {
			int index = binaryParameters.FindIndex(parameters => parameters.ContainsKey(name));
			if (index < 0) {
				throw new ArgumentException(
					$"Could not find matching {nameof(BinaryDataArrayType)} with name '{name}'.");
			}
			byte[] binaryArray = binary[index].binary;
			Dictionary<string, string> mzParameters = binaryParameters[index];
			bool isNumpress = TryGetNumpressCompressionType(mzParameters, out string numpressCompressionType);
			if (isNumpress) {
				return MsNumpress.Decode(numpressCompressionType, binaryArray, binaryArray.Length);
			}
			bool isZlib = mzParameters.ContainsKey(CV.ZLIB_COMPRESSION);
			if (isZlib) {
				byte[] deflated = DecompressZlib(binaryArray);
				return FromUncompressedByteArray(mzParameters, deflated);
			}
			bool isNoCompression = mzParameters.ContainsKey(CV.NO_COMPRESSION);
			if (isNoCompression) {
				return FromUncompressedByteArray(mzParameters, binaryArray);
			}
			throw new Exception($"Could not identify compression type of {name}.");
		}
		private static double[] FromBinaryArrayIntensities(string name, BinaryDataArrayType[] binary,
			List<Dictionary<string, string>> binaryParameters) {
			int index = binaryParameters.FindIndex(parameters => parameters.ContainsKey(name));
			if (index < 0) {
				throw new ArgumentException(
					$"Could not find matching {nameof(BinaryDataArrayType)} with name '{name}'.");
			}
			byte[] binaryArray = binary[index].binary;
			return ArrayUtils.ToDoubles(MzMlReader.ReadBinaryArray(binaryArray, false, 32));
		}

		/// <summary>
		/// Decompress the binary array using zlib.
		/// </summary>
		private static byte[] DecompressZlib(byte[] binaryArray){
			byte[] deflated;
			using (MemoryStream memory = new MemoryStream(binaryArray))
			using (ZOutputStream zOutputStream = new ZOutputStream(memory))
			using (MemoryStream result = new MemoryStream()){
				byte[] buffer = new byte[2000];
				int len;
				while ((len = zOutputStream.Read(buffer, 0, 2000)) > 0){
					result.Write(buffer, 0, len);
				}
				result.Flush();
				zOutputStream.finish();
				deflated = result.ToArray();
			}
			return deflated;
		}

		/// <summary>
		/// Convert the uncompressed byte array into an array of doubles according to the data type
		/// specified in the parameters, e.g. <see cref="CV.FLOAT_64_BIT"/>.
		/// </summary>
		private static double[] FromUncompressedByteArray(Dictionary<string, string> mzParameters, byte[] binaryArray){
			int bytesUsed;
			if (mzParameters.ContainsKey(CV.FLOAT_64_BIT)){
				bytesUsed = 8;
			} else if (mzParameters.ContainsKey(CV.FLOAT_32_BIT)){
				bytesUsed = 4;
			} else{
				throw new ArgumentException(
					$"Cannot read {nameof(BinaryDataArrayType)}. Could not identify known data type (e.g. 64-bit float).");
			}
			double[] values = new double[binaryArray.Length / bytesUsed];
			Buffer.BlockCopy(binaryArray, 0, values, 0, binaryArray.Length);
			return values;
		}

		protected override double[] Index2K0(int scanNumber, double[] imsInds){
			// ion-mobility only...
			return null;
		}

		protected override ScanInfo GetInfoForScanNumber(int scanNumber){
			if (!preInitialized){
				PreInit();
			}
			if (scanNumber >= Convert.ToInt32(offset.Length)){
				throw new ArgumentException();
			}
			SpectrumType spectrum = DeserializeSpectrum(scanNumber);
			Dictionary<string, string> parameters = Parameters(spectrum);
			MsLevel msLevel = MsLevel(parameters, scanNumber);
			ScanListType scanList = spectrum.scanList;
			Dictionary<string, string> scanListParameters = Parameters(scanList);
			if (!scanListParameters.ContainsKey(CV.NO_COMBINATION)){
				throw new NotImplementedException(
					$"{nameof(spectrum.scanList)} should have 'no combination' parameter");
			}
			ScanType scan = spectrum.scanList.scan.Single();
			Dictionary<string, string> scanParameters = Parameters(scan);
			MassAnalyzerEnum analyzer = MassAnalyzer(scanNumber, scan);
			ScanInfo scanInfo = new ScanInfo{
				positiveIonMode = parameters.ContainsKey(CV.POSITIVE_SCAN),
				msLevel = msLevel,
				basepeakIntensity = Convert.ToDouble(parameters[CV.BASE_PEAK_INTENSITY]),
				tic = Convert.ToDouble(parameters[CV.TOTAL_ION_CURRENT]),
				//hasProfile = parameters.ContainsKey(CV.PROFILE_SPECTRUM),
				//hasCentroid = parameters.ContainsKey(CV.CENTROID_SPECTRUM),
				hasProfile = true,
				hasCentroid = false,
				analyzer = analyzer,
				min = GetParameterOrNaN(CV.LOWEST_OBSERVED_M_Z, parameters),
				max = GetParameterOrNaN(CV.HIGHEST_OBSERVED_M_Z, parameters),
				resolution = 30000, // TODO just taken from internal SciexWiffRawFile implementation
				isSim = false, // TODO just taken from internal SciexWiffRawFile implementation
				elapsedTime = 1, // TODO just taken from internal SciexWiffRawFile implementation
				// TODO diagnostic data
				agcFill = double.NaN,
				intenseCompFactor = double.NaN,
				emIntenseComp = double.NaN,
				rawOvFtT = double.NaN,
				rt = Convert.ToDouble(scanParameters[CV.SCAN_START_TIME]),
			};
			if (double.IsNaN(scanInfo.min) || double.IsNaN(scanInfo.max)){
				BinaryDataArrayType[] binary = spectrum.binaryDataArrayList.binaryDataArray;
				List<Dictionary<string, string>> binaryParameters = binary.Select(Parameters).ToList();
				double[] masses = FromBinaryArray(CV.M_Z_ARRAY, binary, binaryParameters);
				ArrayUtils.MinMax(masses, out double min, out double max);
				scanInfo.min = min;
				scanInfo.max = max;
			}
			if (spectrum.precursorList != null){
				PrecursorType precursor = spectrum.precursorList.precursor.Single();
				Dictionary<string, string> activationParameters = Parameters(precursor.activation);
				scanInfo.energy = GetParameterOrNaN(CV.COLLISION_ENERGY, activationParameters);
				FragmentationTypeEnum fragmentationType;
				if (activationParameters.ContainsKey(CV.COLLISION_INDUCED_DISSOCIATION)){
					fragmentationType = FragmentationTypeEnum.Cid;
				} else if (activationParameters.ContainsKey(CV.BEAM_TYPE_COLLISION_INDUCED_DISSOCIATION)){
					fragmentationType = FragmentationTypeEnum.Hcd;
				} else{
					throw new ArgumentException($"Could not identify fragmentation type for spectrum {scanNumber}.");
				}
				scanInfo.fragType = fragmentationType;
				Dictionary<string, string> isolationWindowParameters = Parameters(precursor.isolationWindow);
				scanInfo.ms2MonoMz = double.NaN; // TODO taken from internal SciexWiffRawFile implementation
				scanInfo.ms2ParentMz = Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_TARGET_M_Z]);
				double dm = isolationWindowParameters.ContainsKey(CV.ISOLATION_WINDOW_LOWER_OFFSET)
					? Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_LOWER_OFFSET])
					: 1.5;
				scanInfo.ms2IsolationMin = scanInfo.ms2ParentMz - dm;
				double dp = isolationWindowParameters.ContainsKey(CV.ISOLATION_WINDOW_UPPER_OFFSET)
					? Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_UPPER_OFFSET])
					: 1.5;
				scanInfo.ms2IsolationMax = scanInfo.ms2ParentMz + dp;
			}
			return scanInfo;
		}

		/// <summary>
		/// Get parameter by name and try to parse it as double. Otherwise return NaN.
		/// </summary>
		private static double GetParameterOrNaN(string name, Dictionary<string, string> parameters){
			if (!parameters.TryGetValue(name, out string value)){
				value = "NaN";
			}
			return Convert.ToDouble(value);
		}

		/// <summary>
		/// Read mass analyzer from scan configuration or default to run configuration.
		/// If more than one analyzer is defined in <see cref="InstrumentConfigurationListType.instrumentConfiguration"/>,
		/// the last one is assumed to be the one that produced the spectrum.
		/// </summary>
		private MassAnalyzerEnum MassAnalyzer(int scanNumber, ScanType scan){
			string instrumentConfigurationRef = scan.instrumentConfigurationRef ?? defaultInstrumentConfigurationRef;
			InstrumentConfigurationType instrumentConfiguration = instrumentConfigurations[instrumentConfigurationRef];
			ComponentListType cl = instrumentConfiguration.componentList;
			Dictionary<string, string> analyzerParameters = null;
			if (cl != null){
				AnalyzerComponentType analyzerComponent = cl.analyzer.Last();
				analyzerParameters = Parameters(analyzerComponent);
			} else{
				analyzerParameters = new Dictionary<string, string>();
			}
			MassAnalyzerEnum analyzer = MassAnalyzerEnum.Tof;
			if (analyzerParameters.ContainsKey(CV.FOURIER_TRANSFORM_ION_CYCLOTRON_RESONANCE_MASS_SPECTROMETER)){
				analyzer = MassAnalyzerEnum.Ftms;
			} else if (analyzerParameters.ContainsKey(CV.RADIAL_EJECTION_LINEAR_ION_TRAP)){
				analyzer = MassAnalyzerEnum.Itms;
			} else if (analyzerParameters.ContainsKey(CV.TIME_OF_FLIGHT)){
				analyzer = MassAnalyzerEnum.Tof;
			}
			return analyzer;
		}

		/// <summary>
		/// Parse <see cref="MsLevel"/> from <see cref="SpectrumType"/> parameters.
		/// </summary>
		private static MsLevel MsLevel(Dictionary<string, string> parameters, int scanNumber = 0){
			MsLevel msLevel;
			switch (parameters[CV.MS_LEVEL]){
				case "1":
					msLevel = BaseLibS.Ms.MsLevel.Ms1;
					break;
				case "2":
					msLevel = BaseLibS.Ms.MsLevel.Ms2;
					break;
				case "3":
					msLevel = BaseLibS.Ms.MsLevel.Ms3;
					break;
				default:
					throw new ArgumentException($"Could not read 'ms level' of spectrum {scanNumber}.");
			}
			return msLevel;
		}

		/// <summary>
		/// Create (name, value) dictionary from local <see cref="CVParamType"/>.
		/// Global <see cref="ReferenceableParamGroupRefType"/> references are not resolved and an exception is thrown.
		/// </summary>
		Dictionary<string, string> Parameters(ParamGroupType paramGroup){
			if ((paramGroup.referenceableParamGroupRef?.Length ?? 0) > 0){
				throw new ArgumentException(
					$"mzml feature {nameof(paramGroup.referenceableParamGroupRef)} is not supported by this software.");
			}
			return paramGroup.cvParam.ToDictionary(param => param.accession, param => param.value);
		}
	}
}