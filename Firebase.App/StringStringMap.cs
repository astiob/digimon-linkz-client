using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class StringStringMap : IDisposable, IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal StringStringMap(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public StringStringMap() : this(AppUtilPINVOKE.new_StringStringMap__SWIG_0(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public StringStringMap(StringStringMap other) : this(AppUtilPINVOKE.new_StringStringMap__SWIG_1(StringStringMap.getCPtr(other)), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(StringStringMap obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~StringStringMap()
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
						AppUtilPINVOKE.delete_StringStringMap(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public string this[string key]
		{
			get
			{
				return this.getitem(key);
			}
			set
			{
				this.setitem(key, value);
			}
		}

		public bool TryGetValue(string key, out string value)
		{
			if (this.ContainsKey(key))
			{
				value = this[key];
				return true;
			}
			value = null;
			return false;
		}

		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				ICollection<string> collection = new List<string>();
				int count = this.Count;
				if (count > 0)
				{
					IntPtr swigiterator = this.create_iterator_begin();
					for (int i = 0; i < count; i++)
					{
						collection.Add(this.get_next_key(swigiterator));
					}
					this.destroy_iterator(swigiterator);
				}
				return collection;
			}
		}

		public ICollection<string> Values
		{
			get
			{
				ICollection<string> collection = new List<string>();
				foreach (KeyValuePair<string, string> keyValuePair in this)
				{
					collection.Add(keyValuePair.Value);
				}
				return collection;
			}
		}

		public void Add(KeyValuePair<string, string> item)
		{
			this.Add(item.Key, item.Value);
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return this[item.Key] == item.Value;
		}

		public void CopyTo(KeyValuePair<string, string>[] array)
		{
			this.CopyTo(array, 0);
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("Multi dimensional array.", "array");
			}
			if (arrayIndex + this.Count > array.Length)
			{
				throw new ArgumentException("Number of elements to copy is too large.");
			}
			IList<string> list = new List<string>(this.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				string key = list[i];
				array.SetValue(new KeyValuePair<string, string>(key, this[key]), arrayIndex + i);
			}
		}

		IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
		{
			return new StringStringMap.StringStringMapEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new StringStringMap.StringStringMapEnumerator(this);
		}

		public StringStringMap.StringStringMapEnumerator GetEnumerator()
		{
			return new StringStringMap.StringStringMapEnumerator(this);
		}

		private uint size()
		{
			uint result = AppUtilPINVOKE.StringStringMap_size(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool empty()
		{
			bool result = AppUtilPINVOKE.StringStringMap_empty(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Clear()
		{
			AppUtilPINVOKE.StringStringMap_Clear(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private string getitem(string key)
		{
			string result = AppUtilPINVOKE.StringStringMap_getitem(this.swigCPtr, key);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void setitem(string key, string x)
		{
			AppUtilPINVOKE.StringStringMap_setitem(this.swigCPtr, key, x);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool ContainsKey(string key)
		{
			bool result = AppUtilPINVOKE.StringStringMap_ContainsKey(this.swigCPtr, key);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Add(string key, string val)
		{
			AppUtilPINVOKE.StringStringMap_Add(this.swigCPtr, key, val);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool Remove(string key)
		{
			bool result = AppUtilPINVOKE.StringStringMap_Remove(this.swigCPtr, key);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private IntPtr create_iterator_begin()
		{
			IntPtr result = AppUtilPINVOKE.StringStringMap_create_iterator_begin(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private string get_next_key(IntPtr swigiterator)
		{
			string result = AppUtilPINVOKE.StringStringMap_get_next_key(this.swigCPtr, swigiterator);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void destroy_iterator(IntPtr swigiterator)
		{
			AppUtilPINVOKE.StringStringMap_destroy_iterator(this.swigCPtr, swigiterator);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public sealed class StringStringMapEnumerator : IEnumerator, IEnumerator<KeyValuePair<string, string>>, IDisposable
		{
			private StringStringMap collectionRef;

			private IList<string> keyCollection;

			private int currentIndex;

			private object currentObject;

			private int currentSize;

			public StringStringMapEnumerator(StringStringMap collection)
			{
				this.collectionRef = collection;
				this.keyCollection = new List<string>(collection.Keys);
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			public KeyValuePair<string, string> Current
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
					return (KeyValuePair<string, string>)this.currentObject;
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
					string key = this.keyCollection[this.currentIndex];
					this.currentObject = new KeyValuePair<string, string>(key, this.collectionRef[key]);
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
