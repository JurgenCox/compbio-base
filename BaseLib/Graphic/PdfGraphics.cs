using System;
using System.Drawing.Drawing2D;
using System.IO;
using BaseLibS.Graph;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BaseLib.Graphic {
	public class PdfGraphics : IGraphics, IPdfGraphics {
		private float currentWidth;
		private float currentHeight;
		private readonly float originalWidth;
		private readonly float originalHeight;
		private readonly Document document;
		private readonly PdfWriter writer;
		private readonly PdfTemplate topTemplate;
		private PdfTemplate template;

		public PdfGraphics(string filename, int width, int height) : this(new FileStream(filename, FileMode.Create), width,
			height) { }

		public PdfGraphics(Stream stream, int width, int height) {
			originalWidth = currentWidth = width;
			originalHeight = currentHeight = height;
			document = new Document(new Rectangle(width, height), 50, 50, 50, 50);
			document.AddAuthor("");
			document.AddSubject("");
			try {
				writer = PdfWriter.GetInstance(document, stream);
				document.Open();
				PdfContentByte content = writer.DirectContent;
				template = topTemplate = content.CreateTemplate(width, height);
				content.AddTemplate(template, 0, -20);
			} catch (DocumentException de) {
				throw new IOException(de.Message);
			}
		}

		public void Dispose() {
			document?.Dispose();
			writer?.Dispose();
		}

		public SmoothingMode2 SmoothingMode { get; set; }

		public void ScaleTransform(float sx, float sy) {
			Matrix m = new Matrix();
			m.Scale(sx, sy);
			template.Transform(m);
		}

		public void SetClippingMask(float width, float height, float x, float y) {
			template = topTemplate.CreateTemplate(width, height);
			topTemplate.AddTemplate(template, x, topTemplate.Height - height - y);
			currentWidth = width;
			currentHeight = height;
		}

		public void RotateTransform(float angle) {
			System.Diagnostics.Debug.Assert(angle == 90 || angle == -90,
				"PdfGraphics.RotateTransform(float) currently only supports -90 and 90 degrees");
			Matrix m = new Matrix();
			if (angle == 90) {
				m.Translate(-currentHeight, currentHeight);
				m.Rotate(-angle);
			} else if (angle == -90) {
				m.Rotate(-angle);
				m.Translate(currentHeight, -currentHeight);
			}
			template.Transform(m);
		}

		public void Clear(Color2 color) {
			template.SetRGBColorFill(color.R, color.G, color.B);
			template.Rectangle(0, 0, template.Width, template.Height);
			template.FillStroke();
		}

		public void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2) {
			SetPen(pen);
			template.MoveTo(x1, currentHeight - y1);
			template.LineTo(x2, currentHeight - y2);
			template.Stroke();
		}

		public void DrawInterceptedLine(Pen2 pen, float x1, float y1, float x2, float y2, float len) {
			throw new NotImplementedException();
		}

		public void DrawArrow(Pen2 pen, float x1, float y1, float x2, float y2, float side) {
			throw new NotImplementedException();
		}

		public void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2, string title, string description) {
			DrawLine(pen, x1, y1, x2, y2);
		}

		public void DrawPath(Pen2 pen, GraphicsPath2 path) {
			SetPen(pen);
			for (int index = 0; index < path.PathPoints.Length; index++) {
				Point2 point = path.PathPoints[index];
				if (index == 0) {
					template.MoveTo(point.X, currentHeight - point.Y);
				} else {
					template.LineTo(point.X, currentHeight - point.Y);
					template.Stroke();
					template.MoveTo(point.X, currentHeight - point.Y);
				}
			}
			template.Stroke();
		}

		public void DrawLines(Pen2 pen, Point2[] points) {
			SetPen(pen);
			for (int index = 0; index < points.Length; index++) {
				Point2 point = points[index];
				if (index == 0) {
					template.MoveTo(point.X, currentHeight - point.Y);
				} else {
					template.LineTo(point.X, currentHeight - point.Y);
					template.Stroke();
					template.MoveTo(point.X, currentHeight - point.Y);
				}
			}
			template.Stroke();
		}

		public void DrawEllipse(Pen2 pen, float x, float y, float width, float height) {
			SetPen(pen);
			template.Ellipse(x, currentHeight - y, x + width, currentHeight - (y + height));
			template.Stroke();
		}

		public void FillEllipse(Brush2 brush, float x, float y, float width, float height) {
			SetBrush(brush);
			template.Ellipse(x, currentHeight - y, x + width, currentHeight - (y + height));
			template.Fill();
		}

		public void DrawRectangle(Pen2 pen, float x, float y, float width, float height) {
			SetPen(pen);
			template.Rectangle(x, currentHeight - y, width, -height);
			template.Stroke();
		}

		public void FillRectangle(Brush2 brush, float x, float y, float width, float height) {
			if ((brush).Color.IsEmpty) {
				return;
			}
			SetBrush(brush);
			template.Rectangle(x, currentHeight - y, width, -height);
			template.Fill();
		}

		public void DrawRoundedRectangle(Pen2 pen, float x, float y, float width, float height, float radius) {
			throw new NotImplementedException();
		}

		public void FillRoundedRactangle(Brush2 brush, float x, float y, float width, float height, float radius) {
			throw new NotImplementedException();
		}

		public void DrawArc(Pen2 pen, Rectangle2 rec, float startAngle, float sweepAngle) {
			throw new NotImplementedException();
		}

		public void DrawPolygon(Pen2 pen, Point2[] points) {
			SetPen(pen);
			for (int index = 0; index < points.Length; index++) {
				Point2 point = points[index];
				if (index == 0) {
					template.MoveTo(point.X, currentHeight - point.Y);
				} else {
					template.LineTo(point.X, currentHeight - point.Y);
					template.Stroke();
					template.MoveTo(point.X, currentHeight - point.Y);
				}
			}
			template.Stroke();
		}

		public void FillPolygon(Brush2 brush, Point2[] points) {
			SetBrush(brush);
			for (int index = 0; index < points.Length; index++) {
				Point2 point = points[index];
				if (index == 0) {
					template.MoveTo(point.X, currentHeight - point.Y);
				} else {
					template.LineTo(point.X, currentHeight - point.Y);
					template.Stroke();
					template.MoveTo(point.X, currentHeight - point.Y);
				}
			}
			template.Fill();
		}

		public Size2 MeasureString(string text, Font2 font) {
			SetFont(font);
			Chunk chunk = new Chunk(text, PdfGraphicUtils.GetFont(font));
			return new Size2(chunk.GetWidthPoint() * 1.5f, font.Height * 0.5f * 1.5f);
		}

		public Size2 MeasureString(string text, Font2 font, float width) {
			return MeasureString(text, font);
		}

		public void FillClosedCurve(Brush2 brush, Point2[] points) {
			FillPolygon(brush, points);
		}

		public void DrawCurve(Pen2 pen, Point2[] points) {
			DrawPolygon(pen, points);
		}

		public void TranslateTransform(float dx, float dy) {
			Matrix m = new Matrix();
			m.Translate(dx, -dy);
		}

		//TODO: is the transform additive?
		public void ResetTransform() {
			template.Transform(new Matrix());
		}

		public void ResetClip() {
			//TODO
		}

		public void SetClip(Rectangle2 rectangle) {
			template = topTemplate.CreateTemplate(rectangle.Width, rectangle.Height);
			topTemplate.AddTemplate(template, rectangle.X, topTemplate.Height - rectangle.Height - rectangle.Y);
			currentWidth = rectangle.Width;
			currentHeight = rectangle.Height;
		}

		public void Close() {
			document.Close();
			writer.Close();
		}

		public float GetDpiScale() {
			return 1;
		}

		public void DrawString(string s, Font2 font, Brush2 brush, float x, float y) {
			StringFormat2 format = new StringFormat2 {Alignment = StringAlignment2.Near, LineAlignment = StringAlignment2.Near};
			DrawString(s, font, brush, new Rectangle2(new Point2(x, y), MeasureString(s, font)), format);
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 point, StringFormat2 format) {
			DrawString(s, font, brush, new Rectangle2(point, MeasureString(s, font)), format);
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF, StringFormat2 format) {
			template.BeginText();
			SetFont(font);
			SetBrush(brush);
			float x = rectangleF.X;
			float y = Math.Max(0, rectangleF.Y - rectangleF.Height / 2);
			Size2 size = MeasureString(s, font);
			if (format != null) {
				switch (format.Alignment) {
					case StringAlignment2.Center:
						x = x + (rectangleF.Width - size.Width) / 2f;
						break;
					case StringAlignment2.Near:
						x = x + 2;
						break;
					case StringAlignment2.Far:
						x = x + (rectangleF.Width - size.Width) - 2;
						break;
				}
				switch (format.LineAlignment) {
					case StringAlignment2.Center:
						y = y + font.Height / 2f;
						break;
					case StringAlignment2.Near:
						y = y + 2;
						break;
					case StringAlignment2.Far:
						y = y + font.Height;
						break;
				}
			}
			template.SetTextMatrix(x, currentHeight - y - font.Height * 0.5f * 1.5f);
			template.ShowText(s.TrimStart().TrimEnd());
			template.EndText();
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Point2 location) {
			DrawString(s, font, brush, new Rectangle2(location, new Size2(0, 0)), new StringFormat2());
		}

		public void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF) {
			DrawString(s, font, brush, rectangleF, new StringFormat2());
		}

		public void DrawImage(Bitmap2 image, float x, float y, float width, float height) {
			Image img = null;
			try {
				img = Image.GetInstance(GraphUtils.ToBitmap(image), System.Drawing.Imaging.ImageFormat.Tiff);
			} catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
			}
			if (img != null) {
				img.ScaleAbsolute(width, height);
				img.SetAbsolutePosition(x, currentHeight - img.ScaledHeight - y);
				template.AddImage(img);
			}
		}

		public void DrawImageUnscaled(Bitmap2 image, float x, float y) {
			// TODO reduce the resolution to fit (?)
			try {
				Image img = Image.GetInstance(GraphUtils.ToBitmap(image), System.Drawing.Imaging.ImageFormat.Tiff);
				img.SetAbsolutePosition(x, currentHeight - img.ScaledHeight - y);
				template.AddImage(img);
			} catch { }
		}

		private void SetFont(Font2 font) {
			Font f = PdfGraphicUtils.GetFont(font);
			template.SetFontAndSize(f.BaseFont, font.Size);
		}

	    private void SetPen(Pen2 pen) {
			template.SetRGBColorStroke(pen.Color.R, pen.Color.G, pen.Color.B);
			template.SetLineWidth(pen.Width);
			switch (pen.DashCap) {
				case DashCap2.Round:
					template.SetLineCap(PdfContentByte.LINE_CAP_ROUND);
					break;
			}
			switch (pen.DashStyle) {
				case DashStyle2.Solid:
					template.SetLineDash(0f);
					break;
				case DashStyle2.Dash:
					template.SetLineDash(pen.DashPattern, pen.DashOffset);
					break;
				case DashStyle2.Custom:
					template.SetLineDash(pen.DashPattern, pen.DashOffset);
					break;
			}
		}

		private void SetBrush(Brush2 b) {
			template.SetRGBColorFill(b.Color.R, b.Color.G, b.Color.B);
		}
	}
}