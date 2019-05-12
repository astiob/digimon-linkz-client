using System;
using System.Collections;
using UnityEngine;

public class AppCoroutine : MonoBehaviour
{
	private static AppCoroutine scene;

	private static AppCoroutine resident;

	private static bool isApplicationQuit;

	private void OnApplicationQuit()
	{
		AppCoroutine.isApplicationQuit = true;
	}

	public static Coroutine Start(IEnumerator routine, bool isResident = false)
	{
		if (!AppThread.isMainThread)
		{
			string message = "It is not possible to produce a co-routine from non-main thread.\n";
			global::Debug.LogError(message);
			throw new Exception(message);
		}
		if (AppCoroutine.isApplicationQuit)
		{
			return null;
		}
		if (isResident)
		{
			if (AppCoroutine.resident == null)
			{
				GameObject gameObject = new GameObject("AppCoroutine.resident");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				AppCoroutine.resident = gameObject.AddComponent<AppCoroutine>();
			}
			return AppCoroutine.resident.StartCoroutine(routine);
		}
		if (AppCoroutine.scene == null)
		{
			GameObject gameObject2 = new GameObject("AppCoroutine.scene");
			AppCoroutine.scene = gameObject2.AddComponent<AppCoroutine>();
		}
		return AppCoroutine.scene.StartCoroutine(routine);
	}

	public static void Stop(IEnumerator routine, bool isResident = false)
	{
		if (routine == null)
		{
			return;
		}
		if (!AppThread.isMainThread)
		{
			string message = "It is not possible to stop the coroutine from other than the main thread.\n";
			global::Debug.LogError(message);
			throw new Exception(message);
		}
		if (AppCoroutine.isApplicationQuit)
		{
			return;
		}
		if (isResident)
		{
			if (AppCoroutine.resident == null)
			{
				return;
			}
			AppCoroutine.resident.StopCoroutine(routine);
		}
		else
		{
			if (AppCoroutine.scene == null)
			{
				return;
			}
			AppCoroutine.scene.StopCoroutine(routine);
		}
	}

	public static void Stop(Coroutine routine, bool isResident = false)
	{
		if (routine == null)
		{
			return;
		}
		if (!AppThread.isMainThread)
		{
			string message = "It is not possible to stop the coroutine from other than the main thread.\n";
			global::Debug.LogError(message);
			throw new Exception(message);
		}
		if (AppCoroutine.isApplicationQuit)
		{
			return;
		}
		if (isResident)
		{
			if (AppCoroutine.resident == null)
			{
				return;
			}
			AppCoroutine.resident.StopCoroutine(routine);
		}
		else
		{
			if (AppCoroutine.scene == null)
			{
				return;
			}
			AppCoroutine.scene.StopCoroutine(routine);
		}
	}

	public static void Start(IEnumerator routine, System.Action onFinish, bool isResident = false)
	{
		AppCoroutine appCoroutine;
		if (isResident)
		{
			if (AppCoroutine.resident == null)
			{
				GameObject gameObject = new GameObject("AppCoroutine.resident");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				AppCoroutine.resident = gameObject.AddComponent<AppCoroutine>();
			}
			appCoroutine = AppCoroutine.resident;
		}
		else
		{
			if (AppCoroutine.scene == null)
			{
				GameObject gameObject2 = new GameObject("AppCoroutine.scene");
				AppCoroutine.scene = gameObject2.AddComponent<AppCoroutine>();
			}
			appCoroutine = AppCoroutine.scene;
		}
		appCoroutine.StartCoroutine(AppCoroutine.StartEnumerator(routine, onFinish, isResident));
	}

	private static IEnumerator StartEnumerator(IEnumerator routine, System.Action onFinish, bool isResident = false)
	{
		yield return AppCoroutine.Start(routine, isResident);
		if (onFinish != null)
		{
			onFinish();
		}
		yield break;
	}

	public delegate IEnumerator Action();

	public delegate IEnumerator Action<T1>(T1 t1);

	public delegate IEnumerator Action<T1, T2>(T1 t1, T2 t2);

	public delegate IEnumerator Action<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

	public delegate IEnumerator Action<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

	public delegate IEnumerator Action<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
}
