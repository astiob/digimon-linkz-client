using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class VariantVariantMap : IDisposable, IDictionary<Variant, Variant>, ICollection<KeyValuePair<Variant, Variant>>, IEnumerable<KeyValuePair<Variant, Variant>>, IEnumerable
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal VariantVariantMap(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public VariantVariantMap() : this(AppUtilPINVOKE.new_VariantVariantMap__SWIG_0(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public VariantVariantMap(VariantVariantMap other) : this(AppUtilPINVOKE.new_VariantVariantMap__SWIG_1(VariantVariantMap.getCPtr(other)), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(VariantVariantMap obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~VariantVariantMap()
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
						AppUtilPINVOKE.delete_VariantVariantMap(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Variant this[Variant key]
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

		public bool TryGetValue(Variant key, out Variant value)
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

		public ICollection<Variant> Keys
		{
			get
			{
				ICollection<Variant> collection = new List<Variant>();
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

		public ICollection<Variant> Values
		{
			get
			{
				ICollection<Variant> collection = new List<Variant>();
				foreach (KeyValuePair<Variant, Variant> keyValuePair in this)
				{
					collection.Add(keyValuePair.Value);
				}
				return collection;
			}
		}

		public void Add(KeyValuePair<Variant, Variant> item)
		{
			this.Add(item.Key, item.Value);
		}

		public bool Remove(KeyValuePair<Variant, Variant> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		public bool Contains(KeyValuePair<Variant, Variant> item)
		{
			return this[item.Key] == item.Value;
		}

		public void CopyTo(KeyValuePair<Variant, Variant>[] array)
		{
			this.CopyTo(array, 0);
		}

		public void CopyTo(KeyValuePair<Variant, Variant>[] array, int arrayIndex)
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
			IList<Variant> list = new List<Variant>(this.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				Variant key = list[i];
				array.SetValue(new KeyValuePair<Variant, Variant>(key, this[key]), arrayIndex + i);
			}
		}

		IEnumerator<KeyValuePair<Variant, Variant>> IEnumerable<KeyValuePair<Variant, Variant>>.GetEnumerator()
		{
			return new VariantVariantMap.VariantVariantMapEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new VariantVariantMap.VariantVariantMapEnumerator(this);
		}

		public VariantVariantMap.VariantVariantMapEnumerator GetEnumerator()
		{
			return new VariantVariantMap.VariantVariantMapEnumerator(this);
		}

		private uint size()
		{
			uint result = AppUtilPINVOKE.VariantVariantMap_size(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool empty()
		{
			bool result = AppUtilPINVOKE.VariantVariantMap_empty(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Clear()
		{
			AppUtilPINVOKE.VariantVariantMap_Clear(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private Variant getitem(Variant key)
		{
			Variant result = new Variant(AppUtilPINVOKE.VariantVariantMap_getitem(this.swigCPtr, Variant.getCPtr(key)), false);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void setitem(Variant key, Variant x)
		{
			AppUtilPINVOKE.VariantVariantMap_setitem(this.swigCPtr, Variant.getCPtr(key), Variant.getCPtr(x));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool ContainsKey(Variant key)
		{
			bool result = AppUtilPINVOKE.VariantVariantMap_ContainsKey(this.swigCPtr, Variant.getCPtr(key));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void Add(Variant key, Variant val)
		{
			AppUtilPINVOKE.VariantVariantMap_Add(this.swigCPtr, Variant.getCPtr(key), Variant.getCPtr(val));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool Remove(Variant key)
		{
			bool result = AppUtilPINVOKE.VariantVariantMap_Remove(this.swigCPtr, Variant.getCPtr(key));
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private IntPtr create_iterator_begin()
		{
			IntPtr result = AppUtilPINVOKE.VariantVariantMap_create_iterator_begin(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private Variant get_next_key(IntPtr swigiterator)
		{
			Variant result = new Variant(AppUtilPINVOKE.VariantVariantMap_get_next_key(this.swigCPtr, swigiterator), false);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private void destroy_iterator(IntPtr swigiterator)
		{
			AppUtilPINVOKE.VariantVariantMap_destroy_iterator(this.swigCPtr, swigiterator);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public sealed class VariantVariantMapEnumerator : IEnumerator, IEnumerator<KeyValuePair<Variant, Variant>>, IDisposable
		{
			private VariantVariantMap collectionRef;

			private IList<Variant> keyCollection;

			private int currentIndex;

			private object currentObject;

			private int currentSize;

			public VariantVariantMapEnumerator(VariantVariantMap collection)
			{
				this.collectionRef = collection;
				this.keyCollection = new List<Variant>(collection.Keys);
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			public KeyValuePair<Variant, Variant> Current
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
					return (KeyValuePair<Variant, Variant>)this.currentObject;
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
					Variant key = this.keyCollection[this.currentIndex];
					this.currentObject = new KeyValuePair<Variant, Variant>(key, this.collectionRef[key]);
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
