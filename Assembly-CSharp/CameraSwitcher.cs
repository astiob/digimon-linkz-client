using System;
using UnityEngine;

public sealed class CameraSwitcher : MonoBehaviour
{
	private const float INTERVAL = 16f;

	private const float MIN_DISTANCE = 0.5f;

	[Header("メインカメラ")]
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	[Header("サブカメラ")]
	private Camera subCamera;

	[SerializeField]
	[Header("カメラポジションルート")]
	private Transform[] cameraAnchorList;

	private Transform target;

	private Vector3 followPoint;

	private Vector3 subCameraLocator;

	private int cameraAnchorIndex;

	private float cameraPositionChangeInterval;

	public float angle { get; set; }

	private void Awake()
	{
		this.angle = 45f;
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup > this.cameraPositionChangeInterval)
		{
			this.cameraPositionChangeInterval = Time.realtimeSinceStartup + 16f;
			base.transform.localPosition = this.GetCameraAnchor();
		}
	}

	private void LateUpdate()
	{
		if (this.mainCamera.enabled)
		{
			this.followPoint = this.target.position;
			this.followPoint.y = this.followPoint.y - 0.3f;
			base.transform.LookAt(this.followPoint);
		}
		if (this.subCamera.enabled)
		{
			this.subCamera.transform.RotateAround(this.subCameraLocator, Vector3.up, -this.angle * Time.deltaTime);
			this.subCamera.transform.LookAt(this.target.position);
		}
	}

	private Vector3 GetCameraAnchor()
	{
		Transform transform = this.cameraAnchorList[this.cameraAnchorIndex];
		this.cameraAnchorIndex++;
		if (this.cameraAnchorList.Length <= this.cameraAnchorIndex)
		{
			this.cameraAnchorIndex = 0;
		}
		return transform.localPosition;
	}

	public void SetLookAtObject(Transform t)
	{
		this.target = t;
	}

	public void SetSubCameraAnchor(Vector3 locatorPosition)
	{
		this.subCameraLocator = locatorPosition;
	}

	public void EnableMainCamera()
	{
		this.mainCamera.enabled = true;
		this.subCamera.enabled = false;
	}

	public void EnableSubCamera()
	{
		this.subCamera.enabled = true;
		this.mainCamera.enabled = false;
	}
}
