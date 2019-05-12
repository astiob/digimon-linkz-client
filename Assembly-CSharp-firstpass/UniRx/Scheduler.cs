using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using UniRx.InternalUtil;
using UnityEngine;

namespace UniRx
{
	public static class Scheduler
	{
		public static readonly IScheduler CurrentThread = new Scheduler.CurrentThreadScheduler();

		public static readonly IScheduler Immediate = new Scheduler.ImmediateScheduler();

		public static readonly IScheduler ThreadPool = new Scheduler.ThreadPoolScheduler();

		private static IScheduler mainThread;

		private static IScheduler mainThreadIgnoreTimeScale;

		private static IScheduler mainThreadFixedUpdate;

		private static IScheduler mainThreadEndOfFrame;

		public static bool IsCurrentThreadSchedulerScheduleRequired
		{
			get
			{
				return Scheduler.CurrentThreadScheduler.IsScheduleRequired;
			}
		}

		public static DateTimeOffset Now
		{
			get
			{
				return DateTimeOffset.UtcNow;
			}
		}

		public static TimeSpan Normalize(TimeSpan timeSpan)
		{
			return (!(timeSpan >= TimeSpan.Zero)) ? TimeSpan.Zero : timeSpan;
		}

		public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
		{
			return scheduler.Schedule(dueTime - scheduler.Now, action);
		}

		public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate()
			{
				action(delegate
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					object gate;
					d = scheduler.Schedule(delegate()
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction();
					});
					gate = gate;
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(recursiveAction));
			return group;
		}

		public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate()
			{
				action(delegate(TimeSpan dt)
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					object gate;
					d = scheduler.Schedule(dt, delegate()
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction();
					});
					gate = gate;
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(dueTime, recursiveAction));
			return group;
		}

		public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
		{
			CompositeDisposable group = new CompositeDisposable(1);
			object gate = new object();
			Action recursiveAction = null;
			recursiveAction = delegate()
			{
				action(delegate(DateTimeOffset dt)
				{
					bool isAdded = false;
					bool isDone = false;
					IDisposable d = null;
					object gate;
					d = scheduler.Schedule(dt, delegate()
					{
						object gate2 = gate;
						lock (gate2)
						{
							if (isAdded)
							{
								group.Remove(d);
							}
							else
							{
								isDone = true;
							}
						}
						recursiveAction();
					});
					gate = gate;
					lock (gate)
					{
						if (!isDone)
						{
							group.Add(d);
							isAdded = true;
						}
					}
				});
			};
			group.Add(scheduler.Schedule(dueTime, recursiveAction));
			return group;
		}

		public static void SetDefaultForUnity()
		{
			Scheduler.DefaultSchedulers.ConstantTimeOperations = Scheduler.Immediate;
			Scheduler.DefaultSchedulers.TailRecursion = Scheduler.Immediate;
			Scheduler.DefaultSchedulers.Iteration = Scheduler.CurrentThread;
			Scheduler.DefaultSchedulers.TimeBasedOperations = Scheduler.MainThread;
			Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.ThreadPool;
		}

		public static IScheduler MainThread
		{
			get
			{
				IScheduler result;
				if ((result = Scheduler.mainThread) == null)
				{
					result = (Scheduler.mainThread = new Scheduler.MainThreadScheduler());
				}
				return result;
			}
		}

		public static IScheduler MainThreadIgnoreTimeScale
		{
			get
			{
				IScheduler result;
				if ((result = Scheduler.mainThreadIgnoreTimeScale) == null)
				{
					result = (Scheduler.mainThreadIgnoreTimeScale = new Scheduler.IgnoreTimeScaleMainThreadScheduler());
				}
				return result;
			}
		}

		public static IScheduler MainThreadFixedUpdate
		{
			get
			{
				IScheduler result;
				if ((result = Scheduler.mainThreadFixedUpdate) == null)
				{
					result = (Scheduler.mainThreadFixedUpdate = new Scheduler.FixedUpdateMainThreadScheduler());
				}
				return result;
			}
		}

		public static IScheduler MainThreadEndOfFrame
		{
			get
			{
				IScheduler result;
				if ((result = Scheduler.mainThreadEndOfFrame) == null)
				{
					result = (Scheduler.mainThreadEndOfFrame = new Scheduler.EndOfFrameMainThreadScheduler());
				}
				return result;
			}
		}

		private class CurrentThreadScheduler : IScheduler
		{
			[ThreadStatic]
			private static SchedulerQueue s_threadLocalQueue;

			[ThreadStatic]
			private static Stopwatch s_clock;

			private static SchedulerQueue GetQueue()
			{
				return Scheduler.CurrentThreadScheduler.s_threadLocalQueue;
			}

			private static void SetQueue(SchedulerQueue newQueue)
			{
				Scheduler.CurrentThreadScheduler.s_threadLocalQueue = newQueue;
			}

			private static TimeSpan Time
			{
				get
				{
					if (Scheduler.CurrentThreadScheduler.s_clock == null)
					{
						Scheduler.CurrentThreadScheduler.s_clock = Stopwatch.StartNew();
					}
					return Scheduler.CurrentThreadScheduler.s_clock.Elapsed;
				}
			}

			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public static bool IsScheduleRequired
			{
				get
				{
					return Scheduler.CurrentThreadScheduler.GetQueue() == null;
				}
			}

			public IDisposable Schedule(Action action)
			{
				return this.Schedule(TimeSpan.Zero, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				TimeSpan dueTime2 = Scheduler.CurrentThreadScheduler.Time + Scheduler.Normalize(dueTime);
				ScheduledItem scheduledItem = new ScheduledItem(action, dueTime2);
				SchedulerQueue schedulerQueue = Scheduler.CurrentThreadScheduler.GetQueue();
				if (schedulerQueue == null)
				{
					schedulerQueue = new SchedulerQueue(4);
					schedulerQueue.Enqueue(scheduledItem);
					Scheduler.CurrentThreadScheduler.SetQueue(schedulerQueue);
					try
					{
						Scheduler.CurrentThreadScheduler.Trampoline.Run(schedulerQueue);
					}
					finally
					{
						Scheduler.CurrentThreadScheduler.SetQueue(null);
					}
				}
				else
				{
					schedulerQueue.Enqueue(scheduledItem);
				}
				return scheduledItem.Cancellation;
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			private static class Trampoline
			{
				public static void Run(SchedulerQueue queue)
				{
					while (queue.Count > 0)
					{
						ScheduledItem scheduledItem = queue.Dequeue();
						if (!scheduledItem.IsCanceled)
						{
							TimeSpan timeout = scheduledItem.DueTime - Scheduler.CurrentThreadScheduler.Time;
							if (timeout.Ticks > 0L)
							{
								Thread.Sleep(timeout);
							}
							if (!scheduledItem.IsCanceled)
							{
								scheduledItem.Invoke();
							}
						}
					}
				}
			}
		}

		private class ImmediateScheduler : IScheduler
		{
			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IDisposable Schedule(Action action)
			{
				action();
				return Disposable.Empty;
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				TimeSpan timeout = Scheduler.Normalize(dueTime);
				if (timeout.Ticks > 0L)
				{
					Thread.Sleep(timeout);
				}
				action();
				return Disposable.Empty;
			}
		}

		public static class DefaultSchedulers
		{
			private static IScheduler constantTime;

			private static IScheduler tailRecursion;

			private static IScheduler iteration;

			private static IScheduler timeBasedOperations;

			private static IScheduler asyncConversions;

			public static IScheduler ConstantTimeOperations
			{
				get
				{
					IScheduler result;
					if ((result = Scheduler.DefaultSchedulers.constantTime) == null)
					{
						result = (Scheduler.DefaultSchedulers.constantTime = Scheduler.Immediate);
					}
					return result;
				}
				set
				{
					Scheduler.DefaultSchedulers.constantTime = value;
				}
			}

			public static IScheduler TailRecursion
			{
				get
				{
					IScheduler result;
					if ((result = Scheduler.DefaultSchedulers.tailRecursion) == null)
					{
						result = (Scheduler.DefaultSchedulers.tailRecursion = Scheduler.Immediate);
					}
					return result;
				}
				set
				{
					Scheduler.DefaultSchedulers.tailRecursion = value;
				}
			}

			public static IScheduler Iteration
			{
				get
				{
					IScheduler result;
					if ((result = Scheduler.DefaultSchedulers.iteration) == null)
					{
						result = (Scheduler.DefaultSchedulers.iteration = Scheduler.CurrentThread);
					}
					return result;
				}
				set
				{
					Scheduler.DefaultSchedulers.iteration = value;
				}
			}

			public static IScheduler TimeBasedOperations
			{
				get
				{
					IScheduler result;
					if ((result = Scheduler.DefaultSchedulers.timeBasedOperations) == null)
					{
						result = (Scheduler.DefaultSchedulers.timeBasedOperations = Scheduler.MainThread);
					}
					return result;
				}
				set
				{
					Scheduler.DefaultSchedulers.timeBasedOperations = value;
				}
			}

			public static IScheduler AsyncConversions
			{
				get
				{
					IScheduler result;
					if ((result = Scheduler.DefaultSchedulers.asyncConversions) == null)
					{
						result = (Scheduler.DefaultSchedulers.asyncConversions = Scheduler.ThreadPool);
					}
					return result;
				}
				set
				{
					Scheduler.DefaultSchedulers.asyncConversions = value;
				}
			}

			public static void SetDotNetCompatible()
			{
				Scheduler.DefaultSchedulers.ConstantTimeOperations = Scheduler.Immediate;
				Scheduler.DefaultSchedulers.TailRecursion = Scheduler.Immediate;
				Scheduler.DefaultSchedulers.Iteration = Scheduler.CurrentThread;
				Scheduler.DefaultSchedulers.TimeBasedOperations = Scheduler.ThreadPool;
				Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.ThreadPool;
			}
		}

		private class ThreadPoolScheduler : IScheduler, ISchedulerPeriodic
		{
			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable d = new BooleanDisposable();
				System.Threading.ThreadPool.QueueUserWorkItem(delegate(object _)
				{
					if (!d.IsDisposed)
					{
						action();
					}
				});
				return d;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				return new Scheduler.ThreadPoolScheduler.Timer(dueTime, action);
			}

			public IDisposable SchedulePeriodic(TimeSpan period, Action action)
			{
				return new Scheduler.ThreadPoolScheduler.PeriodicTimer(period, action);
			}

			public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(delegate(object callBackState)
				{
					if (!cancel.IsDisposed)
					{
						action((T)((object)callBackState));
					}
				}, state);
			}

			private sealed class Timer : IDisposable
			{
				private static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();

				private readonly SingleAssignmentDisposable _disposable;

				private Action _action;

				private System.Threading.Timer _timer;

				private bool _hasAdded;

				private bool _hasRemoved;

				public Timer(TimeSpan dueTime, Action action)
				{
					this._disposable = new SingleAssignmentDisposable();
					this._disposable.Disposable = Disposable.Create(new Action(this.Unroot));
					this._action = action;
					this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), null, dueTime, TimeSpan.FromMilliseconds(-1.0));
					object obj = Scheduler.ThreadPoolScheduler.Timer.s_timers;
					lock (obj)
					{
						if (!this._hasRemoved)
						{
							Scheduler.ThreadPoolScheduler.Timer.s_timers.Add(this._timer);
							this._hasAdded = true;
						}
					}
				}

				private void Tick(object state)
				{
					try
					{
						if (!this._disposable.IsDisposed)
						{
							this._action();
						}
					}
					finally
					{
						this.Unroot();
					}
				}

				private void Unroot()
				{
					this._action = Stubs.Nop;
					System.Threading.Timer timer = null;
					object obj = Scheduler.ThreadPoolScheduler.Timer.s_timers;
					lock (obj)
					{
						if (!this._hasRemoved)
						{
							timer = this._timer;
							this._timer = null;
							if (this._hasAdded && timer != null)
							{
								Scheduler.ThreadPoolScheduler.Timer.s_timers.Remove(timer);
							}
							this._hasRemoved = true;
						}
					}
					if (timer != null)
					{
						timer.Dispose();
					}
				}

				public void Dispose()
				{
					this._disposable.Dispose();
				}
			}

			private sealed class PeriodicTimer : IDisposable
			{
				private static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();

				private Action _action;

				private System.Threading.Timer _timer;

				private readonly AsyncLock _gate;

				public PeriodicTimer(TimeSpan period, Action action)
				{
					this._action = action;
					this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), null, period, period);
					this._gate = new AsyncLock();
					object obj = Scheduler.ThreadPoolScheduler.PeriodicTimer.s_timers;
					lock (obj)
					{
						Scheduler.ThreadPoolScheduler.PeriodicTimer.s_timers.Add(this._timer);
					}
				}

				private void Tick(object state)
				{
					this._gate.Wait(delegate
					{
						this._action();
					});
				}

				public void Dispose()
				{
					System.Threading.Timer timer = null;
					object obj = Scheduler.ThreadPoolScheduler.PeriodicTimer.s_timers;
					lock (obj)
					{
						timer = this._timer;
						this._timer = null;
						if (timer != null)
						{
							Scheduler.ThreadPoolScheduler.PeriodicTimer.s_timers.Remove(timer);
						}
					}
					if (timer != null)
					{
						timer.Dispose();
						this._action = Stubs.Nop;
					}
				}
			}
		}

		private class MainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
		{
			private readonly Action<object> scheduleAction;

			public MainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
				this.scheduleAction = new Action<object>(this.Schedule);
			}

			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				if (dueTime == TimeSpan.Zero)
				{
					yield return null;
				}
				else
				{
					yield return new WaitForSeconds((float)dueTime.TotalSeconds);
				}
				if (cancellation.IsDisposed)
				{
					yield break;
				}
				MainThreadDispatcher.UnsafeSend(action);
				yield break;
			}

			private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
			{
				if (period == TimeSpan.Zero)
				{
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				float seconds = (float)(period.TotalMilliseconds / 1000.0);
				WaitForSeconds yieldInstruction = new WaitForSeconds(seconds);
				for (;;)
				{
					yield return yieldInstruction;
					if (cancellation.IsDisposed)
					{
						break;
					}
					MainThreadDispatcher.UnsafeSend(action);
				}
				yield break;
				yield break;
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			private void Schedule(object state)
			{
				Tuple<BooleanDisposable, Action> tuple = (Tuple<BooleanDisposable, Action>)state;
				if (!tuple.Item1.IsDisposed)
				{
					tuple.Item2();
				}
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				MainThreadDispatcher.Post(this.scheduleAction, Tuple.Create<BooleanDisposable, Action>(booleanDisposable, action));
				return booleanDisposable;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				MainThreadDispatcher.SendStartCoroutine(this.DelayAction(dueTime2, action, booleanDisposable));
				return booleanDisposable;
			}

			public IDisposable SchedulePeriodic(TimeSpan period, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan period2 = Scheduler.Normalize(period);
				MainThreadDispatcher.SendStartCoroutine(this.PeriodicAction(period2, action, booleanDisposable));
				return booleanDisposable;
			}

			private void ScheduleQueueing<T>(object state)
			{
				Tuple<ICancelable, T, Action<T>> tuple = (Tuple<ICancelable, T, Action<T>>)state;
				if (!tuple.Item1.IsDisposed)
				{
					tuple.Item3(tuple.Item2);
				}
			}

			public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
			{
				MainThreadDispatcher.Post(Scheduler.MainThreadScheduler.QueuedAction<T>.Instance, Tuple.Create<ICancelable, T, Action<T>>(cancel, state, action));
			}

			private static class QueuedAction<T>
			{
				public static readonly Action<object> Instance = new Action<object>(Scheduler.MainThreadScheduler.QueuedAction<T>.Invoke);

				public static void Invoke(object state)
				{
					Tuple<ICancelable, T, Action<T>> tuple = (Tuple<ICancelable, T, Action<T>>)state;
					if (!tuple.Item1.IsDisposed)
					{
						tuple.Item3(tuple.Item2);
					}
				}
			}
		}

		private class IgnoreTimeScaleMainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
		{
			private readonly Action<object> scheduleAction;

			public IgnoreTimeScaleMainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
				this.scheduleAction = new Action<object>(this.Schedule);
			}

			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				if (dueTime == TimeSpan.Zero)
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						yield break;
					}
					MainThreadDispatcher.UnsafeSend(action);
				}
				else
				{
					float elapsed = 0f;
					float dt = (float)dueTime.TotalSeconds;
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						elapsed += Time.unscaledDeltaTime;
						if (elapsed >= dt)
						{
							goto Block_4;
						}
					}
					goto IL_FF;
					Block_4:
					MainThreadDispatcher.UnsafeSend(action);
				}
				IL_FF:
				yield break;
			}

			private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
			{
				if (period == TimeSpan.Zero)
				{
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				float elapsed = 0f;
				float dt = (float)period.TotalSeconds;
				for (;;)
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						break;
					}
					elapsed += Time.unscaledDeltaTime;
					if (elapsed >= dt)
					{
						MainThreadDispatcher.UnsafeSend(action);
						elapsed = 0f;
					}
				}
				yield break;
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			private void Schedule(object state)
			{
				Tuple<BooleanDisposable, Action> tuple = (Tuple<BooleanDisposable, Action>)state;
				if (!tuple.Item1.IsDisposed)
				{
					tuple.Item2();
				}
			}

			public IDisposable Schedule(Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				MainThreadDispatcher.Post(this.scheduleAction, Tuple.Create<BooleanDisposable, Action>(booleanDisposable, action));
				return booleanDisposable;
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				MainThreadDispatcher.SendStartCoroutine(this.DelayAction(dueTime2, action, booleanDisposable));
				return booleanDisposable;
			}

			public IDisposable SchedulePeriodic(TimeSpan period, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan period2 = Scheduler.Normalize(period);
				MainThreadDispatcher.SendStartCoroutine(this.PeriodicAction(period2, action, booleanDisposable));
				return booleanDisposable;
			}

			public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
			{
				MainThreadDispatcher.Post(Scheduler.IgnoreTimeScaleMainThreadScheduler.QueuedAction<T>.Instance, Tuple.Create<ICancelable, T, Action<T>>(cancel, state, action));
			}

			private static class QueuedAction<T>
			{
				public static readonly Action<object> Instance = new Action<object>(Scheduler.IgnoreTimeScaleMainThreadScheduler.QueuedAction<T>.Invoke);

				public static void Invoke(object state)
				{
					Tuple<ICancelable, T, Action<T>> tuple = (Tuple<ICancelable, T, Action<T>>)state;
					if (!tuple.Item1.IsDisposed)
					{
						tuple.Item3(tuple.Item2);
					}
				}
			}
		}

		private class FixedUpdateMainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
		{
			public FixedUpdateMainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
			}

			private IEnumerator ImmediateAction<T>(T state, Action<T> action, ICancelable cancellation)
			{
				yield return null;
				if (cancellation.IsDisposed)
				{
					yield break;
				}
				MainThreadDispatcher.UnsafeSend<T>(action, state);
				yield break;
			}

			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				if (dueTime == TimeSpan.Zero)
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						yield break;
					}
					MainThreadDispatcher.UnsafeSend(action);
				}
				else
				{
					float startTime = Time.fixedTime;
					float dt = (float)dueTime.TotalSeconds;
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						float elapsed = Time.fixedTime - startTime;
						if (elapsed >= dt)
						{
							goto Block_4;
						}
					}
					goto IL_FF;
					Block_4:
					MainThreadDispatcher.UnsafeSend(action);
				}
				IL_FF:
				yield break;
			}

			private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
			{
				if (period == TimeSpan.Zero)
				{
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				float startTime = Time.fixedTime;
				float dt = (float)period.TotalSeconds;
				for (;;)
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						break;
					}
					float ft = Time.fixedTime;
					float elapsed = ft - startTime;
					if (elapsed >= dt)
					{
						MainThreadDispatcher.UnsafeSend(action);
						startTime = ft;
					}
				}
				yield break;
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IDisposable Schedule(Action action)
			{
				return this.Schedule(TimeSpan.Zero, action);
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.DelayAction(dueTime2, action, booleanDisposable));
				return booleanDisposable;
			}

			public IDisposable SchedulePeriodic(TimeSpan period, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan period2 = Scheduler.Normalize(period);
				MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.PeriodicAction(period2, action, booleanDisposable));
				return booleanDisposable;
			}

			public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
			{
				MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.ImmediateAction<T>(state, action, cancel));
			}
		}

		private class EndOfFrameMainThreadScheduler : IScheduler, ISchedulerPeriodic, ISchedulerQueueing
		{
			public EndOfFrameMainThreadScheduler()
			{
				MainThreadDispatcher.Initialize();
			}

			private IEnumerator ImmediateAction<T>(T state, Action<T> action, ICancelable cancellation)
			{
				yield return null;
				if (cancellation.IsDisposed)
				{
					yield break;
				}
				MainThreadDispatcher.UnsafeSend<T>(action, state);
				yield break;
			}

			private IEnumerator DelayAction(TimeSpan dueTime, Action action, ICancelable cancellation)
			{
				if (dueTime == TimeSpan.Zero)
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						yield break;
					}
					MainThreadDispatcher.UnsafeSend(action);
				}
				else
				{
					float elapsed = 0f;
					float dt = (float)dueTime.TotalSeconds;
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						elapsed += Time.deltaTime;
						if (elapsed >= dt)
						{
							goto Block_4;
						}
					}
					goto IL_FF;
					Block_4:
					MainThreadDispatcher.UnsafeSend(action);
				}
				IL_FF:
				yield break;
			}

			private IEnumerator PeriodicAction(TimeSpan period, Action action, ICancelable cancellation)
			{
				if (period == TimeSpan.Zero)
				{
					for (;;)
					{
						yield return null;
						if (cancellation.IsDisposed)
						{
							break;
						}
						MainThreadDispatcher.UnsafeSend(action);
					}
					yield break;
				}
				float elapsed = 0f;
				float dt = (float)period.TotalSeconds;
				for (;;)
				{
					yield return null;
					if (cancellation.IsDisposed)
					{
						break;
					}
					elapsed += Time.deltaTime;
					if (elapsed >= dt)
					{
						MainThreadDispatcher.UnsafeSend(action);
						elapsed = 0f;
					}
				}
				yield break;
			}

			public DateTimeOffset Now
			{
				get
				{
					return Scheduler.Now;
				}
			}

			public IDisposable Schedule(Action action)
			{
				return this.Schedule(TimeSpan.Zero, action);
			}

			public IDisposable Schedule(DateTimeOffset dueTime, Action action)
			{
				return this.Schedule(dueTime - this.Now, action);
			}

			public IDisposable Schedule(TimeSpan dueTime, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan dueTime2 = Scheduler.Normalize(dueTime);
				MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.DelayAction(dueTime2, action, booleanDisposable));
				return booleanDisposable;
			}

			public IDisposable SchedulePeriodic(TimeSpan period, Action action)
			{
				BooleanDisposable booleanDisposable = new BooleanDisposable();
				TimeSpan period2 = Scheduler.Normalize(period);
				MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.PeriodicAction(period2, action, booleanDisposable));
				return booleanDisposable;
			}

			public void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action)
			{
				MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.ImmediateAction<T>(state, action, cancel));
			}
		}
	}
}
