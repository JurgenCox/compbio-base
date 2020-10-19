using BaseLibS.Api.Generic;
using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageMetaStudyProcessing : IImageProcessingItem, IMetaStudyProcessing{
		void ProcessData(IImageStudy[] mdata, Parameters param);
	}
}