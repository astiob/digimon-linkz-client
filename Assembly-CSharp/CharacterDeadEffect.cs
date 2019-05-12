using System;
using System.Collections;
using UnityEngine;

public class CharacterDeadEffect
{
	private CharacterParams characterParams;

	private IEnumerator playDeadAnimation;

	public CharacterDeadEffect(CharacterParams characterParams)
	{
		this.characterParams = characterParams;
	}

	public void PlayAnimation(HitEffectParams hitEffectParams, float deathEffectGenerationInterval, Action callbackDeathEffect = null)
	{
		if (this.playDeadAnimation != null)
		{
			this.characterParams.StopCoroutine(this.playDeadAnimation);
		}
		this.playDeadAnimation = this.DeadEffectCorutine(hitEffectParams, deathEffectGenerationInterval, callbackDeathEffect);
		this.characterParams.StartCoroutine(this.playDeadAnimation);
	}

	private IEnumerator DeadEffectCorutine(HitEffectParams hitEffectParams, float deathEffectGenerationInterval, Action callbackDeathEffect = null)
	{
		float timing = Time.time + TimeExtension.GetTimeScaleDivided(deathEffectGenerationInterval);
		while (Time.time < timing)
		{
			yield return null;
		}
		if (hitEffectParams != null)
		{
			hitEffectParams.gameObject.SetActive(true);
			hitEffectParams.PlayAnimationTrigger(this.characterParams);
			if (callbackDeathEffect != null)
			{
				callbackDeathEffect();
			}
		}
		callbackDeathEffect = null;
		yield break;
	}

	public void StopAnimation()
	{
		if (this.playDeadAnimation != null)
		{
			this.characterParams.StopCoroutine(this.playDeadAnimation);
		}
	}
}
