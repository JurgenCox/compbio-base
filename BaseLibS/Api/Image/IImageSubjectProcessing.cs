using BaseLibS.Api.Generic;
using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageSubjectProcessing : IImageProcessingItem, ISubjectProcessing{
		void ProcessData(IImageSubject mdata, Parameters param);
	}
}