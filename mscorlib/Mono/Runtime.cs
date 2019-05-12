using System;
using System.Runtime.CompilerServices;

namespace Mono
{
	internal class Runtime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void mono_runtime_install_handlers();

		internal static void InstallSignalHandlers()
		{
			Runtime.mono_runtime_install_handlers();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDisplayName();
	}
}
