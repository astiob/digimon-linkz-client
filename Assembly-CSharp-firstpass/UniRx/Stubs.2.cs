using System;

namespace UniRx
{
	internal static class Stubs<T>
	{
		public static readonly Action<T> Ignore = delegate(T t)
		{
		};

		public static readonly Func<T, T> Identity = (T t) => t;

		public static readonly Action<Exception, T> Throw = delegate(Exception ex, T _)
		{
			throw ex;
		};
	}
}
