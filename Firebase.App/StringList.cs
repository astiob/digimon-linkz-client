using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class StringList : IDisposable, IEnumerable, IList<string>, ICollection<string>, IEnumerable<string>
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal StringList(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public StringList(ICollection c) : this()
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
					string x = (string)obj;
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

		public StringList() : this(AppUtilPINVOKE.new_StringList__SWIG_0(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public StringList(StringList other) : this(AppUtilPINVOKE.new_StringList__SWIG_1(StringList.getCPtr(other)), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public StringList(int capacity) : this(AppUtilPINVOKE.new_StringList__SWIG_2(capacity), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(StringList obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~StringList()
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
						AppUtilPINVOKE.delete_StringList(this.swigCPtr);
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

		public string this[int index]
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

		public void CopyTo(string[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		public void CopyTo(int index, string[] array, int arrayIndex, int count)
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

		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return new StringList.StringListEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new StringList.StringListEnumerator(this);
		}

		public StringList.StringListEnumerator GetEnumerator()
		{
			return new StringList.StringListEnumerator(this);
		}

		public void Clear()
		{
			AppUtilPINVOKE.StringList_Clear(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Add(string x)
		{
			AppUtilPINVOKE.StringList_Add(this.swigCPtr, x);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private uint size()
		{
			uint result = AppUtilPINVOKE.StringList_size(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private uint capacity()
		{
			uint result = AppUtilPINVOKE.StringList_capacity(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void reserve(uint n)
		{
			AppUtilPINVOKE.StringList_reserve(this.swigCPtr, n);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private string getitemcopy(int index)
		{
			string result = AppUtilPINVOKE.StringList_getitemcopy(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private string getitem(int index)
		{
			string result = AppUtilPINVOKE.StringList_getitem(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void setitem(int index, string val)
		{
			AppUtilPINVOKE.StringList_setitem(this.swigCPtr, index, val);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void AddRange(StringList values)
		{
			AppUtilPINVOKE.StringList_AddRange(this.swigCPtr, StringList.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public StringList GetRange(int index, int count)
		{
			IntPtr intPtr = AppUtilPINVOKE.StringList_GetRange(this.swigCPtr, index, count);
			StringList result = (!(intPtr == IntPtr.Zero)) ? new StringList(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Insert(int index, string x)
		{
			AppUtilPINVOKE.StringList_Insert(this.swigCPtr, index, x);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void InsertRange(int index, StringList values)
		{
			AppUtilPINVOKE.StringList_InsertRange(this.swigCPtr, index, StringList.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RemoveAt(int index)
		{
			AppUtilPINVOKE.StringList_RemoveAt(this.swigCPtr, index);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void RemoveRange(int index, int count)
		{
			AppUtilPINVOKE.StringList_RemoveRange(this.swigCPtr, index, count);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static StringList Repeat(string value, int count)
		{
			IntPtr intPtr = AppUtilPINVOKE.StringList_Repeat(value, count);
			StringList result = (!(intPtr == IntPtr.Zero)) ? new StringList(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Reverse()
		{
			AppUtilPINVOKE.StringList_Reverse__SWIG_0(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void Reverse(int index, int count)
		{
			AppUtilPINVOKE.StringList_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetRange(int index, StringList values)
		{
			AppUtilPINVOKE.StringList_SetRange(this.swigCPtr, index, StringList.getCPtr(values));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool Contains(string value)
		{
			bool result = AppUtilPINVOKE.StringList_Contains(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public int IndexOf(string value)
		{
			int result = AppUtilPINVOKE.StringList_IndexOf(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public int LastIndexOf(string value)
		{
			int result = AppUtilPINVOKE.StringList_LastIndexOf(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool Remove(string value)
		{
			bool result = AppUtilPINVOKE.StringList_Remove(this.swigCPtr, value);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public sealed class StringListEnumerator : IEnumerator, IEnumerator<string>, IDisposable
		{
			private StringList collectionRef;

			private int currentIndex;

			private object currentObject;

			private int currentSize;

			public StringListEnumerator(StringList collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			public string Current
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
					return (string)this.currentObject;
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
