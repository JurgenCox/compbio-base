using System.Xml.Serialization;

namespace BaseLibS.Mol{
	public class ModificationSite{
		[XmlAttribute("site")]
		public string Site{
			get => "" + Aa;
			set => Aa = value[0];
		}

		[XmlIgnore]
		public char Aa { get; set; }

		[XmlArrayItem("neutralloss")]
		public NeutralLoss[] neutralloss_collection { get; set; }

		[XmlArrayItem("diagnostic")]
		public DiagnosticPeak[] diagnostic_collection { get; set; }

		public bool HasNeutralLoss => neutralloss_collection != null && neutralloss_collection.Length != 0;

		public bool HasDiagnosticPeak => diagnostic_collection != null && diagnostic_collection.Length != 0;
	}
}