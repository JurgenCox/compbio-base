using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Xml.Serialization;
using BaseLibS.Ms;
using zlib;

namespace PluginRawMzMl
{
    public class MzMLRawFile : RawFile
    {
	    private indexedmzML _indexedmzML;
	    private SpectrumListType _spectrumList => _indexedmzML.mzML.run.spectrumList;

	    protected override double MaximumIntensity
	    {
		    get
		    {
			    if (!preInitialized)
			    {
				    PreInit();
			    }
			    var maximumIntensity = _indexedmzML.mzML.run.spectrumList.spectrum.Max(spectrum =>
					Convert.ToDouble(spectrum.cvParam.SingleOrDefault(cvParam => cvParam.name.Equals("base peak intensity"))?.value ?? "NaN"));
			    return maximumIntensity;
		    }
	    }
	    protected override void PreInit()
	    {
		    using (var reader = new StreamReader(Path))
		    {
			    var serializer = new XmlSerializer(typeof(indexedmzML));
			    _indexedmzML = serializer.Deserialize(reader) as indexedmzML;
		    }
		    preInitialized = true;
	    }

	    public override string Suffix => ".mzml";
	    public override bool IsFolderBased => false;
	    public override string Name => "MzML file";
	    public override MsInstrument DefaultInstrument { get; }
	    public override bool IsInstalled => true;
	    public override bool NeedsGrid => false;
	    public override bool HasIms => false;
	    public override string InstallMessage => "No install necessary";

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
			    return  Convert.ToInt32(_indexedmzML.mzML.run.spectrumList.count) - 1;
		    }
	    }

	    protected override void GetSpectrum(int scanNumberMin, int scanNumberMax, int imsIndexMin, int imsIndexMax, bool readCentroids,
		    out double[] masses, out double[] intensities, double resolution, double mzMin, double mzMax)
	    {
		    if (!preInitialized)
		    {
				PreInit();
		    }
			if (scanNumberMin >= Convert.ToInt32(_spectrumList.count))
			{
				throw new ArgumentException($"{nameof(scanNumberMin)} cannot be larger than {nameof(_spectrumList.count)} of {_spectrumList.count}.");
			}
			var spectrum = _spectrumList.spectrum[scanNumberMin];
		    var binary = spectrum.binaryDataArrayList.binaryDataArray;
		    var binaryParameters = binary.Select(array => Parameters(array.cvParam, array.referenceableParamGroupRef)).ToList();
		    masses = FromBinaryArray(CV.M_Z_ARRAY, binary, binaryParameters);
		    try
		    {
			    intensities = FromBinaryArray(CV.INTENSITY_ARRAY, binary, binaryParameters);
		    }
		    catch (Exception e)
		    {
				// TODO Errors seem to be associated mainly with short int arrays which can't be decompressed...
				Console.WriteLine($"Failed to read intensities for {spectrum.id}:{spectrum.index}:{e.Message}");
				Debug.WriteLine($"Failed to read intensities for {spectrum.id}:{spectrum.index}:{e.Message}");
				intensities = new double[masses.Length];
		    }
		}

		private static bool TryGetNumpressCompressionType(Dictionary<string, string> parameters, out string compressionType)
		{
			compressionType = CV.NUMPRESS_ALL.SingleOrDefault(parameters.ContainsKey);
			return !string.IsNullOrEmpty(compressionType);
		}

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

	    // TODO what does this do? seems to return null for many raw files.
	    protected override double[] Index2K0(int scanNumber, double[] imsInds)
	    {
		    return null;
	    }

	    protected override ScanInfo GetInfoForScanNumber(int scanNumber)
	    {
		    if (!preInitialized)
		    {
				PreInit();
		    }
		    if (scanNumber >= Convert.ToInt32(_spectrumList.count))
		    {
				throw new ArgumentException();
		    }
		    var spectrum = _spectrumList.spectrum[scanNumber];
		    var parameters = Parameters(spectrum.cvParam, spectrum.referenceableParamGroupRef);
		    var msLevel = MsLevel(parameters, scanNumber);
		    var scanList = spectrum.scanList;
		    var scanListParameters = Parameters(scanList.cvParam, scanList.referenceableParamGroupRef);
		    if (!scanListParameters.ContainsKey(CV.NO_COMBINATION))
		    {
			    throw new NotImplementedException($"{nameof(spectrum.scanList)} should have 'no combination' parameter");
		    }
		    var scan = spectrum.scanList.scan.Single();
		    var scanParameters = Parameters(scan.cvParam, scan.referenceableParamGroupRef);
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
		    if (spectrum.precursorList != null)
		    {
			    var precursor = spectrum.precursorList.precursor.Single();
			    var activationParameters = Parameters(precursor.activation.cvParam, precursor.activation.referenceableParamGroupRef);
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
			    var isolationWindowParameters = Parameters(precursor.isolationWindow.cvParam, precursor.isolationWindow.referenceableParamGroupRef);
			    scanInfo.ms2MonoMz = double.NaN; // TODO taken from internal SciexWiffRawFile implementation
			    scanInfo.ms2ParentMz = Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_TARGET_M_Z]);
			    scanInfo.ms2IsolationMin = scanInfo.ms2ParentMz - Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_LOWER_OFFSET]);
			    scanInfo.ms2IsolationMin = scanInfo.ms2ParentMz + Convert.ToDouble(isolationWindowParameters[CV.ISOLATION_WINDOW_UPPER_OFFSET]);
		    }
		    return scanInfo;
	    }

	    private static double GetParameterOrNaN(string name, Dictionary<string, string> parameters)
	    {
		    if (!parameters.TryGetValue(name, out var lowestMz))
		    {
			    lowestMz = "NaN";
		    }
		    return Convert.ToDouble(lowestMz);
	    }

	    /// <summary>
		/// Read mass analyzer from scan configuration or default to run configuration.
		/// If more than one analyzer is defined in <see cref="InstrumentConfigurationListType.instrumentConfiguration"/>,
		/// the last one is assumed to be the one that produced the spectrum.
		/// </summary>
	    private MassAnalyzerEnum MassAnalyzer(int scanNumber, ScanType scan)
	    {
		    var instrumentConfigurationRef =
			    scan.instrumentConfigurationRef ?? _indexedmzML.mzML.run.defaultInstrumentConfigurationRef;
		    var instrumentConfiguration =
			    _indexedmzML.mzML.instrumentConfigurationList.instrumentConfiguration.Single(conf =>
				    conf.id.Equals(instrumentConfigurationRef));
		    var analyzerComponent = instrumentConfiguration.componentList.analyzer.Last();
		    var analyzerParameters = Parameters(analyzerComponent.cvParam, analyzerComponent.referenceableParamGroupRef);
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
		/// Create (name, value) dictionary from local <see cref="CVParamType"/> and global <see cref="ReferenceableParamGroupRefType"/>,
		/// which are resolved automatically.
		/// </summary>
	    Dictionary<string, string> Parameters(CVParamType[] parameters, ReferenceableParamGroupRefType[] parameterReferences)
		{
		    var commonParameters = parameterReferences?.SelectMany(reftype =>
			    _indexedmzML.mzML.referenceableParamGroupList.referenceableParamGroup.Single(x => x.id.Equals(reftype.@ref)).cvParam)
			    ?? new CVParamType[0];
		    return parameters.Concat(commonParameters).ToDictionary(param => param.accession, param => param.value);
	    }
    }
}
