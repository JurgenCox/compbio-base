using System;

namespace BaseLibS.Api.Image{
	public interface IImageSubject : ICloneable{
		string Name{ get; }
		int SessionCount{ get; }
		IImageSession GetSessionAt(int index);
		int GetTotalAnatCount();
		int GetTotalFuncCount();
		int GetTotalDwiCount();
		int GetTotalParMapCount();
		void AddSession(string name);
	}
}