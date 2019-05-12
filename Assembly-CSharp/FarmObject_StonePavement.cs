using System;
using UnityEngine;

public class FarmObject_StonePavement : FarmObject
{
	[SerializeField]
	private Vector3 movingScale;

	[SerializeField]
	private Vector3 movingPos;

	private Vector3 originalScale;

	private Vector3 originalPosition;

	public override void SetSettingMark(FarmField farmField, FarmSettingMark mark)
	{
		mark.SetParent(base.gameObject);
		Vector2 gridSize = new Vector2(farmField.gridHorizontal, farmField.gridVertical);
		mark.SetSize(this.sizeX, this.sizeY, gridSize);
		if (null != this.floorObject)
		{
			Transform transform = this.floorObject.transform;
			this.originalScale = transform.localScale;
			Vector3 localScale = new Vector3(this.originalScale.x * this.movingScale.x, this.originalScale.y * this.movingScale.y, this.originalScale.z * this.movingScale.z);
			transform.localScale = localScale;
			this.originalPosition = transform.localPosition;
			transform.localPosition = this.movingPos;
		}
	}

	public override void DisplayFloorObject()
	{
		if (null != this.floorObject)
		{
			Transform transform = this.floorObject.transform;
			transform.localScale = this.originalScale;
			transform.localPosition = this.originalPosition;
		}
	}
}
