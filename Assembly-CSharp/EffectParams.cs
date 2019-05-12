using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[AddComponentMenu("Digimon Effects/Effect Params")]
public class EffectParams : EffectParamsGeneric
{
	[FormerlySerializedAs("autoDisable")]
	[SerializeField]
	private bool _autoDisable;

	protected IEnumerator _PlayAnimation;

	public bool autoDisable
	{
		get
		{
			return this._autoDisable;
		}
		set
		{
			this._autoDisable = value;
		}
	}

	public void PlayAnimationTrigger(Transform position = null, float distance = 0f)
	{
		this.StopAnimationInternal();
		this._PlayAnimation = this.PlayAnimation(position, distance);
		base.StartCoroutine(this._PlayAnimation);
	}

	protected IEnumerator PlayAnimation(Transform position = null, float distance = 0f)
	{
		this._isPlaying = true;
		this.SetPosition(position, null);
		this.BillboardObjectInitializeInternal(position, distance);
		this.ParticheControllerInitializeInternal();
		this._effectAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
		this._effectAnimation[this._effectAnimation.clip.name].time = 0f;
		this._effectAnimation.Play();
		while (this._effectAnimation.isPlaying)
		{
			yield return null;
		}
		this._isPlaying = false;
		if (this._autoDisable)
		{
			base.gameObject.SetActive(false);
		}
		yield break;
	}

	protected virtual void IsPlayingUpdate()
	{
	}

	protected override void StopAnimationInternal()
	{
		if (this._PlayAnimation != null)
		{
			base.StopCoroutine(this._PlayAnimation);
			this._PlayAnimation = null;
		}
		base.StopAnimationInternal();
	}

	public virtual void StopAnimation()
	{
		this.StopAnimationInternal();
	}

	protected override void LateUpdateProcess()
	{
		this.IsPlayingUpdate();
		this.BillboardObjectUpdateInternal();
	}

	protected override bool IsUpdate()
	{
		return this._effectAnimation != null && this._effectAnimation.isPlaying;
	}
}
