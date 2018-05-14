using System;
using System.IO;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Mol{
	public class PeptideModificationCounts{
		private int hash;
		public ushort[] ModificationTypes { get; private set; }
		public ushort[] ModificationCounts { get; private set; }

		public PeptideModificationCounts(ushort[] types, ushort[] counts){
			ModificationTypes = types;
			ModificationCounts = counts;
		}

		public PeptideModificationCounts(){
			ModificationTypes = new ushort[0];
			ModificationCounts = new ushort[0];
		}

		public PeptideModificationCounts(BinaryReader reader){
			int len = reader.ReadUInt16();
			ModificationTypes = new ushort[len];
			ModificationCounts = new ushort[len];
			for (int i = 0; i < len; i++){
				ModificationTypes[i] = reader.ReadUInt16();
				ModificationCounts[i] = reader.ReadUInt16();
			}
		}

		public bool IsEmpty => ModificationTypes.Length == 0;

		public PeptideModificationCounts Add(PeptideModificationCounts other){
			ushort[] allTypes = ArrayUtils.UniqueValues(ArrayUtils.Concat(ModificationTypes, other.ModificationTypes));
			ushort[] allCounts = new ushort[allTypes.Length];
			for (int i = 0; i < ModificationTypes.Length; i++) {
				int ind = Array.BinarySearch(allTypes, ModificationTypes[i]);
				allCounts[ind] += ModificationCounts[i];
			}
			for (int i = 0; i < other.ModificationTypes.Length; i++) {
				int ind = Array.BinarySearch(allTypes, other.ModificationTypes[i]);
				allCounts[ind] += other.ModificationCounts[i];
			}
			return new PeptideModificationCounts(allTypes, allCounts);
		}

		public Molecule GetMolecule(){
			Molecule[] x = new Molecule[ModificationTypes.Length];
			for (int i = 0; i < x.Length; i++){
				Modification mod = Tables.ModificationList[ModificationTypes[i]];
				x[i] = mod.GetMolecule();
			}
			return Molecule.Sum(x, ModificationCounts);
		}

		public double DeltaMass{
			get{
				double deltaMass = 0;
				for (int i = 0; i < ModificationCounts.Length; i++){
					Modification mod = Tables.ModificationList[ModificationTypes[i]];
					deltaMass += ModificationCounts[i]*mod.DeltaMass;
				}
				return deltaMass;
			}
		}

		public void Write(BinaryWriter writer){
			writer.Write((ushort) ModificationTypes.Length);
			for (int i = 0; i < ModificationTypes.Length; i++){
				writer.Write(ModificationTypes[i]);
				writer.Write(ModificationCounts[i]);
			}
		}

		public override bool Equals(object obj){
			if (this == obj){
				return true;
			}
			if (obj is PeptideModificationCounts){
				PeptideModificationCounts other = (PeptideModificationCounts) obj;
				if (other.ModificationTypes.Length != ModificationTypes.Length){
					return false;
				}
				for (int i = 0; i < ModificationTypes.Length; i++){
					if (ModificationTypes[i] != other.ModificationTypes[i]){
						return false;
					}
				}
				for (int i = 0; i < ModificationTypes.Length; i++){
					if (ModificationCounts[i] != other.ModificationCounts[i]){
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode(){
			int h = hash;
			if (h == 0){
				foreach (ushort t in ModificationTypes){
					h = 31*h + t;
				}
				foreach (ushort t in ModificationCounts){
					h = 31*h + t;
				}
				hash = h;
			}
			return h;
		}

		public override string ToString(){
			if (ModificationTypes.Length == 0){
				return "Unmodified";
			}
			string[] s = new string[ModificationTypes.Length];
			for (int i = 0; i < s.Length; i++){
				if (ModificationCounts[i] == 1){
					s[i] = Tables.ModificationList[ModificationTypes[i]].Name;
				} else{
					s[i] = "" + ModificationCounts[i] + " " + Tables.ModificationList[ModificationTypes[i]].Name;
				}
			}
			return StringUtils.Concat(",", s);
		}

		public string ToStringSemicolon(){
			if (ModificationTypes.Length == 0){
				return "Unmodified";
			}
			string[] s = new string[ModificationTypes.Length];
			for (int i = 0; i < s.Length; i++){
				if (ModificationCounts[i] == 1){
					s[i] = Tables.ModificationList[ModificationTypes[i]].Name;
				} else{
					s[i] = "" + ModificationCounts[i] + " " + Tables.ModificationList[ModificationTypes[i]].Name;
				}
			}
			return StringUtils.Concat(";", s);
		}

		public ushort GetModificationCount(ushort type){
			int index = Array.BinarySearch(ModificationTypes, type);
			return index < 0 ? (ushort) 0 : ModificationCounts[index];
		}

		public void Dispose(){
			ModificationTypes = null;
			ModificationCounts = null;
		}
	}
}