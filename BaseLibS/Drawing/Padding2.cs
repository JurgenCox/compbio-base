using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
namespace BaseLibS.Drawing{
	[Serializable]
	public struct Padding2{
		private bool _all;
		private int _top;
		private int _left;
		private int _right;
		private int _bottom;
		public static readonly Padding2 Empty = new Padding2(0);
		public Padding2(int all){
			this._all = true;
			this._top = this._left = this._right = this._bottom = all;
		}
		public Padding2(int left, int top, int right, int bottom){
			this._top = top;
			this._left = left;
			this._right = right;
			this._bottom = bottom;
			this._all = this._top == this._left && this._top == this._right && this._top == this._bottom;
		}
		[RefreshProperties(RefreshProperties.All)]
		public int All{
			get => !this._all ? -1 : this._top;
			set{
				if (this._all && this._top == value)
					return;
				this._all = true;
				this._top = this._left = this._right = this._bottom = value;
			}
		}
		[RefreshProperties(RefreshProperties.All)]
		public int Bottom{
			get => this._all ? this._top : this._bottom;
			set{
				if (!this._all && this._bottom == value)
					return;
				this._all = false;
				this._bottom = value;
			}
		}
		[RefreshProperties(RefreshProperties.All)]
		public int Left{
			get => this._all ? this._top : this._left;
			set{
				if (!this._all && this._left == value)
					return;
				this._all = false;
				this._left = value;
			}
		}
		[RefreshProperties(RefreshProperties.All)]
		public int Right{
			get => this._all ? this._top : this._right;
			set{
				if (!this._all && this._right == value)
					return;
				this._all = false;
				this._right = value;
			}
		}
		[RefreshProperties(RefreshProperties.All)]
		public int Top{
			get => this._top;
			set{
				if (!this._all && this._top == value)
					return;
				this._all = false;
				this._top = value;
			}
		}
		[Browsable(false)] public int Horizontal => this.Left + this.Right;
		[Browsable(false)] public int Vertical => this.Top + this.Bottom;
		[Browsable(false)] public Size2 Size => new Size2(this.Horizontal, this.Vertical);
		public static Padding2 Add(Padding2 p1, Padding2 p2) => p1 + p2;
		public static Padding2 Subtract(Padding2 p1, Padding2 p2) => p1 - p2;
		public override bool Equals(object other) => other is Padding2 padding && padding == this;
		public static Padding2 operator +(Padding2 p1, Padding2 p2) => new Padding2(p1.Left + p2.Left, p1.Top + p2.Top,
			p1.Right + p2.Right, p1.Bottom + p2.Bottom);
		public static Padding2 operator -(Padding2 p1, Padding2 p2) => new Padding2(p1.Left - p2.Left, p1.Top - p2.Top,
			p1.Right - p2.Right, p1.Bottom - p2.Bottom);
		public static bool operator ==(Padding2 p1, Padding2 p2) => p1.Left == p2.Left && p1.Top == p2.Top &&
		                                                            p1.Right == p2.Right && p1.Bottom == p2.Bottom;
		public static bool operator !=(Padding2 p1, Padding2 p2) => !(p1 == p2);
		public override int GetHashCode() => this.Left ^ RotateLeft(this.Top, 8) ^ RotateLeft(this.Right, 16) ^
		                                     RotateLeft(this.Bottom, 24);
		public override string ToString() => "{Left=" +
		                                     this.Left.ToString((IFormatProvider) CultureInfo.CurrentCulture) +
		                                     ",Top=" + this.Top.ToString((IFormatProvider) CultureInfo.CurrentCulture) +
		                                     ",Right=" +
		                                     this.Right.ToString((IFormatProvider) CultureInfo.CurrentCulture) +
		                                     ",Bottom=" +
		                                     this.Bottom.ToString((IFormatProvider) CultureInfo.CurrentCulture) + "}";
		private void ResetAll() => this.All = 0;
		private void ResetBottom() => this.Bottom = 0;
		private void ResetLeft() => this.Left = 0;
		private void ResetRight() => this.Right = 0;
		private void ResetTop() => this.Top = 0;
		internal void Scale(float dx, float dy){
			this._top = (int) ((double) this._top * (double) dy);
			this._left = (int) ((double) this._left * (double) dx);
			this._right = (int) ((double) this._right * (double) dx);
			this._bottom = (int) ((double) this._bottom * (double) dy);
		}
		internal bool ShouldSerializeAll() => this._all;
		[Conditional("DEBUG")]
		private void Debug_SanityCheck(){
			int num = this._all ? 1 : 0;
		}
		public static int RotateLeft(int value, int nBits){
			nBits %= 32;
			return value << nBits | value >> 32 - nBits;
		}
	}
}