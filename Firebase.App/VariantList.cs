using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class VariantList : IDisposable, IEnumerable, IEnumerable<Variant>
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal VariantList(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public VariantList(ICollection c) : this()
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
					Variant x = (Variant)obj;
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

		public VariantList() : this(AppUtilPINVOKE.new_VariantList__SWIG_0(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public VariantList(VariantList other) : this(AppUtilPINVOKE.new_VariantList__SWIG_1(VariantList.getCPtr(other)), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public VariantList(int capacity) : this(AppUtilPINVOKE.new_VariantList__SWIG_2(capacity), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(VariantList obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~VariantList()
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
						AppUtilPINVOKE.delete_VariantList(this.swigCPtr);
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

		public Variant this[int index]
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

		public void CopyTo(Variant[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		public void CopyTo(Variant[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		public void CopyTo(int index, Variant[] array, int arrayIndex, int count)
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

		IEnumerator<Variant> IEnumerable<Variant>.GetEnumerator()
		{
			return new VariantList.VariantListEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new VariantList.VariantListEnumerator(this);
		}

		public VariantList.VariantListEnumerator GetEnumerator()
		{
			return new VariantList.VariantListEnumerator(this);
		}

		public void Clear()
		{
			AppUtilPINVOKE.VariantList_Clear(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Add(Variant x)
		{
			AppUtilPINVOKE.VariantList_Add(this.swigCPtr, Variant.getCPtr(x));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private uint size()
		{
			uint result = AppUtilPINVOKE.VariantList_size(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private uint capacity()
		{
			uint result = AppUtilPINVOKE.VariantList_capacity(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void reserve(uint n)
		{
			AppUtilPINVOKE.VariantList_reserve(this.swigCPtr, n);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private Variant getitemcopy(int index)
		{
			Variant result = new Variant(AppUtilPINVOKE.VariantList_getitemcopy(this.swigCPtr, index), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private Variant getitem(int index)
		{
			Variant result = new Variant(AppUtilPINVOKE.VariantList_getitem(this.swigCPtr, index), false);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void setitem(int index, Variant val)
		{
			AppUtilPINVOKE.VariantList_setitem(this.swigCPtr, index, Variant.getCPtr(val));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void AddRange(VariantList values)
		{
			AppUtilPINVOKE.VariantList_AddRange(this.swigCPtr, VariantList.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public VariantList GetRange(int index, int count)
		{
			IntPtr intPtr = AppUtilPINVOKE.VariantList_GetRange(this.swigCPtr, index, count);
			VariantList result = (!(intPtr == IntPtr.Zero)) ? new VariantList(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Insert(int index, Variant x)
		{
			AppUtilPINVOKE.VariantList_Insert(this.swigCPtr, index, Variant.getCPtr(x));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void InsertRange(int index, VariantList values)
		{
			AppUtilPINVOKE.VariantList_InsertRange(this.swigCPtr, index, VariantList.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RemoveAt(int index)
		{
			AppUtilPINVOKE.VariantList_RemoveAt(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RemoveRange(int index, int count)
		{
			AppUtilPINVOKE.VariantList_RemoveRange(this.swigCPtr, index, count);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static VariantList Repeat(Variant value, int count)
		{
			IntPtr intPtr = AppUtilPINVOKE.VariantList_Repeat(Variant.getCPtr(value), count);
			VariantList result = (!(intPtr == IntPtr.Zero)) ? new VariantList(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Reverse()
		{
			AppUtilPINVOKE.VariantList_Reverse__SWIG_0(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Reverse(int index, int count)
		{
			AppUtilPINVOKE.VariantList_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetRange(int index, VariantList values)
		{
			AppUtilPINVOKE.VariantList_SetRange(this.swigCPtr, index, VariantList.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public sealed class VariantListEnumerator : IEnumerator, IEnumerator<Variant>, IDisposable
		{
			private VariantList collectionRef;

			private int currentIndex;

			private object currentObject;

			private int currentSize;

			public VariantListEnumerator(VariantList collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			public Variant Current
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
					return (Variant)this.currentObject;
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
