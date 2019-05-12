using System;
using System.Collections;
using System.Collections.Generic;

public sealed class TaskExecutor : ClassSingleton<TaskExecutor>
{
	private Exception exception;

	private bool IsSuccess
	{
		get
		{
			return null == this.exception;
		}
	}

	public bool IsDispose { get; set; }

	public IEnumerator Execution(TaskBase taskHeader, Action onSuccess = null, Action<Exception> onFailed = null, Func<Exception, IEnumerator> onAlert = null)
	{
		List<TaskBase> taskList = taskHeader.GetTask();
		while (taskList != null && 0 < taskList.Count)
		{
			this.exception = null;
			IEnumerator ie = this.StartTask(taskList, onAlert);
			while (this.MoveNext(ie))
			{
				yield return ie.Current;
			}
			if (this.IsSuccess)
			{
				taskList.RemoveAt(0);
			}
			else if (taskList[0].GetAfterBehavior() == TaskBase.AfterAlertClosed.RETURN)
			{
				break;
			}
		}
		taskHeader.DestroyTask();
		if (!this.IsDispose)
		{
			if (this.IsSuccess)
			{
				if (onSuccess != null)
				{
					onSuccess();
				}
			}
			else if (onFailed != null)
			{
				onFailed(this.exception);
			}
		}
		else
		{
			this.IsDispose = false;
		}
		yield break;
	}

	private IEnumerator StartTask(List<TaskBase> taskList, Func<Exception, IEnumerator> onAlert)
	{
		IEnumerator ie = taskList[0].Execution();
		while (this.MoveNext(ie))
		{
			yield return ie.Current;
		}
		if (!this.IsSuccess)
		{
			IEnumerator errorIE = this.OnError(taskList[0], onAlert);
			while (this.MoveNext(errorIE))
			{
				yield return errorIE.Current;
			}
		}
		yield break;
	}

	private IEnumerator OnError(TaskBase task, Func<Exception, IEnumerator> onAlert)
	{
		IEnumerator result;
		if (task.IsBackTopScreen(this.exception))
		{
			result = task.OnAlert(this.exception);
		}
		else if (onAlert != null)
		{
			result = onAlert(this.exception);
		}
		else
		{
			result = task.OnAlert(this.exception);
		}
		return result;
	}

	private bool MoveNext(IEnumerator ie)
	{
		bool result = false;
		if (ie != null)
		{
			try
			{
				result = ie.MoveNext();
			}
			catch (Exception ex)
			{
				this.exception = ex;
				Debug.Log(this.exception);
			}
		}
		return result;
	}
}
