using System;
using BaseLibS.Api.Generic;

namespace BaseLibS.Api.Image{
	public interface IImageSubject : ISubject{

		int SessionCount { get; }
		IImageSession GetSessionAt(int index);
		int GetTotalAnatCount();
		int GetTotalFuncCount();
		int GetTotalDwiCount();
		void AddSession(string name);

		//int TotalAnatCount{ get; }
		//int TotalFuncCount{ get; }
		//int TotalDwiCount{ get; }
		//IImageSeries GetAnatAt(int index);
		//IImageSeries GetFuncAt(int index);
		//IImageSeries GetDwiAt(int index);
		//IImageSeries GetAt(MriType type, int index);

	}
}