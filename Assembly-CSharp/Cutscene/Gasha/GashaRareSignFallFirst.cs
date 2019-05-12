using Monster;
using System;
using System.Collections;
using UnityEngine;

namespace Cutscene.Gasha
{
	public sealed class GashaRareSignFallFirst : GashaAnimationBase, IGashaAnimation
	{
		public ParticleSystem rareSignParticleSpark;

		public Animator rareSignAnimation;

		public Animator subCameraAnimation;

		public CutsceneSound sound;

		public ParticleSystem shockWaveparticle;

		public ParticleSystem circleParticleAppear;

		public ParticleSystem auraParticleBlue;

		public ParticleSystem auraParticleYellow;

		public ParticleSystem auraParticleRed;

		public string growStep;

		public IEnumerator StartAnimation()
		{
			this.sound.PlaySE("SEInternal/Cutscene/se_209");
			this.shockWaveparticle.Play();
			this.circleParticleAppear.Play();
			yield return new WaitForSeconds(1.5f);
			this.sound.PlaySE("SEInternal/Cutscene/se_210");
			if (MonsterGrowStepData.IsRipeScope(this.growStep))
			{
				this.auraParticleYellow.Play();
			}
			else if (MonsterGrowStepData.IsGrowStepHigh(this.growStep))
			{
				this.auraParticleRed.Play();
			}
			else
			{
				this.auraParticleBlue.Play();
			}
			yield return new WaitForSeconds(0.9f);
			this.rareSignParticleSpark.Stop();
			yield return new WaitForSeconds(1.2f);
			base.EndCallback();
			yield break;
		}

		public void SkipAnimation()
		{
			GashaAnimationCommon.PlayParticle(this.shockWaveparticle);
			GashaAnimationCommon.PlayParticle(this.circleParticleAppear);
			GashaAnimationCommon.StopParticle(this.rareSignParticleSpark);
			GashaAnimationCommon.StopParticle(this.auraParticleBlue);
			GashaAnimationCommon.StopParticle(this.auraParticleYellow);
			GashaAnimationCommon.StopParticle(this.auraParticleRed);
			this.sound.StopAllSE();
			this.rareSignAnimation.SetTrigger("stop");
			this.subCameraAnimation.SetTrigger("stop");
		}
	}
}
