using System.Collections.Generic;

namespace BaseLibS.Graph{
	public class GraphicsPath2{
		private readonly List<Point2> pathPoints = new List<Point2>();
		private readonly List<byte> pathTypes = new List<byte>();
		public Point2[] PathPoints => pathPoints.ToArray();
		public byte[] PathTypes => pathTypes.ToArray();
		public int PointCount => pathPoints.Count;

		public void AddLine(Point2 p1, Point2 p2){
			AddLine(p1.X, p1.Y, p2.X, p2.Y);
		}

		public void AddLine(float x1, float y1, float x2, float y2){
			pathPoints.Add(new Point2(x1, y1));
			pathPoints.Add(new Point2(x2, y2));
			pathTypes.Add(0);
			pathTypes.Add(1);
		}

		public void Reset(){
			pathPoints.Clear();
			pathTypes.Clear();
		}

		public GraphicsPath2 Scale(float s){
			GraphicsPath2 result = new GraphicsPath2();
			for (int i = 0; i < pathPoints.Count; i++){
				result.pathPoints.Add(new Point2(s*pathPoints[i].X, s*pathPoints[i].Y));
				result.pathTypes.Add(pathTypes[i]);
			}
			return result;
		}
	}
}