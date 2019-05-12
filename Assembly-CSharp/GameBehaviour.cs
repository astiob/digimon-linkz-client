using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
	public I GetInterfaceComponent<I>() where I : class
	{
		return base.GetComponent(typeof(I)) as I;
	}

	public static List<I> FindObjectsOfInterface<I>() where I : class
	{
		MonoBehaviour[] array = UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		List<I> list = new List<I>();
		foreach (MonoBehaviour monoBehaviour in array)
		{
			I i2 = monoBehaviour.GetComponent(typeof(I)) as I;
			if (i2 != null)
			{
				list.Add(i2);
			}
		}
		return list;
	}
}
