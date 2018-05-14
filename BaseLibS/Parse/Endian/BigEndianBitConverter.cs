namespace BaseLibS.Parse.Endian{
	public sealed class BigEndianBitConverter : EndianBitConverter{
		public override bool IsLittleEndian() => false;
		protected internal override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index){
			int endOffset = index + bytes - 1;
			for (int i = 0; i < bytes; i++){
				buffer[endOffset - i] = unchecked((byte) (value & 0xff));
				value = value >> 8;
			}
		}
		protected internal override long FromBytes(byte[] buffer, int startIndex, int bytesToConvert){
			long ret = 0;
			for (int i = 0; i < bytesToConvert; i++){
				ret = unchecked((ret << 8) | buffer[startIndex + i]);
			}
			return ret;
		}
	}
}