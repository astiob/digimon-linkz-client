using System;
using System.Collections.Generic;

public static class BattleInputUtility
{
	private const string errorMessage = "イベントが追加できませんでした. BattleStateManager.inputがインスタンスされていません.";

	private static BattleInputFunctionBase input
	{
		get
		{
			return BattleStateManager.current.input;
		}
	}

	public static void AddEvent(List<EventDelegate> eventDelegate, Action callback)
	{
		if (BattleInputUtility.input == null)
		{
			Debug.LogWarning("イベントが追加できませんでした. BattleStateManager.inputがインスタンスされていません.");
		}
		EventDelegate ev = new EventDelegate(BattleInputUtility.input, callback.Method.Name);
		EventDelegate.Add(eventDelegate, ev);
	}

	public static void AddEvent(List<EventDelegate> eventDelegate, Action<bool> callback, bool value)
	{
		if (BattleInputUtility.input == null)
		{
			Debug.LogWarning("イベントが追加できませんでした. BattleStateManager.inputがインスタンスされていません.");
		}
		EventDelegate eventDelegate2 = new EventDelegate(BattleInputUtility.input, callback.Method.Name);
		eventDelegate2.parameters[0] = new EventDelegate.Parameter(value);
		EventDelegate.Add(eventDelegate, eventDelegate2);
	}

	public static void AddEvent(List<EventDelegate> eventDelegate, Action<int> callback, int value)
	{
		if (BattleInputUtility.input == null)
		{
			Debug.LogWarning("イベントが追加できませんでした. BattleStateManager.inputがインスタンスされていません.");
		}
		EventDelegate eventDelegate2 = new EventDelegate(BattleInputUtility.input, callback.Method.Name);
		eventDelegate2.parameters[0] = new EventDelegate.Parameter(value);
		EventDelegate.Add(eventDelegate, eventDelegate2);
	}
}
