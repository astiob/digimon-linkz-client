using System;

namespace UniRx
{
	public interface ISchedulerLongRunning
	{
		IDisposable ScheduleLongRunning(Action<ICancelable> action);
	}
}
