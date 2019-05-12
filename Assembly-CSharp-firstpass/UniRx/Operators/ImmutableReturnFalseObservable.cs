using System;

namespace UniRx.Operators
{
	internal class ImmutableReturnFalseObservable : IObservable<bool>, IOptimizedObservable<bool>
	{
		internal static ImmutableReturnFalseObservable Instance = new ImmutableReturnFalseObservable();

		private ImmutableReturnFalseObservable()
		{
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<bool> observer)
		{
			observer.OnNext(false);
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}
}
