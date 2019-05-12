using System;
using UnityEngine;

namespace Facebook.Unity.Canvas
{
	internal class CanvasJSWrapper : ICanvasJSWrapper
	{
		public string GetSDKVersion()
		{
			return "v2.6";
		}

		public void ExternalCall(string functionName, params object[] args)
		{
			Application.ExternalCall(functionName, args);
		}

		public void ExternalEval(string script)
		{
			Application.ExternalEval(script);
		}

		public void DisableFullScreen()
		{
			if (Screen.fullScreen)
			{
				Screen.fullScreen = false;
			}
		}
	}
}
