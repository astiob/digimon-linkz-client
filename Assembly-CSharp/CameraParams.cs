using System;
using System.Collections;
using UnityEngine;
using UnityEngine.CameraParams.Internal;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
[AddComponentMenu("Digimon Effects/Camera Params")]
public class CameraParams : MonoBehaviour
{
	public static CameraParams current;

	private static readonly Vector3 inverseScale = new Vector3(-1f, 1f, 1f);

	[FormerlySerializedAs("cameraType")]
	[SerializeField]
	private CameraParams.CameraType _cameraType;

	[FormerlySerializedAs("rootPosition")]
	[SerializeField]
	private CameraParams.RootPosition _rootPosition;

	[FormerlySerializedAs("useAnimation")]
	[SerializeField]
	private bool _useAnimation = true;

	[FormerlySerializedAs("isLoopAnimation")]
	[SerializeField]
	private bool _onLoopAnimation;

	[FormerlySerializedAs("fieldOfView")]
	[SerializeField]
	[Range(1f, 179f)]
	private float _fieldOfView = 60f;

	[FormerlySerializedAs("cameraAnimation")]
	[SerializeField]
	private Animation _cameraAnimation;

	[FormerlySerializedAs("animationEndStop")]
	[SerializeField]
	private bool _animationEndStop = true;

	[FormerlySerializedAs("endTime")]
	[SerializeField]
	private float _endTime = 3f;

	[FormerlySerializedAs("cameraTarget")]
	[SerializeField]
	private Transform _cameraTarget;

	[FormerlySerializedAs("cameraLookTarget")]
	[SerializeField]
	private Transform _cameraLookTarget;

	[FormerlySerializedAs("onPossibleInverse")]
	[SerializeField]
	private bool _onPossibleInverse;

	[SerializeField]
	private float _cameraWorldDifference;

	[SerializeField]
	private CameraParams.CameraTargetValues[] _cameraTargetValues = new CameraParams.CameraTargetValues[1];

	[SerializeField]
	private int _cameraTargetIndex;

	private bool _isPlaying;

	private bool _isFollowUp = true;

	private Camera _currentTargetCamera;

	private IEnumerator currentPlayAnimationCorutine;

	private IEnumerator currentPlayShakeCorutine;

	private Transform cameraPositonDummy;

	private CameraPostEffectController _postEffectController;

	private bool isCameraUpdate;

	private Vector3 shakePosition = Vector3.zero;

	private int _previousTargetIndex;

	private CameraTargetIndexSelector indexSelector;

	private Transform cameraTransform;

	private CameraParams.CameraTargetValues currentTargetValue
	{
		get
		{
			return this._cameraTargetValues[this._cameraTargetIndex];
		}
	}

	private CameraParams.CameraType cameraType
	{
		get
		{
			return this.currentTargetValue.cameraType;
		}
	}

	private CameraParams.RootPosition rootPosition
	{
		get
		{
			return this._rootPosition;
		}
	}

	private bool useAnimation
	{
		get
		{
			return this._useAnimation;
		}
	}

	private bool onLoopAnimation
	{
		get
		{
			return this._onLoopAnimation;
		}
	}

	public float fieldOfView
	{
		get
		{
			return this.currentTargetValue.fieldOfView;
		}
	}

	private Animation cameraAnimation
	{
		get
		{
			return this._cameraAnimation;
		}
	}

	private bool animationEndStop
	{
		get
		{
			return this._animationEndStop;
		}
	}

	private float endTime
	{
		get
		{
			return this._endTime;
		}
	}

	private Transform cameraTarget
	{
		get
		{
			return this.currentTargetValue.cameraTarget;
		}
	}

	private Transform cameraLookTarget
	{
		get
		{
			return this.currentTargetValue.cameraLookTarget;
		}
	}

	private bool onPossibleInverse
	{
		get
		{
			return this._onPossibleInverse;
		}
	}

	private float cameraWorldDifference
	{
		get
		{
			return this.currentTargetValue.cameraWorldDifference;
		}
	}

	private CameraPostEffectController postEffectController
	{
		get
		{
			return this._postEffectController;
		}
	}

	public Camera currentTargetCamera
	{
		get
		{
			return this._currentTargetCamera;
		}
		set
		{
			this._currentTargetCamera = value;
		}
	}

	public Transform currentTarget
	{
		get
		{
			return this.cameraPositonDummy;
		}
	}

	public bool isPlaying
	{
		get
		{
			return this._isPlaying;
		}
		private set
		{
			this._isPlaying = value;
		}
	}

	public bool isFollowUp
	{
		get
		{
			return this._isFollowUp;
		}
		set
		{
			this._isFollowUp = value;
		}
	}

	public float time
	{
		get
		{
			if (this._useAnimation && this._cameraAnimation != null)
			{
				return this._cameraAnimation[this._cameraAnimation.clip.name].time / this._cameraAnimation[this._cameraAnimation.clip.name].length;
			}
			return 1f;
		}
		set
		{
			if (this._useAnimation && this._cameraAnimation != null)
			{
				this._cameraAnimation[this._cameraAnimation.clip.name].time = value * this._cameraAnimation[this._cameraAnimation.clip.name].length;
			}
		}
	}

	private void Awake()
	{
		CameraParams.CameraTargetValues currentTargetValue = this.currentTargetValue;
		currentTargetValue.cameraType = this._cameraType;
		currentTargetValue.fieldOfView = this._fieldOfView;
		currentTargetValue.cameraTarget = this._cameraTarget;
		currentTargetValue.cameraLookTarget = this._cameraLookTarget;
		currentTargetValue.cameraWorldDifference = this._cameraWorldDifference;
		this._previousTargetIndex = this._cameraTargetIndex;
	}

	private void Start()
	{
		this.cameraPositonDummy = new GameObject("CameraTarget").transform;
		this.cameraPositonDummy.SetParent(this.cameraTarget);
		foreach (CameraParams.CameraTargetValues cameraTargetValues2 in this._cameraTargetValues)
		{
			cameraTargetValues2.GetController();
		}
		this.indexSelector = base.GetComponentInChildren<CameraTargetIndexSelector>();
	}

	private void LateUpdate()
	{
		if (!this.isCameraUpdate)
		{
			return;
		}
		if (this._previousTargetIndex != this._cameraTargetIndex)
		{
			this.cameraPositonDummy.SetParent(this.currentTargetValue.cameraTarget);
			this._previousTargetIndex = this._cameraTargetIndex;
		}
		this.currentTargetValue.ApplyController();
		this.cameraPositonDummy.position = this.cameraTarget.position;
		this.cameraPositonDummy.localPosition += this.shakePosition;
		if (this.cameraType == CameraParams.CameraType.targetCamera)
		{
			this.cameraPositonDummy.LookAt(this.cameraLookTarget.position);
		}
		else
		{
			this.cameraPositonDummy.rotation = this.cameraTarget.rotation;
		}
		this.cameraPositonDummy.Translate(this.cameraPositonDummy.forward * this.cameraWorldDifference, Space.World);
		if (this.currentTargetCamera != null && this.isFollowUp)
		{
			this.cameraTransform = this.currentTargetCamera.transform;
			this.cameraTransform.position = this.cameraPositonDummy.position;
			this.cameraTransform.rotation = this.cameraPositonDummy.rotation;
			this.currentTargetCamera.fieldOfView = this.fieldOfView;
		}
		if (this._postEffectController != null)
		{
			this._postEffectController.ManualUpdate();
		}
		else if (CameraPostEffect.current != null)
		{
			CameraPostEffect.current.Off();
		}
	}

	public void GetPostEffectController(CameraPostEffect postEffect)
	{
		this._postEffectController = base.GetComponentInChildren<CameraPostEffectController>();
		if (this._postEffectController != null)
		{
			this._postEffectController.Initialize(postEffect);
		}
	}

	public void PlayCameraAnimation(Vector3 targetRootPosition, Vector3 targetRootRotation, bool onInverse = false, bool onClampAnimation = false)
	{
		base.gameObject.SetActive(true);
		this.StopCameraAnimationInternal();
		this.currentPlayAnimationCorutine = this.PlayCameraAnimationCorutine(targetRootPosition, targetRootRotation, onInverse, onClampAnimation);
		this.currentPlayAnimationCorutine.MoveNext();
		base.StartCoroutine(this.currentPlayAnimationCorutine);
	}

	public void PlayCameraAnimation(Transform targetRootPosition, bool onInverse = false, bool onClampAnimation = false)
	{
		base.gameObject.SetActive(true);
		this.StopCameraAnimationInternal();
		this.currentPlayAnimationCorutine = this.PlayCameraAnimationCorutine(targetRootPosition.position, targetRootPosition.eulerAngles, onInverse, onClampAnimation);
		this.currentPlayAnimationCorutine.MoveNext();
		base.StartCoroutine(this.currentPlayAnimationCorutine);
	}

	public void PlayCameraAnimation(CharacterParams characterParams, bool onInverse = false, bool onClampAnimation = false)
	{
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.PlayCameraAnimationIE(characterParams, onInverse, onClampAnimation));
	}

	private IEnumerator PlayCameraAnimationIE(CharacterParams characterParams, bool onInverse = false, bool onClampAnimation = false)
	{
		yield return new WaitForEndOfFrame();
		this.StopCameraAnimationInternal();
		this.currentPlayAnimationCorutine = this.PlayCameraAnimationCorutine(characterParams, onInverse, onClampAnimation);
		this.currentPlayAnimationCorutine.MoveNext();
		base.StartCoroutine(this.currentPlayAnimationCorutine);
		yield break;
	}

	private IEnumerator PlayCameraAnimationCorutine(Vector3 targetRootPosition, Vector3 targetRootRotation, bool onInverse, bool onClampAnimation)
	{
		this.isPlaying = true;
		this.isCameraUpdate = false;
		CameraParams.current = this;
		this.time = 0f;
		if (this.onPossibleInverse && onInverse)
		{
			base.transform.localScale = CameraParams.inverseScale;
		}
		else
		{
			base.transform.localScale = Vector3.one;
		}
		base.transform.position = targetRootPosition;
		base.transform.rotation = Quaternion.Euler(targetRootRotation);
		if (this.currentTargetCamera != null && this.isFollowUp)
		{
			this.currentTargetCamera.fieldOfView = this.fieldOfView;
		}
		bool onEndAnimation = false;
		float endTimeCache = this.endTime;
		bool onEndAnimationStop = false;
		if (onClampAnimation)
		{
			if (this.useAnimation)
			{
				onEndAnimationStop = true;
				if (!this.onLoopAnimation)
				{
					this.cameraAnimation.wrapMode = WrapMode.ClampForever;
				}
				else
				{
					this.cameraAnimation.wrapMode = WrapMode.Loop;
				}
				this.cameraAnimation.Play();
			}
			endTimeCache = float.PositiveInfinity;
		}
		else if (this.useAnimation)
		{
			if (this.animationEndStop)
			{
				if (!this.onLoopAnimation)
				{
					this.cameraAnimation.wrapMode = WrapMode.ClampForever;
				}
				else
				{
					this.cameraAnimation.wrapMode = WrapMode.Loop;
				}
			}
			else
			{
				this.cameraAnimation.wrapMode = WrapMode.Default;
			}
			this.cameraAnimation.Play();
		}
		float timeWait = 0f;
		while (!onEndAnimation)
		{
			this.isCameraUpdate = true;
			if (this.indexSelector != null)
			{
				this._cameraTargetIndex = this.indexSelector.cameraTargetIndex;
			}
			if (onEndAnimationStop)
			{
				if (!this.cameraAnimation.isPlaying)
				{
					onEndAnimation = true;
				}
			}
			else
			{
				float timeScaleDivided = TimeExtension.GetTimeScaleDivided(endTimeCache);
				if (timeScaleDivided < timeWait)
				{
					onEndAnimation = true;
				}
				if (endTimeCache == float.PositiveInfinity)
				{
					onEndAnimation = false;
				}
				timeWait += Time.deltaTime;
			}
			yield return null;
		}
		this.isCameraUpdate = false;
		this.isPlaying = false;
		yield break;
	}

	private IEnumerator PlayCameraAnimationCorutine(CharacterParams characterParams, bool onInverse, bool onClampAnimation)
	{
		IEnumerator playCameraAnimationCorutine;
		if (characterParams == null)
		{
			playCameraAnimationCorutine = this.PlayCameraAnimationCorutine(Vector3.zero, base.transform.eulerAngles, onInverse, onClampAnimation);
		}
		else if (this.rootPosition == CameraParams.RootPosition.targetRoot)
		{
			playCameraAnimationCorutine = this.PlayCameraAnimationCorutine(characterParams.transform.position, characterParams.transform.eulerAngles, onInverse, onClampAnimation);
		}
		else if (this.rootPosition == CameraParams.RootPosition.worldRoot)
		{
			playCameraAnimationCorutine = this.PlayCameraAnimationCorutine(Vector3.zero, characterParams.transform.eulerAngles, onInverse, onClampAnimation);
		}
		else if (this.rootPosition == CameraParams.RootPosition.targetCenter)
		{
			playCameraAnimationCorutine = this.PlayCameraAnimationCorutine(characterParams.characterCenterTarget.position, characterParams.transform.eulerAngles, onInverse, onClampAnimation);
		}
		else
		{
			playCameraAnimationCorutine = this.PlayCameraAnimationCorutine(characterParams.characterFaceCenterTarget.position, characterParams.transform.eulerAngles, onInverse, onClampAnimation);
		}
		while (playCameraAnimationCorutine.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	public void StopCameraAnimation()
	{
		this.StopCameraAnimationInternal();
		base.gameObject.SetActive(false);
	}

	private void StopCameraAnimationInternal()
	{
		this.StopCameraShake();
		if (this.isPlaying && this.currentTargetCamera != null)
		{
			if (this.currentPlayAnimationCorutine != null)
			{
				base.StopCoroutine(this.currentPlayAnimationCorutine);
			}
			if (this.useAnimation)
			{
				this.cameraAnimation.Stop();
			}
			this.isCameraUpdate = false;
			this.isPlaying = false;
		}
	}

	public void PlayCameraShake()
	{
		this.currentPlayShakeCorutine = this.PlayCameraShakeCoroutine();
		base.StartCoroutine(this.currentPlayShakeCorutine);
	}

	public void StopCameraShake()
	{
		if (this.currentPlayShakeCorutine != null)
		{
			base.StopCoroutine(this.currentPlayShakeCorutine);
		}
		this.ResetShakePosition();
	}

	private IEnumerator PlayCameraShakeCoroutine()
	{
		float maxShake = 0.25f;
		float currentShake = maxShake;
		bool isRev = true;
		this.ResetShakePosition();
		while (currentShake > 0.001f)
		{
			if (Time.timeScale == 0f)
			{
				yield return null;
			}
			else
			{
				this.shakePosition = new Vector3(0f, (!isRev) ? currentShake : (-currentShake), 0f);
				isRev = !isRev;
				currentShake -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
		this.ResetShakePosition();
		yield break;
	}

	private void ResetShakePosition()
	{
		this.shakePosition = Vector3.zero;
	}

	private enum RootPosition
	{
		targetRoot,
		targetCenter,
		worldRoot,
		targetFace
	}

	private enum CameraType
	{
		targetCamera,
		freeCamera
	}

	[Serializable]
	private sealed class CameraTargetValues
	{
		[SerializeField]
		private CameraParams.CameraType _cameraType;

		[SerializeField]
		[Range(1f, 179f)]
		private float _fieldOfView = 60f;

		[SerializeField]
		private Transform _cameraTarget;

		[SerializeField]
		private Transform _cameraLookTarget;

		[SerializeField]
		private float _cameraWorldDifference;

		private CameraTargetController controller;

		public CameraParams.CameraType cameraType
		{
			get
			{
				return this._cameraType;
			}
			set
			{
				this._cameraType = value;
			}
		}

		public float fieldOfView
		{
			get
			{
				return this._fieldOfView;
			}
			set
			{
				this._fieldOfView = value;
			}
		}

		public Transform cameraTarget
		{
			get
			{
				return this._cameraTarget;
			}
			set
			{
				this._cameraTarget = value;
			}
		}

		public Transform cameraLookTarget
		{
			get
			{
				return this._cameraLookTarget;
			}
			set
			{
				this._cameraLookTarget = value;
			}
		}

		public float cameraWorldDifference
		{
			get
			{
				return this._cameraWorldDifference;
			}
			set
			{
				this._cameraWorldDifference = value;
			}
		}

		public void ApplyController()
		{
			if (this.controller == null)
			{
				return;
			}
			this._fieldOfView = this.controller.fieldOfView;
			this._cameraWorldDifference = this.controller.cameraWorldDifference;
		}

		public void GetController()
		{
			this.controller = this._cameraTarget.GetComponent<CameraTargetController>();
		}
	}
}
