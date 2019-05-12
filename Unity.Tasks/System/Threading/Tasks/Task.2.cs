using System;

namespace System.Threading.Tasks
{
	public sealed class Task<T> : Task
	{
		private T result;

		internal Task()
		{
		}

		public T Result
		{
			get
			{
				base.Wait();
				return this.result;
			}
		}

		public Task ContinueWith(Action<Task<T>> continuation)
		{
			return base.ContinueWith(delegate(Task t)
			{
				continuation((Task<T>)t);
			});
		}

		public Task<TResult> ContinueWith<TResult>(Func<Task<T>, TResult> continuation)
		{
			return base.ContinueWith<TResult>((Task t) => continuation((Task<T>)t));
		}

		private void RunContinuations()
		{
			object mutex = this.mutex;
			lock (mutex)
			{
				foreach (Action<Task> action in this.continuations)
				{
					action(this);
				}
				this.continuations = null;
			}
		}

		internal bool TrySetResult(T result)
		{
			object mutex = this.mutex;
			bool flag;
			lock (mutex)
			{
				if (this.isCompleted)
				{
					flag = false;
				}
				else
				{
					this.isCompleted = true;
					this.result = result;
					Monitor.PulseAll(this.mutex);
					this.RunContinuations();
					flag = true;
				}
			}
			return flag;
		}

		internal bool TrySetCanceled()
		{
			object mutex = this.mutex;
			bool flag;
			lock (mutex)
			{
				if (this.isCompleted)
				{
					flag = false;
				}
				else
				{
					this.isCompleted = true;
					this.isCanceled = true;
					Monitor.PulseAll(this.mutex);
					this.RunContinuations();
					flag = true;
				}
			}
			return flag;
		}

		internal bool TrySetException(AggregateException exception)
		{
			object mutex = this.mutex;
			bool flag;
			lock (mutex)
			{
				if (this.isCompleted)
				{
					flag = false;
				}
				else
				{
					this.isCompleted = true;
					this.exception = exception;
					Monitor.PulseAll(this.mutex);
					this.RunContinuations();
					flag = true;
				}
			}
			return flag;
		}
	}
}
