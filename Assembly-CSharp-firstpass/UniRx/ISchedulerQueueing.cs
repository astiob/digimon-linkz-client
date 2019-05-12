using System;

namespace UniRx
{
	public interface ISchedulerQueueing
	{
		void ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action);
	}
}
