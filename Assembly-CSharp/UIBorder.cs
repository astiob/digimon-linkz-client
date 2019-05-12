using System;
using UnityEngine;

public class UIBorder : MonoBehaviour
{
	public float value = 1f;

	private float yVL;

	private float yCL;

	private void Start()
	{
		UIWidget component = base.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			this.yVL = (float)component.height;
		}
		GUICollider component2 = base.gameObject.GetComponent<GUICollider>();
		if (component2 != null)
		{
			this.yCL = component2.height;
		}
		this.AdjustBorder();
	}

	private void Update()
	{
		this.AdjustBorder();
	}

	private void AdjustBorder()
	{
		UIWidget component = base.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.height = (int)Mathf.Round(this.yVL + GUIMain.VerticalSpaceSize * this.value * 2f);
		}
		BoxCollider component2 = base.gameObject.GetComponent<BoxCollider>();
		if (component2 != null)
		{
			Vector3 size = component2.size;
			size.y = this.yCL + GUIMain.VerticalSpaceSize * this.value * 2f;
			component2.size = size;
		}
	}
}
