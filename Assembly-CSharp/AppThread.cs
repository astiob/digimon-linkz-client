using System;
using System.Threading;

public class AppThread
{
	private static Thread mainThread = Thread.CurrentThread;

	public static bool isMainThread
	{
		get
		{
			return AppThread.mainThread == null || Thread.CurrentThread.Equals(AppThread.mainThread);
		}
	}
}
