using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Firebase.Messaging
{
	public sealed class FirebaseMessage : IDisposable
	{
		private HandleRef swigCPtr;

		private bool swigCMemOwn;

		internal FirebaseMessage(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public FirebaseMessage() : this(FirebaseMessagingPINVOKE.new_FirebaseMessage(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(FirebaseMessage obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~FirebaseMessage()
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
						FirebaseMessagingPINVOKE.delete_FirebaseMessage(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public TimeSpan TimeToLive
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.TimeToLiveInternal);
			}
		}

		public FirebaseNotification Notification
		{
			get
			{
				IntPtr intPtr = FirebaseMessaging.MessageCopyNotification(this.swigCPtr.Handle);
				if (intPtr != IntPtr.Zero)
				{
					return new FirebaseNotification(intPtr, true);
				}
				return null;
			}
		}

		public Uri Link
		{
			get
			{
				return FirebaseApp.UrlStringToUri(this.LinkInternal);
			}
		}

		public string From
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_From_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string To
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_To_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				FirebaseMessagingPINVOKE.FirebaseMessage_To_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string CollapseKey
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_CollapseKey_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public IDictionary<string, string> Data
		{
			get
			{
				IntPtr intPtr = FirebaseMessagingPINVOKE.FirebaseMessage_Data_get(this.swigCPtr);
				StringStringMap result = (!(intPtr == IntPtr.Zero)) ? new StringStringMap(intPtr, false) : null;
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				StringStringMap stringStringMap = new StringStringMap();
				foreach (KeyValuePair<string, string> item in value)
				{
					stringStringMap.Add(item);
				}
				FirebaseMessagingPINVOKE.FirebaseMessage_Data_set(this.swigCPtr, StringStringMap.getCPtr(stringStringMap));
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string RawData
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_RawData_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string MessageId
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_MessageId_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				FirebaseMessagingPINVOKE.FirebaseMessage_MessageId_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string MessageType
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_MessageType_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Priority
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_Priority_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		internal int TimeToLiveInternal
		{
			get
			{
				int result = FirebaseMessagingPINVOKE.FirebaseMessage_TimeToLiveInternal_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string Error
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_Error_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public string ErrorDescription
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_ErrorDescription_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		public bool NotificationOpened
		{
			get
			{
				bool result = FirebaseMessagingPINVOKE.FirebaseMessage_NotificationOpened_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		internal string LinkInternal
		{
			get
			{
				string result = FirebaseMessagingPINVOKE.FirebaseMessage_LinkInternal_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}
	}
}
