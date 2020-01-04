using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Ms{
	/// <summary>
	/// Only field is an array of double deltaM. Not used yet (2014-04-01), 
	/// but will be soon. (See also field NeedsGrid of class RawFile.)
	/// </summary>
	public class MassGridInfo{
		private readonly double[] deltaM;

		internal MassGridInfo(double[] deltaM){
			this.deltaM = deltaM;
		}

		internal MassGridInfo(BinaryReader reader){
			deltaM = FileUtils.ReadDoubleArray(reader);
		}

		internal void Write(BinaryWriter writer){
			FileUtils.Write(deltaM, writer);
		}
	}
}