using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public class FormUtils{
		public static float GetDpiScale(Graphics g){
			try {
				return g.DpiX / 96f;
			} catch (Exception) {
				try {
					return GetDpiScale1();
				} catch (Exception) {
					return 1f;
				}
			}
		}

		private static float GetDpiScale1(){
			PropertyInfo dpiXProperty = typeof (SystemParameter).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
			if (dpiXProperty == null){
				return 1;
			}
			int dpiX = (int) dpiXProperty.GetValue(null, null);
			return dpiX/96f;
		}

		public static void SelectExact(ICollection<string> colNames, IList<string> colTypes, MultiListSelectorControl mls){
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