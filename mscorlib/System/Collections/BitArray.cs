using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Manages a compact array of bit values, which are represented as Booleans, where true indicates that the bit is on (1) and false indicates the bit is off (0).</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class BitArray : IEnumerable, ICloneable, ICollection
	{
		private int[] m_array;

		private int m_length;

		private int _version;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.BitArray" /> class that contains bit values copied from the specified <see cref="T:System.Collections.BitArray" />.</summary>
		/// <param name="bits">The <see cref="T:System.Collections.BitArray" /> to copy. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bits" /> is null. </exception>
		public BitArray(BitArray bits)
		{
			if (bits == null)
			{
				throw new ArgumentNullException("bits");
			}
			this.m_length = bits.m_length;
			this.m_array = new int[(this.m_length + 31) / 32];
			if (this.m_array.Length == 1)
			{
				this.m_array[0] = bits.m_array[0];
			}
			else
			{
				Array.Copy(bits.m_array, this.m_array, this.m_array.Length);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.BitArray" /> class that contains bit values copied from the specified array of Booleans.</summary>
		/// <param name="values">An array of Booleans to copy. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is null. </exception>
		public BitArray(bool[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.m_length = values.Length;
			this.m_array = new int[(this.m_length + 31) / 32];
			for (int i = 0; i < values.Length; i++)
			{
				this[i] = values[i];
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.BitArray" /> class that contains bit values copied from the specified array of bytes. </summary>
		/// <param name="bytes">An array of bytes containing the values to copy, where each byte represents eight consecutive bits. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="bytes" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		public BitArray(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.m_length = bytes.Length * 8;
			this.m_array = new int[(this.m_length + 31) / 32];
			for (int i = 0; i < bytes.Length; i++)
			{
				this.setByte(i, bytes[i]);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.BitArray" /> class that contains bit values copied from the specified array of 32-bit integers.</summary>
		/// <param name="values">An array of integers containing the values to copy, where each integer represents 32 consecutive bits. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="values" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="values" /> is greater than <see cref="F:System.Int32.MaxValue" /></exception>
		public BitArray(int[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			int num = values.Length;
			this.m_length = num * 32;
			this.m_array = new int[num];
			Array.Copy(values, this.m_array, num);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.BitArray" /> class that can hold the specified number of bit values, which are initially set to false.</summary>
		/// <param name="length">The number of bit values in the new <see cref="T:System.Collections.BitArray" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than zero. </exception>
		public BitArray(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			this.m_length = length;
			this.m_array = new int[(this.m_length + 31) / 32];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.BitArray" /> class that can hold the specified number of bit values, which are initially set to the specified value.</summary>
		/// <param name="length">The number of bit values in the new <see cref="T:System.Collections.BitArray" />. </param>
		/// <param name="defaultValue">The Boolean value to assign to each bit. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than zero. </exception>
		public BitArray(int length, bool defaultValue) : this(length)
		{
			if (defaultValue)
			{
				for (int i = 0; i < this.m_array.Length; i++)
				{
					this.m_array[i] = -1;
				}
			}
		}

		private BitArray(int[] array, int length)
		{
			this.m_array = array;
			this.m_length = length;
		}

		private byte getByte(int byteIndex)
		{
			int num = byteIndex / 4;
			int num2 = byteIndex % 4 * 8;
			int num3 = this.m_array[num] & 255 << num2;
			return (byte)(num3 >> num2 & 255);
		}

		private void setByte(int byteIndex, byte value)
		{
			int num = byteIndex / 4;
			int num2 = byteIndex % 4 * 8;
			this.m_array[num] &= ~(255 << num2);
			this.m_array[num] |= (int)value << num2;
			this._version++;
		}

		private void checkOperand(BitArray operand)
		{
			if (operand == null)
			{
				throw new ArgumentNullException();
			}
			if (operand.m_length != this.m_length)
			{
				throw new ArgumentException();
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.BitArray" />.</returns>
		/// <filterpriority>2</filterpriority>
		public int Count
		{
			get
			{
				return this.m_length;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.BitArray" /> is read-only.</summary>
		/// <returns>This property is always false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.BitArray" /> is synchronized (thread safe).</summary>
		/// <returns>This property is always false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the value of the bit at a specific position in the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>The value of the bit at position <paramref name="index" />.</returns>
		/// <param name="index">The zero-based index of the value to get or set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.BitArray.Count" />. </exception>
		/// <filterpriority>2</filterpriority>
		public bool this[int index]
		{
			get
			{
				return this.Get(index);
			}
			set
			{
				this.Set(index, value);
			}
		}

		/// <summary>Gets or sets the number of elements in the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>The number of elements in the <see cref="T:System.Collections.BitArray" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is set to a value that is less than zero. </exception>
		/// <filterpriority>2</filterpriority>
		public int Length
		{
			get
			{
				return this.m_length;
			}
			set
			{
				if (this.m_length == value)
				{
					return;
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (value > this.m_length)
				{
					int num = (value + 31) / 32;
					int num2 = (this.m_length + 31) / 32;
					if (num > this.m_array.Length)
					{
						int[] array = new int[num];
						Array.Copy(this.m_array, array, this.m_array.Length);
						this.m_array = array;
					}
					else
					{
						Array.Clear(this.m_array, num2, num - num2);
					}
					int num3 = this.m_length % 32;
					if (num3 > 0)
					{
						this.m_array[num2 - 1] &= (1 << num3) - 1;
					}
				}
				this.m_length = value;
				this._version++;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.BitArray" />.</returns>
		/// <filterpriority>2</filterpriority>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Collections.BitArray" />.</returns>
		/// <filterpriority>2</filterpriority>
		public object Clone()
		{
			return new BitArray(this);
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.BitArray" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.BitArray" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.BitArray" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.BitArray" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("array", "Array rank must be 1");
			}
			if (index >= array.Length && this.m_length > 0)
			{
				throw new ArgumentException("index", "index is greater than array.Length");
			}
			if (array is bool[])
			{
				if (array.Length - index < this.m_length)
				{
					throw new ArgumentException();
				}
				bool[] array2 = (bool[])array;
				for (int i = 0; i < this.m_length; i++)
				{
					array2[index + i] = this[i];
				}
			}
			else if (array is byte[])
			{
				int num = (this.m_length + 7) / 8;
				if (array.Length - index < num)
				{
					throw new ArgumentException();
				}
				byte[] array3 = (byte[])array;
				for (int j = 0; j < num; j++)
				{
					array3[index + j] = this.getByte(j);
				}
			}
			else
			{
				if (!(array is int[]))
				{
					throw new ArgumentException("array", "Unsupported type");
				}
				Array.Copy(this.m_array, 0, array, index, (this.m_length + 31) / 32);
			}
		}

		/// <summary>Inverts all the bit values in the current <see cref="T:System.Collections.BitArray" />, so that elements set to true are changed to false, and elements set to false are changed to true.</summary>
		/// <returns>The current instance with inverted bit values.</returns>
		/// <filterpriority>2</filterpriority>
		public BitArray Not()
		{
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] = ~this.m_array[i];
			}
			this._version++;
			return this;
		}

		/// <summary>Performs the bitwise AND operation on the elements in the current <see cref="T:System.Collections.BitArray" /> against the corresponding elements in the specified <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>A <see cref="T:System.Collections.BitArray" /> containing the result of the bitwise AND operation on the elements in the current <see cref="T:System.Collections.BitArray" /> against the corresponding elements in the specified <see cref="T:System.Collections.BitArray" />.</returns>
		/// <param name="value">The <see cref="T:System.Collections.BitArray" /> with which to perform the bitwise AND operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> and the current <see cref="T:System.Collections.BitArray" /> do not have the same number of elements. </exception>
		/// <filterpriority>2</filterpriority>
		public BitArray And(BitArray value)
		{
			this.checkOperand(value);
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] &= value.m_array[i];
			}
			this._version++;
			return this;
		}

		/// <summary>Performs the bitwise OR operation on the elements in the current <see cref="T:System.Collections.BitArray" /> against the corresponding elements in the specified <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>A <see cref="T:System.Collections.BitArray" /> containing the result of the bitwise OR operation on the elements in the current <see cref="T:System.Collections.BitArray" /> against the corresponding elements in the specified <see cref="T:System.Collections.BitArray" />.</returns>
		/// <param name="value">The <see cref="T:System.Collections.BitArray" /> with which to perform the bitwise OR operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> and the current <see cref="T:System.Collections.BitArray" /> do not have the same number of elements. </exception>
		/// <filterpriority>2</filterpriority>
		public BitArray Or(BitArray value)
		{
			this.checkOperand(value);
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] |= value.m_array[i];
			}
			this._version++;
			return this;
		}

		/// <summary>Performs the bitwise exclusive OR operation on the elements in the current <see cref="T:System.Collections.BitArray" /> against the corresponding elements in the specified <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>A <see cref="T:System.Collections.BitArray" /> containing the result of the bitwise exclusive OR operation on the elements in the current <see cref="T:System.Collections.BitArray" /> against the corresponding elements in the specified <see cref="T:System.Collections.BitArray" />.</returns>
		/// <param name="value">The <see cref="T:System.Collections.BitArray" /> with which to perform the bitwise exclusive OR operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> and the current <see cref="T:System.Collections.BitArray" /> do not have the same number of elements. </exception>
		/// <filterpriority>2</filterpriority>
		public BitArray Xor(BitArray value)
		{
			this.checkOperand(value);
			int num = (this.m_length + 31) / 32;
			for (int i = 0; i < num; i++)
			{
				this.m_array[i] ^= value.m_array[i];
			}
			this._version++;
			return this;
		}

		/// <summary>Gets the value of the bit at a specific position in the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>The value of the bit at position <paramref name="index" />.</returns>
		/// <param name="index">The zero-based index of the value to get. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than or equal to the number of elements in the <see cref="T:System.Collections.BitArray" />. </exception>
		/// <filterpriority>2</filterpriority>
		public bool Get(int index)
		{
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (this.m_array[index >> 5] & 1 << index) != 0;
		}

		/// <summary>Sets the bit at a specific position in the <see cref="T:System.Collections.BitArray" /> to the specified value.</summary>
		/// <param name="index">The zero-based index of the bit to set. </param>
		/// <param name="value">The Boolean value to assign to the bit. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than or equal to the number of elements in the <see cref="T:System.Collections.BitArray" />. </exception>
		/// <filterpriority>2</filterpriority>
		public void Set(int index, bool value)
		{
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (value)
			{
				this.m_array[index >> 5] |= 1 << index;
			}
			else
			{
				this.m_array[index >> 5] &= ~(1 << index);
			}
			this._version++;
		}

		/// <summary>Sets all bits in the <see cref="T:System.Collections.BitArray" /> to the specified value.</summary>
		/// <param name="value">The Boolean value to assign to all bits. </param>
		/// <filterpriority>2</filterpriority>
		public void SetAll(bool value)
		{
			if (value)
			{
				for (int i = 0; i < this.m_array.Length; i++)
				{
					this.m_array[i] = -1;
				}
			}
			else
			{
				Array.Clear(this.m_array, 0, this.m_array.Length);
			}
			this._version++;
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.BitArray" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the entire <see cref="T:System.Collections.BitArray" />.</returns>
		/// <filterpriority>2</filterpriority>
		public IEnumerator GetEnumerator()
		{
			return new BitArray.BitArrayEnumerator(this);
		}

		[Serializable]
		private class BitArrayEnumerator : IEnumerator, ICloneable
		{
			private BitArray _bitArray;

			private bool _current;

			private int _index;

			private int _version;

			public BitArrayEnumerator(BitArray ba)
			{
				this._index = -1;
				this._bitArray = ba;
				this._version = ba._version;
			}

			public object Clone()
			{
				return base.MemberwiseClone();
			}

			public object Current
			{
				get
				{
					if (this._index == -1)
					{
						throw new InvalidOperationException("Enum not started");
					}
					if (this._index >= this._bitArray.Count)
					{
						throw new InvalidOperationException("Enum Ended");
					}
					return this._current;
				}
			}

			public bool MoveNext()
			{
				this.checkVersion();
				if (this._index < this._bitArray.Count - 1)
				{
					this._current = this._bitArray[++this._index];
					return true;
				}
				this._index = this._bitArray.Count;
				return false;
			}

			public void Reset()
			{
				this.checkVersion();
				this._index = -1;
			}

			private void checkVersion()
			{
				if (this._version != this._bitArray._version)
				{
					throw new InvalidOperationException();
				}
			}
		}
	}
}
