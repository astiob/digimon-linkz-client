using System;
using UnityEngine;

namespace Cutscene.Gasha
{
	public static class GashaAnimationCommon
	{
		public static void SetTextureAlpha(UITexture[] textureList, float alpha)
		{
			for (int i = 0; i < textureList.Length; i++)
			{
				textureList[i].alpha = alpha;
			}
		}

		public static void StopParticle(ParticleSystem particle)
		{
			if (particle.isPlaying)
			{
				particle.Clear();
				particle.Stop();
			}
		}

		public static void PlayParticle(ParticleSystem particle)
		{
			if (!particle.isPlaying)
			{
				particle.Play();
			}
		}
	}
}
