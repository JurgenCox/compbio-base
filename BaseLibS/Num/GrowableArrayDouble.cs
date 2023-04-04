using System;

namespace BaseLibS.Num {
	public class GrowableArrayDouble {
		private double[] data;
		private int dataLen;
		public GrowableArrayDouble() : this(16) {
		}
		public GrowableArrayDouble(int capacity) {
			capacity = Math.Max(capacity, 16);
			data = new double[capacity];
			dataLen = capacity;
		}
		public double[] ToArray(int length) {
			double[] result = new double[length];
			Array.Copy(data, result, Math.Min(result.Length, data.Length));
			return result;
		}
		public double this[int i] {
			get {
				if (i >= dataLen) {
					return 0;
				}
				return data[i];
			}
			set {
				if (i >= dataLen) {
					Ensure(i + 1);
				}
				data[i] = value;
			}
		}
		public void Add(int i, double value) {
			if (i >= dataLen) {
				Ensure(i + 1);
			}
			data[i] += value;
		}
		private void Ensure(int size) {
			int newSize = 2 * dataLen;
			newSize = Math.Max(size, newSize);
			double[] newData = new double[newSize];
			Array.Copy(data, newData, dataLen);
			dataLen = newData.Length;
			data = newData;
		}
	}
}
