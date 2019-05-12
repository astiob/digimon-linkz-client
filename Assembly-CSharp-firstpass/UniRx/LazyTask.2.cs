using System;
using UnityEngine;

namespace UniRx
{
	public class LazyTask<T> : LazyTask
	{
		private readonly IObservable<T> source;

		private T result;

		public LazyTask(IObservable<T> source)
		{
			this.source = source;
			base.Status = LazyTask.TaskStatus.WaitingToRun;
		}

		public T Result
		{
			get
			{
				if (base.Status != LazyTask.TaskStatus.Completed)
				{
					throw new InvalidOperationException("Task is not completed");
				}
				return this.result;
			}
		}

		public Exception Exception { get; private set; }

		public override Coroutine Start()
		{
			if (base.Status != LazyTask.TaskStatus.WaitingToRun)
			{
				throw new InvalidOperationException("Task already started");
			}
			base.Status = LazyTask.TaskStatus.Running;
			return this.source.StartAsCoroutine(delegate(T x)
			{
				this.result = x;
				base.Status = LazyTask.TaskStatus.Completed;
			}, delegate(Exception ex)
			{
				this.Exception = ex;
				base.Status = LazyTask.TaskStatus.Faulted;
			}, new CancellationToken(this.cancellation));
		}

		public override string ToString()
		{
			switch (base.Status)
			{
			case LazyTask.TaskStatus.WaitingToRun:
				return "Status:WaitingToRun";
			case LazyTask.TaskStatus.Running:
				return "Status:Running";
			case LazyTask.TaskStatus.Completed:
			{
				string str = "Status:Completed, Result:";
				T t = this.Result;
				return str + t.ToString();
			}
			case LazyTask.TaskStatus.Canceled:
				return "Status:Canceled";
			case LazyTask.TaskStatus.Faulted:
			{
				string str2 = "Status:Faulted, Result:";
				T t2 = this.Result;
				return str2 + t2.ToString();
			}
			default:
				return string.Empty;
			}
		}

		public static LazyTask<T> FromResult(T value)
		{
			return new LazyTask<T>(null)
			{
				result = value,
				Status = LazyTask.TaskStatus.Completed
			};
		}
	}
}
