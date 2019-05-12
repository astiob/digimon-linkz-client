using Firebase.Unity;
using System;
using UnityEngine;

namespace Firebase.Platform
{
	internal sealed class FirebaseHandler
	{
		private static FirebaseMonoBehaviour firebaseMonoBehaviour;

		private static FirebaseHandler firebaseHandler;

		private FirebaseHandler()
		{
			if (Application.isEditor)
			{
				this.IsPlayMode = FirebaseEditorDispatcher.EditorIsPlaying;
				FirebaseEditorDispatcher.ListenToPlayState(true);
			}
			else
			{
				this.IsPlayMode = true;
			}
			if (this.IsPlayMode)
			{
				this.StartMonoBehaviour();
			}
			else
			{
				FirebaseEditorDispatcher.StartEditorUpdate();
			}
		}

		public static IFirebaseAppUtils AppUtils { get; private set; } = FirebaseAppUtilsStub.Instance;

		private static Dispatcher ThreadDispatcher { get; set; }

		public bool IsPlayMode { get; set; }

		internal void StartMonoBehaviour()
		{
			if (FirebaseHandler.firebaseHandler == null)
			{
				FirebaseHandler.firebaseHandler = this;
			}
			GameObject gameObject = new GameObject("Firebase Services");
			FirebaseHandler.firebaseMonoBehaviour = gameObject.AddComponent<FirebaseMonoBehaviour>();
			UnitySynchronizationContext.Create(gameObject);
			Object.DontDestroyOnLoad(gameObject);
		}

		internal void StopMonoBehaviour()
		{
			if (FirebaseHandler.firebaseMonoBehaviour != null)
			{
				FirebaseHandler.RunOnMainThread<bool>(delegate
				{
					if (FirebaseHandler.firebaseMonoBehaviour != null)
					{
						UnitySynchronizationContext.Destroy();
						Object.Destroy(FirebaseHandler.firebaseMonoBehaviour.gameObject);
					}
					return true;
				});
			}
		}

		public static TResult RunOnMainThread<TResult>(Func<TResult> f)
		{
			if (FirebaseHandler.ThreadDispatcher != null)
			{
				return FirebaseHandler.ThreadDispatcher.Run<TResult>(f);
			}
			return f();
		}

		internal bool IsMainThread()
		{
			return FirebaseHandler.ThreadDispatcher != null && FirebaseHandler.ThreadDispatcher.ManagesThisThread();
		}

		internal event EventHandler<EventArgs> Updated;

		internal event EventHandler<FirebaseHandler.ApplicationFocusChangedEventArgs> ApplicationFocusChanged;

		internal static FirebaseHandler DefaultInstance
		{
			get
			{
				return FirebaseHandler.firebaseHandler;
			}
		}

		internal static void CreatePartialOnMainThread(IFirebaseAppUtils appUtils)
		{
			appUtils.TranslateDllNotFoundException(delegate
			{
				object typeFromHandle = typeof(FirebaseHandler);
				lock (typeFromHandle)
				{
					if (FirebaseHandler.firebaseHandler == null)
					{
						FirebaseHandler.AppUtils = appUtils;
						if (FirebaseHandler.ThreadDispatcher == null)
						{
							FirebaseHandler.ThreadDispatcher = new Dispatcher();
						}
						FirebaseHandler.firebaseHandler = new FirebaseHandler();
					}
				}
			});
		}

		internal static void Create(IFirebaseAppUtils appUtils)
		{
			FirebaseHandler.CreatePartialOnMainThread(appUtils);
			UnityPlatformServices.SetupServices();
		}

		internal void Update()
		{
			FirebaseHandler.ThreadDispatcher.PollJobs();
			FirebaseHandler.AppUtils.PollCallbacks();
			if (this.Updated != null)
			{
				this.Updated(this, null);
			}
		}

		internal void OnApplicationFocus(bool hasFocus)
		{
			if (this.ApplicationFocusChanged != null)
			{
				this.ApplicationFocusChanged(null, new FirebaseHandler.ApplicationFocusChangedEventArgs
				{
					HasFocus = hasFocus
				});
			}
		}

		internal static void Terminate()
		{
			if (FirebaseHandler.firebaseHandler != null)
			{
				FirebaseEditorDispatcher.Terminate(FirebaseHandler.firebaseHandler.IsPlayMode);
				FirebaseHandler.firebaseHandler.StopMonoBehaviour();
			}
			FirebaseHandler.firebaseHandler = null;
		}

		internal static void OnMonoBehaviourDestroyed(FirebaseMonoBehaviour behaviour)
		{
			if (behaviour == FirebaseHandler.firebaseMonoBehaviour)
			{
				FirebaseHandler.firebaseMonoBehaviour = null;
			}
		}

		internal class ApplicationFocusChangedEventArgs : EventArgs
		{
			public bool HasFocus { get; set; }
		}
	}
}
