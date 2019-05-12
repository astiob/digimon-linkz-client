using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/GlitchEffect")]
	public class GlitchEffect : ImageEffectBase
	{
		public Texture2D displacementMap;

		private float glitchup;

		private float glitchdown;

		private float flicker;

		private float glitchupTime = 0.05f;

		private float glitchdownTime = 0.05f;

		private float flickerTime = 0.5f;

		public float intensity;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetFloat("_Intensity", this.intensity);
			base.material.SetTexture("_DispTex", this.displacementMap);
			this.glitchup += Time.deltaTime * this.intensity;
			this.glitchdown += Time.deltaTime * this.intensity;
			this.flicker += Time.deltaTime * this.intensity;
			if (this.flicker > this.flickerTime)
			{
				base.material.SetFloat("filterRadius", UnityEngine.Random.Range(-3f, 3f) * this.intensity);
				this.flicker = 0f;
				this.flickerTime = UnityEngine.Random.value;
			}
			if (this.glitchup > this.glitchupTime)
			{
				if (UnityEngine.Random.value < 0.1f * this.intensity)
				{
					base.material.SetFloat("flip_up", UnityEngine.Random.Range(0f, 1f) * this.intensity);
				}
				else
				{
					base.material.SetFloat("flip_up", 0f);
				}
				this.glitchup = 0f;
				this.glitchupTime = UnityEngine.Random.value / 10f;
			}
			if (this.glitchdown > this.glitchdownTime)
			{
				if (UnityEngine.Random.value < 0.1f * this.intensity)
				{
					base.material.SetFloat("flip_down", 1f - UnityEngine.Random.Range(0f, 1f) * this.intensity);
				}
				else
				{
					base.material.SetFloat("flip_down", 1f);
				}
				this.glitchdown = 0f;
				this.glitchdownTime = UnityEngine.Random.value / 10f;
			}
			if ((double)UnityEngine.Random.value < 0.05 * (double)this.intensity)
			{
				base.material.SetFloat("displace", UnityEngine.Random.value * this.intensity);
				base.material.SetFloat("scale", 1f - UnityEngine.Random.value * this.intensity);
			}
			else
			{
				base.material.SetFloat("displace", 0f);
			}
			Graphics.Blit(source, destination, base.material);
		}
	}
}
