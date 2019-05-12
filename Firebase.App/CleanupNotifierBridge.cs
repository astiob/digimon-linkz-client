using System;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class CleanupNotifierBridge : IDisposable
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal CleanupNotifierBridge(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		internal static HandleRef getCPtr(CleanupNotifierBridge obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						throw new MethodAccessException("C++ destructor does not have public access");
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public static bool RegisterCleanupDelegate(IntPtr cleanupObject, IntPtr notifyObject, CleanupNotifierBridge.CleanupDelegate cleanupDelegate, IntPtr context)
		{
			bool result = AppUtilPINVOKE.CleanupNotifierBridge_RegisterCleanupDelegate(cleanupObject, notifyObject, cleanupDelegate, context);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static void UnregisterCleanupDelegate(IntPtr cleanupObject, IntPtr notifyObject)
		{
			AppUtilPINVOKE.CleanupNotifierBridge_UnregisterCleanupDelegate(cleanupObject, notifyObject);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static bool GetAndDestroyNotifiedFlag(IntPtr notifyObject)
		{
			bool result = AppUtilPINVOKE.CleanupNotifierBridge_GetAndDestroyNotifiedFlag(notifyObject);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal delegate void CleanupDelegate(IntPtr cleanupObject, IntPtr notifyObject, IntPtr context);
	}
}
