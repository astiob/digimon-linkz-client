using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>A general-purpose DownloadHandler implementation which stores received data in a native byte buffer.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerBuffer : DownloadHandler
	{
	}
}
