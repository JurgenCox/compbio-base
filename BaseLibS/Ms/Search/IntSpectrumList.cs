using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Ms.Search{
	public class IntSpectrumList{
		public IntSpectrum[] spectra;

		public IntSpectrumList(BinaryReader reader){
			int n = reader.ReadInt32();
			spectra = new IntSpectrum[n];
			for (int i = 0; i < n; i++){
				spectra[i] = new IntSpectrum(reader);
			}
		}

		public void Write(BinaryWriter writer){
			writer.Write(spectra.Length);
			foreach (IntSpectrum spectrum in spectra){
				spectrum.Write(writer);
			}
		}

		private static byte[] ReadByteArray1(BinaryReader reader){
			int n = reader.ReadInt32();
			return reader.ReadBytes(n);
		}

		public static IntSpectrumList ReadCompressed(BinaryReader reader){
			return FromBytes(FileUtils.Decompress(ReadByteArray1(reader)));
		}

		public void WriteCompressed(BinaryWriter writer){
			FileUtils.Write(FileUtils.Compress(GetBytes()), writer);
		}

		private static IntSpectrumList FromBytes(byte[] bytes){
			using (MemoryStream fs = new MemoryStream(bytes))
			using (BinaryReader br = new BinaryReader(fs))
				return new IntSpectrumList(br);
		}

		private byte[] GetBytes(){
			using (MemoryStream fs = new MemoryStream())
			using (BinaryWriter bw = new BinaryWriter(fs)){
				Write(bw);
				bw.Flush();
				fs.Flush();
				byte[] bytes = fs.ToArray();
				fs.Close();
				return bytes;
			}
		}
	}
}