using System;
using UnityEngine;

[AddComponentMenu("GUI/BillBoardController")]
public class BillBoardController : MonoBehaviour
{
	[Header("必要ならカメラ設定")]
	[SerializeField]
	private Camera cam;

	[Header("解決する軸タイプ + オフセット回転してカメラへ向ける")]
	[SerializeField]
	private BillBoardController.AXIS_TYPE type = BillBoardController.AXIS_TYPE.Y;

	[Header("オフセット回転量 (0 90 -90 180 など)")]
	[SerializeField]
	private float offset;

	[SerializeField]
	private bool useLateUpdate = true;

	public void SetUp(Camera c)
	{
		this.cam = c;
	}

	private void LateUpdate()
	{
		if (this.useLateUpdate)
		{
			this.UpdateBillBoard();
		}
	}

	private void OnWillRenderObject()
	{
		if (!this.useLateUpdate)
		{
			this.UpdateBillBoard();
		}
	}

	private void UpdateBillBoard()
	{
		if (this.cam != null)
		{
			if (this.type == BillBoardController.AXIS_TYPE.OFSZ_AND_CAMERA || this.type == BillBoardController.AXIS_TYPE.OFSX_AND_CAMERA || this.type == BillBoardController.AXIS_TYPE.OFSY_AND_CAMERA)
			{
				Vector3 zero = Vector3.zero;
				switch (this.type)
				{
				case BillBoardController.AXIS_TYPE.OFSZ_AND_CAMERA:
					zero.z = this.offset;
					break;
				case BillBoardController.AXIS_TYPE.OFSX_AND_CAMERA:
					zero.x = this.offset;
					break;
				case BillBoardController.AXIS_TYPE.OFSY_AND_CAMERA:
					zero.y = this.offset;
					break;
				}
				Quaternion rhs = Quaternion.Euler(zero.x, zero.y, zero.z);
				Quaternion rotation = this.cam.transform.rotation * rhs;
				base.transform.rotation = rotation;
				return;
			}
			if (this.type == BillBoardController.AXIS_TYPE.OFSZ_AND_TO_CAMERA || this.type == BillBoardController.AXIS_TYPE.OFSX_AND_TO_CAMERA || this.type == BillBoardController.AXIS_TYPE.OFSY_AND_TO_CAMERA)
			{
				Vector3 zero2 = Vector3.zero;
				switch (this.type)
				{
				case BillBoardController.AXIS_TYPE.OFSZ_AND_TO_CAMERA:
					zero2.z = this.offset;
					break;
				case BillBoardController.AXIS_TYPE.OFSX_AND_TO_CAMERA:
					zero2.x = this.offset;
					break;
				case BillBoardController.AXIS_TYPE.OFSY_AND_TO_CAMERA:
					zero2.y = this.offset;
					break;
				}
				Quaternion rhs2 = Quaternion.Euler(zero2.x, zero2.y, zero2.z);
				Vector3 forward = this.cam.transform.position - base.transform.position;
				Quaternion lhs = Quaternion.LookRotation(forward);
				Quaternion rotation2 = lhs * rhs2;
				base.transform.rotation = rotation2;
				return;
			}
			Vector3 point = this.cam.transform.position - base.transform.position;
			Quaternion rotation3 = base.transform.rotation;
			Quaternion rotation4 = Quaternion.Inverse(rotation3);
			Vector3 vector = rotation4 * point;
			Vector3 vector2 = Vector3.zero;
			switch (this.type)
			{
			case BillBoardController.AXIS_TYPE.Z:
				vector2.x = vector.x;
				vector2.y = vector.y;
				break;
			case BillBoardController.AXIS_TYPE.X:
				vector2.x = vector.y;
				vector2.y = vector.z;
				break;
			case BillBoardController.AXIS_TYPE.Y:
				vector2.x = vector.x;
				vector2.y = vector.z;
				break;
			}
			vector2 = vector2.normalized;
			float num = Mathf.Asin(vector2.y);
			if (vector2.x < 0f)
			{
				num = 3.14159274f - num;
			}
			num *= 57.29578f;
			Vector3 zero3 = Vector3.zero;
			switch (this.type)
			{
			case BillBoardController.AXIS_TYPE.Z:
				zero3.z = num + this.offset;
				break;
			case BillBoardController.AXIS_TYPE.X:
				zero3.x = num + this.offset;
				break;
			case BillBoardController.AXIS_TYPE.Y:
				zero3.y = -(num + this.offset);
				break;
			}
			Quaternion rhs3 = Quaternion.Euler(zero3.x, zero3.y, zero3.z);
			Quaternion rotation5 = rotation3 * rhs3;
			base.transform.rotation = rotation5;
		}
	}

	public enum AXIS_TYPE
	{
		Z,
		X,
		Y,
		OFSZ_AND_CAMERA,
		OFSX_AND_CAMERA,
		OFSY_AND_CAMERA,
		OFSZ_AND_TO_CAMERA,
		OFSX_AND_TO_CAMERA,
		OFSY_AND_TO_CAMERA
	}
}
