using System;

namespace System.Collections.Generic
{
	[Serializable]
	internal sealed class GenericEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
	{
		public override int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		public override bool Equals(T x, T y)
		{
			if (x == null)
			{
				return y == null;
			}
			return x.Equals(y);
		}
	}
}
