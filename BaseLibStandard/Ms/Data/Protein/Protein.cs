using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Mol;
using BaseLibS.Ms.Search;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Ms.Data.Protein{
	public class Protein : IDisposable{
		/// <summary>
		/// Name of the fasta file this protein sequence originates from. It is just the name, not the complete path.
		/// </summary>
		public string FastaFileName{ get; set; }

		public string TaxonomyId{ get; set; }
		public bool Decoy{ get; set; }
		public bool Contaminant{ get; }
		public bool Proteogenomic{ get; }
		public string Accession{ get; set; }
		public string Description{ get; set; }
		public double MolecularWeight{ get; }
		public string AaSequence{ get; private set; }
		public byte[] CodonInds{ get; }
		public string[] KnownVariationAaBefore{ get; private set; }
		public string[] KnownVariationAaAfter{ get; private set; }
		public string[] KnownVariationNames{ get; private set; }
		public int[] KnownVariationPos{ get; private set; }

		public Protein(string sequence, string accession, string description, string variants, bool decoy,
			bool contaminant, string fastaFileName, string taxonomyId, bool isCodons, bool proteogenomic){
			if (isCodons){
				AaSequence = Translate(sequence, out byte[] codonInds);
				CodonInds = codonInds;
			} else{
				AaSequence = sequence;
				CodonInds = new byte[0];
			}
			Accession = accession;
			Description = description ?? "";
			MolecularWeight = AminoAcids.CalcMolecularWeight(sequence);
			Decoy = decoy;
			Contaminant = contaminant;
			Proteogenomic = proteogenomic;
			FastaFileName = fastaFileName;
			TaxonomyId = taxonomyId;
			if (string.IsNullOrEmpty(variants)){
				KnownVariationAaBefore = new string[0];
				KnownVariationAaAfter = new string[0];
				KnownVariationNames = new string[0];
				KnownVariationPos = new int[0];
			} else{
				ParseVariation(variants);
			}
		}

		internal static string Translate(string baseSequence, out byte[] codonInds){
			List<char> result = new List<char>();
			List<byte> codInds = new List<byte>();
			for (int i = 0; i < baseSequence.Length - 2; i += 3){
				string codon = baseSequence.Substring(i, 3);
				if (AminoAcids.CodonToAa.ContainsKey(codon)){
					char aa = AminoAcids.CodonToAa[codon];
					byte ind = AminoAcids.CodonToInd[codon];
					if (aa == '*'){
						break;
					}
					result.Add(aa);
					codInds.Add(ind);
				} else{
					result.Add('X');
					codInds.Add(0);
				}
			}
			codonInds = codInds.ToArray();
			return new string(result.ToArray());
		}

		private void ParseVariation(string mutations){
			List<string> names = new List<string>();
			List<string> aasBefore = new List<string>();
			List<string> aasAfter = new List<string>();
			List<int> positions = new List<int>();
			foreach (string s in mutations.Split(';')){
				string[] q = s.Split(':');
				if (q.Length != 2){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid mutation format: " + mutations);
				}
				string name = q[0];
				string mut = q[1].ToUpper();
				if (mut.Length < 2){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid mutation format: " + mutations);
				}
				if (!char.IsLetter(mut, 0) && !char.IsLetter(mut, mut.Length - 1)){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid mutation format: " + mutations);
				}
				int firstDigitPos = GetFirstDigitPosition(mut);
				int lastDigitPos = GetLastDigitPosition(mut);
				if (firstDigitPos < 0 || lastDigitPos < 0){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid mutation format: " + mutations);
				}
				string before = mut.Substring(0, firstDigitPos);
				string posStr = mut.Substring(firstDigitPos, lastDigitPos - firstDigitPos + 1);
				string after = mut.Substring(lastDigitPos + 1);
				if (!Parser.TryInt(posStr, out int pos)){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid mutation format: " + mutations);
				}
				pos -= 1;
				if (before.Contains("*")){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid mutation format: " + mutations);
				}
				RemoveRedundantPrefixAndSuffix(ref before, ref after, ref pos);
				before = before.ToUpper();
				after = after.ToUpper();
				if (before.Equals(after)){
					continue;
				}
				if (before.Length == 0 && after.Length == 0){
					continue;
				}
				if (!ValidSequence(before)){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid sequence in mutation: " + before);
				}
				int ind = after.IndexOf('*');
				if (ind >= 0 && ind < after.Length - 1){
					after = after.Substring(ind);
				}
				string check = ind < 0 ? after : after.Substring(0, after.Length - 1);
				if (!ValidSequence(check)){
					continue;
					//TODO: add some logging
					throw new Exception("Invalid sequence in mutation: " + before);
				}
				// Sequences 'before' and 'after' have now no common prefix or suffix and consist of uppercase standard 20 amino 
				// acid letters. 'after' might end with a '*' for a new stop codon. 'before' and 'after' are not equal and 
				// cannot both be empty.
				names.Add(name);
				aasBefore.Add(before);
				aasAfter.Add(after);
				positions.Add(pos);
			}
			KnownVariationNames = names.ToArray();
			KnownVariationAaBefore = aasBefore.ToArray();
			KnownVariationAaAfter = aasAfter.ToArray();
			KnownVariationPos = positions.ToArray();
			int[] o = ArrayUtils.Order(KnownVariationPos);
			KnownVariationNames = ArrayUtils.SubArray(KnownVariationNames, o);
			KnownVariationAaBefore = ArrayUtils.SubArray(KnownVariationAaBefore, o);
			KnownVariationAaAfter = ArrayUtils.SubArray(KnownVariationAaAfter, o);
			KnownVariationPos = ArrayUtils.SubArray(KnownVariationPos, o);
		}

		internal static bool ValidSequence(string s){
			foreach (char c in s){
				if (AminoAcids.StandardSingleLetterAas.IndexOf(c) < 0){
					return false;
				}
			}
			return true;
		}

		internal static void RemoveRedundantPrefixAndSuffix(ref string before, ref string after, ref int pos){
			int l1 = StringUtils.GetCommonPrefixLength(new[]{before, after});
			if (l1 > 0){
				before = before.Substring(l1);
				after = after.Substring(l1);
				pos += l1;
			}
			l1 = StringUtils.GetCommonSuffixLength(new[]{before, after});
			if (l1 > 0){
				before = before.Substring(0, before.Length - l1);
				after = after.Substring(0, after.Length - l1);
			}
		}

		internal static int GetFirstDigitPosition(string s){
			for (int i = 0; i < s.Length; i++){
				if (char.IsDigit(s[i])){
					return i;
				}
			}
			return -1;
		}

		internal static int GetLastDigitPosition(string s){
			for (int i = s.Length - 1; i >= 0; i--){
				if (char.IsDigit(s[i])){
					return i;
				}
			}
			return -1;
		}

		public bool HasCodons => CodonInds.Length > 0;
		public int Length => AaSequence.Length;
		public bool HasKnownMutations => KnownVariationPos.Length > 0;

		public void Digest(EnzymeMode enzymeMode, int minPepLen, int maxPepLen, int missedCleavages, Enzyme[] enzymes,
			bool independentEnzymes, Action<string, bool, bool, byte, string[], string> addPeptide,
			VariationMode variationMode, string variationParseRule, bool mutationNames){
			const int maxSubstitutions = 3;
			const int maxCombiMutations = 100;
			DigestionUtil.Digest(this, enzymeMode, minPepLen, maxPepLen, missedCleavages, enzymes, independentEnzymes,
				addPeptide, variationMode, variationParseRule, maxSubstitutions, maxCombiMutations, mutationNames);
		}

		public Protein(BinaryReader reader){
			AaSequence = new AaSequence(reader).ToString();
			CodonInds = FileUtils.ReadByteArray(reader);
			Accession = reader.ReadString();
			Description = reader.ReadString();
			MolecularWeight = reader.ReadDouble();
			Decoy = reader.ReadBoolean();
			Contaminant = reader.ReadBoolean();
			Proteogenomic = reader.ReadBoolean();
			FastaFileName = reader.ReadString();
			TaxonomyId = reader.ReadString();
			KnownVariationAaBefore = FileUtils.ReadStringArray(reader);
			KnownVariationAaAfter = FileUtils.ReadStringArray(reader);
			KnownVariationNames = FileUtils.ReadStringArray(reader);
			KnownVariationPos = FileUtils.ReadInt32Array(reader);
		}

		public void Write(BinaryWriter writer){
			new AaSequence(AaSequence).Write(writer);
			FileUtils.Write(CodonInds, writer);
			writer.Write(Accession);
			writer.Write(Description);
			writer.Write(MolecularWeight);
			writer.Write(Decoy);
			writer.Write(Contaminant);
			writer.Write(Proteogenomic);
			writer.Write(FastaFileName);
			writer.Write(TaxonomyId);
			FileUtils.Write(KnownVariationAaBefore, writer);
			FileUtils.Write(KnownVariationAaAfter, writer);
			FileUtils.Write(KnownVariationNames, writer);
			FileUtils.Write(KnownVariationPos, writer);
		}

		public void Dispose(){
			AaSequence = null;
			Accession = null;
			Description = null;
			FastaFileName = null;
			KnownVariationPos = null;
			KnownVariationAaBefore = null;
			KnownVariationAaAfter = null;
			KnownVariationNames = null;
		}
	}
}