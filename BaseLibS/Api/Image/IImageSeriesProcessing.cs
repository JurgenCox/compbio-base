using BaseLibS.Api.Generic;
using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageSeriesProcessing : IImageProcessingItem, IAtomProcessing{
		void ProcessData(IImageSeries mdata, Parameters param);
	}
}