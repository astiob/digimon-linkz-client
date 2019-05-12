using System;
using System.Runtime.CompilerServices;

namespace UniRx.Operators
{
	internal class CatchObservable<T, TException> : OperatorObservableBase<T> where TException : Exception
	{
		private readonly IObservable<T> source;

		private readonly Func<TException, IObservable<T>> errorHandler;

		public CatchObservable(IObservable<T> source, Func<TException, IObservable<T>> errorHandler) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.errorHandler = errorHandler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new CatchObservable<T, TException>.Catch(this, observer, cancel).Run();
		}

		private class Catch : OperatorObserverBase<T, T>
		{
			private readonly CatchObservable<T, TException> parent;

			private SingleAssignmentDisposable sourceSubscription;

			private SingleAssignmentDisposable exceptionSubscription;

			[CompilerGenerated]
			private static Func<TException, IObservable<T>> <>f__mg$cache0;

			public Catch(CatchObservable<T, TException> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.sourceSubscription = new SingleAssignmentDisposable();
				this.exceptionSubscription = new SingleAssignmentDisposable();
				this.sourceSubscription.Disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.sourceSubscription, this.exceptionSubscription);
			}

			public override void OnNext(T value)
			{
				this.observer.OnNext(value);
			}

			public override void OnError(Exception error)
			{
				TException ex = error as TException;
				if (ex != null)
				{
					IObservable<T> observable;
					try
					{
						Delegate errorHandler = this.parent.errorHandler;
						if (CatchObservable<T, TException>.Catch.<>f__mg$cache0 == null)
						{
							CatchObservable<T, TException>.Catch.<>f__mg$cache0 = new Func<TException, IObservable<T>>(Stubs.CatchIgnore<T>);
						}
						if (errorHandler == CatchObservable<T, TException>.Catch.<>f__mg$cache0)
						{
							observable = Observable.Empty<T>();
						}
						else
						{
							observable = this.parent.errorHandler(ex);
						}
					}
					catch (Exception error2)
					{
						try
						{
							this.observer.OnError(error2);
						}
						finally
						{
							base.Dispose();
						}
						return;
					}
					this.exceptionSubscription.Disposable = observable.Subscribe(this.observer);
					return;
				}
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
