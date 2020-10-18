using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageSeriesProcessing : IImageProcessingAtom{
		void ProcessData(IImageSeries mdata, Parameters param);
	}
}