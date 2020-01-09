using System;
using System.Collections.Generic;
using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public class SymbolTypeCircle : SymbolType{
		public SymbolTypeCircle(int index) : base(index) {}
		public override string Name => "Circle";

		public override void GetPath(int size, out int[] pathX, out int[] pathY){
			int s2 = size/2;
			List<int> x = new List<int>();
			List<int> y = new List<int>();
			for (int i = -s2 + 1; i < s2; i++){
				x.Add(i);
				int j = (int) Math.Round(Math.Sqrt(s2*s2 - i*i));
				y.Add(j);
				x.Add(i);
				y.Add(-j);
				x.Add(j);
				y.Add(i);
				x.Add(-j);
				y.Add(i);
			}
			pathX = x.ToArray();
			pathY = y.ToArray();
		}

		public override void Draw(int size, float x, float y, IGraphics g, Pen2 pen, Brush2 brush){
			int s2 = size/2;
			g.DrawEllipse(pen, x - s2, y - s2, size, size);
		}
	}
}