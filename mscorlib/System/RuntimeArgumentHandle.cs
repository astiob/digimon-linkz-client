using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>References a variable-length argument list.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public struct RuntimeArgumentHandle
	{
		internal IntPtr args;
	}
}
