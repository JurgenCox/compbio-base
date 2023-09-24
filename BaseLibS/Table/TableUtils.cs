using BaseLibS.Util;
using System.IO;
using System;

namespace BaseLibS.Table {
	public static class TableUtils {
		public static void WriteElement(BinaryWriter writer, object o, ColumnType columnType) {
			switch (columnType) {
				case ColumnType.Boolean:
					if (o is DBNull) {
						writer.Write(false);
					} else {
						writer.Write((bool)o);
					}
					break;
				case ColumnType.Text:
					if (o == null) {
						writer.Write("");
					} else if (o is DBNull) {
						writer.Write("");
					} else if (o is char) {
						writer.Write("" + (char)o);
					} else {
						writer.Write((string)o);
					}
					break;
				case ColumnType.Integer:
					if (o is DBNull) {
						writer.Write(int.MaxValue);
					} else if (o is UInt16) {
						UInt16 x = (UInt16)o;
						int y = x;
						writer.Write(y);
					} else if (o is byte) {
						byte x = (byte)o;
						int y = x;
						writer.Write(y);
					} else if (o is UInt64) {
						ulong x = (ulong)o;
						int y = (int)x;
						writer.Write(y);
					} else if (o is Int64) {
						long x = (long)o;
						int y = (int)x;
						writer.Write(y);
					} else {
						writer.Write((int)o);
					}
					break;
				case ColumnType.Numeric:
					if (o is DBNull) {
						writer.Write(double.NaN);
					} else if (o is int) {
						int x = (int)o;
						double d = x;
						writer.Write(d);
					} else if (o is float) {
						float x = (float)o;
						double d = x;
						writer.Write(d);
					} else {
						writer.Write((double)o);
					}
					break;
				case ColumnType.Categorical:
					if (o is DBNull) {
						writer.Write("");
					} else if (o is Boolean) {
						bool x = (bool)o;
						string y = Parser.ToString(x);
						writer.Write(y);
					} else if (o is char) {
						char x = (char)o;
						string y = "" + x;
						writer.Write(y);
					} else {
						writer.Write((string)o);
					}
					break;
				case ColumnType.MultiNumeric:
					writer.Write((string)o);
					break;
				case ColumnType.MultiInteger:
					if (o is int[]) {
						int[] x = (int[])o;
						string s = StringUtils.Concat(";", x);
						writer.Write(s);
					} else {
						writer.Write((string)o);
					}
					break;
				case ColumnType.DateTime:
					throw new Exception("Not implemented.");
				case ColumnType.Color:
					throw new Exception("Not implemented.");
				case ColumnType.DashStyle:
					throw new Exception("Not implemented.");
				default:
					throw new Exception("Never get here.");
			}
		}
		public static object ReadElement(BinaryReader reader, ColumnType columnType) {
			switch (columnType) {
				case ColumnType.Boolean:
					return reader.ReadBoolean();
				case ColumnType.Text:
					return reader.ReadString();
				case ColumnType.Integer:
					return reader.ReadInt32();
				case ColumnType.Numeric:
					return reader.ReadDouble();
				case ColumnType.Categorical:
					return reader.ReadString();
				case ColumnType.MultiNumeric:
					return reader.ReadString();
				case ColumnType.MultiInteger:
					return reader.ReadString();
				case ColumnType.DateTime:
					throw new Exception("Not implemented.");
				case ColumnType.Color:
					throw new Exception("Not implemented.");
				case ColumnType.DashStyle:
					throw new Exception("Not implemented.");
				default:
					throw new Exception("Never get here.");
			}
		}
	}
}
