using System;

namespace BaseLibS.Calc.Util{
	internal class BigComplex : IFormattable, IEquatable<BigComplex>{
		private readonly BigRational realPart;
		private readonly BigRational imaginaryPart;

		internal BigComplex(BigRational realPart, BigRational imaginaryPart){
			this.realPart = realPart;
			this.imaginaryPart = imaginaryPart;
		}

		public string ToString(string format, IFormatProvider formatProvider) { throw new NotImplementedException(); }
		public bool Equals(BigComplex other) { return realPart.Equals(other.realPart) && imaginaryPart.Equals(other.imaginaryPart); }
	}
}