using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniRx
{
	public abstract class LazyTask
	{
		protected readonly BooleanDisposable cancellation = new BooleanDisposable();

		public LazyTask.TaskStatus Status { get; protected set; }

		public abstract Coroutine Start();

		public void Cancel()
		{
			if (this.Status == LazyTask.TaskStatus.WaitingToRun || this.Status == LazyTask.TaskStatus.Running)
			{
				this.Status = LazyTask.TaskStatus.Canceled;
				this.cancellation.Dispose();
			}
		}

		public static LazyTask<T> FromResult<T>(T value)
		{
			return LazyTask<T>.FromResult(value);
		}

		public static Coroutine WhenAll(params LazyTask[] tasks)
		{
			return LazyTask.WhenAll(tasks.AsEnumerable<LazyTask>());
		}

		public static Coroutine WhenAll(IEnumerable<LazyTask> tasks)
		{
			Coroutine[] coroutines = tasks.Select((LazyTask x) => x.Start()).ToArray<Coroutine>();
			return MainThreadDispatcher.StartCoroutine(LazyTask.WhenAllCore(coroutines));
		}

		private static IEnumerator WhenAllCore(Coroutine[] coroutines)
		{
			foreach (Coroutine item in coroutines)
			{
				yield return item;
			}
			yield break;
		}

		public enum TaskStatus
		{
			WaitingToRun,
			Running,
			Completed,
			Canceled,
			Faulted
		}
	}
}
