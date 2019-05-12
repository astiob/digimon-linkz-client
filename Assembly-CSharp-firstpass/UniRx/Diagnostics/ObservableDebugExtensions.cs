using System;

namespace UniRx.Diagnostics
{
	public static class ObservableDebugExtensions
	{
		public static IObservable<T> Debug<T>(this IObservable<T> source, string label = null)
		{
			return source;
		}

		public static IObservable<T> Debug<T>(this IObservable<T> source, Logger logger)
		{
			return source;
		}
	}
}
