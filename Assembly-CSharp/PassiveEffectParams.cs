using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Passive Effect Params")]
[DisallowMultipleComponent]
public class PassiveEffectParams : EffectParamsGeneric
{
	[SerializeField]
	private CharacterTarget _target = CharacterTarget.CharacterRoot;

	private IEnumerator _playAnimation;

	public void PlayAnimation(CharacterParams characterParams)
	{
		base.gameObject.SetActive(true);
		this._playAnimation = this.PlayAnimationCorutine(characterParams);
		base.StartCoroutine(this._playAnimation);
	}

	public IEnumerator PlayAnimationCorutine(CharacterParams characterParams)
	{
		if (this._effectAnimation == null)
		{
			yield break;
		}
		base.gameObject.SetActive(true);
		this.StopAnimationInternal();
		if (characterParams != null)
		{
			switch (this._target)
			{
			case CharacterTarget.CharacterCenter:
				this.SetPosition(characterParams.characterCenterTarget, null);
				break;
			case CharacterTarget.CharacterRoot:
				this.SetPosition(characterParams.transform, null);
				break;
			case CharacterTarget.CharacterFaceCenter:
				this.SetPosition(characterParams.characterFaceCenterTarget, null);
				break;
			case CharacterTarget.WorldRoot:
				this.SetPosition(null, null);
				break;
			}
		}
		this._isPlaying = true;
		this._effectAnimation.clip.wrapMode = WrapMode.Once;
		this._effectAnimation.PlayQueued(this._effectAnimation.clip.name, QueueMode.PlayNow);
		this.CharacterFollowingInitializeInternal(characterParams);
		this.BillboardObjectInitializeInternal(base.transform, characterParams.RootToCenterDistance());
		this.ParticheControllerInitializeInternal();
		while (this._effectAnimation.isPlaying)
		{
			yield return null;
		}
		this.StopAnimationInternal();
		yield break;
	}

	protected override void StopAnimationInternal()
	{
		if (this._playAnimation != null)
		{
			base.StopCoroutine(this._playAnimation);
			this._playAnimation = null;
		}
		base.StopAnimationInternal();
	}

	public void StopAnimation()
	{
		this.StopAnimationInternal();
		base.gameObject.SetActive(false);
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
}
