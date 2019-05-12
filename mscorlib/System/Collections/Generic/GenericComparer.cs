using System;

namespace System.Collections.Generic
{
	[Serializable]
	internal sealed class GenericComparer<T> : Comparer<T> where T : IComparable<T>
	{
		public override int Compare(T x, T y)
		{
			if (x == null)
			{
				return (y != null) ? -1 : 0;
			}
			if (y == null)
			{
				return 1;
			}
			return x.CompareTo(y);
		}
	}
}
