using System;

namespace BaseLibS.Num.NdArray.Slice{
	public class Slice : ASlice{
		public int Start;
		public int Stride;
		public int End;

		public Slice(int start, int end, int stride = 1){
			Start = start;
			Stride = stride;
			End = end;
		}

		public override void Match(Action<Slice> sliceAction, Action everythingAction){
			sliceAction(this);
		}
	}
}