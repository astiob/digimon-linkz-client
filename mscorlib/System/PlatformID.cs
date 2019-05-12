using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Identifies the operating system, or platform, supported by an assembly.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public enum PlatformID
	{
		/// <summary>The operating system is Win32s. Win32s is a layer that runs on 16-bit versions of Windows to provide access to 32-bit applications.</summary>
		Win32S,
		/// <summary>The operating system is Windows 95 or Windows 98.</summary>
		Win32Windows,
		/// <summary>The operating system is Windows NT or later.</summary>
		Win32NT,
		/// <summary>The operating system is Windows CE.</summary>
		WinCE,
		/// <summary>The operating system is Unix.</summary>
		Unix,
		/// <summary>The development platform is Xbox 360.</summary>
		Xbox,
		/// <summary>The operating system is Macintosh.</summary>
		MacOSX
	}
}
