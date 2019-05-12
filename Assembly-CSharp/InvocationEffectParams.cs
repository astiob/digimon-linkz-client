using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Digimon Effects/Invocation Effect Params")]
[DisallowMultipleComponent]
public class InvocationEffectParams : EffectParamsGeneric
{
	[SerializeField]
	private SkillType _attackAnimationType;

	[SerializeField]
	private int _motionIndex;

	[FormerlySerializedAs("cameraMotionId")]
	[SerializeField]
	private string _cameraMotionId;

	[SerializeField]
	[FormerlySerializedAs("hideStage")]
	private bool _hideStage;

	[FormerlySerializedAs("hideStageBackgroundColor")]
	[SerializeField]
	private Color _hideStageBackgroundColor = Color.black;

	[SerializeField]
	private Color _hideStageLightColor = Color.white;

	[SerializeField]
	private Vector3 _hideStageLightEulerAngles = new Vector3(-90f, 0f, 0f);

	[SerializeField]
	private InvocationEffectParams.AttachEffects[] _attachEffects;

	private GameObject _stageObject;

	private LightColorChanger _lightColorChanger;

	private Color currentStageColor;

	private Color currentSunLightColor;

	private Quaternion currentSunLightRotation;

	private bool isUpdate;

	private CharacterParams m_attacker;

	public SkillType attackAnimationType
	{
		get
		{
			return this._attackAnimationType;
		}
	}

	public string cameraMotionId
	{
		get
		{
			return this._cameraMotionId;
		}
	}

	public IEnumerator SkillInitialize(Camera renderCamera, GameObject stageObject, LightColorChanger lightColorChanger)
	{
		this._stageObject = stageObject;
		this._lightColorChanger = lightColorChanger;
		IEnumerator skillInitialize = base.Initialize(renderCamera);
		while (skillInitialize.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	public IEnumerator PlaySkillAnimation(CharacterParams attacker)
	{
		this.m_attacker = attacker;
		if (attacker != null)
		{
			this.isUpdate = true;
		}
		base.gameObject.SetActive(true);
		IEnumerator wait = this.PlaySkillAnimationInternal(attacker);
		while (wait.MoveNext())
		{
			yield return null;
		}
		this.StopSkillAnimation();
		this.isUpdate = false;
		yield break;
	}

	private IEnumerator PlaySkillAnimationInternal(CharacterParams attacker)
	{
		if (attacker != null)
		{
			if (this._cameraMotionId.Length > 0)
			{
				this.SetPosition(attacker.transform, null);
			}
			else
			{
				this.SetPosition(attacker.transform, new Vector3?(attacker.dropItemOffsetPosition));
			}
		}
		this.StopAnimationInternal();
		this._isPlaying = true;
		this.currentStageColor = Color.black;
		if (this._renderCamera != null)
		{
			this.currentStageColor = this._renderCamera.backgroundColor;
			if (this._hideStage)
			{
				if (this._lightColorChanger != null)
				{
					this._lightColorChanger.isEnable = false;
					this.currentSunLightColor = this._lightColorChanger.light.color;
					this._lightColorChanger.light.color = this._hideStageLightColor;
					this.currentSunLightRotation = this._lightColorChanger.light.transform.rotation;
					this._lightColorChanger.light.transform.rotation = Quaternion.Euler(this._hideStageLightEulerAngles);
				}
				if (this._stageObject != null)
				{
					this._stageObject.SetActive(false);
				}
				this._renderCamera.backgroundColor = this._hideStageBackgroundColor;
			}
		}
		this._effectAnimation.clip.wrapMode = WrapMode.Once;
		this._effectAnimation[this._effectAnimation.clip.name].time = 0f;
		this._effectAnimation.PlayQueued(this._effectAnimation.clip.name, QueueMode.PlayNow);
		if (attacker != null && attacker.GetFindAttackMotion(this._motionIndex))
		{
			attacker.PlayAttackAnimation(this._attackAnimationType, this._motionIndex);
			this.CharacterFollowingInitializeInternal(attacker);
			this.BillboardObjectInitializeInternal(attacker.transform, attacker.RootToCenterDistance());
			this.ParticheControllerInitializeInternal();
		}
		if (this._cameraMotionId.Length == 0)
		{
			this._scale = attacker.effectScale;
			base.transform.localScale = Vector3.one;
			base.transform.localScale *= this._scale;
			foreach (ParticleSystem particleSystem in this._particleSystems)
			{
				ParticleScaler.Scale(particleSystem, this._scale, true, null);
			}
		}
		if (this._attackAnimationType == SkillType.InheritanceTechnique)
		{
			float motionTime = (!(attacker != null)) ? 0f : attacker.AnimationClipLength;
			float effectTime = this._effectAnimation[this._effectAnimation.clip.name].length;
			float time = Mathf.Max(motionTime, effectTime);
			while (time > 0f)
			{
				time -= Time.deltaTime;
				if (!this._effectAnimation.isPlaying && this._effectAnimation.gameObject.activeInHierarchy)
				{
					this._effectAnimation.gameObject.SetActive(false);
				}
				yield return null;
			}
			this._effectAnimation.gameObject.SetActive(true);
		}
		else
		{
			while (this._effectAnimation.isPlaying)
			{
				yield return null;
			}
		}
		yield break;
	}

	public void AnimationKey()
	{
	}

	public void StopSkillAnimation()
	{
		if (!base.isPlaying)
		{
			return;
		}
		if (this._hideStage)
		{
			if (this._lightColorChanger != null)
			{
				this._lightColorChanger.light.color = this.currentSunLightColor;
				this._lightColorChanger.light.transform.rotation = this.currentSunLightRotation;
				this._lightColorChanger.isEnable = true;
			}
			if (this._stageObject != null)
			{
				this._stageObject.SetActive(true);
			}
			if (this._renderCamera != null)
			{
				this._renderCamera.backgroundColor = this.currentStageColor;
			}
		}
		this.StopAnimationInternal();
		base.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (this.isUpdate && this.m_attacker != null && this.m_attacker.attachEffectLocators != null && this._attachEffects != null)
		{
			foreach (InvocationEffectParams.AttachEffects attachEffects2 in this._attachEffects)
			{
				if (this.m_attacker.attachEffectLocators.ContainsKey(attachEffects2.name) && attachEffects2.effectTransform != null)
				{
					Transform transform = this.m_attacker.attachEffectLocators[attachEffects2.name];
					if (transform != null)
					{
						attachEffects2.effectTransform.position = transform.position;
						attachEffects2.effectTransform.rotation = transform.rotation;
						attachEffects2.effectTransform.localScale = transform.localScale;
					}
				}
			}
		}
	}

	protected override void LateUpdateProcess()
	{
		this.CharacterFollowingUpdateInternal();
		this.BillboardObjectUpdateInternal();
	}

	protected override bool IsUpdate()
	{
		return this._effectAnimation != null && this._effectAnimation.isPlaying;
	}

	[Serializable]
	public class AttachEffects
	{
		[SerializeField]
		private string _name = string.Empty;

		[SerializeField]
		private Transform _effectTransform;

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public Transform effectTransform
		{
			get
			{
				return this._effectTransform;
			}
		}
	}
}
