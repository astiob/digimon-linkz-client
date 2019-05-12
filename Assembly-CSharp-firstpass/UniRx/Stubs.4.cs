using System;

namespace UniRx
{
	internal static class Stubs<T1, T2, T3>
	{
		public static readonly Action<T1, T2, T3> Ignore = delegate(T1 x, T2 y, T3 z)
		{
		};

		public static readonly Action<Exception, T1, T2, T3> Throw = delegate(Exception ex, T1 _, T2 __, T3 ___)
		{
			throw ex;
		};
	}
}
