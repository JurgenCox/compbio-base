using System;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Kernel{
	[Serializable]
	public class LinearKernelFunction : IKernelFunction{
		public bool UsesSquares => false;
		public string Name => "Linear";

		public Parameters Parameters{
			set{ }
			get{ return new Parameters(); }
		}

		public double Evaluate(BaseVector xi, BaseVector xj, double xSquarei, double xSquarej){
			return xi.Dot(xj);
		}

		public void Write(BinaryWriter writer){ }

		public void Read(BinaryReader reader){ }

		public KernelType GetKernelType(){
			return KernelType.Linear;
		}

		public object Clone(){
			return new LinearKernelFunction();
		}

		public string Description => "";
		public float DisplayRank => 0;
		public bool IsActive => true;
	}
}