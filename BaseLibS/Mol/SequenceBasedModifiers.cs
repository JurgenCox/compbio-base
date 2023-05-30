using System.Collections.Generic;
using System.Text;
namespace BaseLibS.Mol{
	public static class SequenceBasedModifiers{
		public static string GetCompositionFromSequence(string modseq){
			/* Finding the composition av a peptide sequence given its amino acid sequence. 
             * Input: Amino acid sequence 
             * Output: Composition of atoms as a string of type : H(x1) C(x2) N(x3) O(x4) S(x5)
             */
			Dictionary<string, int> AComp = new Dictionary<string, int>{
				{"C", 3},
				{"H", 5},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> RComp = new Dictionary<string, int>{
				{"C", 6},
				{"H", 12},
				{"N", 4},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> DComp = new Dictionary<string, int>{
				{"C", 4},
				{"H", 5},
				{"N", 1},
				{"O", 3},
				{"S", 0}
			};
			Dictionary<string, int> EComp = new Dictionary<string, int>{
				{"C", 5},
				{"H", 7},
				{"N", 1},
				{"O", 3},
				{"S", 0}
			};
			Dictionary<string, int> FComp = new Dictionary<string, int>{
				{"C", 9},
				{"H", 9},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> CComp = new Dictionary<string, int>{
				{"C", 3},
				{"H", 5},
				{"N", 1},
				{"O", 1},
				{"S", 1}
			};
			Dictionary<string, int> GComp = new Dictionary<string, int>{
				{"C", 2},
				{"H", 3},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> HComp = new Dictionary<string, int>{
				{"C", 6},
				{"H", 7},
				{"N", 3},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> IComp = new Dictionary<string, int>{
				{"C", 6},
				{"H", 11},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> KComp = new Dictionary<string, int>{
				{"C", 6},
				{"H", 12},
				{"N", 2},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> LComp = new Dictionary<string, int>{
				{"C", 6},
				{"H", 11},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> MComp = new Dictionary<string, int>{
				{"C", 5},
				{"H", 9},
				{"N", 1},
				{"O", 1},
				{"S", 1}
			};
			Dictionary<string, int> NComp = new Dictionary<string, int>{
				{"C", 4},
				{"H", 6},
				{"N", 2},
				{"O", 2},
				{"S", 0}
			};
			Dictionary<string, int> QComp = new Dictionary<string, int>{
				{"C", 5},
				{"H", 8},
				{"N", 2},
				{"O", 2},
				{"S", 0}
			};
			Dictionary<string, int> PComp = new Dictionary<string, int>{
				{"C", 5},
				{"H", 7},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> SComp = new Dictionary<string, int>{
				{"C", 3},
				{"H", 5},
				{"N", 1},
				{"O", 2},
				{"S", 0}
			};
			Dictionary<string, int> TComp = new Dictionary<string, int>{
				{"C", 4},
				{"H", 7},
				{"N", 1},
				{"O", 2},
				{"S", 0}
			};
			Dictionary<string, int> WComp = new Dictionary<string, int>{
				{"C", 11},
				{"H", 10},
				{"N", 2},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, int> YComp = new Dictionary<string, int>{
				{"C", 9},
				{"H", 9},
				{"N", 1},
				{"O", 2},
				{"S", 0}
			};
			Dictionary<string, int> VComp = new Dictionary<string, int>{
				{"C", 5},
				{"H", 9},
				{"N", 1},
				{"O", 1},
				{"S", 0}
			};
			Dictionary<string, Dictionary<string, int>> aaDict = new Dictionary<string, Dictionary<string, int>>{
				{"A", AComp},
				{"R", RComp},
				{"N", NComp},
				{"D", DComp},
				{"C", CComp},
				{"E", EComp},
				{"Q", QComp},
				{"G", GComp},
				{"H", HComp},
				{"I", IComp},
				{"L", LComp},
				{"K", KComp},
				{"M", MComp},
				{"F", FComp},
				{"P", PComp},
				{"S", SComp},
				{"T", TComp},
				{"W", WComp},
				{"Y", YComp},
				{"V", VComp}
			};
			string composition;
			int carbons = 0;
			int hydrogens = 0;
			int nitrogens = 0;
			int oxygens = 0;
			int sulfurs = 0;
			for (int i = 0; i < modseq.Length; i++){
				carbons += aaDict[modseq[i].ToString()]["C"];
				hydrogens += aaDict[modseq[i].ToString()]["H"];
				nitrogens += aaDict[modseq[i].ToString()]["N"];
				oxygens += aaDict[modseq[i].ToString()]["O"];
				sulfurs += aaDict[modseq[i].ToString()]["S"];
			}
			composition = "H(" + hydrogens + ") C(" + carbons + ") N(" + nitrogens + ") O(" + oxygens + ")";
			if (sulfurs != 0){
				composition += " S(" + sulfurs + ")";
			}
			return composition;
		}
		public static List<string> GetDiagnosticPeaksSequenceBasedModifier(string modseq){
			/* Finding the diagnostic peaks of an conjugated protein modification after one fragmentation 
             * Only considers one fragmentation.
             * Input: Sequence Based modifer sequence 
             * Output: List of diagnostic peaks 
            */
			StringBuilder builder = new StringBuilder();
			List<string> diagPeaks = new List<string>();
			foreach (char c in modseq){
				builder.Append(c);
				diagPeaks.Add(c.ToString());
			}
			return diagPeaks;
		}
	}
}