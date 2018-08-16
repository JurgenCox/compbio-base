using System.Windows.Forms;

namespace BaseLib.Forms {
	public partial class IsobaricLabelsParamControl : UserControl {
		public IsobaricLabelsParamControl() {
			InitializeComponent();
		}

		public string[][] Value { get; set; }
	}
}
