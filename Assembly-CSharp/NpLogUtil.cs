using System;

public class NpLogUtil
{
	public static void DebugLog(bool isdebug, string msg)
	{
		if (isdebug)
		{
			Debug.Log("#=#=# " + msg);
		}
	}
}
