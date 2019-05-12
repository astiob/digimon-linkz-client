using System;
using System.Threading;
using UniRx.InternalUtil;

namespace UniRx.Operators
{
	public abstract class OperatorObserverBase<TSource, TResult> : IDisposable, IObserver<TSource>
	{
		protected internal volatile IObserver<TResult> observer;

		private IDisposable cancel;

		public OperatorObserverBase(IObserver<TResult> observer, IDisposable cancel)
		{
			this.observer = observer;
			this.cancel = cancel;
		}

		public abstract void OnNext(TSource value);

		public abstract void OnError(Exception error);

		public abstract void OnCompleted();

		public void Dispose()
		{
			this.observer = EmptyObserver<TResult>.Instance;
			IDisposable disposable = Interlocked.Exchange<IDisposable>(ref this.cancel, null);
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
