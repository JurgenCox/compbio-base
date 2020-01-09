using System;
using BaseLibS.Graph;

namespace BaseLibS.Num.Space{
	public class Vector2F{
		public float X { get; set; }
		public float Y { get; set; }

		public Vector2F(float x, float y){
			X = x;
			Y = y;
		}

		public static float Distance(Vector2F v1, Vector2F v2){
			float d2 = (v1.X - v2.X)*(v1.X - v2.X) + (v1.Y - v2.Y)*(v1.Y - v2.Y);
			return (float) Math.Sqrt(d2);
		}

		public static Vector2F Center(Rectangle2 rectangle){
			return new Vector2F(rectangle.Left + rectangle.Width/2, rectangle.Top + rectangle.Height/2);
		}
	}
}