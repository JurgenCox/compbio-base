using System.Collections.Generic;
using BaseLibS.Util;

namespace BaseLibS.Mol {
	public class IsobaricLabelInfo {
		public readonly List<InputParameter> vals = new List<InputParameter> {
			new InputParameter<string>("internalLabel", "internalLabel"),
			new InputParameter<string>("terminalLabel", "terminalLabel"),
			new InputParameter<double>("correctionFactorM2", "correctionFactorM2"),
			new InputParameter<double>("correctionFactorM1", "correctionFactorM1"),
			new InputParameter<double>("correctionFactorP1", "correctionFactorP1"),
			new InputParameter<double>("correctionFactorP2", "correctionFactorP2"),
			new InputParameter<bool>("tmtLike", "tmtLike")
		};

		public readonly Dictionary<string, InputParameter> map;
		public string internalLabel;
		public string terminalLabel;
		public double correctionFactorM2;
		public double correctionFactorM1;
		public double correctionFactorP1;
		public double correctionFactorP2;
		public bool tmtLike;
		public IsobaricLabelInfo() : this("", "", 0, 0, 0, 0, true) { }

		public IsobaricLabelInfo(string[] values) : this(values[0], values[1], Parser.Double(values[2]),
			Parser.Double(values[3]), Parser.Double(values[4]), Parser.Double(values[5]), Parser.Bool(values[6])) { }

		public IsobaricLabelInfo(string internalLabel, string terminalLabel, double correctionFactorM2,
			double correctionFactorM1, double correctionFactorP1, double correctionFactorP2, bool tmtLike) {
			map = new Dictionary<string, InputParameter>();
			foreach (InputParameter val in vals) {
				map.Add(val.Name, val);
			}
			this.internalLabel = internalLabel;
			this.terminalLabel = terminalLabel;
			this.correctionFactorM2 = correctionFactorM2;
			this.correctionFactorM1 = correctionFactorM1;
			this.correctionFactorP1 = correctionFactorP1;
			this.correctionFactorP2 = correctionFactorP2;
			this.tmtLike = tmtLike;
		}
	}
}