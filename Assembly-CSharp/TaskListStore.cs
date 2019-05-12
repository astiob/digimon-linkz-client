using System;
using System.Collections.Generic;

public sealed class TaskListStore : ClassSingleton<TaskListStore>
{
	private List<List<TaskBase>> taskListStore = new List<List<TaskBase>>();

	public List<TaskBase> GetNewTaskList()
	{
		List<TaskBase> list = new List<TaskBase>();
		this.taskListStore.Add(list);
		return list;
	}

	public void RemoveTask(List<TaskBase> removeTaskList)
	{
		this.taskListStore.Remove(removeTaskList);
	}
}
