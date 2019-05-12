using System;
using System.Runtime.InteropServices;

namespace Firebase.Messaging
{
	public sealed class MessagingOptions : IDisposable
	{
		private HandleRef swigCPtr;

		private bool swigCMemOwn;

		internal MessagingOptions(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public MessagingOptions() : this(FirebaseMessagingPINVOKE.new_MessagingOptions(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(MessagingOptions obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~MessagingOptions()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						FirebaseMessagingPINVOKE.delete_MessagingOptions(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public bool SuppressNotificationPermissionPrompt
		{
			get
			{
				bool result = FirebaseMessagingPINVOKE.MessagingOptions_SuppressNotificationPermissionPrompt_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				FirebaseMessagingPINVOKE.MessagingOptions_SuppressNotificationPermissionPrompt_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}
	}
}
