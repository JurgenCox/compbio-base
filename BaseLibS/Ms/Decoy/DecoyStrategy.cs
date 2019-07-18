using System.Collections.Generic;

namespace BaseLibS.Ms.Decoy{
	public abstract class DecoyStrategy{
		protected string specialAas;
		protected readonly ICollection<char> keep;

		protected DecoyStrategy(string specialAas){
			this.specialAas = specialAas;
			keep = new HashSet<char>(specialAas.ToCharArray());
		}

		public string SpecialAas => specialAas;
		public abstract string ProcessProtein(string protSeq, bool isCodon);
		public abstract string ProcessVariation(string mutaions, string protSeq, bool isCodon);
		public abstract string ProcessPeptide(string pepSeq);
		public abstract DecoyMode DecoyMode{ get; }

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == GetType() && Equals((DecoyStrategy) obj);
		}

		protected bool Equals(DecoyStrategy other){
			return string.Equals(specialAas, other.specialAas);
		}
	}
}