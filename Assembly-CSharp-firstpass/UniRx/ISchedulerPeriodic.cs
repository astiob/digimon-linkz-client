using System;

namespace UniRx
{
	public interface ISchedulerPeriodic
	{
		IDisposable SchedulePeriodic(TimeSpan period, Action action);
	}
}
