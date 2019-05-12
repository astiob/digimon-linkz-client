using System;

namespace UnityEngine.Analytics
{
	internal class GameObserver : MonoBehaviour
	{
		internal GameObject m_GameObject;

		private IGameObserverListener m_GameObserverListener;

		private bool m_IsWebPlayerOrEditor;

		public static GameObserver CreateComponent(IPlatformWrapper platformWrapper, IGameObserverListener listener)
		{
			GameObject gameObject = new GameObject("Unity CloudRegistration GameObserver Host")
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			GameObserver gameObserver = gameObject.AddComponent<GameObserver>();
			Object.DontDestroyOnLoad(gameObject);
			gameObserver.m_IsWebPlayerOrEditor = (platformWrapper.isWebPlayer || platformWrapper.isEditor);
			gameObserver.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			gameObserver.m_GameObject = gameObject;
			gameObserver.m_GameObserverListener = listener;
			return gameObserver;
		}

		private void Update()
		{
			if (this.m_IsWebPlayerOrEditor)
			{
				if (Input.GetMouseButtonDown(0))
				{
					this.OnClick();
				}
			}
			else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
			{
				this.OnClick();
			}
		}

		private void OnLevelWasLoaded(int level)
		{
			if (this.m_GameObserverListener != null)
			{
				this.m_GameObserverListener.OnLevelWasLoaded(level);
			}
		}

		private void OnClick()
		{
			if (this.m_GameObserverListener != null)
			{
				this.m_GameObserverListener.OnClick();
			}
		}

		private void OnApplicationPause(bool didPause)
		{
			if (this.m_GameObserverListener != null)
			{
				if (didPause)
				{
					this.m_GameObserverListener.OnAppPause();
				}
				else
				{
					this.m_GameObserverListener.OnAppResume();
				}
			}
		}

		private void OnApplicationQuit()
		{
			if (this.m_GameObserverListener != null)
			{
				this.m_GameObserverListener.OnAppQuit();
			}
		}

		private void OnDestroy()
		{
		}
	}
}
