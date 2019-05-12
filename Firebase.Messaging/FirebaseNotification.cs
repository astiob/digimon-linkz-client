using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase.Messaging
{
	public sealed class FirebaseNotification : IDisposable
	{
		private HandleRef swigCPtr;

		private bool swigCMemOwn;

		internal FirebaseNotification(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public FirebaseNotification() : this(FirebaseMessagingPINVOKE.new_FirebaseNotification(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(FirebaseNotification obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~FirebaseNotification()
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
						FirebaseMessagingPINVOKE.delete_FirebaseNotification(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public string Title
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Title_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Body
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Body_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Icon
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Icon_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Sound
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Sound_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Badge
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Badge_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Tag
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Tag_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Color
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_Color_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string ClickAction
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_ClickAction_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string BodyLocalizationKey
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_BodyLocalizationKey_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public IEnumerable<string> BodyLocalizationArgs
		{
			get
			{
				IntPtr intPtr = FirebaseMessagingPINVOKE.FirebaseNotification_BodyLocalizationArgs_get(this.swigCPtr);
				StringList result = (!(intPtr == IntPtr.Zero)) ? new StringList(intPtr, false) : null;
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string TitleLocalizationKey
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseNotification_TitleLocalizationKey_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public IEnumerable<string> TitleLocalizationArgs
		{
			get
			{
				IntPtr intPtr = FirebaseMessagingPINVOKE.FirebaseNotification_TitleLocalizationArgs_get(this.swigCPtr);
				StringList result = (!(intPtr == IntPtr.Zero)) ? new StringList(intPtr, false) : null;
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}
	}
}
