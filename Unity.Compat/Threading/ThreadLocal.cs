using System;
using System.Collections.Generic;

namespace System.Threading
{
	public class ThreadLocal<T> : IDisposable
	{
		private static long lastId = -1L;

		[ThreadStatic]
		private static IDictionary<long, T> threadLocalData;

		private static IList<WeakReference> allDataDictionaries = new List<WeakReference>();

		private bool disposed;

		private readonly long id;

		private readonly Func<T> valueFactory;

		public ThreadLocal() : this(() => default(T))
		{
		}

		public ThreadLocal(Func<T> valueFactory)
		{
			this.valueFactory = valueFactory;
			this.id = Interlocked.Increment(ref ThreadLocal<T>.lastId);
		}

		private static IDictionary<long, T> ThreadLocalData
		{
			get
			{
				if (ThreadLocal<T>.threadLocalData == null)
				{
					ThreadLocal<T>.threadLocalData = new Dictionary<long, T>();
					object obj = ThreadLocal<T>.allDataDictionaries;
					lock (obj)
					{
						ThreadLocal<T>.allDataDictionaries.Add(new WeakReference(ThreadLocal<T>.threadLocalData));
					}
				}
				return ThreadLocal<T>.threadLocalData;
			}
		}

		public T Value
		{
			get
			{
				this.CheckDisposed();
				T result;
				if (ThreadLocal<T>.ThreadLocalData.TryGetValue(this.id, out result))
				{
					return result;
				}
				T t = this.valueFactory();
				ThreadLocal<T>.ThreadLocalData[this.id] = t;
				return t;
			}
			set
			{
				this.CheckDisposed();
				ThreadLocal<T>.ThreadLocalData[this.id] = value;
			}
		}

		~ThreadLocal()
		{
			if (!this.disposed)
			{
				this.Dispose();
			}
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("ThreadLocal has been disposed.");
			}
		}

		public void Dispose()
		{
			object obj = ThreadLocal<T>.allDataDictionaries;
			lock (obj)
			{
				for (int i = 0; i < ThreadLocal<T>.allDataDictionaries.Count; i++)
				{
					IDictionary<object, T> dictionary = ThreadLocal<T>.allDataDictionaries[i].Target as IDictionary<object, T>;
					if (dictionary == null)
					{
						ThreadLocal<T>.allDataDictionaries.RemoveAt(i);
						i--;
					}
					else
					{
						dictionary.Remove(this.id);
					}
				}
			}
			this.disposed = true;
		}
	}
}
