using System.Collections.Generic;
using BaseLibS.Api.Generic;

namespace BaseLibS.Api.Image{
	public interface IImageStudy : IStudy, IEnumerable<IImageSubject>{
		IImageSubject this[int i]{ get; }
	}
}