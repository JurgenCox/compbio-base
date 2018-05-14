using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using BaseLibS.Graph;

namespace BaseLib.Graphic{
	public static class GraphUtils{
		public static Bitmap ResizeImage(Image image, int width, int height){
			Rectangle destRect = new Rectangle(0, 0, width, height);
			Bitmap destImage = new Bitmap(width, height);
			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using (Graphics graphics = Graphics.FromImage(destImage)){
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				using (ImageAttributes wrapMode = new ImageAttributes()){
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}
			return destImage;
		}

		public static Size2 ToSizeF2(SizeF size){
			return new Size2{Width = size.Width, Height = size.Height};
		}

		public static Color2 ToColor2(Color c){
			return Color2.FromArgb(c.A, c.R, c.G, c.B);
		}

		public static Color ToColor(Color2 c){
			return Color.FromArgb(c.A, c.R, c.G, c.B);
		}

		public static Bitmap2 ToBitmap2(Bitmap bitmap){
			if (bitmap == null){
				return null;
			}
			Bitmap2 result = new Bitmap2(bitmap.Width, bitmap.Height);
			for (int i = 0; i < bitmap.Width; i++){
				for (int j = 0; j < bitmap.Height; j++){
					result.SetPixel(i, j, bitmap.GetPixel(i, j).ToArgb());
				}
			}
			return result;
		}

		public static Bitmap ToBitmap(Bitmap2 bitmap){
			if (bitmap == null){
				return null;
			}
			Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);
			for (int i = 0; i < bitmap.Width; i++){
				for (int j = 0; j < bitmap.Height; j++){
					result.SetPixel(i, j, Color.FromArgb(bitmap.GetPixel(i, j)));
				}
			}
			return result;
		}

		public static PointF ToPointF(Point2 location){
			return new PointF(location.X, location.Y);
		}

		public static Point2 ToPointF2(PointF location){
			return new Point2(location.X, location.Y);
		}

		public static Size2 ToSize2(Size size){
			return new Size2(size.Width, size.Height);
		}

		public static Font ToFont(Font2 f){
			return new Font(f.Name, f.Size, ToFontStyle(f.Style));
		}

		public static Font2 ToFont2(Font f)
		{
            // winforms FontDialog does not assign 'OriginalFontName'
		    var name = string.IsNullOrEmpty(f.OriginalFontName) ? f.Name : f.OriginalFontName;
			return new Font2(name, f.Size, ToFontStyle2(f.Style));
		}

		public static SmoothingMode ToSmoothingMode(SmoothingMode2 mode){
			switch (mode){
				case SmoothingMode2.Default:
					return SmoothingMode.Default;
				case SmoothingMode2.AntiAlias:
					return SmoothingMode.AntiAlias;
				case SmoothingMode2.HighQuality:
					return SmoothingMode.HighQuality;
				case SmoothingMode2.HighSpeed:
					return SmoothingMode.HighSpeed;
				case SmoothingMode2.Invalid:
					return SmoothingMode.Invalid;
				case SmoothingMode2.None:
					return SmoothingMode.None;
				default:
					throw new Exception("Never get here.");
			}
		}

		public static SmoothingMode2 ToSmoothingMode2(SmoothingMode mode){
			switch (mode){
				case SmoothingMode.Default:
					return SmoothingMode2.Default;
				case SmoothingMode.AntiAlias:
					return SmoothingMode2.AntiAlias;
				case SmoothingMode.HighQuality:
					return SmoothingMode2.HighQuality;
				case SmoothingMode.HighSpeed:
					return SmoothingMode2.HighSpeed;
				case SmoothingMode.Invalid:
					return SmoothingMode2.Invalid;
				case SmoothingMode.None:
					return SmoothingMode2.None;
				default:
					throw new Exception("Never get here.");
			}
		}

		public static FontStyle ToFontStyle(FontStyle2 style){
			switch (style){
				case FontStyle2.Regular:
					return FontStyle.Regular;
				case FontStyle2.Bold:
					return FontStyle.Bold;
				case FontStyle2.Italic:
					return FontStyle.Italic;
				case FontStyle2.Strikeout:
					return FontStyle.Strikeout;
				case FontStyle2.Underline:
					return FontStyle.Underline;
				default:
					throw new Exception("Never get here.");
			}
		}

		public static FontStyle2 ToFontStyle2(FontStyle style){
			switch (style){
				case FontStyle.Regular:
					return FontStyle2.Regular;
				case FontStyle.Bold:
					return FontStyle2.Bold;
				case FontStyle.Italic:
					return FontStyle2.Italic;
				case FontStyle.Strikeout:
					return FontStyle2.Strikeout;
				case FontStyle.Underline:
					return FontStyle2.Underline;
				default:
					throw new Exception("Never get here.");
			}
		}

		public static StringFormat ToStringFormat(StringFormat2 format){
			if (format.Vertical){
				return new StringFormat(StringFormatFlags.DirectionVertical){
					Alignment = ToStringAlignment(format.Alignment),
					LineAlignment = ToStringAlignment(format.LineAlignment)
				};
			}
			return new StringFormat{
				Alignment = ToStringAlignment(format.Alignment),
				LineAlignment = ToStringAlignment(format.LineAlignment)
			};
		}

		private static StringAlignment ToStringAlignment(StringAlignment2 alignment){
			switch (alignment){
				case StringAlignment2.Center:
					return StringAlignment.Center;
				case StringAlignment2.Near:
					return StringAlignment.Near;
				case StringAlignment2.Far:
					return StringAlignment.Far;
				default:
					throw new Exception("Never get here.");
			}
		}

		public static Cursor ToCursor(Cursors2 cursor){
			switch (cursor){
				case Cursors2.AppStarting:
					return Cursors.AppStarting;
				case Cursors2.Arrow:
					return Cursors.Arrow;
				case Cursors2.Cross:
					return Cursors.Cross;
				case Cursors2.Default:
					return Cursors.Default;
				case Cursors2.HSplit:
					return Cursors.HSplit;
				case Cursors2.Hand:
					return Cursors.Hand;
				case Cursors2.Help:
					return Cursors.Help;
				case Cursors2.IBeam:
					return Cursors.IBeam;
				case Cursors2.No:
					return Cursors.No;
				case Cursors2.NoMove2D:
					return Cursors.NoMove2D;
				case Cursors2.NoMoveHoriz:
					return Cursors.NoMoveHoriz;
				case Cursors2.NoMoveVert:
					return Cursors.NoMoveVert;
				case Cursors2.PanEast:
					return Cursors.PanEast;
				case Cursors2.PanSouth:
					return Cursors.PanSouth;
				case Cursors2.PanWest:
					return Cursors.PanWest;
				case Cursors2.PanNorth:
					return Cursors.PanNorth;
				case Cursors2.PanNE:
					return Cursors.PanNE;
				case Cursors2.PanNW:
					return Cursors.PanNW;
				case Cursors2.PanSE:
					return Cursors.PanSE;
				case Cursors2.PanSW:
					return Cursors.PanSW;
				case Cursors2.SizeAll:
					return Cursors.SizeAll;
				case Cursors2.SizeNESW:
					return Cursors.SizeNESW;
				case Cursors2.SizeNS:
					return Cursors.SizeNS;
				case Cursors2.SizeNWSE:
					return Cursors.SizeNWSE;
				case Cursors2.SizeWE:
					return Cursors.SizeWE;
				case Cursors2.WaitCursor:
					return Cursors.WaitCursor;
				case Cursors2.UpArrow:
					return Cursors.UpArrow;
				case Cursors2.VSplit:
					return Cursors.VSplit;
				default:
					throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
			}
		}
	}
}