namespace BaseLibS.Graph{
	public struct PointI2{
		public static readonly PointI2 Empty = new PointI2();
		public int X { get; set; }
		public int Y { get; set; }

		public PointI2(int x, int y){
			X = x;
			Y = y;
		}
	}
}