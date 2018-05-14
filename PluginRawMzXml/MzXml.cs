using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BaseLibS.Ms;
using BaseLibS.Num;
using BaseLibS.Util;
using zlib;

namespace PluginRawMzXml {
	/// <summary>
	/// Implementation of a parser for mz-xml files. The index-table is read during initialization,
	/// which contains the location of the scan-data for each scan in the File. This location 
	/// information enables a fast lookup of this data. Furthermore, the class contains two header
	/// class (<see cref="MzxmlHeader"/> and <see cref="ScanHeader"/>), which contain the general
	/// information about the measurement and each scan respectively.
	/// </summary>
	public class MzXml {
		private readonly string filename;
		private readonly MzxmlHeader header;
		private readonly int minScanNumber;
		private readonly int maxScanNumber;
		private readonly Dictionary<int, long> scanNumberToFilePos = new Dictionary<int, long>();
		private readonly Regex regexInteger = new Regex("([\\d]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// Collection of all the information stored in a mz-xml header.
		/// </summary>
		public class MzxmlHeader {
			/// <summary>The number of scans recorded in this File.</summary>
			public int ScanCount { get; set; }

			/// <summary>The retention time at which the first scan was recorded.</summary>
			public double StartTime { get; set; }

			/// <summary>The retention time at which the last scan was recorded.</summary>
			public double EndTime { get; set; }

			/// <summary>The original File from which the data was taken stored in this File.</summary>
			public string ParentFile { get; set; }

			/// <summary>The SHA-1 sum of the contents of the File, which can be used to check whether the File is correct.</summary>
			public string ParentFileSha1 { get; set; }

			/// <summary>The model of the machine with which the data was generated.</summary>
			public string MachineModel { get; set; }

			/// <summary>The manufacturer of the machine with which the data was generated.</summary>
			public string MachineManufacturer { get; set; }

			/// <summary>The resolution used to record the data (This is not required, defaults to 30,000).</summary>
			public int Resolution { get; set; }

			/// <summary>The type of signal (centroid/profile) of the recorded data.</summary>
			public SignalType SignalType { get; set; }
		}

		/// <summary>
		/// Collection of all the information stored in a mz-xml scan header.
		/// </summary>
		public class ScanHeader {
			/// <summary>The level of the recorded data (1=MS1, 2=MS2, 3=MS3, etc.)</summary>
			public int MsLevel { get; set; }

			/// <summary>The unique scan number for this scan.</summary>
			public int ScanNumber { get; set; }

			/// <summary>The number of data-points in this scan.</summary>
			public int PeaksCount { get; set; }

			/// <summary>The retention time at which this scan was recorded.</summary>
			public double RetentionTime { get; set; }

			/// <summary>The low m/z value.</summary>
			public double LowMz { get; set; }

			/// <summary>The high m/z value.</summary>
			public double HighMz { get; set; }

			/// <summary>The m/z value for the most intense peak.</summary>
			public double BasePeakMz { get; set; }

			/// <summary>The intensity value for the most intense peak.</summary>
			public double BasePeakIntensity { get; set; }

			/// <summary>The summed intensity.</summary>
			public double TotalIonCurrent { get; set; }

			/// <summary>The type of scan</summary>
			public string ScanType { get; set; }

			/// <summary>The polarity (positive or negative) used to ionize the molecules.</summary>
			public string Polarity { get; set; }

			/// <summary></summary>
			public string Centroided { get; set; }

			/// <summary>Thermo-specific - contains information about the scan.</summary>
			public string FilterLine { get; set; }

			/// <summary>Tandem-MS specific: the m/z of the precursor.</summary>
			public double PrecursorMz { get; set; }

			/// <summary>Tandem-MS specific: the charge of the precursor.</summary>
			public int PrecursorCharge { get; set; }

			/// <summary>Tandem-MS specific: the intensity of the precursor.</summary>
			public double PrecursorIntensity { get; set; }

			/// <summary>Tandem-MS specific: the collision energy used.</summary>
			public double CollisionEnergy { get; set; }

			/// <summary>Tandem-MS specific: the fragmentation type used.</summary>
			public FragmentationTypeEnum FragmentationType { get; set; }
		}

		/// <summary>
		/// Constructs a new mz-xml File parser for the given filename. In the constructor it is
		/// attempted to read both the header and the index-table. When one of these cannot be
		/// read an exception is thrown to indicate that the mz-xml File is malformed.
		/// </summary>
		/// <param name="filename">The File containing the mz-xml data.</param>
		public MzXml(string filename) {
			// open the stream
			this.filename = filename;
			//_stream = new StreamReader(_filename);
			// load the header
			header = new MzxmlHeader {Resolution = -1, SignalType = SignalType.Profile};
			string headerData = GetHeaderData();
			XmlTextReader xmlHeader = new XmlTextReader(new StringReader(headerData));
			while (xmlHeader.Read()) {
				if (xmlHeader.IsStartElement("msRun")) {
					header.ScanCount = Parser.Int(xmlHeader.GetAttribute("scanCount"));
					string str = xmlHeader.GetAttribute("startTime");
					header.StartTime = Parser.Double(str.Substring(2, str.IndexOf('S') - 2)) / 60.0;
					str = xmlHeader.GetAttribute("endTime");
					header.EndTime = Parser.Double(str.Substring(2, str.IndexOf('S') - 2)) / 60.0;
				} else if (xmlHeader.IsStartElement("parentFile")) {
					header.ParentFile = xmlHeader.GetAttribute("fileName");
					header.ParentFileSha1 = xmlHeader.GetAttribute("fileSha1");
				} else if (xmlHeader.IsStartElement("dataProcessing")) {
					string centroided = xmlHeader.GetAttribute("centroided");
					if (centroided != null && centroided.Equals("1")) {
						header.SignalType = SignalType.Centroid;
					}
				} else if (xmlHeader.IsStartElement("msResolution")) {
					header.Resolution = Parser.Int(xmlHeader.GetAttribute("value"));
				} else if (xmlHeader.IsStartElement("msModel")) {
					header.MachineModel = xmlHeader.GetAttribute("value");
				} else if (xmlHeader.IsStartElement("msManufacturer")) {
					header.MachineManufacturer = xmlHeader.GetAttribute("value");
				}
			}
			// load the lookup table
			minScanNumber = int.MaxValue;
			maxScanNumber = int.MinValue;
			string indexData = GetIndexData();
			XmlTextReader xmlIndex = new XmlTextReader(new StringReader(indexData));
			while (xmlIndex.Read()) {
				xmlIndex.MoveToElement();
				if (!xmlIndex.IsStartElement("offset")) {
					continue;
				}
				int scannumber = Parser.Int(xmlIndex.GetAttribute("id"));
				minScanNumber = Math.Min(minScanNumber, scannumber);
				maxScanNumber = Math.Max(maxScanNumber, scannumber);
				scanNumberToFilePos.Add(scannumber, xmlIndex.ReadElementContentAsLong());
			}
		}

		/// <summary>
		/// Returns the <see cref="MzxmlHeader"/> associated to the mz-xml, which consists of general
		/// information about the machine and pre-processing steps applied.
		/// </summary>
		/// <returns>The header of the File.</returns>
		public MzxmlHeader GetHeader() {
			return header;
		}

		/// <summary>
		/// Returns the number of scans stored in the mz-xml File. This value is derived from the
		/// index lookup table stored at the end of the File.
		/// </summary>
		/// <returns>The number of scans stored in the File.</returns>
		public int GetNumSpectra() {
			return scanNumberToFilePos.Count;
		}

		/// <summary>
		/// Returns the first scan number of the scans stored in the File. This value is derived
		/// from the index lookup table stored at the end of the File.
		/// </summary>
		/// <returns>The first scan number stored in the File.</returns>
		public int GetFirstSpectrumNumber() {
			return minScanNumber;
		}

		/// <summary>
		/// Returns the last scan number of the scans stored in the File. This value is derived
		/// from the index lookup table stored at the end of the File.
		/// </summary>
		/// <returns>The last scan number stored in the File.</returns>
		public int GetLastSpectrumNumber() {
			return maxScanNumber;
		}

		/// <summary>
		/// Returns the <see cref="ScanHeader"/> for the given scan number. From the scan-header
		/// general properties like the retention time, ms-level, polarity and filterline of the
		/// scan can be obtained. The boundries for the scan number can be retrieved with
		/// <see cref="GetFirstSpectrumNumber"/> and <see cref="GetLastSpectrumNumber"/>. This
		/// method will throw an exception when the given scan number is outside these boundries.
		/// </summary>
		/// <param name="scannumber">The scan number.</param>
		/// <returns>The scan-header.</returns>
		public ScanHeader GetScanHeader(int scannumber) {
			if (scannumber < minScanNumber || scannumber > maxScanNumber) {
				throw new Exception("Undefined scan number: " + scannumber + " outside (" + minScanNumber + "," + maxScanNumber +
				                    ")");
			}
			ScanHeader scanHeader = new ScanHeader();
			string scanData = GetScanHeaderData(scannumber);
			XmlTextReader xmlReader = new XmlTextReader(new StringReader(scanData));
			while (xmlReader.Read()) {
				xmlReader.MoveToElement();
				if (xmlReader.IsStartElement("scan")) {
					scanHeader.MsLevel = Parser.Int(xmlReader.GetAttribute("msLevel"));
					scanHeader.ScanNumber = Parser.Int(xmlReader.GetAttribute("num"));
					scanHeader.PeaksCount = Parser.Int(xmlReader.GetAttribute("peaksCount"));
					scanHeader.LowMz = Parser.Double(xmlReader.GetAttribute("lowMz"));
					scanHeader.HighMz = Parser.Double(xmlReader.GetAttribute("highMz"));
					scanHeader.BasePeakMz = Parser.Double(xmlReader.GetAttribute("basePeakMz"));
					scanHeader.BasePeakIntensity = Parser.Double(xmlReader.GetAttribute("basePeakIntensity"));
					scanHeader.TotalIonCurrent = Parser.Double(xmlReader.GetAttribute("totIonCurrent"));
					string str = xmlReader.GetAttribute("retentionTime");
					scanHeader.RetentionTime = Parser.Double(str.Substring(2, str.IndexOf('S') - 2)) / 60.0f;
					str = xmlReader.GetAttribute("collisionEnergy");
					if (str != null) {
						scanHeader.CollisionEnergy = Parser.Double(str);
					}
					scanHeader.ScanType = xmlReader.GetAttribute("scanType");
					scanHeader.Polarity = xmlReader.GetAttribute("polarity");
					scanHeader.FilterLine = xmlReader.GetAttribute("filterLine");
					scanHeader.Centroided = xmlReader.GetAttribute("centroided");
				} else if (xmlReader.IsStartElement("precursorMz")) {
					// hack
					scanHeader.ScanType = "MS2";
					// 'MSConvert from proteowizard' incorrectly sets ScanType to Full for _all_ scans
					// hack
					Parser.TryInt(xmlReader.GetAttribute("precursorCharge"), out int precursorCharge);
					scanHeader.PrecursorCharge = precursorCharge;
					Parser.TryDouble(xmlReader.GetAttribute("precursorIntensity"), out double precursorIntensity);
					scanHeader.PrecursorIntensity = precursorIntensity;
					string fragmentation = xmlReader.GetAttribute("activationMethod");
					if (fragmentation == null)
					{
						fragmentation = "";
					}
					
					if (fragmentation.Equals("CID")) {
						scanHeader.FragmentationType = FragmentationTypeEnum.Cid;
					} else if (fragmentation.Equals("HCD")) {
						scanHeader.FragmentationType = FragmentationTypeEnum.Hcd;
					} else if (fragmentation.Equals("ETD")) {
						scanHeader.FragmentationType = FragmentationTypeEnum.Etd;
					} else {
						scanHeader.FragmentationType = FragmentationTypeEnum.Unknown;
					}
					scanHeader.PrecursorMz = xmlReader.ReadElementContentAsDouble();
				}
			}
			return scanHeader;
		}

		/// <summary>
		/// Returns the spectrum-data (m/z and intensity) of the given scan number.  The boundries
		/// for the scan number can be retrieved with <see cref="GetFirstSpectrumNumber"/> and
		/// <see cref="GetLastSpectrumNumber"/>. This method will throw an exception when the given
		/// scan number is outside these boundries. The spectrum data is returned as a 2-dimensional
		/// list of size (2, nrspectra) in emulation of the thermo RAW access DLL.
		/// </summary>
		/// <param name="scannumber">The scan number.</param>
		/// <returns>The scan-data.</returns>
		public double[,] GetMassListFromScanNum(int scannumber) {
			if (scannumber < minScanNumber || scannumber > maxScanNumber) {
				throw new Exception("Undefined scan number: " + scannumber + " outside (" + minScanNumber + "," + maxScanNumber +
				                    ")");
			}
			int compressedLength = 0;
			string compressionType = null;
			int precision = -1;
			string byteOrder = null;
			string contentType = null;
			string data = null;
			string peakData = GetScanPeakData(scannumber);
			XmlTextReader xmlReader = new XmlTextReader(new StringReader(peakData));
			while (xmlReader.Read()) {
				xmlReader.MoveToElement();
				if (!xmlReader.IsStartElement("peaks")) {
					continue;
				}
				precision = Parser.Int(xmlReader.GetAttribute("precision"));
				byteOrder = xmlReader.GetAttribute("byteOrder");
				contentType = xmlReader.GetAttribute("contentType");
				if (string.IsNullOrEmpty(contentType)) {
					contentType = xmlReader.GetAttribute("pairOrder");
				}
				compressionType = xmlReader.GetAttribute("compressionType");
				string sCompressedLen = xmlReader.GetAttribute("compressedLen");
				compressedLength = string.IsNullOrEmpty(sCompressedLen) ? -1 : Parser.Int(sCompressedLen);
				data = xmlReader.ReadElementContentAsString();
			}
			if (byteOrder == null || contentType == null || data == null) {
				throw new Exception("Malformed mz-xml File.");
			}
			if (!contentType.Equals("m/z-int") && !contentType.Equals("mz-int")) {
				throw new Exception("Non-supported content type: ' " + contentType + "'.");
			}
			// convert from base64 encoding
			byte[] bytes = Convert.FromBase64String(data);
			// decompress
			if (compressionType != null && compressionType.Equals("zlib")) {
				if (compressedLength != bytes.Length) {
					throw new Exception("Attribute 'compressedLen' has a different value from the reconstructed data array.");
				}
				ZStream z = new ZStream();
				const int bufferLen = 1024;
				z.next_in = bytes;
				z.next_in_index = 0;
				z.avail_in = bytes.Length;
				z.inflateInit();
				int totalLength = 0;
				List<byte[]> pieces = new List<byte[]>();
				while (true) {
					z.next_out = new byte[bufferLen];
					z.next_out_index = 0;
					z.avail_out = bufferLen;
					pieces.Add(z.next_out);
					int err = z.inflate(zlibConst.Z_NO_FLUSH);
					totalLength += bufferLen - z.avail_out;
					if (err == zlibConst.Z_STREAM_END) {
						break;
					}
					if (err != zlibConst.Z_OK) {
						throw new ZStreamException(z.msg);
					}
				}
				z.inflateEnd();
				bytes = new byte[totalLength];
				int pos = 0;
				foreach (byte[] piece in pieces) {
					Buffer.BlockCopy(piece, 0, bytes, pos, totalLength - pos > 1024 ? bufferLen : totalLength - pos);
					pos += piece.Length;
				}
			}
			// convert from byte encoding
			double[] massintensities = ByteArray.ToDoubleArray(bytes,
				byteOrder.Equals("network") ? ByteArray.endianBig : ByteArray.endianLittle, precision);
			double[,] masslist = new double[2, massintensities.Length / 2];
			for (int i = 0; i < massintensities.Length; i += 2) {
				masslist[0, i / 2] = massintensities[i];
				masslist[1, i / 2] = massintensities[i + 1];
			}
			return masslist;
		}

		private string GetHeaderData() {
			string line;
			// move stream to the start of the File
			//_stream.BaseStream.Seek(0, SeekOrigin.Begin);
			//_stream.DiscardBufferedData();
			StreamReader stream = new StreamReader(filename);
			stream.BaseStream.Seek(0, SeekOrigin.Begin);
			stream.DiscardBufferedData();
			// look for the opening element 'msRun'
			do {
				line = stream.ReadLine();
			} while (line != null && !line.Contains("<msRun"));
			if (line == null) {
				throw new Exception("Invalid mz-xml File; couldn't locate the opening <msRun> tag.");
			}
			// build up string until we locate the '<scan' tag
			StringBuilder headerData = new StringBuilder(line);
			do {
				line = stream.ReadLine();
				headerData.Append(line);
				headerData.Append("\n");
			} while (line != null && !line.Contains("<scan "));
			if (line == null) {
				throw new Exception("Invalid mz-xml File; couldn't locate the closing </dataProcessing> tag.");
			}
			stream.Close();
			string header1 = headerData.ToString();
			int headerStart = header1.IndexOf("<msRun", StringComparison.InvariantCulture);
			return header1.Substring(headerStart,
				       (header1.IndexOf("<scan ", StringComparison.InvariantCulture) - 1) - headerStart) + "</msRun>";
		}

		private string GetIndexData() {
			// with the backward reader we can quickly locate the origin of the <indexOffset>
			string line = null;
			StreamBackwardReader s = new StreamBackwardReader(filename);
			// look for the closing tag of the mzXML structure
			bool foundSlashMzXml = false;
			while (!foundSlashMzXml) {
				line = s.ReadLine();
				if (line.Contains("</mzXML>")) {
					foundSlashMzXml = true;
				}
			}
			if (!foundSlashMzXml) {
				throw new Exception("Invalid mz-xml File; couldn't locate the ending </mzXML> tag.");
			}
			// look for the index offset so we can read forward again
			// TODO this is not the most elegant way to do this.
			while (!line.Contains("</indexOffset>")) {
				line = s.ReadLine();
			}
			while (!line.Contains("<indexOffset>")) {
				line += s.ReadLine();
			}
			s.Close();
			long indexOffset = Parser.Uint(regexInteger.Match(line).Groups[1].ToString());
			// load the offset table
			StreamReader stream = new StreamReader(filename);
			stream.BaseStream.Seek(indexOffset, SeekOrigin.Begin);
			stream.DiscardBufferedData();
			//_stream.BaseStream.Seek(indexOffset, SeekOrigin.Begin);
			//_stream.DiscardBufferedData();
			StringBuilder indexData = new StringBuilder();
			do {
				line = stream.ReadLine();
				indexData.Append(line);
				indexData.Append("\n");
			} while (!line.Contains("</index>"));
			stream.Close();
			string index = indexData.ToString();
			int indexStart = index.IndexOf("<index", StringComparison.InvariantCulture);
			return index.Substring(indexStart, (index.IndexOf("</index>", StringComparison.InvariantCulture) + 8) - indexStart);
		}

		private string GetScanHeaderData(int scannumber) {
			// move stream to the correct position
			//_stream.BaseStream.Seek(_scanNumberToFilePos[scannumber], SeekOrigin.Begin);
			//_stream.DiscardBufferedData();
			StreamReader stream = new StreamReader(filename);
			stream.BaseStream.Seek(scanNumberToFilePos[scannumber], SeekOrigin.Begin);
			stream.DiscardBufferedData();
			// read the scan element
			string line;
			do {
				line = stream.ReadLine();
			} while (!line.Contains("<scan"));
			StringBuilder scanData = new StringBuilder(line);
			do {
				line = stream.ReadLine();
				scanData.Append(line);
			} while (!line.Contains("<peaks"));
			stream.Close();
			string scan = scanData.ToString();
			int indexStart = scan.IndexOf("<scan", StringComparison.InvariantCulture);
			return scan.Substring(indexStart, (scan.IndexOf("<peaks", StringComparison.InvariantCulture) - 1) - indexStart) +
			       "</scan>";
		}

		private string GetScanPeakData(int scannumber) {
			// move stream to the correct position
			//_stream.BaseStream.Seek(_scanNumberToFilePos[scannumber], SeekOrigin.Begin);
			//_stream.DiscardBufferedData();
			StreamReader stream = new StreamReader(filename);
			stream.BaseStream.Seek(scanNumberToFilePos[scannumber], SeekOrigin.Begin);
			stream.DiscardBufferedData();
			// read the scan element
			string line;
			do {
				line = stream.ReadLine();
			} while (!line.Contains("<peaks"));
			StringBuilder scanData = new StringBuilder(line);
			do {
				line = stream.ReadLine();
				scanData.Append(line);
			} while (line != null && !line.Contains("</peaks>"));
			stream.Close();
			string scan = scanData.ToString();
			int indexStart = scan.IndexOf("<peaks", StringComparison.InvariantCulture);
			return scan.Substring(indexStart, (scan.IndexOf("</peaks>", StringComparison.InvariantCulture) + 8) - indexStart);
		}
	}
}