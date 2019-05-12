using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class BufferObservable<T> : OperatorObservableBase<IList<T>>
	{
		private readonly IObservable<T> source;

		private readonly int count;

		private readonly int skip;

		private readonly TimeSpan timeSpan;

		private readonly TimeSpan timeShift;

		private readonly IScheduler scheduler;

		public BufferObservable(IObservable<T> source, int count, int skip) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.count = count;
			this.skip = skip;
		}

		public BufferObservable(IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.timeSpan = timeSpan;
			this.timeShift = timeShift;
			this.scheduler = scheduler;
		}

		public BufferObservable(IObservable<T> source, TimeSpan timeSpan, int count, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread || source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.timeSpan = timeSpan;
			this.count = count;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<T>> observer, IDisposable cancel)
		{
			if (this.scheduler == null)
			{
				if (this.skip == 0)
				{
					return new BufferObservable<T>.Buffer(this, observer, cancel).Run();
				}
				return new BufferObservable<T>.Buffer_(this, observer, cancel).Run();
			}
			else
			{
				if (this.count > 0)
				{
					return new BufferObservable<T>.BufferTC(this, observer, cancel).Run();
				}
				if (this.timeSpan == this.timeShift)
				{
					return new BufferObservable<T>.BufferT(this, observer, cancel).Run();
				}
				return new BufferObservable<T>.BufferTS(this, observer, cancel).Run();
			}
		}

		private class Buffer : OperatorObserverBase<T, IList<T>>
		{
			private readonly BufferObservable<T> parent;

			private List<T> list;

			public Buffer(BufferObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.list = new List<T>(this.parent.count);
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				this.list.Add(value);
				if (this.list.Count == this.parent.count)
				{
					this.observer.OnNext(this.list);
					this.list = new List<T>(this.parent.count);
				}
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
				if (this.list.Count > 0)
				{
					this.observer.OnNext(this.list);
				}
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

		private class Buffer_ : OperatorObserverBase<T, IList<T>>
		{
			private readonly BufferObservable<T> parent;

			private Queue<List<T>> q;

			private int index;

			public Buffer_(BufferObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.q = new Queue<List<T>>();
				this.index = -1;
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				this.index++;
				if (this.index % this.parent.skip == 0)
				{
					this.q.Enqueue(new List<T>(this.parent.count));
				}
				int count = this.q.Count;
				for (int i = 0; i < count; i++)
				{
					List<T> list = this.q.Dequeue();
					list.Add(value);
					if (list.Count == this.parent.count)
					{
						this.observer.OnNext(list);
					}
					else
					{
						this.q.Enqueue(list);
					}
				}
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
				foreach (List<T> value in this.q)
				{
					this.observer.OnNext(value);
				}
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

		private class BufferT : OperatorObserverBase<T, IList<T>>
		{
			private static readonly T[] EmptyArray = new T[0];

			private readonly BufferObservable<T> parent;

			private readonly object gate = new object();

			private List<T> list;

			public BufferT(BufferObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.list = new List<T>();
				IDisposable disposable = Observable.Interval(this.parent.timeSpan, this.parent.scheduler).Subscribe(new BufferObservable<T>.BufferT.Buffer(this));
				IDisposable disposable2 = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, disposable2);
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.list.Add(value);
				}
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
				object obj = this.gate;
				List<T> value;
				lock (obj)
				{
					value = this.list;
				}
				this.observer.OnNext(value);
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
			}

			private class Buffer : IObserver<long>
			{
				private BufferObservable<T>.BufferT parent;

				public Buffer(BufferObservable<T>.BufferT parent)
				{
					this.parent = parent;
				}

				public void OnNext(long value)
				{
					bool flag = false;
					object gate = this.parent.gate;
					List<T> list;
					lock (gate)
					{
						list = this.parent.list;
						if (list.Count != 0)
						{
							this.parent.list = new List<T>();
						}
						else
						{
							flag = true;
						}
					}
					this.parent.observer.OnNext((!flag) ? list : BufferObservable<T>.BufferT.EmptyArray);
				}

				public void OnError(Exception error)
				{
				}

				public void OnCompleted()
				{
				}
			}
		}

		private class BufferTS : OperatorObserverBase<T, IList<T>>
		{
			private readonly BufferObservable<T> parent;

			private readonly object gate = new object();

			private Queue<IList<T>> q;

			private TimeSpan totalTime;

			private TimeSpan nextShift;

			private TimeSpan nextSpan;

			private SerialDisposable timerD;

			public BufferTS(BufferObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.totalTime = TimeSpan.Zero;
				this.nextShift = this.parent.timeShift;
				this.nextSpan = this.parent.timeSpan;
				this.q = new Queue<IList<T>>();
				this.timerD = new SerialDisposable();
				this.q.Enqueue(new List<T>());
				this.CreateTimer();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, this.timerD);
			}

			private void CreateTimer()
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.timerD.Disposable = singleAssignmentDisposable;
				bool isSpan = false;
				bool isShift = false;
				if (this.nextSpan == this.nextShift)
				{
					isSpan = true;
					isShift = true;
				}
				else if (this.nextSpan < this.nextShift)
				{
					isSpan = true;
				}
				else
				{
					isShift = true;
				}
				TimeSpan t = (!isSpan) ? this.nextShift : this.nextSpan;
				TimeSpan dueTime = t - this.totalTime;
				this.totalTime = t;
				if (isSpan)
				{
					this.nextSpan += this.parent.timeShift;
				}
				if (isShift)
				{
					this.nextShift += this.parent.timeShift;
				}
				singleAssignmentDisposable.Disposable = this.parent.scheduler.Schedule(dueTime, delegate()
				{
					object obj = this.gate;
					lock (obj)
					{
						if (isShift)
						{
							List<T> item = new List<T>();
							this.q.Enqueue(item);
						}
						if (isSpan)
						{
							IList<T> value = this.q.Dequeue();
							this.observer.OnNext(value);
						}
					}
					this.CreateTimer();
				});
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					foreach (IList<T> list in this.q)
					{
						list.Add(value);
					}
				}
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
				object obj = this.gate;
				lock (obj)
				{
					foreach (IList<T> value in this.q)
					{
						this.observer.OnNext(value);
					}
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

		private class BufferTC : OperatorObserverBase<T, IList<T>>
		{
			private static readonly T[] EmptyArray = new T[0];

			private readonly BufferObservable<T> parent;

			private readonly object gate = new object();

			private List<T> list;

			private long timerId;

			private SerialDisposable timerD;

			public BufferTC(BufferObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.list = new List<T>();
				this.timerId = 0L;
				this.timerD = new SerialDisposable();
				this.CreateTimer();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, this.timerD);
			}

			private void CreateTimer()
			{
				long currentTimerId = this.timerId;
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.timerD.Disposable = singleAssignmentDisposable;
				ISchedulerPeriodic schedulerPeriodic = this.parent.scheduler as ISchedulerPeriodic;
				if (schedulerPeriodic != null)
				{
					singleAssignmentDisposable.Disposable = schedulerPeriodic.SchedulePeriodic(this.parent.timeSpan, delegate
					{
						this.OnNextTick(currentTimerId);
					});
				}
				else
				{
					singleAssignmentDisposable.Disposable = this.parent.scheduler.Schedule(this.parent.timeSpan, delegate(Action<TimeSpan> self)
					{
						this.OnNextRecursive(currentTimerId, self);
					});
				}
			}

			private void OnNextTick(long currentTimerId)
			{
				bool flag = false;
				object obj = this.gate;
				List<T> list;
				lock (obj)
				{
					if (currentTimerId != this.timerId)
					{
						return;
					}
					list = this.list;
					if (list.Count != 0)
					{
						this.list = new List<T>();
					}
					else
					{
						flag = true;
					}
				}
				this.observer.OnNext((!flag) ? list : BufferObservable<T>.BufferTC.EmptyArray);
			}

			private void OnNextRecursive(long currentTimerId, Action<TimeSpan> self)
			{
				bool flag = false;
				object obj = this.gate;
				List<T> list;
				lock (obj)
				{
					if (currentTimerId != this.timerId)
					{
						return;
					}
					list = this.list;
					if (list.Count != 0)
					{
						this.list = new List<T>();
					}
					else
					{
						flag = true;
					}
				}
				this.observer.OnNext((!flag) ? list : BufferObservable<T>.BufferTC.EmptyArray);
				self(this.parent.timeSpan);
			}

			public override void OnNext(T value)
			{
				List<T> list = null;
				object obj = this.gate;
				lock (obj)
				{
					this.list.Add(value);
					if (this.list.Count == this.parent.count)
					{
						list = this.list;
						this.list = new List<T>();
						this.timerId += 1L;
						this.CreateTimer();
					}
				}
				if (list != null)
				{
					this.observer.OnNext(list);
				}
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
				object obj = this.gate;
				List<T> value;
				lock (obj)
				{
					this.timerId += 1L;
					value = this.list;
				}
				this.observer.OnNext(value);
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
