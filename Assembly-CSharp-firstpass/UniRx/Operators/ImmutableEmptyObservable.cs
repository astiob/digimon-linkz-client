using System;

namespace UniRx.Operators
{
	internal class ImmutableEmptyObservable<T> : IObservable<T>, IOptimizedObservable<T>
	{
		internal static ImmutableEmptyObservable<T> Instance = new ImmutableEmptyObservable<T>();

		private ImmutableEmptyObservable()
		{
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}
}
