using System;

namespace System.Collections.Specialized
{
	internal class ProcessStringDictionary : StringDictionary, IEnumerable
	{
		private Hashtable table;

		public ProcessStringDictionary()
		{
			IHashCodeProvider hcp = null;
			IComparer comparer = null;
			int platform = (int)Environment.OSVersion.Platform;
			if (platform != 4 && platform != 128)
			{
				hcp = CaseInsensitiveHashCodeProvider.DefaultInvariant;
				comparer = CaseInsensitiveComparer.DefaultInvariant;
			}
			this.table = new Hashtable(hcp, comparer);
		}

		public override int Count
		{
			get
			{
				return this.table.Count;
			}
		}

		public override bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public override string this[string key]
		{
			get
			{
				return (string)this.table[key];
			}
			set
			{
				this.table[key] = value;
			}
		}

		public override ICollection Keys
		{
			get
			{
				return this.table.Keys;
			}
		}

		public override ICollection Values
		{
			get
			{
				return this.table.Values;
			}
		}

		public override object SyncRoot
		{
			get
			{
				return this.table.SyncRoot;
			}
		}

		public override void Add(string key, string value)
		{
			this.table.Add(key, value);
		}

		public override void Clear()
		{
			this.table.Clear();
		}

		public override bool ContainsKey(string key)
		{
			return this.table.ContainsKey(key);
		}

		public override bool ContainsValue(string value)
		{
			return this.table.ContainsValue(value);
		}

		public override void CopyTo(Array array, int index)
		{
			this.table.CopyTo(array, index);
		}

		public override IEnumerator GetEnumerator()
		{
			return this.table.GetEnumerator();
		}

		public override void Remove(string key)
		{
			this.table.Remove(key);
		}
	}
}
