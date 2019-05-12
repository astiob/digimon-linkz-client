using System;

namespace UniRx.Operators
{
	public abstract class OperatorObservableBase<T> : IObservable<T>, IOptimizedObservable<T>
	{
		private readonly bool isRequiredSubscribeOnCurrentThread;

		public OperatorObservableBase(bool isRequiredSubscribeOnCurrentThread)
		{
			this.isRequiredSubscribeOnCurrentThread = isRequiredSubscribeOnCurrentThread;
		}

		public bool IsRequiredSubscribeOnCurrentThread()
		{
			return this.isRequiredSubscribeOnCurrentThread;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
			if (this.isRequiredSubscribeOnCurrentThread && Scheduler.IsCurrentThreadSchedulerScheduleRequired)
			{
				Scheduler.CurrentThread.Schedule(delegate()
				{
					subscription.Disposable = this.SubscribeCore(observer, subscription);
				});
			}
			else
			{
				subscription.Disposable = this.SubscribeCore(observer, subscription);
			}
			return subscription;
		}

		protected abstract IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel);
	}
}
