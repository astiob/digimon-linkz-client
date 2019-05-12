using System;
using System.Text;

namespace System.Collections.Specialized
{
	/// <summary>Provides a simple structure that stores Boolean values and small integers in 32 bits of memory.</summary>
	public struct BitVector32
	{
		private int bits;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.BitVector32" /> structure containing the data represented in an existing <see cref="T:System.Collections.Specialized.BitVector32" /> structure.</summary>
		/// <param name="value">A <see cref="T:System.Collections.Specialized.BitVector32" /> structure that contains the data to copy. </param>
		public BitVector32(BitVector32 source)
		{
			this.bits = source.bits;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.BitVector32" /> structure containing the data represented in an integer.</summary>
		/// <param name="data">An integer representing the data of the new <see cref="T:System.Collections.Specialized.BitVector32" />. </param>
		public BitVector32(int init)
		{
			this.bits = init;
		}

		/// <summary>Gets the value of the <see cref="T:System.Collections.Specialized.BitVector32" /> as an integer.</summary>
		/// <returns>The value of the <see cref="T:System.Collections.Specialized.BitVector32" /> as an integer.</returns>
		public int Data
		{
			get
			{
				return this.bits;
			}
		}

		/// <summary>Gets or sets the value stored in the specified <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</summary>
		/// <returns>The value stored in the specified <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</returns>
		/// <param name="section">A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> that contains the value to get or set. </param>
		public int this[BitVector32.Section section]
		{
			get
			{
				return this.bits >> (int)section.Offset & (int)section.Mask;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Section can't hold negative values");
				}
				if (value > (int)section.Mask)
				{
					throw new ArgumentException("Value too large to fit in section");
				}
				this.bits &= ~((int)section.Mask << (int)section.Offset);
				this.bits |= value << (int)section.Offset;
			}
		}

		/// <summary>Gets or sets the state of the bit flag indicated by the specified mask.</summary>
		/// <returns>true if the specified bit flag is on (1); otherwise, false.</returns>
		/// <param name="bit">A mask that indicates the bit to get or set. </param>
		public bool this[int mask]
		{
			get
			{
				return (this.bits & mask) == mask;
			}
			set
			{
				if (value)
				{
					this.bits |= mask;
				}
				else
				{
					this.bits &= ~mask;
				}
			}
		}

		/// <summary>Creates the first mask in a series of masks that can be used to retrieve individual bits in a <see cref="T:System.Collections.Specialized.BitVector32" /> that is set up as bit flags.</summary>
		/// <returns>A mask that isolates the first bit flag in the <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public static int CreateMask()
		{
			return 1;
		}

		/// <summary>Creates an additional mask following the specified mask in a series of masks that can be used to retrieve individual bits in a <see cref="T:System.Collections.Specialized.BitVector32" /> that is set up as bit flags.</summary>
		/// <returns>A mask that isolates the bit flag following the one that <paramref name="previous" /> points to in <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
		/// <param name="previous">The mask that indicates the previous bit flag. </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="previous" /> indicates the last bit flag in the <see cref="T:System.Collections.Specialized.BitVector32" />. </exception>
		public static int CreateMask(int prev)
		{
			if (prev == 0)
			{
				return 1;
			}
			if (prev == -2147483648)
			{
				throw new InvalidOperationException("all bits set");
			}
			return prev << 1;
		}

		/// <summary>Creates the first <see cref="T:System.Collections.Specialized.BitVector32.Section" /> in a series of sections that contain small integers.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> that can hold a number from zero to <paramref name="maxValue" />.</returns>
		/// <param name="maxValue">A 16-bit signed integer that specifies the maximum value for the new <see cref="T:System.Collections.Specialized.BitVector32.Section" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="maxValue" /> is less than 1. </exception>
		public static BitVector32.Section CreateSection(short maxValue)
		{
			return BitVector32.CreateSection(maxValue, new BitVector32.Section(0, 0));
		}

		/// <summary>Creates a new <see cref="T:System.Collections.Specialized.BitVector32.Section" /> following the specified <see cref="T:System.Collections.Specialized.BitVector32.Section" /> in a series of sections that contain small integers.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> that can hold a number from zero to <paramref name="maxValue" />.</returns>
		/// <param name="maxValue">A 16-bit signed integer that specifies the maximum value for the new <see cref="T:System.Collections.Specialized.BitVector32.Section" />. </param>
		/// <param name="previous">The previous <see cref="T:System.Collections.Specialized.BitVector32.Section" /> in the <see cref="T:System.Collections.Specialized.BitVector32" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="maxValue" /> is less than 1. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="previous" /> includes the final bit in the <see cref="T:System.Collections.Specialized.BitVector32" />.-or- <paramref name="maxValue" /> is greater than the highest value that can be represented by the number of bits after <paramref name="previous" />. </exception>
		public static BitVector32.Section CreateSection(short maxValue, BitVector32.Section previous)
		{
			if (maxValue < 1)
			{
				throw new ArgumentException("maxValue");
			}
			int num = BitVector32.HighestSetBit((int)maxValue);
			int num2 = (1 << num) - 1;
			int num3 = (int)previous.Offset + BitVector32.HighestSetBit((int)previous.Mask);
			if (num3 + num > 32)
			{
				throw new ArgumentException("Sections cannot exceed 32 bits in total");
			}
			return new BitVector32.Section((short)num2, (short)num3);
		}

		/// <summary>Determines whether the specified object is equal to the <see cref="T:System.Collections.Specialized.BitVector32" />.</summary>
		/// <returns>true if the specified object is equal to the <see cref="T:System.Collections.Specialized.BitVector32" />; otherwise, false.</returns>
		/// <param name="o">The object to compare with the current <see cref="T:System.Collections.Specialized.BitVector32" />. </param>
		public override bool Equals(object o)
		{
			return o is BitVector32 && this.bits == ((BitVector32)o).bits;
		}

		/// <summary>Serves as a hash function for the <see cref="T:System.Collections.Specialized.BitVector32" />.</summary>
		/// <returns>A hash code for the <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
		public override int GetHashCode()
		{
			return this.bits.GetHashCode();
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Collections.Specialized.BitVector32" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
		public override string ToString()
		{
			return BitVector32.ToString(this);
		}

		/// <summary>Returns a string that represents the specified <see cref="T:System.Collections.Specialized.BitVector32" />.</summary>
		/// <returns>A string that represents the specified <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
		/// <param name="value">The <see cref="T:System.Collections.Specialized.BitVector32" /> to represent. </param>
		public static string ToString(BitVector32 value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BitVector32{");
			for (long num = (long)((ulong)int.MinValue); num > 0L; num >>= 1)
			{
				stringBuilder.Append((((long)value.bits & num) != 0L) ? '1' : '0');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		private static int HighestSetBit(int i)
		{
			int num = 0;
			while (i >> num != 0)
			{
				num++;
			}
			return num;
		}

		/// <summary>Represents a section of the vector that can contain an integer number.</summary>
		public struct Section
		{
			private short mask;

			private short offset;

			internal Section(short mask, short offset)
			{
				this.mask = mask;
				this.offset = offset;
			}

			/// <summary>Gets a mask that isolates this section within the <see cref="T:System.Collections.Specialized.BitVector32" />.</summary>
			/// <returns>A mask that isolates this section within the <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
			public short Mask
			{
				get
				{
					return this.mask;
				}
			}

			/// <summary>Gets the offset of this section from the start of the <see cref="T:System.Collections.Specialized.BitVector32" />.</summary>
			/// <returns>The offset of this section from the start of the <see cref="T:System.Collections.Specialized.BitVector32" />.</returns>
			public short Offset
			{
				get
				{
					return this.offset;
				}
			}

			/// <summary>Determines whether the specified <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object is the same as the current <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</summary>
			/// <returns>true if the <paramref name="obj" /> parameter is the same as the current <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object; otherwise false.</returns>
			/// <param name="obj">The <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object to compare with the current <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</param>
			public bool Equals(BitVector32.Section obj)
			{
				return this.mask == obj.mask && this.offset == obj.offset;
			}

			/// <summary>Determines whether the specified object is the same as the current <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</summary>
			/// <returns>true if the specified object is the same as the current <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object; otherwise, false.</returns>
			/// <param name="o">The object to compare with the current <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</param>
			public override bool Equals(object o)
			{
				if (!(o is BitVector32.Section))
				{
					return false;
				}
				BitVector32.Section section = (BitVector32.Section)o;
				return this.mask == section.mask && this.offset == section.offset;
			}

			/// <summary>Serves as a hash function for the current <see cref="T:System.Collections.Specialized.BitVector32.Section" />, suitable for hashing algorithms and data structures, such as a hash table.</summary>
			/// <returns>A hash code for the current <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</returns>
			public override int GetHashCode()
			{
				return (int)this.mask << (int)this.offset;
			}

			/// <summary>Returns a string that represents the current <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</summary>
			/// <returns>A string that represents the current <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</returns>
			public override string ToString()
			{
				return BitVector32.Section.ToString(this);
			}

			/// <summary>Returns a string that represents the specified <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</summary>
			/// <returns>A string that represents the specified <see cref="T:System.Collections.Specialized.BitVector32.Section" />.</returns>
			/// <param name="value">The <see cref="T:System.Collections.Specialized.BitVector32.Section" /> to represent.</param>
			public static string ToString(BitVector32.Section value)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Section{0x");
				stringBuilder.Append(Convert.ToString(value.Mask, 16));
				stringBuilder.Append(", 0x");
				stringBuilder.Append(Convert.ToString(value.Offset, 16));
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}

			/// <summary>Determines whether two specified <see cref="T:System.Collections.Specialized.BitVector32.Section" /> objects are equal.</summary>
			/// <returns>true if the <paramref name="a" /> and <paramref name="b" /> parameters represent the same <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object, otherwise, false.</returns>
			/// <param name="a">A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</param>
			/// <param name="b">A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</param>
			public static bool operator ==(BitVector32.Section v1, BitVector32.Section v2)
			{
				return v1.mask == v2.mask && v1.offset == v2.offset;
			}

			/// <summary>Determines whether two <see cref="T:System.Collections.Specialized.BitVector32.Section" /> objects have different values.</summary>
			/// <returns>true if the <paramref name="a" /> and <paramref name="b" /> parameters represent different <see cref="T:System.Collections.Specialized.BitVector32.Section" /> objects; otherwise, false.</returns>
			/// <param name="a">A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</param>
			/// <param name="b">A <see cref="T:System.Collections.Specialized.BitVector32.Section" /> object.</param>
			public static bool operator !=(BitVector32.Section v1, BitVector32.Section v2)
			{
				return v1.mask != v2.mask || v1.offset != v2.offset;
			}
		}
	}
}
