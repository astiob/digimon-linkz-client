using System;

namespace UniRx.Operators
{
	internal class ImmediateReturnObservable<T> : IObservable<T>, IOptimizedObservable<T>
	{
		private readonly T value;

		public ImmediateReturnObservable(T value)
		{
			this.value = value;
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return false;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			observer.OnNext(this.value);
			observer.OnCompleted();
			return Disposable.Empty;
		}
	}
}
