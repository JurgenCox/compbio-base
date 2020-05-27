using System.Collections.Generic;
using System.IO;

namespace BaseLibS.Ms{
	public class DiaWindowGroup{
		public readonly List<DiaWindow> diaWindows = new List<DiaWindow>();

		public DiaWindowGroup(BinaryReader reader){
			int len = reader.ReadInt32();
			for (int i = 0; i < len; i++){
				diaWindows.Add(new DiaWindow(reader));
			}
		}

		public DiaWindowGroup(){ }

		public void Add(int scanNumBegin, int scanNumEnd, double isolationMz, double isolationWidth,
			double collisionEnergy){
			diaWindows.Add(new DiaWindow(scanNumBegin, scanNumEnd, isolationMz, isolationWidth, collisionEnergy));
		}

		public void Write(BinaryWriter writer){
			writer.Write(diaWindows.Count);
			foreach (DiaWindow t in diaWindows){
				t.Write(writer);
			}
		}
	}
}