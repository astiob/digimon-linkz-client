using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ConcatObservable<T> : OperatorObservableBase<T>
	{
		private readonly IEnumerable<IObservable<T>> sources;

		public ConcatObservable(IEnumerable<IObservable<T>> sources) : base(true)
		{
			this.sources = sources;
		}

		public IObservable<T> Combine(IEnumerable<IObservable<T>> combineSources)
		{
			return new ConcatObservable<T>(ConcatObservable<T>.CombineSources(this.sources, combineSources));
		}

		private static IEnumerable<IObservable<T>> CombineSources(IEnumerable<IObservable<T>> first, IEnumerable<IObservable<T>> second)
		{
			foreach (IObservable<T> item in first)
			{
				yield return item;
			}
			foreach (IObservable<T> item2 in second)
			{
				yield return item2;
			}
			yield break;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new ConcatObservable<T>.Concat(this, observer, cancel).Run();
		}

		private class Concat : OperatorObserverBase<T, T>
		{
			private readonly ConcatObservable<T> parent;

			private readonly object gate = new object();

			private bool isDisposed;

			private IEnumerator<IObservable<T>> e;

			private SerialDisposable subscription;

			private Action nextSelf;

			public Concat(ConcatObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.isDisposed = false;
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
						IObservable<T> observable = null;
						bool flag = false;
						Exception ex = null;
						try
						{
							flag = this.e.MoveNext();
							if (flag)
							{
								observable = this.e.Current;
								if (observable == null)
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
							IObservable<T> observable2 = observable;
							SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
							this.subscription.Disposable = singleAssignmentDisposable;
							singleAssignmentDisposable.Disposable = observable2.Subscribe(this);
						}
					}
				}
			}

			public override void OnNext(T value)
			{
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
				this.nextSelf();
			}
		}
	}
}
