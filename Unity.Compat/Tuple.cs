using System;

namespace System
{
	public static class Tuple
	{
		public static Tuple<T1, T2> Create<T1, T2>(T1 t1, T2 t2)
		{
			return new Tuple<T1, T2>(t1, t2);
		}
	}
}
