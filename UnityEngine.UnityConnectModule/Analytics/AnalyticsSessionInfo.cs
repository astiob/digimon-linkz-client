using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
	[RequiredByNativeCode]
	[NativeHeader("Runtime/UnityConnect/UnityConnectClient.h")]
	[NativeHeader("UnityConnectScriptingClasses.h")]
	public static class AnalyticsSessionInfo
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event AnalyticsSessionInfo.SessionStateChanged sessionStateChanged;

		[RequiredByNativeCode]
		internal static void CallSessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged)
		{
			AnalyticsSessionInfo.SessionStateChanged sessionStateChanged = AnalyticsSessionInfo.sessionStateChanged;
			if (sessionStateChanged != null)
			{
				sessionStateChanged(sessionState, sessionId, sessionElapsedTime, sessionChanged);
			}
		}

		public static extern AnalyticsSessionState sessionState { [NativeMethod("GetPlayerSessionState")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern long sessionId { [NativeMethod("GetPlayerSessionId")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern long sessionElapsedTime { [NativeMethod("GetPlayerSessionElapsedTime")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern string userId { [NativeMethod("GetUserId")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public delegate void SessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged);
	}
}
