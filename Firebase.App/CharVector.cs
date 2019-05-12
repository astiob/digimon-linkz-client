using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class CharVector : IDisposable, IEnumerable, IList<byte>, ICollection<byte>, IEnumerable<byte>
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal CharVector(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public CharVector(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			IEnumerator enumerator = c.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					byte x = (byte)obj;
					this.Add(x);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public CharVector() : this(AppUtilPINVOKE.new_CharVector__SWIG_0(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CharVector(CharVector other) : this(AppUtilPINVOKE.new_CharVector__SWIG_1(CharVector.getCPtr(other)), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CharVector(int capacity) : this(AppUtilPINVOKE.new_CharVector__SWIG_2(capacity), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(CharVector obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~CharVector()
		{
			this.Dispose();
		}

		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AppUtilPINVOKE.delete_CharVector(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public byte this[int index]
		{
			get
			{
				return this.getitem(index);
			}
			set
			{
				this.setitem(index, value);
			}
		}

		public int Capacity
		{
			get
			{
				return (int)this.capacity();
			}
			set
			{
				if ((long)value < (long)((ulong)this.size()))
				{
					throw new ArgumentOutOfRangeException("Capacity");
				}
				this.reserve((uint)value);
			}
		}

		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public void CopyTo(byte[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		public void CopyTo(byte[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		public void CopyTo(int index, byte[] array, int arrayIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Value is less than zero");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Value is less than zero");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("Multi dimensional array.", "array");
			}
			if (index + count > this.Count || arrayIndex + count > array.Length)
			{
				throw new ArgumentException("Number of elements to copy is too large.");
			}
			for (int i = 0; i < count; i++)
			{
				array.SetValue(this.getitemcopy(index + i), arrayIndex + i);
			}
		}

		IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
		{
			return new CharVector.CharVectorEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CharVector.CharVectorEnumerator(this);
		}

		public CharVector.CharVectorEnumerator GetEnumerator()
		{
			return new CharVector.CharVectorEnumerator(this);
		}

		public void Clear()
		{
			AppUtilPINVOKE.CharVector_Clear(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Add(byte x)
		{
			AppUtilPINVOKE.CharVector_Add(this.swigCPtr, x);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private uint size()
		{
			uint result = AppUtilPINVOKE.CharVector_size(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private uint capacity()
		{
			uint result = AppUtilPINVOKE.CharVector_capacity(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void reserve(uint n)
		{
			AppUtilPINVOKE.CharVector_reserve(this.swigCPtr, n);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private byte getitemcopy(int index)
		{
			byte result = AppUtilPINVOKE.CharVector_getitemcopy(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private byte getitem(int index)
		{
			byte result = AppUtilPINVOKE.CharVector_getitem(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void setitem(int index, byte val)
		{
			AppUtilPINVOKE.CharVector_setitem(this.swigCPtr, index, val);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void AddRange(CharVector values)
		{
			AppUtilPINVOKE.CharVector_AddRange(this.swigCPtr, CharVector.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public CharVector GetRange(int index, int count)
		{
			IntPtr intPtr = AppUtilPINVOKE.CharVector_GetRange(this.swigCPtr, index, count);
			CharVector result = (!(intPtr == IntPtr.Zero)) ? new CharVector(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Insert(int index, byte x)
		{
			AppUtilPINVOKE.CharVector_Insert(this.swigCPtr, index, x);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void InsertRange(int index, CharVector values)
		{
			AppUtilPINVOKE.CharVector_InsertRange(this.swigCPtr, index, CharVector.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RemoveAt(int index)
		{
			AppUtilPINVOKE.CharVector_RemoveAt(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RemoveRange(int index, int count)
		{
			AppUtilPINVOKE.CharVector_RemoveRange(this.swigCPtr, index, count);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static CharVector Repeat(byte value, int count)
		{
			IntPtr intPtr = AppUtilPINVOKE.CharVector_Repeat(value, count);
			CharVector result = (!(intPtr == IntPtr.Zero)) ? new CharVector(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Reverse()
		{
			AppUtilPINVOKE.CharVector_Reverse__SWIG_0(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Reverse(int index, int count)
		{
			AppUtilPINVOKE.CharVector_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetRange(int index, CharVector values)
		{
			AppUtilPINVOKE.CharVector_SetRange(this.swigCPtr, index, CharVector.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool Contains(byte value)
		{
			bool result = AppUtilPINVOKE.CharVector_Contains(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public int IndexOf(byte value)
		{
			int result = AppUtilPINVOKE.CharVector_IndexOf(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public int LastIndexOf(byte value)
		{
			int result = AppUtilPINVOKE.CharVector_LastIndexOf(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool Remove(byte value)
		{
			bool result = AppUtilPINVOKE.CharVector_Remove(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public sealed class CharVectorEnumerator : IEnumerator, IEnumerator<byte>, IDisposable
		{
			private CharVector collectionRef;

			private int currentIndex;

			private object currentObject;

			private int currentSize;

			public CharVectorEnumerator(CharVector collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			public byte Current
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException("Enumeration not started.");
					}
					if (this.currentIndex > this.currentSize - 1)
					{
						throw new InvalidOperationException("Enumeration finished.");
					}
					if (this.currentObject == null)
					{
						throw new InvalidOperationException("Collection modified.");
					}
					return (byte)this.currentObject;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			public bool MoveNext()
			{
				int count = this.collectionRef.Count;
				bool flag = this.currentIndex + 1 < count && count == this.currentSize;
				if (flag)
				{
					this.currentIndex++;
					this.currentObject = this.collectionRef[this.currentIndex];
				}
				else
				{
					this.currentObject = null;
				}
				return flag;
			}

			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}
		}
	}
}
