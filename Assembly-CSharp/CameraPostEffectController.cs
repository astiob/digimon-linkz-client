using System;
using UnityEngine;

public class CameraPostEffectController : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float _effectLevel;

	[NonSerialized]
	private CameraPostEffect _postEffect;

	public void Initialize(CameraPostEffect postEffect)
	{
		this._postEffect = postEffect;
	}

	private void Update()
	{
		this.ManualUpdate();
	}

	public void ManualUpdate()
	{
		if (this._postEffect == null)
		{
			return;
		}
		this._postEffect.effectLevel = this._effectLevel;
	}
}
