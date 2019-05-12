using System;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[AddComponentMenu("Digimon Effects/Tools/Always Effect Params Relay")]
public class AlwaysEffectParamsRelay : MonoBehaviour
{
	private AlwaysEffectParams _parentParams;

	private AlwaysEffectState _nextState;

	private void Awake()
	{
		this.OnEnable();
	}

	private void OnEnable()
	{
		if (this._parentParams == null)
		{
			this._parentParams = base.GetComponentInParent<AlwaysEffectParams>();
		}
	}

	public void SetState(AlwaysEffectState state)
	{
		this._nextState = state;
	}

	public void AnimationEnd()
	{
		this.OnEnable();
		if (this._parentParams == null)
		{
			global::Debug.LogException(new NullReferenceException());
			return;
		}
		this._parentParams.SetState(this._nextState);
	}
}
