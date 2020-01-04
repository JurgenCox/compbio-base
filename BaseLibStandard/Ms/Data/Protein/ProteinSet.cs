using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BaseLibS.Mol;
using BaseLibS.Ms.Decoy;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Ms.Data.Protein{
	public class ProteinSet : IDisposable{
		private readonly object longIndexReaderSync = new object();
		private readonly object proteinDataReaderSync = new object();
		private long tmpCount;
		public long Count{ get; private set; }
		private int blockSize;
		private int endBlockSize;
		private long[] filePosShort;
		private string[] namesShort;
		private List<string> tempFileNames = new List<string>();
		private List<int> lengths2 = new List<int>();
		private List<bool> decoy2 = new List<bool>();
		private List<string> names2 = new List<string>();
		private List<long> filePos2 = new List<long>();
		private List<string> duplicates = new List<string>();
		private readonly Dictionary<int, int> lengthDist = new Dictionary<int, int>();

		// paths and readers
		public readonly string folderName;

		private string ProteinDataPath => Path.Combine(folderName, "sequences");
		public BinaryReader ProteinDataReader{ get; private set; } // set in constructor to read ProteinDataPath
		private string LongIndexPath => Path.Combine(folderName, "sequences.ind");
		private BinaryReader longIndexReader; // set in constructor to read LongIndexPath
		private string ShortIndexPath => Path.Combine(folderName, "sequences.inds");
		private string StatisticsPath => Path.Combine(folderName, "sequences.stat");
		private string DuplicatesPath => Path.Combine(folderName, "duplicates");
		private string TempIndexPath => Path.Combine(folderName, "tmpProtInd" + tempFileNames.Count);

		public ProteinSet(string folderName){
			this.folderName = folderName;
			BinaryReader reader = FileUtils.GetBinaryReader(ShortIndexPath);
			Count = reader.ReadInt64();
			blockSize = reader.ReadInt32();
			endBlockSize = reader.ReadInt32();
			int c = reader.ReadInt32();
			filePosShort = new long[c];
			namesShort = new string[c];
			for (int i = 0; i < c; i++){
				filePosShort[i] = reader.ReadInt64();
				namesShort[i] = reader.ReadString();
			}
			reader.Close();
			ProteinDataReader = FileUtils.GetBinaryReader(ProteinDataPath);
			longIndexReader = FileUtils.GetBinaryReader(LongIndexPath);
		}

		public ProteinSet(string folderName, IEnumerable<FastaFileInfo> fastaFiles,
			IEnumerable<FastaFileInfo> fastaFilesPg, DecoyStrategy decoyStrategy, bool includeContaminants){
			this.folderName = folderName;
			BinaryWriter proteinWriter = FileUtils.GetBinaryWriter(ProteinDataPath);
			foreach (FastaFileInfo fastaFile in fastaFiles){
				AddFastaFile(fastaFile, proteinWriter, false, decoyStrategy, false);
			}
			foreach (FastaFileInfo fastaFile in fastaFilesPg){
				AddFastaFile(fastaFile, proteinWriter, false, decoyStrategy, true);
			}
			if (includeContaminants){
				AddFastaFile(
					new FastaFileInfo(FileUtils.GetContaminantFilePath(), FileUtils.GetContaminantParseRule(), "", "",
						"", "", "-1"), proteinWriter, true, decoyStrategy, false);
			}
			proteinWriter.Close();
			Finish();
			ProteinDataReader = FileUtils.GetBinaryReader(ProteinDataPath);
			longIndexReader = FileUtils.GetBinaryReader(LongIndexPath);
		}

		public static Regex GetRegex(string s){
			Regex regex = null;
			if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s)){
				regex = new Regex(s);
			}
			return regex;
		}

		private void AddFastaFile(FastaFileInfo fastaFile, BinaryWriter proteinWriter, bool contaminant,
			DecoyStrategy decoyStrategy, bool proteogenomic){
			bool isCodons = CheckIfCodons(fastaFile.fastaFilePath);
			string fasta = Path.GetFileNameWithoutExtension(fastaFile.fastaFilePath);
			Regex identifierRegex = GetRegex(fastaFile.identifierParseRule);
			Regex descriptionRegex = GetRegex(fastaFile.descriptionParseRule);
			Regex taxonomyRegex = GetRegex(fastaFile.taxonomyParseRule);
			Regex variationRegex = GetRegex(fastaFile.variationParseRule);
			Regex modificationRegex = GetRegex(fastaFile.modificationParseRule);
			StreamReader reader = new StreamReader(fastaFile.fastaFilePath);
			string line;
			string header = null;
			StringBuilder sequence = null;
			while ((line = reader.ReadLine()) != null){
				if (line.Length == 0 || line.StartsWith("#")){
					continue;
				}
				if (line.StartsWith(">")){
					if (header != null){
						string name = identifierRegex.Match(header).Groups[1].ToString();
						name = StringUtils.RemoveWhitespace(name);
						string description = null;
						if (descriptionRegex != null){
							description = descriptionRegex.Match(header).Groups[1].ToString();
						}
						if (taxonomyRegex != null){
							fastaFile.taxonomyId = taxonomyRegex.Match(header).Groups[1].ToString();
						}
						string variation = null;
						if (variationRegex != null){
							variation = variationRegex.Match(header).Groups[1].ToString();
						}
						string modification = null;
						if (modificationRegex != null){
							modification = modificationRegex.Match(header).Groups[1].ToString();
						}
						string seq = sequence.ToString().Replace(" ", "");
						if (seq.EndsWith("*")){
							seq = seq.Substring(0, seq.Length - 1);
						}
						if (contaminant){
							name = GlobalConstants.conPrefix + name;
						}
						Add(name, description, variation, modification, seq, proteinWriter, false, contaminant, fasta,
							fastaFile.taxonomyId, isCodons, proteogenomic);
						if (decoyStrategy != null){
							string revSeq = decoyStrategy.ProcessProtein(seq, isCodons);
							string revVar = decoyStrategy.ProcessVariation(variation, seq, isCodons);
							//TODO
							const string revMod = "";
							string revName = GlobalConstants.revPrefix + name;
							Add(revName, description, revVar, revMod, revSeq, proteinWriter, true, contaminant, fasta,
								fastaFile.taxonomyId, isCodons, proteogenomic);
						}
					}
					header = line.Trim();
					sequence = new StringBuilder();
				} else{
					sequence.Append(line.Trim().ToUpper());
				}
			}
			if (header != null){
				string name = identifierRegex.Match(header).Groups[1].ToString();
				name = StringUtils.RemoveWhitespace(name);
				string description = null;
				if (descriptionRegex != null){
					description = descriptionRegex.Match(header).Groups[1].ToString();
				}
				if (taxonomyRegex != null){
					fastaFile.taxonomyId = taxonomyRegex.Match(header).Groups[1].ToString();
				}
				string variation = null;
				if (variationRegex != null){
					variation = variationRegex.Match(header).Groups[1].ToString();
				}
				string modification = null;
				if (modificationRegex != null){
					modification = modificationRegex.Match(header).Groups[1].ToString();
				}
				string seq = sequence.ToString().Replace(" ", "");
				if (seq.EndsWith("*")){
					seq = seq.Substring(0, seq.Length - 1);
				}
				if (contaminant){
					name = GlobalConstants.conPrefix + name;
				}
				Add(name, description, variation, modification, seq, proteinWriter, false, contaminant, fasta,
					fastaFile.taxonomyId, isCodons, proteogenomic);
				if (decoyStrategy != null){
					string revSeq = decoyStrategy.ProcessProtein(seq, isCodons);
					string revName = GlobalConstants.revPrefix + name;
					string revVar = decoyStrategy.ProcessVariation(variation, seq, isCodons);
					//TODO
					const string revMod = "";
					Add(revName, description, revVar, revMod, revSeq, proteinWriter, true, contaminant, fasta,
						fastaFile.taxonomyId, isCodons, proteogenomic);
				}
			}
			reader.Close();
		}

		internal static bool CheckIfCodons(string fastaFileName){
			StreamReader reader = new StreamReader(fastaFileName);
			string line;
			StringBuilder sequence = new StringBuilder();
			while ((line = reader.ReadLine()) != null){
				if (line.Length == 0 || line.StartsWith("#")){
					continue;
				}
				if (!line.StartsWith(">")){
					sequence.Append(line.Trim().ToUpper());
					if (sequence.Length > 1000){
						break;
					}
				}
			}
			reader.Close();
			HashSet<char> bases = new HashSet<char>(new[]{'A', 'C', 'G', 'T', 'U', 'N'});
			string seq = sequence.ToString().ToUpper();
			foreach (char c in seq){
				if (!bases.Contains(c)){
					return false;
				}
			}
			return true;
		}

		//TODO
		private void Add(string name, string description, string variation, string modification, string sequence,
			BinaryWriter proteinWriter, bool decoy, bool contaminant, string fasta, string taxonomyId, bool isCodons,
			bool proteogenomic){
			if (filePos2.Count > 1000000){
				DumpTempFile();
			}
			Protein p = new Protein(sequence, name, description, variation, decoy, contaminant, fasta, taxonomyId,
				isCodons, proteogenomic);
			filePos2.Add(proteinWriter.BaseStream.Position);
			p.Write(proteinWriter);
			names2.Add(p.Accession);
			int len = sequence.Length;
			lengths2.Add(len);
			if (!lengthDist.ContainsKey(len)){
				lengthDist.Add(len, 0);
			}
			lengthDist[len]++;
			decoy2.Add(p.Decoy);
		}

		public void DeleteFiles(){
			if (File.Exists(ProteinDataPath)){
				File.Delete(ProteinDataPath);
			}
			if (File.Exists(LongIndexPath)){
				File.Delete(LongIndexPath);
			}
		}

		private void DumpTempFile(){
			string filename = TempIndexPath;
			tempFileNames.Add(filename);
			BinaryWriter writer = FileUtils.GetBinaryWriter(filename);
			string[] names = names2.ToArray();
			int[] o = ArrayUtils.Order(names);
			names = ArrayUtils.SubArray(names, o);
			for (int i = 0; i < o.Length; i++){
				if (i == 0 || !names[i].Equals(names[i - 1])){
					writer.Write(names[i]);
					writer.Write(lengths2[o[i]]);
					writer.Write(decoy2[o[i]]);
					writer.Write(filePos2[o[i]]);
					tmpCount++;
				} else{
					duplicates.Add(names[i]);
				}
			}
			writer.Close();
			names2.Clear();
			lengths2.Clear();
			filePos2.Clear();
			GC.Collect();
		}

		public void WriteStatistics(){
			if (StatisticsPath == null){
				return;
			}
			StreamWriter writer = new StreamWriter(StatisticsPath);
			writer.WriteLine("Number of proteins\t" + Count);
			int[] lens = lengthDist.Keys.ToArray();
			Array.Sort(lens);
			foreach (int len in lens){
				writer.WriteLine("Length dist(" + len + ")\t" + lengthDist[len]);
			}
			writer.Close();
		}

		public void WriteShortIndex(){
			if (ShortIndexPath == null){
				return;
			}
			BinaryWriter writer = FileUtils.GetBinaryWriter(ShortIndexPath);
			writer.Write(Count);
			writer.Write(blockSize);
			writer.Write(endBlockSize);
			writer.Write(filePosShort.Length);
			for (int i = 0; i < filePosShort.Length; i++){
				writer.Write(filePosShort[i]);
				writer.Write(namesShort[i]);
			}
			writer.Close();
		}

		public void WriteDuplicates(){
			if (DuplicatesPath == null){
				return;
			}
			StreamWriter writer = new StreamWriter(DuplicatesPath);
			foreach (string s in duplicates){
				writer.WriteLine(s);
			}
			writer.Close();
		}

		public Protein Get(string name){
			try{
				return Get(GetIndex(name));
			} catch (Exception){
				if (name.StartsWith("REV_")){
					return Get(GetIndex(name.Substring(4)));
				}
				throw;
			}
		}

		public bool GetIsDecoy(string name){
			return GetIsDecoy(GetIndex(name));
		}

		public int GetLength(string name){
			return GetLength(GetIndex(name));
		}

		public Protein Get(int index){
			lock (proteinDataReaderSync){
				ProteinDataReader.BaseStream.Seek(GetFilePos(index), SeekOrigin.Begin);
				return new Protein(ProteinDataReader);
			}
		}

		private long GetFilePos(int index){
			lock (longIndexReaderSync){
				int block = index / blockSize;
				int off = index % blockSize;
				longIndexReader.BaseStream.Seek(filePosShort[block] + 8 * off, SeekOrigin.Begin);
				return longIndexReader.ReadInt64();
			}
		}

		public int GetLength(int index){
			lock (longIndexReaderSync){
				int block = index / blockSize;
				int off = index % blockSize;
				int bs = block == filePosShort.Length - 1 ? endBlockSize : blockSize;
				longIndexReader.BaseStream.Seek(filePosShort[block] + 8 * bs + 4 * off, SeekOrigin.Begin);
				return longIndexReader.ReadInt32();
			}
		}

		public bool GetIsDecoy(int index){
			lock (longIndexReaderSync){
				int block = index / blockSize;
				int off = index % blockSize;
				int bs = block == filePosShort.Length - 1 ? endBlockSize : blockSize;
				longIndexReader.BaseStream.Seek(filePosShort[block] + 12 * bs + off, SeekOrigin.Begin);
				return longIndexReader.ReadBoolean();
			}
		}

		public string GetName(int index){
			lock (longIndexReaderSync){
				if (blockSize == 1){
					return namesShort[index];
				}
				int block = index / blockSize;
				int off = index % blockSize;
				int bs = block == filePosShort.Length - 1 ? endBlockSize : blockSize;
				longIndexReader.BaseStream.Seek(filePosShort[block] + 13 * bs, SeekOrigin.Begin);
				for (int i = 0; i < off; i++){
					longIndexReader.ReadString();
				}
				return longIndexReader.ReadString();
			}
		}

		public int GetIndex(string name){
			lock (longIndexReaderSync){
				if (blockSize == 1){
					int a1 = Array.BinarySearch(namesShort, name);
					if (a1 < 0){
						throw new Exception("Search index cannot be negative for blockSize = 1.");
					}
					return a1;
				}
				int a = Array.BinarySearch(namesShort, name);
				if (a < 0){
					a = -2 - a;
				}
				int bs = a == filePosShort.Length - 1 ? endBlockSize : blockSize;
				longIndexReader.BaseStream.Seek(filePosShort[a] + 13 * bs, SeekOrigin.Begin);
				for (int i = 0; i < bs; i++){
					string s = longIndexReader.ReadString();
					if (s.Equals(name)){
						return a * blockSize + i;
					}
				}
				throw new Exception("Protein " + name + " does not exist in the fasta file.");
			}
		}

		private void Finish(){
			if (filePos2.Count > 0){
				DumpTempFile();
			}
			blockSize = (int) (tmpCount / 1000000) + 1;
			filePos2 = null;
			names2 = null;
			lengths2 = null;
			decoy2 = null;
			int n = tempFileNames.Count;
			BinaryReader[] readers = new BinaryReader[n];
			long[] nextFilePos = new long[n];
			string[] nextNames = new string[n];
			int[] nextLengths = new int[n];
			bool[] nextDecoys = new bool[n];
			BinaryWriter longIndexWriter = FileUtils.GetBinaryWriter(LongIndexPath);
			for (int i = 0; i < n; i++){
				readers[i] = FileUtils.GetBinaryReader(tempFileNames[i]);
				Read(readers[i], out nextFilePos[i], out nextLengths[i], out nextDecoys[i], out nextNames[i]);
			}
			List<long> blockFilePos = new List<long>();
			List<int> blockLengths = new List<int>();
			List<bool> blockDecoys = new List<bool>();
			List<string> blockNames = new List<string>();
			List<long> filePosShort2 = new List<long>();
			List<string> namesShort2 = new List<string>();
			for (;;){
				int smallestInd = GetSmallestInd(nextNames);
				for (int i = 0; i < n; i++){
					if (i == smallestInd){
						continue;
					}
					if (nextNames[i] == null){
						continue;
					}
					if (nextNames[i].Equals(nextNames[smallestInd])){
						Read(readers[i], out nextFilePos[i], out nextLengths[i], out nextDecoys[i], out nextNames[i]);
						duplicates.Add(nextNames[i]);
					}
				}
				blockFilePos.Add(nextFilePos[smallestInd]);
				blockLengths.Add(nextLengths[smallestInd]);
				blockDecoys.Add(nextDecoys[smallestInd]);
				blockNames.Add(nextNames[smallestInd]);
				if (blockFilePos.Count == blockSize){
					WriteLongIndexBlock(blockFilePos, blockLengths, blockDecoys, blockNames, longIndexWriter,
						filePosShort2, namesShort2);
				}
				Read(readers[smallestInd], out nextFilePos[smallestInd], out nextLengths[smallestInd],
					out nextDecoys[smallestInd], out nextNames[smallestInd]);
				bool finished = true;
				for (int i = 0; i < n; i++){
					if (nextNames[i] != null){
						finished = false;
						break;
					}
				}
				if (finished){
					break;
				}
			}
			if (blockFilePos.Count > 0){
				endBlockSize = blockFilePos.Count;
				WriteLongIndexBlock(blockFilePos, blockLengths, blockDecoys, blockNames, longIndexWriter, filePosShort2,
					namesShort2);
			} else{
				endBlockSize = blockSize;
			}
			filePosShort = filePosShort2.ToArray();
			namesShort = namesShort2.ToArray();
			longIndexWriter.Close();
			foreach (BinaryReader t in readers){
				t.Close();
			}
			foreach (string t in tempFileNames){
				File.Delete(t);
			}
			tempFileNames = null;
		}

		internal static int GetSmallestInd(IList<string> strings){
			int ind = -1;
			string smallest = null;
			for (int i = 0; i < strings.Count; i++){
				if (strings[i] == null){
					continue;
				}
				if (smallest == null){
					smallest = strings[i];
					ind = i;
				} else{
					if (string.Compare(strings[i], smallest, StringComparison.InvariantCulture) < 0){
						smallest = strings[i];
						ind = i;
					}
				}
			}
			return ind;
		}

		internal static void Read(BinaryReader reader, out long proteinFilePos, out int proteinLength,
			out bool proteinDecoy, out string proteinName){
			try{
				proteinName = reader.ReadString();
				proteinLength = reader.ReadInt32();
				proteinDecoy = reader.ReadBoolean();
				proteinFilePos = reader.ReadInt64();
			} catch (EndOfStreamException){
				proteinFilePos = long.MaxValue;
				proteinLength = int.MaxValue;
				proteinDecoy = false;
				proteinName = null;
			}
		}

		private void WriteLongIndexBlock(ICollection<long> blockFilePos, ICollection<int> blockProteinSequenceLengths,
			ICollection<bool> blockIsDecoys, IList<string> blockProteinNames, BinaryWriter longIndexWriter,
			ICollection<long> filePosShort2, ICollection<string> namesShort2){
			Count += blockFilePos.Count;
			filePosShort2.Add(longIndexWriter.BaseStream.Position);
			foreach (long l in blockFilePos){
				longIndexWriter.Write(l);
			}
			foreach (int i in blockProteinSequenceLengths){
				longIndexWriter.Write(i);
			}
			foreach (bool i in blockIsDecoys){
				longIndexWriter.Write(i);
			}
			foreach (string s in blockProteinNames){
				longIndexWriter.Write(s);
			}
			namesShort2.Add(blockProteinNames[0]);
			blockFilePos.Clear();
			blockProteinSequenceLengths.Clear();
			blockIsDecoys.Clear();
			blockProteinNames.Clear();
		}

		public void Dispose(){
			if (ProteinDataReader != null){
				ProteinDataReader.Close();
				ProteinDataReader = null;
			}
			if (longIndexReader != null){
				longIndexReader.Close();
				longIndexReader = null;
			}
			filePosShort = null;
			namesShort = null;
			if (tempFileNames != null){
				tempFileNames.Clear();
				tempFileNames = null;
			}
			if (lengths2 != null){
				lengths2.Clear();
				lengths2 = null;
			}
			if (decoy2 != null){
				decoy2.Clear();
				decoy2 = null;
			}
			if (names2 != null){
				names2.Clear();
				names2 = null;
			}
			if (filePos2 != null){
				filePos2.Clear();
				filePos2 = null;
			}
			if (duplicates != null){
				duplicates.Clear();
				duplicates = null;
			}
		}
	}
}