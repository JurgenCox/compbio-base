namespace BaseLibS.Api.Generic{
	public interface IStudy{
		int SubjectCount{ get; }
		ISubject GetSubjectAt(int index);
		string Name { get; }
	}
}