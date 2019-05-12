using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace UnityEngine.Collections
{
	[NativeContainer]
	[NativeContainerSupportsMinMaxWriteRestriction]
	[NativeContainerSupportsDeallocateOnJobCompletion]
	[DebuggerDisplay("Length = {Length}")]
	[DebuggerTypeProxy(typeof(NativeArrayDebugView<>))]
	public struct NativeArray<T> : IDisposable, IEnumerable<T>, IEnumerable where T : struct
	{
		internal IntPtr m_Buffer;

		internal int m_Length;

		private Allocator m_AllocatorLabel;

		public NativeArray(int length, Allocator allocMode)
		{
			NativeArray<T>.Allocate(length, allocMode, out this);
			UnsafeUtility.MemClear(this.m_Buffer, this.Length * UnsafeUtility.SizeOf<T>());
		}

		public NativeArray(T[] array, Allocator allocMode)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			NativeArray<T>.Allocate(array.Length, allocMode, out this);
			this.CopyFrom(array);
		}

		public NativeArray(NativeArray<T> array, Allocator allocMode)
		{
			NativeArray<T>.Allocate(array.Length, allocMode, out this);
			this.CopyFrom(array);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static NativeArray<T> ConvertExistingDataToNativeArrayInternal(IntPtr dataPointer, int length, AtomicSafetyHandle safety, Allocator allocMode)
		{
			return new NativeArray<T>
			{
				m_Buffer = dataPointer,
				m_Length = length,
				m_AllocatorLabel = allocMode
			};
		}

		private static void Allocate(int length, Allocator allocator, out NativeArray<T> array)
		{
			if (allocator <= Allocator.None)
			{
				throw new ArgumentOutOfRangeException("allocMode", "Allocator must be Temp, Job or Persistent");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be >= 0");
			}
			long num = (long)UnsafeUtility.SizeOf<T>() * (long)length;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("length", "Length * sizeof(T) cannot exceed " + int.MaxValue + "bytes");
			}
			array.m_Buffer = UnsafeUtility.Malloc((int)num, UnsafeUtility.AlignOf<T>(), allocator);
			array.m_Length = length;
			array.m_AllocatorLabel = allocator;
		}

		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		public T this[int index]
		{
			get
			{
				if (index >= this.m_Length)
				{
					this.FailOutOfRangeError(index);
				}
				return UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, index);
			}
			set
			{
				if (index >= this.m_Length)
				{
					this.FailOutOfRangeError(index);
				}
				UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, index, value);
			}
		}

		public bool IsCreated
		{
			get
			{
				return this.m_Buffer != IntPtr.Zero;
			}
		}

		public void Dispose()
		{
			UnsafeUtility.Free(this.m_Buffer, this.m_AllocatorLabel);
			this.m_Buffer = IntPtr.Zero;
			this.m_Length = 0;
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

		internal void GetUnsafeBufferPointerWithoutChecksInternal(out AtomicSafetyHandle handle, out IntPtr ptr)
		{
			ptr = this.m_Buffer;
			handle = default(AtomicSafetyHandle);
		}

		public void CopyFrom(T[] array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException("Array length does not match the length of this instance");
			}
			for (int i = 0; i < this.Length; i++)
			{
				UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, i, array[i]);
			}
		}

		public void CopyFrom(NativeArray<T> array)
		{
			array.CopyTo(this);
		}

		public void CopyTo(T[] array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException("Array length does not match the length of this instance");
			}
			for (int i = 0; i < this.Length; i++)
			{
				array[i] = UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, i);
			}
		}

		public void CopyTo(NativeArray<T> array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException("Array length does not match the length of this instance");
			}
			UnsafeUtility.MemCpy(array.m_Buffer, this.m_Buffer, this.Length * UnsafeUtility.SizeOf<T>());
		}

		public T[] ToArray()
		{
			T[] array = new T[this.Length];
			this.CopyTo(array);
			return array;
		}

		private void FailOutOfRangeError(int index)
		{
			throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, this.Length));
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new NativeArray<T>.Enumerator(ref this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private NativeArray<T> array;

			private int index;

			public Enumerator(ref NativeArray<T> array)
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
