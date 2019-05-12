using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	internal struct kevent : IDisposable
	{
		public int ident;

		public short filter;

		public ushort flags;

		public uint fflags;

		public int data;

		public IntPtr udata;

		public void Dispose()
		{
			if (this.udata != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.udata);
			}
		}
	}
}
