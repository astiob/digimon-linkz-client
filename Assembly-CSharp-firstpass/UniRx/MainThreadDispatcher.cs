using System;
using System.Collections;
using UniRx.InternalUtil;
using UnityEngine;

namespace UniRx
{
	public sealed class MainThreadDispatcher : MonoBehaviour
	{
		public static MainThreadDispatcher.CullingMode cullingMode = MainThreadDispatcher.CullingMode.Self;

		private ThreadSafeQueueWorker queueWorker = new ThreadSafeQueueWorker();

		private Action<Exception> unhandledExceptionCallback = delegate(Exception ex)
		{
			global::Debug.LogException(ex);
		};

		private MicroCoroutine updateMicroCoroutine;

		private MicroCoroutine fixedUpdateMicroCoroutine;

		private MicroCoroutine endOfFrameMicroCoroutine;

		private static MainThreadDispatcher instance;

		private static bool initialized;

		private static bool isQuitting;

		[ThreadStatic]
		private static object mainThreadToken;

		private Subject<Unit> update;

		private Subject<Unit> lateUpdate;

		private Subject<bool> onApplicationFocus;

		private Subject<bool> onApplicationPause;

		private Subject<Unit> onApplicationQuit;

		public static void Post(Action<object> action, object state)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (!MainThreadDispatcher.isQuitting && !object.ReferenceEquals(mainThreadDispatcher, null))
			{
				mainThreadDispatcher.queueWorker.Enqueue(action, state);
			}
		}

		public static void Send(Action<object> action, object state)
		{
			if (MainThreadDispatcher.mainThreadToken != null)
			{
				try
				{
					action(state);
				}
				catch (Exception obj)
				{
					MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
					if (mainThreadDispatcher != null)
					{
						mainThreadDispatcher.unhandledExceptionCallback(obj);
					}
				}
			}
			else
			{
				MainThreadDispatcher.Post(action, state);
			}
		}

		public static void UnsafeSend(Action action)
		{
			try
			{
				action();
			}
			catch (Exception obj)
			{
				MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
				if (mainThreadDispatcher != null)
				{
					mainThreadDispatcher.unhandledExceptionCallback(obj);
				}
			}
		}

		public static void UnsafeSend<T>(Action<T> action, T state)
		{
			try
			{
				action(state);
			}
			catch (Exception obj)
			{
				MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
				if (mainThreadDispatcher != null)
				{
					mainThreadDispatcher.unhandledExceptionCallback(obj);
				}
			}
		}

		public static void SendStartCoroutine(IEnumerator routine)
		{
			if (MainThreadDispatcher.mainThreadToken != null)
			{
				MainThreadDispatcher.StartCoroutine(routine);
			}
			else
			{
				MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
				if (!MainThreadDispatcher.isQuitting && !object.ReferenceEquals(mainThreadDispatcher, null))
				{
					mainThreadDispatcher.queueWorker.Enqueue(delegate(object _)
					{
						MainThreadDispatcher mainThreadDispatcher2 = MainThreadDispatcher.Instance;
						if (mainThreadDispatcher2 != null)
						{
							mainThreadDispatcher2.StartCoroutine(routine);
						}
					}, null);
				}
			}
		}

		public static void StartUpdateMicroCoroutine(IEnumerator routine)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (mainThreadDispatcher != null)
			{
				mainThreadDispatcher.updateMicroCoroutine.AddCoroutine(routine);
			}
		}

		public static void StartFixedUpdateMicroCoroutine(IEnumerator routine)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (mainThreadDispatcher != null)
			{
				mainThreadDispatcher.fixedUpdateMicroCoroutine.AddCoroutine(routine);
			}
		}

		public static void StartEndOfFrameMicroCoroutine(IEnumerator routine)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (mainThreadDispatcher != null)
			{
				mainThreadDispatcher.endOfFrameMicroCoroutine.AddCoroutine(routine);
			}
		}

		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			MainThreadDispatcher mainThreadDispatcher = MainThreadDispatcher.Instance;
			if (mainThreadDispatcher != null)
			{
				return mainThreadDispatcher.StartCoroutine(routine);
			}
			return null;
		}

		public static void RegisterUnhandledExceptionCallback(Action<Exception> exceptionCallback)
		{
			if (exceptionCallback == null)
			{
				MainThreadDispatcher.Instance.unhandledExceptionCallback = Stubs<Exception>.Ignore;
			}
			else
			{
				MainThreadDispatcher.Instance.unhandledExceptionCallback = exceptionCallback;
			}
		}

		public static string InstanceName
		{
			get
			{
				if (MainThreadDispatcher.instance == null)
				{
					throw new NullReferenceException("MainThreadDispatcher is not initialized.");
				}
				return MainThreadDispatcher.instance.name;
			}
		}

		public static bool IsInitialized
		{
			get
			{
				return MainThreadDispatcher.initialized && MainThreadDispatcher.instance != null;
			}
		}

		private static MainThreadDispatcher Instance
		{
			get
			{
				MainThreadDispatcher.Initialize();
				return MainThreadDispatcher.instance;
			}
		}

		public static void Initialize()
		{
			if (!MainThreadDispatcher.initialized)
			{
				MainThreadDispatcher mainThreadDispatcher = null;
				try
				{
					mainThreadDispatcher = UnityEngine.Object.FindObjectOfType<MainThreadDispatcher>();
				}
				catch
				{
					Exception ex = new Exception("UniRx requires a MainThreadDispatcher component created on the main thread. Make sure it is added to the scene before calling UniRx from a worker thread.");
					UnityEngine.Debug.LogException(ex);
					throw ex;
				}
				if (MainThreadDispatcher.isQuitting)
				{
					return;
				}
				if (mainThreadDispatcher == null)
				{
					new GameObject("MainThreadDispatcher").AddComponent<MainThreadDispatcher>();
				}
				else
				{
					mainThreadDispatcher.Awake();
				}
			}
		}

		public static bool IsInMainThread
		{
			get
			{
				return MainThreadDispatcher.mainThreadToken != null;
			}
		}

		private void Awake()
		{
			if (MainThreadDispatcher.instance == null)
			{
				MainThreadDispatcher.instance = this;
				MainThreadDispatcher.mainThreadToken = new object();
				MainThreadDispatcher.initialized = true;
				this.updateMicroCoroutine = new MicroCoroutine(delegate(Exception ex)
				{
					this.unhandledExceptionCallback(ex);
				});
				this.fixedUpdateMicroCoroutine = new MicroCoroutine(delegate(Exception ex)
				{
					this.unhandledExceptionCallback(ex);
				});
				this.endOfFrameMicroCoroutine = new MicroCoroutine(delegate(Exception ex)
				{
					this.unhandledExceptionCallback(ex);
				});
				MainThreadDispatcher.StartCoroutine(this.RunUpdateMicroCoroutine());
				MainThreadDispatcher.StartCoroutine(this.RunFixedUpdateMicroCoroutine());
				MainThreadDispatcher.StartCoroutine(this.RunEndOfFrameMicroCoroutine());
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else if (this != MainThreadDispatcher.instance)
			{
				if (MainThreadDispatcher.cullingMode == MainThreadDispatcher.CullingMode.Self)
				{
					global::Debug.LogWarning("There is already a MainThreadDispatcher in the scene. Removing myself...");
					MainThreadDispatcher.DestroyDispatcher(this);
				}
				else if (MainThreadDispatcher.cullingMode == MainThreadDispatcher.CullingMode.All)
				{
					global::Debug.LogWarning("There is already a MainThreadDispatcher in the scene. Cleaning up all excess dispatchers...");
					MainThreadDispatcher.CullAllExcessDispatchers();
				}
				else
				{
					global::Debug.LogWarning("There is already a MainThreadDispatcher in the scene.");
				}
			}
		}

		private IEnumerator RunUpdateMicroCoroutine()
		{
			for (;;)
			{
				yield return null;
				this.updateMicroCoroutine.Run();
			}
			yield break;
		}

		private IEnumerator RunFixedUpdateMicroCoroutine()
		{
			for (;;)
			{
				yield return YieldInstructionCache.WaitForFixedUpdate;
				this.fixedUpdateMicroCoroutine.Run();
			}
			yield break;
		}

		private IEnumerator RunEndOfFrameMicroCoroutine()
		{
			for (;;)
			{
				yield return YieldInstructionCache.WaitForEndOfFrame;
				this.endOfFrameMicroCoroutine.Run();
			}
			yield break;
		}

		private static void DestroyDispatcher(MainThreadDispatcher aDispatcher)
		{
			if (aDispatcher != MainThreadDispatcher.instance)
			{
				Component[] components = aDispatcher.gameObject.GetComponents<Component>();
				if (aDispatcher.gameObject.transform.childCount == 0 && components.Length == 2)
				{
					if (components[0] is Transform && components[1] is MainThreadDispatcher)
					{
						UnityEngine.Object.Destroy(aDispatcher.gameObject);
					}
				}
				else
				{
					UnityEngine.Object.Destroy(aDispatcher);
				}
			}
		}

		public static void CullAllExcessDispatchers()
		{
			MainThreadDispatcher[] array = UnityEngine.Object.FindObjectsOfType<MainThreadDispatcher>();
			for (int i = 0; i < array.Length; i++)
			{
				MainThreadDispatcher.DestroyDispatcher(array[i]);
			}
		}

		private void OnDestroy()
		{
			if (MainThreadDispatcher.instance == this)
			{
				MainThreadDispatcher.instance = UnityEngine.Object.FindObjectOfType<MainThreadDispatcher>();
				MainThreadDispatcher.initialized = (MainThreadDispatcher.instance != null);
			}
		}

		private void Update()
		{
			if (this.update != null)
			{
				try
				{
					this.update.OnNext(Unit.Default);
				}
				catch (Exception obj)
				{
					this.unhandledExceptionCallback(obj);
				}
			}
			this.queueWorker.ExecuteAll(this.unhandledExceptionCallback);
		}

		public static IObservable<Unit> UpdateAsObservable()
		{
			Subject<Unit> result;
			if ((result = MainThreadDispatcher.Instance.update) == null)
			{
				result = (MainThreadDispatcher.Instance.update = new Subject<Unit>());
			}
			return result;
		}

		private void LateUpdate()
		{
			if (this.lateUpdate != null)
			{
				this.lateUpdate.OnNext(Unit.Default);
			}
		}

		public static IObservable<Unit> LateUpdateAsObservable()
		{
			Subject<Unit> result;
			if ((result = MainThreadDispatcher.Instance.lateUpdate) == null)
			{
				result = (MainThreadDispatcher.Instance.lateUpdate = new Subject<Unit>());
			}
			return result;
		}

		private void OnApplicationFocus(bool focus)
		{
			if (this.onApplicationFocus != null)
			{
				this.onApplicationFocus.OnNext(focus);
			}
		}

		public static IObservable<bool> OnApplicationFocusAsObservable()
		{
			Subject<bool> result;
			if ((result = MainThreadDispatcher.Instance.onApplicationFocus) == null)
			{
				result = (MainThreadDispatcher.Instance.onApplicationFocus = new Subject<bool>());
			}
			return result;
		}

		private void OnApplicationPause(bool pause)
		{
			if (this.onApplicationPause != null)
			{
				this.onApplicationPause.OnNext(pause);
			}
		}

		public static IObservable<bool> OnApplicationPauseAsObservable()
		{
			Subject<bool> result;
			if ((result = MainThreadDispatcher.Instance.onApplicationPause) == null)
			{
				result = (MainThreadDispatcher.Instance.onApplicationPause = new Subject<bool>());
			}
			return result;
		}

		private void OnApplicationQuit()
		{
			MainThreadDispatcher.isQuitting = true;
			if (this.onApplicationQuit != null)
			{
				this.onApplicationQuit.OnNext(Unit.Default);
			}
		}

		public static IObservable<Unit> OnApplicationQuitAsObservable()
		{
			Subject<Unit> result;
			if ((result = MainThreadDispatcher.Instance.onApplicationQuit) == null)
			{
				result = (MainThreadDispatcher.Instance.onApplicationQuit = new Subject<Unit>());
			}
			return result;
		}

		public enum CullingMode
		{
			Disabled,
			Self,
			All
		}
	}
}
