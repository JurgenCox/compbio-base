using System;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Mol{
	public class EnzymeCombination : IComparable<EnzymeCombination>{
		private readonly EnzymeMode mode;
		private readonly string[] enzymes;

		public EnzymeCombination(EnzymeMode mode, string[] enzymes){
			this.mode = mode;
			if (mode == EnzymeMode.None || mode == EnzymeMode.Unspecific){
				enzymes = new string[0];
			} else{
				this.enzymes = enzymes;
				Array.Sort(this.enzymes);
			}
		}

		public int CompareTo(EnzymeCombination other){
			if (mode != other.mode){
				return mode.CompareTo(other.mode);
			}
			return String.Compare(StringUtils.Concat("%", enzymes), StringUtils.Concat("%", other.enzymes),
				StringComparison.InvariantCulture);
		}

		public override bool Equals(object obj){
			return obj is EnzymeCombination && Equals((EnzymeCombination) obj);
		}

		protected bool Equals(EnzymeCombination other){
			return mode == other.mode && ArrayUtils.EqualArrays(enzymes, other.enzymes);
		}

		public override int GetHashCode(){
			unchecked{
				return ((int) mode * 397) ^ (enzymes != null ? ArrayUtils.GetArrayHashCode(enzymes) : 0);
			}
		}
	}
}