using BaseLibS.Api.Generic;
using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageStudyProcessing : IImageProcessingItem, IStudyProcessing{
		void ProcessData(IImageStudy mdata, Parameters param);
	}
}