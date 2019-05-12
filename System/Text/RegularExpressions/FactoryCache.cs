using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class FactoryCache
	{
		private int capacity;

		private Hashtable factories;

		private MRUList mru_list;

		public FactoryCache(int capacity)
		{
			this.capacity = capacity;
			this.factories = new Hashtable(capacity);
			this.mru_list = new MRUList();
		}

		public void Add(string pattern, RegexOptions options, IMachineFactory factory)
		{
			lock (this)
			{
				FactoryCache.Key key = new FactoryCache.Key(pattern, options);
				this.Cleanup();
				this.factories[key] = factory;
				this.mru_list.Use(key);
			}
		}

		private void Cleanup()
		{
			while (this.factories.Count >= this.capacity && this.capacity > 0)
			{
				object obj = this.mru_list.Evict();
				if (obj != null)
				{
					this.factories.Remove((FactoryCache.Key)obj);
				}
			}
		}

		public IMachineFactory Lookup(string pattern, RegexOptions options)
		{
			lock (this)
			{
				FactoryCache.Key key = new FactoryCache.Key(pattern, options);
				if (this.factories.Contains(key))
				{
					this.mru_list.Use(key);
					return (IMachineFactory)this.factories[key];
				}
			}
			return null;
		}

		public int Capacity
		{
			get
			{
				return this.capacity;
			}
			set
			{
				lock (this)
				{
					this.capacity = value;
					this.Cleanup();
				}
			}
		}

		private class Key
		{
			public string pattern;

			public RegexOptions options;

			public Key(string pattern, RegexOptions options)
			{
				this.pattern = pattern;
				this.options = options;
			}

			public override int GetHashCode()
			{
				return this.pattern.GetHashCode() ^ (int)this.options;
			}

			public override bool Equals(object o)
			{
				if (o == null || !(o is FactoryCache.Key))
				{
					return false;
				}
				FactoryCache.Key key = (FactoryCache.Key)o;
				return this.options == key.options && this.pattern.Equals(key.pattern);
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"('",
					this.pattern,
					"', [",
					this.options,
					"])"
				});
			}
		}
	}
}
