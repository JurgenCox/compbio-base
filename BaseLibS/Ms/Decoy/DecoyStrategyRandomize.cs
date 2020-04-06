using System.Collections.Generic;
using System.Text;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Ms.Decoy{
	public class DecoyStrategyRandomize : DecoyStrategy{
		private readonly Random2 rand;
		private readonly int seed;

		public DecoyStrategyRandomize(string specialAas, int seed) : base(specialAas){
			this.seed = seed;
			rand = new Random2(seed);
		}

		public override string ProcessProtein(string protSeq, bool isCodon){
			char[] rev = new char[protSeq.Length];
			List<int> inds = new List<int>();
			StringBuilder w = new StringBuilder();
			for (int i = 0; i < protSeq.Length; i++){
				char c = protSeq[i];
				if (i == 0 || keep.Contains(c)){
					rev[i] = c;
				} else{
					inds.Add(i);
					w.Append(c);
				}
			}
			int[] p = rand.NextPermutation(inds.Count);
			for (int i = 0; i < p.Length; i++){
				rev[inds[i]] = w[p[i]];
			}
			return new string(rev);
		}

		public override string ProcessVariation(string mutaions, string protSeq, bool isCodon){
			return mutaions;
		}

		public override string ProcessPeptide(string pepSeq){
			return pepSeq;
		}

		public override int GetHashCode(){
			unchecked{
				return ((specialAas != null ? HashCode.GetDeterministicHashCode(specialAas) : 3) * 397 + seed);
			}
		}

		public override DecoyMode DecoyMode => DecoyMode.Randomize;
	}
}