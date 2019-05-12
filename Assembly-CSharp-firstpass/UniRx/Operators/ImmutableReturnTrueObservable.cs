using System;

namespace UniRx.Operators
{
	internal class ImmutableReturnTrueObservable : IObservable<bool>, IOptimizedObservable<bool>
	{
		internal static ImmutableReturnTrueObservable Instance = new ImmutableReturnTrueObservable();

		private ImmutableReturnTrueObservable()
		{
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<bool> observer)
		{
			observer.OnNext(true);
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}
}
