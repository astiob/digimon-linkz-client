using System;
using System.Collections;

public class NormalTask : TaskBase
{
	private Func<IEnumerator> task;

	public NormalTask()
	{
	}

	public NormalTask(IEnumerator task)
	{
		this.task = (() => task);
	}

	public NormalTask(Func<IEnumerator> task)
	{
		this.task = task;
	}

	public override IEnumerator Execution()
	{
		if (this.task != null)
		{
			return this.task();
		}
		return null;
	}
}
