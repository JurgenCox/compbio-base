using System;
using System.Drawing;

namespace BaseLib.Forms.Base{
	public static class FormUtil{
		private static readonly Color[] predefinedColors ={
			Color.Blue, Color.FromArgb(255, 144, 144),
			Color.FromArgb(255, 0, 255), Color.FromArgb(168, 156, 82), Color.LightBlue, Color.Orange, Color.Cyan, Color.Pink,
			Color.Turquoise, Color.LightGreen, Color.Brown, Color.DarkGoldenrod, Color.DeepPink, Color.LightSkyBlue,
			Color.BlueViolet, Color.Crimson
		};

		public static Color GetPredefinedColor(int index){
			return predefinedColors[Math.Abs(index%predefinedColors.Length)];
		}
	}
}