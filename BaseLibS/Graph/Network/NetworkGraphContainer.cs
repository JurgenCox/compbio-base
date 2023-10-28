using System;
using System.IO;
using BaseLibS.Drawing;
namespace BaseLibS.Graph.Network{
	[Serializable]
	public abstract class NetworkGraphContainer {
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		protected NetworkGraphContainer(BinaryReader reader) {
			X = reader.ReadInt32();
			Y = reader.ReadInt32();
			Z = reader.ReadInt32();
			Width = reader.ReadInt32();
			Height = reader.ReadInt32();
		}
		public virtual void Write(BinaryWriter writer) {
			writer.Write(X);
			writer.Write(Y);
			writer.Write(Z);
			writer.Write(Width);
			writer.Write(Height);
		}
		public abstract void Paint(IGraphics g, int x1, int y1);
		public abstract void PaintSelected(IGraphics g, int x1, int y1);
		public virtual bool Hits(int x1, int y1){
			return x1 >= X && x1 <= X + Width && y1 >= Y && y1 <= Y + Height;
		}
		public bool Intersects(int x1, int y1, int width1, int height1){
			return x1 < X + Width && x1 + width1 >= X && y1 < Y + Height && y1 + height1 >= Y;
		}
	}
}