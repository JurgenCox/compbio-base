using System;
using System.IO;
using System.Text;

namespace BaseLibS.Mol{
	[Serializable]
	public class AaSequence : IComparable, IDisposable, ICloneable {
		public const int aasPerLong = 12;
		private const int basis = 32;
		private const int basis4 = 32*32*32*32;
		private static char[] aas;
		private static int indexOfX;
		private static string[] q;

		static AaSequence(){
			Prepare();
		}

		private ulong[] sequence;
		public int Length { get; private set; }
		private int hash;

		public AaSequence(string seq){
			Length = seq.Length;
			int a = Length/aasPerLong;
			int b = Length%aasPerLong;
			int n = b == 0 ? a : a + 1;
			sequence = new ulong[n];
			for (int i = 0; i < a; i++){
				sequence[i] = Encode(seq.Substring(aasPerLong*i, aasPerLong));
			}
			if (b > 0){
				sequence[a] = Encode(seq.Substring(aasPerLong*a, Length - aasPerLong*a));
			}
		}

		public AaSequence(BinaryReader reader){
			Length = reader.ReadInt32();
			int n = reader.ReadInt32();
			sequence = new ulong[n];
			for (int i = 0; i < sequence.Length; i++){
				sequence[i] = reader.ReadUInt64();
			}
		}

		private	AaSequence(){}

		public int GetNumLongs(){
			int a = Length/aasPerLong;
			int b = Length%aasPerLong;
			return (b == 0) ? a : a + 1;
		}

		public override string ToString(){
			if (sequence.Length == 0){
				return "";
			}
			StringBuilder b = new StringBuilder();
			for (int i = 0; i < sequence.Length - 1; i++){
				Decode(b, sequence[i], aasPerLong);
			}
			int l = Length - (sequence.Length - 1)*aasPerLong;
			Decode(b, sequence[sequence.Length - 1], l);
			return b.ToString();
		}

		public override int GetHashCode(){
			int h = hash;
			if (h == 0){
				foreach (ulong t in sequence){
					h = 31*h + (int) t;
				}
				hash = h;
			}
			return h;
		}

		public object Clone() { 
			return new AaSequence{sequence = sequence, Length = Length, hash = hash};
		}

		public override bool Equals(object obj){
			if (this == obj){
				return true;
			}
			if (obj is AaSequence){
				AaSequence other = (AaSequence) obj;
				if (other.Length != Length){
					return false;
				}
				for (int i = 0; i < sequence.Length; i++){
					if (sequence[i] != other.sequence[i]){
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public void Write(BinaryWriter writer){
			writer.Write(Length);
			writer.Write(sequence.Length);
			foreach (ulong u in sequence){
				writer.Write(u);
			}
		}

		public int CompareTo(object obj){
			AaSequence other = (AaSequence) obj;
			int min = Math.Min(sequence.Length, other.sequence.Length);
			for (int i = 0; i < min; i++){
				if (sequence[i] != other.sequence[i]){
					if (sequence[i] < other.sequence[i]){
						return -1;
					}
					return 1;
				}
			}
			if (sequence.Length != other.sequence.Length){
				if (sequence.Length < other.sequence.Length){
					return -1;
				}
				return 1;
			}
			if (Length != other.Length){
				if (Length < other.Length){
					return -1;
				}
				return 1;
			}
			return 0;
		}

		public void Dispose() { sequence = null; }

		private static void Prepare(){
			q = new string[basis4];
			aas = (AminoAcids.SingleLetterAas + "X").ToCharArray();
			Array.Sort(aas);
			indexOfX = new string(aas).IndexOf('X');
			int l = aas.Length;
			for (int i1 = 0; i1 < l; i1++){
				int q1 = i1;
				for (int i2 = 0; i2 < l; i2++){
					int q2 = i2 << 5;
					for (int i3 = 0; i3 < l; i3++){
						int q3 = i3 << 10;
						for (int i4 = 0; i4 < l; i4++){
							int q4 = i4 << 15;
							char[] s = new char[4];
							s[0] = aas[i1];
							s[1] = aas[i2];
							s[2] = aas[i3];
							s[3] = aas[i4];
							int n = q1 + q2 + q3 + q4;
							q[n] = new string(s);
						}
					}
				}
			}
		}

		private static ulong Encode(string seq){
			if (aas == null){
				Prepare();
			}
			seq = seq.ToUpper();
			ulong result = 0;
			int n = seq.Length;
			for (int i = 0; i < n; i++){
				char c = seq[n - 1 - i];
				int x = Array.BinarySearch(aas, c);
				if (x < 0){
					x = indexOfX;
				}
				result = (result << 5) + (ulong) x;
			}
			return result;
		}

		private static void Decode(StringBuilder b, ulong a, int n){
			if (aas == null){
				Prepare();
			}
			int bins = n/4;
			for (int i = 0; i < bins; i++){
				int x = (int) (a%basis4);
				a = a >> 20;
				b.Append(q[x]);
			}
			n -= bins*4;
			for (int i = 0; i < n; i++){
				int x = (int) (a%basis);
				a = a >> 5;
				b.Append(aas[x]);
			}
		}
	}
}