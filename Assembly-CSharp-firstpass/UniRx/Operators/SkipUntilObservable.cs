using System;
using UniRx.InternalUtil;

namespace UniRx.Operators
{
	internal class SkipUntilObservable<T, TOther> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IObservable<TOther> other;

		public SkipUntilObservable(IObservable<T> source, IObservable<TOther> other) : base(source.IsRequiredSubscribeOnCurrentThread<T>() || other.IsRequiredSubscribeOnCurrentThread<TOther>())
		{
			this.source = source;
			this.other = other;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new SkipUntilObservable<T, TOther>.SkipUntilOuterObserver(this, observer, cancel).Run();
		}

		private class SkipUntilOuterObserver : OperatorObserverBase<T, T>
		{
			private readonly SkipUntilObservable<T, TOther> parent;

			public SkipUntilOuterObserver(SkipUntilObservable<T, TOther> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				SkipUntilObservable<T, TOther>.SkipUntilOuterObserver.SkipUntil skipUntil = new SkipUntilObservable<T, TOther>.SkipUntilOuterObserver.SkipUntil(this, singleAssignmentDisposable);
				SingleAssignmentDisposable singleAssignmentDisposable2 = new SingleAssignmentDisposable();
				SkipUntilObservable<T, TOther>.SkipUntilOuterObserver.SkipUntilOther observer = new SkipUntilObservable<T, TOther>.SkipUntilOuterObserver.SkipUntilOther(this, skipUntil, singleAssignmentDisposable2);
				singleAssignmentDisposable.Disposable = this.parent.source.Subscribe(skipUntil);
				singleAssignmentDisposable2.Disposable = this.parent.other.Subscribe(observer);
				return StableCompositeDisposable.Create(singleAssignmentDisposable2, singleAssignmentDisposable);
			}

			public override void OnNext(T value)
			{
			}

			public override void OnError(Exception error)
			{
			}

			public override void OnCompleted()
			{
			}

			private class SkipUntil : IObserver<T>
			{
				public volatile IObserver<T> observer;

				private readonly SkipUntilObservable<T, TOther>.SkipUntilOuterObserver parent;

				private readonly IDisposable subscription;

				public SkipUntil(SkipUntilObservable<T, TOther>.SkipUntilOuterObserver parent, IDisposable subscription)
				{
					this.parent = parent;
					this.observer = EmptyObserver<T>.Instance;
					this.subscription = subscription;
				}

				public void OnNext(T value)
				{
					this.observer.OnNext(value);
				}

				public void OnError(Exception error)
				{
					try
					{
						this.observer.OnError(error);
					}
					finally
					{
						this.parent.Dispose();
					}
				}

				public void OnCompleted()
				{
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						this.subscription.Dispose();
					}
				}
			}

			private class SkipUntilOther : IObserver<TOther>
			{
				private readonly SkipUntilObservable<T, TOther>.SkipUntilOuterObserver parent;

				private readonly SkipUntilObservable<T, TOther>.SkipUntilOuterObserver.SkipUntil sourceObserver;

				private readonly IDisposable subscription;

				public SkipUntilOther(SkipUntilObservable<T, TOther>.SkipUntilOuterObserver parent, SkipUntilObservable<T, TOther>.SkipUntilOuterObserver.SkipUntil sourceObserver, IDisposable subscription)
				{
					this.parent = parent;
					this.sourceObserver = sourceObserver;
					this.subscription = subscription;
				}

				public void OnNext(TOther value)
				{
					this.sourceObserver.observer = this.parent.observer;
					this.subscription.Dispose();
				}

				public void OnError(Exception error)
				{
					try
					{
						this.parent.observer.OnError(error);
					}
					finally
					{
						this.parent.Dispose();
					}
				}

				public void OnCompleted()
				{
					this.subscription.Dispose();
				}
			}
		}
	}
}
