using BaseLibS.Graph;

namespace BaseLibS.Symbol{
	public abstract class SymbolType{
		public static SymbolType cross = new SymbolTypeCross(0);
		public static SymbolType square = new SymbolTypeSquare(1);
		public static SymbolType circle = new SymbolTypeCircle(2);
		public static SymbolType triangle = new SymbolTypeTriangle(3);
		public static SymbolType diamond = new SymbolTypeDiamond(4);
		public static SymbolType star = new SymbolTypeStar(5);
		public static SymbolType diagonalCross = new SymbolTypeDiagonalCross(6);
		public static SymbolType filledSquare = new SymbolTypeFilledSquare(7);
		public static SymbolType filledTriangle = new SymbolTypeFilledTriangle(8);
		public static SymbolType filledCircle = new SymbolTypeFilledCircle(9);
		public static SymbolType filledDiamond = new SymbolTypeFilledDiamond(10);
		public static SymbolType horizontalDash = new SymbolTypeHorizontalDash(11);
		public static SymbolType[] allSymbols = {
			cross, square, circle, triangle, diamond, star, diagonalCross, filledSquare, filledTriangle, filledCircle,
			filledDiamond, horizontalDash
		};
		public static string[] allNames = InitNames();

		private static string[] InitNames(){
			string[] result = new string[allSymbols.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = allSymbols[i].Name;
			}
			return result;
		}

		protected SymbolType(int index){
			this.Index = index;
		}

		public int Index { get; }
		public abstract string Name { get; }
		public abstract void GetPath(int size, out int[] pathX, out int[] pathY);
		public abstract void Draw(int size, float x, float y, IGraphics g, Pen2 pen, Brush2 brush);
	}
}