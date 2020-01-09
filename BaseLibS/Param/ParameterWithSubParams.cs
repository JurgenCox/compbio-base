using System;

namespace BaseLibS.Param{
	public interface IParameterWithSubParams{
		Parameters GetSubParameters();
		float ParamNameWidth { get; set; }
		float TotalWidth { get; set; }
	}

	[Serializable]
	public abstract class ParameterWithSubParams<T> : Parameter<T>, IParameterWithSubParams{
		protected ParameterWithSubParams(string name) : base(name){}
		public abstract Parameters GetSubParameters();
		public float ParamNameWidth { get; set; }
		public float TotalWidth { get; set; }
	}
}