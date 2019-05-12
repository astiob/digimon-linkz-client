using System;

namespace UniRx.Operators
{
	internal class ImmutableNeverObservable<T> : IObservable<T>, IOptimizedObservable<T>
	{
		internal static ImmutableNeverObservable<T> Instance = new ImmutableNeverObservable<T>();

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return Disposable.Empty;
		}
	}
}
