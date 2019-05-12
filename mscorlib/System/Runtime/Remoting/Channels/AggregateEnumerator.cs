using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	internal class AggregateEnumerator : IEnumerator, IDictionaryEnumerator
	{
		private IDictionary[] dictionaries;

		private int pos;

		private IDictionaryEnumerator currente;

		public AggregateEnumerator(IDictionary[] dics)
		{
			this.dictionaries = dics;
			this.Reset();
		}

		public DictionaryEntry Entry
		{
			get
			{
				return this.currente.Entry;
			}
		}

		public object Key
		{
			get
			{
				return this.currente.Key;
			}
		}

		public object Value
		{
			get
			{
				return this.currente.Value;
			}
		}

		public object Current
		{
			get
			{
				return this.currente.Current;
			}
		}

		public bool MoveNext()
		{
			if (this.pos >= this.dictionaries.Length)
			{
				return false;
			}
			if (this.currente.MoveNext())
			{
				return true;
			}
			this.pos++;
			if (this.pos >= this.dictionaries.Length)
			{
				return false;
			}
			this.currente = this.dictionaries[this.pos].GetEnumerator();
			return this.MoveNext();
		}

		public void Reset()
		{
			this.pos = 0;
			if (this.dictionaries.Length > 0)
			{
				this.currente = this.dictionaries[0].GetEnumerator();
			}
		}
	}
}
