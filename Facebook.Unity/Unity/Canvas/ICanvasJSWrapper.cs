using System;
using System.Collections.Generic;

namespace Facebook.Unity.Canvas
{
	internal interface ICanvasJSWrapper
	{
		string GetSDKVersion();

		void DisableFullScreen();

		void Init(string connectFacebookUrl, string locale, int debug, string initParams, int status);

		void Login(IEnumerable<string> scope, string callback_id);

		void Logout();

		void ActivateApp();

		void LogAppEvent(string eventName, float? valueToSum, string parameters);

		void LogPurchase(float purchaseAmount, string currency, string parameters);

		void Ui(string x, string uid, string callbackMethodName);

		void InitScreenPosition();
	}
}
