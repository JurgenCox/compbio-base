using System;

namespace BaseLibS.Symbol{
	public class SymbolLineProperties : IComparable<SymbolLineProperties>{
		public SymbolProperties SymbolProperties { get; set; }
		public LineProperties LineProperties { get; set; }

		public SymbolLineProperties(SymbolProperties symbolProperties, LineProperties lineProperties){
			SymbolProperties = symbolProperties;
			LineProperties = lineProperties;
		}

		public int CompareTo(SymbolLineProperties other){
			return !SymbolProperties.Equals(other.SymbolProperties)
				? SymbolProperties.CompareTo(other.SymbolProperties)
				: LineProperties.CompareTo(other.LineProperties);
		}

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == typeof (SymbolLineProperties) && Equals((SymbolLineProperties) obj);
		}

		public bool Equals(SymbolLineProperties other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return other.SymbolProperties != null && other.LineProperties != null &&
					other.SymbolProperties.Equals(SymbolProperties) && other.LineProperties.Equals(LineProperties);
		}

		public override int GetHashCode(){
			unchecked{
				return ((SymbolProperties?.GetHashCode() ?? 0)*397) ^ (LineProperties?.GetHashCode() ?? 0);
			}
		}
	}
}