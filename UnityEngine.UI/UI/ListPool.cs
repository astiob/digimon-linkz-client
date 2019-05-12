using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI
{
	internal static class ListPool<T>
	{
		private static readonly ObjectPool<List<T>> s_ListPool;

		[CompilerGenerated]
		private static UnityAction<List<T>> <>f__mg$cache0;

		private static void Clear(List<T> l)
		{
			l.Clear();
		}

		public static List<T> Get()
		{
			return ListPool<T>.s_ListPool.Get();
		}

		public static void Release(List<T> toRelease)
		{
			ListPool<T>.s_ListPool.Release(toRelease);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ListPool()
		{
			UnityAction<List<T>> actionOnGet = null;
			if (ListPool<T>.<>f__mg$cache0 == null)
			{
				ListPool<T>.<>f__mg$cache0 = new UnityAction<List<T>>(ListPool<T>.Clear);
			}
			ListPool<T>.s_ListPool = new ObjectPool<List<T>>(actionOnGet, ListPool<T>.<>f__mg$cache0);
		}
	}
}
