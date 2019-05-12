using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.CrashReportHandler
{
	[NativeHeader("Modules/CrashReporting/CrashReportHandler.h")]
	public class CrashReportHandler
	{
		private CrashReportHandler()
		{
		}

		public static extern bool enableCaptureExceptions { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
