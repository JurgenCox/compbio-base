using System;
using BaseLibS.Api.Generic;

namespace BaseLibS.Api.Image{
	public interface IImageSubject : ISubject{

		int SessionCount { get; }
		IImageSession GetSessionAt(int index);
		int GetTotalAnatCount();
		int GetTotalFuncCount();
		int GetTotalDwiCount();
		int GetTotalParMapCount();
		void AddSession(string name);

	}
}