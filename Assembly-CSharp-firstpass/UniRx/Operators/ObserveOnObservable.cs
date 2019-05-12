using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ObserveOnObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IScheduler scheduler;

		public ObserveOnObservable(IObservable<T> source, IScheduler scheduler) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			ISchedulerQueueing schedulerQueueing = this.scheduler as ISchedulerQueueing;
			if (schedulerQueueing == null)
			{
				return new ObserveOnObservable<T>.ObserveOn(this, observer, cancel).Run();
			}
			return new ObserveOnObservable<T>.ObserveOn_(this, schedulerQueueing, observer, cancel).Run();
		}

		private class ObserveOn : OperatorObserverBase<T, T>
		{
			private readonly ObserveOnObservable<T> parent;

			private readonly LinkedList<ObserveOnObservable<T>.ObserveOn.SchedulableAction> actions = new LinkedList<ObserveOnObservable<T>.ObserveOn.SchedulableAction>();

			private bool isDisposed;

			public ObserveOn(ObserveOnObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.isDisposed = false;
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, Disposable.Create(delegate
				{
					object obj = this.actions;
					lock (obj)
					{
						this.isDisposed = true;
						while (this.actions.Count > 0)
						{
							this.actions.First.Value.Dispose();
						}
					}
				}));
			}

			public override void OnNext(T value)
			{
				this.QueueAction(new Notification<T>.OnNextNotification(value));
			}

			public override void OnError(Exception error)
			{
				this.QueueAction(new Notification<T>.OnErrorNotification(error));
			}

			public override void OnCompleted()
			{
				this.QueueAction(new Notification<T>.OnCompletedNotification());
			}

			private void QueueAction(Notification<T> data)
			{
				ObserveOnObservable<T>.ObserveOn.SchedulableAction schedulableAction = new ObserveOnObservable<T>.ObserveOn.SchedulableAction
				{
					data = data
				};
				object obj = this.actions;
				lock (obj)
				{
					if (!this.isDisposed)
					{
						schedulableAction.node = this.actions.AddLast(schedulableAction);
						this.ProcessNext();
					}
				}
			}

			private void ProcessNext()
			{
				object obj = this.actions;
				lock (obj)
				{
					if (this.actions.Count != 0 && !this.isDisposed)
					{
						ObserveOnObservable<T>.ObserveOn.SchedulableAction action = this.actions.First.Value;
						if (!action.IsScheduled)
						{
							action.schedule = this.parent.scheduler.Schedule(delegate()
							{
								try
								{
									NotificationKind kind = action.data.Kind;
									if (kind != NotificationKind.OnNext)
									{
										if (kind != NotificationKind.OnError)
										{
											if (kind == NotificationKind.OnCompleted)
											{
												this.observer.OnCompleted();
											}
										}
										else
										{
											this.observer.OnError(action.data.Exception);
										}
									}
									else
									{
										this.observer.OnNext(action.data.Value);
									}
								}
								finally
								{
									object obj2 = this.actions;
									lock (obj2)
									{
										action.Dispose();
									}
									if (action.data.Kind == NotificationKind.OnNext)
									{
										this.ProcessNext();
									}
									else
									{
										this.Dispose();
									}
								}
							});
						}
					}
				}
			}

			private class SchedulableAction : IDisposable
			{
				public Notification<T> data;

				public LinkedListNode<ObserveOnObservable<T>.ObserveOn.SchedulableAction> node;

				public IDisposable schedule;

				public void Dispose()
				{
					if (this.schedule != null)
					{
						this.schedule.Dispose();
					}
					this.schedule = null;
					if (this.node.List != null)
					{
						this.node.List.Remove(this.node);
					}
				}

				public bool IsScheduled
				{
					get
					{
						return this.schedule != null;
					}
				}
			}
		}

		private class ObserveOn_ : OperatorObserverBase<T, T>
		{
			private readonly ObserveOnObservable<T> parent;

			private readonly ISchedulerQueueing scheduler;

			private readonly BooleanDisposable isDisposed;

			private readonly Action<T> onNext;

			public ObserveOn_(ObserveOnObservable<T> parent, ISchedulerQueueing scheduler, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.scheduler = scheduler;
				this.isDisposed = new BooleanDisposable();
				this.onNext = new Action<T>(this.OnNext_);
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, this.isDisposed);
			}

			private void OnNext_(T value)
			{
				this.observer.OnNext(value);
			}

			private void OnError_(Exception error)
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

			private void OnCompleted_(Unit _)
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

			public override void OnNext(T value)
			{
				this.scheduler.ScheduleQueueing<T>(this.isDisposed, value, this.onNext);
			}

			public override void OnError(Exception error)
			{
				this.scheduler.ScheduleQueueing<Exception>(this.isDisposed, error, new Action<Exception>(this.OnError_));
			}

			public override void OnCompleted()
			{
				this.scheduler.ScheduleQueueing<Unit>(this.isDisposed, Unit.Default, new Action<Unit>(this.OnCompleted_));
			}
		}
	}
}
