using System;
using UnityEngine;

public class ITweenResumer : MonoBehaviour
{
	private void OnEnable()
	{
		global::Debug.Log("============================= ITweenResumer was enabled");
		iTween component = base.gameObject.GetComponent<iTween>();
		if (component != null)
		{
			iTween.Resume(base.gameObject);
		}
	}
}
