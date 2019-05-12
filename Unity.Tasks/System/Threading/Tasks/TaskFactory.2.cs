using System;

namespace System.Threading.Tasks
{
	internal class TaskFactory<T>
	{
		private readonly TaskFactory factory;

		internal TaskFactory(TaskScheduler scheduler, CancellationToken cancellationToken)
		{
			this.factory = new TaskFactory(scheduler, cancellationToken);
		}

		public TaskFactory(TaskScheduler scheduler) : this(scheduler, CancellationToken.None)
		{
		}

		public TaskFactory(CancellationToken cancellationToken) : this(new TaskScheduler(null), cancellationToken)
		{
		}

		public TaskFactory() : this(new TaskScheduler(null), CancellationToken.None)
		{
		}

		public TaskScheduler Scheduler
		{
			get
			{
				return this.factory.Scheduler;
			}
		}

		public Task<T> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return this.factory.FromAsync<TArg1, TArg2, TArg3, T>(beginMethod, endMethod, arg1, arg2, arg3, state);
		}

		public Task<T> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return this.factory.FromAsync<TArg1, TArg2, T>(beginMethod, endMethod, arg1, arg2, state);
		}

		public Task<T> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, TArg1 arg1, object state)
		{
			return this.factory.FromAsync<TArg1, T>(beginMethod, endMethod, arg1, state);
		}

		public Task<T> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, object state)
		{
			return this.factory.FromAsync<T>(beginMethod, endMethod, state);
		}
	}
}
