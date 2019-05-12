using System;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
[AddComponentMenu("Digimon Effects/Hit Effect Params")]
public class HitEffectParams : EffectParams
{
	[SerializeField]
	[FormerlySerializedAs("target")]
	private CharacterTarget _target;

	[SerializeField]
	private string _seId = string.Empty;

	private CharacterParams cachedCharacter;

	public string seId
	{
		get
		{
			return this._seId;
		}
	}

	public void PlayAnimationTrigger(CharacterParams param)
	{
		this.StopAnimation();
		base.gameObject.SetActive(true);
		if (this._target == CharacterTarget.CharacterCenter)
		{
			this._PlayAnimation = base.PlayAnimation(param.characterCenterTarget, param.RootToCenterDistance());
		}
		else if (this._target == CharacterTarget.CharacterFaceCenter)
		{
			this._PlayAnimation = base.PlayAnimation(param.characterFaceCenterTarget, param.RootToCenterDistance());
		}
		else if (this._target == CharacterTarget.CharacterRoot)
		{
			this._PlayAnimation = base.PlayAnimation(param.transform, param.RootToCenterDistance());
		}
		else
		{
			this._PlayAnimation = base.PlayAnimation(null, param.RootToCenterDistance());
		}
		this.CharacterFollowingInitializeInternal(param);
		this.cachedCharacter = param;
		base.StartCoroutine(this._PlayAnimation);
		this._scale = this.cachedCharacter.effectScale;
		base.transform.localScale = Vector3.one;
		base.transform.localScale *= this._scale;
		foreach (ParticleSystem particles in this._particleSystems)
		{
			ParticleScaler.Scale(particles, this._scale, true, null);
		}
	}

	protected override void IsPlayingUpdate()
	{
		this.CharacterFollowingUpdateInternal();
		this.RotationFixerUpdateInternal(this.cachedCharacter);
	}

	protected override void StopAnimationInternal()
	{
		base.StopAnimationInternal();
	}

	public override void StopAnimation()
	{
		base.StopAnimation();
		base.gameObject.SetActive(false);
	}
}
