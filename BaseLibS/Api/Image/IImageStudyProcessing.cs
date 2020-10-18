using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageStudyProcessing : IImageProcessingAtom{
		void ProcessData(IStudy mdata, Parameters param);
	}
}