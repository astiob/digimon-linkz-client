using System;
using UnityEngine;

public class Singleton<T> : GameBehaviour where T : Singleton<T>
{
	protected static T instance;

	public static T Instance
	{
		get
		{
			if (Singleton<T>.instance == null)
			{
				Singleton<T>.instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
				if (Singleton<T>.instance == null)
				{
					global::Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
				}
			}
			return Singleton<T>.instance;
		}
	}

	public static bool IsInstance()
	{
		return Singleton<T>.instance != null;
	}
}
