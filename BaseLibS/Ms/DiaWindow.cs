using System.IO;

namespace BaseLibS.Ms{
	public class DiaWindow{
		public readonly int scanNumBegin;
		public readonly int scanNumEnd;
		public readonly double isolationMz;
		public readonly double isolationWidth;
		public readonly double collisionEnergy;

		public DiaWindow(int scanNumBegin, int scanNumEnd, double isolationMz, double isolationWidth,
			double collisionEnergy){
			this.scanNumBegin = scanNumBegin;
			this.scanNumEnd = scanNumEnd;
			this.isolationMz = isolationMz;
			this.isolationWidth = isolationWidth;
			this.collisionEnergy = collisionEnergy;
		}

		public DiaWindow(BinaryReader reader){
			scanNumBegin = reader.ReadInt32();
			scanNumEnd = reader.ReadInt32();
			isolationMz = reader.ReadDouble();
			isolationWidth = reader.ReadDouble();
			collisionEnergy = reader.ReadDouble();
		}

		public void Write(BinaryWriter writer){
			writer.Write(scanNumBegin);
			writer.Write(scanNumEnd);
			writer.Write(isolationMz);
			writer.Write(isolationWidth);
			writer.Write(collisionEnergy);
		}
	}
}