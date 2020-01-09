namespace BaseLibS.Graph{
	public struct Size2{
		public float Width { get; set; }
		public float Height { get; set; }
		public static readonly Size2 Empty = new Size2();

		public Size2(float width, float height){
			Width = width;
			Height = height;
		}

		public bool IsEmpty => Width == 0 && Height == 0;
		public override string ToString(){
			return "width=" + Width + " height=" + Height;
		}
	}
}