using System;

namespace UniRx.Operators
{
	internal class ImmutableReturnUnitObservable : IObservable<Unit>, IOptimizedObservable<Unit>
	{
		internal static ImmutableReturnUnitObservable Instance = new ImmutableReturnUnitObservable();

		private ImmutableReturnUnitObservable()
		{
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<Unit> observer)
		{
			observer.OnNext(Unit.Default);
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}
}
