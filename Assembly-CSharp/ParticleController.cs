using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Particle Controller")]
[DisallowMultipleComponent]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
	[SerializeField]
	private bool _onEmitParticle;

	private ParticleSystem _particleSystem;

	private bool _onEmitParticleCache;

	private void Awake()
	{
		this._particleSystem = base.GetComponent<ParticleSystem>();
		this._particleSystem.playOnAwake = false;
		this._onEmitParticleCache = !this._onEmitParticle;
	}

	private void OnEnable()
	{
		this.UpdateParticle();
	}

	private void LateUpdate()
	{
		if (this._onEmitParticleCache != this._onEmitParticle)
		{
			this.RefleshPartiche();
			this._onEmitParticleCache = this._onEmitParticle;
		}
	}

	private void UpdateParticle()
	{
		if (this._particleSystem == null)
		{
			this.Awake();
		}
		if (this._particleSystem != null)
		{
			if (this._onEmitParticle)
			{
				this._particleSystem.Play();
			}
			else
			{
				this._particleSystem.Stop();
			}
		}
	}

	public void ClearParticle()
	{
		if (this._particleSystem != null)
		{
			this._particleSystem.Stop();
			this._particleSystem.Clear();
		}
	}

	public void RefleshPartiche()
	{
		this.UpdateParticle();
	}
}
