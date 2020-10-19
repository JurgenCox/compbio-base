using System;
using BaseLibS.Api.Generic;

namespace BaseLibS.Api.Image{
	public interface IImageSubject : ISubject{
		int AnatCount{ get; }
		int FuncCount{ get; }
		int DwiCount{ get; }
		IImageSeries GetAnatAt(int index);
		IImageSeries GetFuncAt(int index);
		IImageSeries GetDwiAt(int index);
		IImageSeries GetAt(MriType type, int index);

	}
}