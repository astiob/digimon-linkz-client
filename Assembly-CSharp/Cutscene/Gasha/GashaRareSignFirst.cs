using System;
using System.Collections;
using UnityEngine;

namespace Cutscene.Gasha
{
	public sealed class GashaRareSignFirst : GashaAnimationBase, IGashaAnimation
	{
		public ParticleSystem rareSignParticleRect;

		public ParticleSystem rareSignParticleSpark;

		public IEnumerator StartAnimation()
		{
			this.rareSignParticleRect.Play();
			yield return new WaitForSeconds(1f);
			this.rareSignParticleSpark.Play();
			yield return new WaitForSeconds(0.7f);
			this.rareSignParticleRect.Stop();
			base.EndCallback();
			yield break;
		}

		public void SkipAnimation()
		{
			GashaAnimationCommon.StopParticle(this.rareSignParticleRect);
		}
	}
}
