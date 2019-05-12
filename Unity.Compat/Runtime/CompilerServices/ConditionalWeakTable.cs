using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
	public class ConditionalWeakTable<TKey, TValue> where TKey : class where TValue : class
	{
		private IDictionary<ConditionalWeakTable<TKey, TValue>.Reference, TValue> data;

		public ConditionalWeakTable()
		{
			this.data = new Dictionary<ConditionalWeakTable<TKey, TValue>.Reference, TValue>();
		}

		private void CleanUp()
		{
			foreach (ConditionalWeakTable<TKey, TValue>.Reference reference in new HashSet<ConditionalWeakTable<TKey, TValue>.Reference>(this.data.Keys))
			{
				if (!reference.WeakReference.IsAlive)
				{
					this.data.Remove(reference);
				}
			}
		}

		public TValue GetValue(TKey key, ConditionalWeakTable<TKey, TValue>.CreateValueCallback createValueCallback)
		{
			this.CleanUp();
			ConditionalWeakTable<TKey, TValue>.Reference key2 = new ConditionalWeakTable<TKey, TValue>.Reference(key);
			TValue result;
			if (this.data.TryGetValue(key2, out result))
			{
				return result;
			}
			TValue tvalue = createValueCallback(key);
			this.data[key2] = tvalue;
			return tvalue;
		}

		private class Reference
		{
			private int hashCode;

			public Reference(TKey obj)
			{
				this.hashCode = obj.GetHashCode();
				this.WeakReference = new WeakReference(obj);
			}

			public WeakReference WeakReference { get; private set; }

			public TKey Value
			{
				get
				{
					return (TKey)((object)this.WeakReference.Target);
				}
			}

			public override int GetHashCode()
			{
				return this.hashCode;
			}

			public override bool Equals(object obj)
			{
				ConditionalWeakTable<TKey, TValue>.Reference reference = obj as ConditionalWeakTable<TKey, TValue>.Reference;
				return reference != null && reference.GetHashCode() == this.GetHashCode() && object.ReferenceEquals(reference.WeakReference.Target, this.WeakReference.Target);
			}
		}

		public delegate TValue CreateValueCallback(TKey key);
	}
}
