using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Translate Object")]
public class TranslateObject : MonoBehaviour
{
	[SerializeField]
	private BillboardObject.LookVector _lookVector = BillboardObject.LookVector.Z;

	[SerializeField]
	private float _localDistance;

	[SerializeField]
	private bool _onInverseMove;

	[SerializeField]
	private Transform _followingWorldPosition;

	[SerializeField]
	private Vector3 _differenceLocalPosition = Vector3.zero;

	[SerializeField]
	private bool _onManualLocalEulerAngles;

	[SerializeField]
	private Vector3 _manualLocalEulerAngles = Vector3.zero;

	[SerializeField]
	private float _worldDistance;

	[SerializeField]
	private bool _onUpdate = true;

	[NonSerialized]
	private Vector3? cachedPosition;

	public float worldDistance
	{
		get
		{
			return this._worldDistance;
		}
		set
		{
			this._worldDistance = value;
		}
	}

	public bool onUpdate
	{
		get
		{
			return this._onUpdate;
		}
		set
		{
			this._onUpdate = value;
		}
	}

	public void ManualUpdate()
	{
		Vector3? vector = this.cachedPosition;
		if (vector == null)
		{
			this.cachedPosition = new Vector3?(base.transform.position);
		}
		if (this._followingWorldPosition != null)
		{
			base.transform.position = this._followingWorldPosition.position;
		}
		else
		{
			Transform transform = base.transform;
			Vector3? vector2 = this.cachedPosition;
			transform.position = vector2.Value;
		}
		float num = 1f;
		if (this._onInverseMove)
		{
			num = -1f;
		}
		Vector3 localPosition = base.transform.localPosition;
		base.transform.Translate(Vector3.forward * this._worldDistance, Space.World);
		float num2 = Vector3.Distance(base.transform.localPosition, localPosition);
		base.transform.localPosition = localPosition;
		base.transform.Translate(this.GetInitializeRotation(this._lookVector) * (num2 * num));
		base.transform.Translate(this._differenceLocalPosition);
		switch (this._lookVector)
		{
		case BillboardObject.LookVector.X:
			base.transform.Translate(new Vector3(this._localDistance * num, 0f, 0f));
			break;
		case BillboardObject.LookVector.Y:
			base.transform.Translate(new Vector3(0f, this._localDistance * num, 0f));
			break;
		case BillboardObject.LookVector.Z:
			base.transform.Translate(new Vector3(0f, 0f, this._localDistance * num));
			break;
		}
		if (this._onManualLocalEulerAngles)
		{
			base.transform.localRotation = Quaternion.Euler(this._manualLocalEulerAngles);
		}
	}

	private Vector3 GetInitializeRotation(BillboardObject.LookVector lookVector)
	{
		if (lookVector == BillboardObject.LookVector.X)
		{
			return Vector3.up;
		}
		if (lookVector != BillboardObject.LookVector.Y)
		{
			return Vector3.forward;
		}
		return Vector3.right;
	}

	private void LateUpdate()
	{
		if (this._onUpdate)
		{
			this.ManualUpdate();
		}
	}
}
