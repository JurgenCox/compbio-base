using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibS.Graph;
using SvgNet.SvgGdi;
using IGraphics = BaseLibS.Graph.IGraphics;

namespace BaseLib.Graphic
{
	public class SvgGraphics2 : IGraphics
	{
		private SvgNet.SvgGdi.SvgGraphics _svgGraphics;
		private Stream _stream;

		public SvgGraphics2()
		{
			_svgGraphics = new SvgNet.SvgGdi.SvgGraphics();
		}
		public SvgGraphics2(string file) : this()
		{
			_stream = new FileStream(file, FileMode.Create);
		}

		public SvgGraphics2(Stream stream) : this()
		{
			_stream = stream;
		}

		public SmoothingMode2 SmoothingMode {
			get => GraphUtils.ToSmoothingMode2(_svgGraphics.SmoothingMode);
			set => _svgGraphics.SmoothingMode = GraphUtils.ToSmoothingMode(value);
		}

		public void Dispose()
		{
			_svgGraphics = null;
			_stream.Dispose();
		}

		public void RotateTransform(float angle)
		{
			_svgGraphics.RotateTransform(angle);
		}

		public void ScaleTransform(float sx, float sy)
		{
			_svgGraphics.ScaleTransform(sx, sy);
		}

		public void SetClippingMask(float width, float height, float x, float y)
		{
			// Does nothing - same as WindowsBasedGraphics
		}

		public void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2)
		{
			_svgGraphics.DrawLine(WindowsBasedGraphics.GetPen(pen), x1, y1, x2, y2);
		}

		public void DrawInterceptedLine(Pen2 pen, float x1, float y1, float x2, float y2, float len)
		{
			_svgGraphics.DrawLine(WindowsBasedGraphics.GetPen(pen), x1, y1, x2, y2);
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
			_svgGraphics.DrawLine(GetPen(pen), x3, y3, x4, y4);
		}

		private static Pen GetPen(Pen2 pen)
		{
			return WindowsBasedGraphics.GetPen(pen);
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
				_svgGraphics.DrawLine(GetPen(pen), x1, y1, newX2, newY2);
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
				_svgGraphics.DrawLine(GetPen(pen), x1, y1, newX2, newY2);
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
				_svgGraphics.DrawLine(GetPen(pen), x1, y1, newX2, newY2);
			}
			_svgGraphics.DrawPolygon(GetPen(pen),
				new[] {
					new Point(Convert.ToInt32(x2), Convert.ToInt32(y2)), new Point(Convert.ToInt32(x3), Convert.ToInt32(y3)),
					new Point(Convert.ToInt32(x4), Convert.ToInt32(y4))
				});
		}

		private static Brush GetBrush(Brush2 b)
		{
			return WindowsBasedGraphics.GetBrush(b);
		}

		private static PointF[] ToPointsF(Point2[] p)
		{
			return WindowsBasedGraphics.ToPointsF(p);
		}

		private static GraphicsPath GetGraphicsPath(GraphicsPath2 path)
		{
			return WindowsBasedGraphics.GetGraphicsPath(path);
		}

		private static RectangleF ToRectangleF(Rectangle2 rectangle)
		{
			return WindowsBasedGraphics.ToRectangleF(rectangle);
		}

		public void DrawPath(Pen2 pen, GraphicsPath2 path)
		{
			_svgGraphics.DrawPath(GetPen(pen), GetGraphicsPath(path));
		}

		public void DrawLines(Pen2 pen, Point2[] points)
		{
			_svgGraphics.DrawLines(GetPen(pen), ToPointsF(points));
		}

		public void DrawEllipse(Pen2 pen, float x, float y, float width, float height)
		{
			_svgGraphics.DrawEllipse(GetPen(pen), x, y, width, height);
		}

		public void FillEllipse(Brush2 brush, float x, float y, float width, float height)
		{
			_svgGraphics.FillEllipse(GetBrush(brush), x, y, width, height);
		}

		public void DrawRectangle(Pen2 pen, float x, float y, float width, float height)
		{
			_svgGraphics.DrawRectangle(GetPen(pen), x, y, width, height);
		}

		public void FillRectangle(Brush2 brush, float x, float y, float width, float height)
		{
			_svgGraphics.FillRectangle(GetBrush(brush), x, y, width, height);
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
			_svgGraphics.DrawPath(GetPen(pen), path);
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
			_svgGraphics.FillPath(GetBrush(brush), path);
		}

		public void DrawArc(Pen2 pen, Rectangle2 rec, float startAngle, float sweepAngle)
		{
			_svgGraphics.DrawArc(GetPen(pen), ToRectangleF(rec), startAngle, sweepAngle);
		}

		public void DrawPolygon(Pen2 pen, Point2[] points)
		{
			_svgGraphics.DrawPolygon(GetPen(pen), ToPointsF(points));
		}

		public void FillPolygon(Brush2 brush, Point2[] points)
		{
			_svgGraphics.FillPolygon(GetBrush(brush), ToPointsF(points));
		}

		public Size2 MeasureString(string text, Font2 font)
		{
			return GraphUtils.ToSizeF2(_svgGraphics.MeasureString(text, GraphUtils.ToFont(font)));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, float x, float y)
		{
			_svgGraphics.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), x, y);
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF, StringFormat2 format)
		{
			_svgGraphics.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), ToRectangleF(rectangleF), GraphUtils.ToStringFormat(format));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 point, StringFormat2 format)
		{
			try
			{
				_svgGraphics.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), GraphUtils.ToPointF(point),
					GraphUtils.ToStringFormat(format));
			}
			catch (SvgGdiNotImpl)
			{
				DrawString(s, font, brush, point);
			}
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 location)
		{
			_svgGraphics.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), GraphUtils.ToPointF(location));
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF)
		{
			_svgGraphics.DrawString(s, GraphUtils.ToFont(font), GetBrush(brush), ToRectangleF(rectangleF));
		}

		public void DrawImage(Bitmap2 image, float x, float y, float width, float height)
		{
			// TODO SvgNet implementation creates one rectangle for each pixel which is super slow for heatmap. Instead base64 encode png and embed as inkscape does.
			_svgGraphics.DrawImage(GraphUtils.ToBitmap(image), x, y, width, height);
		}

		public void DrawImageUnscaled(Bitmap2 image, float x, float y)
		{
			// TODO SvgNet implementation creates one rectangle for each pixel which is super slow for heatmap. Instead base64 encode png and embed as inkscape does.
			_svgGraphics.DrawImageUnscaled(GraphUtils.ToBitmap(image), (int) x, (int) y);
		}

		public Size2 MeasureString(string text, Font2 font, float width)
		{
			return GraphUtils.ToSizeF2(_svgGraphics.MeasureString(text, GraphUtils.ToFont(font), (int) width));
		}

		public void FillClosedCurve(Brush2 brush, Point2[] points)
		{
			_svgGraphics.FillClosedCurve(GetBrush(brush), ToPointsF(points));
		}

		public void DrawCurve(Pen2 pen, Point2[] points)
		{
			_svgGraphics.DrawCurve(GetPen(pen), ToPointsF(points));
		}

		public void TranslateTransform(float dx, float dy)
		{
			_svgGraphics.TranslateTransform(dx, dy);
		}

		public void ResetTransform()
		{
			_svgGraphics.ResetTransform();
		}

		public void ResetClip()
		{
			_svgGraphics.ResetClip();
		}

		public void SetClip(Rectangle2 rectangle)
		{
			_svgGraphics.SetClip(ToRectangleF(rectangle));
		}

		public void Close()
		{
			using (var writer = new StreamWriter(_stream))
			{
				writer.Write(_svgGraphics.WriteSVGString());
			}
			_stream.Close();
		}

		public float GetDpiScale()
		{
			return 1.0f;
		}
	}
}
