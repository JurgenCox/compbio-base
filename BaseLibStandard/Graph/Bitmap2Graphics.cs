namespace BaseLibS.Graph{
	public class Bitmap2Graphics : IGraphics{
		public Bitmap2 Bitmap { get; }

		public Bitmap2Graphics(int width, int height){
			Bitmap = new Bitmap2(width, height);
		}

		public SmoothingMode2 SmoothingMode { get; set; }

		public void Dispose(){
			throw new System.NotImplementedException();
		}

		public void RotateTransform(float angle){
			throw new System.NotImplementedException();
		}

	    public void ScaleTransform(float sx, float sy)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void SetClippingMask(float width, float height, float x, float y){
			throw new System.NotImplementedException();
		}

		public void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2){
			throw new System.NotImplementedException();
		}

		public void DrawInterceptedLine(Pen2 pen, float x1, float y1, float x2, float y2, float len){
			throw new System.NotImplementedException();
		}

		public void DrawArrow(Pen2 pen, float x1, float y1, float x2, float y2, float side){
			throw new System.NotImplementedException();
		}

		public void DrawPath(Pen2 pen, GraphicsPath2 path){
			throw new System.NotImplementedException();
		}

		public void DrawLines(Pen2 pen, Point2[] points){
			throw new System.NotImplementedException();
		}

		public void DrawEllipse(Pen2 pen, float x, float y, float width, float height){
			throw new System.NotImplementedException();
		}

		public void FillEllipse(Brush2 brush, float x, float y, float width, float height){
			throw new System.NotImplementedException();
		}

		public void DrawRectangle(Pen2 pen, float x, float y, float width, float height){
			throw new System.NotImplementedException();
		}

		public void FillRectangle(Brush2 brush, float x, float y, float width, float height){
			throw new System.NotImplementedException();
		}

		public void DrawRoundedRectangle(Pen2 pen, float x, float y, float width, float height, float radius){
			throw new System.NotImplementedException();
		}

		public void FillRoundedRactangle(Brush2 brush, float x, float y, float width, float height, float radius){
			throw new System.NotImplementedException();
		}

		public void DrawArc(Pen2 pen, Rectangle2 rec, float startAngle, float sweepAngle){
			throw new System.NotImplementedException();
		}

		public void DrawPolygon(Pen2 pen, Point2[] points){
			throw new System.NotImplementedException();
		}

		public void FillPolygon(Brush2 brush, Point2[] points){
			throw new System.NotImplementedException();
		}

		public Size2 MeasureString(string text, Font2 font){
			throw new System.NotImplementedException();
		}

		public void DrawString(string s, Font2 font, Brush2 brush, float x, float y){
			throw new System.NotImplementedException();
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF, StringFormat2 format){
			throw new System.NotImplementedException();
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 point, StringFormat2 format){
			throw new System.NotImplementedException();
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 location){
			throw new System.NotImplementedException();
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF){
			throw new System.NotImplementedException();
		}

		public void DrawImage(Bitmap2 image, float x, float y, float width, float height){
			throw new System.NotImplementedException();
		}

		public void DrawImageUnscaled(Bitmap2 image, float x, float y){
			throw new System.NotImplementedException();
		}

		public Size2 MeasureString(string text, Font2 font, float width){
			throw new System.NotImplementedException();
		}

		public void FillClosedCurve(Brush2 brush, Point2[] points){
			throw new System.NotImplementedException();
		}

		public void DrawCurve(Pen2 pen, Point2[] points){
			throw new System.NotImplementedException();
		}

		public void TranslateTransform(float dx, float dy){
			throw new System.NotImplementedException();
		}

		public void ResetTransform(){
			throw new System.NotImplementedException();
		}

		public void ResetClip(){
			throw new System.NotImplementedException();
		}

		public void SetClip(Rectangle2 rectangle){
			throw new System.NotImplementedException();
		}

		public void Close(){
			throw new System.NotImplementedException();
		}

		public float GetDpiScale(){
			throw new System.NotImplementedException();
		}
	}
}