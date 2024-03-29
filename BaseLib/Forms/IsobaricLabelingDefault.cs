﻿namespace BaseLib.Forms {
	public class IsobaricLabelingDefault {
		private readonly string[] internalLabels;
		private readonly string[] terminalLabels;
		public IsobaricLabelingDefault(string name, string[] internalLabels, string[] terminalLabels, bool advancedCorrections) {
			Name = name;
			this.internalLabels = internalLabels;
			this.terminalLabels = terminalLabels;
			this.AdvancedCorrections = advancedCorrections;
		}

		public string Name { get; }
		public bool AdvancedCorrections{ get; }
		public int Count => internalLabels.Length;

		public string GetInternalLabel(int index) {
			return internalLabels[index];
		}

		public string GetTerminalLabel(int index) {
			return index < terminalLabels.Length ? terminalLabels[index] : "";
		}

		public bool IsLikelyTmtLike(int index) {
			return internalLabels[index].ToLower().Contains("tmt");
		}
	}
}