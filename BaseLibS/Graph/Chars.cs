using System;
using System.Collections.Generic;

namespace BaseLibS.Graph {
	public static class Chars {
		public static Bitmap2 GetImage(char c, int width, int height, Color2 fgColor, Color2 bgColor) {
			if (height <= 1 || width <= 1) {
				return null;
			}
			Bitmap2 template = GetImage(c);
			if (template == null) {
				return null;
			}
			int twidth = template.Width;
			int theight = template.Height;
			Bitmap2 result = new Bitmap2(width, height);
			for (int i = 0; i < width; i++) {
				float wI1;
				float wI2;
				int indI = CalcInd(i, width, twidth, out wI1, out wI2);
				for (int j = 0; j < height; j++) {
					float wJ1;
					float wJ2;
					int indJ = CalcInd(j, height, theight, out wJ1, out wJ2);
					Color2 ij = IsSet(indI, indJ, template) ? fgColor : bgColor;
					Color2 ipj = IsSet(indI + 1, indJ, template) ? fgColor : bgColor;
					Color2 ijp = IsSet(indI, indJ + 1, template) ? fgColor : bgColor;
					Color2 ipjp = IsSet(indI + 1, indJ + 1, template) ? fgColor : bgColor;
					Color2 col = Average(new[] { ij, ipj, ijp, ipjp }, new[] { wI1 * wJ1, wI2 * wJ1, wI1 * wJ2, wI2 * wJ2 });
					result.SetPixel(i, j, col);
				}
			}
			return result;
		}

		private static Color2 Average(IList<Color2> colors, IList<float> weights) {
			float r = 0;
			float g = 0;
			float b = 0;
			float norm = 0;
			for (int i = 0; i < colors.Count; i++) {
				Color2 col = colors[i];
				float w = weights[i];
				r += col.R * w;
				g += col.G * w;
				b += col.B * w;
				norm += w;
			}
			Color2 x = Color2.FromArgb(255, (int)Math.Round(r / norm), (int)Math.Round(g / norm), (int)Math.Round(b / norm));
			return x;
		}

		private static bool IsSet(int i, int j, Bitmap2 b) {
			return Color2.GetR(b.GetPixel(i, j)) == 0;
		}

		private static int CalcInd(int i, int width, int twidth, out float wI1, out float wI2) {
			double dind = i / (width - 1.0) * (twidth - 1.0);
			int ind = (int)Math.Round(dind);
			if (dind < 0.0001) {
				wI1 = 1;
				wI2 = 0;
				return 0;
			}
			if (dind > twidth - 1.0001) {
				wI1 = 0;
				wI2 = 1;
				return twidth - 2;
			}
			if (ind == 0) {
				wI1 = (float)(1 - dind);
				wI2 = (float)dind;
				return 0;
			}
			if (ind == twidth - 1) {
				wI1 = (float)(twidth - 1 - dind);
				wI2 = (float)(2 - twidth + dind);
				return twidth - 2;
			}
			int ind1 = (int)dind;
			wI1 = (float)(1.0 - dind + ind1);
			wI2 = (float)(dind - ind1);
			return ind1;
		}

		private static Bitmap2 GetImage(char c) {
			switch (c) {
				case 'A':
					return Bitmap2.GetImage("chara.bmp");
				case 'B':
					return Bitmap2.GetImage("charb.bmp");
				case 'C':
					return Bitmap2.GetImage("charc.bmp");
				case 'D':
					return Bitmap2.GetImage("chard.bmp");
				case 'E':
					return Bitmap2.GetImage("chare.bmp");
				case 'F':
					return Bitmap2.GetImage("charf.bmp");
				case 'G':
					return Bitmap2.GetImage("charg.bmp");
				case 'H':
					return Bitmap2.GetImage("charh.bmp");
				case 'I':
					return Bitmap2.GetImage("chari.bmp");
				case 'J':
					return Bitmap2.GetImage("charj.bmp");
				case 'K':
					return Bitmap2.GetImage("chark.bmp");
				case 'L':
					return Bitmap2.GetImage("charl.bmp");
				case 'M':
					return Bitmap2.GetImage("charm.bmp");
				case 'N':
					return Bitmap2.GetImage("charn.bmp");
				case 'O':
					return Bitmap2.GetImage("charo.bmp");
				case 'P':
					return Bitmap2.GetImage("charp.bmp");
				case 'Q':
					return Bitmap2.GetImage("charq.bmp");
				case 'R':
					return Bitmap2.GetImage("charr.bmp");
				case 'S':
					return Bitmap2.GetImage("chars.bmp");
				case 'T':
					return Bitmap2.GetImage("chart.bmp");
				case 'U':
					return Bitmap2.GetImage("charu.bmp");
				case 'V':
					return Bitmap2.GetImage("charv.bmp");
				case 'W':
					return Bitmap2.GetImage("charw.bmp");
				case 'X':
					return Bitmap2.GetImage("charx.bmp");
				case 'Y':
					return Bitmap2.GetImage("chary.bmp");
				case 'Z':
					return Bitmap2.GetImage("charz.bmp");
				case 'a':
					return Bitmap2.GetImage("char_a.bmp");
				case 'b':
					return Bitmap2.GetImage("char_b.bmp");
				case 'c':
					return Bitmap2.GetImage("char_c.bmp");
				case 'd':
					return Bitmap2.GetImage("char_d.bmp");
				case 'e':
					return Bitmap2.GetImage("char_e.bmp");
				case 'f':
					return Bitmap2.GetImage("char_f.bmp");
				case 'g':
					return Bitmap2.GetImage("char_g.bmp");
				case 'h':
					return Bitmap2.GetImage("char_h.bmp");
				case 'i':
					return Bitmap2.GetImage("char_i.bmp");
				case 'j':
					return Bitmap2.GetImage("char_j.bmp");
				case 'k':
					return Bitmap2.GetImage("char_k.bmp");
				case 'l':
					return Bitmap2.GetImage("char_l.bmp");
				case 'm':
					return Bitmap2.GetImage("char_m.bmp");
				case 'n':
					return Bitmap2.GetImage("char_n.bmp");
				case 'o':
					return Bitmap2.GetImage("char_o.bmp");
				case 'p':
					return Bitmap2.GetImage("char_p.bmp");
				case 'q':
					return Bitmap2.GetImage("char_q.bmp");
				case 'r':
					return Bitmap2.GetImage("char_r.bmp");
				case 's':
					return Bitmap2.GetImage("char_s.bmp");
				case 't':
					return Bitmap2.GetImage("char_t.bmp");
				case 'u':
					return Bitmap2.GetImage("char_u.bmp");
				case 'v':
					return Bitmap2.GetImage("char_v.bmp");
				case 'w':
					return Bitmap2.GetImage("char_w.bmp");
				case 'x':
					return Bitmap2.GetImage("char_x.bmp");
				case 'y':
					return Bitmap2.GetImage("char_y.bmp");
				case 'z':
					return Bitmap2.GetImage("char_z.bmp");
			}
			return null;
		}
	}
}
