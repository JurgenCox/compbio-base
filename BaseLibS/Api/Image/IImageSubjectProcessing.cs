using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageSubjectProcessing : IImageProcessingAtom{
		void ProcessData(ISubject mdata, Parameters param);
	}
}