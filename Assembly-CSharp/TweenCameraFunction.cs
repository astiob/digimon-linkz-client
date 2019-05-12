using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenCameraFunction : MonoBehaviour
{
	private float _transitionScale = 1f;

	private List<CameraParams> _cachedCameraParams = new List<CameraParams>();

	private Camera _camera;

	private Vector3 _currentPosition = Vector3.zero;

	private Quaternion _currentRotation = Quaternion.identity;

	private float _currentFieldOfView;

	private float _currentTransition = 1f;

	private int _currentCameraParamsIndex;

	private IEnumerator _moveCamera;

	private bool _isMovingCamera;

	public static TweenCameraFunction Create(Camera camera, params CameraParams[] cameraParams)
	{
		GameObject gameObject = new GameObject("TweenCameraFunction");
		TweenCameraFunction tweenCameraFunction = gameObject.AddComponent<TweenCameraFunction>();
		tweenCameraFunction.Initialize(camera, cameraParams);
		return tweenCameraFunction;
	}

	public void Initialize(Camera camera, params CameraParams[] cameraParams)
	{
		this._camera = camera;
		this._cachedCameraParams = new List<CameraParams>(cameraParams);
	}

	public float transitionScale
	{
		get
		{
			return this._transitionScale;
		}
		set
		{
			this._transitionScale = Mathf.Clamp(value, 0f, float.PositiveInfinity);
		}
	}

	public bool isMoving
	{
		get
		{
			return this._currentTransition < 1f;
		}
	}

	public int currentIndex
	{
		get
		{
			return this._currentCameraParamsIndex;
		}
	}

	private CameraParams currentCameraParams
	{
		get
		{
			return this._cachedCameraParams[this._currentCameraParamsIndex];
		}
	}

	public void SetCameraPosition(Vector3 position, Quaternion rotation, float fieldOfView)
	{
		this._currentPosition = position;
		this._currentRotation = rotation;
		this._currentFieldOfView = fieldOfView;
		this._camera.transform.position = this._currentPosition;
		this._camera.transform.rotation = this._currentRotation;
		this._camera.fieldOfView = this._currentFieldOfView;
	}

	public void SetCamera(int index, float TransitionScale, Transform targetRootPosition, bool onInverse = false)
	{
		this._transitionScale = TransitionScale;
		this.SetCamera(index, targetRootPosition, onInverse);
	}

	public void SetCamera(int index, Transform targetRootPosition, bool onInverse = false)
	{
		if (index < 0 || index >= this._cachedCameraParams.Count)
		{
			global::Debug.LogException(new IndexOutOfRangeException());
			return;
		}
		if (CameraParams.current != null)
		{
			CameraParams.current.StopCameraAnimation();
			CameraParams.current.isFollowUp = true;
		}
		this.StopInternal();
		this._currentCameraParamsIndex = index;
		this.SetCameraPosition(this._camera.transform.position, this._camera.transform.rotation, this._camera.fieldOfView);
		this.currentCameraParams.isFollowUp = false;
		this.currentCameraParams.PlayCameraAnimation(targetRootPosition, onInverse, true);
		this._moveCamera = this.MoveCamera(this.currentCameraParams);
		base.StartCoroutine(this._moveCamera);
	}

	public void SetLastTime()
	{
		this.StopInternal();
		this.currentCameraParams.time = 1f;
		this.SetMoveCamera(this.currentCameraParams, this._currentTransition);
	}

	private void SetMoveCamera(CameraParams currentCamera, float lerp)
	{
		this._camera.transform.position = Vector3.Lerp(this._currentPosition, currentCamera.currentTarget.position, lerp);
		this._camera.transform.rotation = Quaternion.Lerp(this._currentRotation, currentCamera.currentTarget.rotation, lerp);
		this._camera.fieldOfView = Mathf.Lerp(this._currentFieldOfView, currentCamera.fieldOfView, lerp);
	}

	private IEnumerator MoveCamera(CameraParams currentCamera)
	{
		this._currentTransition = 0f;
		while (currentCamera.isPlaying)
		{
			this._isMovingCamera = true;
			this.SetMoveCamera(currentCamera, this._currentTransition);
			yield return null;
		}
		this._isMovingCamera = false;
		yield break;
	}

	private void Update()
	{
		if (!this._isMovingCamera)
		{
			return;
		}
		this._currentTransition = Mathf.Clamp01(this._currentTransition + Time.deltaTime * this._transitionScale);
	}

	private void LateUpdate()
	{
		if (!this._isMovingCamera)
		{
			return;
		}
		this.SetMoveCamera(this.currentCameraParams, this._currentTransition);
	}

	private void StopInternal()
	{
		if (this._moveCamera != null)
		{
			base.StopCoroutine(this._moveCamera);
		}
		this.currentCameraParams.StopCameraAnimation();
		this.currentCameraParams.isFollowUp = true;
		this._currentTransition = 1f;
		this._isMovingCamera = false;
	}

	public void Stop()
	{
		this.StopInternal();
	}
}
