using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BaseLibS.Mol;
using BaseLibS.Ms.Data.Protein;
using BaseLibS.Num;

namespace BaseLibS.Ms.Search{
	public static class DigestionUtil{
		public static int GetNcleave(Enzyme[] enzymes, bool independentEnzymes, string sequence, out bool consecutive){
			int[][] indices = CleavageIndices(sequence, enzymes, independentEnzymes);
			if (indices.Length == 1){
				consecutive = GetConsecutive(sequence, indices[0]);
				return indices[0].Length;
			}
			int[] ncleave = new int[indices.Length];
			for (int i = 0; i < ncleave.Length; i++){
				ncleave[i] = indices[i].Length;
			}
			int ind = ArrayUtils.MinInd(ncleave);
			consecutive = GetConsecutive(sequence, indices[ind]);
			return indices[ind].Length;
		}

		public static int[][] CleavageIndices(string s, Enzyme[] enzymes, bool independentEnzymes){
			if (enzymes.Length == 0){
				return new int[0][];
			}
			if (enzymes.Length == 1){
				return new[]{CleavageIndices(s, enzymes[0])};
			}
			int[][] x = new int[enzymes.Length][];
			for (int i = 0; i < x.Length; i++){
				x[i] = CleavageIndices(s, enzymes[i]);
			}
			return independentEnzymes ? x : new[]{ArrayUtils.UniqueValues(ArrayUtils.Concat(x))};
		}

		public static int CountPeptides(string sequence, Enzyme[] enzymes, bool independentEnzymes, int minLen,
			int maxLen){
			int[][] indices = CleavageIndices(sequence, enzymes, independentEnzymes);
			if (indices.Length == 1){
				return CountPeptides(sequence, indices[0], minLen, maxLen);
			}
			int[] counts = new int[indices.Length];
			for (int i = 0; i < counts.Length; i++){
				counts[i] = CountPeptides(sequence, indices[i], minLen, maxLen);
			}
			return ArrayUtils.Min(counts);
		}

		private static int CountPeptides(string sequence, int[] indices, int minLen, int maxLen){
			IEnumerable<string> pepSeqs = Digest(sequence, indices);
			int count = 0;
			foreach (string m1 in pepSeqs){
				int l = m1.Length;
				if (l >= minLen && l <= maxLen){
					count++;
				}
			}
			return count;
		}

		public static bool ValidPeptide(string pep){
			foreach (char t in pep){
				if (AminoAcids.SingleLetterAas.IndexOf(t) == -1){
					return false;
				}
			}
			return true;
		}

		public static bool ValidPeptide2(string pep, int minPepLen, int maxPepLen){
			if (pep.Length > ushort.MaxValue || pep.Length > maxPepLen || pep.Length < minPepLen){
				return false;
			}
			foreach (char t in pep){
				if (AminoAcids.SingleLetterAas.IndexOf(t) == -1){
					return false;
				}
			}
			return true;
		}

		internal static void Digest(Protein p, EnzymeMode enzymeMode, int minPepLen, int maxPepLen, int missedCleavages,
			Enzyme[] enzymes, bool independentEnzymes, Action<string, bool, bool, byte, string[], string> addPeptide,
			VariationMode variationMode, string variationGroupParseRule, int maxSubstitutions, int maxCombiMutations,
			bool mutationNames){
			Regex variationGroupRegex = ProteinSet.GetRegex(variationGroupParseRule);
			string proteinSeq = p.AaSequence;
			if (proteinSeq.Length > ushort.MaxValue){
				proteinSeq = proteinSeq.Substring(0, ushort.MaxValue);
			}
			if ((variationMode == VariationMode.AllCodonChanges && !p.HasCodons) ||
			    (variationMode == VariationMode.KnownMutations && !p.HasKnownMutations)){
				variationMode = VariationMode.None;
			}
			switch (enzymeMode){
				case EnzymeMode.Specific:
					switch (variationMode){
						case VariationMode.None:
							DigestSpecificNoMutations(proteinSeq, minPepLen, maxPepLen, missedCleavages, enzymes,
								independentEnzymes, addPeptide);
							break;
						case VariationMode.AllCodonChanges:
							DigestSpecificAllCodonChanges(proteinSeq, p.CodonInds, minPepLen, maxPepLen,
								missedCleavages, enzymes, independentEnzymes, addPeptide);
							break;
						case VariationMode.KnownMutations:
							DigestSpecificKnownMutations(proteinSeq, p.KnownVariationAaBefore, p.KnownVariationAaAfter,
								p.KnownVariationPos, mutationNames ? p.KnownVariationNames : null, variationGroupRegex,
								minPepLen, maxPepLen, missedCleavages, enzymes, independentEnzymes, addPeptide,
								maxSubstitutions, maxCombiMutations);
							break;
						default:
							throw new Exception("Never get here.");
					}
					break;
				case EnzymeMode.Semispecific:
				case EnzymeMode.SemispecificFreeCterm:
				case EnzymeMode.SemispecificFreeNterm:
					int[][] indices = CleavageIndices(proteinSeq, enzymes, independentEnzymes);
					switch (variationMode){
						case VariationMode.None:
							DigestUnspecificNoMutations(proteinSeq, minPepLen, maxPepLen, addPeptide, enzymeMode,
								indices);
							break;
						case VariationMode.AllCodonChanges:
							DigestUnspecificAllCodonChanges(proteinSeq, p.CodonInds, minPepLen, maxPepLen, addPeptide,
								enzymeMode, indices);
							break;
						case VariationMode.KnownMutations:
							DigestUnspecificKnownMutations(proteinSeq, p.KnownVariationAaBefore,
								p.KnownVariationAaAfter, p.KnownVariationPos,
								mutationNames ? p.KnownVariationNames : null, variationGroupRegex, minPepLen, maxPepLen,
								addPeptide, enzymeMode, indices);
							break;
						default:
							throw new Exception("Never get here.");
					}
					break;
				case EnzymeMode.Unspecific:
					switch (variationMode){
						case VariationMode.None:
							DigestUnspecificNoMutations(proteinSeq, minPepLen, maxPepLen, addPeptide, enzymeMode, null);
							break;
						case VariationMode.AllCodonChanges:
							DigestUnspecificAllCodonChanges(proteinSeq, p.CodonInds, minPepLen, maxPepLen, addPeptide,
								enzymeMode, null);
							break;
						case VariationMode.KnownMutations:
							DigestUnspecificKnownMutations(proteinSeq, p.KnownVariationAaBefore,
								p.KnownVariationAaAfter, p.KnownVariationPos,
								mutationNames ? p.KnownVariationNames : null, variationGroupRegex, minPepLen, maxPepLen,
								addPeptide, enzymeMode, null);
							break;
						default:
							throw new Exception("Never get here.");
					}
					break;
				//case EnzymeMode.UnspecificSpliced:
				//    switch (variationMode) {
				//        case VariationMode.None:
				//            DigestUnspecificSplicedNoMutations(proteinSeq, minPepLen, maxPepLen, 3, 20, addPeptide);
				//            break;
				//        case VariationMode.AllCodonChanges:
				//        case VariationMode.KnownMutations:
				//        default:
				//            throw new ArgumentOutOfRangeException(nameof(variationMode), variationMode, null);
				//    }
				//    break;
				case EnzymeMode.None:
					switch (variationMode){
						case VariationMode.None:
							DigestNoneNoMutations(proteinSeq, addPeptide);
							break;
						case VariationMode.AllCodonChanges:
							DigestNoneAllCodonChanges(proteinSeq, p.CodonInds, addPeptide);
							break;
						case VariationMode.KnownMutations:
							DigestNoneKnownMutations(proteinSeq, p.KnownVariationAaBefore, p.KnownVariationAaAfter,
								p.KnownVariationPos, mutationNames ? p.KnownVariationNames : null, addPeptide);
							break;
						default:
							throw new Exception("Never get here.");
					}
					break;
				default:
					throw new Exception("Never get here.");
			}
		}

		//TODO
		private static void DigestNoneKnownMutations(string proteinSeq, string[] knownVariationAaBefore,
			string[] knownVariationAaAfter, int[] knownVariationPos, string[] strings,
			Action<string, bool, bool, byte, string[], string> addPeptide){
			addPeptide(proteinSeq, false, false, 0, null, null);
		}

		//TODO
		private static void DigestNoneAllCodonChanges(string proteinSeq, byte[] codonInds,
			Action<string, bool, bool, byte, string[], string> addPeptide){
			addPeptide(proteinSeq, false, false, 0, null, null);
		}

		private static void DigestNoneNoMutations(string proteinSeq,
			Action<string, bool, bool, byte, string[], string> addPeptide){
			addPeptide(proteinSeq, false, false, 0, null, null);
		}

		private static bool GetConsecutive(string s, IList<int> indices){
			if (indices.Count == 0){
				return false;
			}
			if (indices[0] == 1){
				return true;
			}
			if (s.Length - indices[indices.Count - 1] == 1){
				return true;
			}
			for (int i = 1; i < indices.Count; i++){
				if (indices[i] - indices[i - 1] == 1){
					return true;
				}
			}
			return false;
		}

		private static int[] CleavageIndices(string s, Enzyme enzyme){
			List<int> indices = new List<int>();
			for (int i = 0; i < s.Length - 1; i++){
				if (enzyme.Cleaves(s[i], s[i + 1])){
					indices.Add(i + 1);
				}
			}
			return ArrayUtils.UniqueValues(indices.ToArray());
		}

		public static void DigestSpecificNoMutations(string protSequence, int minPepLen, int maxPepLen,
			int maxMissedSites, Enzyme[] enzymes, bool independentEnzymes,
			Action<string, bool, bool, byte, string[], string> addPeptide){
			int[][] indices = CleavageIndices(protSequence, enzymes, independentEnzymes);
			for (int missedSites = 0; missedSites <= maxMissedSites; missedSites++){
				foreach (int[] index in indices){
					DigestSpecificNoMutations(protSequence, index, missedSites, minPepLen, maxPepLen, addPeptide);
				}
			}
		}

		private static void DigestSpecificAllCodonChanges(string protSequence, byte[] codonInds, int minPepLen,
			int maxPepLen, int maxMissedSites, Enzyme[] enzymes, bool independentEnzymes,
			Action<string, bool, bool, byte, string[], string> addPeptide){
			int[][] indices = CleavageIndices(protSequence, enzymes, independentEnzymes);
			for (int missedSites = 0; missedSites <= maxMissedSites; missedSites++){
				foreach (int[] index in indices){
					DigestSpecificNoMutations(protSequence, index, missedSites, minPepLen, maxPepLen, addPeptide);
				}
			}
			string[] protCodons = GetCodonSequence(protSequence, codonInds);
			for (int missedSites = 0; missedSites <= maxMissedSites + 1; missedSites++){
				foreach (int[] index in indices){
					DigestSpecificAllCodonChanges(protSequence, protCodons, index, missedSites, maxMissedSites, enzymes,
						minPepLen, maxPepLen, addPeptide);
				}
			}
		}

		private static void DigestSpecificKnownMutations(string protSequence, string[] knownMutationAaBefore,
			string[] knownMutationAaAfter, IList<int> knownMutationPos, string[] knownMutationNames,
			Regex variationGroupRegex, int minPepLen, int maxPepLen, int maxMissedSites, Enzyme[] enzymes,
			bool independentEnzymes, Action<string, bool, bool, byte, string[], string> addPeptide,
			int maxSubstitutions, int maxCombiMutations){
			int[][] indices = CleavageIndices(protSequence, enzymes, independentEnzymes);
			for (int missedSites = 0; missedSites <= maxMissedSites; missedSites++){
				foreach (int[] index in indices){
					DigestSpecificNoMutations(protSequence, index, missedSites, minPepLen, maxPepLen, addPeptide);
				}
			}
			GetAltSequence(protSequence, knownMutationAaBefore, knownMutationAaAfter, knownMutationNames,
				out knownMutationAaBefore, out knownMutationAaAfter, out knownMutationNames, knownMutationPos);
			//Go to one more missedSites than necessary in order to be able to treat mutations that remove a splice site
			for (int missedSites = 0; missedSites <= maxMissedSites + 1; missedSites++){
				if (independentEnzymes){
					for (int i = 0; i < indices.Length; i++){
						DigestSpecificKnownMutations(protSequence, knownMutationAaBefore, knownMutationAaAfter,
							knownMutationNames, variationGroupRegex, indices[i], missedSites, maxMissedSites,
							new[]{enzymes[i]}, minPepLen, maxPepLen, addPeptide, maxSubstitutions, maxCombiMutations);
					}
				} else{
					DigestSpecificKnownMutations(protSequence, knownMutationAaBefore, knownMutationAaAfter,
						knownMutationNames, variationGroupRegex, indices[0], missedSites, maxMissedSites, enzymes,
						minPepLen, maxPepLen, addPeptide, maxSubstitutions, maxCombiMutations);
				}
			}
		}

		private static void GetAltSequence(string protSequence, IList<string> knownMutationAaBeforeIn,
			IList<string> knownMutationAaAfterIn, IList<string> knownMutationNamesIn,
			out string[] knownMutationAaBeforeOut, out string[] knownMutationAaAfterOut,
			out string[] knownMutationNamesOut, IList<int> knownMutationPos){
			knownMutationAaBeforeOut = new string[protSequence.Length];
			knownMutationAaAfterOut = new string[protSequence.Length];
			knownMutationNamesOut = knownMutationNamesIn != null ? new string[protSequence.Length] : null;
			for (int i = 0; i < knownMutationPos.Count; i++){
				if (knownMutationPos[i] >= protSequence.Length){
					continue;
				}
				knownMutationAaBeforeOut[knownMutationPos[i]] = knownMutationAaBeforeIn[i];
				knownMutationAaAfterOut[knownMutationPos[i]] = knownMutationAaAfterIn[i];
				if (knownMutationNamesIn != null){
					knownMutationNamesOut[knownMutationPos[i]] = knownMutationNamesIn[i];
				}
			}
		}

		private static bool Valid(EnzymeMode enzymeMode, int[][] indices, int offset, int len, bool isNterm,
			bool isCterm){
			if (enzymeMode == EnzymeMode.Unspecific){
				return true;
			}
			foreach (int[] index in indices){
				if (Valid(enzymeMode, index, offset, len, isNterm, isCterm)){
					return true;
				}
			}
			return false;
		}

		private static bool Valid(EnzymeMode enzymeMode, int[] indices, int offset, int len, bool isNterm,
			bool isCterm){
			switch (enzymeMode){
				case EnzymeMode.Unspecific:
					return true;
				case EnzymeMode.Semispecific:
					return isNterm || isCterm || Array.BinarySearch(indices, offset) >= 0 ||
					       Array.BinarySearch(indices, offset + len) >= 0;
				case EnzymeMode.SemispecificFreeNterm:
					return isCterm || Array.BinarySearch(indices, offset + len) >= 0;
				case EnzymeMode.SemispecificFreeCterm:
					return isNterm || Array.BinarySearch(indices, offset) >= 0;
				default:
					throw new Exception("Never get here.");
			}
		}

		private static void DigestUnspecificAllCodonChanges(string protSequence, byte[] codonInds, int minPepLen,
			int maxPepLen, Action<string, bool, bool, byte, string[], string> addPeptide, EnzymeMode enzymeMode,
			int[][] indices){
			string[] protCodons = GetCodonSequence(protSequence, codonInds);
			for (int offset = 0; offset < protSequence.Length - minPepLen; offset++){
				int maxLen = Math.Min(protSequence.Length - offset, maxPepLen);
				for (int len = minPepLen; len <= maxLen; len++){
					string s = protSequence.Substring(offset, len);
					bool isNterm = offset == 0 || (offset == 1 && protSequence[0] == 'M');
					bool isCterm = offset + len == protSequence.Length;
					if (Valid(enzymeMode, indices, offset, len, isNterm, isCterm)){
						addPeptide(s, isNterm, isCterm, 0, null, null);
						for (int pos = 0; pos < s.Length; pos++){
							string codon = protCodons[offset + pos];
							char[] aas = AminoAcids.CodonMutatesToAa[codon];
							foreach (char aa in aas){
								string newSequence = MutateSingleAaSubstitution(s, pos, aa);
								addPeptide(newSequence, isNterm, isCterm, 1, new[]{codon + "->" + aa}, s);
							}
						}
					}
				}
			}
		}

		private static void DigestUnspecificNoMutations(string protSequence, int minPepLen, int maxPepLen,
			Action<string, bool, bool, byte, string[], string> addPeptide, EnzymeMode enzymeMode, int[][] indices){
			for (int offset = 0; offset < protSequence.Length - minPepLen; offset++){
				int maxLen = Math.Min(protSequence.Length - offset, maxPepLen);
				for (int len = minPepLen; len <= maxLen; len++){
					string s = protSequence.Substring(offset, len);
					bool isNterm = offset == 0 || (offset == 1 && protSequence[0] == 'M');
					bool isCterm = offset + len == protSequence.Length;
					if (Valid(enzymeMode, indices, offset, len, isNterm, isCterm)){
						addPeptide(s, isNterm, isCterm, 0, null, null);
					}
				}
			}
		}

		private static void DigestUnspecificSplicedNoMutations(string protSequence, int minPepLen, int maxPepLen,
			int minOverhang, int maxIntronLen, Action<string, bool, bool, byte, string[], string> addPeptide){
			for (int offset = 0; offset < protSequence.Length - minPepLen; offset++){
				int maxLen = Math.Min(protSequence.Length - offset, maxPepLen);
				for (int len = minPepLen; len <= maxLen; len++){
					string s = protSequence.Substring(offset, len);
					bool isNterm = offset == 0 || (offset == 1 && protSequence[0] == 'M');
					int maxLen0 = Math.Min(protSequence.Length - offset - len, maxIntronLen);
					for (int intronLen = 1; intronLen <= maxLen0; intronLen++){
						for (int leftLen = minOverhang; leftLen <= len - minOverhang; leftLen++){
							string left = protSequence.Substring(offset, leftLen);
							string right = protSequence.Substring(offset + leftLen + intronLen, len - leftLen);
							string s0 = string.Concat(left, right);
							string s1 = string.Concat(right, left);
							if (s0 == s || s1 == s) continue;
							addPeptide(s0, isNterm, offset + len + intronLen == protSequence.Length, 0, null, null);
							//new[] { $"splice_{offset}_{offset + leftLen}_{offset + leftLen + intronLen}_{offset + len + intronLen}" },s
							addPeptide(s1, isNterm, offset + len + intronLen == protSequence.Length, 0, null, null);
							//,new[] { $"splice_{offset + leftLen + intronLen}_{offset + len + intronLen}_{offset}_{offset + leftLen}" },s
						}
					}
				}
			}
		}

		private static void DigestUnspecificKnownMutations(string protSequence, string[] knownMutationAaBefore,
			string[] knownMutationAaAfter, IList<int> knownMutationPos, string[] knownVariationNames,
			Regex variationGroupRegex, int minPepLen, int maxPepLen,
			Action<string, bool, bool, byte, string[], string> addPeptide, EnzymeMode enzymeMode, int[][] indices){
			GetAltSequence(protSequence, knownMutationAaBefore, knownMutationAaAfter, knownVariationNames,
				out knownMutationAaBefore, out knownMutationAaAfter, out knownVariationNames, knownMutationPos);
			for (int offset = 0; offset < protSequence.Length - minPepLen; offset++){
				int maxLen = Math.Min(protSequence.Length - offset, maxPepLen);
				for (int len = minPepLen; len <= maxLen; len++){
					string s = protSequence.Substring(offset, len);
					bool isNterm = offset == 0 || (offset == 1 && protSequence[0] == 'M');
					bool isCterm = offset + len == protSequence.Length;
					if (Valid(enzymeMode, indices, offset, len, isNterm, isCterm)){
						addPeptide(s, isNterm, isCterm, 0, null, null);
						for (int pos = 0; pos < s.Length; pos++){
							string oldAa = knownMutationAaBefore[offset + pos];
							string newAa = knownMutationAaAfter[offset + pos];
							if (oldAa != null && newAa != null && newAa.Length != 0){
								//TODO: works only for single aa substitutions
								char aa = newAa[0];
								string newSequence = MutateSingleAaSubstitution(s, pos, aa);
								string[] names = (knownVariationNames != null)
									? new[]{knownVariationNames[offset + pos]}
									: null;
								addPeptide(newSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex),
									names, s);
							}
						}
					}
				}
			}
		}

		private static byte GetMutationLevel(string[] names, Regex variationGroupRegex){
			if (variationGroupRegex == null || names == null || names.Length < 1){
				return 1;
			}
			if (!variationGroupRegex.IsMatch(names[0])){
				return 1;
			}
			return 2;
		}

		private static string[] GetCodonSequence(string protSequence, IList<byte> codonInds){
			string[] result = new string[protSequence.Length];
			for (int i = 0; i < protSequence.Length; i++){
				AminoAcid aa = AminoAcids.FromLetter(protSequence[i]);
				if (aa != null){
					result[i] = aa.Codons[codonInds[i]];
				}
			}
			return result;
		}

		private static bool ValidPeptide3(string pep, int minPepLen){
			if (pep.Length < minPepLen){
				return false;
			}
			foreach (char t in pep){
				if (AminoAcids.SingleLetterAas.IndexOf(t) == -1){
					return false;
				}
			}
			return true;
		}

		private static IEnumerable<string> Digest(string s, IList<int> indices){
			int count = indices.Count;
			List<string> result = new List<string>();
			if (count < 0){ } else if (count == 0){
				if (ValidPeptide(s)){
					result.Add(s);
				}
			} else{
				string p = s.Substring(0, indices[0]);
				if (ValidPeptide(p)){
					result.Add(p);
				}
				for (int i = 1; i < count; i++){
					p = s.Substring(indices[i - 1], indices[i] - indices[i - 1]);
					if (ValidPeptide(p)){
						result.Add(p);
					}
				}
				p = s.Substring(indices[count - 1], s.Length - indices[count - 1]);
				if (ValidPeptide(p)){
					result.Add(p);
				}
			}
			return result.ToArray();
		}

		private static void DigestSpecificNoMutations(string protSequence, IList<int> indices, int missedSites,
			int minPepLen, int maxPepLen, Action<string, bool, bool, byte, string[], string> addPeptide){
			int count = indices.Count;
			if (count < missedSites){ } else if (count == missedSites){
				if (ValidPeptide2(protSequence, minPepLen, maxPepLen)){
					addPeptide(protSequence, true, true, 0, null, null);
					if (protSequence.StartsWith("M") && protSequence.Length > minPepLen){
						addPeptide(protSequence.Substring(1), true, true, 0, null, null);
					}
				}
			} else{
				string pepSequence = protSequence.Substring(0, indices[missedSites]);
				if (ValidPeptide2(pepSequence, minPepLen, maxPepLen)){
					addPeptide(pepSequence, true, false, 0, null, null);
					if (protSequence.StartsWith("M") && pepSequence.Length > minPepLen){
						addPeptide(pepSequence.Substring(1), true, false, 0, null, null);
					}
				}
				for (int i = 1; i < count - missedSites; i++){
					pepSequence = protSequence.Substring(indices[i - 1], indices[i + missedSites] - indices[i - 1]);
					if (ValidPeptide2(pepSequence, minPepLen, maxPepLen)){
						addPeptide(pepSequence, false, false, 0, null, null);
					}
				}
				pepSequence = protSequence.Substring(indices[count - 1 - missedSites],
					protSequence.Length - indices[count - 1 - missedSites]);
				if (ValidPeptide2(pepSequence, minPepLen, maxPepLen)){
					addPeptide(pepSequence, false, true, 0, null, null);
				}
			}
		}

		private static void DigestSpecificKnownMutations(string protSequence, IList<string> knownMutationAaBefore,
			IList<string> knownMutationAaAfter, IList<string> knownMutationNames, Regex variationGroupRegex,
			IList<int> indices, int missedCleaves, int maxMissedCleaves, Enzyme[] enzymes, int minPepLen, int maxPepLen,
			Action<string, bool, bool, byte, string[], string> addPeptide, int maxSubstitutions, int maxCombiMutations){
			int count = indices.Count;
			if (count < missedCleaves){
				//There are fewer possible cleavage sites in the protein than required. Do nothing.
			} else if (count == missedCleaves){
				//The number of possible cleavage sites in the protein is equal to the number of missed cleavages. Add the whole protein sequence.
				if (ValidPeptide3(protSequence, minPepLen)){
					AddPeptideKnownMutations(protSequence, '*', '*', knownMutationAaBefore, knownMutationAaAfter,
						knownMutationNames, variationGroupRegex, true, true, addPeptide, enzymes, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, maxSubstitutions, maxCombiMutations);
					if (protSequence.StartsWith("M") && protSequence.Length > minPepLen){
						AddPeptideKnownMutations(protSequence.Substring(1), '*', '*',
							ArrayUtils.SubArray(knownMutationAaBefore, 1, knownMutationAaBefore.Count),
							ArrayUtils.SubArray(knownMutationAaAfter, 1, knownMutationAaAfter.Count),
							knownMutationNames != null
								? ArrayUtils.SubArray(knownMutationNames, 1, knownMutationNames.Count)
								: null, variationGroupRegex, true, true, addPeptide, enzymes, missedCleaves,
							maxMissedCleaves, minPepLen, maxPepLen, maxSubstitutions, maxCombiMutations);
					}
				}
			} else{
				string pepSequence = protSequence.Substring(0, indices[missedCleaves]);
				string[] pepKnownMutationAaBefore =
					ArrayUtils.SubArray(knownMutationAaBefore, 0, indices[missedCleaves] + 1);
				string[] pepKnownMutationAaAfter =
					ArrayUtils.SubArray(knownMutationAaAfter, 0, indices[missedCleaves] + 1);
				string[] pepKnownMutationNames = knownMutationNames != null
					? ArrayUtils.SubArray(knownMutationNames, 0, indices[missedCleaves] + 1)
					: null;
				if (ValidPeptide3(pepSequence, minPepLen)){
					AddPeptideKnownMutations(pepSequence, '*', protSequence[indices[missedCleaves]],
						pepKnownMutationAaBefore, pepKnownMutationAaAfter, pepKnownMutationNames, variationGroupRegex,
						true, false, addPeptide, enzymes, missedCleaves, maxMissedCleaves, minPepLen, maxPepLen,
						maxSubstitutions, maxCombiMutations);
					if (protSequence.StartsWith("M") && pepSequence.Length > minPepLen){
						AddPeptideKnownMutations(pepSequence.Substring(1), '*', protSequence[indices[missedCleaves]],
							ArrayUtils.SubArray(pepKnownMutationAaBefore, 1, pepKnownMutationAaBefore.Length),
							ArrayUtils.SubArray(pepKnownMutationAaAfter, 1, pepKnownMutationAaAfter.Length),
							pepKnownMutationNames != null
								? ArrayUtils.SubArray(pepKnownMutationNames, 1, pepKnownMutationNames.Length)
								: null, variationGroupRegex, true, false, addPeptide, enzymes, missedCleaves,
							maxMissedCleaves, minPepLen, maxPepLen, maxSubstitutions, maxCombiMutations);
					}
				}
				for (int i = 1; i < count - missedCleaves; i++){
					pepSequence = protSequence.Substring(indices[i - 1], indices[i + missedCleaves] - indices[i - 1]);
					pepKnownMutationAaBefore = ArrayUtils.SubArray(knownMutationAaBefore, indices[i - 1],
						indices[i + missedCleaves] + 1);
					pepKnownMutationAaAfter = ArrayUtils.SubArray(knownMutationAaAfter, indices[i - 1],
						indices[i + missedCleaves] + 1);
					pepKnownMutationNames = knownMutationNames != null
						? ArrayUtils.SubArray(knownMutationNames, indices[i - 1], indices[i + missedCleaves] + 1)
						: null;
					if (ValidPeptide3(pepSequence, minPepLen)){
						AddPeptideKnownMutations(pepSequence, protSequence[indices[i - 1] - 1],
							protSequence[indices[i + missedCleaves]], pepKnownMutationAaBefore, pepKnownMutationAaAfter,
							pepKnownMutationNames, variationGroupRegex, false, false, addPeptide, enzymes,
							missedCleaves, maxMissedCleaves, minPepLen, maxPepLen, maxSubstitutions, maxCombiMutations);
					}
				}
				pepSequence = protSequence.Substring(indices[count - 1 - missedCleaves],
					protSequence.Length - indices[count - 1 - missedCleaves]);
				pepKnownMutationAaBefore = ArrayUtils.SubArray(knownMutationAaBefore,
					indices[count - 1 - missedCleaves], protSequence.Length);
				pepKnownMutationAaAfter = ArrayUtils.SubArray(knownMutationAaAfter, indices[count - 1 - missedCleaves],
					protSequence.Length);
				pepKnownMutationNames = knownMutationNames != null
					? ArrayUtils.SubArray(knownMutationNames, indices[count - 1 - missedCleaves], protSequence.Length)
					: null;
				if (ValidPeptide3(pepSequence, minPepLen)){
					AddPeptideKnownMutations(pepSequence, protSequence[indices[count - 1 - missedCleaves] - 1], '*',
						pepKnownMutationAaBefore, pepKnownMutationAaAfter, pepKnownMutationNames, variationGroupRegex,
						false, true, addPeptide, enzymes, missedCleaves, maxMissedCleaves, minPepLen, maxPepLen,
						maxSubstitutions, maxCombiMutations);
				}
			}
		}

		private static void DigestSpecificAllCodonChanges(string protSequence, IList<string> protCodons,
			IList<int> indices, int missedCleaves, int maxMissedCleaves, Enzyme[] enzymes, int minPepLen, int maxPepLen,
			Action<string, bool, bool, byte, string[], string> addPeptide){
			int count = indices.Count;
			if (count < missedCleaves){ } else if (count == missedCleaves){
				if (ValidPeptide3(protSequence, minPepLen)){
					AddPeptideAllCodonChanges(protSequence, '*', '*', protCodons, true, true, addPeptide, enzymes,
						missedCleaves, maxMissedCleaves, minPepLen, maxPepLen);
					if (protSequence.StartsWith("M") && protSequence.Length > minPepLen){
						AddPeptideAllCodonChanges(protSequence.Substring(1), '*', '*',
							ArrayUtils.SubArray(protCodons, 1, protCodons.Count), true, true, addPeptide, enzymes,
							missedCleaves, maxMissedCleaves, minPepLen, maxPepLen);
					}
				}
			} else{
				string pepSequence = protSequence.Substring(0, indices[missedCleaves]);
				string[] pepCodons = ArrayUtils.SubArray(protCodons, 0, indices[missedCleaves]);
				if (ValidPeptide3(pepSequence, minPepLen)){
					AddPeptideAllCodonChanges(pepSequence, '*', protSequence[indices[missedCleaves]], pepCodons, true,
						false, addPeptide, enzymes, missedCleaves, maxMissedCleaves, minPepLen, maxPepLen);
					if (protSequence.StartsWith("M") && pepSequence.Length > minPepLen){
						AddPeptideAllCodonChanges(pepSequence.Substring(1), '*', protSequence[indices[missedCleaves]],
							ArrayUtils.SubArray(pepCodons, 1, pepCodons.Length), true, false, addPeptide, enzymes,
							missedCleaves, maxMissedCleaves, minPepLen, maxPepLen);
					}
				}
				for (int i = 1; i < count - missedCleaves; i++){
					pepSequence = protSequence.Substring(indices[i - 1], indices[i + missedCleaves] - indices[i - 1]);
					pepCodons = ArrayUtils.SubArray(protCodons, indices[i - 1], indices[i + missedCleaves]);
					if (ValidPeptide3(pepSequence, minPepLen)){
						AddPeptideAllCodonChanges(pepSequence, protSequence[indices[i - 1] - 1],
							protSequence[indices[i + missedCleaves]], pepCodons, false, false, addPeptide, enzymes,
							missedCleaves, maxMissedCleaves, minPepLen, maxPepLen);
					}
				}
				pepSequence = protSequence.Substring(indices[count - 1 - missedCleaves],
					protSequence.Length - indices[count - 1 - missedCleaves]);
				pepCodons = ArrayUtils.SubArray(protCodons, indices[count - 1 - missedCleaves], protSequence.Length);
				if (ValidPeptide3(pepSequence, minPepLen)){
					AddPeptideAllCodonChanges(pepSequence, protSequence[indices[count - 1 - missedCleaves] - 1], '*',
						pepCodons, false, true, addPeptide, enzymes, missedCleaves, maxMissedCleaves, minPepLen,
						maxPepLen);
				}
			}
		}

		private static int[] GetPositions(int len, IList<string> knownMutationAaBefore,
			IList<string> knownMutationAaAfter, IList<string> knownMutationNames, out AaMutationType[] types,
			out int[] oldAaLens, out string[] newAas, out string[] names){
			List<int> positions = new List<int>();
			List<AaMutationType> types1 = new List<AaMutationType>();
			List<int> oldAaLens1 = new List<int>();
			List<string> newAas1 = new List<string>();
			List<string> names1 = new List<string>();
			for (int pos = 0; pos <= len; pos++){
				if (pos == len && knownMutationAaBefore.Count < len + 1){
					break;
				}
				string oldAa = knownMutationAaBefore[pos];
				if (oldAa == null){
					continue;
				}
				string newAa = knownMutationAaAfter[pos];
				AaMutationType type = GetMutationType(oldAa, newAa);
				if (pos < len){
					positions.Add(pos);
					types1.Add(type);
					oldAaLens1.Add(oldAa.Length);
					newAas1.Add(newAa);
					if (knownMutationNames != null){
						names1.Add(knownMutationNames[pos]);
					}
				} else if (type == AaMutationType.SingleAaInsertion || type == AaMutationType.MultiAaInsertion ||
				           type == AaMutationType.MultiAaInsertionWithStopCodon){
					positions.Add(pos);
					types1.Add(type);
					oldAaLens1.Add(oldAa.Length);
					newAas1.Add(newAa);
					if (knownMutationNames != null){
						names1.Add(knownMutationNames[pos]);
					}
				}
			}
			types = types1.ToArray();
			oldAaLens = oldAaLens1.ToArray();
			newAas = newAas1.ToArray();
			names = knownMutationNames != null ? names1.ToArray() : null;
			return positions.ToArray();
		}

		private static void AddPeptideKnownMutations(string sequence, char previousAa, char nextAa,
			IList<string> knownMutationAaBefore, IList<string> knownMutationAaAfter, IList<string> knownMutationNames,
			Regex variationGroupRegex, bool isNterm, bool isCterm,
			Action<string, bool, bool, byte, string[], string> addPeptide, Enzyme[] enzymes, int missedCleaves,
			int maxMissedCleaves, int minPepLen, int maxPepLen, int maxSubstitutions, int maxCombiMutations){
			int len = sequence.Length;
			int[] positions = GetPositions(len, knownMutationAaBefore, knownMutationAaAfter, knownMutationNames,
				out AaMutationType[] types, out int[] oldAaLens, out string[] newAas, out string[] names);
			for (int i = 0; i < positions.Length; i++){
				AddPeptideKnownMutations(sequence, previousAa, nextAa, isNterm, isCterm, addPeptide, enzymes,
					missedCleaves, maxMissedCleaves, minPepLen, maxPepLen, positions[i], oldAaLens[i], newAas[i],
					types[i], names?[i], variationGroupRegex);
			}
			int[] substInds = GetSingleAaSubstInds(types); //TODO count not just SingleAaSubstitution
			if (substInds.Length < 2){
				return;
			}
			positions = ArrayUtils.SubArray(positions, substInds);
			char[] newAa1 = new char[substInds.Length];
			string[] names1 = names != null ? new string[substInds.Length] : null;
			for (int i = 0; i < substInds.Length; i++){
				newAa1[i] = newAas[substInds[i]][0];
				if (names != null){
					names1[i] = names[substInds[i]];
				}
			}
			for (int nsub = 2; nsub <= Math.Min(maxSubstitutions, positions.Length); nsub++){
				int[][] comb = NumUtils.GetCombinations(positions.Length, nsub, maxCombiMutations, out bool incompl);
				foreach (int[] inds in comb){
					int[] posi = ArrayUtils.SubArray(positions, inds);
					char[] newAa2 = ArrayUtils.SubArray(newAa1, inds);
					string[] names2 = names != null ? ArrayUtils.SubArray(names1, inds) : null;
					AddPeptideMultipleSubstitutions(sequence, isNterm, isCterm, addPeptide, enzymes, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, posi, newAa2, names2, variationGroupRegex);
				}
			}
		}

		/// <summary>
		/// Apply multiple different single amino acid substititions to the same peptide.
		/// </summary>
		private static void AddPeptideMultipleSubstitutions(string sequence, bool isNterm, bool isCterm,
			Action<string, bool, bool, byte, string[], string> addPeptide, Enzyme[] enzymes, int missedCleaves,
			int maxMissedCleaves, int minPepLen, int maxPepLen, int[] pos, char[] newAa, string[] names,
			Regex variationGroupRegex){
			string newSequence = MutateSeveralSingleAaSubstitution(sequence, pos, newAa);
			bool[] cleavesBefore = GetCleavagePattern(sequence, enzymes);
			bool[] cleavesAfter = GetCleavagePattern(newSequence, enzymes);
			List<int> cleaveRemoved = new List<int>();
			List<int> cleaveAdded = new List<int>();
			for (int i = 0; i < cleavesBefore.Length; i++){
				if (cleavesBefore[i] && !cleavesAfter[i]){
					cleaveRemoved.Add(i);
				}
				if (!cleavesBefore[i] && cleavesAfter[i]){
					cleaveAdded.Add(i);
				}
			}
			int deltaCleave = cleaveAdded.Count - cleaveRemoved.Count;
			if (missedCleaves + deltaCleave <= maxMissedCleaves){
				if (newSequence.Length <= maxPepLen){
					addPeptide(newSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex), names,
						sequence);
				}
			}
			foreach (int ind in cleaveAdded){
				bool leftSide = LeftSideHasMutations(pos, ind);
				if (leftSide){
					string s = newSequence.Substring(0, ind + 1);
					if (s.Length >= minPepLen){
						addPeptide(s, isNterm, false, GetMutationLevel(names, variationGroupRegex), names, sequence);
					}
				}
				bool rightSide = RightSideHasMutations(pos, ind);
				if (rightSide){
					string s = newSequence.Substring(ind + 1, newSequence.Length - ind - 1);
					if (s.Length >= minPepLen){
						addPeptide(s, false, isCterm, GetMutationLevel(names, variationGroupRegex), names, sequence);
					}
				}
			}
		}

		private static bool RightSideHasMutations(IEnumerable<int> pos, int ind){
			//pos contains something > ind+1
			foreach (int i in pos){
				if (i > ind + 1){
					return true;
				}
			}
			return false;
		}

		private static bool LeftSideHasMutations(IEnumerable<int> pos, int ind){
			//pos contains something < ind
			foreach (int i in pos){
				if (i < ind){
					return true;
				}
			}
			return false;
		}

		private static bool[] GetCleavagePattern(string sequence, Enzyme[] enzymes){
			bool[] result = new bool[sequence.Length - 1];
			for (int i = 0; i < result.Length; i++){
				result[i] = Cleaves(sequence[i], sequence[i + 1], enzymes);
			}
			return result;
		}

		private static int[] GetSingleAaSubstInds(IList<AaMutationType> types){
			List<int> result = new List<int>();
			for (int i = 0; i < types.Count; i++){
				if (types[i] == AaMutationType.SingleAaSubstitution){
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Apply exhaustively all mutations to a peptide. There can be only one mutation per peptide here.
		/// </summary>
		private static void AddPeptideKnownMutations(string sequence, char previousAa, char nextAa, bool isNterm,
			bool isCterm, Action<string, bool, bool, byte, string[], string> addPeptide, Enzyme[] enzymes,
			int missedCleaves, int maxMissedCleaves, int minPepLen, int maxPepLen, int pos, int oldAaLen, string newAa,
			AaMutationType type, string name, Regex variationGroupRegex){
			int len = sequence.Length;
			switch (type){
				case AaMutationType.SingleAaSubstitution:{
					char aa = newAa[0];
					bool cleaveLeftBefore;
					bool cleaveRightBefore;
					bool cleaveLeftAfter;
					bool cleaveRightAfter;
					if (pos == 0){
						cleaveLeftAfter = Cleaves(previousAa, aa, enzymes);
						if (!cleaveLeftAfter){
							//taken care of with higher missed cleavages
							return;
						}
						cleaveLeftBefore = true;
						cleaveRightBefore = Cleaves(sequence[pos], sequence[pos + 1], enzymes);
						cleaveRightAfter = Cleaves(aa, sequence[pos + 1], enzymes);
					} else if (pos == len - 1){
						cleaveRightAfter = Cleaves(aa, nextAa, enzymes);
						if (!cleaveRightAfter){
							//taken care of with higher missed cleavages
							return;
						}
						cleaveRightBefore = true;
						cleaveLeftBefore = Cleaves(sequence[pos - 1], sequence[pos], enzymes);
						cleaveLeftAfter = Cleaves(sequence[pos - 1], aa, enzymes);
					} else{
						cleaveLeftBefore = Cleaves(sequence[pos - 1], sequence[pos], enzymes);
						cleaveRightBefore = Cleaves(sequence[pos], sequence[pos + 1], enzymes);
						cleaveLeftAfter = Cleaves(sequence[pos - 1], aa, enzymes);
						cleaveRightAfter = Cleaves(aa, sequence[pos + 1], enzymes);
					}
					ProcessSingleAaSubstitution(cleaveLeftBefore, cleaveRightBefore, cleaveLeftAfter, cleaveRightAfter,
						missedCleaves, maxMissedCleaves, minPepLen, maxPepLen, sequence, pos, aa, isNterm, isCterm,
						addPeptide, name != null ? new[]{name} : new string[0]);
				}
					break;
				case AaMutationType.StopCodonInsertion:
					ProcessStopCodonInsertion(sequence.Substring(0, pos), isNterm, addPeptide, minPepLen,
						name != null ? new[]{name} : new string[0], variationGroupRegex, sequence);
					break;
				case AaMutationType.SingleAaDeletion:{
					bool cleaveLeftBefore;
					bool cleaveRightBefore;
					bool cleaveAfter;
					if (pos == 0){
						cleaveAfter = Cleaves(previousAa, sequence[pos + 1], enzymes);
						if (!cleaveAfter){
							//taken care of with higher missed cleavages
							return;
						}
						cleaveLeftBefore = true;
						cleaveRightBefore = Cleaves(sequence[pos], sequence[pos + 1], enzymes);
					} else if (pos == len - 1){
						cleaveAfter = Cleaves(sequence[pos - 1], nextAa, enzymes);
						if (!cleaveAfter){
							//taken care of with higher missed cleavages
							return;
						}
						cleaveRightBefore = true;
						cleaveLeftBefore = Cleaves(sequence[pos - 1], sequence[pos], enzymes);
					} else{
						cleaveLeftBefore = Cleaves(sequence[pos - 1], sequence[pos], enzymes);
						cleaveRightBefore = Cleaves(sequence[pos], sequence[pos + 1], enzymes);
						cleaveAfter = Cleaves(sequence[pos - 1], sequence[pos + 1], enzymes);
					}
					ProcessSingleAaDeletion(cleaveLeftBefore, cleaveRightBefore, cleaveAfter, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, sequence, pos, isNterm, isCterm, addPeptide,
						name != null ? new[]{name} : new string[0], variationGroupRegex);
				}
					break;
				case AaMutationType.SingleAaInsertion:{
					char aa = newAa[0];
					bool cleaveBefore;
					bool cleaveLeftAfter;
					bool cleaveRightAfter;
					if (pos == 0){
						cleaveLeftAfter = Cleaves(previousAa, aa, enzymes);
						if (!cleaveLeftAfter){
							//taken care of with higher missed cleavages
							return;
						}
						cleaveBefore = true;
						cleaveRightAfter = Cleaves(aa, sequence[pos], enzymes);
					} else if (pos == len){
						cleaveRightAfter = Cleaves(aa, nextAa, enzymes);
						if (!cleaveRightAfter){
							//taken care of with higher missed cleavages
							return;
						}
						cleaveBefore = true;
						cleaveLeftAfter = Cleaves(sequence[pos - 1], aa, enzymes);
					} else{
						cleaveBefore = Cleaves(sequence[pos - 1], sequence[pos], enzymes);
						cleaveLeftAfter = Cleaves(sequence[pos - 1], aa, enzymes);
						cleaveRightAfter = Cleaves(aa, sequence[pos], enzymes);
					}
					ProcessSingleAaInsertion(cleaveBefore, cleaveLeftAfter, cleaveRightAfter, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, sequence, pos, aa, isNterm, isCterm, addPeptide,
						name != null ? new[]{name} : new string[0], variationGroupRegex);
				}
					break;
				case AaMutationType.MultiAaInsertionWithStopCodon:{
					string newSequence = sequence.Substring(0, pos) + newAa.Substring(0, newAa.Length - 1);
					ProcessNewSequence(newSequence, isNterm, true, addPeptide, enzymes, missedCleaves, maxMissedCleaves,
						minPepLen, maxPepLen, name != null ? new[]{name} : new string[0], variationGroupRegex,
						sequence);
				}
					break;
				case AaMutationType.MultiAaInsertion:{
					string newSequence = sequence.Substring(0, pos) + newAa +
					                     sequence.Substring(pos, sequence.Length - pos);
					ProcessNewSequence(newSequence, isNterm, isCterm, addPeptide, enzymes, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, name != null ? new[]{name} : new string[0],
						variationGroupRegex, sequence);
				}
					break;
				case AaMutationType.MultiAaDeletion:{
					if (pos + oldAaLen > sequence.Length){
						return;
					}
					string newSequence = sequence.Substring(0, pos) +
					                     sequence.Substring(pos + oldAaLen, sequence.Length - pos - oldAaLen);
					ProcessNewSequence(newSequence, isNterm, isCterm, addPeptide, enzymes, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, name != null ? new[]{name} : new string[0],
						variationGroupRegex, sequence);
				}
					break;
				case AaMutationType.ComplexSubstitution:{
					if (pos + oldAaLen > sequence.Length){
						return;
					}
					string newSequence = sequence.Substring(0, pos) + newAa +
					                     sequence.Substring(pos + oldAaLen, sequence.Length - pos - oldAaLen);
					ProcessNewSequence(newSequence, isNterm, isCterm, addPeptide, enzymes, missedCleaves,
						maxMissedCleaves, minPepLen, maxPepLen, name != null ? new[]{name} : new string[0],
						variationGroupRegex, sequence);
				}
					break;
			}
		}

		private static void ProcessNewSequence(string newSequence, bool isNterm, bool isCterm,
			Action<string, bool, bool, byte, string[], string> addPeptide, Enzyme[] enzymes, int missedCleaves,
			int maxMissedCleaves, int minPepLen, int maxPepLen, string[] names, Regex variationGroupRegex,
			string baseSeq){
			missedCleaves = Math.Min(missedCleaves, maxMissedCleaves);
			int[][] indices = CleavageIndices(newSequence, enzymes, false);
			foreach (int[] index in indices){
				DigestSpecificSubsequence(newSequence, isNterm, isCterm, index, missedCleaves, minPepLen, maxPepLen,
					addPeptide, names, variationGroupRegex, baseSeq);
			}
		}

		private static void DigestSpecificSubsequence(string protSequence, bool isNterm, bool isCterm,
			IList<int> indices, int missedSites, int minPepLen, int maxPepLen,
			Action<string, bool, bool, byte, string[], string> addPeptide, string[] names, Regex variationGroupRegex,
			string baseSeq){
			int count = indices.Count;
			if (count < missedSites){ } else if (count == missedSites){
				if (ValidPeptide2(protSequence, minPepLen, maxPepLen)){
					addPeptide(protSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex), names,
						baseSeq);
					if (isNterm && protSequence.StartsWith("M") && protSequence.Length > minPepLen){
						addPeptide(protSequence.Substring(1), true, isCterm,
							GetMutationLevel(names, variationGroupRegex), names, baseSeq);
					}
				}
			} else{
				string pepSequence = protSequence.Substring(0, indices[missedSites]);
				if (ValidPeptide2(pepSequence, minPepLen, maxPepLen)){
					addPeptide(pepSequence, true, false, GetMutationLevel(names, variationGroupRegex), names, baseSeq);
					if (isNterm && protSequence.StartsWith("M") && pepSequence.Length > minPepLen){
						addPeptide(pepSequence.Substring(1), true, false, GetMutationLevel(names, variationGroupRegex),
							names, baseSeq);
					}
				}
				for (int i = 1; i < count - missedSites; i++){
					pepSequence = protSequence.Substring(indices[i - 1], indices[i + missedSites] - indices[i - 1]);
					if (ValidPeptide2(pepSequence, minPepLen, maxPepLen)){
						addPeptide(pepSequence, false, false, GetMutationLevel(names, variationGroupRegex), names,
							baseSeq);
					}
				}
				pepSequence = protSequence.Substring(indices[count - 1 - missedSites],
					protSequence.Length - indices[count - 1 - missedSites]);
				if (ValidPeptide2(pepSequence, minPepLen, maxPepLen)){
					addPeptide(pepSequence, false, isCterm, GetMutationLevel(names, variationGroupRegex), names,
						baseSeq);
				}
			}
		}

		private static void ProcessSingleAaInsertion(bool cleaveBefore, bool cleaveLeftAfter, bool cleaveRightAfter,
			int missedCleaves, int maxMissedCleaves, int minPepLen, int maxPepLen, string sequence, int pos, char aa,
			bool isNterm, bool isCterm, Action<string, bool, bool, byte, string[], string> addPeptide, string[] names,
			Regex variationGroupRegex){
			int numC = 0;
			if (cleaveBefore){
				numC--;
			}
			if (cleaveLeftAfter){
				numC++;
			}
			if (cleaveRightAfter){
				numC++;
			}
			bool cleaveIncreases = numC > 0;
			bool cleaveDecreases = numC < 0;
			if (missedCleaves > maxMissedCleaves && !cleaveDecreases){
				return;
			}
			string newSequence = sequence.Substring(0, pos) + aa + sequence.Substring(pos, sequence.Length - pos);
			if (cleaveIncreases){
				if (missedCleaves < maxMissedCleaves){
					if (newSequence.Length <= maxPepLen){
						addPeptide(newSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex), names,
							sequence);
					}
				}
				if (!cleaveBefore){
					if (cleaveLeftAfter){
						string seq1 = newSequence.Substring(0, pos);
						string seq2 = newSequence.Substring(pos, sequence.Length - pos);
						if (seq1.Length >= minPepLen && seq1.Length <= maxPepLen){
							addPeptide(seq1, isNterm, false, GetMutationLevel(names, variationGroupRegex), names,
								sequence);
						}
						if (seq2.Length >= minPepLen && seq2.Length <= maxPepLen){
							addPeptide(seq2, false, isCterm, GetMutationLevel(names, variationGroupRegex), names,
								sequence);
						}
					}
					if (cleaveRightAfter){
						string seq1 = newSequence.Substring(0, pos + 1);
						string seq2 = newSequence.Substring(pos, sequence.Length - pos - 1);
						if (seq1.Length >= minPepLen && seq1.Length <= maxPepLen){
							addPeptide(seq1, isNterm, false, GetMutationLevel(names, variationGroupRegex), names,
								sequence);
						}
						if (seq2.Length >= minPepLen && seq2.Length <= maxPepLen){
							addPeptide(seq2, false, isCterm, GetMutationLevel(names, variationGroupRegex), names,
								sequence);
						}
					}
				} else{
					if (cleaveLeftAfter){
						string seq2 = newSequence.Substring(pos, sequence.Length - pos);
						if (seq2.Length >= minPepLen && seq2.Length <= maxPepLen){
							addPeptide(seq2, false, isCterm, GetMutationLevel(names, variationGroupRegex), names,
								sequence);
						}
					}
					if (cleaveRightAfter){
						string seq1 = newSequence.Substring(0, pos + 1);
						if (seq1.Length >= minPepLen && seq1.Length <= maxPepLen){
							addPeptide(seq1, isNterm, false, GetMutationLevel(names, variationGroupRegex), names,
								sequence);
						}
					}
				}
			} else{
				if (newSequence.Length <= maxPepLen){
					addPeptide(newSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex), names,
						sequence);
				}
			}
		}

		private static void ProcessSingleAaDeletion(bool cleaveLeftBefore, bool cleaveRightBefore, bool cleaveAfter,
			int missedCleaves, int maxMissedCleaves, int minPepLen, int maxPepLen, string sequence, int pos,
			bool isNterm, bool isCterm, Action<string, bool, bool, byte, string[], string> addPeptide, string[] names,
			Regex variationGroupRegex){
			int numC = 0;
			if (cleaveLeftBefore){
				numC--;
			}
			if (cleaveRightBefore){
				numC--;
			}
			if (cleaveAfter){
				numC++;
			}
			bool cleaveIncreases = numC > 0;
			bool cleaveDecreases = numC < 0;
			if (missedCleaves > maxMissedCleaves && !cleaveDecreases){
				return;
			}
			string newSequence = sequence.Substring(0, pos) + sequence.Substring(pos + 1, sequence.Length - pos - 1);
			if (cleaveIncreases){
				if (missedCleaves < maxMissedCleaves){
					if (newSequence.Length <= maxPepLen){
						addPeptide(newSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex), names,
							sequence);
					}
				}
				string seq1 = newSequence.Substring(0, pos);
				string seq2 = newSequence.Substring(pos + 1, newSequence.Length - pos - 1);
				if (seq1.Length >= minPepLen && seq1.Length <= maxPepLen){
					addPeptide(seq1, isNterm, false, GetMutationLevel(names, variationGroupRegex), names, sequence);
				}
				if (seq2.Length >= minPepLen && seq2.Length <= maxPepLen){
					addPeptide(seq2, false, isCterm, GetMutationLevel(names, variationGroupRegex), names, sequence);
				}
			} else{
				if (newSequence.Length <= maxPepLen){
					addPeptide(newSequence, isNterm, isCterm, GetMutationLevel(names, variationGroupRegex), names,
						sequence);
				}
			}
		}

		/// <summary>
		/// Determine from the amino acid sequences stretches before and after the mutation what the type of the mutation is
		/// </summary>
		/// <param name="oldAa">Amino acid sequence stretch before the mutation has been applied.</param>
		/// <param name="newAa">Amino acid sequence stretch after the mutation has been applied.</param>
		/// <returns>The type of the mutation</returns>
		private static AaMutationType GetMutationType(string oldAa, string newAa){
			switch (oldAa.Length){
				case 0:
					if (newAa.Length == 1){
						return newAa[0] == '*' ? AaMutationType.StopCodonInsertion : AaMutationType.SingleAaInsertion;
					}
					return newAa[newAa.Length - 1] == '*'
						? AaMutationType.MultiAaInsertionWithStopCodon
						: AaMutationType.MultiAaInsertion;
				case 1:
					switch (newAa.Length){
						case 0:
							return AaMutationType.SingleAaDeletion;
						case 1:
							return newAa[0] == '*'
								? AaMutationType.StopCodonInsertion
								: AaMutationType.SingleAaSubstitution;
						default:
							return newAa[newAa.Length - 1] == '*'
								? AaMutationType.MultiAaInsertionWithStopCodon
								: AaMutationType.ComplexSubstitution;
					}
				default:
					switch (newAa.Length){
						case 0:
							return AaMutationType.MultiAaDeletion;
						case 1:
							return newAa[0] == '*'
								? AaMutationType.StopCodonInsertion
								: AaMutationType.ComplexSubstitution;
						default:
							return newAa[newAa.Length - 1] == '*'
								? AaMutationType.MultiAaInsertionWithStopCodon
								: AaMutationType.ComplexSubstitution;
					}
			}
		}

		private static void AddPeptideAllCodonChanges(string sequence, char aaBefore, char aaAfter,
			IList<string> codons, bool isNterm, bool isCterm,
			Action<string, bool, bool, byte, string[], string> addPeptide, Enzyme[] enzymes, int missedCleaves,
			int maxMissedCleaves, int minPepLen, int maxPepLen){
			int len = sequence.Length;
			{
				string codon = codons[0];
				if (codon != null){
					char[] aas = AminoAcids.CodonMutatesToAa[codon];
					bool cleaveRightBefore = Cleaves(sequence[0], sequence[1], enzymes);
					foreach (char aa in aas){
						bool cleaveLeftAfter = Cleaves(aaBefore, aa, enzymes);
						if (!cleaveLeftAfter){
							continue;
						}
						bool cleaveRightAfter = Cleaves(aa, sequence[1], enzymes);
						ProcessSingleAaSubstitution(true, cleaveRightBefore, true, cleaveRightAfter, missedCleaves,
							maxMissedCleaves, minPepLen, maxPepLen, sequence, 0, aa, isNterm, isCterm, addPeptide,
							new[]{codon + "->" + aa});
					}
				}
			}
			for (int pos = 1; pos < len - 1; pos++){
				string codon = codons[pos];
				if (codon == null){
					continue;
				}
				char[] aas = AminoAcids.CodonMutatesToAa[codon];
				bool cleaveLeftBefore = Cleaves(sequence[pos - 1], sequence[pos], enzymes);
				bool cleaveRightBefore = Cleaves(sequence[pos], sequence[pos + 1], enzymes);
				foreach (char aa in aas){
					if (aa == '*'){
						ProcessStopCodonInsertion(sequence.Substring(0, pos), isNterm, addPeptide, minPepLen,
							new[]{codon + "->" + aa}, null, sequence);
					} else{
						bool cleaveLeftAfter = Cleaves(sequence[pos - 1], aa, enzymes);
						bool cleaveRightAfter = Cleaves(aa, sequence[pos + 1], enzymes);
						ProcessSingleAaSubstitution(cleaveLeftBefore, cleaveRightBefore, cleaveLeftAfter,
							cleaveRightAfter, missedCleaves, maxMissedCleaves, minPepLen, maxPepLen, sequence, pos, aa,
							isNterm, isCterm, addPeptide, new[]{codon + "->" + aa});
					}
				}
			}
			{
				string codon = codons[len - 1];
				if (codon != null){
					char[] aas = AminoAcids.CodonMutatesToAa[codon];
					bool cleaveLeftBefore = Cleaves(sequence[len - 2], sequence[len - 1], enzymes);
					foreach (char aa in aas){
						if (aa == '*'){
							ProcessStopCodonInsertion(sequence.Substring(0, len - 1), isNterm, addPeptide, minPepLen,
								new[]{codon + "->" + aa}, null, sequence);
						} else{
							bool cleaveLeftAfter = Cleaves(sequence[len - 2], aa, enzymes);
							bool cleaveRightAfter = Cleaves(aa, aaAfter, enzymes);
							if (!cleaveRightAfter){
								continue;
							}
							ProcessSingleAaSubstitution(cleaveLeftBefore, true, cleaveLeftAfter, true, missedCleaves,
								maxMissedCleaves, minPepLen, maxPepLen, sequence, len - 1, aa, isNterm, isCterm,
								addPeptide, new[]{codon + "->" + aa});
						}
					}
				}
			}
		}

		private static void ProcessStopCodonInsertion(string sequence, bool isNterm,
			Action<string, bool, bool, byte, string[], string> addPeptide, int minPeptideLen, string[] names,
			Regex variationGroupRegex, string baseSeq){
			if (sequence.Length >= minPeptideLen){
				addPeptide(sequence, isNterm, true, GetMutationLevel(names, variationGroupRegex), names, baseSeq);
			}
		}

		private static void ProcessSingleAaSubstitution(bool cleaveLeftBefore, bool cleaveRightBefore,
			bool cleaveLeftAfter, bool cleaveRightAfter, int missedCleaves, int maxMissedCleaves, int minPepLen,
			int maxPepLen, string sequence, int pos, char aa, bool isNterm, bool isCterm,
			Action<string, bool, bool, byte, string[], string> addPeptide, string[] names){
			bool cleaveIncreases = (!cleaveLeftBefore && cleaveLeftAfter) || (!cleaveRightBefore && cleaveRightAfter);
			bool cleaveDecreases = (cleaveLeftBefore && !cleaveLeftAfter) || (cleaveRightBefore && !cleaveRightAfter);
			if (missedCleaves > maxMissedCleaves && !cleaveDecreases){
				return;
			}
			string newSequence = MutateSingleAaSubstitution(sequence, pos, aa);
			if (cleaveIncreases){
				if (missedCleaves < maxMissedCleaves){
					if (newSequence.Length <= maxPepLen){
						addPeptide(newSequence, isNterm, isCterm, 1, names, sequence);
					}
				}
				string seq1;
				string seq2;
				if (!cleaveLeftBefore && cleaveLeftAfter){
					seq1 = newSequence.Substring(0, pos);
					seq2 = newSequence.Substring(pos, sequence.Length - pos);
				} else{
					seq1 = newSequence.Substring(0, pos + 1);
					seq2 = newSequence.Substring(pos + 1, sequence.Length - pos - 1);
				}
				if (seq1.Length >= minPepLen && seq1.Length <= maxPepLen){
					addPeptide(seq1, isNterm, false, 1, names, sequence);
				}
				if (seq2.Length >= minPepLen && seq2.Length <= maxPepLen){
					addPeptide(seq2, false, isCterm, 1, names, sequence);
				}
			} else{
				if (newSequence.Length <= maxPepLen){
					addPeptide(newSequence, isNterm, isCterm, 1, names, sequence);
				}
			}
		}

		private static string MutateSingleAaSubstitution(string sequence, int pos, char aa){
			char[] x = sequence.ToCharArray();
			x[pos] = aa;
			return new string(x);
		}

		private static string MutateSeveralSingleAaSubstitution(string sequence, int[] pos, char[] aa){
			char[] x = sequence.ToCharArray();
			for (int i = 0; i < pos.Length; i++){
				x[pos[i]] = aa[i];
			}
			return new string(x);
		}

		/// <summary>
		/// Determines if the set of enzymes all applied between the two amimo acids would cleave the protein sequence.
		/// </summary>
		/// <param name="a">Amino acid before the potential cleavage position.</param>
		/// <param name="b">Amino acid after the potential cleavage position.</param>
		/// <param name="enzymes">Enzyme rules applied to the two amino acids.</param>
		/// <returns>True if cleaves.</returns>
		private static bool Cleaves(char a, char b, IEnumerable<Enzyme> enzymes){
			if (a == '*' || b == '*'){
				return true;
			}
			foreach (Enzyme enzyme in enzymes){
				if (enzyme.Cleaves(a, b)){
					return true;
				}
			}
			return false;
		}
	}
}