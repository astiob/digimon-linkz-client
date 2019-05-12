using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class EffectParamsGeneric : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("effectAnimation")]
	protected Animation _effectAnimation;

	protected bool _isPlaying;

	protected List<BillboardObject> _billboardObjects = new List<BillboardObject>();

	protected List<TranslateObject> _translateObjects = new List<TranslateObject>();

	protected List<CharacterFollowingTarget> _characterFollowingTargets = new List<CharacterFollowingTarget>();

	protected List<ParticleController> _particleControllers = new List<ParticleController>();

	protected List<RotationFixer> _rotationFixer = new List<RotationFixer>();

	protected Camera _renderCamera;

	protected float _scale = 1f;

	protected bool _isAwakeInitialized;

	protected ParticleSystem[] _particleSystems = new ParticleSystem[0];

	public bool isPlaying
	{
		get
		{
			return this._isPlaying;
		}
	}

	private void Awake()
	{
		this._particleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
	}

	private void OnEnable()
	{
		if (this._effectAnimation == null)
		{
			this._effectAnimation = base.GetComponentInChildren<Animation>();
		}
		if (this._effectAnimation != null)
		{
			this._effectAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		}
	}

	public IEnumerator Initialize(Camera renderCamera)
	{
		bool isActive = base.gameObject.activeSelf;
		base.gameObject.SetActive(true);
		if (!this._isAwakeInitialized)
		{
			this.InitializeAwake();
		}
		this._renderCamera = renderCamera;
		foreach (BillboardObject b in this._billboardObjects)
		{
			b.lookTarget = this._renderCamera;
		}
		base.gameObject.SetActive(isActive);
		yield break;
	}

	private void InitializeAwake()
	{
		Transform transform = null;
		if (!base.gameObject.activeInHierarchy)
		{
			transform = base.transform.parent;
		}
		if (transform != null)
		{
			base.transform.SetParent(null);
		}
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(BillboardObject), true);
		foreach (Component component in componentsInChildren)
		{
			this._billboardObjects.Add(component as BillboardObject);
		}
		componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(TranslateObject), true);
		foreach (Component component2 in componentsInChildren)
		{
			this._translateObjects.Add(component2 as TranslateObject);
		}
		componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(CharacterFollowingTarget), true);
		foreach (Component component3 in componentsInChildren)
		{
			this._characterFollowingTargets.Add(component3 as CharacterFollowingTarget);
		}
		componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ParticleController), true);
		foreach (Component component4 in componentsInChildren)
		{
			this._particleControllers.Add(component4 as ParticleController);
		}
		componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(RotationFixer), true);
		foreach (Component component5 in componentsInChildren)
		{
			this._rotationFixer.Add(component5 as RotationFixer);
		}
		if (transform != null)
		{
			base.transform.SetParent(transform);
		}
		this._isAwakeInitialized = true;
	}

	public virtual void SetPosition(Transform position = null, Vector3? offsetPosition = null)
	{
		if (position != null)
		{
			Vector3 b = (offsetPosition == null) ? Vector3.zero : offsetPosition.Value;
			base.transform.position = position.position + b;
			base.transform.rotation = position.rotation;
		}
	}

	protected virtual void BillboardObjectInitializeInternal(Transform position, float distance)
	{
		foreach (BillboardObject billboardObject in this._billboardObjects)
		{
			if (!billboardObject.onIgnoreManage)
			{
				if (!billboardObject.onIgnoreFollowingTransformOverride)
				{
					billboardObject.manualPositionTransform = base.transform;
				}
				billboardObject.distance = distance;
			}
			billboardObject.onBillboard = true;
			billboardObject.ManualUpdate();
		}
		foreach (TranslateObject translateObject in this._translateObjects)
		{
			translateObject.worldDistance = distance;
			translateObject.onUpdate = true;
			translateObject.ManualUpdate();
		}
	}

	protected virtual void BillboardObjectUpdateInternal()
	{
		foreach (BillboardObject billboardObject in this._billboardObjects)
		{
			billboardObject.ManualUpdate();
		}
		foreach (TranslateObject translateObject in this._translateObjects)
		{
			translateObject.ManualUpdate();
		}
	}

	protected virtual void CharacterFollowingInitializeInternal(CharacterParams character)
	{
		foreach (CharacterFollowingTarget characterFollowingTarget in this._characterFollowingTargets)
		{
			switch (characterFollowingTarget.characterTarget)
			{
			case CharacterTarget.CharacterCenter:
				characterFollowingTarget.following = character.characterCenterTarget;
				break;
			case CharacterTarget.CharacterRoot:
				characterFollowingTarget.following = character.transform;
				break;
			case CharacterTarget.CharacterFaceCenter:
				characterFollowingTarget.following = character.characterFaceCenterTarget;
				break;
			default:
				characterFollowingTarget.following = null;
				break;
			}
			characterFollowingTarget.onAutoUpdate = false;
			characterFollowingTarget.ManualUpdate();
		}
	}

	protected virtual void CharacterFollowingUpdateInternal()
	{
		foreach (CharacterFollowingTarget characterFollowingTarget in this._characterFollowingTargets)
		{
			characterFollowingTarget.ManualUpdate();
		}
	}

	protected virtual void ParticheControllerInitializeInternal()
	{
		foreach (ParticleController particleController in this._particleControllers)
		{
			particleController.ClearParticle();
			particleController.RefleshPartiche();
		}
	}

	protected virtual void RotationFixerUpdateInternal(CharacterParams character)
	{
		foreach (RotationFixer rotationFixer in this._rotationFixer)
		{
			rotationFixer.SetRotation(character);
		}
	}

	protected virtual void StopAnimationInternal()
	{
		if (this._effectAnimation != null && this._effectAnimation.isPlaying)
		{
			this._effectAnimation.Stop();
			this._effectAnimation[this._effectAnimation.clip.name].time = this._effectAnimation[this._effectAnimation.clip.name].length;
		}
		foreach (ParticleController particleController in this._particleControllers)
		{
			particleController.ClearParticle();
		}
		this._isPlaying = false;
		float scale = 1f / this._scale;
		base.transform.localScale = Vector3.one;
		foreach (ParticleSystem particles in this._particleSystems)
		{
			ParticleScaler.Scale(particles, scale, true, null);
		}
	}

	private void LateUpdate()
	{
		if (this.IsUpdate())
		{
			this.LateUpdateProcess();
		}
	}

	protected virtual void LateUpdateProcess()
	{
		this.CharacterFollowingUpdateInternal();
		this.BillboardObjectUpdateInternal();
	}

	protected virtual bool IsUpdate()
	{
		return false;
	}

	private void OnDisable()
	{
		foreach (ParticleController particleController in this._particleControllers)
		{
			particleController.RefleshPartiche();
			particleController.ClearParticle();
		}
	}
}
