using System;
using UnityEngine;

public class AdjustWrapper : MonoBehaviour
{
	private static GameObject go;

	private static AdjustWrapper instance;

	public static AdjustWrapper Instance
	{
		get
		{
			if (AdjustWrapper.instance == null)
			{
				AdjustWrapper.go = new GameObject("AdjustWrapper");
				UnityEngine.Object.DontDestroyOnLoad(AdjustWrapper.go);
				AdjustWrapper.instance = AdjustWrapper.go.AddComponent<AdjustWrapper>();
			}
			return AdjustWrapper.instance;
		}
	}

	public void StartAdjust()
	{
	}

	public void TrackEvent(string eventToken)
	{
	}
}
