using System;
using BaseLibS.Api.Generic;

namespace BaseLibS.Api.Image{
	public interface IImageSession : ICloneable{
		string Name { get; }
		int AnatCount { get; }
		int FuncCount { get; }
		int DwiCount { get; }
		IImageSeries GetAnatAt(int index);
		IImageSeries GetFuncAt(int index);
		IImageSeries GetDwiAt(int index);
		IImageSeries GetAt(MriType type, int index);
		void AddAnat(IImageSeries anat);
		void AddFunc(IImageSeries func);
		void AddDwi(IImageSeries dwi);
	}
}