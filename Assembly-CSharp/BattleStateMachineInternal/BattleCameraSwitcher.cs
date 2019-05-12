using System;
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

		[FormerlySerializedAs("minDistance")]
		[SerializeField]
		private float selectPointMinDistance = 0.5f;

		[SerializeField]
		private float minFov = 30f;

		[SerializeField]
		private float maxFov = 70f;

		private Transform _targetLookAtPoint;

		private Transform[] _cameraPositions;

		private Vector3 followPoint = Vector3.zero;

		private float _minDistance = float.PositiveInfinity;

		private float _maxDistance;

		private Transform _currentPosition;

		private bool isInitialized;

		private Transform root;

		private bool isPlaying;

		private float time;

		public void Initialize(Camera camera)
		{
			if (this.isInitialized)
			{
				return;
			}
			this._camera = camera;
			this._cameraPositions = this.GetCameraPositions();
			this.root = new GameObject("Root").transform;
			this.root.SetParent(base.transform);
			this.root.localPosition = Vector3.zero;
			this.root.localRotation = Quaternion.identity;
			this.root.localScale = Vector3.one;
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

		private void SetCharacter(CharacterParams character)
		{
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

		private Transform[] GetCameraPositions()
		{
			List<Transform> list = new List<Transform>(base.transform.GetComponentsInChildren<Transform>());
			list.Remove(base.transform);
			return list.ToArray();
		}

		private void Update()
		{
			if (!this.isPlaying)
			{
				return;
			}
			this.time += Time.deltaTime;
			if (this.time > this.interval)
			{
				this.time = 0f;
				this.followPoint = this._targetLookAtPoint.position + UnityEngine.Random.insideUnitSphere;
				this._currentPosition = this.ChooseAnotherPoint(this._currentPosition);
				this._camera.transform.position = this._currentPosition.position;
				float t = Mathf.Clamp01(this._minDistance - Vector3.Distance(this._targetLookAtPoint.position, this._camera.transform.position) / this._maxDistance);
				this._camera.fieldOfView = Mathf.Lerp(this.minFov, this.maxFov, t);
			}
			this.followPoint = Vector3.Lerp(this._targetLookAtPoint.position, this.followPoint, Mathf.Exp(-this.rotationSpeed * Time.deltaTime));
			this._camera.transform.LookAt(this.followPoint);
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
			return this._cameraPositions[UnityEngine.Random.Range(0, this._cameraPositions.Length - 1)];
		}

		public void StartAutoChange(CharacterParams character)
		{
			if (this._cameraPositions.Length <= 0 || character == null)
			{
				return;
			}
			this.SetCharacter(character);
			this.isPlaying = true;
			base.gameObject.SetActive(true);
			this.time = this.interval;
			this.followPoint = this._targetLookAtPoint.position + UnityEngine.Random.insideUnitSphere;
			this._currentPosition = this.RandomCameraPosition();
		}

		public void StopAutoChange()
		{
			this.isPlaying = false;
			base.gameObject.SetActive(false);
		}
	}
}
