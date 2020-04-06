using BaseLibS.Util;

namespace BaseLibS.Ms.Decoy{
	public class DecoyStrategyRevert : DecoyStrategy{
		public DecoyStrategyRevert(string specialAas) : base(specialAas){ }

		public override string ProcessProtein(string protSeq, bool isCodon){
			if (isCodon){
				char[] rev1 = new char[protSeq.Length];
				for (int i = 0; i < protSeq.Length - 2; i += 3){
					rev1[i] = protSeq[protSeq.Length - i - 1 - 2];
					rev1[i + 1] = protSeq[protSeq.Length - i - 1 - 1];
					rev1[i + 2] = protSeq[protSeq.Length - i - 1];
				}
				return new string(rev1);
			}
			char[] rev = new char[protSeq.Length];
			for (int i = 0; i < protSeq.Length; i++){
				rev[i] = protSeq[protSeq.Length - i - 1];
			}
			for (int i = 1; i < protSeq.Length; i++){
				if (keep.Contains(rev[i])){
					char c = rev[i - 1];
					rev[i - 1] = rev[i];
					rev[i] = c;
				}
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
				return ((specialAas != null ? HashCode.GetDeterministicHashCode(specialAas) : 3) * 395);
			}
		}

		public override DecoyMode DecoyMode => DecoyMode.Revert;
	}
}