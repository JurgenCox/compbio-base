using BaseLibS.Api.Generic;
using BaseLibS.Param;

namespace BaseLibS.Api.Image{
	public interface IImageProcessingAtom : IActivityWithHeading{
		Parameters GetParameters();
	}
}