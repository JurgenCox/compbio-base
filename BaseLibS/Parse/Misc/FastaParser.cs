using System.IO;
using System.Text;
using BaseLibS.Util;

namespace BaseLibS.Parse.Misc{
	public delegate bool HandleFastaEntry(string header, string sequence);
	public class FastaParser{
		private readonly string filename;
		private readonly HandleFastaEntry process;
		private readonly bool removeGreaterSign;

		public FastaParser(string filename, HandleFastaEntry process, bool removeGreaterSign){
			this.filename = filename;
			this.process = process;
			this.removeGreaterSign = removeGreaterSign;
		}

		public FastaParser(string filename, HandleFastaEntry process) : this(filename, process, true) {}

		public void Parse(){
			StreamReader reader = FileUtils.GetReader(filename);
			string line;
			string header = null;
			StringBuilder sequence = new StringBuilder();
			while ((line = reader.ReadLine()) != null){
				if (line.StartsWith("#")){
					continue;
				}
				if (line.StartsWith(">")){
					if (header != null){
						bool stop = process(header, sequence.ToString());
						if (stop){
							break;
						}
					}
					header = removeGreaterSign ? line.Substring(1) : line;
					sequence = new StringBuilder();
				} else{
					sequence.Append(StringUtils.RemoveWhitespace(line.Trim()));
				}
			}
			if (header != null){
				process(header, sequence.ToString());
			}
			reader.Close();
		}
	}
}