using System;

namespace UnityEngine.Analytics
{
	internal interface IGameObserverListener
	{
		void OnAppPause();

		void OnAppResume();

		void OnAppQuit();

		void OnClick();

		void OnKey();

		void OnLevelWasLoaded(int level);
	}
}
