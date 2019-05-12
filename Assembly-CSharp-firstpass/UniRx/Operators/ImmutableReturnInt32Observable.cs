using System;

namespace UniRx.Operators
{
	internal class ImmutableReturnInt32Observable : IObservable<int>, IOptimizedObservable<int>
	{
		private static ImmutableReturnInt32Observable[] Caches = new ImmutableReturnInt32Observable[]
		{
			new ImmutableReturnInt32Observable(-1),
			new ImmutableReturnInt32Observable(0),
			new ImmutableReturnInt32Observable(1),
			new ImmutableReturnInt32Observable(2),
			new ImmutableReturnInt32Observable(3),
			new ImmutableReturnInt32Observable(4),
			new ImmutableReturnInt32Observable(5),
			new ImmutableReturnInt32Observable(6),
			new ImmutableReturnInt32Observable(7),
			new ImmutableReturnInt32Observable(8),
			new ImmutableReturnInt32Observable(9)
		};

		private readonly int x;

		private ImmutableReturnInt32Observable(int x)
		{
			this.x = x;
		}

		public static IObservable<int> GetInt32Observable(int x)
		{
			if (-1 <= x && x <= 9)
			{
				return ImmutableReturnInt32Observable.Caches[x + 1];
			}
			return new ImmediateReturnObservable<int>(x);
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<int> observer)
		{
			observer.OnNext(this.x);
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}
}
