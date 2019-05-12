using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class DkLog
{
	private static int LOG_MAX = 2048;

	private static Color[] LOG_COLOR = new Color[]
	{
		Color.gray,
		Color.white,
		Color.cyan,
		Color.yellow,
		Color.red,
		Color.white
	};

	private static DkLog.Level _logLevel = DkLog.Level.Debug;

	private static readonly Queue<DkLog.LogData> LogQue = new Queue<DkLog.LogData>(DkLog.LOG_MAX);

	private static Vector2 _scrollPosition = Vector2.zero;

	private static bool _isNeedScrollReset;

	[Conditional("USE_DK_LOG")]
	public static void V(string message, bool isConsole = false)
	{
		DkLog._Push(DkLog.Level.Verbose, message, isConsole);
	}

	[Conditional("USE_DK_LOG")]
	public static void D(string message, bool isConsole = false)
	{
		DkLog._Push(DkLog.Level.Debug, message, isConsole);
	}

	[Conditional("USE_DK_LOG")]
	public static void I(string message, bool isConsole = false)
	{
		DkLog._Push(DkLog.Level.Information, message, isConsole);
	}

	[Conditional("USE_DK_LOG")]
	public static void W(string message, bool isConsole = false)
	{
		DkLog._Push(DkLog.Level.Warning, message, isConsole);
	}

	[Conditional("USE_DK_LOG")]
	public static void E(string message, bool isConsole = false)
	{
		DkLog._Push(DkLog.Level.Error, message, isConsole);
	}

	[Conditional("USE_DK_LOG")]
	public static void B(string message, bool isConsole = false)
	{
		DkLog._Push(DkLog.Level.BattleLog, message, isConsole);
	}

	public static void DrawLogWindow(Rect drawArea, bool isEditor = false)
	{
		if (Application.isEditor && !isEditor)
		{
			return;
		}
		GUI.Box(drawArea, string.Empty);
		GUILayout.BeginArea(drawArea);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button(DkLog._logLevel.ToString(), new GUILayoutOption[0]))
		{
			DkLog._logLevel = (DkLog._logLevel + 1) % DkLog.Level.Max;
			DkLog._isNeedScrollReset = true;
		}
		GUILayout.EndHorizontal();
		DkLog._scrollPosition = GUILayout.BeginScrollView(DkLog._scrollPosition, true, true, new GUILayoutOption[0]);
		int num = 0;
		GUIStyle guistyle = new GUIStyle();
		foreach (DkLog.LogData logData in DkLog.LogQue)
		{
			if (logData.Level >= DkLog._logLevel)
			{
				guistyle.normal.textColor = DkLog.LOG_COLOR[(int)logData.Level];
				if (!isEditor)
				{
					GUILayout.Label(logData.Message, guistyle, new GUILayoutOption[0]);
				}
				num++;
			}
		}
		if (DkLog._isNeedScrollReset)
		{
			DkLog._isNeedScrollReset = false;
			DkLog._scrollPosition.y = (float)((num <= 0) ? 0 : ((num - 1) * 20));
		}
		GUILayout.EndScrollView();
		if (GUILayout.Button("Clear", new GUILayoutOption[0]))
		{
			DkLog.LogQue.Clear();
		}
		GUILayout.EndArea();
	}

	private static void _Push(DkLog.Level level, string message, bool isConsole)
	{
		if (DkLog.LogQue.Count >= DkLog.LOG_MAX)
		{
			DkLog.LogQue.Dequeue();
		}
		DkLog.LogData item = new DkLog.LogData
		{
			Level = level,
			Message = message,
			TimeStamp = DateTime.Now
		};
		DkLog.LogQue.Enqueue(item);
		DkLog._isNeedScrollReset = true;
		if (isConsole)
		{
			if (level != DkLog.Level.Warning)
			{
				if (level != DkLog.Level.Error)
				{
					global::Debug.Log(message);
				}
				else
				{
					global::Debug.LogError(message);
				}
			}
			else
			{
				global::Debug.LogWarning(message);
			}
		}
	}

	private static string _GetLogString(DkLog.Level minLevel)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DkLog.LogData logData in DkLog.LogQue)
		{
			if (logData.Level >= minLevel)
			{
				stringBuilder.AppendLine(logData.TimeStamp.ToString("MM/dd HH:mm:ss \t ") + Enum.GetName(typeof(DkLog.Level), logData.Level).Substring(0, 1) + "\t" + logData.Message);
			}
		}
		return stringBuilder.ToString();
	}

	public static void Clear()
	{
		DkLog.LogQue.Clear();
	}

	public enum Level
	{
		Verbose,
		Debug,
		Information,
		Warning,
		Error,
		BattleLog,
		Max
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct LogData
	{
		public DkLog.Level Level { get; set; }

		public string Message { get; set; }

		public DateTime TimeStamp { get; set; }
	}
}
