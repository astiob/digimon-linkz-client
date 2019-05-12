using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerScript.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class DownloadHandlerScript : DownloadHandler
	{
		public DownloadHandlerScript()
		{
			this.InternalCreateScript();
		}

		public DownloadHandlerScript(byte[] preallocatedBuffer)
		{
			if (preallocatedBuffer == null || preallocatedBuffer.Length < 1)
			{
				throw new ArgumentException("Cannot create a preallocated-buffer DownloadHandlerScript backed by a null or zero-length array");
			}
			this.InternalCreateScript();
			this.SetPreallocatedBuffer(preallocatedBuffer);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerScript obj);

		private void InternalCreateScript()
		{
			this.m_Ptr = DownloadHandlerScript.Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPreallocatedBuffer(byte[] buffer);
	}
}
