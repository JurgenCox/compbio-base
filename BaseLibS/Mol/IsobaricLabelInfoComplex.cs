using System.Collections.Generic;
using BaseLibS.Util;
namespace BaseLibS.Mol{
	public class IsobaricLabelInfoComplex : IsobaricLabelInfo{
		public readonly List<InputParameter> vals = new List<InputParameter>{
			new InputParameter<string>("internalLabel", "internalLabel"),
			new InputParameter<string>("terminalLabel", "terminalLabel"),
			new InputParameter<double>("correctionFactorM2X13C", "correctionFactorM2X13C"),
			new InputParameter<double>("correctionFactorM13C15N", "correctionFactorM13C15N"),
			new InputParameter<double>("correctionFactorM13C", "correctionFactorM13C"),
			new InputParameter<double>("correctionFactorM15N", "correctionFactorM15N"),
			new InputParameter<double>("correctionFactorP15N", "correctionFactorP15N"),
			new InputParameter<double>("correctionFactorP13C", "correctionFactorP13C"),
			new InputParameter<double>("correctionFactorP15N13C", "correctionFactorP15N13C"),
			new InputParameter<double>("correctionFactorP2X13C", "correctionFactorP2X13C"),
			new InputParameter<bool>("tmtLike", "tmtLike")
		};
		public double correctionFactorM2X13C;
		public double correctionFactorM13C15N;
		public double correctionFactorM13C;
		public double correctionFactorM15N;
		public double correctionFactorP15N;
		public double correctionFactorP13C;
		public double correctionFactorP15N13C;
		public double correctionFactorP2X13C;
		public IsobaricLabelInfoComplex() : this("", "", 0, 0, 0, 0, 0, 0, 0, 0, true){
		}
		public IsobaricLabelInfoComplex(string[] values) : this(values[0], values[1],
			Parser.Double(values[2]), Parser.Double(values[3]),
			Parser.Double(values[4]), Parser.Double(values[5]),
			Parser.Double(values[6]), Parser.Double(values[7]),
			Parser.Double(values[8]), Parser.Double(values[9]),
			Parser.Bool(values[10])){
		}
		public IsobaricLabelInfoComplex(string internalLabel, string terminalLabel, double correctionFactorM2X13C,
			double correctionFactorM13C15N, double correctionFactorM13C, double correctionFactorM15N,
			double correctionFactorP15N, double correctionFactorP13C, double correctionFactorP15N13C, 
			double correctionFactorP2X13C, bool tmtLike) : base(internalLabel, terminalLabel, tmtLike){
			foreach (InputParameter val in vals){
				map.Add(val.Name, val);
			}
			this.correctionFactorM2X13C = correctionFactorM2X13C;
			this.correctionFactorM13C15N = correctionFactorM13C15N;
			this.correctionFactorM13C = correctionFactorM13C;
			this.correctionFactorM15N = correctionFactorM15N;
			this.correctionFactorP15N = correctionFactorP15N;
			this.correctionFactorP13C = correctionFactorP13C;
			this.correctionFactorP15N13C = correctionFactorP15N13C;
			this.correctionFactorP2X13C = correctionFactorP2X13C;
		}
	}
}