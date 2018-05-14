using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using BaseLibS.Num.Space;

namespace BaseLibS.Graph{
	[Serializable, DebuggerDisplay("{NameAndArgbValue}")]
	public struct Color2{
		public static readonly Color2 Empty = new Color2();
		// static list of known colors... 
		public static Color2 Transparent => new Color2(KnownColor.Transparent);
		public static Color2 AliceBlue => new Color2(KnownColor.AliceBlue);
		public static Color2 AntiqueWhite => new Color2(KnownColor.AntiqueWhite);
		public static Color2 Aqua => new Color2(KnownColor.Aqua);
		public static Color2 Aquamarine => new Color2(KnownColor.Aquamarine);
		public static Color2 Azure => new Color2(KnownColor.Azure);
		public static Color2 Beige => new Color2(KnownColor.Beige);
		public static Color2 Bisque => new Color2(KnownColor.Bisque);
		public static Color2 Black => new Color2(KnownColor.Black);
		public static Color2 BlanchedAlmond => new Color2(KnownColor.BlanchedAlmond);
		public static Color2 Blue => new Color2(KnownColor.Blue);
		public static Color2 BlueViolet => new Color2(KnownColor.BlueViolet);
		public static Color2 Brown => new Color2(KnownColor.Brown);
		public static Color2 BurlyWood => new Color2(KnownColor.BurlyWood);
		public static Color2 CadetBlue => new Color2(KnownColor.CadetBlue);
		public static Color2 Chartreuse => new Color2(KnownColor.Chartreuse);
		public static Color2 Chocolate => new Color2(KnownColor.Chocolate);
		public static Color2 Coral => new Color2(KnownColor.Coral);
		public static Color2 CornflowerBlue => new Color2(KnownColor.CornflowerBlue);
		public static Color2 Cornsilk => new Color2(KnownColor.Cornsilk);
		public static Color2 Crimson => new Color2(KnownColor.Crimson);
		public static Color2 Cyan => new Color2(KnownColor.Cyan);
		public static Color2 DarkBlue => new Color2(KnownColor.DarkBlue);
		public static Color2 DarkCyan => new Color2(KnownColor.DarkCyan);
		public static Color2 DarkGoldenrod => new Color2(KnownColor.DarkGoldenrod);
		public static Color2 DarkGray => new Color2(KnownColor.DarkGray);
		public static Color2 DarkGreen => new Color2(KnownColor.DarkGreen);
		public static Color2 DarkKhaki => new Color2(KnownColor.DarkKhaki);
		public static Color2 DarkMagenta => new Color2(KnownColor.DarkMagenta);
		public static Color2 DarkOliveGreen => new Color2(KnownColor.DarkOliveGreen);
		public static Color2 DarkOrange => new Color2(KnownColor.DarkOrange);
		public static Color2 DarkOrchid => new Color2(KnownColor.DarkOrchid);
		public static Color2 DarkRed => new Color2(KnownColor.DarkRed);
		public static Color2 DarkSalmon => new Color2(KnownColor.DarkSalmon);
		public static Color2 DarkSeaGreen => new Color2(KnownColor.DarkSeaGreen);
		public static Color2 DarkSlateBlue => new Color2(KnownColor.DarkSlateBlue);
		public static Color2 DarkSlateGray => new Color2(KnownColor.DarkSlateGray);
		public static Color2 DarkTurquoise => new Color2(KnownColor.DarkTurquoise);
		public static Color2 DarkViolet => new Color2(KnownColor.DarkViolet);
		public static Color2 DeepPink => new Color2(KnownColor.DeepPink);
		public static Color2 DeepSkyBlue => new Color2(KnownColor.DeepSkyBlue);
		public static Color2 DimGray => new Color2(KnownColor.DimGray);
		public static Color2 DodgerBlue => new Color2(KnownColor.DodgerBlue);
		public static Color2 Firebrick => new Color2(KnownColor.Firebrick);
		public static Color2 FloralWhite => new Color2(KnownColor.FloralWhite);
		public static Color2 ForestGreen => new Color2(KnownColor.ForestGreen);
		public static Color2 Fuchsia => new Color2(KnownColor.Fuchsia);
		public static Color2 Gainsboro => new Color2(KnownColor.Gainsboro);
		public static Color2 GhostWhite => new Color2(KnownColor.GhostWhite);
		public static Color2 Gold => new Color2(KnownColor.Gold);
		public static Color2 Goldenrod => new Color2(KnownColor.Goldenrod);
		public static Color2 Gray => new Color2(KnownColor.Gray);
		public static Color2 Green => new Color2(KnownColor.Green);
		public static Color2 GreenYellow => new Color2(KnownColor.GreenYellow);
		public static Color2 Honeydew => new Color2(KnownColor.Honeydew);
		public static Color2 HotPink => new Color2(KnownColor.HotPink);
		public static Color2 IndianRed => new Color2(KnownColor.IndianRed);
		public static Color2 Indigo => new Color2(KnownColor.Indigo);
		public static Color2 Ivory => new Color2(KnownColor.Ivory);
		public static Color2 Khaki => new Color2(KnownColor.Khaki);
		public static Color2 Lavender => new Color2(KnownColor.Lavender);
		public static Color2 LavenderBlush => new Color2(KnownColor.LavenderBlush);
		public static Color2 LawnGreen => new Color2(KnownColor.LawnGreen);
		public static Color2 LemonChiffon => new Color2(KnownColor.LemonChiffon);
		public static Color2 LightBlue => new Color2(KnownColor.LightBlue);
		public static Color2 LightCoral => new Color2(KnownColor.LightCoral);
		public static Color2 LightCyan => new Color2(KnownColor.LightCyan);
		public static Color2 LightGoldenrodYellow => new Color2(KnownColor.LightGoldenrodYellow);
		public static Color2 LightGreen => new Color2(KnownColor.LightGreen);
		public static Color2 LightGray => new Color2(KnownColor.LightGray);
		public static Color2 LightPink => new Color2(KnownColor.LightPink);
		public static Color2 LightSalmon => new Color2(KnownColor.LightSalmon);
		public static Color2 LightSeaGreen => new Color2(KnownColor.LightSeaGreen);
		public static Color2 LightSkyBlue => new Color2(KnownColor.LightSkyBlue);
		public static Color2 LightSlateGray => new Color2(KnownColor.LightSlateGray);
		public static Color2 LightSteelBlue => new Color2(KnownColor.LightSteelBlue);
		public static Color2 LightYellow => new Color2(KnownColor.LightYellow);
		public static Color2 Lime => new Color2(KnownColor.Lime);
		public static Color2 LimeGreen => new Color2(KnownColor.LimeGreen);
		public static Color2 Linen => new Color2(KnownColor.Linen);
		public static Color2 Magenta => new Color2(KnownColor.Magenta);
		public static Color2 Maroon => new Color2(KnownColor.Maroon);
		public static Color2 MediumAquamarine => new Color2(KnownColor.MediumAquamarine);
		public static Color2 MediumBlue => new Color2(KnownColor.MediumBlue);
		public static Color2 MediumOrchid => new Color2(KnownColor.MediumOrchid);
		public static Color2 MediumPurple => new Color2(KnownColor.MediumPurple);
		public static Color2 MediumSeaGreen => new Color2(KnownColor.MediumSeaGreen);
		public static Color2 MediumSlateBlue => new Color2(KnownColor.MediumSlateBlue);
		public static Color2 MediumSpringGreen => new Color2(KnownColor.MediumSpringGreen);
		public static Color2 MediumTurquoise => new Color2(KnownColor.MediumTurquoise);
		public static Color2 MediumVioletRed => new Color2(KnownColor.MediumVioletRed);
		public static Color2 MidnightBlue => new Color2(KnownColor.MidnightBlue);
		public static Color2 MintCream => new Color2(KnownColor.MintCream);
		public static Color2 MistyRose => new Color2(KnownColor.MistyRose);
		public static Color2 Moccasin => new Color2(KnownColor.Moccasin);
		public static Color2 NavajoWhite => new Color2(KnownColor.NavajoWhite);
		public static Color2 Navy => new Color2(KnownColor.Navy);
		public static Color2 OldLace => new Color2(KnownColor.OldLace);
		public static Color2 Olive => new Color2(KnownColor.Olive);
		public static Color2 OliveDrab => new Color2(KnownColor.OliveDrab);
		public static Color2 Orange => new Color2(KnownColor.Orange);
		public static Color2 OrangeRed => new Color2(KnownColor.OrangeRed);
		public static Color2 Orchid => new Color2(KnownColor.Orchid);
		public static Color2 PaleGoldenrod => new Color2(KnownColor.PaleGoldenrod);
		public static Color2 PaleGreen => new Color2(KnownColor.PaleGreen);
		public static Color2 PaleTurquoise => new Color2(KnownColor.PaleTurquoise);
		public static Color2 PaleVioletRed => new Color2(KnownColor.PaleVioletRed);
		public static Color2 PapayaWhip => new Color2(KnownColor.PapayaWhip);
		public static Color2 PeachPuff => new Color2(KnownColor.PeachPuff);
		public static Color2 Peru => new Color2(KnownColor.Peru);
		public static Color2 Pink => new Color2(KnownColor.Pink);
		public static Color2 Plum => new Color2(KnownColor.Plum);
		public static Color2 PowderBlue => new Color2(KnownColor.PowderBlue);
		public static Color2 Purple => new Color2(KnownColor.Purple);
		public static Color2 Red => new Color2(KnownColor.Red);
		public static Color2 RosyBrown => new Color2(KnownColor.RosyBrown);
		public static Color2 RoyalBlue => new Color2(KnownColor.RoyalBlue);
		public static Color2 SaddleBrown => new Color2(KnownColor.SaddleBrown);
		public static Color2 Salmon => new Color2(KnownColor.Salmon);
		public static Color2 SandyBrown => new Color2(KnownColor.SandyBrown);
		public static Color2 SeaGreen => new Color2(KnownColor.SeaGreen);
		public static Color2 SeaShell => new Color2(KnownColor.SeaShell);
		public static Color2 Sienna => new Color2(KnownColor.Sienna);
		public static Color2 Silver => new Color2(KnownColor.Silver);
		public static Color2 SkyBlue => new Color2(KnownColor.SkyBlue);
		public static Color2 SlateBlue => new Color2(KnownColor.SlateBlue);
		public static Color2 SlateGray => new Color2(KnownColor.SlateGray);
		public static Color2 Snow => new Color2(KnownColor.Snow);
		public static Color2 SpringGreen => new Color2(KnownColor.SpringGreen);
		public static Color2 SteelBlue => new Color2(KnownColor.SteelBlue);
		public static Color2 Tan => new Color2(KnownColor.Tan);
		public static Color2 Teal => new Color2(KnownColor.Teal);
		public static Color2 Thistle => new Color2(KnownColor.Thistle);
		public static Color2 Tomato => new Color2(KnownColor.Tomato);
		public static Color2 Turquoise => new Color2(KnownColor.Turquoise);
		public static Color2 Violet => new Color2(KnownColor.Violet);
		public static Color2 Wheat => new Color2(KnownColor.Wheat);
		public static Color2 White => new Color2(KnownColor.White);
		public static Color2 WhiteSmoke => new Color2(KnownColor.WhiteSmoke);
		public static Color2 Yellow => new Color2(KnownColor.Yellow);
		public static Color2 YellowGreen => new Color2(KnownColor.YellowGreen);
		// NOTE : The "zero" pattern (all members being 0) must represent
		//      : "not set". This allows "Color c;" to be correct. 
		private const short stateKnownColorValid = 0x0001;
		private const short stateArgbValueValid = 0x0002;
		private const short stateValueMask = stateArgbValueValid;
		private const short stateNameValid = 0x0008;
		private const int notDefinedValue = 0;
		// Shift count and bit mask for A, R, G, B components in ARGB mode! 
		private const int argbAlphaShift = 24;
		private const int argbRedShift = 16;
		private const int argbGreenShift = 8;
		private const int argbBlueShift = 0;

		/// <summary>
		/// will contain standard 32bit sRGB (ARGB) 
		/// </summary>
		private readonly int value;

		/// <summary>
		/// ignored, unless "state" says it is valid 
		/// </summary>
		private readonly short knownColor;

		/// <summary>
		/// implementation specific information 
		/// </summary>
		private readonly short state;

		internal Color2(KnownColor knownColor){
			value = 0;
			state = stateKnownColorValid;
			this.knownColor = (short) knownColor;
		}

		private Color2(int value, short state, KnownColor knownColor){
			this.value = value;
			this.state = state;
			this.knownColor = (short) knownColor;
		}

		/// <summary>
		/// Gets the red component value for this <code>Color2</code>.
		/// </summary>
		public byte R => (byte) ((Value >> argbRedShift) & 0xFF);

		/// <summary>
		/// Gets the green component value for this <code>Color2</code>.
		/// </summary>
		public byte G => (byte) ((Value >> argbGreenShift) & 0xFF);

		/// <summary>
		/// Gets the blue component value for this <code>Color2</code>.
		/// </summary>
		public byte B => (byte) ((Value >> argbBlueShift) & 0xFF);

		/// <summary>
		/// Gets the alpha component value for this <code>Color2</code>.
		/// </summary>
		public byte A => (byte) ((Value >> argbAlphaShift) & 0xFF);

		/// <summary>
		/// Gets the red component value for the given <code>value</code>.
		/// </summary>
		public static byte GetR(int value){
			return (byte) ((value >> argbRedShift) & 0xFF);
		}

		/// <summary>
		/// Gets the green component value for the given <code>value</code>.
		/// </summary>
		public static byte GetG(int value){
			return (byte) ((value >> argbGreenShift) & 0xFF);
		}

		/// <summary>
		/// Gets the blue component value for the given <code>value</code>.
		/// </summary>
		public static byte GetB(int value){
			return (byte) ((value >> argbBlueShift) & 0xFF);
		}

		/// <summary>
		/// Gets the alpha component value for the given <code>value</code>.
		/// </summary>
		public static byte GetA(int value){
			return (byte) ((value >> argbAlphaShift) & 0xFF);
		}

		/// <summary>
		/// Specifies whether this <code>Color2</code> is a known (predefined) color.
		/// Predefined colors are defined in the <code>KnownColor</code> enum.
		/// </summary>
		public bool IsKnownColor => (state & stateKnownColorValid) != 0;

		/// <summary>
		///    Specifies whether this <code>Color2</code> is uninitialized.
		/// </summary>
		public bool IsEmpty => state == 0;

		/// <summary>
		///    Specifies whether this <code>Color2</code> has a name or is a <code>KnownColor</code>. 
		/// </summary>
		public bool IsNamedColor => ((state & stateNameValid) != 0) || IsKnownColor;

		/// <summary>
		///     Determines if this color is a system color.
		/// </summary>
		public bool IsSystemColor
			=>
				IsKnownColor &&
				(((KnownColor) knownColor <= KnownColor.WindowText) || ((KnownColor) knownColor > KnownColor.YellowGreen));

		private string NameAndArgbValue
			=> string.Format(CultureInfo.CurrentCulture, "{{Name={0}, ARGB=({1}, {2}, {3}, {4})}}", Name, A, R, G, B);

		/// <summary>
		///       Gets the name of this <code>Color2</code> . This will either return the user
		///       defined name of the color, if the color was created from a name, or 
		///       the name of the known color. For custom colors, the RGB value will 
		///       be returned.
		/// </summary>
		public string Name{
			get{
				if ((state & stateNameValid) != 0){
					return null;
				}
				if (IsKnownColor){
					// first try the table so we can avoid the (slow!) .ToString() 
					string tablename = KnownColorTable.KnownColorToName((KnownColor) knownColor);
					if (tablename != null){
						return tablename;
					}
					Debug.Assert(false, "Could not find known color '" + (KnownColor) knownColor + "' in the KnownColorTable");
				}

				// if we reached here, just encode the value
				//
				return Convert.ToString(value, 16);
			}
		}

		/// <summary>
		///     Actual color to be rendered. 
		/// </summary>
		public int Value{
			get{
				if ((state & stateValueMask) != 0){
					return value;
				}
				return IsKnownColor ? KnownColorTable.KnownColorToArgb((KnownColor) knownColor) : notDefinedValue;
			}
		}

		private static byte CheckByte(int value){
			if (value < 0 || value > 255){
				throw new ArgumentException("Invalid value");
			}
			return (byte) value;
		}

		/// <summary>
		/// Encodes the four values into ARGB (32 bit) format.
		/// </summary>
		private static long MakeArgb(byte alpha, byte red, byte green, byte blue){
			return
				(long) (uint) (red << argbRedShift | green << argbGreenShift | blue << argbBlueShift | alpha << argbAlphaShift) &
				0xffffffff;
		}

		/// <summary>
		/// Creates a Color from its 32-bit component (alpha, red, green, and blue) values.
		/// </summary>
		public static Color2 FromArgb(int argb){
			return new Color2(argb, stateArgbValueValid, 0);
		}

		public Vector4F ToVector4(){
			return new Vector4F(R/255F, G/255F, B/255F, A/255F);
		}

		public static Color2 FromHexString(string hex){
			// assuming AARRGGBB
			hex = hex.StartsWith("#") ? hex.Substring(1) : hex;
			if (hex.Length != 8 && hex.Length != 6 && hex.Length != 3){
				throw new ArgumentException("Hexadecimal string is not in the correct format.", nameof(hex));
			}
			int r;
			int g;
			int b;
			int a;
			if (hex.Length == 8){
				r = Convert.ToByte(hex.Substring(2, 2), 16);
				g = Convert.ToByte(hex.Substring(4, 2), 16);
				b = Convert.ToByte(hex.Substring(6, 2), 16);
				a = Convert.ToByte(hex.Substring(0, 2), 16);
			} else if (hex.Length == 6){
				r = Convert.ToByte(hex.Substring(0, 2), 16);
				g = Convert.ToByte(hex.Substring(2, 2), 16);
				b = Convert.ToByte(hex.Substring(4, 2), 16);
				a = 255;
			} else{
				string rh = char.ToString(hex[0]);
				string gh = char.ToString(hex[1]);
				string bh = char.ToString(hex[2]);
				r = Convert.ToByte(rh + rh, 16);
				g = Convert.ToByte(gh + gh, 16);
				b = Convert.ToByte(bh + bh, 16);
				a = 255;
			}
			return FromArgb(a, r, g, b);
		}

		public static Color2 FromVector4(Vector4F vector){
			Vector4F clamped = Vector4F.Clamp(vector, Vector4F.Zero, Vector4F.One)*255F;
			byte r = (byte) Math.Round(clamped.X);
			byte g = (byte) Math.Round(clamped.Y);
			byte b = (byte) Math.Round(clamped.Z);
			byte a = (byte) Math.Round(clamped.W);
			return FromArgb(a, r, g, b);
		}

		/// <summary>
		///       Creates a Color from its 32-bit component (alpha, red, green, and blue) values.
		/// </summary>
		public static Color2 FromArgb(int alpha, int red, int green, int blue){
			return new Color2((int) MakeArgb(CheckByte(alpha), CheckByte(red), CheckByte(green), CheckByte(blue)),
				stateArgbValueValid, 0);
		}

		/// <summary>
		///       Creates a new <code>Color2</code> from the specified <code>Color2</code>, but with 
		///       the new specified alpha value.
		/// </summary>
		public static Color2 FromArgb(int alpha, Color2 baseColor){
			return new Color2((int) MakeArgb(CheckByte(alpha), baseColor.R, baseColor.G, baseColor.B), stateArgbValueValid, 0);
		}

		public static Color2 Darker(Color2 baseColor, int n){
			return
				new Color2(
					(int)
						MakeArgb(CheckByte(baseColor.A), CheckByte(Math.Max(0, baseColor.R - n)), CheckByte(Math.Max(0, baseColor.G - n)),
							CheckByte(Math.Max(0, baseColor.B - n))), stateArgbValueValid, 0);
		}

		public static Color2 Lighter(Color2 baseColor, int n){
			return
				new Color2(
					(int)
						MakeArgb(CheckByte(baseColor.A), CheckByte(Math.Min(255, baseColor.R + n)),
							CheckByte(Math.Min(255, baseColor.G + n)), CheckByte(Math.Min(255, baseColor.B + n))), stateArgbValueValid, 0);
		}

		/// <summary>
		///       Creates a <code>Color2</code> from the specified red, green, and
		///       blue values.
		/// </summary>
		public static Color2 FromArgb(int red, int green, int blue){
			return FromArgb(255, red, green, blue);
		}

		/// <summary>
		///       Creates a <code>Color2</code> from the specified <code>KnownColor</code> . 
		/// </summary>
		internal static Color2 FromKnownColor(KnownColor color){
			return new Color2(color);
		}

		/// <summary>
		///       Returns the Hue-Saturation-Brightness (HSB) brightness
		///       for this <code>Color2</code>. 
		/// </summary>
		public float GetBrightness(){
			float r = R/255.0f;
			float g = G/255.0f;
			float b = B/255.0f;
			float max = r;
			float min = r;
			if (g > max){
				max = g;
			}
			if (b > max){
				max = b;
			}
			if (g < min){
				min = g;
			}
			if (b < min){
				min = b;
			}
			return (max + min)/2;
		}

		/// <summary>
		///       Returns the Hue-Saturation-Brightness (HSB) hue 
		///       value, in degrees, for this <code>Color2</code>.
		///       If R == G == B, the hue is meaningless, and the return value is 0. 
		/// </summary>
		public float GetHue(){
			if (R == G && G == B){
				return 0; // 0 makes as good an UNDEFINED value as any
			}
			float r = R/255.0f;
			float g = G/255.0f;
			float b = B/255.0f;
			float hue = 0.0f;
			float max = r;
			float min = r;
			if (g > max){
				max = g;
			}
			if (b > max){
				max = b;
			}
			if (g < min){
				min = g;
			}
			if (b < min){
				min = b;
			}
			float delta = max - min;
			if (r == max){
				hue = (g - b)/delta;
			} else if (g == max){
				hue = 2 + (b - r)/delta;
			} else if (b == max){
				hue = 4 + (r - g)/delta;
			}
			hue *= 60;
			if (hue < 0.0f){
				hue += 360.0f;
			}
			return hue;
		}

		/// <summary>
		///       The Hue-Saturation-Brightness (HSB) saturation for this 
		///    <code>Color2</code>.
		/// </summary>
		public float GetSaturation(){
			float r = R/255.0f;
			float g = G/255.0f;
			float b = B/255.0f;
			float s = 0;
			float max = r;
			float min = r;
			if (g > max){
				max = g;
			}
			if (b > max){
				max = b;
			}
			if (g < min){
				min = g;
			}
			if (b < min){
				min = b;
			}
			// if max == min, then there is no color and 
			// the saturation is zero.
			//
			if (max != min){
				float l = (max + min)/2;
				if (l <= .5){
					s = (max - min)/(max + min);
				} else{
					s = (max - min)/(2 - max - min);
				}
			}
			return s;
		}

		/// <summary>
		///       Returns the ARGB value of this <code>Color2</code>.
		/// </summary>
		/// 
		public int ToArgb(){
			return Value;
		}

		public byte[] ToBytes(){
			return new[]{R, G, B, A};
		}

		public int GetPackedValue(){
			return Value;
		}

		private static readonly Lazy<Color2[]> safeColors = new Lazy<Color2[]>(GetWebSafeColors);
		public static Color2[] WebSafeColors => safeColors.Value;

		private static Color2[] GetWebSafeColors(){
			return new[]{
				AliceBlue, AntiqueWhite, Aqua, Aquamarine, Azure, Beige, Bisque, Black, BlanchedAlmond, Blue, BlueViolet, Brown,
				BurlyWood, CadetBlue, Chartreuse, Chocolate, Coral, CornflowerBlue, Cornsilk, Crimson, Cyan, DarkBlue, DarkCyan,
				DarkGoldenrod, DarkGray, DarkGreen, DarkKhaki, DarkMagenta, DarkOliveGreen, DarkOrange, DarkOrchid, DarkRed,
				DarkSalmon, DarkSeaGreen, DarkSlateBlue, DarkSlateGray, DarkTurquoise, DarkViolet, DeepPink, DeepSkyBlue, DimGray,
				DodgerBlue, Firebrick, FloralWhite, ForestGreen, Fuchsia, Gainsboro, GhostWhite, Gold, Goldenrod, Gray, Green,
				GreenYellow, Honeydew, HotPink, IndianRed, Indigo, Ivory, Khaki, Lavender, LavenderBlush, LawnGreen, LemonChiffon,
				LightBlue, LightCoral, LightCyan, LightGoldenrodYellow, LightGray, LightGreen, LightPink, LightSalmon, LightSeaGreen,
				LightSkyBlue, LightSlateGray, LightSteelBlue, LightYellow, Lime, LimeGreen, Linen, Magenta, Maroon, MediumAquamarine,
				MediumBlue, MediumOrchid, MediumPurple, MediumSeaGreen, MediumSlateBlue, MediumSpringGreen, MediumTurquoise,
				MediumVioletRed, MidnightBlue, MintCream, MistyRose, Moccasin, NavajoWhite, Navy, OldLace, Olive, OliveDrab, Orange,
				OrangeRed, Orchid, PaleGoldenrod, PaleGreen, PaleTurquoise, PaleVioletRed, PapayaWhip, PeachPuff, Peru, Pink, Plum,
				PowderBlue, Purple, Red, RosyBrown, RoyalBlue, SaddleBrown, Salmon, SandyBrown, SeaGreen, SeaShell, Sienna, Silver,
				SkyBlue, SlateBlue, SlateGray, Snow, SpringGreen, SteelBlue, Tan, Teal, Thistle, Tomato, Transparent, Turquoise,
				Violet, Wheat, White, WhiteSmoke, Yellow, YellowGreen
			};
		}

		public static implicit operator Color2(YCbCr2 color){
			float y = color.Y;
			float cb = color.Cb - 128;
			float cr = color.Cr - 128;
			byte r = Clamp((byte) (y + 1.402*cr), 0, 255);
			byte g = Clamp((byte) (y - 0.34414*cb - 0.71414*cr), 0, 255);
			byte b = Clamp((byte) (y + 1.772*cb), 0, 255);
			return FromArgb(r, g, b);
		}

		public static byte Clamp(byte value, byte min, byte max){
			if (value > max){
				return max;
			}
			return value < min ? min : value;
		}

		public override string ToString(){
			StringBuilder sb = new StringBuilder(32);
			sb.Append(GetType().Name);
			sb.Append(" [");
			if ((state & stateNameValid) != 0){
				sb.Append(Name);
			} else if ((state & stateKnownColorValid) != 0){
				sb.Append(Name);
			} else if ((state & stateValueMask) != 0){
				sb.Append("A=");
				sb.Append(A);
				sb.Append(", R=");
				sb.Append(R);
				sb.Append(", G=");
				sb.Append(G);
				sb.Append(", B=");
				sb.Append(B);
			} else{
				sb.Append("empty");
			}
			sb.Append("]");
			return sb.ToString();
		}

		public static bool operator ==(Color2 left, Color2 right){
			return left.value == right.value && left.state == right.state && left.knownColor == right.knownColor;
		}

		public static bool operator !=(Color2 left, Color2 right){
			return !(left == right);
		}

		public override bool Equals(object obj){
			if (obj is Color2){
				Color2 right = (Color2) obj;
				if (value == right.value && state == right.state && knownColor == right.knownColor){
					return true;
				}
			}
			return false;
		}

		public override int GetHashCode(){
			return value.GetHashCode() ^ state.GetHashCode() ^ knownColor.GetHashCode();
		}
	}
}