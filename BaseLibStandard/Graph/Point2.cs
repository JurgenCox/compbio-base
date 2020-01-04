using System.Globalization;

namespace BaseLibS.Graph{
	public struct Point2{
		public static readonly Point2 Empty = new Point2();
		public float X { get; set; }
		public float Y { get; set; }

		public Point2(float x, float y){
			X = x;
			Y = y;
		}

		public bool IsEmpty => X == 0f && Y == 0f;

		public static Point2 operator +(Point2 pt, Size2 sz){
			return Add(pt, sz);
		}

		public static Point2 operator -(Point2 pt, Size2 sz){
			return Subtract(pt, sz);
		}

		public static bool operator ==(Point2 left, Point2 right){
			return left.X == right.X && left.Y == right.Y;
		}

		public static bool operator !=(Point2 left, Point2 right){
			return !(left == right);
		}

		public static Point2 Add(Point2 pt, Size2 sz){
			return new Point2(pt.X + sz.Width, pt.Y + sz.Height);
		}

		public static Point2 Subtract(Point2 pt, Size2 sz){
			return new Point2(pt.X - sz.Width, pt.Y - sz.Height);
		}

		public Point2 Scale(float s){
			return new Point2(X*s, Y*s);
		}

		public override bool Equals(object obj){
			if (!(obj is Point2)){
				return false;
			}
			Point2 comp = (Point2) obj;
			return comp.X == this.X && comp.Y == this.Y && comp.GetType().Equals(this.GetType());
		}

		public override int GetHashCode(){
			return base.GetHashCode();
		}

		/// <include file="doc\PointF.uex" path='docs/doc[@for="PointF.ToString"]/*'>
		public override string ToString(){
			return string.Format(CultureInfo.CurrentCulture, "{{X={0}, Y={1}}}", X, Y);
		}
	}
}