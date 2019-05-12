using System;
using UnityEngine;

public static class ParticleScaler
{
	public static ParticleScalerOptions defaultOptions = new ParticleScalerOptions();

	public static void ScaleByTransform(ParticleSystem particles, float scale, bool includeChildren = true)
	{
		particles.transform.localScale = particles.transform.localScale * scale;
		ParticleSystem.MainModule mainModule;
		particles.main.gravityModifierMultiplier = mainModule.gravityModifierMultiplier * scale;
		if (includeChildren)
		{
			ParticleSystem[] componentsInChildren = particles.GetComponentsInChildren<ParticleSystem>();
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				if (!(componentsInChildren[num] == particles))
				{
					componentsInChildren[num].transform.localScale = componentsInChildren[num].transform.localScale * scale;
					ParticleSystem.MainModule mainModule2;
					componentsInChildren[num].main.gravityModifierMultiplier = mainModule2.gravityModifierMultiplier * scale;
				}
			}
		}
	}

	public static void Scale(ParticleSystem particles, float scale, bool includeChildren = true, ParticleScalerOptions options = null)
	{
		ParticleScaler.ScaleSystem(particles, scale, false, options);
		if (includeChildren)
		{
			ParticleSystem[] componentsInChildren = particles.GetComponentsInChildren<ParticleSystem>();
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				if (!(componentsInChildren[num] == particles))
				{
					ParticleScaler.ScaleSystem(componentsInChildren[num], scale, true, options);
				}
			}
		}
	}

	private static void ScaleSystem(ParticleSystem particles, float scale, bool scalePosition, ParticleScalerOptions options = null)
	{
		if (options == null)
		{
			options = ParticleScaler.defaultOptions;
		}
		if (scalePosition)
		{
			particles.transform.localPosition *= scale;
		}
		ParticleSystem.MainModule main = particles.main;
		main.startSizeMultiplier *= scale;
		main.gravityModifierMultiplier *= scale;
		main.startSpeedMultiplier *= scale;
	}

	private static void ScaleCurve(AnimationCurve curve, float scale)
	{
		if (curve == null)
		{
			return;
		}
		for (int i = 0; i < curve.keys.Length; i++)
		{
			Keyframe[] keys = curve.keys;
			int num = i;
			keys[num].value = keys[num].value * scale;
		}
	}
}
