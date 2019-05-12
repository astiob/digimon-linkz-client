using System;
using UnityEngine;

public class ManualScaling : MonoBehaviour
{
	public bool onScaling;

	public Vector3 scale = Vector3.one;

	public void ManualUpdate()
	{
		base.transform.localScale = this.scale;
	}

	private void Update()
	{
		if (this.onScaling)
		{
			this.ManualUpdate();
		}
	}
}
