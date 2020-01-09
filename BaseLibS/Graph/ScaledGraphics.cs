namespace BaseLibS.Graph{
	public class ScaledGraphics : IGraphics{
		private readonly IGraphics g;
		private readonly float s;

		public ScaledGraphics(IGraphics g, float s){
			this.g = g;
			this.s = s;
		}

		public SmoothingMode2 SmoothingMode{
			get => g.SmoothingMode;
			set => g.SmoothingMode = value;
		}

		public void Dispose(){
			g.Dispose();
		}

		public void RotateTransform(float angle){
			g.RotateTransform(angle);
		}

		public void ScaleTransform(float sx, float sy){
			g.ScaleTransform(s*sx, s*sy);
		}

		public void SetClippingMask(float width, float height, float x, float y){
			g.SetClippingMask(s*width, s*height, s*x, s*y);
		}

		public void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2){
			g.DrawLine(pen.Scale(s), s*x1, s*y1, s*x2, s*y2);
		}

		public void DrawInterceptedLine(Pen2 pen, float x1, float y1, float x2, float y2, float len){
			g.DrawInterceptedLine(pen.Scale(s), s*x1, s*y1, s*x2, s*y2, s*len);
		}

		public void DrawArrow(Pen2 pen, float x1, float y1, float x2, float y2, float side){
			g.DrawArrow(pen.Scale(s), s*x1, s*y1, s*x2, s*y2, s*side);
		}

		public void DrawPath(Pen2 pen, GraphicsPath2 path){
			g.DrawPath(pen.Scale(s), path.Scale(s));
		}

		public void DrawLines(Pen2 pen, Point2[] points){
			g.DrawLines(pen.Scale(s), Scale(points, s));
		}

		public void DrawEllipse(Pen2 pen, float x, float y, float width, float height){
			g.DrawEllipse(pen.Scale(s), s*x, s*y, s*width, s*height);
		}

		public void FillEllipse(Brush2 brush, float x, float y, float width, float height){
			g.FillEllipse(brush, s*x, s*y, s*width, s*height);
		}

		public void DrawRectangle(Pen2 pen, float x, float y, float width, float height){
			g.DrawRectangle(pen.Scale(s), s*x, s*y, s*width, s*height);
		}

		public void FillRectangle(Brush2 brush, float x, float y, float width, float height){
			g.FillRectangle(brush, s*x, s*y, s*width, s*height);
		}

		public void DrawRoundedRectangle(Pen2 pen, float x, float y, float width, float height, float radius){
			g.DrawRoundedRectangle(pen.Scale(s), s*x, s*y, s*width, s*height, s*radius);
		}

		public void FillRoundedRactangle(Brush2 brush, float x, float y, float width, float height, float radius){
			g.FillRoundedRactangle(brush, s*x, s*y, s*width, s*height, s*radius);
		}

		public void DrawArc(Pen2 pen, Rectangle2 rectangle, float startAngle, float sweepAngle){
			g.DrawArc(pen.Scale(s), rectangle.Scale(s), startAngle, sweepAngle);
		}

		public void DrawPolygon(Pen2 pen, Point2[] points){
			g.DrawPolygon(pen.Scale(s), Scale(points, s));
		}

		public void FillPolygon(Brush2 brush, Point2[] points){
			g.FillPolygon(brush, Scale(points, s));
		}

		public Size2 MeasureString(string text, Font2 font){
			return g.MeasureString(text, font);
		}

		public void DrawString(string str, Font2 font, Brush2 brush, float x, float y){
			g.DrawString(str, font.Scale(s), brush, s*x, s*y);
		}

		public void DrawString(string str, Font2 font, Brush2 brush, Rectangle2 rectangle, StringFormat2 format){
			g.DrawString(str, font.Scale(s), brush, rectangle.Scale(s), format);
		}

		public void DrawString(string str, Font2 font, Brush2 brush, Point2 point, StringFormat2 format){
			g.DrawString(str, font.Scale(s), brush, point.Scale(s), format);
		}

		public void DrawString(string str, Font2 font, Brush2 brush, Point2 location){
			g.DrawString(str, font.Scale(s), brush, location.Scale(s));
		}

		public void DrawString(string str, Font2 font, Brush2 brush, Rectangle2 rectangle){
			g.DrawString(str, font.Scale(s), brush, rectangle.Scale(s));
		}

		public void DrawImage(Bitmap2 image, float x, float y, float width, float height){
			g.DrawImage(image, s*x, s*y, s*width, s*height);
		}

		public void DrawImageUnscaled(Bitmap2 image, float x, float y){
			g.DrawImageUnscaled(image, s*x, s*y);
		}

		public Size2 MeasureString(string text, Font2 font, float width){
			return g.MeasureString(text, font, width);
		}

		public void FillClosedCurve(Brush2 brush, Point2[] points){
			g.FillClosedCurve(brush, Scale(points, s));
		}

		public void DrawCurve(Pen2 pen, Point2[] points){
			g.DrawCurve(pen.Scale(s), Scale(points, s));
		}

		public void TranslateTransform(float dx, float dy){
			g.TranslateTransform(s*dx, s*dy);
		}

		public void ResetTransform(){
			g.ResetTransform();
		}

		public void ResetClip(){
			g.ResetClip();
		}

		public void SetClip(Rectangle2 rectangle){
			g.SetClip(rectangle.Scale(s));
		}

		public void Close(){
			g.Close();
		}

		public float GetDpiScale(){
			return g.GetDpiScale();
		}

		private static Point2[] Scale(Point2[] points, float s){
			Point2[] result = new Point2[points.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = points[i].Scale(s);
			}
			return result;
		}
	}
}