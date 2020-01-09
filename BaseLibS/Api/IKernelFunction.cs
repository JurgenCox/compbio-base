using System;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api{
	public interface IKernelFunction : ICloneable, INamedListItem{
		bool UsesSquares { get; }
		Parameters Parameters { get; set; }
		double Evaluate(BaseVector xi, BaseVector xj, double xSquarei, double xSquarej);
	}
}