using System.Collections.Generic;
using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public class SymbolTypeSquare : SymbolType{
		public SymbolTypeSquare(int index) : base(index) {}
		public override string Name => "Square";

		public override void GetPath(int size, out int[] pathX, out int[] pathY){
			int s2 = size/2;
			List<int> x = new List<int>();
			List<int> y = new List<int>();
			for (int i = -s2; i <= s2; i++){
				x.Add(i);
				y.Add(s2);
			}
			for (int i = s2 - 1; i >= -s2; i--){
				x.Add(s2);
				y.Add(i);
			}
			for (int i = s2 - 1; i >= -s2; i--){
				x.Add(i);
				y.Add(-s2);
			}
			for (int i = -s2 + 1; i <= s2 - 1; i++){
				x.Add(-s2);
				y.Add(i);
			}
			pathX = x.ToArray();
			pathY = y.ToArray();
		}

		public override void Draw(int size, float x, float y, IGraphics g, Pen2 pen, Brush2 brush){
			int s2 = size/2;
			g.DrawRectangle(pen, x - s2, y - s2, size, size);
		}
	}
}