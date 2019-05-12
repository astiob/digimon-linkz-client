using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Digimon Effects/Always Effect Params")]
public class AlwaysEffectParams : EffectParamsGeneric
{
	[SerializeField]
	private AnimationClip _inAnimationClip;

	[SerializeField]
	private AnimationClip _alwaysAnimationClip;

	[SerializeField]
	private AnimationClip _outAnimationClip;

	[SerializeField]
	private string _inSeId = string.Empty;

	[SerializeField]
	private string _alwaysSeId = string.Empty;

	[SerializeField]
	private string _outSeId = string.Empty;

	[SerializeField]
	private Transform _targetPosition;

	private IEnumerator _playAnimation;

	private bool _onBillboard;

	private AlwaysEffectState _currentState;

	public string inSeId
	{
		get
		{
			return this._inSeId;
		}
	}

	public string alwaysSeId
	{
		get
		{
			return this._alwaysSeId;
		}
	}

	public string outSeId
	{
		get
		{
			return this._outSeId;
		}
	}

	public AlwaysEffectState currentState
	{
		get
		{
			return this._currentState;
		}
	}

	public Transform targetPosition
	{
		get
		{
			return this._targetPosition;
		}
	}

	public void PlayAnimationTrigger(AlwaysEffectState effectState)
	{
		base.gameObject.SetActive(true);
		if (this._playAnimation != null)
		{
			base.StopCoroutine(this._playAnimation);
		}
		this._currentState = effectState;
		this._playAnimation = this.PlayAnimation(effectState, null, 0f);
		base.StartCoroutine(this._playAnimation);
	}

	private IEnumerator PlayAnimation(AlwaysEffectState effectState, Transform position = null, float distance = 0f)
	{
		this._isPlaying = true;
		Transform _position = base.transform;
		if (position != null)
		{
			_position = position;
		}
		if (effectState == AlwaysEffectState.In)
		{
			this.BillboardObjectInitializeInternal(_position, distance);
			this._effectAnimation.clip = this._inAnimationClip;
			this._effectAnimation.PlayQueued(this._effectAnimation.clip.name, QueueMode.PlayNow);
			this._effectAnimation.CrossFadeQueued(this._alwaysAnimationClip.name, 0.5f, QueueMode.CompleteOthers);
			this._onBillboard = true;
			yield break;
		}
		if (effectState != AlwaysEffectState.Always)
		{
			this._effectAnimation.clip = this._outAnimationClip;
			this._effectAnimation.CrossFadeQueued(this._effectAnimation.clip.name, 0.5f, QueueMode.PlayNow);
			this._onBillboard = true;
			while (this._effectAnimation.isPlaying)
			{
				yield return null;
			}
			this._effectAnimation[this._effectAnimation.clip.name].normalizedTime = 1f;
			this._onBillboard = false;
			this._isPlaying = false;
			yield break;
		}
		if (!this._effectAnimation.isPlaying || this._alwaysAnimationClip != this._effectAnimation.clip)
		{
			this.BillboardObjectInitializeInternal(_position, distance);
			this._effectAnimation.clip = this._alwaysAnimationClip;
			this._effectAnimation.PlayQueued(this._effectAnimation.clip.name, QueueMode.PlayNow);
			this._onBillboard = true;
		}
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
		this._onBillboard = false;
		this._effectAnimation.clip = this._outAnimationClip;
		this.StopAnimationInternal();
		base.gameObject.SetActive(false);
	}

	public void SetState(AlwaysEffectState state)
	{
		this._currentState = state;
	}

	protected override void LateUpdateProcess()
	{
		this.BillboardObjectUpdateInternal();
	}

	protected override bool IsUpdate()
	{
		return this._isPlaying && this._onBillboard;
	}
}
