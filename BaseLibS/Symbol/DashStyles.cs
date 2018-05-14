using System;
using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public static class DashStyles{
		public static DashStyle2 DashStyleFromIndex(int index){
			switch (index){
				case 0:
					return DashStyle2.Solid;
				case 1:
					return DashStyle2.Dash;
				case 2:
					return DashStyle2.Dot;
				case 3:
					return DashStyle2.DashDot;
				case 4:
					return DashStyle2.DashDotDot;
				default:
					throw new ArgumentException();
			}
		}

		public static int DashStyleToIndex(DashStyle2 dashStyle){
			switch (dashStyle){
				case DashStyle2.Solid:
					return 0;
				case DashStyle2.Dash:
					return 1;
				case DashStyle2.Dot:
					return 2;
				case DashStyle2.DashDot:
					return 3;
				case DashStyle2.DashDotDot:
					return 4;
				default:
					throw new ArgumentException();
			}
		}
	}
}