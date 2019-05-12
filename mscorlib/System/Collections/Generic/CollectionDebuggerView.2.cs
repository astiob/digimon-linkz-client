using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	internal sealed class CollectionDebuggerView<T, U>
	{
		private readonly ICollection<KeyValuePair<T, U>> c;

		public CollectionDebuggerView(ICollection<KeyValuePair<T, U>> col)
		{
			this.c = col;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<T, U>[] Items
		{
			get
			{
				KeyValuePair<T, U>[] array = new KeyValuePair<T, U>[this.c.Count];
				this.c.CopyTo(array, 0);
				return array;
			}
		}
	}
}
