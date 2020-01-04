using System;

namespace BaseLibS.Graph{
	/// <summary>
	/// This interface provides the abstract basis for graphics routines for different devices (window, pdf,
	/// etc). As such, graphics routines can be fully generalized and can be reused when exporting graphics
	/// to a file. The routines defined here are fully based on those from the C# class Graphics, in order
	/// to keep the routines legible.
	/// <p/>
	/// It is not intended to be complete
	/// </summary>
	public interface IGraphics{
		/// <summary>
		/// 
		/// </summary>
		SmoothingMode2 SmoothingMode { get; set; }

		void Dispose();

		/// <summary>
		/// Applies the specified rotation to the transformation matrix.
		/// </summary>
		/// <param name="angle">Angle of rotation in degrees.</param>
		void RotateTransform(float angle);

        /// <summary>
        /// Scales the drawing.
        /// </summary>
        /// <param name="sx">x-direction</param>
        /// <param name="sy">y-direction</param>
	    void ScaleTransform(float sx, float sy);

		/// <summary>
		/// Sets the clipping-mask for the graphics device to draw in. This is required for applications which
		/// draw on a single canvas, where the normal grapics device draws in multiple controls. The origin (i.e.
		/// (0,0)) is moved to the given x and y position and the width and height is set to the new values. For
		/// file formats, this should have the effect that a new element is created in which all the graphics
		/// operations are located.
		/// </summary>
		/// <param name="width">The width of the clipping mask.</param>
		/// <param name="height">The height of the clipping mask.</param>
		/// <param name="x">The x-position of the clipping mask.</param>
		/// <param name="y">The y-position of the clipping mask.</param>
		//TODO:somehow clipping and transforming is mixed up here.
		[Obsolete]
		void SetClippingMask(float width, float height, float x, float y);

		/// <summary>
		/// Draws a line connecting the two points specified by the coordinate pairs.
		/// </summary>
		/// <param name="pen">Pen that determines the color, width, and style of the line.</param>
		/// <param name="x1">The x-coordinate of the first point.</param>
		/// <param name="y1">The y-coordinate of the first point. </param>
		/// <param name="x2">The x-coordinate of the second point.</param>
		/// <param name="y2">The y-coordinate of the second point. </param>
		void DrawLine(Pen2 pen, float x1, float y1, float x2, float y2);

       
	    void DrawInterceptedLine(Pen2 pen, float x1, float y1, float x2, float y2, float len);

	    void DrawArrow(Pen2 pen, float x1, float y1, float x2, float y2, float side);

		/// <summary>
		/// Draws a GraphicsPath.
		/// </summary>
		/// <param name="pen">Pen that determines the color, width, and style of the path.</param>
		/// <param name="path">GraphicsPath to draw.</param>
		void DrawPath(Pen2 pen, GraphicsPath2 path);

		void DrawLines(Pen2 pen, Point2[] points);

		/// <summary>
		/// Draws an ellipse defined by a bounding rectangle specified by coordinates for the upper-left corner of
		/// the rectangle, a height, and a width.
		/// </summary>
		/// <param name="pen">Pen that determines the color, width, and style of the ellipse.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
		void DrawEllipse(Pen2 pen, float x, float y, float width, float height);

		/// <summary>
		/// Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.
		/// </summary>
		/// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
		void FillEllipse(Brush2 brush, float x, float y, float width, float height);

		/// <summary>
		/// Draws a rectangle specified by a coordinate pair, a width, and a height.
		/// </summary>
		/// <param name="pen">Pen  that determines the color, width, and style of the rectangle.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="width">Width of the rectangle to draw.</param>
		/// <param name="height">Height of the rectangle to draw.</param>
		void DrawRectangle(Pen2 pen, float x, float y, float width, float height);

		/// <summary>
		/// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
		/// </summary>
		/// <param name="brush">Brush that determines the characteristics of the fill.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="width">Width of the rectangle to fill.</param>
		/// <param name="height">Height of the rectangle to fill.</param>
		void FillRectangle(Brush2 brush, float x, float y, float width, float height);


	    void DrawRoundedRectangle(Pen2 pen, float x, float y, float width, float height, float radius);
	    void FillRoundedRactangle(Brush2 brush, float x, float y, float width, float height, float radius);
		void DrawArc(Pen2 pen, Rectangle2 rec, float startAngle, float sweepAngle);

		void DrawPolygon(Pen2 pen, Point2[] points);
		void FillPolygon(Brush2 brush, Point2[] points);

		/// <summary>
		/// Measures the specified string when drawn with the specified Font.
		/// </summary>
		/// <param name="text">String to measure.</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <returns></returns>
		Size2 MeasureString(string text, Font2 font);

		/// <summary>
		/// Draws the specified text string at the specified location with the specified Brush and Font objects.
		/// </summary>
		/// <param name="s">String to draw.</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
		void DrawString(string s, Font2 font, Brush2 brush, float x, float y);

		/// <summary>
		/// Draws the specified text string at the specified location with the specified Brush and Font objects.
		/// </summary>
		/// <param name="s">String to draw.</param>
		/// <param name="font">Font that defines the text format of the string.</param>
		/// <param name="brush">Brush that determines the color and texture of the drawn text.</param>
		/// <param name="rectangleF">System.Drawing.RectangleF structure that specifies the location of the drawn text.</param>
		/// <param name="format">System.Drawing.StringFormat that specifies formatting attributes, such as line 
		/// spacing and alignment, that are applied to the drawn text.</param>
		void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF, StringFormat2 format);

		void DrawString(string s, Font2 font, Brush2 brush, Point2 point, StringFormat2 format);

		void DrawString(string s, Font2 font, Brush2 brush, Point2 location);
		void DrawString(string s, Font2 font, Brush2 brush, Rectangle2 rectangleF);

		/// <summary>
		/// Draws the specified Image at the specified location and with the specified size.
		/// </summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="width">Width of the drawn image.</param>
		/// <param name="height">Height of the drawn image.</param>
		void DrawImage(Bitmap2 image, float x, float y, float width, float height);

		/// <summary>
		/// Draws the specified image using its original physical size at the location specified by a coordinate pair.
		/// </summary>
		/// <param name="image">Image to draw.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image.</param>
		void DrawImageUnscaled(Bitmap2 image, float x, float y);

		Size2 MeasureString(string text, Font2 font, float width);
		void FillClosedCurve(Brush2 brush, Point2[] points);
		void DrawCurve(Pen2 pen, Point2[] points);
		void TranslateTransform(float dx, float dy);
		void ResetTransform();
		void ResetClip();
		void SetClip(Rectangle2 rectangle);
		void Close();
		float GetDpiScale();
	}
}