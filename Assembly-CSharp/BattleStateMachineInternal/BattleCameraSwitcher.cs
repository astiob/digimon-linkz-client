using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleStateMachineInternal
{
	public class BattleCameraSwitcher : MonoBehaviour
	{
		[SerializeField]
		private CharacterParams _characterParams;

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private float interval = 3f;

		[SerializeField]
		private float rotationSpeed = 2f;

		[SerializeField]
		[FormerlySerializedAs("minDistance")]
		private float selectPointMinDistance = 0.5f;

		[SerializeField]
		private float minFov = 30f;

		[SerializeField]
		private float maxFov = 70f;

		[SerializeField]
		private bool autoChange = true;

		private Transform _targetLookAtPoint;

		private bool _isFollowing;

		private Transform _cameraTransform;

		private Transform[] _cameraPositions;

		private Vector3 followPoint = Vector3.zero;

		private float _minDistance = float.PositiveInfinity;

		private float _maxDistance;

		private Transform _currentPosition;

		private bool _changePosition;

		private IEnumerator onAutoChangeCorutine;

		private bool isInitialized;

		private Transform root;

		private bool isPlaying;

		public bool isFollowing
		{
			get
			{
				return this._isFollowing;
			}
			set
			{
				this._isFollowing = value;
			}
		}

		public bool onAutoChange
		{
			get
			{
				return this.autoChange;
			}
			set
			{
				this.autoChange = value;
			}
		}

		public CharacterParams characterParams
		{
			get
			{
				return this._characterParams;
			}
			set
			{
				this._characterParams = value;
			}
		}

		public Camera camera
		{
			get
			{
				return this._camera;
			}
			set
			{
				this._camera = value;
			}
		}

		public void Initialize()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			if (this._cameraTransform == null)
			{
				this._cameraTransform = this._camera.transform;
			}
			this._cameraPositions = this.GetCameraPositions();
			if (this.root == null)
			{
				this.root = new GameObject("Root").transform;
				this.root.SetParent(base.transform);
				this.root.localPosition = Vector3.zero;
				this.root.localRotation = Quaternion.identity;
				this.root.localScale = Vector3.one;
			}
			foreach (Transform transform in this._cameraPositions)
			{
				if (this.root != transform.parent)
				{
					transform.SetParent(this.root);
				}
			}
			if (this._camera.transform == base.transform)
			{
				this.root.SetParent(base.transform.parent);
			}
			this.isInitialized = true;
		}

		public void SetCharacter(CharacterParams character)
		{
			this.StopAutoChange();
			this._characterParams = character;
			this._targetLookAtPoint = this._characterParams.characterFaceCenterTarget;
			this._minDistance = float.PositiveInfinity;
			this._maxDistance = 0f;
			foreach (Transform transform in this._cameraPositions)
			{
				float a = Vector3.Distance(transform.position, this._targetLookAtPoint.position);
				this._maxDistance = Mathf.Max(a, this._maxDistance);
				this._minDistance = Mathf.Min(a, this._minDistance);
			}
			this.root.localScale = Vector3.one * this._characterParams.RootToCenterDistance() * 1.2f;
			this.root.position = this._characterParams.characterCenterTarget.position;
		}

		private void OnEnable()
		{
			if (this.autoChange)
			{
				this.Initialize();
				this.StartAutoChange();
			}
		}

		private Transform[] GetCameraPositions()
		{
			List<Transform> list = new List<Transform>(base.transform.GetComponentsInChildren<Transform>());
			list.Remove(base.transform);
			return list.ToArray();
		}

		private void Update()
		{
			if (!this.isPlaying || !this.isInitialized || this._currentPosition == null)
			{
				return;
			}
			if (this._changePosition)
			{
				this.ChangePosition(this._currentPosition, false);
				this._changePosition = false;
			}
			this.followPoint = Vector3.Lerp(this._targetLookAtPoint.position, this.followPoint, Mathf.Exp(-this.rotationSpeed * Time.deltaTime));
			if (this.isFollowing)
			{
				this._cameraTransform.LookAt(this.followPoint);
			}
		}

		public void ChangePosition(Transform destination, bool forceStable = false)
		{
			if (!base.enabled)
			{
				return;
			}
			if (this._targetLookAtPoint == null)
			{
				return;
			}
			if (this.isFollowing)
			{
				this._cameraTransform.position = destination.position;
			}
			this.followPoint = this._targetLookAtPoint.position + UnityEngine.Random.insideUnitSphere;
			float t = Mathf.Clamp01(this._minDistance - Vector3.Distance(this._targetLookAtPoint.position, this._cameraTransform.position) / this._maxDistance);
			this._camera.fieldOfView = Mathf.Lerp(this.minFov, this.maxFov, t);
		}

		private Transform ChooseAnotherPoint(Transform current)
		{
			int num = 100;
			Transform transform;
			do
			{
				transform = this.RandomCameraPosition();
				if (transform == null)
				{
					break;
				}
				float num2 = Vector3.Distance(transform.position, this._targetLookAtPoint.position);
				if (transform != current && num2 > this.selectPointMinDistance)
				{
					break;
				}
				num--;
			}
			while (num >= 0);
			return transform;
		}

		private Transform RandomCameraPosition()
		{
			if (this._cameraPositions.Length <= 0)
			{
				return null;
			}
			return this._cameraPositions[UnityEngine.Random.Range(0, this._cameraPositions.Length - 1)];
		}

		private IEnumerator AutoChange()
		{
			if (this._cameraPositions.Length <= 0)
			{
				yield break;
			}
			this._currentPosition = this.RandomCameraPosition();
			this.followPoint = this._currentPosition.position;
			for (;;)
			{
				this._currentPosition = this.ChooseAnotherPoint(this._currentPosition);
				this._changePosition = true;
				this.isPlaying = true;
				yield return new WaitForSeconds(this.interval);
			}
			yield break;
		}

		public void StartAutoChange()
		{
			this.StopAutoChange();
			if (!this.onAutoChange)
			{
				base.gameObject.SetActive(true);
			}
			this.onAutoChangeCorutine = this.AutoChange();
			base.StartCoroutine(this.onAutoChangeCorutine);
		}

		public void StopAutoChange()
		{
			this.isPlaying = false;
			this._currentPosition = null;
			this._changePosition = false;
			if (this.onAutoChangeCorutine != null)
			{
				base.StopCoroutine(this.onAutoChangeCorutine);
			}
			if (!this.onAutoChange)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
