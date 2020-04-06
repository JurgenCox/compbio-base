using BaseLibS.Util;

namespace BaseLibS.Ms.Decoy{
	public class DecoyStrategyReward : DecoyStrategy{
		public DecoyStrategyReward(string specialAas) : base(specialAas){ }

		public override string ProcessProtein(string protSeq, bool isCodon){
			return protSeq;
			//char[] rev = new char[protSeq.Length];
			//for (int i = 0; i < protSeq.Length; i++){
			//	rev[i] = protSeq[protSeq.Length - i - 1];
			//}
			//for (int i = 1; i < protSeq.Length; i++){
			//	if (keep.Contains(rev[i])){
			//		char c = rev[i - 1];
			//		rev[i - 1] = rev[i];
			//		rev[i] = c;
			//	}
			//}
			//return new string(rev);
		}

		public override string ProcessVariation(string mutaions, string protSeq, bool isCodon){
			return mutaions;
		}

		public override string ProcessPeptide(string pepSeq){
			bool firstHalf = (pepSeq.GetHashCode() / 2) % 2 == 1;
			//const bool firstHalf = true;
			char[] result = pepSeq.ToCharArray();
			int n2 = (pepSeq.Length - 1) / 2;
			bool odd = n2 % 2 == 1;
			return firstHalf
				? ProcessPeptideFirstHalf(pepSeq, result, n2)
				: ProcessPeptideSecondHalf(pepSeq, result, n2, odd);
		}

		private static string ProcessPeptideSecondHalf(string pepSeq, char[] result, int n2, bool odd){
			for (int i = 1; i <= n2 - (odd ? 1 : 0); i++){
				result[n2 + i] = pepSeq[pepSeq.Length - 1 - i];
			}
			return new string(result);
		}

		private static string ProcessPeptideFirstHalf(string pepSeq, char[] result, int n2){
			for (int i = 1; i <= n2; i++){
				result[i] = pepSeq[n2 + 1 - i];
			}
			return new string(result);
		}

		public override int GetHashCode(){
			unchecked{
				return ((specialAas != null ? HashCode.GetDeterministicHashCode(specialAas) : 3) * 393);
			}
		}

		public override DecoyMode DecoyMode{
			get{ return DecoyMode.Reward; }
		}
	}
}