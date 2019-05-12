using System;

namespace Facebook.Unity.Canvas
{
	internal interface ICanvasJSWrapper
	{
		string GetSDKVersion();

		void ExternalCall(string functionName, params object[] args);

		void DisableFullScreen();

		void ExternalEval(string script);
	}
}
