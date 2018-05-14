using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using BaseLib.Forms;
using BaseLibS.Graph;

namespace BaseLib.Graphic {
	//TODO: should not be exposed
	public abstract class WindowsBasedGraphics : IGraphics {
		protected Graphics gc;

		protected WindowsBasedGraphics(Graphics gc) {
			this.gc = gc;
		}

		public SmoothingMode2 SmoothingMode {
			get => GraphUtils.ToSmoothingMode2(gc.SmoothingMode);
			set => gc.SmoothingMode = GraphUtils.ToSmoothingMode(value);
		}

		public Graphics Graphics => gc;

		//TODO?
		public void ScaleTransform(float sx, float sy) {
			gc.ScaleTransform(sx, sy);
		}

		public void SetClippingMask(float width, float height, float x, float y) { }

		public void Dispose() {
			gc.Dispose();
		}

		public void RotateTransform(float angle) {
			gc.RotateTransform(angle);
		}

		public void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2) {
			gc.DrawLine(GetPen(pen), x1, y1, x2, y2);
		}

		public void DrawInterceptedLine(Pen2 pen, float x1, float y1, float x2, float y2, float len) {
			gc.DrawLine(GetPen(pen), x1, y1, x2, y2);
			float x3, x4, y3, y4;
			if (y1.Equals(y2)) {
				x3 = x2;
				x4 = x2;
				y3 = y2 - len;
				y4 = y2 + len;
			} else if (x1.Equals(x2)) {
				y3 = y2;
				y4 = y2;
				x3 = x2 - len;
				x4 = x2 + len;
			} else {
				float m = -1 / ((y2 - y1) / (x2 - x1));
				float m2 = m * m;
				float sq = (float) Math.Sqrt(1 + m2);
				x3 = x2 + len / sq;
				y3 = y2 + m * len / sq;
				x4 = x2 - len / sq;
				y4 = y2 - m * len / sq;
			}
			gc.DrawLine(GetPen(pen), x3, y3, x4, y4);
		}

		private Point ComputeShiftedPoint(float x1, float y1, float x2, float y2, float x, float y, float dist) {
			float x3, y3;
			if (y1.Equals(y2)) {
				x3 = x2 - dist;
				y3 = y2;
			} else if (x1.Equals(x2)) {
				x3 = x2;
				y3 = y2 - dist;
			} else {
				float m = (y2 - y1) / (x2 - x1);
				float m2 = m * m;
				float sq = (float) Math.Sqrt(1 + m2);
				x3 = x2 + dist / sq;
				y3 = y2 + m * dist / sq;
			}
			return new Point(Convert.ToInt32(x3), Convert.ToInt32(y3));
		}

		public void DrawArrow(Pen2 pen, float x1, float y1, float x2, float y2, float side) {
			float offset = (float) Math.Sqrt(3) + side;
			float newX2;
			float newY2;
			float x3, x4, y3, y4;
			if (y1.Equals(y2)) {
				if (x1 < x2) {
					newX2 = x2 - offset;
				} else {
					newX2 = x2 + offset;
				}
				newY2 = y2;
				x3 = newX2;
				x4 = newX2;
				y3 = newY2 - side / 2;
				y4 = newY2 + side / 2;
				gc.DrawLine(GetPen(pen), x1, y1, newX2, newY2);
			} else if (x1.Equals(x2)) {
				if (y1 < y2) {
					newY2 = y2 - offset;
				} else {
					newY2 = y2 + offset;
				}
				newX2 = x2;
				y3 = newY2;
				y4 = newY2;
				x3 = newX2 - side / 2;
				x4 = newX2 + side / 2;
				gc.DrawLine(GetPen(pen), x1, y1, newX2, newY2);
			} else {
				float m = (y2 - y1) / (x2 - x1);
				float m2 = m * m;
				float sq = (float) Math.Sqrt(1 + m2);
				float n = -1 / ((y2 - y1) / (x2 - x1));
				float n2 = n * n;
				float sqn = (float) Math.Sqrt(1 + n2);
				if (x2 > x1) {
					newX2 = x2 - offset / sq;
					newY2 = y2 - m * offset / sq;
				} else {
					newX2 = x2 + offset / sq;
					newY2 = y2 + m * offset / sq;
				}
				x3 = newX2 + (side / 2) / sqn;
				y3 = newY2 + n * (side / 2) / sqn;
				x4 = newX2 - (side / 2) / sqn;
				y4 = newY2 - n * (side / 2) / sqn;
				gc.DrawLine(GetPen(pen), x1, y1, newX2, newY2);
			}
			gc.DrawPolygon(GetPen(pen),
				new[] {
					new Point(Convert.ToInt32(x2), Convert.ToInt32(y2)), new Point(Convert.ToInt32(x3), Convert.ToInt32(y3)),
					new Point(Convert.ToInt32(x4), Convert.ToInt32(y4))
				});
		}

		public void DrawPath(Pen2 pen, GraphicsPath2 path) {
			gc.DrawPath(GetPen(pen), GetGraphicsPath(path));
		}

		public void DrawLines(Pen2 pen, Point2[] points) {
			gc.DrawLines(GetPen(pen), ToPointsF(points));
		}

		public void DrawEllipse(Pen2 pen, float x, float y, float width, float height) {
			gc.DrawEllipse(GetPen(pen), x, y, width, height);
		}

		public void FillEllipse(Brush2 brush, float x, float y, float width, float height) {
			gc.FillEllipse(GetBrush(brush), x, y, width, height);
		}

		public void DrawRectangle(Pen2 pen, float x, float y, float width, float height) {
			gc.DrawRectangle(GetPen(pen), x, y, width, height);
		}

		public void DrawArc(Pen2 pen, Rectangle2 rec, float startAngle, float sweepAngle) {
			gc.DrawArc(GetPen(pen), ToRectangleF(rec), startAngle, sweepAngle);
		}

		public void FillRectangle(Brush2 brush, float x, float y, float width, float height) {
			gc.FillRectangle(GetBrush(brush), x, y, width, height);
		}

		public void DrawRoundedRectangle(Pen2 pen, float x, float y, float width, float height, float radius) {
			float diameter = radius * 2;
			Size size = new Size((int) diameter, (int) diameter);
			RectangleF bounds = new RectangleF(x, y, width, height);
			RectangleF arc = new RectangleF(bounds.Location, size);
			GraphicsPath path = new GraphicsPath();
			try {
				path.AddArc(arc, 180, 90);
				arc.X = (int) (bounds.Right - diameter);
				path.AddArc(arc, 270, 90);
				arc.Y = (int) (bounds.Bottom - diameter);
				path.AddArc(arc, 0, 90);
				arc.X = bounds.Left;
				path.AddArc(arc, 90, 90);
			} catch (Exception) { }
			path.CloseFigure();
			gc.DrawPath(GetPen(pen), path);
		}

		public void FillRoundedRactangle(Brush2 brush, float x, float y, float width, float height, float radius) {
			int diameter = (int) (radius * 2);
			Size size = new Size(diameter, diameter);
			RectangleF bounds = new RectangleF(x, y, width, height);
			RectangleF arc = new RectangleF(bounds.Location, size);
			GraphicsPath path = new GraphicsPath();
			try {
				path.AddArc(arc, 180, 90);
				arc.X = bounds.Right - diameter;
				path.AddArc(arc, 270, 90);
				arc.Y = bounds.Bottom - diameter;
				path.AddArc(arc, 0, 90);
				arc.X = bounds.Left;
				path.AddArc(arc, 90, 90);
			} catch (Exception) { }
			path.CloseFigure();
			gc.FillPath(GetBrush(brush), path);
		}

		public void DrawPolygon(Pen2 pen, Point2[] points) {
			gc.DrawPolygon(GetPen(pen), ToPointsF(points));
		}

		public void FillPolygon(Brush2 brush, Point2[] points) {
			gc.FillPolygon(GetBrush(brush), ToPointsF(points));
		}

		public Size2 MeasureString(string text, Font2 font) {
			return GraphUtils.ToSizeF2(gc.MeasureString(text, GraphUtils.ToFont(font)));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, float x, float y) {
			gc.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), x, y);
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF, StringFormat2 format) {
			gc.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), ToRectangleF(rectangleF),
				GraphUtils.ToStringFormat(format));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 location) {
			gc.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), GraphUtils.ToPointF(location));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF) {
			gc.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), ToRectangleF(rectangleF));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 point, StringFormat2 format) {
			gc.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), GraphUtils.ToPointF(point),
				GraphUtils.ToStringFormat(format));
		}

		public void DrawImage(Bitmap2 image, float x, float y, float width, float height) {
			gc.DrawImage(GraphUtils.ToBitmap(image), x, y, width, height);
		}

		public void DrawImageUnscaled(Bitmap2 image, float x, float y) {
			gc.DrawImageUnscaled(GraphUtils.ToBitmap(image), (int) x, (int) y);
		}

		public Size2 MeasureString(string text, Font2 font, float width) {
			return GraphUtils.ToSizeF2(gc.MeasureString(text, GraphUtils.ToFont(font), (int) width));
		}

		public void FillClosedCurve(Brush2 brush, Point2[] points) {
			gc.FillClosedCurve(GetBrush(brush), ToPointsF(points));
		}

		public void DrawCurve(Pen2 pen, Point2[] points) {
			gc.DrawCurve(GetPen(pen), ToPointsF(points));
		}

		public void TranslateTransform(float dx, float dy) {
			gc.TranslateTransform(dx, dy);
		}

		public void ResetTransform() {
			gc.ResetTransform();
		}

		public void ResetClip() {
			gc.ResetClip();
		}

		public void SetClip(Rectangle2 rectangle) {
			gc.SetClip(ToRectangleF(rectangle));
		}

		public abstract void Close();

		public float GetDpiScale() {
			return FormUtils.GetDpiScale(gc);
		}

		private static DashStyle ConvertDashStyle(DashStyle2 style) {
			switch (style) {
				case DashStyle2.Custom: return DashStyle.Custom;
				case DashStyle2.Dash: return DashStyle.Dash;
				case DashStyle2.DashDot: return DashStyle.DashDot;
				case DashStyle2.DashDotDot: return DashStyle.DashDotDot;
				case DashStyle2.Dot: return DashStyle.Dot;
				case DashStyle2.Solid: return DashStyle.Solid;
			}
			throw new NotImplementedException($"Cannot convert {style}");
		}

		public static Pen GetPen(Pen2 p) {
			return new Pen(Color.FromArgb(p.Color.A, p.Color.R, p.Color.G, p.Color.B), p.Width) {
				DashStyle = ConvertDashStyle(p.DashStyle),
			};
		}

		private static Brush GetBrush(Brush2 b) {
			return new SolidBrush(Color.FromArgb(b.Color.A, b.Color.R, b.Color.G, b.Color.B));
		}

		private static PointF[] ToPointsF(Point2[] p) {
			PointF[] result = new PointF[p.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = GraphUtils.ToPointF(p[i]);
			}
			return result;
		}

		private static GraphicsPath GetGraphicsPath(GraphicsPath2 path) {
			byte[] types = new byte[path.PathPoints.Length];
			for (int i = 1; i < types.Length; i++) {
				types[i] = 1;
			}
			return new GraphicsPath(ToPointsF(path.PathPoints), new byte[path.PathPoints.Length]);
		}

		private static RectangleF ToRectangleF(Rectangle2 rectangle) {
			return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
	}
}