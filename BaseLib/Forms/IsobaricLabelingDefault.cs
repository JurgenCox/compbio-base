namespace BaseLib.Forms {
	public class IsobaricLabelingDefault {
		private string[] internalLabels;
		private string[] terminalLabels;

		public IsobaricLabelingDefault(string name, string[] internalLabels, string[] terminalLabels) {
			Name = name;
			this.internalLabels = internalLabels;
			this.terminalLabels = terminalLabels;
		}

		public string Name { get; }
	}
}