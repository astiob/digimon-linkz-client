using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx.Operators
{
	internal class RepeatUntilObservable<T> : OperatorObservableBase<T>
	{
		private readonly IEnumerable<IObservable<T>> sources;

		private readonly IObservable<Unit> trigger;

		private readonly GameObject lifeTimeChecker;

		public RepeatUntilObservable(IEnumerable<IObservable<T>> sources, IObservable<Unit> trigger, GameObject lifeTimeChecker) : base(true)
		{
			this.sources = sources;
			this.trigger = trigger;
			this.lifeTimeChecker = lifeTimeChecker;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new RepeatUntilObservable<T>.RepeatUntil(this, observer, cancel).Run();
		}

		private class RepeatUntil : OperatorObserverBase<T, T>
		{
			private readonly RepeatUntilObservable<T> parent;

			private readonly object gate = new object();

			private IEnumerator<IObservable<T>> e;

			private SerialDisposable subscription;

			private SingleAssignmentDisposable schedule;

			private Action nextSelf;

			private bool isStopped;

			private bool isDisposed;

			private bool isFirstSubscribe;

			private IDisposable stopper;

			public RepeatUntil(RepeatUntilObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.isFirstSubscribe = true;
				this.isDisposed = false;
				this.isStopped = false;
				this.e = this.parent.sources.GetEnumerator();
				this.subscription = new SerialDisposable();
				this.schedule = new SingleAssignmentDisposable();
				this.stopper = this.parent.trigger.Subscribe(delegate(Unit _)
				{
					object obj = this.gate;
					lock (obj)
					{
						this.isStopped = true;
						this.e.Dispose();
						this.subscription.Dispose();
						this.schedule.Dispose();
						this.observer.OnCompleted();
					}
				}, new Action<Exception>(this.observer.OnError));
				this.schedule.Disposable = Scheduler.CurrentThread.Schedule(new Action<Action>(this.RecursiveRun));
				return new CompositeDisposable(new IDisposable[]
				{
					this.schedule,
					this.subscription,
					this.stopper,
					Disposable.Create(delegate
					{
						object obj = this.gate;
						lock (obj)
						{
							this.isDisposed = true;
							this.e.Dispose();
						}
					})
				});
			}

			private void RecursiveRun(Action self)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.nextSelf = self;
					if (!this.isDisposed)
					{
						if (!this.isStopped)
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
								this.stopper.Dispose();
								this.observer.OnError(ex);
							}
							else if (!flag)
							{
								this.stopper.Dispose();
								this.observer.OnCompleted();
							}
							else
							{
								IObservable<T> observable = this.e.Current;
								SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
								this.subscription.Disposable = singleAssignmentDisposable;
								if (this.isFirstSubscribe)
								{
									this.isFirstSubscribe = false;
									singleAssignmentDisposable.Disposable = observable.Subscribe(this);
								}
								else
								{
									MainThreadDispatcher.SendStartCoroutine(RepeatUntilObservable<T>.RepeatUntil.SubscribeAfterEndOfFrame(singleAssignmentDisposable, observable, this, this.parent.lifeTimeChecker));
								}
							}
						}
					}
				}
			}

			private static IEnumerator SubscribeAfterEndOfFrame(SingleAssignmentDisposable d, IObservable<T> source, IObserver<T> observer, GameObject lifeTimeChecker)
			{
				yield return YieldInstructionCache.WaitForEndOfFrame;
				if (!d.IsDisposed && lifeTimeChecker != null)
				{
					d.Disposable = source.Subscribe(observer);
				}
				yield break;
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
				if (!this.isDisposed)
				{
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
