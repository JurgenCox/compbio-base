using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using BaseLibS.Ms;
using BaseLibS.Num;
using BaseLibS.Util;
using zlib;
using Exception = System.Exception;

namespace PluginRawMzMl
{
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
	public class MzMLRawFile : RawFile
	{
		private OffsetType[] _offset;
		private Dictionary<string, InstrumentConfigurationType> _instrumentConfigurations;
		private string _defaultInstrumentConfigurationRef;
		/// <summary>
		/// Open reader to <see cref="RawFile.Path"/>.
		/// Should not be closed manually!
		/// </summary>
		private StreamReader _reader;

		public override string Suffix => ".mzML";
		public override bool IsFolderBased => false;
		public override string Name => "mzML";
		public override MsInstrument DefaultInstrument { get; }
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

		public override int LastScanNumber
		{
			get
			{
				if (!preInitialized)
				{
					PreInit();
				}
				return _offset.Length - 1;
			}
		}

		/// <summary>
		/// Pre-initialize the raw file. Should be called before accessing any fields, properties or methods.
		/// 
		/// 1. Look for <code>indexListOffset</code> tag
		/// 2. Seek indexList
		/// 3. Deserialize indexList
		/// </summary>
		protected override void PreInit()
		{
			preInitialized = true;
			_reader = File.OpenText(Path);
			PreInitOffset();
			_reader.DiscardBufferedData();
			_reader.BaseStream.Seek(0, SeekOrigin.Begin);
			using (var xml = XmlReader.Create(_reader))
			{
				// This might take longer than expected if <instrumentConfigurationList> is at the end of file rather than beginning.
				xml.ReadToDescendant(Xml.InstrumentConfigurationListElementName);
				var instrumentConfigurationList = (InstrumentConfigurationListType) Xml.InstrumentConfigurationListSerializer.Deserialize(xml);
				_instrumentConfigurations = instrumentConfigurationList.instrumentConfiguration.ToDictionary(config => config.id, config => config);
				xml.ReadToFollowing(Xml.RunElementName);
				_defaultInstrumentConfigurationRef = xml.GetAttribute(Xml.DefaultInstrumentConfigurationRefAttributeName);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			_reader?.Dispose();
		}

		/// <summary>
		/// Initialize <see cref="_offset"/>.
		/// </summary>
		private void PreInitOffset()
		{
			IndexListType indexList;
			// Try unsafe
			var indexListOffset = FindIndexListOffsetUnsafe();
			if (indexListOffset >= 0)
			{
				indexList = ReadIndexList(indexListOffset);
			}
			else // Fall back to safe
			{
				indexListOffset = FindIndexListOffsetSafe();
				indexList = ReadIndexList(indexListOffset);
			}

			_offset = indexList.index?.SingleOrDefault(index => index.name == IndexTypeName.spectrum)?.offset;
			if (_offset == null)
			{
				throw new Exception(
					$"mzML rawfile {System.IO.Path.GetFileNameWithoutExtension(Path)} did not contain {nameof(IndexTypeName.spectrum)} index.");
			}
		}

		/// <summary>
		/// Seek the location of the indexList according to the offset and deserialize the list.
		/// </summary>
		private IndexListType ReadIndexList(long indexListOffset)
		{
			_reader.DiscardBufferedData();
			_reader.BaseStream.Seek(indexListOffset, SeekOrigin.Begin);
			using (var xml = MzmlXmlReader(_reader))
			{
				xml.Read();
				var indexList = (IndexListType)Xml.IndexListSerializer.Deserialize(xml);
				return indexList;
			}
		}

		/// <summary>
		/// Create XmlReader suitable for reading from the middle of the file.
		/// The mzml namespace is added manually.
		/// </summary>
		private XmlReader MzmlXmlReader(TextReader reader)
		{
			var settings = new XmlReaderSettings { NameTable = new NameTable() };
			var xmlns = new XmlNamespaceManager(settings.NameTable);
			xmlns.AddNamespace("", "http://psi.hupo.org/ms/mzml");
			var context = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
			return XmlReader.Create(reader, settings, context);
		}

		/// <summary>
		/// Find index list offset in an unsafe way by reading from the end of the file
		/// and manually parsing the xml tags.
		///
		/// This is hopefully faster than <see cref="FindIndexListOffsetSafe"/>.
		/// </summary>
		private long FindIndexListOffsetUnsafe()
		{
			var target = new Regex(@"<indexListOffset>(?<indexListOffset>\d+)</indexListOffset>");
			var wentTooFar = new Regex("</indexList>");
			using (var reader = new StreamBackwardReader(Path))
			{
				foreach (var line in reader.ReadLines())
				{
					var match = target.Match(line);
					if (match.Success)
					{
						return long.TryParse(match.Groups["indexListOffset"].Value, out long indexListOffset) ? indexListOffset : -1;
					}
					if (wentTooFar.IsMatch(line))
					{
						return -1;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Find index list offset in a safe manner using <see cref="XmlReader"/>.
		///
		/// This is hopefully safter than <see cref="FindIndexListOffsetUnsafe"/>.
		/// </summary>
		private long FindIndexListOffsetSafe()
		{
			long indexListOffset;
			_reader.DiscardBufferedData();
			_reader.BaseStream.Seek(0, SeekOrigin.Begin);
			using (var xml = XmlReader.Create(_reader))
			{
				var isIndexedMzML = xml.ReadToFollowing(Xml.IndexedmzMLElementName);
				if (!isIndexedMzML)
				{
					throw new ArgumentException($"mzML without index not supported. Could not find 'indexedmzML' tag in {Path}.");
				}

				var hasOffset = xml.ReadToDescendant(Xml.IndexListOffsetElementName);
				if (!hasOffset)
				{
					throw new ArgumentException($"Could not find indexListOffset tag required by indexed mzML standard in {Path}.");
				}

				var indexListReader = xml.ReadSubtree();
				var maybeIndexList = (long?)Xml.IndexListOffsetSerializer.Deserialize(indexListReader);
				if (!maybeIndexList.HasValue)
				{
					throw new ArgumentException($"Could not parse indexListOffset required by indexed mzML standard in {Path}.");
				}

				indexListOffset = maybeIndexList.Value;
			}

			return indexListOffset;
		}


		protected override void GetSpectrum(int scanNumberMin, int scanNumberMax, int imsIndexMin, int imsIndexMax, bool readCentroids,
			out double[] masses, out double[] intensities, double resolution, double mzMin, double mzMax)
		{
			if (!preInitialized)
			{
				PreInit();
			}
			if (scanNumberMin != scanNumberMax)
			{
				throw new NotImplementedException("TODO: No idea what to do for a range of scan numbers.");
			}
			var scanNumber = scanNumberMin;
			var spectrum = DeserializeSpectrum(scanNumber);
			if (int.TryParse(spectrum.scanList?.count, out var scanListCount) && scanListCount > 1)
			{
				throw new ArgumentException(
					$"Unsupported mzml feature: Found more than one {nameof(ScanListType)} in {nameof(SpectrumType)} {spectrum.id}.");
			}
			var binary = spectrum.binaryDataArrayList.binaryDataArray;
			var binaryParameters = binary.Select(Parameters).ToList();
			masses = FromBinaryArray(CV.M_Z_ARRAY, binary, binaryParameters);
			intensities = FromBinaryArray(CV.INTENSITY_ARRAY, binary, binaryParameters);
		}


		private SpectrumType DeserializeSpectrum(int scanNumber)
		{
			var offset = _offset[scanNumber];
			_reader.DiscardBufferedData();
			_reader.BaseStream.Seek(offset.Value, SeekOrigin.Begin);
			using (var xml = MzmlXmlReader(_reader))
			{
				xml.Read();
				var spectrum = (SpectrumType)Xml.SpectrumSerializer.Deserialize(xml);
				Debug.Assert(offset.idRef.Equals(spectrum.id));
				return spectrum;
			}
		}

		/// <summary>
		/// Check if the parameter dictionary contains at least on CV term associated with ms-numpress <see cref="CV.NUMPRESS_ALL"/>.
		/// </summary>
		private static bool TryGetNumpressCompressionType(Dictionary<string, string> parameters, out string compressionType)
		{
			compressionType = CV.NUMPRESS_ALL.SingleOrDefault(parameters.ContainsKey);
			return !string.IsNullOrEmpty(compressionType);
		}

		/// <summary>
		/// Decompress binary array with the given name using the appropriate compression algorithm.
		/// </summary>
		private static double[] FromBinaryArray(string name, BinaryDataArrayType[] binary, List<Dictionary<string, string>> binaryParameters)
	    {
		    var index = binaryParameters.FindIndex(parameters => parameters.ContainsKey(name));
		    if (index < 0)
		    {
				throw new ArgumentException($"Could not find matching {nameof(BinaryDataArrayType)} with name '{name}'.");
		    }
		    var binaryArray = binary[index].binary;
		    var mzParameters = binaryParameters[index];
		    var isNumpress = TryGetNumpressCompressionType(mzParameters, out var numpressCompressionType);
		    if (isNumpress)
		    {
			    return MSNumpress.MSNumpress.decode(numpressCompressionType, binaryArray, binaryArray.Length);
		    }
		    var isZlib = mzParameters.ContainsKey(CV.ZLIB_COMPRESSION);
		    if (isZlib)
		    {
			    var deflated = DecompressZlib(binaryArray);
			    return FromUncompressedByteArray(mzParameters, deflated);
		    }
		    var isNoCompression = mzParameters.ContainsKey(CV.NO_COMPRESSION);
			if (isNoCompression)
			{
				return FromUncompressedByteArray(mzParameters, binaryArray);
			}
			throw new Exception($"Could not identify compression type of {name}.");
		}

		/// <summary>
		/// Decompress the binary array using zlib.
		/// </summary>
	    private static byte[] DecompressZlib(byte[] binaryArray)
	    {
		    byte[] deflated;
		    using (var memory = new MemoryStream(binaryArray))
		    using (var zOutputStream = new ZOutputStream(memory))
		    using (var result = new MemoryStream())
		    {
			    var buffer = new byte[2000];
			    int len;
			    while ((len = zOutputStream.Read(buffer, 0, 2000)) > 0)
			    {
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
	    private static double[] FromUncompressedByteArray(Dictionary<string, string> mzParameters, byte[] binaryArray)
	    {
		    int bytesUsed;
		    if (mzParameters.ContainsKey(CV.FLOAT_64_BIT))
		    {
			    bytesUsed = 8;
		    }
		    else if (mzParameters.ContainsKey(CV.FLOAT_32_BIT))
		    {
			    bytesUsed = 4;
		    }
		    else
		    {
			    throw new ArgumentException(
				    $"Cannot read {nameof(BinaryDataArrayType)}. Could not identify known data type (e.g. 64-bit float).");
		    }

		    var values = new double[binaryArray.Length / bytesUsed];
		    Buffer.BlockCopy(binaryArray, 0, values, 0, binaryArray.Length);
		    return values;
	    }

		protected override double[] Index2K0(int scanNumber, double[] imsInds)
		{
			// ion-mobility only...
			return null;
		}

		protected override ScanInfo GetInfoForScanNumber(int scanNumber)
		{
		    if (!preInitialized)
		    {
				PreInit();
		    }
		    if (scanNumber >= Convert.ToInt32(_offset.Length))
		    {
				throw new ArgumentException();
		    }
			var spectrum = DeserializeSpectrum(scanNumber);
		    var parameters = Parameters(spectrum);
		    var msLevel = MsLevel(parameters, scanNumber);
		    var scanList = spectrum.scanList;
		    var scanListParameters = Parameters(scanList);
		    if (!scanListParameters.ContainsKey(CV.NO_COMBINATION))
		    {
			    throw new NotImplementedException($"{nameof(spectrum.scanList)} should have 'no combination' parameter");
		    }
		    var scan = spectrum.scanList.scan.Single();
			var scanParameters = Parameters(scan);
		    var analyzer = MassAnalyzer(scanNumber, scan);
		    var scanInfo = new ScanInfo
			{
				positiveIonMode = parameters.ContainsKey(CV.POSITIVE_SCAN),
				msLevel = msLevel,
				basepeakIntensity = Convert.ToDouble(parameters[CV.BASE_PEAK_INTENSITY]),
				tic = Convert.ToDouble(parameters[CV.TOTAL_ION_CURRENT]),
				hasProfile = parameters.ContainsKey(CV.PROFILE_SPECTRUM),
				hasCentroid = parameters.ContainsKey(CV.CENTROID_SPECTRUM),
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
			if (double.IsNaN(scanInfo.min) || double.IsNaN(scanInfo.max))
			{
				var binary = spectrum.binaryDataArrayList.binaryDataArray;
				var binaryParameters = binary.Select(Parameters).ToList();
				var masses = FromBinaryArray(CV.M_Z_ARRAY, binary, binaryParameters);
				ArrayUtils.MinMax(masses, out var min, out var max);
				scanInfo.min = min;
				scanInfo.max = max;
			}
		    if (spectrum.precursorList != null)
		    {
			    var precursor = spectrum.precursorList.precursor.Single();
			    var activationParameters = Parameters(precursor.activation);
			    scanInfo.energy = GetParameterOrNaN(CV.COLLISION_ENERGY, activationParameters);
			    FragmentationTypeEnum fragmentationType;
			    if (activationParameters.ContainsKey(CV.COLLISION_INDUCED_DISSOCIATION))
			    {
				    fragmentationType = FragmentationTypeEnum.Cid;
			    }
			    else if (activationParameters.ContainsKey(CV.BEAM_TYPE_COLLISION_INDUCED_DISSOCIATION))
			    {
				    fragmentationType = FragmentationTypeEnum.Hcd;
			    }
			    else
			    {
					throw new ArgumentException($"Could not identify fragmentation type for spectrum {scanNumber}.");
			    }
			    scanInfo.ms2FragType = fragmentationType;
			    var isolationWindowParameters = Parameters(precursor.isolationWindow);
			    scanInfo.ms2MonoMz = double.NaN; // TODO taken from internal SciexWiffRawFile implementation
			    scanInfo.ms2ParentMz = Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_TARGET_M_Z]);
			    scanInfo.ms2IsolationMin = scanInfo.ms2ParentMz - Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_LOWER_OFFSET]);
			    scanInfo.ms2IsolationMax = scanInfo.ms2ParentMz + Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_UPPER_OFFSET]);
		    }
		    return scanInfo;
		}
		
		/// <summary>
		/// Get parameter by name and try to parse it as double. Otherwise return NaN.
		/// </summary>
	    private static double GetParameterOrNaN(string name, Dictionary<string, string> parameters)
	    {
		    if (!parameters.TryGetValue(name, out var value))
		    {
			    value = "NaN";
		    }
		    return Convert.ToDouble(value);
	    }

	    /// <summary>
		/// Read mass analyzer from scan configuration or default to run configuration.
		/// If more than one analyzer is defined in <see cref="InstrumentConfigurationListType.instrumentConfiguration"/>,
		/// the last one is assumed to be the one that produced the spectrum.
		/// </summary>
	    private MassAnalyzerEnum MassAnalyzer(int scanNumber, ScanType scan)
	    {
		    var instrumentConfigurationRef = scan.instrumentConfigurationRef ?? _defaultInstrumentConfigurationRef;
		    var instrumentConfiguration = _instrumentConfigurations[instrumentConfigurationRef];
		    var analyzerComponent = instrumentConfiguration.componentList.analyzer.Last();
		    var analyzerParameters = Parameters(analyzerComponent);
		    MassAnalyzerEnum analyzer;
			if (analyzerParameters.ContainsKey(CV.FOURIER_TRANSFORM_ION_CYCLOTRON_RESONANCE_MASS_SPECTROMETER))
		    {
			    analyzer = MassAnalyzerEnum.Ftms;
		    }
		    else if (analyzerParameters.ContainsKey(CV.RADIAL_EJECTION_LINEAR_ION_TRAP))
		    {
			    analyzer = MassAnalyzerEnum.Itms;
		    }
		    else if (analyzerParameters.ContainsKey(CV.TIME_OF_FLIGHT))
		    {
			    analyzer = MassAnalyzerEnum.Tof;
		    }
		    else
		    {
			    throw new ArgumentException($"Could not identify mass analyzer for spectrum {scanNumber}.");
		    }

		    return analyzer;
	    }

	    /// <summary>
		/// Parse <see cref="MsLevel"/> from <see cref="SpectrumType"/> parameters.
		/// </summary>
	    private static MsLevel MsLevel(Dictionary<string, string> parameters, int scanNumber)
	    {
		    MsLevel msLevel;
		    switch (parameters[CV.MS_LEVEL])
		    {
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
	    Dictionary<string, string> Parameters(ParamGroupType paramGroup)
		{
			if ((paramGroup.referenceableParamGroupRef?.Length ?? 0) > 0)
			{
				throw new ArgumentException($"mzml feature {nameof(paramGroup.referenceableParamGroupRef)} is not supported by this software.");
			}
			return paramGroup.cvParam.ToDictionary(param => param.accession, param => param.value);
		}
	}
}