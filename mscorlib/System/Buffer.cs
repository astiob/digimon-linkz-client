using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Manipulates arrays of primitive types.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public static class Buffer
	{
		/// <summary>Returns the number of bytes in the specified array.</summary>
		/// <returns>The number of bytes in the array.</returns>
		/// <param name="array">An array. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not a primitive. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ByteLength(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = Buffer.ByteLengthInternal(array);
			if (num < 0)
			{
				throw new ArgumentException(Locale.GetText("Object must be an array of primitives."));
			}
			return num;
		}

		/// <summary>Retrieves the byte at a specified location in a specified array.</summary>
		/// <returns>Returns the <paramref name="index" /> byte in the array.</returns>
		/// <param name="array">An array. </param>
		/// <param name="index">A location in the array. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not a primitive. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is negative or greater than the length of <paramref name="array" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte GetByte(Array array, int index)
		{
			if (index < 0 || index >= Buffer.ByteLength(array))
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value must be non-negative and less than the size of the collection."));
			}
			return Buffer.GetByteInternal(array, index);
		}

		/// <summary>Assigns a specified value to a byte at a particular location in a specified array.</summary>
		/// <param name="array">An array. </param>
		/// <param name="index">A location in the array. </param>
		/// <param name="value">A value to assign. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not a primitive. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is negative or greater than the length of <paramref name="array" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static void SetByte(Array array, int index, byte value)
		{
			if (index < 0 || index >= Buffer.ByteLength(array))
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value must be non-negative and less than the size of the collection."));
			}
			Buffer.SetByteInternal(array, index, (int)value);
		}

		/// <summary>Copies a specified number of bytes from a source array starting at a particular offset to a destination array starting at a particular offset.</summary>
		/// <param name="src">The source buffer. </param>
		/// <param name="srcOffset">The zero-based byte offset into <paramref name="src" />. </param>
		/// <param name="dst">The destination buffer. </param>
		/// <param name="dstOffset">The zero-based byte offset into <paramref name="dst" />. </param>
		/// <param name="count">The number of bytes to copy. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="src" /> or <paramref name="dst" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="src" /> or <paramref name="dst" /> is not an array of primitives.-or- The length of <paramref name="src" /> is less than <paramref name="srcOffset" /> plus <paramref name="count" />.-or- The length of <paramref name="dst" /> is less than <paramref name="dstOffset" /> plus <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="srcOffset" />, <paramref name="dstOffset" />, or <paramref name="count" /> is less than 0. </exception>
		/// <filterpriority>1</filterpriority>
		public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
		{
			if (src == null)
			{
				throw new ArgumentNullException("src");
			}
			if (dst == null)
			{
				throw new ArgumentNullException("dst");
			}
			if (srcOffset < 0)
			{
				throw new ArgumentOutOfRangeException("srcOffset", Locale.GetText("Non-negative number required."));
			}
			if (dstOffset < 0)
			{
				throw new ArgumentOutOfRangeException("dstOffset", Locale.GetText("Non-negative number required."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Locale.GetText("Non-negative number required."));
			}
			if (!Buffer.BlockCopyInternal(src, srcOffset, dst, dstOffset, count) && (srcOffset > Buffer.ByteLength(src) - count || dstOffset > Buffer.ByteLength(dst) - count))
			{
				throw new ArgumentException(Locale.GetText("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ByteLengthInternal(Array array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte GetByteInternal(Array array, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetByteInternal(Array array, int index, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool BlockCopyInternal(Array src, int src_offset, Array dest, int dest_offset, int count);
	}
}
