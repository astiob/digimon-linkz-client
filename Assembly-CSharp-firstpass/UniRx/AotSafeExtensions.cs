using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public static class AotSafeExtensions
	{
		public static IEnumerable<T> AsSafeEnumerable<T>(this IEnumerable<T> source)
		{
			IEnumerator e = source.GetEnumerator();
			using (e as IDisposable)
			{
				while (e.MoveNext())
				{
					object obj = e.Current;
					yield return (T)((object)obj);
				}
			}
			yield break;
		}
	}
}
