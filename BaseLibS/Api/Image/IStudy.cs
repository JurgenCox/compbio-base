using System.Collections.Generic;

namespace BaseLibS.Api.Image{
	public interface IStudy : IEnumerable<ISubject>{
		ISubject this[int i]{ get; }
		void Write(int i);
	}
}