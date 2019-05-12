using Firebase.Platform;
using System;

namespace Firebase
{
	internal class ErrorMessages
	{
		private static string DEPENDENCY_NOT_FOUND_ERROR_ANDROID = "On Android, Firebase requires C/C++ and Java components\nthat are distributed with the Firebase and Android SDKs.\n\nIt's likely the required dependencies for Firebase were not included\nin your Unity project.\nAssets/Plugins/Android/ in your Unity project should contain\nAAR files in the form firebase-*.aar\nYou may have disabled the Android Resolver which would\nhave added the AAR dependencies for you.\n\nDo the following to enable the Android Resolver in Unity:\n* Select the menu option 'Assets -> Play Services Resolver -> \n  Android Resolver -> Settings'\n* In the Android Resolver settings check\n  'Enable Background Resolution'\n* Select the menu option 'Assets -> Play Services Resolver ->\n  Android Resolver -> Resolve Client Jars' to force Android\n  dependency resolution.\n* Rebuild your APK and deploy.\n";

		private static string DEPENDENCY_NOT_FOUND_ERROR_IOS = "On iOS, Firebase requires native (C/C++) and Cocoapod components\nthat are distributed with the Firebase SDK and via Cocoapods.\n\nIt's likely that you did not include the require Cocoapod\ndependencies for Firebase in your Unity project.\nYou may have disabled the iOS Resolver which would have added\nthe Cocoapod dependencies for you.\n\nDo the following to enable the iOS Resolver in Unity:\n* Select the menu option 'Assets -> Play Services Resolver ->\n  iOS Resolver -> Settings'\n* In the iOS Resolver settings check 'Podfile Generation' and\n  'Add Cocoapods to Generated Xcode Project'.\n* Build your iOS project and check the Unity console for any\n  errors associated with Cocoapod tool execution.\n  You will need to correctly install Cocoapods tools to generate\n  a working build.\n";

		private static string DEPENDENCY_NOT_FOUND_ERROR_GENERIC = "Firebase is distributed with native (C/C++) dependencies\nthat are required by the SDK.\n\nIt's possible that parts of Firebase SDK have been removed from\nyour Unity project.\n\nTo resolve the problem, try re-importing your Firebase plugins and\nbuilding again.\n\nAlternatively, you may be trying to use Firebase on an unsupported\nplatform.  See the Firebase website for the list of supported\nplatforms.\n";

		private static string DLL_NOT_FOUND_ERROR_ANDROID = "Firebase's libApp.so was not found for this device's architecture\nin your APK.\n";

		private static string DLL_NOT_FOUND_ERROR_IOS = "A Firebase static library (e.g libApp.a) was not linked with your\niOS application.\n";

		private static string DLL_NOT_FOUND_ERROR_GENERIC = "A Firebase shared library (.dll / .so) could not be loaded.\n";

		internal static string DependencyNotFoundErrorMessage
		{
			get
			{
				if (PlatformInformation.IsAndroid)
				{
					return ErrorMessages.DEPENDENCY_NOT_FOUND_ERROR_ANDROID;
				}
				if (PlatformInformation.IsIOS)
				{
					return ErrorMessages.DEPENDENCY_NOT_FOUND_ERROR_IOS;
				}
				return ErrorMessages.DEPENDENCY_NOT_FOUND_ERROR_GENERIC;
			}
		}

		internal static string DllNotFoundExceptionErrorMessage
		{
			get
			{
				string str;
				if (PlatformInformation.IsAndroid)
				{
					str = ErrorMessages.DLL_NOT_FOUND_ERROR_ANDROID;
				}
				else if (PlatformInformation.IsIOS)
				{
					str = ErrorMessages.DLL_NOT_FOUND_ERROR_IOS;
				}
				else
				{
					str = ErrorMessages.DLL_NOT_FOUND_ERROR_GENERIC;
				}
				return str + ErrorMessages.DependencyNotFoundErrorMessage;
			}
		}
	}
}
