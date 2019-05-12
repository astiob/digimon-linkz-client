using System;
using UnityEngine;

public class UIHeader : MonoBehaviour
{
	public float value = 1f;

	public float yVL;

	private void Start()
	{
		this.yVL = base.gameObject.transform.localPosition.y;
		this.Adjust();
	}

	private void Update()
	{
		this.Adjust();
	}

	private void Adjust()
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.y = this.yVL + GUIMain.VerticalSpaceSize * this.value;
		base.gameObject.transform.localPosition = localPosition;
	}
}
