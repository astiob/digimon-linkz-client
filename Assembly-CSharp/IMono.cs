using System;
using System.Collections;
using UnityEngine;

public abstract class IMono
{
	protected Coroutine StartCoroutine(IEnumerator routine)
	{
		return AppCoroutine.Start(routine, false);
	}

	protected void StopCoroutine(IEnumerator routine)
	{
		AppCoroutine.Stop(routine, false);
	}

	protected void Destroy(UnityEngine.Object obj)
	{
		UnityEngine.Object.Destroy(obj);
	}

	protected UnityEngine.Object Instantiate(UnityEngine.Object original)
	{
		return UnityEngine.Object.Instantiate(original);
	}

	protected T Instantiate<T>(T original) where T : UnityEngine.Object
	{
		return UnityEngine.Object.Instantiate<T>(original);
	}

	protected UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation)
	{
		return UnityEngine.Object.Instantiate(original, position, rotation);
	}

	public static bool operator false(IMono a)
	{
		return a == null;
	}

	public static bool operator true(IMono a)
	{
		return a != null;
	}
}
