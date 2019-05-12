using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Access to application run-time data.</para>
	/// </summary>
	public sealed class Application
	{
		internal static AdvertisingIdentifierCallback OnAdvertisingIdentifierCallback;

		private static Application.LogCallback s_LogCallbackHandler;

		private static Application.LogCallback s_LogCallbackHandlerThreaded;

		private static volatile Application.LogCallback s_RegisterLogCallbackDeprecated;

		public static event Application.LogCallback logMessageReceived
		{
			add
			{
				Application.s_LogCallbackHandler = (Application.LogCallback)Delegate.Combine(Application.s_LogCallbackHandler, value);
				Application.SetLogCallbackDefined(true, Application.s_LogCallbackHandlerThreaded != null);
			}
			remove
			{
				Application.s_LogCallbackHandler = (Application.LogCallback)Delegate.Remove(Application.s_LogCallbackHandler, value);
			}
		}

		public static event Application.LogCallback logMessageReceivedThreaded
		{
			add
			{
				Application.s_LogCallbackHandlerThreaded = (Application.LogCallback)Delegate.Combine(Application.s_LogCallbackHandlerThreaded, value);
				Application.SetLogCallbackDefined(true, true);
			}
			remove
			{
				Application.s_LogCallbackHandlerThreaded = (Application.LogCallback)Delegate.Remove(Application.s_LogCallbackHandlerThreaded, value);
			}
		}

		/// <summary>
		///   <para>Quits the player application.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Quit();

		/// <summary>
		///   <para>Cancels quitting the application. This is useful for showing a splash screen at the end of a game.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CancelQuit();

		/// <summary>
		///   <para>The level index that was last loaded (Read Only).</para>
		/// </summary>
		public static extern int loadedLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The name of the level that was last loaded (Read Only).</para>
		/// </summary>
		public static extern string loadedLevelName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Loads the level by its name or index.</para>
		/// </summary>
		/// <param name="index">The level to load.</param>
		/// <param name="name">The name of the level to load.</param>
		public static void LoadLevel(int index)
		{
			Application.LoadLevelAsync(null, index, false, true);
		}

		/// <summary>
		///   <para>Loads the level by its name or index.</para>
		/// </summary>
		/// <param name="index">The level to load.</param>
		/// <param name="name">The name of the level to load.</param>
		public static void LoadLevel(string name)
		{
			Application.LoadLevelAsync(name, -1, false, true);
		}

		/// <summary>
		///   <para>Loads the level asynchronously in the background.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="levelName"></param>
		public static AsyncOperation LoadLevelAsync(int index)
		{
			return Application.LoadLevelAsync(null, index, false, false);
		}

		/// <summary>
		///   <para>Loads the level asynchronously in the background.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="levelName"></param>
		public static AsyncOperation LoadLevelAsync(string levelName)
		{
			return Application.LoadLevelAsync(levelName, -1, false, false);
		}

		/// <summary>
		///   <para>Loads the level additively and asynchronously in the background.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="levelName"></param>
		public static AsyncOperation LoadLevelAdditiveAsync(int index)
		{
			return Application.LoadLevelAsync(null, index, true, false);
		}

		/// <summary>
		///   <para>Loads the level additively and asynchronously in the background.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="levelName"></param>
		public static AsyncOperation LoadLevelAdditiveAsync(string levelName)
		{
			return Application.LoadLevelAsync(levelName, -1, true, false);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AsyncOperation LoadLevelAsync(string monoLevelName, int index, bool additive, bool mustCompleteNextFrame);

		/// <summary>
		///   <para>Unloads all GameObject associated with the given scene. Note that assets are currently not unloaded, in order to free up asset memory call Resources.UnloadAllUnusedAssets.</para>
		/// </summary>
		/// <param name="index">Index of the scene in the PlayerSettings to unload.</param>
		/// <param name="scenePath">Name of the scene to Unload.</param>
		/// <returns>
		///   <para>Return true if the scene is unloaded.</para>
		/// </returns>
		public static bool UnloadLevel(int index)
		{
			return Application.UnloadLevel(string.Empty, index);
		}

		/// <summary>
		///   <para>Unloads all GameObject associated with the given scene. Note that assets are currently not unloaded, in order to free up asset memory call Resources.UnloadAllUnusedAssets.</para>
		/// </summary>
		/// <param name="index">Index of the scene in the PlayerSettings to unload.</param>
		/// <param name="scenePath">Name of the scene to Unload.</param>
		/// <returns>
		///   <para>Return true if the scene is unloaded.</para>
		/// </returns>
		public static bool UnloadLevel(string scenePath)
		{
			return Application.UnloadLevel(scenePath, -1);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UnloadLevel(string monoScenePath, int index);

		/// <summary>
		///   <para>Loads a level additively.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="name"></param>
		public static void LoadLevelAdditive(int index)
		{
			Application.LoadLevelAsync(null, index, true, true);
		}

		/// <summary>
		///   <para>Loads a level additively.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="name"></param>
		public static void LoadLevelAdditive(string name)
		{
			Application.LoadLevelAsync(name, -1, true, true);
		}

		/// <summary>
		///   <para>Is some level being loaded? (Read Only)</para>
		/// </summary>
		[Obsolete("This property is deprecated, please use LoadLevelAsync to detect if a specific scene is currently loading.")]
		public static extern bool isLoadingLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The total number of levels available (Read Only).</para>
		/// </summary>
		public static extern int levelCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetStreamProgressForLevelByName(string levelName);

		/// <summary>
		///   <para>How far has the download progressed? [0...1].</para>
		/// </summary>
		/// <param name="levelIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetStreamProgressForLevel(int levelIndex);

		/// <summary>
		///   <para>How far has the download progressed? [0...1].</para>
		/// </summary>
		/// <param name="levelName"></param>
		public static float GetStreamProgressForLevel(string levelName)
		{
			return Application.GetStreamProgressForLevelByName(levelName);
		}

		/// <summary>
		///   <para>How many bytes have we downloaded from the main unity web stream (Read Only).</para>
		/// </summary>
		public static extern int streamedBytes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanStreamedLevelBeLoadedByName(string levelName);

		/// <summary>
		///   <para>Can the streamed level be loaded?</para>
		/// </summary>
		/// <param name="levelIndex"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CanStreamedLevelBeLoaded(int levelIndex);

		/// <summary>
		///   <para>Can the streamed level be loaded?</para>
		/// </summary>
		/// <param name="levelName"></param>
		public static bool CanStreamedLevelBeLoaded(string levelName)
		{
			return Application.CanStreamedLevelBeLoadedByName(levelName);
		}

		/// <summary>
		///   <para>Returns true when in any kind of player (Read Only).</para>
		/// </summary>
		public static extern bool isPlaying { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are we running inside the Unity editor? (Read Only)</para>
		/// </summary>
		public static extern bool isEditor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are we running inside a web player? (Read Only)</para>
		/// </summary>
		public static extern bool isWebPlayer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the platform the game is running (Read Only).</para>
		/// </summary>
		public static extern RuntimePlatform platform { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the current Runtime platform a known mobile platform.</para>
		/// </summary>
		public static bool isMobilePlatform
		{
			get
			{
				switch (Application.platform)
				{
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.MetroPlayerARM:
				case RuntimePlatform.WP8Player:
				case RuntimePlatform.BlackBerryPlayer:
				case RuntimePlatform.TizenPlayer:
					return true;
				}
				return false;
			}
		}

		/// <summary>
		///   <para>Is the current Runtime platform a known console platform.</para>
		/// </summary>
		public static bool isConsolePlatform
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				return platform == RuntimePlatform.PS3 || platform == RuntimePlatform.PS4 || platform == RuntimePlatform.XBOX360 || platform == RuntimePlatform.XboxOne;
			}
		}

		/// <summary>
		///   <para>Captures a screenshot at path filename as a PNG file.</para>
		/// </summary>
		/// <param name="filename">Pathname to save the screenshot file to.</param>
		/// <param name="superSize">Factor by which to increase resolution.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CaptureScreenshot(string filename, [DefaultValue("0")] int superSize);

		/// <summary>
		///   <para>Captures a screenshot at path filename as a PNG file.</para>
		/// </summary>
		/// <param name="filename">Pathname to save the screenshot file to.</param>
		/// <param name="superSize">Factor by which to increase resolution.</param>
		[ExcludeFromDocs]
		public static void CaptureScreenshot(string filename)
		{
			int superSize = 0;
			Application.CaptureScreenshot(filename, superSize);
		}

		/// <summary>
		///   <para>Should the player be running when the application is in the background?</para>
		/// </summary>
		public static extern bool runInBackground { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("use Application.isEditor instead")]
		public static bool isPlayer
		{
			get
			{
				return !Application.isEditor;
			}
		}

		/// <summary>
		///   <para>Is Unity activated with the Pro license?</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasProLicense();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasAdvancedLicense();

		internal static extern bool isBatchmode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal static extern bool isHumanControllingUs { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal static extern bool isRunningUnitTests { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasARGV(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetValueForARGV(string name);

		[Obsolete("Use Object.DontDestroyOnLoad instead")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DontDestroyOnLoad(Object mono);

		/// <summary>
		///   <para>Contains the path to the game data folder (Read Only).</para>
		/// </summary>
		public static extern string dataPath { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Contains the path to the StreamingAssets folder (Read Only).</para>
		/// </summary>
		public static extern string streamingAssetsPath { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Contains the path to a persistent data directory (Read Only).</para>
		/// </summary>
		[SecurityCritical]
		public static extern string persistentDataPath { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Contains the path to a temporary data / cache directory (Read Only).</para>
		/// </summary>
		public static extern string temporaryCachePath { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The path to the web player data file relative to the html file (Read Only).</para>
		/// </summary>
		public static extern string srcValue { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The absolute path to the web player data file (Read Only).</para>
		/// </summary>
		public static extern string absoluteURL { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		private static string ObjectToJSString(object o)
		{
			if (o == null)
			{
				return "null";
			}
			if (o is string)
			{
				string text = o.ToString().Replace("\\", "\\\\");
				text = text.Replace("\"", "\\\"");
				text = text.Replace("\n", "\\n");
				text = text.Replace("\r", "\\r");
				text = text.Replace("\0", string.Empty);
				text = text.Replace("\u2028", string.Empty);
				text = text.Replace("\u2029", string.Empty);
				return '"' + text + '"';
			}
			if (o is int || o is short || o is uint || o is ushort || o is byte)
			{
				return o.ToString();
			}
			if (o is float)
			{
				NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
				return ((float)o).ToString(numberFormat);
			}
			if (o is double)
			{
				NumberFormatInfo numberFormat2 = CultureInfo.InvariantCulture.NumberFormat;
				return ((double)o).ToString(numberFormat2);
			}
			if (o is char)
			{
				if ((char)o == '"')
				{
					return "\"\\\"\"";
				}
				return '"' + o.ToString() + '"';
			}
			else
			{
				if (o is IList)
				{
					IList list = (IList)o;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("new Array(");
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						if (i != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(Application.ObjectToJSString(list[i]));
					}
					stringBuilder.Append(")");
					return stringBuilder.ToString();
				}
				return Application.ObjectToJSString(o.ToString());
			}
		}

		/// <summary>
		///   <para>Calls a function in the containing web page (Web Player only).</para>
		/// </summary>
		/// <param name="functionName"></param>
		/// <param name="args"></param>
		public static void ExternalCall(string functionName, params object[] args)
		{
			Application.Internal_ExternalCall(Application.BuildInvocationForArguments(functionName, args));
		}

		private static string BuildInvocationForArguments(string functionName, params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			int num = args.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(Application.ObjectToJSString(args[i]));
			}
			stringBuilder.Append(')');
			stringBuilder.Append(';');
			return stringBuilder.ToString();
		}

		/// <summary>
		///   <para>Evaluates script function in the containing web page.</para>
		/// </summary>
		/// <param name="script">The Javascript function to call.</param>
		public static void ExternalEval(string script)
		{
			if (script.Length > 0 && script[script.Length - 1] != ';')
			{
				script += ';';
			}
			Application.Internal_ExternalCall(script);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ExternalCall(string script);

		/// <summary>
		///   <para>The version of the Unity runtime used to play the content.</para>
		/// </summary>
		public static extern string unityVersion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetBuildUnityVersion();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetNumericUnityVersion(string version);

		/// <summary>
		///   <para>Returns application version number  (Read Only).</para>
		/// </summary>
		public static extern string version { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns application bundle identifier at runtime.</para>
		/// </summary>
		public static extern string bundleIdentifier { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns application install mode (Read Only).</para>
		/// </summary>
		public static extern ApplicationInstallMode installMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns application running in sandbox (Read Only).</para>
		/// </summary>
		public static extern ApplicationSandboxType sandboxType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns application product name (Read Only).</para>
		/// </summary>
		public static extern string productName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Return application company name (Read Only).</para>
		/// </summary>
		public static extern string companyName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>A unique cloud project identifier. It is unique for every project (Read Only).</para>
		/// </summary>
		public static extern string cloudProjectId { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal static void InvokeOnAdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled)
		{
			if (Application.OnAdvertisingIdentifierCallback != null)
			{
				Application.OnAdvertisingIdentifierCallback(advertisingId, trackingEnabled);
			}
		}

		/// <summary>
		///   <para>Indicates whether Unity's webplayer security model is enabled.</para>
		/// </summary>
		public static extern bool webSecurityEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern string webSecurityHostUrl { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Opens the url in a browser.</para>
		/// </summary>
		/// <param name="url"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void OpenURL(string url);

		[Obsolete("For internal use only")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CommitSuicide(int mode);

		/// <summary>
		///   <para>Instructs game to try to render at a specified frame rate.</para>
		/// </summary>
		public static extern int targetFrameRate { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The language the user's operating system is running in.</para>
		/// </summary>
		public static extern SystemLanguage systemLanguage { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		private static void CallLogCallback(string logString, string stackTrace, LogType type, bool invokedOnMainThread)
		{
			if (invokedOnMainThread)
			{
				Application.LogCallback logCallback = Application.s_LogCallbackHandler;
				if (logCallback != null)
				{
					logCallback(logString, stackTrace, type);
				}
			}
			Application.LogCallback logCallback2 = Application.s_LogCallbackHandlerThreaded;
			if (logCallback2 != null)
			{
				logCallback2(logString, stackTrace, type);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLogCallbackDefined(bool defined, bool threaded);

		/// <summary>
		///   <para>Stack trace logging options. The default value is StackTraceLogType.ScriptOnly.</para>
		/// </summary>
		public static extern StackTraceLogType stackTraceLogType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Priority of background loading thread.</para>
		/// </summary>
		public static extern ThreadPriority backgroundLoadingPriority { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns the type of Internet reachability currently possible on the device.</para>
		/// </summary>
		public static extern NetworkReachability internetReachability { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns false if application is altered in any way after it was built.</para>
		/// </summary>
		public static extern bool genuine { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns true if application integrity can be confirmed.</para>
		/// </summary>
		public static extern bool genuineCheckAvailable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Request authorization to use the webcam or microphone in the Web Player.</para>
		/// </summary>
		/// <param name="mode"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AsyncOperation RequestUserAuthorization(UserAuthorization mode);

		/// <summary>
		///   <para>Check if the user has authorized use of the webcam or microphone in the Web Player.</para>
		/// </summary>
		/// <param name="mode"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasUserAuthorization(UserAuthorization mode);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ReplyToUserAuthorizationRequest(bool reply, [DefaultValue("false")] bool remember);

		[ExcludeFromDocs]
		internal static void ReplyToUserAuthorizationRequest(bool reply)
		{
			bool remember = false;
			Application.ReplyToUserAuthorizationRequest(reply, remember);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetUserAuthorizationRequestMode_Internal();

		internal static UserAuthorization GetUserAuthorizationRequestMode()
		{
			return (UserAuthorization)Application.GetUserAuthorizationRequestMode_Internal();
		}

		internal static extern bool submitAnalytics { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Checks whether splash screen is being shown.</para>
		/// </summary>
		public static extern bool isShowingSplashScreen { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("Application.RegisterLogCallback is deprecated. Use Application.logMessageReceived instead.")]
		public static void RegisterLogCallback(Application.LogCallback handler)
		{
			Application.RegisterLogCallback(handler, false);
		}

		[Obsolete("Application.RegisterLogCallbackThreaded is deprecated. Use Application.logMessageReceivedThreaded instead.")]
		public static void RegisterLogCallbackThreaded(Application.LogCallback handler)
		{
			Application.RegisterLogCallback(handler, true);
		}

		private static void RegisterLogCallback(Application.LogCallback handler, bool threaded)
		{
			if (Application.s_RegisterLogCallbackDeprecated != null)
			{
				Application.logMessageReceived -= Application.s_RegisterLogCallbackDeprecated;
				Application.logMessageReceivedThreaded -= Application.s_RegisterLogCallbackDeprecated;
			}
			Application.s_RegisterLogCallbackDeprecated = handler;
			if (handler != null)
			{
				if (threaded)
				{
					Application.logMessageReceivedThreaded += handler;
				}
				else
				{
					Application.logMessageReceived += handler;
				}
			}
		}

		/// <summary>
		///   <para>Use this delegate type with Application.logMessageReceived or Application.logMessageReceivedThreaded to monitor what gets logged.</para>
		/// </summary>
		/// <param name="condition"></param>
		/// <param name="stackTrace"></param>
		/// <param name="type"></param>
		public delegate void LogCallback(string condition, string stackTrace, LogType type);
	}
}
