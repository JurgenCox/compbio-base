namespace BaseLibS.Graph{
	public struct SizeI2{
		public int Width { get; set; }
		public int Height { get; set; }
		public static readonly SizeI2 Empty = new SizeI2();

		public SizeI2(int width, int height){
			Width = width;
			Height = height;
		}

		public bool IsEmpty => Width == 0 && Height == 0;
	}
}