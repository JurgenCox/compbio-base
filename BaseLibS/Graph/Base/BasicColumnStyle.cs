namespace BaseLibS.Graph.Base{
	public class BasicColumnStyle : BasicTableLayoutStyle{
		public float Width { get; set; }

		public BasicColumnStyle(BasicSizeType sizeType, float width) : base(sizeType){
			Width = width;
		}

		public override float Size { get => Width;
			set => Width = value;
		}
	}
}