using System;

namespace BaseLibS.Param{
	public interface IParameterWithSubParams{
		Parameters GetSubParameters();
		float ParamNameWidth{ get; set; }
		float TotalWidth{ get; set; }
	}

	[Serializable]
	public abstract class ParameterWithSubParams<T> : Parameter<T>, IParameterWithSubParams{
		protected ParameterWithSubParams(string name) : base(name){ }

		protected ParameterWithSubParams(string name, string help, string url, bool visible, T value, T default1,
			float paramNameWidth, float totalWidth) : base(name, help, url, visible, value, default1){
			ParamNameWidth = paramNameWidth;
			TotalWidth = totalWidth;
		}

		public abstract Parameters GetSubParameters();
		public float ParamNameWidth{ get; set; }
		public float TotalWidth{ get; set; }
	}
}