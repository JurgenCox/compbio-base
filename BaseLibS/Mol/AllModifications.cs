using System;
using System.Collections.Generic;

namespace BaseLibS.Mol{
	[Serializable]
	public class AllModifications{
		public Dictionary<char, ushort[]> Modifications{ get; set; }
		public ushort[] NTermModifications{ get; set; }
		public ushort[] CTermModifications{ get; set; }
	}
}