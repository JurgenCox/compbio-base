using System.IO;
using System.Text;

namespace BaseLibS.Util{
	public class StreamBackwardReader{
		public StreamBackwardReader(string filename){
			stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			stream.Seek(0, SeekOrigin.End);
		}

		private readonly FileStream stream;

		public string ReadLine(){
			StringBuilder str = new StringBuilder();
			int ch;
			do{
				if (stream.Position == 0){
					return null;
				}
				stream.Seek(-1, SeekOrigin.Current);
				ch = stream.ReadByte();
				stream.Seek(-1, SeekOrigin.Current);
				if (ch != '\n'){
					str.Insert(0, (char) ch);
				}
			} while (ch != '\n');
			return str.ToString();
		}

		public void Close(){
			stream.Close();
		}
	}
}