using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class DelayObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly TimeSpan dueTime;

		private readonly IScheduler scheduler;

		public DelayObservable(IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.dueTime = dueTime;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DelayObservable<T>.Delay(this, observer, cancel).Run();
		}

		private class Delay : OperatorObserverBase<T, T>
		{
			private readonly DelayObservable<T> parent;

			private readonly object gate = new object();

			private bool hasFailed;

			private bool running;

			private bool active;

			private Exception exception;

			private Queue<Timestamped<T>> queue;

			private bool onCompleted;

			private DateTimeOffset completeAt;

			private IDisposable sourceSubscription;

			private TimeSpan delay;

			private bool ready;

			private SerialDisposable cancelable;

			public Delay(DelayObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.cancelable = new SerialDisposable();
				this.active = false;
				this.running = false;
				this.queue = new Queue<Timestamped<T>>();
				this.onCompleted = false;
				this.completeAt = default(DateTimeOffset);
				this.hasFailed = false;
				this.exception = null;
				this.ready = true;
				this.delay = Scheduler.Normalize(this.parent.dueTime);
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.sourceSubscription = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.sourceSubscription, this.cancelable);
			}

			public override void OnNext(T value)
			{
				DateTimeOffset timestamp = this.parent.scheduler.Now.Add(this.delay);
				bool flag = false;
				object obj = this.gate;
				lock (obj)
				{
					this.queue.Enqueue(new Timestamped<T>(value, timestamp));
					flag = (this.ready && !this.active);
					this.active = true;
				}
				if (flag)
				{
					this.cancelable.Disposable = this.parent.scheduler.Schedule(this.delay, new Action<Action<TimeSpan>>(this.DrainQueue));
				}
			}

			public override void OnError(Exception error)
			{
				this.sourceSubscription.Dispose();
				bool flag = false;
				object obj = this.gate;
				lock (obj)
				{
					this.queue.Clear();
					this.exception = error;
					this.hasFailed = true;
					flag = !this.running;
				}
				if (flag)
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
			}

			public override void OnCompleted()
			{
				this.sourceSubscription.Dispose();
				DateTimeOffset dateTimeOffset = this.parent.scheduler.Now.Add(this.delay);
				bool flag = false;
				object obj = this.gate;
				lock (obj)
				{
					this.completeAt = dateTimeOffset;
					this.onCompleted = true;
					flag = (this.ready && !this.active);
					this.active = true;
				}
				if (flag)
				{
					this.cancelable.Disposable = this.parent.scheduler.Schedule(this.delay, new Action<Action<TimeSpan>>(this.DrainQueue));
				}
			}

			private void DrainQueue(Action<TimeSpan> recurse)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.hasFailed)
					{
						return;
					}
					this.running = true;
				}
				bool flag = false;
				bool flag2;
				Exception error;
				bool flag4;
				bool flag5;
				TimeSpan obj2;
				for (;;)
				{
					flag2 = false;
					error = null;
					bool flag3 = false;
					T value = default(T);
					flag4 = false;
					flag5 = false;
					obj2 = default(TimeSpan);
					object obj3 = this.gate;
					lock (obj3)
					{
						if (flag2)
						{
							error = this.exception;
							flag2 = true;
							this.running = false;
						}
						else if (this.queue.Count > 0)
						{
							DateTimeOffset timestamp = this.queue.Peek().Timestamp;
							if (timestamp.CompareTo(this.parent.scheduler.Now) <= 0 && !flag)
							{
								value = this.queue.Dequeue().Value;
								flag3 = true;
							}
							else
							{
								flag5 = true;
								obj2 = Scheduler.Normalize(timestamp.Subtract(this.parent.scheduler.Now));
								this.running = false;
							}
						}
						else if (this.onCompleted)
						{
							if (this.completeAt.CompareTo(this.parent.scheduler.Now) <= 0 && !flag)
							{
								flag4 = true;
							}
							else
							{
								flag5 = true;
								obj2 = Scheduler.Normalize(this.completeAt.Subtract(this.parent.scheduler.Now));
								this.running = false;
							}
						}
						else
						{
							this.running = false;
							this.active = false;
						}
					}
					if (!flag3)
					{
						break;
					}
					this.observer.OnNext(value);
					flag = true;
				}
				if (flag4)
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
				else if (flag2)
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
				else if (flag5)
				{
					recurse(obj2);
				}
			}
		}
	}
}
