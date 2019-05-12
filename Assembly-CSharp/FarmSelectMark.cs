using System;
using UnityEngine;

public class FarmSelectMark : MonoBehaviour
{
	[SerializeField]
	private Transform selectMark;

	[SerializeField]
	private Vector2 adjustScale;

	public void SetSize(int sizeX, int sizeY, Vector2 gridSize)
	{
		Vector3 localScale = this.selectMark.localScale;
		localScale.x = (float)sizeX * gridSize.x + this.adjustScale.x;
		localScale.y = (float)sizeY * gridSize.y + this.adjustScale.y;
		this.selectMark.localScale = localScale;
	}

	public void SetParent(GameObject parent)
	{
		base.transform.parent = parent.transform;
		base.transform.localScale = Vector3.one;
		base.transform.localPosition = Vector3.zero;
		base.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
	}
}
