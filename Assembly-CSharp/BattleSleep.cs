using System;
using UnityEngine;

public class BattleSleep : BattleFunctionBase
{
	public void SetSleepOff(bool onSleepOff)
	{
		Screen.sleepTimeout = ((!onSleepOff) ? -2 : -1);
	}
}
