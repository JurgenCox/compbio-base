using System;
using System.IO;

namespace BaseLibS.Ms {
	[Serializable]
	public class PasefFrameMsMsInfo {
		public double CollisionEnergy { get; set; }
		public int Frame { get; set; }
		public double IsolationWidth { get; set; }
		public int Precursor { get; set; }
		public int ScanNumBegin { get; set; }
		public int ScanNumEnd { get; set; }
		public double TriggerMass { get; set; }

		public PasefFrameMsMsInfo(int frame, int precursor, int scanNumBegin, int scanNumEnd, double triggerMass,
			double isolationWidth, double collisionEnergy) {
			Frame = frame;
			Precursor = precursor;
			ScanNumBegin = scanNumBegin;
			ScanNumEnd = scanNumEnd;
			TriggerMass = triggerMass;
			IsolationWidth = isolationWidth;
			CollisionEnergy = collisionEnergy;
		}

		public PasefFrameMsMsInfo(BinaryReader reader) {
			CollisionEnergy = reader.ReadDouble();
			Frame = reader.ReadInt32();
			IsolationWidth = reader.ReadDouble();
			Precursor = reader.ReadInt32();
			ScanNumBegin = reader.ReadInt32();
			ScanNumEnd = reader.ReadInt32();
			TriggerMass = reader.ReadDouble();
		}

		public void Write(BinaryWriter writer) {
			writer.Write(CollisionEnergy);
			writer.Write(Frame);
			writer.Write(IsolationWidth);
			writer.Write(Precursor);
			writer.Write(ScanNumBegin);
			writer.Write(ScanNumEnd);
			writer.Write(TriggerMass);
		}
	}
}