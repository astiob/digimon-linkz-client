using System;
using UnityEngine;

public class UIDontDestroy : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
