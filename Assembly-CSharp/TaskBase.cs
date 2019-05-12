using System;
using System.Collections;
using System.Collections.Generic;

public abstract class TaskBase
{
	protected TaskBase.AfterAlertClosed afterAlertClosedBehavior;

	private List<TaskBase> taskList;

	public TaskBase Add(TaskBase task)
	{
		if (this.taskList == null)
		{
			this.taskList = ClassSingleton<TaskListStore>.Instance.GetNewTaskList();
			this.taskList.Add(this);
		}
		if (task.taskList == null)
		{
			this.taskList.Add(task);
		}
		else
		{
			for (int i = 0; i < task.taskList.Count; i++)
			{
				this.taskList.Add(task.taskList[i]);
			}
		}
		task.taskList = this.taskList;
		return this.taskList[0];
	}

	public TaskBase Delegate(TaskBase task)
	{
		if (task.taskList == null)
		{
			this.Add(task);
		}
		else
		{
			if (this.taskList == null)
			{
				this.taskList = ClassSingleton<TaskListStore>.Instance.GetNewTaskList();
				this.taskList.Add(this);
			}
			for (int i = 0; i < task.taskList.Count; i++)
			{
				this.taskList.Add(task.taskList[i]);
			}
			task.DestroyTask();
			task.taskList = this.taskList;
		}
		return this.taskList[0];
	}

	public abstract IEnumerator Execution();

	public List<TaskBase> GetTask()
	{
		if (this.taskList == null)
		{
			this.taskList = ClassSingleton<TaskListStore>.Instance.GetNewTaskList();
			this.taskList.Add(this);
		}
		return this.taskList;
	}

	public virtual bool IsBackTopScreen(Exception exception)
	{
		return false;
	}

	public virtual IEnumerator OnAlert(Exception exception)
	{
		this.afterAlertClosedBehavior = TaskBase.AfterAlertClosed.RETURN;
		yield break;
	}

	public void SetAfterBehavior(TaskBase.AfterAlertClosed behavior)
	{
		this.afterAlertClosedBehavior = behavior;
	}

	public TaskBase.AfterAlertClosed GetAfterBehavior()
	{
		return this.afterAlertClosedBehavior;
	}

	protected void ClearTask()
	{
		if (this.taskList != null)
		{
			this.taskList.Clear();
		}
	}

	public void DestroyTask()
	{
		if (this.taskList != null)
		{
			ClassSingleton<TaskListStore>.Instance.RemoveTask(this.taskList);
			this.taskList.Clear();
			this.taskList = null;
		}
	}

	public enum AfterAlertClosed
	{
		RETURN,
		RETRY
	}
}
