using System;
using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public class LineProperties : IComparable<LineProperties>{
		public Color2 Color { get; set; }
		public int Width { get; set; }
		public DashStyle2 DashStyle { get; set; }

		public LineProperties(Color2 color, int size, DashStyle2 dashStyle) {
			Color = color;
			Width = size;
			DashStyle = dashStyle;
		}

		public int CompareTo(LineProperties other){
			if (Color.R != other.Color.R){
				return Color.R.CompareTo(other.Color.R);
			}
			if (Color.G != other.Color.G){
				return Color.G.CompareTo(other.Color.G);
			}
			if (Color.B != other.Color.B){
				return Color.B.CompareTo(other.Color.B);
			}
			if (Width != other.Width) {
				return Width.CompareTo(other.Width);
			}
			return DashStyle != other.DashStyle ? DashStyles.DashStyleToIndex(DashStyle).CompareTo(DashStyles.DashStyleToIndex(other.DashStyle)) : 0;
		}

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == typeof (LineProperties) && Equals((LineProperties) obj);
		}

		public bool Equals(LineProperties other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return other.Color.Equals(Color) && other.Width == Width && other.DashStyle.Equals(DashStyle);
		}

		public override int GetHashCode(){
			unchecked{
				int result = Color.GetHashCode();
				result = (result * 397) ^ Width;
				result = (result * 397) ^ DashStyle.GetHashCode();
				return result;
			}
		}
	}
}