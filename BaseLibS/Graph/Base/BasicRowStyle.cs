namespace BaseLibS.Graph.Base{
	public class BasicRowStyle : BasicTableLayoutStyle{
		public float Height { get; set; }

		public BasicRowStyle(BasicSizeType sizeType, float height) : base(sizeType){
			Height = height;
		}

		public override float Size { get => Height;
			set => Height = value;
		}
	}
}