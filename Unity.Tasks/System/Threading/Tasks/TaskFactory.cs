using System;

namespace System.Threading.Tasks
{
	public class TaskFactory
	{
		private readonly TaskScheduler scheduler;

		private readonly CancellationToken cancellationToken;

		internal TaskFactory(TaskScheduler scheduler, CancellationToken cancellationToken)
		{
			this.scheduler = scheduler;
			this.cancellationToken = cancellationToken;
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

		public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) : this(scheduler, cancellationToken)
		{
		}

		public TaskScheduler Scheduler
		{
			get
			{
				return this.scheduler;
			}
		}

		public Task<T> StartNew<T>(Func<T> func)
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			this.scheduler.Post(delegate
			{
				try
				{
					tcs.SetResult(func());
				}
				catch (Exception exception)
				{
					tcs.SetException(exception);
				}
			});
			return tcs.Task;
		}

		public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return this.FromAsync((AsyncCallback callback, object _) => beginMethod(arg1, arg2, arg3, callback, state), endMethod, state);
		}

		public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
		{
			return this.FromAsync<TResult>((AsyncCallback callback, object _) => beginMethod(arg1, arg2, arg3, callback, state), endMethod, state);
		}

		public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return this.FromAsync((AsyncCallback callback, object _) => beginMethod(arg1, arg2, callback, state), endMethod, state);
		}

		public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
		{
			return this.FromAsync<TResult>((AsyncCallback callback, object _) => beginMethod(arg1, arg2, callback, state), endMethod, state);
		}

		public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
		{
			return this.FromAsync((AsyncCallback callback, object _) => beginMethod(arg1, callback, state), endMethod, state);
		}

		public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
		{
			return this.FromAsync<TResult>((AsyncCallback callback, object _) => beginMethod(arg1, callback, state), endMethod, state);
		}

		public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
		{
			return this.FromAsync<int>(beginMethod, delegate(IAsyncResult result)
			{
				endMethod(result);
				return 0;
			}, state);
		}

		public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
		{
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
			CancellationTokenRegistration cancellation = this.cancellationToken.Register(delegate
			{
				tcs.TrySetCanceled();
			});
			if (this.cancellationToken.IsCancellationRequested)
			{
				tcs.TrySetCanceled();
				cancellation.Dispose();
				return tcs.Task;
			}
			try
			{
				beginMethod(delegate(IAsyncResult result)
				{
					try
					{
						TResult result2 = endMethod(result);
						tcs.TrySetResult(result2);
						cancellation.Dispose();
					}
					catch (Exception exception2)
					{
						tcs.TrySetException(exception2);
						cancellation.Dispose();
					}
				}, state);
			}
			catch (Exception exception)
			{
				tcs.TrySetException(exception);
				cancellation.Dispose();
			}
			return tcs.Task;
		}

		public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
		{
			int remaining = tasks.Length;
			TaskCompletionSource<Task[]> tcs = new TaskCompletionSource<Task[]>();
			if (remaining == 0)
			{
				tcs.TrySetResult(tasks);
			}
			foreach (Task task in tasks)
			{
				task.ContinueWith(delegate(Task _)
				{
					if (Interlocked.Decrement(ref remaining) == 0)
					{
						tcs.TrySetResult(tasks);
					}
				});
			}
			return tcs.Task.ContinueWith(delegate(Task<Task[]> t)
			{
				continuationAction(t.Result);
			});
		}
	}
}
