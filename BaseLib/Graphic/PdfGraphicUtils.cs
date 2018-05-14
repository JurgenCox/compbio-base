using System;
using System.IO;
using BaseLibS.Graph;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BaseLib.Graphic
{
    public static class PdfGraphicUtils
    {
        public static Font GetFont(Font2 font) {
            Font f = FontFactory.GetFont("c:/windows/fonts/arial.ttf", BaseFont.CP1252, BaseFont.EMBEDDED,
                font.Size * 0.667f, font.Bold ? 1 : 0);
            try {
                string file;
                switch (font.Name) {
                    case "Lucida Sans Unicode":
                        file = "c:/windows/fonts/L_10646.TTF";
                        break;
                    case "Arial Unicode MS":
                        file = "c:/windows/fonts/ARIALUNI.TTF";
                        break;
                    default:
                        file = $"c:/windows/fonts/{font.Name}.ttf";
                        break;
                }
                if (File.Exists(file)) {
                    f = FontFactory.GetFont(file, BaseFont.CP1252, BaseFont.EMBEDDED, font.Size * 0.667f, font.Bold ? 1 : 0);
                }
            } catch (Exception) {
                // do nothing
            }
            return f;
        }
    }
}