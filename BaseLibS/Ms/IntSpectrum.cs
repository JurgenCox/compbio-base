using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Ms{
	public class IntSpectrum{
		public uint[] Masses{ get; }

		public uint[] Intensities { get; }
		public int FrameId { get; }

		public IntSpectrum(uint[] masses, uint[] intensities, int frameId){
			Masses = masses;
			Intensities = intensities;
			FrameId = frameId;
		}

		public IntSpectrum(BinaryReader reader){
			Masses = FileUtils.ReadUintArray(reader);
			Intensities = FileUtils.ReadUintArray(reader);
			FrameId = reader.ReadInt32();
		}

		public void Write(BinaryWriter writer){
			FileUtils.Write(Masses, writer);
			FileUtils.Write(Intensities, writer);
			writer.Write(FrameId);
		}
	}
}