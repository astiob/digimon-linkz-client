using System;
using System.Collections;

public static class TaskExtensions
{
	public static IEnumerator Run(this TaskBase task, Action onSuccess = null, Action<Exception> onFailed = null, Func<Exception, IEnumerator> onAlert = null)
	{
		return ClassSingleton<TaskExecutor>.Instance.Execution(task, onSuccess, onFailed, onAlert);
	}
}
