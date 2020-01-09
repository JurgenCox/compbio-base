using System;
using System.Collections.Generic;
using System.IO;

namespace BaseLibS.Ms {
	[Serializable]
	public class PasefPrecursorInfo {
		public int Id { get; set; }
		public int Parent { get; set; }
		public double LargestPeakMz { get; set; }
		public double AverageMz { get; set; }
		public double MonoisotopicMz { get; set; }
		public int Charge { get; set; }
		public double ScanNumber { get; set; }
		public double Intensity { get; set; }
		public List<int> PasefMsmsInds { get; set; } = new List<int>();

		public PasefPrecursorInfo(int id, int parent, double largestPeakMz, double averageMz, double monoisotopicMz,
			int charge, double scanNumber, double intensity) {
			Id = id;
			Parent = parent;
			LargestPeakMz = largestPeakMz;
			AverageMz = averageMz;
			MonoisotopicMz = monoisotopicMz;
			Charge = charge;
			ScanNumber = scanNumber;
			Intensity = intensity;
		}

		public PasefPrecursorInfo(BinaryReader reader) {
			Id = reader.ReadInt32();
			Parent = reader.ReadInt32();
			LargestPeakMz = reader.ReadDouble();
			AverageMz = reader.ReadDouble();
			MonoisotopicMz = reader.ReadDouble();
			Charge = reader.ReadInt32();
			ScanNumber = reader.ReadDouble();
			Intensity = reader.ReadDouble();
			int len = reader.ReadInt32();
			for (int i = 0; i < len; i++) {
				PasefMsmsInds.Add(reader.ReadInt32());
			}
		}

		public void Write(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write(Parent);
			writer.Write(LargestPeakMz);
			writer.Write(AverageMz);
			writer.Write(MonoisotopicMz);
			writer.Write(Charge);
			writer.Write(ScanNumber);
			writer.Write(Intensity);
			writer.Write(PasefMsmsInds.Count);
			foreach (int ind in PasefMsmsInds) {
				writer.Write(ind);
			}
		}
	}
}