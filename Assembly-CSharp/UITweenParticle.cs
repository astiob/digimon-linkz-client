using System;
using UnityEngine;

public class UITweenParticle : UITweener
{
	private const float Threshold = 0.5f;

	[SerializeField]
	private ParticleSystem[] _particleSystems;

	[SerializeField]
	private TrailRenderer[] _trailRenderers;

	private bool currentEnable;

	protected override void Start()
	{
		foreach (ParticleSystem particleSystem in this._particleSystems)
		{
			particleSystem.Clear();
		}
		foreach (TrailRenderer trailRenderer in this._trailRenderers)
		{
			trailRenderer.enabled = false;
		}
		base.Start();
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		bool flag = this.currentEnable;
		bool flag2 = this.currentEnable;
		if (flag2)
		{
			if (factor < 0.5f)
			{
				this.currentEnable = !this.currentEnable;
			}
		}
		else if (factor >= 0.5f)
		{
			this.currentEnable = !this.currentEnable;
		}
		if (flag != this.currentEnable)
		{
			this.SetEnabledRenderers(this.currentEnable);
		}
	}

	private void SetEnabledRenderers(bool isEnable)
	{
		foreach (ParticleSystem particleSystem in this._particleSystems)
		{
			if (!isEnable)
			{
				particleSystem.Stop();
			}
			else
			{
				particleSystem.Play();
			}
		}
		foreach (TrailRenderer trailRenderer in this._trailRenderers)
		{
			trailRenderer.enabled = isEnable;
		}
	}

	public override void ApplyTweenFromValue()
	{
		this.OnUpdate(0f, false);
	}

	public override void ApplyTweenToValue()
	{
		this.OnUpdate(1f, false);
	}
}
