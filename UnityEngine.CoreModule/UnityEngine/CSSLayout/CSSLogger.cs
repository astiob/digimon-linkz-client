using System;
using System.Runtime.InteropServices;

namespace UnityEngine.CSSLayout
{
	internal static class CSSLogger
	{
		public static CSSLogger.Func Logger = null;

		public static void Initialize()
		{
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void Func(CSSLogLevel level, string message);
	}
}
