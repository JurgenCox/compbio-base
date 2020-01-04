using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BaseLibS.Graph.Network{
	[Serializable]
	public abstract class NetworkGraphNode : IXmlSerializable{
		public float X { get; set; }
		public float Y { get; set; }
		public abstract int Width { get; }
		public abstract int Height { get; }
		public virtual bool Hits(int x1, int y1){
			return x1 >= X && x1 <= X + Width && y1 >= Y && y1 <= Y + Height;
		}
		public bool Intersects(float x1, float y1, int width1, int height1){
			return x1 < X + Width && x1 + width1 >= X && y1 < Y + Height && y1 + height1 >= Y;
		}
		public abstract XmlSchema GetSchema();
		public abstract void ReadXml(XmlReader reader);
		public abstract void WriteXml(XmlWriter writer);
		public abstract float GetInputPosX(int index);
		public abstract float GetInputPosY(int index);
		public abstract float GetOutputPosX(int index);
		public abstract float GetOutputPosY(int index);
		public abstract Brush2 GetInputBrush(int index);
		public abstract Brush2 GetOutputBrush(int index);
	}
	[Serializable]
	public abstract class NetworkGraphNode<Te, Ti> : NetworkGraphNode where Te : NetworkGraphEdge where Ti : IDrawOptions{
		public List<Te> inputEdges = new List<Te>();
		public List<List<Te>> outputEdges = new List<List<Te>>();
		public abstract void Paint(IGraphics g, float x1, float y1, Ti options);
		public abstract void PaintSelected(IGraphics g, float x1, float y1);
		public abstract int InputCount { get; }
		public abstract int OutputCount { get; }
		public bool Intersects(NetworkGraphNode<Te, Ti> node){
			return Intersects(node.X, node.Y, node.Width, node.Height);
		}
	}
}