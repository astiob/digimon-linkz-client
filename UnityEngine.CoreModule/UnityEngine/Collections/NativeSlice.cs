using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.Collections
{
	[NativeContainer]
	[DebuggerTypeProxy(typeof(NativeSliceDebugView<>))]
	[DebuggerDisplay("Length = {Length}")]
	[NativeContainerSupportsMinMaxWriteRestriction]
	public struct NativeSlice<T> : IEnumerable<T>, IEnumerable where T : struct
	{
		private IntPtr m_Buffer;

		private int m_Stride;

		private int m_Length;

		public NativeSlice(NativeArray<T> array)
		{
			this = new NativeSlice<T>(array, 0, array.Length);
		}

		public NativeSlice(NativeArray<T> array, int start)
		{
			this = new NativeSlice<T>(array, start, array.Length - start);
		}

		public unsafe NativeSlice(NativeArray<T> array, int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentException("Slice start range must be >= 0.");
			}
			if (length < 0)
			{
				throw new ArgumentException("Slice length must be >= 0.");
			}
			if (start + length > array.Length)
			{
				throw new ArgumentException("Slice start + length range must be <= array.Length");
			}
			this.m_Stride = UnsafeUtility.SizeOf<T>();
			byte* value = (byte*)((void*)array.m_Buffer) + this.m_Stride * start;
			this.m_Buffer = (IntPtr)((void*)value);
			this.m_Length = length;
		}

		public NativeSlice<U> SliceConvert<U>() where U : struct
		{
			if (this.m_Stride != UnsafeUtility.SizeOf<T>())
			{
				throw new ArgumentException("SliceConvert requires that stride matches the size of the source type");
			}
			NativeSlice<U> result;
			result.m_Buffer = this.m_Buffer;
			int num = UnsafeUtility.SizeOf<U>();
			result.m_Stride = num;
			if (this.m_Stride * this.m_Length % num != 0)
			{
				throw new ArgumentException("SliceConvert requires that Length * Stride is a multiple of sizeof(U).");
			}
			result.m_Length = this.m_Length * this.m_Stride / num;
			return result;
		}

		public unsafe NativeSlice<U> SliceWithStride<U>(int offset) where U : struct
		{
			if (offset < 0)
			{
				throw new ArgumentException("SliceWithStride offset must be >= 0");
			}
			if (offset + UnsafeUtility.SizeOf<U>() > UnsafeUtility.SizeOf<T>())
			{
				throw new ArgumentException("SliceWithStride sizeof(U) + offset must be <= sizeof(T)");
			}
			byte* value = (byte*)((void*)this.m_Buffer) + offset;
			NativeSlice<U> result;
			result.m_Buffer = (IntPtr)((void*)value);
			result.m_Stride = this.m_Stride;
			result.m_Length = this.m_Length;
			return result;
		}

		public NativeSlice<U> SliceWithStride<U>() where U : struct
		{
			return this.SliceWithStride<U>(0);
		}

		public NativeSlice<U> SliceWithStride<U>(string offsetFieldName) where U : struct
		{
			int offset = UnsafeUtility.OffsetOf<T>(offsetFieldName);
			return this.SliceWithStride<U>(offset);
		}

		public T this[int index]
		{
			get
			{
				if (index >= this.m_Length)
				{
					this.FailOutOfRangeError(index);
				}
				return UnsafeUtility.ReadArrayElementWithStride<T>(this.m_Buffer, index, this.m_Stride);
			}
			set
			{
				if (index >= this.m_Length)
				{
					this.FailOutOfRangeError(index);
				}
				UnsafeUtility.WriteArrayElementWithStride<T>(this.m_Buffer, index, this.m_Stride, value);
			}
		}

		private void FailOutOfRangeError(int index)
		{
			throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, this.Length));
		}

		public void CopyFrom(NativeSlice<T> slice)
		{
			if (this.Length != slice.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match NativeSlice.Length({1}).", slice.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				this[num] = slice[num];
			}
		}

		public void CopyFrom(T[] array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match NativeSlice.Length({1}).", array.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				this[num] = array[num];
			}
		}

		public void CopyTo(NativeArray<T> array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match NativeSlice.Length({1}).", array.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				array[num] = this[num];
			}
		}

		public void CopyTo(T[] array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match NativeSlice.Length({1}).", array.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				array[num] = this[num];
			}
		}

		public T[] ToArray()
		{
			T[] array = new T[this.Length];
			this.CopyTo(array);
			return array;
		}

		public int Stride
		{
			get
			{
				return this.m_Stride;
			}
		}

		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		public IntPtr UnsafePtr
		{
			get
			{
				return this.m_Buffer;
			}
		}

		public IntPtr UnsafeReadOnlyPtr
		{
			get
			{
				return this.m_Buffer;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new NativeSlice<T>.Enumerator(ref this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private NativeSlice<T> array;

			private int index;

			public Enumerator(ref NativeSlice<T> array)
			{
				this.array = array;
				this.index = -1;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				this.index++;
				return this.index < this.array.Length;
			}

			public void Reset()
			{
				this.index = -1;
			}

			public T Current
			{
				get
				{
					return this.array[this.index];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}
		}
	}
}
