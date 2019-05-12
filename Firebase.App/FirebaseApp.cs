using Firebase.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Firebase
{
	public sealed class FirebaseApp : IDisposable
	{
		private HandleRef swigCPtr;

		private bool swigCMemOwn;

		private string name;

		private static Dictionary<string, WeakReference> nameToProxy = new Dictionary<string, WeakReference>();

		private static Dictionary<IntPtr, WeakReference> cPtrToProxy = new Dictionary<IntPtr, WeakReference>();

		private static bool AppUtilCallbacksInitialized = false;

		private static object AppUtilCallbacksLock = new object();

		private static bool PreventOnAllAppsDestroyed = false;

		private static bool installedCerts = false;

		private static bool crashlyticsInitializationAttempted = false;

		private const int CheckDependenciesNoThread = -1;

		private const int CheckDependenciesPendingThread = -2;

		private static int CheckDependenciesThread = -1;

		private static object CheckDependenciesThreadLock = new object();

		private FirebaseAppPlatform appPlatform;

		[CompilerGenerated]
		private static FirebaseApp.LogMessageDelegate <>f__mg$cache0;

		internal FirebaseApp(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		internal static HandleRef getCPtr(FirebaseApp obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~FirebaseApp()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.RemoveReference();
		}

		internal static PlatformLogLevel ConvertLogLevel(LogLevel logLevel)
		{
			switch (logLevel)
			{
			case LogLevel.Verbose:
				return PlatformLogLevel.Verbose;
			case LogLevel.Info:
				return PlatformLogLevel.Info;
			case LogLevel.Warning:
				return PlatformLogLevel.Warning;
			case LogLevel.Error:
				return PlatformLogLevel.Error;
			}
			return PlatformLogLevel.Debug;
		}

		[FirebaseApp.MonoPInvokeCallbackAttribute(typeof(FirebaseApp.LogMessageDelegate))]
		internal static void LogMessage(LogLevel logLevel, string message)
		{
			FirebaseLogger.LogMessage(FirebaseApp.ConvertLogLevel(logLevel), message);
		}

		internal static void TranslateDllNotFoundException(Action closureToExecute)
		{
			try
			{
				closureToExecute();
			}
			catch (Exception ex)
			{
				if (ex.GetBaseException() is DllNotFoundException)
				{
					throw new InitializationException(InitResult.FailedMissingDependency, ErrorMessages.DllNotFoundExceptionErrorMessage);
				}
				throw;
			}
		}

		public static FirebaseApp DefaultInstance
		{
			get
			{
				FirebaseApp instance = FirebaseApp.GetInstance(FirebaseApp.DefaultName);
				return (instance == null) ? FirebaseApp.Create() : instance;
			}
		}

		public static FirebaseApp GetInstance(string name)
		{
			FirebaseApp.ThrowIfCheckDependenciesRunning();
			FirebaseApp firebaseApp = null;
			object obj = FirebaseApp.nameToProxy;
			FirebaseApp result;
			lock (obj)
			{
				WeakReference weakReference;
				if (FirebaseApp.nameToProxy.TryGetValue(name, out weakReference))
				{
					firebaseApp = (FirebaseApp.WeakReferenceGetTarget(weakReference) as FirebaseApp);
					if (firebaseApp == null)
					{
						FirebaseApp.nameToProxy.Remove(name);
					}
				}
				result = firebaseApp;
			}
			return result;
		}

		public static FirebaseApp Create()
		{
			return FirebaseApp.CreateAndTrack(() => FirebaseApp.CreateInternal(), FirebaseApp.GetInstance(FirebaseApp.DefaultName));
		}

		public static FirebaseApp Create(AppOptions options)
		{
			return FirebaseApp.CreateAndTrack(() => FirebaseApp.CreateInternal(options.ConvertToInternal()), FirebaseApp.GetInstance(FirebaseApp.DefaultName));
		}

		public static FirebaseApp Create(AppOptions options, string name)
		{
			return FirebaseApp.CreateAndTrack(() => FirebaseApp.CreateInternal(options.ConvertToInternal(), name), FirebaseApp.GetInstance(name));
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public static LogLevel LogLevel
		{
			get
			{
				return (LogLevel)AppUtilPINVOKE.FirebaseApp_GetLogLevelInternal();
			}
			set
			{
				AppUtilPINVOKE.FirebaseApp_SetLogLevelInternal((int)value);
			}
		}

		private bool IsDataCollectionDefaultEnabled
		{
			get
			{
				return this.IsDataCollectionDefaultEnabledInternal();
			}
			set
			{
				this.SetDataCollectionDefaultEnabledInternal(value);
			}
		}

		private void AddReference()
		{
			FirebaseApp.ThrowIfCheckDependenciesRunning();
			object obj = FirebaseApp.nameToProxy;
			lock (obj)
			{
				WeakReference value = new WeakReference(this, false);
				this.swigCMemOwn = true;
				FirebaseApp.nameToProxy[this.Name] = value;
				FirebaseApp.cPtrToProxy[this.swigCPtr.Handle] = value;
			}
		}

		private void RemoveReference()
		{
			FirebaseApp.ThrowIfCheckDependenciesRunning();
			object obj = FirebaseApp.nameToProxy;
			lock (obj)
			{
				IntPtr handle = this.swigCPtr.Handle;
				if (handle != IntPtr.Zero)
				{
					GC.SuppressFinalize(this);
					if (this.swigCMemOwn)
					{
						int count = FirebaseApp.nameToProxy.Count;
						FirebaseApp.cPtrToProxy.Remove(handle);
						FirebaseApp.nameToProxy.Remove(this.Name);
						AppUtilPINVOKE.delete_FirebaseApp(this.swigCPtr);
						if (count > 0 && FirebaseApp.nameToProxy.Count == 0)
						{
							FirebaseApp.OnAllAppsDestroyed();
						}
					}
				}
				this.swigCMemOwn = false;
				this.swigCPtr = new HandleRef(null, IntPtr.Zero);
			}
		}

		private void ThrowIfNull()
		{
			if (this.swigCPtr.Handle == IntPtr.Zero)
			{
				throw new NullReferenceException("App has been disposed");
			}
		}

		private static void InitializeAppUtilCallbacks()
		{
			object appUtilCallbacksLock = FirebaseApp.AppUtilCallbacksLock;
			lock (appUtilCallbacksLock)
			{
				if (!FirebaseApp.AppUtilCallbacksInitialized)
				{
					if (FirebaseApp.<>f__mg$cache0 == null)
					{
						FirebaseApp.<>f__mg$cache0 = new FirebaseApp.LogMessageDelegate(FirebaseApp.LogMessage);
					}
					AppUtil.SetLogFunction(FirebaseApp.<>f__mg$cache0);
					AppUtil.AppEnableLogCallback(true);
					if (!PlatformInformation.IsAndroid)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>
						{
							{
								"Firebase.Analytics.FirebaseAnalytics, Firebase.Analytics",
								"analytics"
							},
							{
								"Firebase.Auth.FirebaseAuth, Firebase.Auth",
								"auth"
							},
							{
								"Firebase.Database.FirebaseDatabase, Firebase.Database",
								"database"
							},
							{
								"Firebase.DynamicLinks.DynamicLinks, Firebase.DynamicLinks",
								"dynamic_links"
							},
							{
								"Firebase.Functions.FirebaseFunctions, Firebase.Functions",
								"functions"
							},
							{
								"Firebase.InstanceId.FirebaseInstanceId, Firebase.InstanceId",
								"instance_id"
							},
							{
								"Firebase.Invites.FirebaseInvites, Firebase.Invites",
								"invites"
							},
							{
								"Firebase.Messaging.FirebaseMessaging, Firebase.Messaging",
								"messaging"
							},
							{
								"Firebase.RemoteConfig.FirebaseRemoteConfig, Firebase.RemoteConfig",
								"remote_config"
							},
							{
								"Firebase.Storage.FirebaseStorage, Firebase.Storage",
								"storage"
							}
						};
						AppUtil.SetEnabledAllAppCallbacks(false);
						foreach (KeyValuePair<string, string> keyValuePair in dictionary)
						{
							try
							{
								if (Type.GetType(keyValuePair.Key) != null)
								{
									FirebaseApp.LogMessage(LogLevel.Debug, string.Format("Enabling module '{0}' for '{1}'", keyValuePair.Value, keyValuePair.Key));
									AppUtil.SetEnabledAppCallbackByName(keyValuePair.Value, true);
								}
							}
							catch (Exception)
							{
							}
						}
					}
					FirebaseApp.AppUtilCallbacksInitialized = true;
				}
			}
		}

		private static void OnAllAppsDestroyed()
		{
			if (FirebaseApp.PreventOnAllAppsDestroyed || FirebaseApp.nameToProxy.Count > 0)
			{
				return;
			}
			object appUtilCallbacksLock = FirebaseApp.AppUtilCallbacksLock;
			lock (appUtilCallbacksLock)
			{
				if (FirebaseApp.AppUtilCallbacksInitialized)
				{
					if (!PlatformInformation.IsAndroid)
					{
						AppUtil.SetEnabledAllAppCallbacks(false);
					}
					AppUtil.AppEnableLogCallback(false);
					AppUtil.SetLogFunction(null);
					FirebaseApp.AppUtilCallbacksInitialized = false;
				}
			}
		}

		internal static Uri UrlStringToUri(string urlString)
		{
			if (string.IsNullOrEmpty(urlString))
			{
				return null;
			}
			Uri result;
			try
			{
				result = new Uri(urlString);
			}
			catch (UriFormatException)
			{
				result = null;
			}
			return result;
		}

		internal static string UriToUrlString(Uri uri)
		{
			return (!(uri != null)) ? string.Empty : uri.ToString();
		}

		internal static object WeakReferenceGetTarget(WeakReference weakReference)
		{
			if (weakReference != null)
			{
				try
				{
					return weakReference.Target;
				}
				catch (InvalidOperationException)
				{
					return null;
				}
			}
			return null;
		}

		private static bool InitializeCrashlyticsIfPresent()
		{
			try
			{
				Assembly assembly = Assembly.Load("Firebase.Crashlytics");
				Type type = assembly.GetType("Firebase.Crashlytics.Crashlytics");
				if (type == null)
				{
					throw new InitializationException(InitResult.FailedMissingDependency, "Crashlytics initialization failed. Could not find Crashlytics class.");
				}
				MethodInfo method = type.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					throw new InitializationException(InitResult.FailedMissingDependency, "Crashlytics initialization failed. Could not find Crashlytics initializer.");
				}
				method.Invoke(null, null);
			}
			catch (FileNotFoundException)
			{
				return false;
			}
			catch (Exception inner)
			{
				throw new InitializationException(InitResult.FailedMissingDependency, "Crashlytics initialization failed with an unexpected error.", inner);
			}
			return true;
		}

		private static FirebaseApp CreateAndTrack(FirebaseApp.CreateDelegate createDelegate, FirebaseApp existingProxy)
		{
			FirebaseApp.ThrowIfCheckDependenciesRunning();
			FirebaseHandler.Create(FirebaseAppUtils.Instance);
			object obj = FirebaseApp.nameToProxy;
			FirebaseApp result2;
			lock (obj)
			{
				FirebaseApp.InitializeAppUtilCallbacks();
				HandleRef cptr = new HandleRef(null, IntPtr.Zero);
				FirebaseApp firebaseApp;
				try
				{
					FirebaseApp.AppSetDefaultConfigPath(PlatformInformation.DefaultConfigLocation);
					firebaseApp = createDelegate();
					if (AppUtilPINVOKE.SWIGPendingException.Pending)
					{
						throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
					}
					if (firebaseApp == null)
					{
						throw new InitializationException(InitResult.FailedMissingDependency, "App creation failed with an unknown error.");
					}
					cptr = FirebaseApp.getCPtr(firebaseApp);
				}
				catch (ApplicationException ex)
				{
					FirebaseApp.OnAllAppsDestroyed();
					string text = ex.Message;
					InitResult result = InitResult.FailedMissingDependency;
					int num = text.IndexOf(":");
					if (num >= 0)
					{
						result = (InitResult)int.Parse(text.Substring(0, num));
						text = text.Substring(num + 1);
					}
					if (text.IndexOf("Please verify the AAR") >= 0)
					{
						text = text + "\n" + ErrorMessages.DependencyNotFoundErrorMessage;
					}
					throw new InitializationException(result, text);
				}
				catch (Exception ex2)
				{
					FirebaseApp.OnAllAppsDestroyed();
					throw ex2;
				}
				if (cptr.Handle == IntPtr.Zero)
				{
					result2 = null;
				}
				else
				{
					WeakReference weakReference;
					if (FirebaseApp.cPtrToProxy.TryGetValue(cptr.Handle, out weakReference))
					{
						FirebaseApp firebaseApp2 = FirebaseApp.WeakReferenceGetTarget(weakReference) as FirebaseApp;
						if (firebaseApp2 != null)
						{
							if (existingProxy != firebaseApp2)
							{
								FirebaseApp.LogMessage(LogLevel.Warning, string.Format("Detected multiple FirebaseApp proxies for {0}", existingProxy.Name));
								existingProxy.Dispose();
							}
							return firebaseApp2;
						}
					}
					if (FirebaseApp.cPtrToProxy.Count == 0)
					{
						string text2 = "fire-" + PlatformInformation.RuntimeName;
						FirebaseApp.RegisterLibraryInternal(text2, VersionInfo.SdkVersion);
						FirebaseApp.RegisterLibraryInternal(text2 + "-ver", PlatformInformation.RuntimeVersion);
					}
					firebaseApp.name = firebaseApp.NameInternal;
					firebaseApp.AddReference();
					if (!FirebaseApp.installedCerts)
					{
						FirebaseApp.installedCerts = true;
						Services.RootCerts.Install(firebaseApp.AppPlatform);
					}
					if (!FirebaseApp.crashlyticsInitializationAttempted)
					{
						FirebaseApp.crashlyticsInitializationAttempted = true;
						FirebaseHandler.RunOnMainThread<bool>(() => FirebaseApp.InitializeCrashlyticsIfPresent());
					}
					result2 = firebaseApp;
				}
			}
			return result2;
		}

		private static void SetCheckDependenciesThread(int threadId)
		{
			object checkDependenciesThreadLock = FirebaseApp.CheckDependenciesThreadLock;
			lock (checkDependenciesThreadLock)
			{
				if (FirebaseApp.CheckDependenciesThread != -1 && FirebaseApp.CheckDependenciesThread != -2 && FirebaseApp.CheckDependenciesThread != Thread.CurrentThread.ManagedThreadId)
				{
					throw new InvalidOperationException("Don't call other Firebase functions while CheckDependencies is running.");
				}
				FirebaseApp.CheckDependenciesThread = threadId;
			}
		}

		private static void ThrowIfCheckDependenciesRunning()
		{
			object checkDependenciesThreadLock = FirebaseApp.CheckDependenciesThreadLock;
			lock (checkDependenciesThreadLock)
			{
				if (FirebaseApp.CheckDependenciesThread != -1 && FirebaseApp.CheckDependenciesThread != Thread.CurrentThread.ManagedThreadId)
				{
					throw new InvalidOperationException("Don't call Firebase functions before CheckDependencies has finished");
				}
			}
		}

		public static Task<DependencyStatus> CheckDependenciesAsync()
		{
			FirebaseApp.SetCheckDependenciesThread(-2);
			FirebaseHandler.CreatePartialOnMainThread(FirebaseAppUtils.Instance);
			FirebaseApp.InitializeAppUtilCallbacks();
			return Task.Run<DependencyStatus>(delegate()
			{
				FirebaseApp.SetCheckDependenciesThread(Thread.CurrentThread.ManagedThreadId);
				DependencyStatus result = FirebaseApp.CheckDependencies();
				FirebaseApp.SetCheckDependenciesThread(-1);
				return result;
			});
		}

		public static Task<DependencyStatus> CheckAndFixDependenciesAsync()
		{
			return FirebaseApp.CheckDependenciesAsync().ContinueWith<Task<DependencyStatus>>(delegate(Task<DependencyStatus> checkTask)
			{
				DependencyStatus result = checkTask.Result;
				if (result != DependencyStatus.Available)
				{
					return FirebaseApp.FixDependenciesAsync().ContinueWith<Task<DependencyStatus>>((Task t) => FirebaseApp.CheckDependenciesAsync()).Unwrap<DependencyStatus>();
				}
				return checkTask;
			}).Unwrap<DependencyStatus>();
		}

		public static DependencyStatus CheckDependencies()
		{
			DependencyStatus status = DependencyStatus.Available;
			FirebaseApp.TranslateDllNotFoundException(delegate
			{
				status = FirebaseApp.CheckDependenciesInternal();
			});
			return status;
		}

		private static DependencyStatus CheckDependenciesInternal()
		{
			if (!PlatformInformation.IsAndroid || FirebaseApp.GetInstance(FirebaseApp.DefaultName) != null)
			{
				return DependencyStatus.Available;
			}
			InitResult initResult = InitResult.Success;
			FirebaseApp firebaseApp = null;
			try
			{
				firebaseApp = FirebaseApp.DefaultInstance;
			}
			catch (InitializationException ex)
			{
				initResult = ex.InitResult;
				if (initResult != InitResult.FailedMissingDependency)
				{
					throw ex;
				}
			}
			finally
			{
				if (firebaseApp != null)
				{
					firebaseApp.Dispose();
				}
			}
			switch (AppUtil.CheckAndroidDependencies())
			{
			case GooglePlayServicesAvailability.AvailabilityAvailable:
				return DependencyStatus.Available;
			case GooglePlayServicesAvailability.AvailabilityUnavailableDisabled:
				return DependencyStatus.UnavailableDisabled;
			case GooglePlayServicesAvailability.AvailabilityUnavailableInvalid:
				return DependencyStatus.UnavailableInvalid;
			case GooglePlayServicesAvailability.AvailabilityUnavailableMissing:
				return DependencyStatus.UnavilableMissing;
			case GooglePlayServicesAvailability.AvailabilityUnavailablePermissions:
				return DependencyStatus.UnavailablePermission;
			case GooglePlayServicesAvailability.AvailabilityUnavailableUpdateRequired:
				return DependencyStatus.UnavailableUpdaterequired;
			case GooglePlayServicesAvailability.AvailabilityUnavailableUpdating:
				return DependencyStatus.UnavailableUpdating;
			case GooglePlayServicesAvailability.AvailabilityUnavailableOther:
				return DependencyStatus.UnavailableOther;
			default:
				return (initResult != InitResult.Success) ? DependencyStatus.UnavailableOther : DependencyStatus.Available;
			}
		}

		public static Task FixDependenciesAsync()
		{
			Task task = null;
			FirebaseApp.TranslateDllNotFoundException(delegate
			{
				task = AppUtil.FixAndroidDependenciesAsync().ContinueWith(delegate(Task t)
				{
					if (t.Exception != null)
					{
						throw t.Exception;
					}
					FirebaseApp.ResetDefaultAppCPtr();
				});
			});
			return task;
		}

		private static void ResetDefaultAppCPtr()
		{
			FirebaseApp.ThrowIfCheckDependenciesRunning();
			object obj = FirebaseApp.nameToProxy;
			lock (obj)
			{
				AppUtil.InitializePlayServicesInternal();
				FirebaseApp.PreventOnAllAppsDestroyed = true;
				FirebaseApp defaultInstance = FirebaseApp.DefaultInstance;
				defaultInstance.RemoveReference();
				defaultInstance.swigCPtr = new HandleRef(defaultInstance, AppUtilPINVOKE.FirebaseApp_CreateInternal__SWIG_0());
				defaultInstance.AddReference();
				FirebaseApp.PreventOnAllAppsDestroyed = false;
				AppUtil.TerminatePlayServicesInternal();
			}
		}

		public AppOptions Options
		{
			get
			{
				this.ThrowIfNull();
				return new AppOptions(this.options());
			}
		}

		internal static SynchronizationContext ThreadSynchronizationContext
		{
			get
			{
				return (FirebaseHandler.DefaultInstance == null || !FirebaseHandler.DefaultInstance.IsMainThread()) ? null : PlatformInformation.SynchronizationContext;
			}
		}

		internal FirebaseAppPlatform AppPlatform
		{
			get
			{
				object typeFromHandle = typeof(FirebaseAppPlatform);
				FirebaseAppPlatform result;
				lock (typeFromHandle)
				{
					if (this.appPlatform == null)
					{
						this.appPlatform = new FirebaseAppPlatform(this);
					}
					result = this.appPlatform;
				}
				return result;
			}
		}

		internal AppOptionsInternal options()
		{
			AppOptionsInternal result = new AppOptionsInternal(AppUtilPINVOKE.FirebaseApp_options(this.swigCPtr), false);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal void SetDataCollectionDefaultEnabledInternal(bool enabled)
		{
			AppUtilPINVOKE.FirebaseApp_SetDataCollectionDefaultEnabledInternal(this.swigCPtr, enabled);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal bool IsDataCollectionDefaultEnabledInternal()
		{
			bool result = AppUtilPINVOKE.FirebaseApp_IsDataCollectionDefaultEnabledInternal(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal string NameInternal
		{
			get
			{
				string result = AppUtilPINVOKE.FirebaseApp_NameInternal_get(this.swigCPtr);
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		internal static FirebaseApp CreateInternal()
		{
			IntPtr intPtr = AppUtilPINVOKE.FirebaseApp_CreateInternal__SWIG_0();
			FirebaseApp result = (!(intPtr == IntPtr.Zero)) ? new FirebaseApp(intPtr, false) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static FirebaseApp CreateInternal(AppOptionsInternal options)
		{
			IntPtr intPtr = AppUtilPINVOKE.FirebaseApp_CreateInternal__SWIG_1(AppOptionsInternal.getCPtr(options));
			FirebaseApp result = (!(intPtr == IntPtr.Zero)) ? new FirebaseApp(intPtr, false) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static FirebaseApp CreateInternal(AppOptionsInternal options, string name)
		{
			IntPtr intPtr = AppUtilPINVOKE.FirebaseApp_CreateInternal__SWIG_2(AppOptionsInternal.getCPtr(options), name);
			FirebaseApp result = (!(intPtr == IntPtr.Zero)) ? new FirebaseApp(intPtr, false) : null;
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static void SetLogLevelInternal(LogLevel level)
		{
			AppUtilPINVOKE.FirebaseApp_SetLogLevelInternal((int)level);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static LogLevel GetLogLevelInternal()
		{
			LogLevel result = (LogLevel)AppUtilPINVOKE.FirebaseApp_GetLogLevelInternal();
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		internal static void RegisterLibraryInternal(string library, string version)
		{
			AppUtilPINVOKE.FirebaseApp_RegisterLibraryInternal(library, version);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		internal static void AppSetDefaultConfigPath(string path)
		{
			AppUtilPINVOKE.FirebaseApp_AppSetDefaultConfigPath(path);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static string DefaultName
		{
			get
			{
				string result = AppUtilPINVOKE.FirebaseApp_DefaultName_get();
				if (AppUtilPINVOKE.SWIGPendingException.Pending)
				{
					throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
				}
				return result;
			}
		}

		internal delegate void LogMessageDelegate(LogLevel log_level, string message);

		[AttributeUsage(AttributeTargets.Method)]
		private sealed class MonoPInvokeCallbackAttribute : Attribute
		{
			public MonoPInvokeCallbackAttribute(Type t)
			{
			}
		}

		private delegate FirebaseApp CreateDelegate();
	}
}
