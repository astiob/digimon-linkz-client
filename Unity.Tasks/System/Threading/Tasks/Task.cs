using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Tasks.Internal;

namespace System.Threading.Tasks
{
	public abstract class Task
	{
		private static readonly ThreadLocal<int> executionDepth = new ThreadLocal<int>(() => 0);

		private static readonly Action<Action> immediateExecutor = delegate(Action a)
		{
			bool flag = AppDomain.CurrentDomain.FriendlyName.Equals("IL2CPP Root Domain");
			int num = 10;
			if (flag)
			{
				num = 200;
			}
			Task.executionDepth.Value++;
			try
			{
				if (Task.executionDepth.Value <= num)
				{
					a();
				}
				else
				{
					Task.Factory.Scheduler.Post(a);
				}
			}
			finally
			{
				Task.executionDepth.Value--;
			}
		};

		internal readonly object mutex = new object();

		internal IList<Action<Task>> continuations = new List<Action<Task>>();

		internal AggregateException exception;

		internal bool isCanceled;

		internal bool isCompleted;

		internal Task()
		{
		}

		public static TaskFactory Factory
		{
			get
			{
				return new TaskFactory();
			}
		}

		public AggregateException Exception
		{
			get
			{
				object obj = this.mutex;
				AggregateException result;
				lock (obj)
				{
					result = this.exception;
				}
				return result;
			}
		}

		public bool IsCanceled
		{
			get
			{
				object obj = this.mutex;
				bool result;
				lock (obj)
				{
					result = this.isCanceled;
				}
				return result;
			}
		}

		public bool IsCompleted
		{
			get
			{
				object obj = this.mutex;
				bool result;
				lock (obj)
				{
					result = this.isCompleted;
				}
				return result;
			}
		}

		public bool IsFaulted
		{
			get
			{
				return this.Exception != null;
			}
		}

		public void Wait()
		{
			object obj = this.mutex;
			lock (obj)
			{
				if (!this.IsCompleted)
				{
					Monitor.Wait(this.mutex);
				}
				if (this.IsFaulted)
				{
					throw this.Exception;
				}
			}
		}

		public Task<T> ContinueWith<T>(Func<Task, T> continuation)
		{
			return this.ContinueWith<T>(continuation, CancellationToken.None);
		}

		public Task<T> ContinueWith<T>(Func<Task, T> continuation, CancellationToken cancellationToken)
		{
			bool flag = false;
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			CancellationTokenRegistration cancellation = cancellationToken.Register(delegate
			{
				tcs.TrySetCanceled();
			});
			Action<Task> action = delegate(Task t)
			{
				Task.immediateExecutor(delegate
				{
					try
					{
						tcs.TrySetResult(continuation(t));
						cancellation.Dispose();
					}
					catch (Exception ex)
					{
						tcs.TrySetException(ex);
						cancellation.Dispose();
					}
				});
			};
			object obj = this.mutex;
			lock (obj)
			{
				flag = this.IsCompleted;
				if (!flag)
				{
					this.continuations.Add(action);
				}
			}
			if (flag)
			{
				action(this);
			}
			return tcs.Task;
		}

		public Task ContinueWith(Action<Task> continuation)
		{
			return this.ContinueWith(continuation, CancellationToken.None);
		}

		public Task ContinueWith(Action<Task> continuation, CancellationToken cancellationToken)
		{
			return this.ContinueWith<int>(delegate(Task t)
			{
				continuation(t);
				return 0;
			}, cancellationToken);
		}

		public static Task WhenAll(params Task[] tasks)
		{
			return Task.WhenAll(tasks);
		}

		public static Task WhenAll(IEnumerable<Task> tasks)
		{
			Task[] taskArr = tasks.ToArray<Task>();
			if (taskArr.Length == 0)
			{
				return Task.FromResult<int>(0);
			}
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
			Task.Factory.ContinueWhenAll(taskArr, delegate(Task[] _)
			{
				AggregateException[] array = taskArr.Where((Task t) => t.IsFaulted).Select((Task t) => t.Exception).ToArray<AggregateException>();
				if (array.Length > 0)
				{
					tcs.SetException(new AggregateException(array));
				}
				else if (taskArr.Any((Task t) => t.IsCanceled))
				{
					tcs.SetCanceled();
				}
				else
				{
					tcs.SetResult(0);
				}
			});
			return tcs.Task;
		}

		internal static Task<Task> WhenAny(params Task[] tasks)
		{
			return Task.WhenAny(tasks);
		}

		internal static Task<Task> WhenAny(IEnumerable<Task> tasks)
		{
			TaskCompletionSource<Task> tcs = new TaskCompletionSource<Task>();
			foreach (Task task in tasks)
			{
				task.ContinueWith<bool>((Task t) => tcs.TrySetResult(t));
			}
			return tcs.Task;
		}

		public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks)
		{
			return Task.WhenAll(tasks.Cast<Task>()).OnSuccess((Task _) => tasks.Select((Task<T> t) => t.Result).ToArray<T>());
		}

		public static Task<T> FromResult<T>(T result)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(result);
			return taskCompletionSource.Task;
		}

		public static Task<T> Run<T>(Func<T> toRun)
		{
			return Task.Factory.StartNew<T>(toRun);
		}

		public static Task Run(Action toRun)
		{
			return Task.Factory.StartNew<int>(delegate
			{
				toRun();
				return 0;
			});
		}

		public static Task Delay(TimeSpan timespan)
		{
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
			Timer timer = new Timer(delegate(object _)
			{
				tcs.TrySetResult(0);
			});
			timer.Change(timespan, TimeSpan.FromMilliseconds(-1.0));
			return tcs.Task;
		}
	}
}
