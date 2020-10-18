using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageMetaStudyProcessing : IImageProcessingAtom{
		void ProcessData(IStudy[] mdata, Parameters param);
	}
}