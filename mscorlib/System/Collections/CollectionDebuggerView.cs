using System;
using System.Diagnostics;

namespace System.Collections
{
	internal sealed class CollectionDebuggerView
	{
		private readonly ICollection c;

		public CollectionDebuggerView(ICollection col)
		{
			this.c = col;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public object[] Items
		{
			get
			{
				object[] array = new object[Math.Min(this.c.Count, 1024)];
				this.c.CopyTo(array, 0);
				return array;
			}
		}
	}
}
