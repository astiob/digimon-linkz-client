using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenCameraTargetFunction : MonoBehaviour
{
	[SerializeField]
	private Transform _cameraPosition;

	[SerializeField]
	private Transform _allyCameraPosition;

	[Range(1f, 179f)]
	[SerializeField]
	private float _fieldOfView = 65f;

	private bool _isBigBoss;

	private float _transitionScale = 1f;

	private Camera _camera;

	private List<Vector3> _targets;

	private Vector3 _inverseCameraPosition;

	private Quaternion _inverseCameraRotation;

	private Vector3 _currentPosition = Vector3.zero;

	private Quaternion _currentRotation = Quaternion.identity;

	private float _currentFieldOfView;

	private int _currentTargetIndex;

	private float _currentTransition = 1f;

	private IEnumerator _moveCamera;

	private bool _isMovingCamera;

	private bool _isInverse;

	public void Initialize(Camera camera)
	{
		this._camera = camera;
		Quaternion localRotation = base.transform.localRotation;
		base.transform.Rotate(new Vector3(0f, 180f, 0f));
		this._inverseCameraPosition = this._cameraPosition.position;
		this._inverseCameraRotation = this._cameraPosition.rotation;
		base.transform.localRotation = localRotation;
	}

	public void SetTargets(params Transform[] targets)
	{
		this._targets = new List<Vector3>();
		foreach (Transform transform in targets)
		{
			this._targets.Add(transform.position);
		}
	}

	public void SetTargets(params Vector3[] targets)
	{
		this._targets = new List<Vector3>(targets);
	}

	public bool isFoolowUp { get; set; }

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
			return this._currentTargetIndex;
		}
	}

	private Vector3 currentPosition
	{
		get
		{
			return this._targets[this._currentTargetIndex];
		}
	}

	private void StopInternal()
	{
		if (this._moveCamera != null)
		{
			base.StopCoroutine(this._moveCamera);
		}
		this._currentTransition = 1f;
		this._isMovingCamera = false;
	}

	public void Stop()
	{
		this.StopInternal();
		this.isFoolowUp = false;
	}

	private void LateUpdate()
	{
		if (!this._isMovingCamera)
		{
			return;
		}
		this._currentTransition = Mathf.Clamp01(this._currentTransition + Time.deltaTime * this._transitionScale);
		if (this._currentTargetIndex > -1)
		{
			this.SetMoveCamera(this.currentPosition, this._currentTransition);
		}
		else
		{
			this.SetMoveCamera(Vector3.zero, this._currentTransition);
		}
	}

	private void SetMoveCamera(Vector3 position, float lerp)
	{
		if (this._isBigBoss && this._allyCameraPosition != null)
		{
			this._camera.transform.position = Vector3.Lerp(this._currentPosition, this._isInverse ? this._allyCameraPosition.position : this._cameraPosition.position, lerp);
		}
		else
		{
			this._camera.transform.position = Vector3.Lerp(this._currentPosition, this._isInverse ? this._inverseCameraPosition : this._cameraPosition.position, lerp);
		}
		this._camera.fieldOfView = Mathf.Lerp(this._currentFieldOfView, this._fieldOfView, lerp);
		if (this._isBigBoss && this._allyCameraPosition != null)
		{
			this._camera.transform.rotation = Quaternion.Lerp(this._currentRotation, (this._currentTargetIndex < 0) ? (this._isInverse ? this._allyCameraPosition.rotation : this._cameraPosition.rotation) : Quaternion.LookRotation(position - this._camera.transform.position), lerp);
		}
		else
		{
			this._camera.transform.rotation = Quaternion.Lerp(this._currentRotation, (this._currentTargetIndex < 0) ? (this._isInverse ? this._inverseCameraRotation : this._cameraPosition.rotation) : Quaternion.LookRotation(position - this._camera.transform.position), lerp);
		}
	}

	public void SetLastTime()
	{
		this.StopInternal();
		if (this._currentTargetIndex > -1)
		{
			this.SetMoveCamera(this.currentPosition, this._currentTransition);
		}
		else
		{
			this.SetMoveCamera(Vector3.zero, this._currentTransition);
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

	public void SetCamera(bool isInverse, bool isBigBoss = false)
	{
		if (CameraParams.current != null)
		{
			CameraParams.current.StopCameraAnimation();
			CameraParams.current.isFollowUp = true;
		}
		this.StopInternal();
		this._currentTargetIndex = -1;
		this._isInverse = isInverse;
		this._isBigBoss = isBigBoss;
		this.SetCameraPosition(this._camera.transform.position, this._camera.transform.rotation, this._camera.fieldOfView);
		this._moveCamera = this.MoveCamera(this._cameraPosition.position);
		base.StartCoroutine(this._moveCamera);
	}

	public void SetCamera(int index, bool isInverse, bool isBigBoss = false)
	{
		if (index < 0 || index >= this._targets.Count)
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
		this._currentTargetIndex = index;
		this._isInverse = isInverse;
		this._isBigBoss = isBigBoss;
		this.SetCameraPosition(this._camera.transform.position, this._camera.transform.rotation, this._camera.fieldOfView);
		this._moveCamera = this.MoveCamera(this.currentPosition);
		base.StartCoroutine(this._moveCamera);
	}

	private IEnumerator MoveCamera(Vector3 currentCamera)
	{
		this._currentTransition = 0f;
		while (this.isMoving)
		{
			this._isMovingCamera = true;
			this.SetMoveCamera(currentCamera, this._currentTransition);
			yield return new WaitForEndOfFrame();
		}
		this._currentTransition = 1f;
		this._isMovingCamera = false;
		yield break;
	}
}
