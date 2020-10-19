using System;

namespace BaseLibS.Api.Generic{
	public interface ISubject : ICloneable{
		int AtomCount{ get; }
		string Name{ get; }

		IAtom GetAtomAt(int index);
	}
}