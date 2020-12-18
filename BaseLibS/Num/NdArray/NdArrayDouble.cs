using System;

namespace BaseLibS.Num.NdArray{
	/// <summary>
	/// Class for providing operator overloading
	/// this cannot be implemented for the generic NdArray type because
	/// operators are not applicable to generic types.
	/// </summary>
	[Serializable]
	public class NdArrayDouble : NdArray<double>{
		public NdArrayDouble(double[] data) : base(data){ }
		public NdArrayDouble(double[] data, int[] shape) : base(data, shape){ }

		public NdArrayDouble(double[] data, int[] shape, int[] stride, int offset = 0) : base(data, shape, stride,
			offset){ }

		public NdArrayDouble(NdArray<double> array) : base(array.Data, array.Shape, array.Stride, array.Offset){ }

		public static NdArrayDouble operator +(double a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x + y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator +(NdArrayDouble a, double b){
			return Util.Broadcast((x, y) => x + y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator +(NdArrayDouble a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x + y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator -(double a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x - y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator -(NdArrayDouble a, double b){
			return Util.Broadcast((x, y) => x - y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator -(NdArrayDouble a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x - y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator *(double a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x * y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator *(NdArrayDouble a, double b){
			return Util.Broadcast((x, y) => x * y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator *(NdArrayDouble a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x * y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator /(double a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x / y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator /(NdArrayDouble a, double b){
			return Util.Broadcast((x, y) => x / y, a, b).AsNdArrayDouble();
		}

		public static NdArrayDouble operator /(NdArrayDouble a, NdArrayDouble b){
			return Util.Broadcast((x, y) => x / y, a, b).AsNdArrayDouble();
		}
	}
}