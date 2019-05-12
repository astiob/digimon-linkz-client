using System;

namespace System.Threading.Tasks
{
	public class TaskScheduler
	{
		private static SynchronizationContext defaultContext = new SynchronizationContext();

		private SynchronizationContext context;

		public TaskScheduler(SynchronizationContext context)
		{
			this.context = (context ?? TaskScheduler.defaultContext);
		}

		public void Post(Action action)
		{
			this.context.Post(delegate(object o)
			{
				action();
			}, null);
		}

		public static TaskScheduler FromCurrentSynchronizationContext()
		{
			return new TaskScheduler(SynchronizationContext.Current);
		}
	}
}
