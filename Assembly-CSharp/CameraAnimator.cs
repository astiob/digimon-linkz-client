using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : MonoBehaviour
{
	private static Camera currentCamera;

	private static Transform currentCameraTransform;

	private static List<CameraAnimator> current = new List<CameraAnimator>();

	private static int useIndex = 0;

	private static CameraAnimatorManager manager;

	[Range(1f, 179f)]
	[SerializeField]
	private float _fieldOfView = 60f;

	private int _myIndex;

	private Transform cachedTransform;

	public static void InitializeManager(Camera camera)
	{
		CameraAnimator.current.Clear();
		CameraAnimator.currentCamera = camera;
		CameraAnimator.currentCameraTransform = camera.transform;
		CameraAnimator.useIndex = 0;
		if (CameraAnimator.manager == null)
		{
			CameraAnimator.manager = new GameObject("CameraAnimatorManager").AddComponent<CameraAnimatorManager>();
		}
	}

	private static void UpdateIndex()
	{
		foreach (CameraAnimator cameraAnimator in CameraAnimator.current)
		{
			cameraAnimator._myIndex = CameraAnimator.current.IndexOf(cameraAnimator);
		}
	}

	public static void SetUseCamera(int index)
	{
		CameraAnimator.useIndex = index;
	}

	public static void UpdateCameraPosition()
	{
		if (CameraAnimator.useIndex >= CameraAnimator.current.Count)
		{
			return;
		}
		if (CameraAnimator.current[CameraAnimator.useIndex] == null)
		{
			return;
		}
		CameraAnimator.currentCameraTransform.position = CameraAnimator.current[CameraAnimator.useIndex].cachedTransform.position;
		CameraAnimator.currentCameraTransform.rotation = CameraAnimator.current[CameraAnimator.useIndex].cachedTransform.rotation;
		CameraAnimator.currentCamera.fieldOfView = CameraAnimator.current[CameraAnimator.useIndex]._fieldOfView;
	}

	private void Awake()
	{
		this.Initialize();
	}

	public void Initialize()
	{
		this.cachedTransform = base.transform;
		CameraAnimator.current.Remove(null);
		if (!CameraAnimator.current.Contains(this))
		{
			CameraAnimator.current.Add(this);
		}
		CameraAnimator.UpdateIndex();
	}

	public void SetUseMe()
	{
		CameraAnimator.useIndex = this._myIndex;
	}
}
