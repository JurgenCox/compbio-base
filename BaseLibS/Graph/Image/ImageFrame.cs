namespace BaseLibS.Graph.Image{
	public class ImageFrame : ImageBase{
		public ImageFrame(){}
		public ImageFrame(ImageBase frame) : base(frame){}
		public override IPixelAccessor Lock(){
			return Bootstrapper.instance.GetPixelAccessor(this);
		}
	}
}