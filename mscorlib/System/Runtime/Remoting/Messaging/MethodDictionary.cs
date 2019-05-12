using System;
using System.Collections;

namespace System.Runtime.Remoting.Messaging
{
	[Serializable]
	internal class MethodDictionary : IEnumerable, ICollection, IDictionary
	{
		private IDictionary _internalProperties;

		protected IMethodMessage _message;

		private string[] _methodKeys;

		private bool _ownProperties;

		public MethodDictionary(IMethodMessage message)
		{
			this._message = message;
		}

		public MethodDictionary(string[] keys)
		{
			this._methodKeys = keys;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new MethodDictionary.DictionaryEnumerator(this);
		}

		internal bool HasInternalProperties
		{
			get
			{
				if (this._internalProperties == null)
				{
					return false;
				}
				if (this._internalProperties is MethodDictionary)
				{
					return ((MethodDictionary)this._internalProperties).HasInternalProperties;
				}
				return this._internalProperties.Count > 0;
			}
		}

		internal IDictionary InternalProperties
		{
			get
			{
				if (this._internalProperties != null && this._internalProperties is MethodDictionary)
				{
					return ((MethodDictionary)this._internalProperties).InternalProperties;
				}
				return this._internalProperties;
			}
		}

		public string[] MethodKeys
		{
			get
			{
				return this._methodKeys;
			}
			set
			{
				this._methodKeys = value;
			}
		}

		protected virtual IDictionary AllocInternalProperties()
		{
			this._ownProperties = true;
			return new Hashtable();
		}

		public IDictionary GetInternalProperties()
		{
			if (this._internalProperties == null)
			{
				this._internalProperties = this.AllocInternalProperties();
			}
			return this._internalProperties;
		}

		private bool IsOverridenKey(string key)
		{
			if (this._ownProperties)
			{
				return false;
			}
			foreach (string b in this._methodKeys)
			{
				if (key == b)
				{
					return true;
				}
			}
			return false;
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

		public object this[object key]
		{
			get
			{
				string text = (string)key;
				for (int i = 0; i < this._methodKeys.Length; i++)
				{
					if (this._methodKeys[i] == text)
					{
						return this.GetMethodProperty(text);
					}
				}
				if (this._internalProperties != null)
				{
					return this._internalProperties[key];
				}
				return null;
			}
			set
			{
				this.Add(key, value);
			}
		}

		protected virtual object GetMethodProperty(string key)
		{
			switch (key)
			{
			case "__Uri":
				return this._message.Uri;
			case "__MethodName":
				return this._message.MethodName;
			case "__TypeName":
				return this._message.TypeName;
			case "__MethodSignature":
				return this._message.MethodSignature;
			case "__CallContext":
				return this._message.LogicalCallContext;
			case "__Args":
				return this._message.Args;
			case "__OutArgs":
				return ((IMethodReturnMessage)this._message).OutArgs;
			case "__Return":
				return ((IMethodReturnMessage)this._message).ReturnValue;
			}
			return null;
		}

		protected virtual void SetMethodProperty(string key, object value)
		{
			switch (key)
			{
			case "__CallContext":
			case "__OutArgs":
			case "__Return":
				return;
			case "__MethodName":
			case "__TypeName":
			case "__MethodSignature":
			case "__Args":
				throw new ArgumentException("key was invalid");
			case "__Uri":
				((IInternalMessage)this._message).Uri = (string)value;
				return;
			}
		}

		public ICollection Keys
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < this._methodKeys.Length; i++)
				{
					arrayList.Add(this._methodKeys[i]);
				}
				if (this._internalProperties != null)
				{
					foreach (object obj in this._internalProperties.Keys)
					{
						string text = (string)obj;
						if (!this.IsOverridenKey(text))
						{
							arrayList.Add(text);
						}
					}
				}
				return arrayList;
			}
		}

		public ICollection Values
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < this._methodKeys.Length; i++)
				{
					arrayList.Add(this.GetMethodProperty(this._methodKeys[i]));
				}
				if (this._internalProperties != null)
				{
					foreach (object obj in this._internalProperties)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (!this.IsOverridenKey((string)dictionaryEntry.Key))
						{
							arrayList.Add(dictionaryEntry.Value);
						}
					}
				}
				return arrayList;
			}
		}

		public void Add(object key, object value)
		{
			string text = (string)key;
			for (int i = 0; i < this._methodKeys.Length; i++)
			{
				if (this._methodKeys[i] == text)
				{
					this.SetMethodProperty(text, value);
					return;
				}
			}
			if (this._internalProperties == null)
			{
				this._internalProperties = this.AllocInternalProperties();
			}
			this._internalProperties[key] = value;
		}

		public void Clear()
		{
			if (this._internalProperties != null)
			{
				this._internalProperties.Clear();
			}
		}

		public bool Contains(object key)
		{
			string b = (string)key;
			for (int i = 0; i < this._methodKeys.Length; i++)
			{
				if (this._methodKeys[i] == b)
				{
					return true;
				}
			}
			return this._internalProperties != null && this._internalProperties.Contains(key);
		}

		public void Remove(object key)
		{
			string b = (string)key;
			for (int i = 0; i < this._methodKeys.Length; i++)
			{
				if (this._methodKeys[i] == b)
				{
					throw new ArgumentException("key was invalid");
				}
			}
			if (this._internalProperties != null)
			{
				this._internalProperties.Remove(key);
			}
		}

		public int Count
		{
			get
			{
				if (this._internalProperties != null)
				{
					return this._internalProperties.Count + this._methodKeys.Length;
				}
				return this._methodKeys.Length;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.Values.CopyTo(array, index);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return new MethodDictionary.DictionaryEnumerator(this);
		}

		private class DictionaryEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private MethodDictionary _methodDictionary;

			private IDictionaryEnumerator _hashtableEnum;

			private int _posMethod;

			public DictionaryEnumerator(MethodDictionary methodDictionary)
			{
				this._methodDictionary = methodDictionary;
				IDictionaryEnumerator hashtableEnum;
				if (this._methodDictionary._internalProperties != null)
				{
					IDictionaryEnumerator enumerator = this._methodDictionary._internalProperties.GetEnumerator();
					hashtableEnum = enumerator;
				}
				else
				{
					hashtableEnum = null;
				}
				this._hashtableEnum = hashtableEnum;
				this._posMethod = -1;
			}

			public object Current
			{
				get
				{
					return this.Entry.Value;
				}
			}

			public bool MoveNext()
			{
				if (this._posMethod != -2)
				{
					this._posMethod++;
					if (this._posMethod < this._methodDictionary._methodKeys.Length)
					{
						return true;
					}
					this._posMethod = -2;
				}
				if (this._hashtableEnum == null)
				{
					return false;
				}
				while (this._hashtableEnum.MoveNext())
				{
					if (!this._methodDictionary.IsOverridenKey((string)this._hashtableEnum.Key))
					{
						return true;
					}
				}
				return false;
			}

			public void Reset()
			{
				this._posMethod = -1;
				this._hashtableEnum.Reset();
			}

			public DictionaryEntry Entry
			{
				get
				{
					if (this._posMethod >= 0)
					{
						return new DictionaryEntry(this._methodDictionary._methodKeys[this._posMethod], this._methodDictionary.GetMethodProperty(this._methodDictionary._methodKeys[this._posMethod]));
					}
					if (this._posMethod == -1 || this._hashtableEnum == null)
					{
						throw new InvalidOperationException("The enumerator is positioned before the first element of the collection or after the last element");
					}
					return this._hashtableEnum.Entry;
				}
			}

			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}
		}
	}
}
