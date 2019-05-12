using System;

namespace UniRx.Operators
{
	internal class ContinueWithObservable<TSource, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TSource> source;

		private readonly Func<TSource, IObservable<TResult>> selector;

		public ContinueWithObservable(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.selector = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			return new ContinueWithObservable<TSource, TResult>.ContinueWith(this, observer, cancel).Run();
		}

		private class ContinueWith : OperatorObserverBase<TSource, TResult>
		{
			private readonly ContinueWithObservable<TSource, TResult> parent;

			private readonly SerialDisposable serialDisposable = new SerialDisposable();

			private bool seenValue;

			private TSource lastValue;

			public ContinueWith(ContinueWithObservable<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.serialDisposable.Disposable = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = this.parent.source.Subscribe(this);
				return this.serialDisposable;
			}

			public override void OnNext(TSource value)
			{
				this.seenValue = true;
				this.lastValue = value;
			}

			public override void OnError(Exception error)
			{
				try
				{
					this.observer.OnError(error);
				}
				finally
				{
					base.Dispose();
				}
			}

			public override void OnCompleted()
			{
				if (this.seenValue)
				{
					IObservable<TResult> observable = this.parent.selector(this.lastValue);
					this.serialDisposable.Disposable = observable.Subscribe(this.observer);
				}
				else
				{
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
				}
			}
		}
	}
}
