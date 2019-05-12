using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

[RequireComponent(typeof(CapsuleCollider))]
[DisallowMultipleComponent]
public class CharacterParams : MonoBehaviour
{
	public const float DeathEffectGenerationInterval = 1f;

	private const float HUDDistance = 0.15f;

	private const float AnimationCrossFadeLength = 0.5f;

	private const float StartAnimationCrossFadeLength = 0.5f;

	private const string QueueName = " - Queued Clone";

	[SerializeField]
	private Animation _characterAnimation;

	[SerializeField]
	private Transform _characterFaceCenterTarget;

	[SerializeField]
	private Transform _characterCenterTarget;

	[SerializeField]
	private Transform _hudTarget;

	[SerializeField]
	private Vector3 _previewCameraDifference = new Vector3(0f, 0.65f, 3.4f);

	[SerializeField]
	private float _targetToDistance = -1f;

	[SerializeField]
	private float _effectScale = 1f;

	[SerializeField]
	private Vector3 _dropItemOffsetPosition = Vector3.zero;

	[SerializeField]
	private CharacterAnimationClip _clips;

	[SerializeField]
	private bool _isBigBoss;

	[SerializeField]
	private string _battleInSE = string.Empty;

	[SerializeField]
	private string _battleOutSE = string.Empty;

	[SerializeField]
	private CharacterParams.AttachEffectLocators[] _attachEffectLocators;

	private float _hudHeightCache = float.NegativeInfinity;

	private float _hudFrontCache = float.PositiveInfinity;

	private Vector3? centerPositionCache;

	private List<BillboardObject> _billboardObjects = new List<BillboardObject>();

	private CharacterDeadEffect characterDeadEffect;

	private Renderer[] _isHavingRenderers = new Renderer[0];

	private List<GameObject> _particleObjectList = new List<GameObject>();

	private MaterialController[] materialControllers = new MaterialController[0];

	private ShadowParams shadowObject;

	private bool isPlayingAnimation;

	private CharacterStateControl _characterStateControl;

	public Collider collider { get; private set; }

	public bool isActiveAnimation
	{
		get
		{
			return this._characterAnimation.isPlaying;
		}
	}

	public CharacterAnimationType currentAnimationType { get; private set; }

	public Transform characterFaceCenterTarget
	{
		get
		{
			return this._characterFaceCenterTarget;
		}
	}

	public Transform characterCenterTarget
	{
		get
		{
			return this._characterCenterTarget;
		}
	}

	public Transform hudTarget
	{
		get
		{
			return this._hudTarget;
		}
	}

	public float AnimationClipLength
	{
		get
		{
			return this._characterAnimation.clip.length;
		}
	}

	public float effectScale
	{
		get
		{
			return this._effectScale;
		}
	}

	public Vector3 dropItemOffsetPosition
	{
		get
		{
			return this._dropItemOffsetPosition;
		}
	}

	public string battleInSE
	{
		get
		{
			return this._battleInSE;
		}
	}

	public string battleOutSE
	{
		get
		{
			return this._battleOutSE;
		}
	}

	public Dictionary<string, Transform> attachEffectLocators { get; private set; }

	private void Awake()
	{
		this._clips = new CharacterAnimationClip(this._clips, this._characterAnimation);
		this.collider = base.GetComponentInChildren<Collider>();
		if (this._isHavingRenderers.Length <= 0)
		{
			MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
			SkinnedMeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<SkinnedMeshRenderer>();
			List<Renderer> list = new List<Renderer>();
			foreach (MeshRenderer item in componentsInChildren)
			{
				list.Add(item);
			}
			foreach (SkinnedMeshRenderer item2 in componentsInChildren2)
			{
				list.Add(item2);
			}
			this._isHavingRenderers = list.ToArray();
			MaterialController[] componentsInChildren3 = base.GetComponentsInChildren<MaterialController>(true);
			if (componentsInChildren3 != null)
			{
				this.materialControllers = componentsInChildren3;
			}
		}
		if (this._particleObjectList.Count <= 0)
		{
			ParticleSystem[] componentsInChildren4 = base.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren4)
			{
				this._particleObjectList.Add(particleSystem.gameObject);
			}
		}
		this.characterDeadEffect = new CharacterDeadEffect(this);
		this.attachEffectLocators = new Dictionary<string, Transform>();
		if (this._attachEffectLocators != null)
		{
			foreach (CharacterParams.AttachEffectLocators attachEffectLocators2 in this._attachEffectLocators)
			{
				this.attachEffectLocators[attachEffectLocators2.name] = attachEffectLocators2.locatorTransform;
			}
		}
	}

	public void Initialize(Camera camera)
	{
		base.enabled = true;
		bool activeSelf = base.gameObject.activeSelf;
		base.gameObject.SetActive(true);
		this.SetActiveRenderers(true);
		this._billboardObjects = new List<BillboardObject>(base.GetComponentsInChildren<BillboardObject>());
		for (int i = 0; i < this._billboardObjects.Count; i++)
		{
			this._billboardObjects[i].lookTarget = camera;
			this._billboardObjects[i].onBillboard = true;
			if (!this._billboardObjects[i].onIgnoreManage)
			{
				this._billboardObjects[i].distance = this._targetToDistance;
				if (!this._billboardObjects[i].onIgnoreFollowingTransformOverride)
				{
					this._billboardObjects[i].manualPositionTransform = base.transform;
				}
			}
			this._billboardObjects[i].ManualUpdate();
		}
		base.transform.localScale = Vector3.one;
		this.StopAnimation();
		base.gameObject.SetActive(activeSelf);
		if (this.centerPositionCache == null)
		{
			this.centerPositionCache = new Vector3?(base.transform.InverseTransformPoint(this._characterCenterTarget.position));
			if (this.hudTarget != null)
			{
				this._hudHeightCache = this.hudTarget.position.y;
				this._hudFrontCache = (this.hudTarget.position - base.transform.position).z;
			}
		}
		if (BattleStateManager.current != null)
		{
			BattleStateManager.current.soundPlayer.AddEffectSe(this._battleInSE);
			BattleStateManager.current.soundPlayer.AddEffectSe(this._battleOutSE);
		}
		this.SetBillBoardCamera(camera);
	}

	private void LateUpdate()
	{
		if (this.shadowObject != null)
		{
			this.shadowObject.UpdateShadowPosition(this);
		}
		foreach (BillboardObject billboardObject in this._billboardObjects)
		{
			billboardObject.ManualUpdate();
		}
	}

	private void SetActiveRenderers(bool value)
	{
		foreach (Renderer renderer in this._isHavingRenderers)
		{
			renderer.enabled = value;
		}
		if (this.shadowObject != null)
		{
			this.shadowObject.shadowEnable = value;
		}
		foreach (MaterialController materialController in this.materialControllers)
		{
			materialController.enabled = value;
		}
		foreach (GameObject gameObject in this._particleObjectList)
		{
			gameObject.SetActive(value);
		}
	}

	public void SetBillBoardCamera(Camera cam)
	{
		BillBoardController[] componentsInChildren = base.gameObject.GetComponentsInChildren<BillBoardController>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetUp(cam);
		}
	}

	public float RootToCenterDistance()
	{
		return this._targetToDistance * Vector3Extension.GetMaxFloat(base.transform.localScale);
	}

	public Vector3 HudPosition()
	{
		if (this.hudTarget != null)
		{
			Vector3 result = new Vector3(base.transform.position.x, this._hudHeightCache + 0.15f, base.transform.position.z + this._hudFrontCache);
			return result;
		}
		return base.transform.position;
	}

	public Vector3 GetFixableCenterPosition()
	{
		if (this.centerPositionCache != null)
		{
			return base.transform.TransformPoint(this.centerPositionCache.Value) + this.dropItemOffsetPosition;
		}
		return this._characterCenterTarget.position + this.dropItemOffsetPosition;
	}

	public void SetShadowObject()
	{
		this.shadowObject = ShadowParams.SetShadowObject(this);
	}

	public bool isPlaying(CharacterAnimationType animationType)
	{
		string name = string.Empty;
		switch (animationType)
		{
		case CharacterAnimationType.idle:
			name = this._clips.idle.name;
			break;
		case CharacterAnimationType.hit:
			name = this._clips.hit.name;
			break;
		case CharacterAnimationType.dead:
			name = this._clips.down.name;
			break;
		case CharacterAnimationType.guard:
			name = this._clips.guard.name;
			break;
		case CharacterAnimationType.revival:
			name = this._clips.revival.name;
			break;
		case CharacterAnimationType.win:
			name = this._clips.win.name;
			break;
		case CharacterAnimationType.eat:
			name = this._clips.eat.name;
			break;
		case CharacterAnimationType.move:
			name = this._clips.move.name;
			break;
		case CharacterAnimationType.down:
			name = this._clips.down.name;
			break;
		case CharacterAnimationType.getup:
			name = this._clips.getup.name;
			break;
		case CharacterAnimationType.strongHit:
			name = this._clips.down.name;
			break;
		}
		return this._characterAnimation.IsPlaying(name);
	}

	public bool GetFindAttackMotion(int index)
	{
		Exception findAttackClip = this._clips.GetFindAttackClip(index);
		if (typeof(IndexOutOfRangeException) == findAttackClip.GetType())
		{
			global::Debug.LogWarning(string.Concat(new object[]
			{
				base.name,
				"の固有技アニメーションの長さに合っていません. (探した値: ",
				index,
				")"
			}));
		}
		else
		{
			if (typeof(NullReferenceException) != findAttackClip.GetType())
			{
				return true;
			}
			global::Debug.LogWarning(string.Concat(new object[]
			{
				base.name,
				"の固有技アニメーションがnullです. (探した値: ",
				index,
				")"
			}));
		}
		return false;
	}

	public void PlayIdleAnimation()
	{
		this.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
	}

	public void PlayDeadAnimation(HitEffectParams hitEffectParams, Action callback)
	{
		this.PlayAnimation(CharacterAnimationType.dead, SkillType.Attack, 0, hitEffectParams, callback);
	}

	public void PlayAttackAnimation(SkillType attackType = SkillType.Attack, int motionIndex = 0)
	{
		this.PlayAnimation(CharacterAnimationType.attacks, attackType, motionIndex, null, null);
	}

	public void PlayAttackAnimationSmooth(SkillType attackType = SkillType.Attack, int motionIndex = 0)
	{
		this.PlayAnimationSmooth(CharacterAnimationType.attacks, attackType, motionIndex, null, null);
	}

	public void PlayHitAnimation()
	{
		this.PlayAnimation(CharacterAnimationType.hit, SkillType.Attack, 0, null, null);
	}

	public void PlayRevivalAnimation()
	{
		this.PlayAnimation(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
	}

	public void PlayRevivalAnimationSmooth()
	{
		this.PlayAnimationSmooth(CharacterAnimationType.revival, SkillType.Attack, 0, null, null);
	}

	public void PlayGuardAnimation()
	{
		this.PlayAnimation(CharacterAnimationType.guard, SkillType.Attack, 0, null, null);
	}

	public void PlayAnimationSmooth(CharacterAnimationType type, SkillType attackType = SkillType.Attack, int motionIndex = 0, HitEffectParams hitEffectParams = null, Action callback = null)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		this.PlayAnimationCorutine(type, attackType, motionIndex, hitEffectParams, callback, true);
	}

	public void PlayAnimation(CharacterAnimationType type, SkillType attackType = SkillType.Attack, int motionIndex = 0, HitEffectParams hitEffectParams = null, Action callback = null)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		this.PlayAnimationCorutine(type, attackType, motionIndex, hitEffectParams, callback, false);
	}

	private void PlayAnimationCorutine(CharacterAnimationType type, SkillType attackType = SkillType.Attack, int motionIndex = 0, HitEffectParams hitEffectParams = null, Action callback = null, bool isSmooth = false)
	{
		if (!this._characterAnimation)
		{
			return;
		}
		if (this._isBigBoss && type == CharacterAnimationType.strongHit)
		{
			type = CharacterAnimationType.hit;
		}
		this.isPlayingAnimation = true;
		AnimationClip clip = this._characterAnimation.clip;
		this.currentAnimationType = type;
		switch (type)
		{
		case CharacterAnimationType.idle:
			if (!this._clips.idle)
			{
				return;
			}
			this._clips.idle.wrapMode = WrapMode.Loop;
			this._characterAnimation.clip = this._clips.idle;
			goto IL_3D4;
		case CharacterAnimationType.hit:
			if (!this._clips.hit)
			{
				return;
			}
			this._clips.hit.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.hit;
			goto IL_3D4;
		case CharacterAnimationType.dead:
			if (!this._clips.down)
			{
				return;
			}
			this._clips.down.wrapMode = WrapMode.ClampForever;
			this._characterAnimation.clip = this._clips.down;
			goto IL_3D4;
		case CharacterAnimationType.guard:
			if (!this._clips.guard)
			{
				return;
			}
			this._clips.guard.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.guard;
			goto IL_3D4;
		case CharacterAnimationType.revival:
			if (!this._clips.revival)
			{
				return;
			}
			this._clips.revival.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.revival;
			goto IL_3D4;
		case CharacterAnimationType.win:
			if (!this._clips.win)
			{
				return;
			}
			this._clips.win.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.win;
			goto IL_3D4;
		case CharacterAnimationType.eat:
			if (!this._clips.eat)
			{
				return;
			}
			this._clips.eat.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.eat;
			goto IL_3D4;
		case CharacterAnimationType.move:
			if (!this._clips.move)
			{
				return;
			}
			if (this._isBigBoss)
			{
				this._clips.move.wrapMode = WrapMode.Once;
			}
			else
			{
				this._clips.move.wrapMode = WrapMode.Loop;
			}
			this._characterAnimation.clip = this._clips.move;
			goto IL_3D4;
		case CharacterAnimationType.down:
			if (!this._clips.down)
			{
				return;
			}
			this._clips.down.wrapMode = WrapMode.ClampForever;
			this._characterAnimation.clip = this._clips.down;
			goto IL_3D4;
		case CharacterAnimationType.getup:
			if (!this._clips.getup)
			{
				return;
			}
			this._clips.getup.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.getup;
			goto IL_3D4;
		case CharacterAnimationType.strongHit:
			if (!this._clips.down)
			{
				return;
			}
			this._clips.down.wrapMode = WrapMode.Once;
			this._characterAnimation.clip = this._clips.down;
			goto IL_3D4;
		}
		AnimationClip attackClip = this._clips.GetAttackClip(attackType, motionIndex);
		if (!attackClip)
		{
			return;
		}
		if (attackType == SkillType.Deathblow)
		{
			attackClip.wrapMode = WrapMode.ClampForever;
		}
		else
		{
			attackClip.wrapMode = WrapMode.Once;
		}
		this._characterAnimation.clip = attackClip;
		IL_3D4:
		if (type != CharacterAnimationType.strongHit)
		{
			if (this._characterAnimation.clip.wrapMode == WrapMode.Once || this._characterAnimation.clip.wrapMode == WrapMode.Default)
			{
				if (isSmooth)
				{
					this._characterAnimation.CrossFadeQueued(this._characterAnimation.clip.name, 0.5f, QueueMode.PlayNow);
				}
				else
				{
					this._characterAnimation.PlayQueued(this._characterAnimation.clip.name, QueueMode.PlayNow);
				}
				if (this._clips.idle != null)
				{
					this._characterAnimation.CrossFadeQueued(this._clips.idle.name, 0.5f, QueueMode.CompleteOthers);
				}
			}
			else if (this._characterAnimation.clip.wrapMode == WrapMode.Loop)
			{
				if (!this._characterAnimation.isPlaying)
				{
					if (isSmooth)
					{
						this._characterAnimation.CrossFadeQueued(this._characterAnimation.clip.name, 0.5f, QueueMode.PlayNow);
					}
					else
					{
						this._characterAnimation.PlayQueued(this._characterAnimation.clip.name, QueueMode.PlayNow);
					}
				}
				else if (this._characterAnimation.clip != clip)
				{
					QueueMode queue = QueueMode.CompleteOthers;
					if (type != CharacterAnimationType.idle)
					{
						queue = QueueMode.PlayNow;
					}
					this._characterAnimation.CrossFadeQueued(this._characterAnimation.clip.name, 0.5f, queue);
				}
			}
			else if (isSmooth)
			{
				this._characterAnimation.CrossFadeQueued(this._characterAnimation.clip.name, 0.5f, QueueMode.PlayNow);
			}
			else
			{
				this._characterAnimation.PlayQueued(this._characterAnimation.clip.name, QueueMode.PlayNow);
			}
		}
		else
		{
			if (isSmooth)
			{
				this._characterAnimation.CrossFadeQueued(this._characterAnimation.clip.name, 0.5f, QueueMode.PlayNow);
			}
			else
			{
				this._characterAnimation.PlayQueued(this._characterAnimation.clip.name, QueueMode.PlayNow);
			}
			foreach (object obj in this._characterAnimation)
			{
				AnimationState animationState = obj as AnimationState;
				if (animationState.name.Replace(" - Queued Clone", string.Empty).Equals(this._characterAnimation.clip.name))
				{
					animationState.wrapMode = this._clips.getup.wrapMode;
				}
			}
			if (this._clips.getup)
			{
				this._clips.getup.wrapMode = WrapMode.Once;
				this._characterAnimation.CrossFadeQueued(this._clips.getup.name, 0.5f, QueueMode.CompleteOthers);
			}
			if (this._clips.idle != null)
			{
				this._characterAnimation.CrossFadeQueued(this._clips.idle.name, 0.5f, QueueMode.CompleteOthers);
			}
		}
		this.SetActiveRenderers(true);
		if (type == CharacterAnimationType.dead)
		{
			Action callbackDeathEffect = delegate()
			{
				if (callback != null)
				{
					callback();
				}
				this.SetActiveRenderers(false);
				this.StopAnimation();
			};
			this.characterDeadEffect.PlayAnimation(hitEffectParams, 1f, callbackDeathEffect);
		}
	}

	public void StopAnimation()
	{
		if (this.isPlayingAnimation)
		{
			this.characterDeadEffect.StopAnimation();
			if (this._clips.idle != null)
			{
				this._characterAnimation.PlayQueued(this._clips.idle.name, QueueMode.PlayNow);
				this._characterAnimation.Stop();
			}
			this.isPlayingAnimation = false;
		}
	}

	public Vector3 GetPreviewCameraPosition(Transform cameraParent = null)
	{
		Quaternion rotation = base.transform.rotation;
		base.transform.rotation = ((!(cameraParent != null)) ? Quaternion.identity : cameraParent.rotation);
		Vector3 result = base.transform.TransformPoint(this._previewCameraDifference);
		base.transform.rotation = rotation;
		return result;
	}

	public void SetPreviewCamera(Camera camera)
	{
		camera.transform.localRotation = Quaternion.identity;
		camera.transform.position = this.GetPreviewCameraPosition(camera.transform);
		camera.transform.rotation = Quaternion.Euler((!(camera.transform.parent != null)) ? new Vector3(0f, 180f, 0f) : (camera.transform.parent.eulerAngles - new Vector3(0f, 180f, 0f)));
	}

	public Vector3 GetPreviewCameraDifference()
	{
		return this._previewCameraDifference;
	}

	public CharacterStateControl characterStateControl
	{
		get
		{
			return this._characterStateControl;
		}
		set
		{
			this._characterStateControl = value;
		}
	}

	[Serializable]
	public class AttachEffectLocators
	{
		[SerializeField]
		private string _name = string.Empty;

		[SerializeField]
		private Transform _locatorTransform;

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public Transform locatorTransform
		{
			get
			{
				return this._locatorTransform;
			}
		}
	}
}
