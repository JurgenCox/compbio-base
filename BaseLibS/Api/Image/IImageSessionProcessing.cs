using BaseLibS.Api.Generic;
using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageSessionProcessing : IImageProcessingItem { 
		void ProcessData(IImageStudy mdata, Parameters param);
	}
}
