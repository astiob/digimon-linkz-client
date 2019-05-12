using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ThreeDHoldPressButton : HoldPressButtonBase
{
	[SerializeField]
	private Camera _camera3D;

	private LayerMask colliderLayerMask;

	private Collider _myCollider;

	private bool _isStartClickMe;

	public Camera camera3D
	{
		set
		{
			this._camera3D = value;
		}
	}

	private void Awake()
	{
		this._myCollider = base.GetComponent<Collider>();
		int layer = base.gameObject.layer;
		this.colliderLayerMask = 1 << layer;
	}

	private bool GetMouseHit(out RaycastHit outHit)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this._camera3D.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity, this.colliderLayerMask))
		{
			outHit = raycastHit;
			return true;
		}
		outHit = default(RaycastHit);
		return false;
	}

	private void Update()
	{
		if (this._camera3D == null)
		{
			return;
		}
		bool flag = false;
		RaycastHit raycastHit;
		if (Input.GetKeyDown(KeyCode.Mouse0) && this.GetMouseHit(out raycastHit))
		{
			if (raycastHit.collider == this._myCollider)
			{
				this._isStartClickMe = true;
			}
			else
			{
				this._isStartClickMe = false;
			}
		}
		if (Input.GetKey(KeyCode.Mouse0) && this._isStartClickMe)
		{
			flag = true;
		}
		if (!base.isActiveAndEnabled)
		{
			this.nextTime = Time.unscaledTime;
			this.pressed = false;
			this._isStartClickMe = false;
			return;
		}
		if (this.pressed)
		{
			base.PressCount();
		}
		if (this.pressed != flag)
		{
			base.OnPressStartEnd(flag);
		}
		if (!this._isStartClickMe)
		{
			return;
		}
		if (!this.pressed)
		{
			this._isStartClickMe = false;
		}
		RaycastHit raycastHit2;
		if (!flag && Input.GetKeyUp(KeyCode.Mouse0) && this.GetMouseHit(out raycastHit2) && raycastHit2.collider == this._myCollider)
		{
			base.OnClick();
		}
	}
}
