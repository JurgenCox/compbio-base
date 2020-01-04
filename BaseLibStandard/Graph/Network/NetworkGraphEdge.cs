using System;

namespace BaseLibS.Graph.Network{
	[Serializable]
	public abstract class NetworkGraphEdge {
		public int FromOutputIndex { get; set; }
		public int ToInputIndex { get; set; }
		public abstract Pen2 Pen { get; }
		public virtual bool Hits(int x1, int y1){
			//TODO
			return false;
		}
	}
	[Serializable]
	public abstract class NetworkGraphEdge<Tn> : NetworkGraphEdge where Tn : NetworkGraphNode{
		public Tn FromNode { get; set; }
		public Tn ToNode { get; set; }
		public virtual void PaintEdge(IGraphics g, float x1, float y1, float x2, float y2){
			g.DrawLine(Pen, x1, y1, x2, y2);
		}
	}
}