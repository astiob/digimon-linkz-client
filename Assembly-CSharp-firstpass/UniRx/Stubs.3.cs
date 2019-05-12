using System;

namespace UniRx
{
	internal static class Stubs<T1, T2>
	{
		public static readonly Action<T1, T2> Ignore = delegate(T1 x, T2 y)
		{
		};

		public static readonly Action<Exception, T1, T2> Throw = delegate(Exception ex, T1 _, T2 __)
		{
			throw ex;
		};
	}
}
