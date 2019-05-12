using System;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal sealed class AppOptionsInternal : IDisposable
	{
		private HandleRef swigCPtr;

		private bool swigCMemOwn;

		internal AppOptionsInternal(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		internal AppOptionsInternal() : this(AppUtilPINVOKE.new_AppOptionsInternal(), true)
		{
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static HandleRef getCPtr(AppOptionsInternal obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~AppOptionsInternal()
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
						AppUtilPINVOKE.delete_AppOptionsInternal(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Uri DatabaseUrl
		{
			get
			{
				return FirebaseApp.UrlStringToUri(this.GetDatabaseUrlInternal());
			}
			set
			{
				this.SetDatabaseUrlInternal(FirebaseApp.UriToUrlString(value));
			}
		}

		internal void SetDatabaseUrlInternal(string url)
		{
			AppUtilPINVOKE.AppOptionsInternal_SetDatabaseUrlInternal(this.swigCPtr, url);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal string GetDatabaseUrlInternal()
		{
			string result = AppUtilPINVOKE.AppOptionsInternal_GetDatabaseUrlInternal(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static AppOptionsInternal LoadFromJsonConfig(string config, AppOptionsInternal options)
		{
			IntPtr intPtr = AppUtilPINVOKE.AppOptionsInternal_LoadFromJsonConfig__SWIG_0(config, AppOptionsInternal.getCPtr(options));
			AppOptionsInternal result = (!(intPtr == IntPtr.Zero)) ? new AppOptionsInternal(intPtr, false) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static AppOptionsInternal LoadFromJsonConfig(string config)
		{
			IntPtr intPtr = AppUtilPINVOKE.AppOptionsInternal_LoadFromJsonConfig__SWIG_1(config);
			AppOptionsInternal result = (!(intPtr == IntPtr.Zero)) ? new AppOptionsInternal(intPtr, false) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public string AppId
		{
			get
			{
				string result = AppUtilPINVOKE.AppOptionsInternal_AppId_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				AppUtilPINVOKE.AppOptionsInternal_AppId_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string ApiKey
		{
			get
			{
				string result = AppUtilPINVOKE.AppOptionsInternal_ApiKey_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				AppUtilPINVOKE.AppOptionsInternal_ApiKey_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string MessageSenderId
		{
			get
			{
				string result = AppUtilPINVOKE.AppOptionsInternal_MessageSenderId_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				AppUtilPINVOKE.AppOptionsInternal_MessageSenderId_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string StorageBucket
		{
			get
			{
				string result = AppUtilPINVOKE.AppOptionsInternal_StorageBucket_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				AppUtilPINVOKE.AppOptionsInternal_StorageBucket_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string ProjectId
		{
			get
			{
				string result = AppUtilPINVOKE.AppOptionsInternal_ProjectId_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				AppUtilPINVOKE.AppOptionsInternal_ProjectId_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}

		public string PackageName
		{
			get
			{
				string result = AppUtilPINVOKE.AppOptionsInternal_PackageName_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
			set
			{
				AppUtilPINVOKE.AppOptionsInternal_PackageName_set(this.swigCPtr, value);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
			}
		}
	}
}
