using System;
using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public class SymbolProperties : IComparable<SymbolProperties>{
		public Color2 Color { get; set; }
		public int Size { get; set; }
		public int Type { get; set; }
		public bool ShowLabel { get; set; }

		public SymbolProperties(Color2 color, int size, int type, bool showLabel){
			Color = color;
			Size = size;
			Type = type;
			ShowLabel = showLabel;
		}

		public int CompareTo(SymbolProperties other){
			if (Color.R != other.Color.R){
				return Color.R.CompareTo(other.Color.R);
			}
			if (Color.G != other.Color.G){
				return Color.G.CompareTo(other.Color.G);
			}
			if (Color.B != other.Color.B){
				return Color.B.CompareTo(other.Color.B);
			}
			if (Type != other.Type){
				return Type.CompareTo(other.Type);
			}
			return Size != other.Size ? Size.CompareTo(other.Size) : 0;
		}

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == typeof (SymbolProperties) && Equals((SymbolProperties) obj);
		}

		public bool Equals(SymbolProperties other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return other.Color.Equals(Color) && other.Size == Size && other.Type == Type;
		}

		public override int GetHashCode(){
			unchecked{
				int result = Color.GetHashCode();
				result = (result*397) ^ Size;
				result = (result * 397) ^ Type;
				return result;
			}
		}
	}
}