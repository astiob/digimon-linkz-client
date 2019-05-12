using System;

namespace Facebook.Unity.Mobile.Android
{
	internal interface IAndroidWrapper
	{
		T CallStatic<T>(string methodName);

		void CallStatic(string methodName, params object[] args);
	}
}
