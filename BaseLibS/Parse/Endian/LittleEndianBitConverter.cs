namespace BaseLibS.Parse.Endian{
	public sealed class LittleEndianBitConverter : EndianBitConverter{
		public override bool IsLittleEndian() => true;
		protected internal override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index){
			for (int i = 0; i < bytes; i++){
				buffer[i + index] = unchecked((byte) (value & 0xff));
				value = value >> 8;
			}
		}
		protected internal override long FromBytes(byte[] buffer, int startIndex, int bytesToConvert){
			long ret = 0;
			for (int i = 0; i < bytesToConvert; i++){
				ret = unchecked((ret << 8) | buffer[startIndex + bytesToConvert - 1 - i]);
			}
			return ret;
		}
	}
}