using System;

namespace BaseLibS.Api.Generic{
	public interface IAtom : ICloneable{
		string Name{ get; }
	}
}