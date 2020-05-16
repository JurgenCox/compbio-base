using System;
using System.Collections.Generic;
using System.Drawing;
using BaseLibS.Util;

namespace BaseLib.Forms.Base{
	public static class FormUtil{
		private static readonly Color[] predefinedColors = {
			Color.Blue, Color.FromArgb(255, 144, 144), Color.FromArgb(255, 0, 255), Color.FromArgb(168, 156, 82),
			Color.LightBlue, Color.Orange, Color.Cyan, Color.Pink, Color.Turquoise, Color.LightGreen, Color.Brown,
			Color.DarkGoldenrod, Color.DeepPink, Color.LightSkyBlue, Color.BlueViolet, Color.Crimson
		};

		public static Color GetPredefinedColor(int index){
			return predefinedColors[Math.Abs(index % predefinedColors.Length)];
		}

		public static void SelectExact(ICollection<string> colNames, IList<string> colTypes,
			MultiListSelectorControl mls){
			for (int i = 0; i < colNames.Count; i++){
				switch (colTypes[i]){
					case "E":
						mls.SetSelected(0, i, true);
						break;
					case "N":
						mls.SetSelected(1, i, true);
						break;
					case "C":
						mls.SetSelected(2, i, true);
						break;
					case "T":
						mls.SetSelected(3, i, true);
						break;
					case "M":
						mls.SetSelected(4, i, true);
						break;
				}
			}
		}

		public static void SelectHeuristic(IList<string> colNames, MultiListSelectorControl mls){
			char guessedType = GuessSilacType(colNames);
			for (int i = 0; i < colNames.Count; i++){
				if (StringUtils.categoricalColDefaultNames.Contains(colNames[i].ToLower())){
					mls.SetSelected(2, i, true);
					continue;
				}
				if (StringUtils.textualColDefaultNames.Contains(colNames[i].ToLower())){
					mls.SetSelected(3, i, true);
					continue;
				}
				if (StringUtils.numericColDefaultNames.Contains(colNames[i].ToLower())){
					mls.SetSelected(1, i, true);
					continue;
				}
				if (StringUtils.multiNumericColDefaultNames.Contains(colNames[i].ToLower())){
					mls.SetSelected(4, i, true);
					continue;
				}
				switch (guessedType){
					case 's':
						if (colNames[i].StartsWith("Norm. Intensity")){
							mls.SetSelected(0, i, true);
						}
						break;
					case 'd':
						if (colNames[i].StartsWith("Ratio H/L Normalized ")){
							mls.SetSelected(0, i, true);
						}
						break;
				}
			}
		}

		public static char GuessSilacType(IEnumerable<string> colnames){
			bool isSilac = false;
			foreach (string s in colnames){
				if (s.StartsWith("Ratio M/L")){
					return 't';
				}
				if (s.StartsWith("Ratio H/L")){
					isSilac = true;
				}
			}
			return isSilac ? 'd' : 's';
		}
	}
}