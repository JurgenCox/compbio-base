namespace BaseLibS.Graph.Image.Formats.Jpg{
	internal class Block{
		public const int blockSize = 64;
		private readonly int[] data;
		public Block(){
			data = new int[blockSize];
		}
		public int this[int index]{
			get => data[index];
			set => data[index] = value;
		}
	}
}