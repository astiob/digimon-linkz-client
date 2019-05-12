using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class containing methods to ease debugging while developing a game.</para>
	/// </summary>
	public sealed class Debug
	{
		/// <summary>
		///   <para>Draws a line between specified start and end points.</para>
		/// </summary>
		/// <param name="start">Point in world space where the line should start.</param>
		/// <param name="end">Point in world space where the line should end.</param>
		/// <param name="color">Color of the line.</param>
		/// <param name="duration">How long the line should be visible for.</param>
		/// <param name="depthTest">Should the line be obscured by objects closer to the camera?</param>
		public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			Debug.INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		/// <summary>
		///   <para>Draws a line between specified start and end points.</para>
		/// </summary>
		/// <param name="start">Point in world space where the line should start.</param>
		/// <param name="end">Point in world space where the line should end.</param>
		/// <param name="color">Color of the line.</param>
		/// <param name="duration">How long the line should be visible for.</param>
		/// <param name="depthTest">Should the line be obscured by objects closer to the camera?</param>
		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
		{
			bool depthTest = true;
			Debug.INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		/// <summary>
		///   <para>Draws a line between specified start and end points.</para>
		/// </summary>
		/// <param name="start">Point in world space where the line should start.</param>
		/// <param name="end">Point in world space where the line should end.</param>
		/// <param name="color">Color of the line.</param>
		/// <param name="duration">How long the line should be visible for.</param>
		/// <param name="depthTest">Should the line be obscured by objects closer to the camera?</param>
		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end, Color color)
		{
			bool depthTest = true;
			float duration = 0f;
			Debug.INTERNAL_CALL_DrawLine(ref start, ref end, ref color, duration, depthTest);
		}

		/// <summary>
		///   <para>Draws a line between specified start and end points.</para>
		/// </summary>
		/// <param name="start">Point in world space where the line should start.</param>
		/// <param name="end">Point in world space where the line should end.</param>
		/// <param name="color">Color of the line.</param>
		/// <param name="duration">How long the line should be visible for.</param>
		/// <param name="depthTest">Should the line be obscured by objects closer to the camera?</param>
		[ExcludeFromDocs]
		public static void DrawLine(Vector3 start, Vector3 end)
		{
			bool depthTest = true;
			float duration = 0f;
			Color white = Color.white;
			Debug.INTERNAL_CALL_DrawLine(ref start, ref end, ref white, duration, depthTest);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawLine(ref Vector3 start, ref Vector3 end, ref Color color, float duration, bool depthTest);

		/// <summary>
		///   <para>Draws a line from start to start + dir in world coordinates.</para>
		/// </summary>
		/// <param name="start">Point in world space where the ray should start.</param>
		/// <param name="dir">Direction and length of the ray.</param>
		/// <param name="color">Color of the drawn line.</param>
		/// <param name="duration">How long the line will be visible for (in seconds).</param>
		/// <param name="depthTest">Should the line be obscured by other objects closer to the camera?</param>
		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
		{
			bool depthTest = true;
			Debug.DrawRay(start, dir, color, duration, depthTest);
		}

		/// <summary>
		///   <para>Draws a line from start to start + dir in world coordinates.</para>
		/// </summary>
		/// <param name="start">Point in world space where the ray should start.</param>
		/// <param name="dir">Direction and length of the ray.</param>
		/// <param name="color">Color of the drawn line.</param>
		/// <param name="duration">How long the line will be visible for (in seconds).</param>
		/// <param name="depthTest">Should the line be obscured by other objects closer to the camera?</param>
		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir, Color color)
		{
			bool depthTest = true;
			float duration = 0f;
			Debug.DrawRay(start, dir, color, duration, depthTest);
		}

		/// <summary>
		///   <para>Draws a line from start to start + dir in world coordinates.</para>
		/// </summary>
		/// <param name="start">Point in world space where the ray should start.</param>
		/// <param name="dir">Direction and length of the ray.</param>
		/// <param name="color">Color of the drawn line.</param>
		/// <param name="duration">How long the line will be visible for (in seconds).</param>
		/// <param name="depthTest">Should the line be obscured by other objects closer to the camera?</param>
		[ExcludeFromDocs]
		public static void DrawRay(Vector3 start, Vector3 dir)
		{
			bool depthTest = true;
			float duration = 0f;
			Color white = Color.white;
			Debug.DrawRay(start, dir, white, duration, depthTest);
		}

		/// <summary>
		///   <para>Draws a line from start to start + dir in world coordinates.</para>
		/// </summary>
		/// <param name="start">Point in world space where the ray should start.</param>
		/// <param name="dir">Direction and length of the ray.</param>
		/// <param name="color">Color of the drawn line.</param>
		/// <param name="duration">How long the line will be visible for (in seconds).</param>
		/// <param name="depthTest">Should the line be obscured by other objects closer to the camera?</param>
		public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
		{
			Debug.DrawLine(start, start + dir, color, duration, depthTest);
		}

		/// <summary>
		///   <para>Pauses the editor.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Break();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DebugBreak();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Log(int level, string msg, [Writable] Object obj);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_LogException(Exception exception, [Writable] Object obj);

		/// <summary>
		///   <para>Logs message to the Unity Console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void Log(object message)
		{
			Debug.Internal_Log(0, (message == null) ? "Null" : message.ToString(), null);
		}

		/// <summary>
		///   <para>Logs message to the Unity Console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void Log(object message, Object context)
		{
			Debug.Internal_Log(0, (message == null) ? "Null" : message.ToString(), context);
		}

		/// <summary>
		///   <para>Logs a formatted message to the Unity Console.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogFormat(string format, params object[] args)
		{
			Debug.Log(string.Format(format, args));
		}

		/// <summary>
		///   <para>Logs a formatted message to the Unity Console.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogFormat(Object context, string format, params object[] args)
		{
			Debug.Log(string.Format(format, args), context);
		}

		/// <summary>
		///   <para>A variant of Debug.Log that logs an error message to the console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogError(object message)
		{
			Debug.Internal_Log(2, (message == null) ? "Null" : message.ToString(), null);
		}

		/// <summary>
		///   <para>A variant of Debug.Log that logs an error message to the console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogError(object message, Object context)
		{
			Debug.Internal_Log(2, message.ToString(), context);
		}

		/// <summary>
		///   <para>Logs a formatted error message to the Unity console.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogErrorFormat(string format, params object[] args)
		{
			Debug.LogError(string.Format(format, args));
		}

		/// <summary>
		///   <para>Logs a formatted error message to the Unity console.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogErrorFormat(Object context, string format, params object[] args)
		{
			Debug.LogError(string.Format(format, args), context);
		}

		/// <summary>
		///   <para>Clears errors from the developer console.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearDeveloperConsole();

		/// <summary>
		///   <para>Opens or closes developer console.</para>
		/// </summary>
		public static extern bool developerConsoleVisible { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void WriteLineToLogFile(string message);

		/// <summary>
		///   <para>A variant of Debug.Log that logs an error message to the console.</para>
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="context"></param>
		public static void LogException(Exception exception)
		{
			Debug.Internal_LogException(exception, null);
		}

		/// <summary>
		///   <para>A variant of Debug.Log that logs an error message to the console.</para>
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="context"></param>
		public static void LogException(Exception exception, Object context)
		{
			Debug.Internal_LogException(exception, context);
		}

		/// <summary>
		///   <para>A variant of Debug.Log that logs a warning message to the console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogWarning(object message)
		{
			Debug.Internal_Log(1, message.ToString(), null);
		}

		/// <summary>
		///   <para>A variant of Debug.Log that logs a warning message to the console.</para>
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogWarning(object message, Object context)
		{
			Debug.Internal_Log(1, message.ToString(), context);
		}

		/// <summary>
		///   <para>Logs a formatted warning message to the Unity Console.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogWarningFormat(string format, params object[] args)
		{
			Debug.LogWarning(string.Format(format, args));
		}

		/// <summary>
		///   <para>Logs a formatted warning message to the Unity Console.</para>
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		/// <param name="context">Object to which the message applies.</param>
		public static void LogWarningFormat(Object context, string format, params object[] args)
		{
			Debug.LogWarning(string.Format(format, args), context);
		}

		/// <summary>
		///   <para>Assert the condition.</para>
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="format">Formatted string for display.</param>
		/// <param name="args">Arguments for the formatted string.</param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				Debug.LogAssertion(null);
			}
		}

		/// <summary>
		///   <para>Assert the condition.</para>
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="format">Formatted string for display.</param>
		/// <param name="args">Arguments for the formatted string.</param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				Debug.LogAssertion(message);
			}
		}

		/// <summary>
		///   <para>Assert the condition.</para>
		/// </summary>
		/// <param name="condition">Condition you expect to be true.</param>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="format">Formatted string for display.</param>
		/// <param name="args">Arguments for the formatted string.</param>
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string format, params object[] args)
		{
			if (!condition)
			{
				Debug.LogAssertion(string.Format(format, args));
			}
		}

		internal static void LogAssertion(string message)
		{
			Debug.Internal_Log(3, message, null);
		}

		/// <summary>
		///   <para>In the Build Settings dialog there is a check box called "Development Build".</para>
		/// </summary>
		public static extern bool isDebugBuild { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OpenConsoleFile();
	}
}
