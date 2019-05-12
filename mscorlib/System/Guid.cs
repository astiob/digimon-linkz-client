using Mono.Security;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace System
{
	/// <summary>Represents a globally unique identifier (GUID).</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public struct Guid : IFormattable, IComparable, IComparable<Guid>, IEquatable<Guid>
	{
		private int _a;

		private short _b;

		private short _c;

		private byte _d;

		private byte _e;

		private byte _f;

		private byte _g;

		private byte _h;

		private byte _i;

		private byte _j;

		private byte _k;

		/// <summary>A read-only instance of the <see cref="T:System.Guid" /> class whose value is guaranteed to be all zeroes.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly Guid Empty = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

		private static object _rngAccess = new object();

		private static RandomNumberGenerator _rng;

		private static RandomNumberGenerator _fastRng;

		/// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> class using the specified array of bytes.</summary>
		/// <param name="b">A 16 element byte array containing values with which to initialize the GUID. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="b" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="b" /> is not 16 bytes long. </exception>
		public Guid(byte[] b)
		{
			Guid.CheckArray(b, 16);
			this._a = BitConverterLE.ToInt32(b, 0);
			this._b = BitConverterLE.ToInt16(b, 4);
			this._c = BitConverterLE.ToInt16(b, 6);
			this._d = b[8];
			this._e = b[9];
			this._f = b[10];
			this._g = b[11];
			this._h = b[12];
			this._i = b[13];
			this._j = b[14];
			this._k = b[15];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> class using the value represented by the specified string.</summary>
		/// <param name="g">A <see cref="T:System.String" /> that contains a GUID in one of the following formats ('d' represents a hexadecimal digit whose case is ignored): 32 contiguous digits: dddddddddddddddddddddddddddddddd -or- Groups of 8, 4, 4, 4, and 12 digits with hyphens between the groups. The entire GUID can optionally be enclosed in matching braces or parentheses: dddddddd-dddd-dddd-dddd-dddddddddddd -or- {dddddddd-dddd-dddd-dddd-dddddddddddd} -or- (dddddddd-dddd-dddd-dddd-dddddddddddd) -or- Groups of 8, 4, and 4 digits, and a subset of eight groups of 2 digits, with each group prefixed by "0x" or "0X", and separated by commas. The entire GUID, as well as the subset, is enclosed in matching braces: {0xdddddddd, 0xdddd, 0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}} All braces, commas, and "0x" prefixes are required. All embedded spaces are ignored. All leading zeroes in a group are ignored.The digits shown in a group are the maximum number of meaningful digits that can appear in that group. You can specify from 1 to the number of digits shown for a group. The specified digits are assumed to be the low order digits of the group. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="g" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The format of <paramref name="g" /> is invalid. </exception>
		/// <exception cref="T:System.OverflowException">The format of <paramref name="g" /> is invalid. </exception>
		/// <exception cref="T:System.Exception">An internal type conversion error occurred. </exception>
		public Guid(string g)
		{
			Guid.CheckNull(g);
			g = g.Trim();
			Guid.GuidParser guidParser = new Guid.GuidParser(g);
			Guid guid = guidParser.Parse();
			this = guid;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> class using the specified integers and byte array.</summary>
		/// <param name="a">The first 4 bytes of the GUID. </param>
		/// <param name="b">The next 2 bytes of the GUID. </param>
		/// <param name="c">The next 2 bytes of the GUID. </param>
		/// <param name="d">The remaining 8 bytes of the GUID. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="d" /> is not 8 bytes long. </exception>
		public Guid(int a, short b, short c, byte[] d)
		{
			Guid.CheckArray(d, 8);
			this._a = a;
			this._b = b;
			this._c = c;
			this._d = d[0];
			this._e = d[1];
			this._f = d[2];
			this._g = d[3];
			this._h = d[4];
			this._i = d[5];
			this._j = d[6];
			this._k = d[7];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> class using the specified integers and bytes.</summary>
		/// <param name="a">The first 4 bytes of the GUID. </param>
		/// <param name="b">The next 2 bytes of the GUID. </param>
		/// <param name="c">The next 2 bytes of the GUID. </param>
		/// <param name="d">The next byte of the GUID. </param>
		/// <param name="e">The next byte of the GUID. </param>
		/// <param name="f">The next byte of the GUID. </param>
		/// <param name="g">The next byte of the GUID. </param>
		/// <param name="h">The next byte of the GUID. </param>
		/// <param name="i">The next byte of the GUID. </param>
		/// <param name="j">The next byte of the GUID. </param>
		/// <param name="k">The next byte of the GUID. </param>
		public Guid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
			this._a = a;
			this._b = b;
			this._c = c;
			this._d = d;
			this._e = e;
			this._f = f;
			this._g = g;
			this._h = h;
			this._i = i;
			this._j = j;
			this._k = k;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> class using the specified unsigned integers and bytes.</summary>
		/// <param name="a">The first 4 bytes of the GUID. </param>
		/// <param name="b">The next 2 bytes of the GUID. </param>
		/// <param name="c">The next 2 bytes of the GUID. </param>
		/// <param name="d">The next byte of the GUID. </param>
		/// <param name="e">The next byte of the GUID. </param>
		/// <param name="f">The next byte of the GUID. </param>
		/// <param name="g">The next byte of the GUID. </param>
		/// <param name="h">The next byte of the GUID. </param>
		/// <param name="i">The next byte of the GUID. </param>
		/// <param name="j">The next byte of the GUID. </param>
		/// <param name="k">The next byte of the GUID. </param>
		[CLSCompliant(false)]
		public Guid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
			this = new Guid((int)a, (short)b, (short)c, d, e, f, g, h, i, j, k);
		}

		static Guid()
		{
			if (MonoTouchAOTHelper.FalseFlag)
			{
				GenericComparer<Guid> genericComparer = new GenericComparer<Guid>();
				GenericEqualityComparer<Guid> genericEqualityComparer = new GenericEqualityComparer<Guid>();
			}
		}

		private static void CheckNull(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException(Locale.GetText("Value cannot be null."));
			}
		}

		private static void CheckLength(byte[] o, int l)
		{
			if (o.Length != l)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Array should be exactly {0} bytes long."), l));
			}
		}

		private static void CheckArray(byte[] o, int l)
		{
			Guid.CheckNull(o);
			Guid.CheckLength(o, l);
		}

		private static int Compare(int x, int y)
		{
			if (x < y)
			{
				return -1;
			}
			return 1;
		}

		/// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Value Description A negative integer This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. A positive integer This instance is greater than <paramref name="value" />.-or- <paramref name="value" /> is null. </returns>
		/// <param name="value">An object to compare, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.Guid" />. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is Guid))
			{
				throw new ArgumentException("value", Locale.GetText("Argument of System.Guid.CompareTo should be a Guid."));
			}
			return this.CompareTo((Guid)value);
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="o" /> is a <see cref="T:System.Guid" /> that has the same value as this instance; otherwise, false.</returns>
		/// <param name="o">The object to compare with this instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object o)
		{
			return o is Guid && this.CompareTo((Guid)o) == 0;
		}

		/// <summary>Compares this instance to a specified <see cref="T:System.Guid" /> object and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Value Description A negative integer This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. A positive integer This instance is greater than <paramref name="value" />. </returns>
		/// <param name="value">A <see cref="T:System.Guid" /> object to compare to this instance.</param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(Guid value)
		{
			if (this._a != value._a)
			{
				return Guid.Compare(this._a, value._a);
			}
			if (this._b != value._b)
			{
				return Guid.Compare((int)this._b, (int)value._b);
			}
			if (this._c != value._c)
			{
				return Guid.Compare((int)this._c, (int)value._c);
			}
			if (this._d != value._d)
			{
				return Guid.Compare((int)this._d, (int)value._d);
			}
			if (this._e != value._e)
			{
				return Guid.Compare((int)this._e, (int)value._e);
			}
			if (this._f != value._f)
			{
				return Guid.Compare((int)this._f, (int)value._f);
			}
			if (this._g != value._g)
			{
				return Guid.Compare((int)this._g, (int)value._g);
			}
			if (this._h != value._h)
			{
				return Guid.Compare((int)this._h, (int)value._h);
			}
			if (this._i != value._i)
			{
				return Guid.Compare((int)this._i, (int)value._i);
			}
			if (this._j != value._j)
			{
				return Guid.Compare((int)this._j, (int)value._j);
			}
			if (this._k != value._k)
			{
				return Guid.Compare((int)this._k, (int)value._k);
			}
			return 0;
		}

		/// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Guid" /> object represent the same value.</summary>
		/// <returns>true if <paramref name="g" /> is equal to this instance; otherwise, false.</returns>
		/// <param name="g">A <see cref="T:System.Guid" /> object to compare to this instance.</param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(Guid g)
		{
			return this.CompareTo(g) == 0;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>The hash code for this instance.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			int num = this._a;
			num ^= ((int)this._b << 16 | (int)this._c);
			num ^= (int)this._d << 24;
			num ^= (int)this._e << 16;
			num ^= (int)this._f << 8;
			num ^= (int)this._g;
			num ^= (int)this._h << 24;
			num ^= (int)this._i << 16;
			num ^= (int)this._j << 8;
			return num ^ (int)this._k;
		}

		private static char ToHex(int b)
		{
			return (char)((b >= 10) ? (97 + b - 10) : (48 + b));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> class.</summary>
		/// <returns>A new <see cref="T:System.Guid" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public static Guid NewGuid()
		{
			byte[] array = new byte[16];
			object rngAccess = Guid._rngAccess;
			lock (rngAccess)
			{
				if (Guid._rng == null)
				{
					Guid._rng = RandomNumberGenerator.Create();
				}
				Guid._rng.GetBytes(array);
			}
			Guid result = new Guid(array);
			result._d = ((result._d & 63) | 128);
			result._c = (short)(((long)result._c & 4095L) | 16384L);
			return result;
		}

		internal static byte[] FastNewGuidArray()
		{
			byte[] array = new byte[16];
			object rngAccess = Guid._rngAccess;
			lock (rngAccess)
			{
				if (Guid._rng != null)
				{
					Guid._fastRng = Guid._rng;
				}
				if (Guid._fastRng == null)
				{
					Guid._fastRng = new RNGCryptoServiceProvider();
				}
				Guid._fastRng.GetBytes(array);
			}
			array[8] = ((array[8] & 63) | 128);
			array[7] = ((array[7] & 15) | 64);
			return array;
		}

		/// <summary>Returns a 16-element byte array that contains the value of this instance.</summary>
		/// <returns>A 16-element byte array.</returns>
		/// <filterpriority>2</filterpriority>
		public byte[] ToByteArray()
		{
			byte[] array = new byte[16];
			int num = 0;
			byte[] bytes = BitConverterLE.GetBytes(this._a);
			for (int i = 0; i < 4; i++)
			{
				array[num++] = bytes[i];
			}
			bytes = BitConverterLE.GetBytes(this._b);
			for (int i = 0; i < 2; i++)
			{
				array[num++] = bytes[i];
			}
			bytes = BitConverterLE.GetBytes(this._c);
			for (int i = 0; i < 2; i++)
			{
				array[num++] = bytes[i];
			}
			array[8] = this._d;
			array[9] = this._e;
			array[10] = this._f;
			array[11] = this._g;
			array[12] = this._h;
			array[13] = this._i;
			array[14] = this._j;
			array[15] = this._k;
			return array;
		}

		private static void AppendInt(StringBuilder builder, int value)
		{
			builder.Append(Guid.ToHex(value >> 28 & 15));
			builder.Append(Guid.ToHex(value >> 24 & 15));
			builder.Append(Guid.ToHex(value >> 20 & 15));
			builder.Append(Guid.ToHex(value >> 16 & 15));
			builder.Append(Guid.ToHex(value >> 12 & 15));
			builder.Append(Guid.ToHex(value >> 8 & 15));
			builder.Append(Guid.ToHex(value >> 4 & 15));
			builder.Append(Guid.ToHex(value & 15));
		}

		private static void AppendShort(StringBuilder builder, short value)
		{
			builder.Append(Guid.ToHex(value >> 12 & 15));
			builder.Append(Guid.ToHex(value >> 8 & 15));
			builder.Append(Guid.ToHex(value >> 4 & 15));
			builder.Append(Guid.ToHex((int)(value & 15)));
		}

		private static void AppendByte(StringBuilder builder, byte value)
		{
			builder.Append(Guid.ToHex(value >> 4 & 15));
			builder.Append(Guid.ToHex((int)(value & 15)));
		}

		private string BaseToString(bool h, bool p, bool b)
		{
			StringBuilder stringBuilder = new StringBuilder(40);
			if (p)
			{
				stringBuilder.Append('(');
			}
			else if (b)
			{
				stringBuilder.Append('{');
			}
			Guid.AppendInt(stringBuilder, this._a);
			if (h)
			{
				stringBuilder.Append('-');
			}
			Guid.AppendShort(stringBuilder, this._b);
			if (h)
			{
				stringBuilder.Append('-');
			}
			Guid.AppendShort(stringBuilder, this._c);
			if (h)
			{
				stringBuilder.Append('-');
			}
			Guid.AppendByte(stringBuilder, this._d);
			Guid.AppendByte(stringBuilder, this._e);
			if (h)
			{
				stringBuilder.Append('-');
			}
			Guid.AppendByte(stringBuilder, this._f);
			Guid.AppendByte(stringBuilder, this._g);
			Guid.AppendByte(stringBuilder, this._h);
			Guid.AppendByte(stringBuilder, this._i);
			Guid.AppendByte(stringBuilder, this._j);
			Guid.AppendByte(stringBuilder, this._k);
			if (p)
			{
				stringBuilder.Append(')');
			}
			else if (b)
			{
				stringBuilder.Append('}');
			}
			return stringBuilder.ToString();
		}

		/// <summary>Returns a <see cref="T:System.String" /> representation of the value of this instance in registry format.</summary>
		/// <returns>A String formatted in this pattern: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx where the value of the GUID is represented as a series of lower-case hexadecimal digits in groups of 8, 4, 4, 4, and 12 digits and separated by hyphens. An example of a return value is "382c74c3-721d-4f34-80e5-57657b6cbc27".</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return this.BaseToString(true, false, false);
		}

		/// <summary>Returns a <see cref="T:System.String" /> representation of the value of this <see cref="T:System.Guid" /> instance, according to the provided format specifier.</summary>
		/// <returns>The value of this <see cref="T:System.Guid" />, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
		/// <param name="format">A single format specifier that indicates how to format the value of this <see cref="T:System.Guid" />. The <paramref name="format" /> parameter can be "N", "D", "B", or "P". If <paramref name="format" /> is null or the empty string (""), "D" is used. </param>
		/// <exception cref="T:System.FormatException">The value of <paramref name="format" /> is not null, the empty string (""), "N", "D", "B", or "P". </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format)
		{
			bool h = true;
			bool p = false;
			bool b = false;
			if (format != null)
			{
				string a = format.ToLowerInvariant();
				if (a == "b")
				{
					b = true;
				}
				else if (a == "p")
				{
					p = true;
				}
				else if (a == "n")
				{
					h = false;
				}
				else if (a != "d" && a != string.Empty)
				{
					throw new FormatException(Locale.GetText("Argument to Guid.ToString(string format) should be \"b\", \"B\", \"d\", \"D\", \"n\", \"N\", \"p\" or \"P\""));
				}
			}
			return this.BaseToString(h, p, b);
		}

		/// <summary>Returns a <see cref="T:System.String" /> representation of the value of this instance of the <see cref="T:System.Guid" /> class, according to the provided format specifier and culture-specific format information.</summary>
		/// <returns>The value of this <see cref="T:System.Guid" />, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
		/// <param name="format">A single format specifier that indicates how to format the value of this <see cref="T:System.Guid" />. The <paramref name="format" /> parameter can be "N", "D", "B", or "P". If <paramref name="format" /> is null or the empty string (""), "D" is used. </param>
		/// <param name="provider">(Reserved) An IFormatProvider reference that supplies culture-specific formatting services. </param>
		/// <exception cref="T:System.FormatException">The value of <paramref name="format" /> is not null, the empty string (""), "N", "D", "B", or "P". </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return this.ToString(format);
		}

		/// <summary>Returns an indication whether the values of two specified <see cref="T:System.Guid" /> objects are equal.</summary>
		/// <returns>true if <paramref name="a" /> and <paramref name="b" /> are equal; otherwise, false.</returns>
		/// <param name="a">A <see cref="T:System.Guid" /> object. </param>
		/// <param name="b">A <see cref="T:System.Guid" /> object. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(Guid a, Guid b)
		{
			return a.Equals(b);
		}

		/// <summary>Returns an indication whether the values of two specified <see cref="T:System.Guid" /> objects are not equal.</summary>
		/// <returns>true if <paramref name="a" /> and <paramref name="b" /> are not equal; otherwise, false.</returns>
		/// <param name="a">A <see cref="T:System.Guid" /> object. </param>
		/// <param name="b">A <see cref="T:System.Guid" /> object. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(Guid a, Guid b)
		{
			return !a.Equals(b);
		}

		internal class GuidParser
		{
			private string _src;

			private int _length;

			private int _cur;

			public GuidParser(string src)
			{
				this._src = src;
				this.Reset();
			}

			private void Reset()
			{
				this._cur = 0;
				this._length = this._src.Length;
			}

			private bool AtEnd()
			{
				return this._cur >= this._length;
			}

			private void ThrowFormatException()
			{
				throw new FormatException(Locale.GetText("Invalid format for Guid.Guid(string)."));
			}

			private ulong ParseHex(int length, bool strictLength)
			{
				ulong num = 0UL;
				bool flag = false;
				int num2 = 0;
				while (!flag && num2 < length)
				{
					if (this.AtEnd())
					{
						if (strictLength || num2 == 0)
						{
							this.ThrowFormatException();
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						char c = char.ToLowerInvariant(this._src[this._cur]);
						if (char.IsDigit(c))
						{
							num = num * 16UL + (ulong)c - 48UL;
							this._cur++;
						}
						else if (c >= 'a' && c <= 'f')
						{
							num = num * 16UL + (ulong)c - 97UL + 10UL;
							this._cur++;
						}
						else if (strictLength || num2 == 0)
						{
							this.ThrowFormatException();
						}
						else
						{
							flag = true;
						}
					}
					num2++;
				}
				return num;
			}

			private bool ParseOptChar(char c)
			{
				if (!this.AtEnd() && this._src[this._cur] == c)
				{
					this._cur++;
					return true;
				}
				return false;
			}

			private void ParseChar(char c)
			{
				if (!this.ParseOptChar(c))
				{
					this.ThrowFormatException();
				}
			}

			private Guid ParseGuid1()
			{
				bool flag = true;
				char c = '}';
				byte[] array = new byte[8];
				bool flag2 = this.ParseOptChar('{');
				if (!flag2)
				{
					flag2 = this.ParseOptChar('(');
					if (flag2)
					{
						c = ')';
					}
				}
				int a = (int)this.ParseHex(8, true);
				if (flag2)
				{
					this.ParseChar('-');
				}
				else
				{
					flag = this.ParseOptChar('-');
				}
				short b = (short)this.ParseHex(4, true);
				if (flag)
				{
					this.ParseChar('-');
				}
				short c2 = (short)this.ParseHex(4, true);
				if (flag)
				{
					this.ParseChar('-');
				}
				for (int i = 0; i < 8; i++)
				{
					array[i] = (byte)this.ParseHex(2, true);
					if (i == 1 && flag)
					{
						this.ParseChar('-');
					}
				}
				if (flag2 && !this.ParseOptChar(c))
				{
					this.ThrowFormatException();
				}
				return new Guid(a, b, c2, array);
			}

			private void ParseHexPrefix()
			{
				this.ParseChar('0');
				this.ParseChar('x');
			}

			private Guid ParseGuid2()
			{
				byte[] array = new byte[8];
				this.ParseChar('{');
				this.ParseHexPrefix();
				int a = (int)this.ParseHex(8, false);
				this.ParseChar(',');
				this.ParseHexPrefix();
				short b = (short)this.ParseHex(4, false);
				this.ParseChar(',');
				this.ParseHexPrefix();
				short c = (short)this.ParseHex(4, false);
				this.ParseChar(',');
				this.ParseChar('{');
				for (int i = 0; i < 8; i++)
				{
					this.ParseHexPrefix();
					array[i] = (byte)this.ParseHex(2, false);
					if (i != 7)
					{
						this.ParseChar(',');
					}
				}
				this.ParseChar('}');
				this.ParseChar('}');
				return new Guid(a, b, c, array);
			}

			public Guid Parse()
			{
				Guid result;
				try
				{
					result = this.ParseGuid1();
				}
				catch (FormatException)
				{
					this.Reset();
					result = this.ParseGuid2();
				}
				if (!this.AtEnd())
				{
					this.ThrowFormatException();
				}
				return result;
			}
		}
	}
}
