using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>Provides an explicit layout that is visible from unmanaged code and that will have the same layout as the Win32 OVERLAPPED structure with additional reserved fields at the end.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public struct NativeOverlapped
	{
		/// <summary>Specifies the handle to an event set to the signaled state when the operation is complete. The calling process must set this member either to zero or to a valid event handle before calling any overlapped functions.</summary>
		/// <filterpriority>2</filterpriority>
		public IntPtr EventHandle;

		/// <summary>Specifies the length of the data transferred. Reserved for operating system use.</summary>
		/// <filterpriority>2</filterpriority>
		public IntPtr InternalHigh;

		/// <summary>Specifies a system-dependent status. Reserved for operating system use.</summary>
		/// <filterpriority>2</filterpriority>
		public IntPtr InternalLow;

		/// <summary>Specifies the high word of the byte offset at which to start the transfer.</summary>
		/// <filterpriority>2</filterpriority>
		public int OffsetHigh;

		/// <summary>Specifies a file position at which to start the transfer.</summary>
		/// <filterpriority>2</filterpriority>
		public int OffsetLow;

		internal int Handle1;

		internal int Handle2;
	}
}
