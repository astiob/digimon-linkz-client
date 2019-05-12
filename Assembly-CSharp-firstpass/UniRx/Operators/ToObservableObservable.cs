using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ToObservableObservable<T> : OperatorObservableBase<T>
	{
		private readonly IEnumerable<T> source;

		private readonly IScheduler scheduler;

		public ToObservableObservable(IEnumerable<T> source, IScheduler scheduler) : base(scheduler == Scheduler.CurrentThread)
		{
			this.source = source;
			this.scheduler = scheduler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new ToObservableObservable<T>.ToObservable(this, observer, cancel).Run();
		}

		private class ToObservable : OperatorObserverBase<T, T>
		{
			private readonly ToObservableObservable<T> parent;

			public ToObservable(ToObservableObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				IEnumerator<T> e = null;
				try
				{
					e = this.parent.source.GetEnumerator();
				}
				catch (Exception error)
				{
					this.OnError(error);
					return Disposable.Empty;
				}
				if (this.parent.scheduler == Scheduler.Immediate)
				{
					for (;;)
					{
						T value = default(T);
						bool flag3;
						try
						{
							flag3 = e.MoveNext();
							if (flag3)
							{
								value = e.Current;
							}
						}
						catch (Exception error2)
						{
							e.Dispose();
							try
							{
								this.observer.OnError(error2);
							}
							finally
							{
								base.Dispose();
							}
							goto IL_FC;
						}
						if (!flag3)
						{
							break;
						}
						this.observer.OnNext(value);
					}
					e.Dispose();
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					IL_FC:
					return Disposable.Empty;
				}
				SingleAssignmentDisposable flag = new SingleAssignmentDisposable();
				flag.Disposable = this.parent.scheduler.Schedule(delegate(Action self)
				{
					if (flag.IsDisposed)
					{
						e.Dispose();
						return;
					}
					T value2 = default(T);
					bool flag2;
					try
					{
						flag2 = e.MoveNext();
						if (flag2)
						{
							value2 = e.Current;
						}
					}
					catch (Exception error3)
					{
						e.Dispose();
						try
						{
							this.observer.OnError(error3);
						}
						finally
						{
							this.Dispose();
						}
						return;
					}
					if (flag2)
					{
						this.observer.OnNext(value2);
						self();
					}
					else
					{
						e.Dispose();
						try
						{
							this.observer.OnCompleted();
						}
						finally
						{
							this.Dispose();
						}
					}
				});
				return flag;
			}

			public override void OnNext(T value)
			{
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
