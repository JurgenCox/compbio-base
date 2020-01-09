using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BaseLibS.Mol;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Genome{
	public class DnaSequence{
		private static readonly char[] symbols = "ACGT".ToCharArray();
		private static readonly Dictionary<char, char> complement = InitComplement();
		private static readonly ulong[] powers = InitPowers();
		private List<ulong> data = new List<ulong>();
		private List<byte> remaining = new List<byte>();

		private static ulong[] InitPowers(){
			ulong[] result = new ulong[27];
			result[0] = 1;
			for (int i = 1; i < 27; i++){
				result[i] = result[i - 1]*5;
			}
			return result;
		}

		private static IList<char> ReadRawSubsequence(BinaryReader reader, long dataStart, long dataEnd, long chromoLen){
			IEnumerable<ulong> data = ReadData(reader, dataStart, dataEnd);
			List<char> result = new List<char>();
			foreach (ulong u in data){
				for (int i = 0; i < 27; i++){
					result.Add(GetCharInLong(u, i));
				}
			}
			long dataLen = (chromoLen - 1)/27;
			bool hasTrailer = dataEnd > dataLen;
			if (hasTrailer){
				IEnumerable<byte> t = ReadTrailer(reader, dataLen);
				foreach (byte b in t){
					result.Add(b == 4 ? 'X' : symbols[b]);
				}
			}
			return result;
		}

		//inclusive end
		public static char[] ReadSubsequence(BinaryReader reader, long start, long end, long chromoLen){
			long dataStart = start/27;
			long dataEnd = end/27;
			IList<char> s = ReadRawSubsequence(reader, dataStart, dataEnd, chromoLen);
			int start1 = (int) (start - dataStart*27);
			int end1 = (int) (end - dataStart*27);
			return start1 < 0 ? new char[0] : ArrayUtils.SubArray(s, start1, end1 + 1);
		}

		private static IEnumerable<byte> ReadTrailer(BinaryReader reader, long len){
			reader.BaseStream.Seek(len*8 + 4, SeekOrigin.Begin);
			int n = reader.ReadInt32();
			byte[] result = new byte[n];
			for (int i = 0; i < n; i++){
				result[i] = reader.ReadByte();
			}
			return result;
		}

		private static IEnumerable<ulong> ReadData(BinaryReader reader, long start, long end){
			ulong[] result = new ulong[end - start + 1];
			try{
				reader.BaseStream.Seek(start*8 + 4, SeekOrigin.Begin);
				for (int i = 0; i < result.Length; i++){
					result[i] = reader.ReadUInt64();
				}
			} catch (Exception){}
			return result;
		}

		public DnaSequence(BinaryReader reader){
			int n = reader.ReadInt32();
			data = new List<ulong>();
			for (int i = 0; i < n; i++){
				data.Add(reader.ReadUInt64());
			}
			n = reader.ReadInt32();
			for (int i = 0; i < n; i++){
				remaining.Add(reader.ReadByte());
			}
		}

		public DnaSequence() {}
		public int Length => data.Count*27 + remaining.Count;

		private static Dictionary<char, char> InitComplement(){
			Dictionary<char, char> result = new Dictionary<char, char>
			{{'A', 'T'}, {'T', 'A'}, {'G', 'C'}, {'C', 'G'}, {'X', 'X'}};
			return result;
		}

		public char GetAaAt(int index){
			string codon = GetCodonAt(index);
			if (codon[0] == 'X' || codon[1] == 'X' || codon[2] == 'X'){
				return 'X';
			}
			char aa = AminoAcids.CodonToAa[codon];
			return aa;
		}

		public char GetComplementAaAt(int index){
			string codon = GetComplementCodonAt(index);
			if (codon[0] == 'X' || codon[1] == 'X' || codon[2] == 'X'){
				return 'X';
			}
			char aa = AminoAcids.CodonToAa[codon];
			return aa;
		}

		public string GetCodonAt(int index){
			return "" + this[index] + this[index + 1] + this[index + 2];
		}

		public string GetComplementCodonAt(int index){
			return "" + complement[this[index]] + complement[this[index - 1]] + complement[this[index - 2]];
		}

		public string GetPeptideAt(int index, int len){
			char[] peptide = new char[len];
			for (int i = 0; i < len; i++){
				char aa = GetAaAt(index + 3*i);
				if (aa == 'X' || aa == '*'){
					return null;
				}
				peptide[i] = aa;
			}
			return new string(peptide);
		}

		public string GetComplementPeptideAt(int index, int len){
			char[] peptide = new char[len];
			for (int i = 0; i < len; i++){
				char aa = GetComplementAaAt(index - 3*i);
				if (aa == 'X' || aa == '*'){
					return null;
				}
				peptide[i] = aa;
			}
			return new string(peptide);
		}

		public void Append(string s){
			s = s.ToUpper();
			s = s.Replace('U', 'T');
			foreach (char c in s){
				int index = Array.BinarySearch(symbols, c);
				if (index < 0){
					index = 4;
				}
				remaining.Add((byte) index);
				if (remaining.Count == 27){
					data.Add(Compress(remaining));
					remaining.Clear();
				}
			}
		}

		private static ulong Compress(IList<byte> remaining){
			ulong result = 0;
			for (int i = 26; i >= 0; i--){
				result = 5*result + remaining[i];
			}
			return result;
		}

		public char this[int i]{
			get{
				if (i >= Length || i < 0){
					throw new IndexOutOfRangeException();
				}
				int index = i/27;
				int pos = i%27;
				if (index == data.Count){
					return remaining[pos] == 4 ? 'X' : symbols[remaining[pos]];
				}
				return GetCharInLong(data[index], pos);
			}
		}

		private static char GetCharInLong(ulong w, int pos){
			int n = (int) ((w/powers[pos])%5);
			return n == 4 ? 'X' : symbols[n];
		}

		public void Dispose(){
			data.Clear();
			data = null;
			remaining.Clear();
			remaining = null;
		}

		public void Write(BinaryWriter writer){
			writer.Write(data.Count);
			foreach (ulong t in data){
				writer.Write(t);
			}
			writer.Write(remaining.Count);
			foreach (byte t in remaining){
				writer.Write(t);
			}
		}

		public void Write(string filename){
			BinaryWriter writer = FileUtils.GetBinaryWriter(filename);
			Write(writer);
			writer.Close();
		}

		public override string ToString(){
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < Length; i++){
				sb.Append(this[i]);
			}
			return sb.ToString();
		}
	}
}