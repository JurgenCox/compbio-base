using System;

namespace BaseLibS.Num.NdArray.Slice{
	public abstract class ASlice{
		protected ASlice(){ }

		public virtual void Match(Action<Slice> sliceAction, Action everythingAction){
			everythingAction();
		}
	}
}