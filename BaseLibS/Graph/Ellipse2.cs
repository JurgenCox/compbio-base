namespace BaseLibS.Graph{
	public class Ellipse2{
		private Point2 center;

		public Ellipse2(Point2 center, float radiusX, float radiusY){
			this.center = center;
			RadiusX = radiusX;
			RadiusY = radiusY;
		}

		public float RadiusX { get; }
		public float RadiusY { get; }

		public bool Contains(int x, int y){
			if (RadiusX <= 0 || RadiusY <= 0){
				return false;
			}
			Point2 normalized = new Point2(x - center.X, y - center.Y);
			float nX = normalized.X;
			float nY = normalized.Y;
			return (double) (nX*nX)/(RadiusX*RadiusX) + (double) (nY*nY)/(RadiusY*RadiusY) <= 1.0;
		}
	}
}