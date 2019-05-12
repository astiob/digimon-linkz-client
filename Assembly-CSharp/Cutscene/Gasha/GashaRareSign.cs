using System;
using System.Collections;
using UnityEngine;

namespace Cutscene.Gasha
{
	public sealed class GashaRareSign : GashaAnimationBase, IGashaAnimation
	{
		public ParticleSystem rareSignParticleSpark;

		public IEnumerator StartAnimation()
		{
			this.rareSignParticleSpark.Play();
			yield return new WaitForSeconds(0.5f);
			base.EndCallback();
			yield break;
		}

		public void SkipAnimation()
		{
		}
	}
}
