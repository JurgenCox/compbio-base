using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BaseLibS.Num;

namespace BaseLibS.Mol{
	public class PeptideModificationState{
		public int Hash { get; set; }
		public ushort[] Modifications { get; set; }
		public ushort NTermModification { get; set; }
		public ushort CTermModification { get; set; }

		private PeptideModificationState(){
			CTermModification = ushort.MaxValue;
			NTermModification = ushort.MaxValue;
			Modifications = new ushort[0];
		}

		public PeptideModificationState(string line){
			string[] w = line.Split(',');
			int len = w.Length - 2;
			Modifications = new ushort[len];
			NTermModification = w[0].Equals("A") ? ushort.MaxValue : ushort.Parse(w[0]);
			CTermModification = w[w.Length - 1].Equals("A") ? ushort.MaxValue : ushort.Parse(w[w.Length - 1]);
			for (int i = 0; i < len; i++){
				Modifications[i] = w[i + 1].Equals("A") ? ushort.MaxValue : ushort.Parse(w[i + 1]);
			}
		}

		public PeptideModificationState(int len){
			CTermModification = ushort.MaxValue;
			NTermModification = ushort.MaxValue;
			Modifications = new ushort[len];
			for (int i = 0; i < len; i++){
				Modifications[i] = ushort.MaxValue;
			}
		}

		public PeptideModificationState RemoveNonCompositionMods(){
			PeptideModificationState result = Clone();
			const ushort m = ushort.MaxValue - 10;
			if (result.NTermModification >= m){
				result.NTermModification = ushort.MaxValue;
			}
			if (result.CTermModification >= m){
				result.CTermModification = ushort.MaxValue;
			}
			for (int i = 0; i < Modifications.Length; i++){
				if (result.Modifications[i] >= m){
					result.Modifications[i] = ushort.MaxValue;
				}
			}
			return result;
		}

		public int Length => Modifications.Length;

		public int Count{
			get{
				int c = 0;
				if (NTermModification != ushort.MaxValue){
					c++;
				}
				if (CTermModification != ushort.MaxValue){
					c++;
				}
				foreach (ushort m in Modifications){
					if (m != ushort.MaxValue){
						c++;
					}
				}
				return c;
			}
		}

		public int CountNonlabel{
			get{
				int c = 0;
				if (NTermModification != ushort.MaxValue){
					if (Modification.IsStandardVarMod(NTermModification)){
						c++;
					}
				}
				if (CTermModification != ushort.MaxValue){
					if (Modification.IsStandardVarMod(CTermModification)){
						c++;
					}
				}
				foreach (ushort m in Modifications){
					if (m != ushort.MaxValue){
						if (Modification.IsStandardVarMod(m)){
							c++;
						}
					}
				}
				return c;
			}
		}

		public ushort GetModificationAt(int index){
			return index >= Modifications.Length ? ushort.MaxValue : Modifications[index];
		}

		public void SetModificationAt(int index, ushort value){
			Modifications[index] = value;
			Hash = 0;
		}

		public static PeptideModificationState Read(BinaryReader reader){
			PeptideModificationState result = new PeptideModificationState{
				CTermModification = reader.ReadUInt16(),
				NTermModification = reader.ReadUInt16()
			};
			int len = reader.ReadInt32();
			result.Modifications = new ushort[len];
			for (int i = 0; i < result.Modifications.Length; i++){
				result.Modifications[i] = reader.ReadUInt16();
			}
			return result;
		}

		public void Write(BinaryWriter writer){
			writer.Write(CTermModification);
			writer.Write(NTermModification);
			writer.Write(Modifications.Length);
			foreach (ushort t in Modifications){
				writer.Write(t);
			}
		}

		public PeptideModificationState Clone(){
			return new PeptideModificationState{
				Modifications = (ushort[]) Modifications.Clone(),
				NTermModification = NTermModification,
				CTermModification = CTermModification
			};
		}

		public PeptideModificationState GetFreshCopy(int len){
			PeptideModificationState result = new PeptideModificationState{
				NTermModification = ushort.MaxValue,
				CTermModification = ushort.MaxValue,
				Modifications = new ushort[len]
			};
			for (int i = 0; i < len; i++){
				result.Modifications[i] = ushort.MaxValue;
			}
			return result;
		}

		public PeptideModificationCounts ProjectToCounts(){
			return ProjectToCounts(Modifications, NTermModification, CTermModification);
		}

		private static PeptideModificationCounts ProjectToCounts(IEnumerable<ushort> modifications, ushort nTermModification,
			ushort cTermModification){
			Dictionary<ushort, ushort> w = new Dictionary<ushort, ushort>();
			foreach (ushort t in modifications.Where(t => t != ushort.MaxValue)){
				if (!w.ContainsKey(t)){
					w.Add(t, 0);
				}
				w[t]++;
			}
			if (nTermModification != ushort.MaxValue){
				if (!w.ContainsKey(nTermModification)){
					w.Add(nTermModification, 0);
				}
				w[nTermModification]++;
			}
			if (cTermModification != ushort.MaxValue){
				if (!w.ContainsKey(cTermModification)){
					w.Add(cTermModification, 0);
				}
				w[cTermModification]++;
			}
			ushort[] counts = new ushort[w.Count];
			ushort[] types = w.Keys.ToArray();
			Array.Sort(types);
			for (int i = 0; i < types.Length; i++){
				counts[i] = w[types[i]];
			}
			return new PeptideModificationCounts(types, counts);
		}

		public ushort[] GetAllInternalModifications(){
			List<ushort> result = new List<ushort>();
			foreach (ushort t in Modifications){
				if (t != ushort.MaxValue){
					result.Add(t);
				}
			}
			return ArrayUtils.UniqueValues(result);
		}

		public ushort[] GetAllModifications(){
			List<ushort> result = new List<ushort>();
			foreach (ushort t in Modifications){
				if (t != ushort.MaxValue){
					result.Add(t);
				}
			}
			if (NTermModification != ushort.MaxValue){
				result.Add(NTermModification);
			}
			if (CTermModification != ushort.MaxValue){
				result.Add(CTermModification);
			}
			return ArrayUtils.UniqueValues(result);
		}

		public override bool Equals(object anObject){
			if (this == anObject){
				return true;
			}
			if (anObject is PeptideModificationState){
				PeptideModificationState other = (PeptideModificationState) anObject;
				if (other.NTermModification != NTermModification){
					return false;
				}
				if (other.CTermModification != CTermModification){
					return false;
				}
				if (other.Length != Length){
					return false;
				}
				for (int i = 0; i < Length; i++){
					if (other.GetModificationAt(i) != GetModificationAt(i)){
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode(){
			int h = Hash;
			if (h == 0){
				for (int i = 0; i < Length; i++){
					h = 31*h + GetModificationAt(i);
				}
				h = 31*h + NTermModification;
				h = 31*h + CTermModification;
				Hash = h;
			}
			return h;
		}

		public PeptideModificationState GetTrueModifications(){
			PeptideModificationState result = GetFreshCopy(Length);
			if (NTermModification != ushort.MaxValue && Modification.IsStandardVarMod(NTermModification)){
				result.NTermModification = NTermModification;
			}
			if (CTermModification != ushort.MaxValue && Modification.IsStandardVarMod(CTermModification)){
				result.CTermModification = CTermModification;
			}
			for (int i = 0; i < Length; i++){
				ushort m = GetModificationAt(i);
				if (m == ushort.MaxValue || !Modification.IsStandardVarMod(m)){
					result.SetModificationAt(i, ushort.MaxValue);
				} else{
					result.SetModificationAt(i, GetModificationAt(i));
				}
			}
			return result;
		}

		public bool HasIsobaricLabels(){
			if (Modification.IsIsobaricLabelMod(NTermModification)){
				return true;
			}
			if (Modification.IsIsobaricLabelMod(CTermModification)){
				return true;
			}
			foreach (ushort m in Modifications){
				if (Modification.IsIsobaricLabelMod(m)){
					return true;
				}
			}
			return false;
		}

		public PeptideModificationState GetLabelModifications(ushort[] labelMods, string sequence){
			PeptideModificationState result = GetFreshCopy(Length);
			if (NTermModification != ushort.MaxValue && !Modification.IsStandardVarMod(NTermModification)){
				result.NTermModification = NTermModification;
			}
			if (CTermModification != ushort.MaxValue && !Modification.IsStandardVarMod(CTermModification)){
				result.CTermModification = CTermModification;
			}
			for (int i = 0; i < Length; i++){
				ushort m = GetModificationAt(i);
				if (m != ushort.MaxValue && !Modification.IsStandardVarMod(m)){
					result.SetModificationAt(i, m);
				} else{
					result.SetModificationAt(i, ushort.MaxValue);
				}
			}
			foreach (ushort labelMod in labelMods){
				if (!Modification.IsStandardVarMod(labelMod)){
					Modification mod = Tables.ModificationList[labelMod];
					if (mod.IsInternal){
						for (int j = 0; j < mod.AaCount; j++){
							char c = mod.GetAaAt(j);
							for (int i = 0; i < Length; i++){
								if (sequence[i] == c){
									result.SetModificationAt(i, mod.Index);
								}
							}
						}
					} else{
						if (mod.IsNterminal){
							result.NTermModification = mod.Index;
						} else{
							result.CTermModification = mod.Index;
						}
					}
				}
			}
			return result;
		}

		public double GetDeltaMass(){
			double result = 0;
			if (NTermModification != ushort.MaxValue){
				result += Tables.ModificationList[NTermModification].DeltaMass;
			}
			if (CTermModification != ushort.MaxValue){
				result += Tables.ModificationList[CTermModification].DeltaMass;
			}
			foreach (ushort m in Modifications){
				if (m != ushort.MaxValue){
					result += Tables.ModificationList[m].DeltaMass;
				}
			}
			return result;
		}

		public PeptideModificationState RemoveLabelModifications(){
			PeptideModificationState result = Clone();
			if (NTermModification != ushort.MaxValue && !Modification.IsStandardVarMod(NTermModification)){
				result.NTermModification = ushort.MaxValue;
			}
			if (CTermModification != ushort.MaxValue && !Modification.IsStandardVarMod(CTermModification)){
				result.CTermModification = ushort.MaxValue;
			}
			for (int i = 0; i < Modifications.Length; i++){
				if (Modifications[i] != ushort.MaxValue && !Modification.IsStandardVarMod(Modifications[i])){
					result.Modifications[i] = ushort.MaxValue;
				}
			}
			return result;
		}

		public PeptideModificationState Revert(){
			PeptideModificationState result = Clone();
			for (int i = 1; i < Modifications.Length - 1; i++){
				result.Modifications[i] = Modifications[Modifications.Length - 1 - i];
			}
			result.Modifications[Modifications.Length - 1] = Modifications[Modifications.Length - 1];
			result.Modifications[0] = Modifications[0];
			return result;
		}

		public void Dispose(){
			//Modifications = null;
			//IsDisposed = true;
		}

		public override string ToString(){
			StringBuilder sb = new StringBuilder();
			sb.Append(UshortToString(NTermModification));
			foreach (ushort modification in Modifications){
				sb.Append(",");
				sb.Append(UshortToString(modification));
			}
			sb.Append(",");
			sb.Append(UshortToString(CTermModification));
			return sb.ToString();
		}

		public string UshortToString(ushort u){
			return u == ushort.MaxValue ? "A" : u.ToString();
		}

		public static PeptideModificationState FillFixedModifications(string sequence, IDictionary<char, ushort> fmods){
			PeptideModificationState result = new PeptideModificationState(sequence.Length);
			for (int i = 0; i < sequence.Length; i++){
				if (fmods.ContainsKey(sequence[i])){
					result.Modifications[i] = fmods[sequence[i]];
				}
			}
			return result;
		}
	}
}