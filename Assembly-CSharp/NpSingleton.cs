using System;
using UnityEngine;

public class NpSingleton<T> : MonoBehaviour where T : NpSingleton<T>
{
	private static T instance;

	public static T Instance
	{
		get
		{
			return NpSingleton<T>.GetInstance();
		}
	}

	public void DeleteInstance()
	{
		NpSingleton<T>.instance = (T)((object)null);
		UnityEngine.Object.Destroy(this);
	}

	protected virtual void Constructor()
	{
	}

	private void Awake()
	{
		if (NpSingleton<T>.instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		NpSingleton<T>.GetInstance();
	}

	private static T GetInstance()
	{
		if (NpSingleton<T>.instance != null)
		{
			return NpSingleton<T>.instance;
		}
		NpSingleton<T>.instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
		if (NpSingleton<T>.instance != null)
		{
			NpSingleton<T>.instance.Constructor();
			return NpSingleton<T>.instance;
		}
		GameObject gameObject = new GameObject(typeof(T) + "Singleton");
		NpSingleton<T>.instance = gameObject.AddComponent<T>();
		NpSingleton<T>.instance.Constructor();
		return NpSingleton<T>.instance;
	}

	private void OnDestroy()
	{
		if (this != NpSingleton<T>.instance)
		{
			return;
		}
		NpSingleton<T>.instance = (T)((object)null);
	}
}
