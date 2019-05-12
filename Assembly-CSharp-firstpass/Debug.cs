using System;
using UnityEngine;

public static class Debug
{
	public static bool isDebugBuild { get; set; }

	public static void Log(object message)
	{
	}

	public static void Log(object message, UnityEngine.Object context)
	{
	}

	public static void LogFormat(string format, params object[] args)
	{
	}

	public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
	{
	}

	public static void LogWarning(object message)
	{
	}

	public static void LogWarning(object message, UnityEngine.Object context)
	{
	}

	public static void LogWarningFormat(string format, params object[] args)
	{
	}

	public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
	{
	}

	public static void LogError(object message)
	{
	}

	public static void LogError(object message, UnityEngine.Object context)
	{
	}

	public static void LogErrorFormat(string format, params object[] args)
	{
	}

	public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
	{
	}

	public static void LogException(Exception exception)
	{
	}

	public static void LogException(Exception exception, UnityEngine.Object context)
	{
	}

	public static void DrawLine(Vector3 start, Vector3 end)
	{
	}

	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0f, bool depthTest = true)
	{
	}

	public static void Assert(bool condition, string message)
	{
	}
}
