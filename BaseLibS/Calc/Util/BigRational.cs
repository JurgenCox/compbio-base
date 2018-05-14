using System;
using System.Numerics;

namespace BaseLibS.Calc.Util{
	internal class BigRational : IFormattable, IComparable, IComparable<BigRational>, IEquatable<BigRational>{
		private static readonly BigRational one = new BigRational(1);

		/// <summary>
		/// Numerator and denominator have no common divisor.
		/// </summary>
		private BigInteger numerator;

		/// <summary>
		/// Numerator and denominator have no common divisor. Denominator is positive.
		/// </summary>
		private BigInteger denominator;

		public BigRational(BigInteger numerator, BigInteger denominator){
			if (denominator.IsZero){
				throw new ArgumentException("Denominator cannot be zero.");
			}
			this.numerator = numerator;
			this.denominator = denominator;
			Normalize();
		}

		public BigRational(long numerator, long denominator) : this(new BigInteger(numerator), new BigInteger(denominator)){}
		public BigRational(int numerator, int denominator) : this(new BigInteger(numerator), new BigInteger(denominator)){}
		public BigRational(ulong numerator, ulong denominator) : this(new BigInteger(numerator), new BigInteger(denominator)){}
		public BigRational(uint numerator, uint denominator) : this(new BigInteger(numerator), new BigInteger(denominator)){}
		public BigRational(int value) : this(value, 1){}
		public BigRational(long value) : this(value, 1){}
		public BigRational(uint value) : this(value, 1){}
		public BigRational(ulong value) : this(value, 1){}
		public BigRational(decimal value) : this(new BigInteger(value), 1){}
		public BigRational(BigInteger value) : this(value, 1){}
		private BigRational(){}

		public string ToString(string format, IFormatProvider formatProvider){
			return numerator + "/" + denominator;
		}

		public int CompareTo(object obj){
			if (obj is BigRational){
				return CompareTo((BigRational) obj);
			}
			if (obj is BigInteger){
				return CompareTo(new BigRational((BigInteger) obj));
			}
			if (obj is long || obj is int || obj is sbyte || obj is short){
				return CompareTo(new BigRational((long) obj));
			}
			if (obj is decimal){
				return CompareTo(new BigRational((decimal) obj));
			}
			if (obj is ulong || obj is uint || obj is byte || obj is ushort || obj is char){
				return CompareTo(new BigRational((ulong) obj));
			}
			throw new ArgumentException("Illegal type " + obj);
		}

		public int CompareTo(BigRational other){
			BigInteger left = numerator*other.denominator;
			BigInteger right = other.numerator*denominator;
			return left.CompareTo(right);
		}

		public bool Equals(BigRational other){
			if (numerator.Equals(BigInteger.Zero) && other.numerator.Equals(BigInteger.Zero)){
				return true;
			}
			return numerator.Equals(other.numerator) && denominator.Equals(other.denominator);
		}

		public static BigRational operator -(BigRational value){
			return new BigRational{numerator = -value.numerator, denominator = value.denominator};
		}

		public static BigRational operator -(BigRational left, BigRational right){
			return left + (-right);
		}

		public static BigRational operator --(BigRational value){
			return value - one;
		}

		public static bool operator !=(BigRational left, BigRational right){
			return left.numerator != right.numerator || left.denominator != right.denominator;
		}

		public static bool operator !=(BigRational left, long right){
			return  left != new BigRational(right);
		}

		public static bool operator !=(BigRational left, ulong right){
			return left != new BigRational(right);
		}

		public static bool operator !=(long left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator !=(ulong left, BigRational right){
			throw new NotImplementedException();
		}

		public static BigInteger operator &(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		public static BigRational operator *(BigRational left, BigRational right){
			return new BigRational(left.numerator*right.numerator, left.denominator*right.denominator);
		}

		public static BigRational operator /(BigRational dividend, BigRational divisor){
			if (divisor.Equals(Zero)){
				throw new ArgumentException("Division by zero is not allowed.");
			}
			return new BigRational(dividend.numerator*divisor.denominator, dividend.denominator*divisor.numerator);
		}

		public static BigInteger operator ^(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		public static BigInteger operator |(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		public static BigInteger operator ~(BigRational value){
			throw new NotImplementedException();
		}

		public static BigRational operator +(BigRational left, BigRational right){
			BigInteger gcd = BigInteger.GreatestCommonDivisor(left.denominator, right.denominator);
			BigInteger leftFact = left.denominator/gcd;
			BigInteger rightFact = right.denominator/gcd;
			BigInteger numerat = left.numerator*rightFact + right.numerator*leftFact;
			BigInteger denom = left.denominator*rightFact;
			return new BigRational{numerator = numerat, denominator = denom};
		}

		public static BigRational operator ++(BigRational value){
			return value + one;
		}

		public static bool operator <(BigRational left, BigRational right){
			return left.numerator*right.denominator < right.numerator*left.denominator;
		}

		public static bool operator <(BigRational left, long right){
			return left.numerator < right*left.denominator;
		}

		public static bool operator <(BigRational left, ulong right){
			return left.numerator < right*left.denominator;
		}

		public static bool operator <(long left, BigRational right){
			return left*right.denominator < right.numerator;
		}

		public static bool operator <(ulong left, BigRational right){
			return left*right.denominator < right.numerator;
		}

		public static bool operator <=(BigRational left, BigRational right){
			return left.numerator*right.denominator <= right.numerator*left.denominator;
		}

		public static bool operator <=(BigRational left, long right){
			return left.numerator <= right*left.denominator;
		}

		public static bool operator <=(BigRational left, ulong right){
			return left.numerator <= right*left.denominator;
		}

		public static bool operator <=(long left, BigRational right){
			return left*right.denominator <= right.numerator;
		}

		public static bool operator <=(ulong left, BigRational right){
			return left*right.denominator <= right.numerator;
		}

		public static bool operator ==(BigRational left, BigRational right){
			if (left.numerator.Equals(BigInteger.Zero) && right.numerator.Equals(BigInteger.Zero)){
				return true;
			}
			return left.numerator.Equals(right.numerator) && left.denominator.Equals(right.denominator);
		}

		public static bool operator ==(BigRational left, long right){
			if (left.numerator.Equals(BigInteger.Zero) && right == 0){
				return true;
			}
			return left.numerator.Equals(right) && left.denominator.Equals(BigInteger.One);
		}

		public static bool operator ==(BigRational left, ulong right){
			if (left.numerator.Equals(BigInteger.Zero) && right == 0){
				return true;
			}
			return left.numerator.Equals(right) && left.denominator.Equals(BigInteger.One);
		}

		public static bool operator ==(long left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator ==(ulong left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator >(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator >(BigRational left, long right){
			throw new NotImplementedException();
		}

		public static bool operator >(BigRational left, ulong right){
			throw new NotImplementedException();
		}

		public static bool operator >(long left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator >(ulong left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator >=(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator >=(BigRational left, long right){
			throw new NotImplementedException();
		}

		public static bool operator >=(BigRational left, ulong right){
			throw new NotImplementedException();
		}

		public static bool operator >=(long left, BigRational right){
			throw new NotImplementedException();
		}

		public static bool operator >=(ulong left, BigRational right){
			throw new NotImplementedException();
		}

		//public static explicit operator sbyte(BigInteger value);
		//public static explicit operator decimal(BigInteger value);
		//public static explicit operator double(BigInteger value);
		//public static explicit operator float(BigInteger value);
		//public static explicit operator ulong(BigInteger value);
		//public static explicit operator long(BigInteger value);
		//public static explicit operator uint(BigInteger value);
		//public static explicit operator int(BigInteger value);
		//public static explicit operator short(BigInteger value);
		//public static explicit operator ushort(BigInteger value);
		//public static explicit operator byte(BigInteger value);
		//public static explicit operator BigInteger(decimal value);
		//public static explicit operator BigInteger(double value);
		//public static explicit operator BigInteger(float value);
		//public static implicit operator BigInteger(byte value);
		//public static implicit operator BigInteger(int value);
		//public static implicit operator BigInteger(long value);
		//public static implicit operator BigInteger(sbyte value);
		//public static implicit operator BigInteger(short value);
		//public static implicit operator BigInteger(uint value);
		//public static implicit operator BigInteger(ulong value);
		//public static implicit operator BigInteger(ushort value);
		public bool IsInteger => denominator.IsOne;
		public bool IsOne => numerator.IsOne && denominator.IsOne;
		public bool IsZero => numerator.IsZero;
		public static BigRational MinusOne { get; } = new BigRational(-1);
		public static BigRational One => one;
		public int Sign => numerator.Sign;
		public static BigRational Zero { get; } = new BigRational(0);

		public static BigRational Abs(BigRational value){
			return new BigRational(BigInteger.Abs(value.numerator), value.denominator);
		}

		//public static BigInteger Add(BigInteger left, BigInteger right);
		//public static int Compare(BigInteger left, BigInteger right);
		//public int CompareTo(BigInteger other);
		//public int CompareTo(long other);
		//public int CompareTo(object obj);
		//public int CompareTo(ulong other);
		//public static BigInteger Divide(BigInteger dividend, BigInteger divisor);
		//public bool Equals(BigInteger other);
		//public bool Equals(long other);
		public override bool Equals(object obj){
			throw new NotImplementedException();
		}

		//public bool Equals(ulong other);
		public override int GetHashCode(){
			throw new NotImplementedException();
		}

		public static BigRational Max(BigRational left, BigRational right){
			return left > right ? left : right;
		}

		public static BigRational Min(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		//public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus);
		public static BigRational Multiply(BigRational left, BigRational right){
			throw new NotImplementedException();
		}

		public static BigRational Negate(BigRational value){
			throw new NotImplementedException();
		}

		//public static BigInteger Parse(string value);
		//public static BigInteger Parse(string value, IFormatProvider provider);
		//public static BigInteger Parse(string value, NumberStyles style);
		//public static BigInteger Parse(string value, NumberStyles style, IFormatProvider provider);
		//public static BigInteger Pow(BigInteger value, int exponent);
		//public static BigInteger Remainder(BigInteger dividend, BigInteger divisor);
		//public static BigInteger Subtract(BigInteger left, BigInteger right);
		//public byte[] ToByteArray();
		//public override string ToString();
		//public string ToString(IFormatProvider provider);
		//public string ToString(string format);
		//public string ToString(string format, IFormatProvider provider);
		//public static bool TryParse(string value, out BigInteger result);
		//public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out BigInteger result);
		private void Normalize(){
			if (denominator < 0){
				numerator = -numerator;
				denominator = -denominator;
			}
			BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
			if (gcd > 1){
				numerator = numerator/gcd;
				denominator = denominator/gcd;
			}
		}
	}
}