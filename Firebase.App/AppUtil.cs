using System;
using System.Threading.Tasks;

namespace Firebase
{
	internal class AppUtil
	{
		internal static void PollCallbacks()
		{
			AppUtilPINVOKE.PollCallbacks();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static void AppEnableLogCallback(bool arg0)
		{
			AppUtilPINVOKE.AppEnableLogCallback(arg0);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static LogLevel AppGetLogLevel()
		{
			LogLevel result = (LogLevel)AppUtilPINVOKE.AppGetLogLevel();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static void SetEnabledAllAppCallbacks(bool arg0)
		{
			AppUtilPINVOKE.SetEnabledAllAppCallbacks(arg0);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static void SetEnabledAppCallbackByName(string arg0, bool arg1)
		{
			AppUtilPINVOKE.SetEnabledAppCallbackByName(arg0, arg1);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static void SetLogFunction(FirebaseApp.LogMessageDelegate arg0)
		{
			AppUtilPINVOKE.SetLogFunction(arg0);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static AppOptionsInternal AppOptionsLoadFromJsonConfig(string config)
		{
			IntPtr intPtr = AppUtilPINVOKE.AppOptionsLoadFromJsonConfig(config);
			AppOptionsInternal result = (!(intPtr == IntPtr.Zero)) ? new AppOptionsInternal(intPtr, true) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static GooglePlayServicesAvailability CheckAndroidDependencies()
		{
			GooglePlayServicesAvailability result = (GooglePlayServicesAvailability)AppUtilPINVOKE.CheckAndroidDependencies();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Task FixAndroidDependenciesAsync()
		{
			return FutureVoid.GetTask(new FutureVoid(AppUtilPINVOKE.FixAndroidDependencies(), true));
		}

		internal static void InitializePlayServicesInternal()
		{
			AppUtilPINVOKE.InitializePlayServicesInternal();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static void TerminatePlayServicesInternal()
		{
			AppUtilPINVOKE.TerminatePlayServicesInternal();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}
	}
}
