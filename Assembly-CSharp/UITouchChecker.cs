using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UITouchChecker : MonoBehaviour
{
	[SerializeField]
	private UICamera _uiCamera;

	[SerializeField]
	private bool _isClicked;

	private Collider cachedCollider;

	public UICamera uiCamera
	{
		set
		{
			this._uiCamera = value;
		}
	}

	public bool isClicked
	{
		get
		{
			return this._isClicked;
		}
	}

	private void Awake()
	{
		this.cachedCollider = base.GetComponent<Collider>();
	}

	private void Update()
	{
		this._isClicked = false;
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this._uiCamera == null)
		{
			return;
		}
		if (this.cachedCollider == null)
		{
			this.cachedCollider = base.GetComponent<Collider>();
		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			if (this.cachedCollider == null)
			{
				return;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(this._uiCamera.cachedCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity, this._uiCamera.eventReceiverMask) && raycastHit.collider == this.cachedCollider)
			{
				this._isClicked = true;
			}
		}
	}
}
