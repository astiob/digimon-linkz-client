using System;

namespace UniRx
{
	public static class OptimizedObservableExtensions
	{
		public static bool IsRequiredSubscribeOnCurrentThread<T>(this IObservable<T> source)
		{
			IOptimizedObservable<T> optimizedObservable = source as IOptimizedObservable<T>;
			return optimizedObservable == null || optimizedObservable.IsRequiredSubscribeOnCurrentThread();
		}

		public static bool IsRequiredSubscribeOnCurrentThread<T>(this IObservable<T> source, IScheduler scheduler)
		{
			return scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>();
		}
	}
}
