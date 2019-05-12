using System;
using UnityEngine;

public class ScreenToWorld : MonoBehaviour
{
	[Header("固定する画面のスミの定義")]
	[SerializeField]
	private ScreenToWorld.CONSTRAINT_TYPE type;

	[SerializeField]
	[Header("オフセット(全て＋の値)")]
	private Vector2 offset;

	[Header("カメラ")]
	[SerializeField]
	private Camera cam;

	private Vector3 v3 = new Vector3(0f, 0f, 1f);

	private void Awake()
	{
		this.AdjustObject();
	}

	private void Update()
	{
		this.AdjustObject();
	}

	private void AdjustObject()
	{
		this.v3.x = (float)Screen.width;
		this.v3.y = (float)Screen.height;
		switch (this.type)
		{
		case ScreenToWorld.CONSTRAINT_TYPE.UP_RIGHT:
			this.v3.x = this.v3.x - this.offset.x;
			this.v3.y = this.v3.y - this.offset.y;
			break;
		case ScreenToWorld.CONSTRAINT_TYPE.UP_LEFT:
			this.v3.x = 0f;
			this.v3.x = this.v3.x + this.offset.x;
			this.v3.y = this.v3.y - this.offset.y;
			break;
		case ScreenToWorld.CONSTRAINT_TYPE.DW_RIGHT:
			this.v3.y = 0f;
			this.v3.x = this.v3.x - this.offset.x;
			this.v3.y = this.v3.y + this.offset.y;
			break;
		case ScreenToWorld.CONSTRAINT_TYPE.DW_LEFT:
			this.v3.x = 0f;
			this.v3.y = 0f;
			this.v3.x = this.v3.x + this.offset.x;
			this.v3.y = this.v3.y + this.offset.y;
			break;
		}
		Vector3 position = this.cam.ScreenToWorldPoint(this.v3);
		position.z = base.gameObject.transform.localPosition.z;
		base.gameObject.transform.position = position;
	}

	private enum CONSTRAINT_TYPE
	{
		UP_RIGHT,
		UP_LEFT,
		DW_RIGHT,
		DW_LEFT
	}
}
