using System;

namespace BaseLibS.Graph.Image.Formats.Gif{
	internal struct PackedField : IEquatable<PackedField>{
		private static readonly bool[] bits = new bool[8];
		public byte Byte{
			get{
				int returnValue = 0;
				int bitShift = 7;
				foreach (bool bit in bits){
					int bitValue;
					if (bit){
						bitValue = 1 << bitShift;
					} else{
						bitValue = 0;
					}
					returnValue |= bitValue;
					bitShift--;
				}
				return Convert.ToByte(returnValue & 0xFF);
			}
		}
		public static PackedField FromInt(byte value){
			PackedField packed = new PackedField();
			packed.SetBits(0, 8, value);
			return packed;
		}
		public void SetBit(int index, bool valueToSet){
			if (index < 0 || index > 7){
				string message = "Index must be between 0 and 7. Supplied index: " + index;
				throw new ArgumentOutOfRangeException(nameof(index), message);
			}
			bits[index] = valueToSet;
		}
		public void SetBits(int startIndex, int length, int valueToSet){
			if (startIndex < 0 || startIndex > 7){
				string message = $"Start index must be between 0 and 7. Supplied index: {startIndex}";
				throw new ArgumentOutOfRangeException(nameof(startIndex), message);
			}
			if (length < 1 || startIndex + length > 8){
				string message = "Length must be greater than zero and the sum of length and start index must be less than 8. " +
								$"Supplied length: {length}. Supplied start index: {startIndex}";
				throw new ArgumentOutOfRangeException(nameof(length), message);
			}
			int bitShift = length - 1;
			for (int i = startIndex; i < startIndex + length; i++){
				int bitValueIfSet = (1 << bitShift);
				int bitValue = (valueToSet & bitValueIfSet);
				int bitIsSet = (bitValue >> bitShift);
				bits[i] = (bitIsSet == 1);
				bitShift--;
			}
		}
		public bool GetBit(int index){
			if (index < 0 || index > 7){
				string message = $"Index must be between 0 and 7. Supplied index: {index}";
				throw new ArgumentOutOfRangeException(nameof(index), message);
			}
			return bits[index];
		}
		public int GetBits(int startIndex, int length){
			if (startIndex < 0 || startIndex > 7){
				string message = $"Start index must be between 0 and 7. Supplied index: {startIndex}";
				throw new ArgumentOutOfRangeException(nameof(startIndex), message);
			}
			if (length < 1 || startIndex + length > 8){
				string message = "Length must be greater than zero and the sum of length and start index must be less than 8. " +
								$"Supplied length: {length}. Supplied start index: {startIndex}";
				throw new ArgumentOutOfRangeException(nameof(length), message);
			}
			int returnValue = 0;
			int bitShift = length - 1;
			for (int i = startIndex; i < startIndex + length; i++){
				int bitValue = (bits[i] ? 1 : 0) << bitShift;
				returnValue += bitValue;
				bitShift--;
			}
			return returnValue;
		}
		public override bool Equals(object obj){
			PackedField? field = obj as PackedField?;
			return Byte == field?.Byte;
		}
		public bool Equals(PackedField other){
			return Byte.Equals(other.Byte);
		}
		public override string ToString(){
			return $"PackedField [ Byte={Byte} ]";
		}
		public override int GetHashCode(){
			return Byte.GetHashCode();
		}
	}
}