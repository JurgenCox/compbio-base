using System.Collections.Generic;
using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public class SymbolTypeDiamond : SymbolType{
		public SymbolTypeDiamond(int index) : base(index) {}
		public override string Name => "Diamond";

		public override void GetPath(int size, out int[] pathX, out int[] pathY){
			int s2 = size/2;
			List<int> x = new List<int>();
			List<int> y = new List<int>();
			for (int i = 0; i < s2; i++){
				x.Add(i);
				y.Add(s2 - i);
			}
			for (int i = 0; i < s2; i++){
				x.Add(s2 - i);
				y.Add(-i);
			}
			for (int i = 0; i < s2; i++){
				x.Add(-i);
				y.Add(-s2 + i);
			}
			for (int i = 0; i < s2; i++){
				x.Add(-s2 + i);
				y.Add(i);
			}
			pathX = x.ToArray();
			pathY = y.ToArray();
		}

		public override void Draw(int size, float x, float y, IGraphics g, Pen2 pen, Brush2 brush){
			int s2 = size/2;
			Point2[] points = {new Point2(x - s2, y), new Point2(x, y - s2), new Point2(x + s2, y), new Point2(x, y + s2)};
			g.DrawPolygon(pen, points);
		}
	}
}