using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Unity.Tasks.Internal
{
	internal static class InternalExtensions
	{
		internal static Task<TResult> OnSuccess<TIn, TResult>(this Task<TIn> task, Func<Task<TIn>, TResult> continuation)
		{
			return task.OnSuccess((Task t) => continuation((Task<TIn>)t));
		}

		internal static Task OnSuccess<TIn>(this Task<TIn> task, Action<Task<TIn>> continuation)
		{
			return task.OnSuccess(delegate(Task<TIn> t)
			{
				continuation(t);
				return null;
			});
		}

		internal static Task<TResult> OnSuccess<TResult>(this Task task, Func<Task, TResult> continuation)
		{
			return task.ContinueWith<Task<TResult>>(delegate(Task t)
			{
				if (t.IsFaulted)
				{
					AggregateException ex = t.Exception.Flatten();
					if (ex.InnerExceptions.Count == 1)
					{
						ExceptionDispatchInfo.Capture(ex.InnerExceptions[0]).Throw();
					}
					else
					{
						ExceptionDispatchInfo.Capture(ex).Throw();
					}
					return Task.FromResult<TResult>(default(TResult));
				}
				if (t.IsCanceled)
				{
					TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
					taskCompletionSource.SetCanceled();
					return taskCompletionSource.Task;
				}
				return Task.FromResult<TResult>(continuation(t));
			}).Unwrap<TResult>();
		}

		internal static Task OnSuccess(this Task task, Action<Task> continuation)
		{
			return task.OnSuccess(delegate(Task t)
			{
				continuation(t);
				return null;
			});
		}

		internal static Task WhileAsync(Func<Task<bool>> predicate, Func<Task> body)
		{
			Func<Task> iterate = null;
			iterate = (() => predicate().OnSuccess(delegate(Task<bool> t)
			{
				if (!t.Result)
				{
					return Task.FromResult<int>(0);
				}
				return body().OnSuccess((Task _) => iterate()).Unwrap();
			}).Unwrap());
			return iterate();
		}
	}
}
