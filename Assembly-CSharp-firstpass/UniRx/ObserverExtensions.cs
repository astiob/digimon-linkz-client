using System;
using UniRx.Operators;

namespace UniRx
{
	public static class ObserverExtensions
	{
		public static IObserver<T> Synchronize<T>(this IObserver<T> observer)
		{
			return new SynchronizedObserver<T>(observer, new object());
		}

		public static IObserver<T> Synchronize<T>(this IObserver<T> observer, object gate)
		{
			return new SynchronizedObserver<T>(observer, gate);
		}
	}
}
