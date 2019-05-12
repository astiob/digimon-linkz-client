using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class RepeatSafeObservable<T> : OperatorObservableBase<T>
	{
		private readonly IEnumerable<IObservable<T>> sources;

		public RepeatSafeObservable(IEnumerable<IObservable<T>> sources, bool isRequiredSubscribeOnCurrentThread) : base(isRequiredSubscribeOnCurrentThread)
		{
			this.sources = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new RepeatSafeObservable<T>.RepeatSafe(this, observer, cancel).Run();
		}

		private class RepeatSafe : OperatorObserverBase<T, T>
		{
			private readonly RepeatSafeObservable<T> parent;

			private readonly object gate = new object();

			private IEnumerator<IObservable<T>> e;

			private SerialDisposable subscription;

			private Action nextSelf;

			private bool isDisposed;

			private bool isRunNext;

			public RepeatSafe(RepeatSafeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.isDisposed = false;
				this.isRunNext = false;
				this.e = this.parent.sources.GetEnumerator();
				this.subscription = new SerialDisposable();
				IDisposable disposable = Scheduler.DefaultSchedulers.TailRecursion.Schedule(new Action<Action>(this.RecursiveRun));
				return StableCompositeDisposable.Create(disposable, this.subscription, Disposable.Create(delegate
				{
					object obj = this.gate;
					lock (obj)
					{
						this.isDisposed = true;
						this.e.Dispose();
					}
				}));
			}

			private void RecursiveRun(Action self)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.nextSelf = self;
					if (!this.isDisposed)
					{
						bool flag = false;
						Exception ex = null;
						try
						{
							flag = this.e.MoveNext();
							if (flag)
							{
								if (this.e.Current == null)
								{
									throw new InvalidOperationException("sequence is null.");
								}
							}
							else
							{
								this.e.Dispose();
							}
						}
						catch (Exception ex2)
						{
							ex = ex2;
							this.e.Dispose();
						}
						if (ex != null)
						{
							try
							{
								this.observer.OnError(ex);
							}
							finally
							{
								base.Dispose();
							}
						}
						else if (!flag)
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
						else
						{
							IObservable<T> observable = this.e.Current;
							SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
							this.subscription.Disposable = singleAssignmentDisposable;
							singleAssignmentDisposable.Disposable = observable.Subscribe(this);
						}
					}
				}
			}

			public override void OnNext(T value)
			{
				this.isRunNext = true;
				this.observer.OnNext(value);
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
				if (this.isRunNext && !this.isDisposed)
				{
					this.isRunNext = false;
					this.nextSelf();
				}
				else
				{
					this.e.Dispose();
					if (!this.isDisposed)
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
}
