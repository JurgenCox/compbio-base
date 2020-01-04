namespace BaseLibS.Graph.Base{
	public abstract class BasicTableLayoutStyle{
		public BasicSizeType SizeType { get; }
		public abstract float Size { get; set; }

		protected BasicTableLayoutStyle(BasicSizeType sizeType){
			SizeType = sizeType;
		}
	}
}