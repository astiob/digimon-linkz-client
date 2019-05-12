using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>A general-purpose UploadHandler subclass, using a native-code memory buffer.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UploadHandlerRaw : UploadHandler
	{
	}
}
