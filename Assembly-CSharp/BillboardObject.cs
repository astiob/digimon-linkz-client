using System;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Digimon Effects/Tools/Billboard Object")]
public class BillboardObject : MonoBehaviour
{
	[NonSerialized]
	private Camera _lookTarget;

	[SerializeField]
	private BillboardObject.LookVector _lookVector = BillboardObject.LookVector.Z;

	[SerializeField]
	private bool _onIgnoreUpdate;

	[FormerlySerializedAs("onIgnoreManage")]
	[SerializeField]
	private bool _onIgnoreManage;

	[SerializeField]
	[FormerlySerializedAs("onIgnoreManage")]
	private bool _onIgnoreFollowingTransformOverride;

	[SerializeField]
	[FormerlySerializedAs("onManualPosition")]
	private bool _onManualPosition;

	[SerializeField]
	[FormerlySerializedAs("manualPositionTransform")]
	private Transform _manualPositionTransform;

	[FormerlySerializedAs("manualLocalPosition")]
	[SerializeField]
	private Vector3 _manualLocalPosition = Vector3.zero;

	[SerializeField]
	private bool _onUseLocalPositionAnimation;

	[SerializeField]
	private Vector3 _manualDistanceLocalPosition = Vector3.zero;

	[FormerlySerializedAs("_distance")]
	[FormerlySerializedAs("distance")]
	[SerializeField]
	private float _localDistance;

	[SerializeField]
	private float _worldDistance;

	[SerializeField]
	private bool _onInverseDistance;

	[FormerlySerializedAs("onInverse")]
	[SerializeField]
	private bool _onInverse;

	[SerializeField]
	private bool _onUseLocalScaleAnimation;

	[SerializeField]
	[FormerlySerializedAs("onBillboard")]
	private bool _onBillboard;

	[SerializeField]
	private bool _freezeDistanceThisVector;

	[FormerlySerializedAs("xFreeze")]
	[SerializeField]
	private bool _xFreeze;

	[FormerlySerializedAs("yFreeze")]
	[SerializeField]
	private bool _yFreeze;

	[FormerlySerializedAs("zFreeze")]
	[SerializeField]
	private bool _zFreeze;

	private Transform cachedTransform;

	private Vector3? cachedEulerAngles;

	private Vector3? cachedPosition;

	private Vector3? cachedScale;

	private bool isDrawed;

	public Camera lookTarget
	{
		get
		{
			return this._lookTarget;
		}
		set
		{
			this._lookTarget = value;
		}
	}

	public bool onIgnoreManage
	{
		get
		{
			return this._onIgnoreManage;
		}
	}

	public bool onIgnoreFollowingTransformOverride
	{
		get
		{
			return this._onIgnoreFollowingTransformOverride;
		}
	}

	public bool onManualPosition
	{
		get
		{
			return this._onManualPosition;
		}
		set
		{
			this._onManualPosition = value;
		}
	}

	public bool onUseLocalPositionAnimation
	{
		get
		{
			return this._onUseLocalPositionAnimation;
		}
	}

	public Transform manualPositionTransform
	{
		get
		{
			return this._manualPositionTransform;
		}
		set
		{
			this._manualPositionTransform = value;
		}
	}

	public Vector3 manualLocalPosition
	{
		get
		{
			return this._manualLocalPosition;
		}
		set
		{
			this._manualLocalPosition = value;
		}
	}

	public Vector3 manualDistanceLocalPosition
	{
		get
		{
			return this._manualDistanceLocalPosition;
		}
		set
		{
			this._manualDistanceLocalPosition = value;
		}
	}

	public float distance
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

	public float localDistance
	{
		get
		{
			return this._localDistance;
		}
		set
		{
			this._localDistance = value;
		}
	}

	public bool onInverse
	{
		get
		{
			return this._onInverse;
		}
		set
		{
			this._onInverse = value;
		}
	}

	public bool onUseLocalScaleAnimation
	{
		get
		{
			return this._onUseLocalScaleAnimation;
		}
	}

	public bool onBillboard
	{
		get
		{
			return this._onBillboard;
		}
		set
		{
			this._onBillboard = value;
		}
	}

	public bool xFreeze
	{
		get
		{
			return this._xFreeze;
		}
		set
		{
			this._xFreeze = value;
		}
	}

	public bool yFreeze
	{
		get
		{
			return this._yFreeze;
		}
		set
		{
			this._yFreeze = value;
		}
	}

	public bool zFreeze
	{
		get
		{
			return this._zFreeze;
		}
		set
		{
			this._zFreeze = value;
		}
	}

	private void Update()
	{
		this.isDrawed = false;
	}

	public void ManualUpdate()
	{
		if (this._onIgnoreManage && this.isDrawed)
		{
			return;
		}
		this.ManualUpdateInternal();
	}

	private void ManualUpdateInternal()
	{
		if (this._onIgnoreUpdate)
		{
			return;
		}
		if (this.lookTarget == null)
		{
			return;
		}
		this.cachedTransform = (this.cachedTransform ?? base.transform);
		if (this.cachedPosition == null)
		{
			this.cachedPosition = new Vector3?(this.cachedTransform.localPosition);
		}
		if (this.onManualPosition && !this._onUseLocalPositionAnimation)
		{
			if (this.manualPositionTransform != null)
			{
				base.transform.position = this.manualPositionTransform.position;
			}
			else
			{
				base.transform.localPosition = this.manualLocalPosition;
			}
			this.cachedPosition = new Vector3?(this.cachedTransform.localPosition);
		}
		if (this.cachedEulerAngles == null)
		{
			this.cachedEulerAngles = new Vector3?(this.cachedTransform.localEulerAngles);
		}
		if (this.onManualPosition)
		{
			float num = (float)((!this._onInverseDistance) ? 1 : -1);
			Vector3 position = this.lookTarget.transform.position;
			if (this._freezeDistanceThisVector)
			{
				if (this._lookVector == BillboardObject.LookVector.X)
				{
					position.Set(this.lookTarget.transform.position.x, this.cachedTransform.position.y, this.lookTarget.transform.position.z);
				}
				else if (this._lookVector == BillboardObject.LookVector.Y)
				{
					position.Set(this.lookTarget.transform.position.x, this.lookTarget.transform.position.y, this.cachedTransform.position.z);
				}
				else
				{
					position.Set(this.lookTarget.transform.position.x, this.cachedTransform.position.y, this.lookTarget.transform.position.z);
				}
			}
			this.cachedTransform.position = this.cachedTransform.position + (position - this.cachedTransform.position).normalized * (this._worldDistance * num);
			this.cachedTransform.LookAt(this.lookTarget.transform);
			if (this._lookVector == BillboardObject.LookVector.X)
			{
				this.cachedTransform.Rotate(new Vector3(0f, -90f, 0f));
			}
			if (this._lookVector == BillboardObject.LookVector.Y)
			{
				this.cachedTransform.Rotate(new Vector3(90f, 0f, 0f));
			}
			if (this._freezeDistanceThisVector)
			{
				if (this._lookVector == BillboardObject.LookVector.X)
				{
					position.Set(this.cachedTransform.localEulerAngles.x, this.cachedTransform.localEulerAngles.y, this.cachedEulerAngles.Value.z);
				}
				else if (this._lookVector == BillboardObject.LookVector.Y)
				{
					position.Set(this.cachedEulerAngles.Value.x, this.cachedTransform.localEulerAngles.y, this.cachedTransform.localEulerAngles.z);
				}
				else
				{
					position.Set(this.cachedEulerAngles.Value.x, this.cachedTransform.localEulerAngles.y, this.cachedTransform.localEulerAngles.z);
				}
				this.cachedTransform.localRotation = Quaternion.Euler(position);
			}
			this.cachedTransform.Translate(this.GetLookVectorToPosition(this._lookVector) * (this._localDistance * num), Space.Self);
			this.cachedTransform.localPosition = new Vector3((!this._freezeDistanceThisVector || !this.xFreeze) ? this.cachedTransform.localPosition.x : this.cachedPosition.Value.x, (!this._freezeDistanceThisVector || !this.yFreeze) ? this.cachedTransform.localPosition.y : this.cachedPosition.Value.y, (!this._freezeDistanceThisVector || !this.zFreeze) ? this.cachedTransform.localPosition.z : this.cachedPosition.Value.z);
			this.cachedTransform.Translate(this._manualDistanceLocalPosition, Space.Self);
		}
		this.cachedTransform.LookAt(this.lookTarget.transform);
		BillboardObject.LookVector lookVector = this._lookVector;
		if (lookVector != BillboardObject.LookVector.X)
		{
			if (lookVector == BillboardObject.LookVector.Y)
			{
				this.cachedTransform.Rotate(new Vector3(90f, 0f, 0f));
			}
		}
		else
		{
			this.cachedTransform.Rotate(new Vector3(0f, -90f, 0f));
		}
		Vector3 euler = new Vector3((!this.xFreeze) ? this.cachedTransform.localEulerAngles.x : this.cachedEulerAngles.Value.x, (!this.yFreeze) ? this.cachedTransform.localEulerAngles.y : this.cachedEulerAngles.Value.y, (!this.zFreeze) ? this.cachedTransform.localEulerAngles.z : this.cachedEulerAngles.Value.z);
		this.cachedTransform.localRotation = Quaternion.Euler(euler);
		Vector3? vector = this.cachedScale;
		if (vector == null)
		{
			this.cachedScale = new Vector3?(this.cachedTransform.localScale);
		}
		if (this.onUseLocalScaleAnimation)
		{
			this.cachedScale = new Vector3?(this.cachedTransform.localScale);
		}
		Vector3 lookVectorToScale = this.GetLookVectorToScale(this._lookVector);
		if (this.onInverse)
		{
			Transform transform = this.cachedTransform;
			Vector3? vector2 = this.cachedScale;
			float x = vector2.Value.x * lookVectorToScale.x;
			Vector3? vector3 = this.cachedScale;
			float y = vector3.Value.y * lookVectorToScale.y;
			Vector3? vector4 = this.cachedScale;
			transform.localScale = new Vector3(x, y, vector4.Value.z * lookVectorToScale.z);
		}
		else
		{
			Transform transform2 = this.cachedTransform;
			Vector3? vector5 = this.cachedScale;
			transform2.localScale = vector5.Value;
		}
		this.isDrawed = true;
	}

	private Vector3 GetLookVectorToPosition(BillboardObject.LookVector lookVector)
	{
		if (lookVector == BillboardObject.LookVector.X)
		{
			return new Vector3(1f, 0f, 0f);
		}
		if (lookVector != BillboardObject.LookVector.Y)
		{
			return new Vector3(0f, 0f, 1f);
		}
		return new Vector3(0f, 1f, 0f);
	}

	private Vector3 GetLookVectorToScale(BillboardObject.LookVector lookVector)
	{
		if (lookVector == BillboardObject.LookVector.X)
		{
			return new Vector3(-1f, 1f, 1f);
		}
		if (lookVector != BillboardObject.LookVector.Y)
		{
			return new Vector3(1f, 1f, -1f);
		}
		return new Vector3(1f, -1f, 1f);
	}

	private void OnWillRenderObject()
	{
		bool flag = false;
		if (FollowTargetCamera.IsVisible())
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		if (!this.onBillboard)
		{
			return;
		}
		this.ManualUpdateInternal();
	}

	private void OnDrawGizmos()
	{
		Color color = Gizmos.color;
		Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
		Vector3? vector = this.cachedPosition;
		if (vector != null)
		{
			Gizmos.DrawSphere(this.cachedTransform.TransformPoint(this.cachedPosition.Value), 0.05f);
		}
		Gizmos.color = color;
	}

	public enum LookVector
	{
		X,
		Y,
		Z
	}
}
