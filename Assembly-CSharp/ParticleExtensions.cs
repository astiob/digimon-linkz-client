using System;
using UnityEngine;

public static class ParticleExtensions
{
	public static void Scale(this ParticleSystem particles, float scale, bool includeChildren = true)
	{
		ParticleScaler.Scale(particles, scale, includeChildren, null);
	}

	public static void ScaleByTransform(this ParticleSystem particles, float scale, bool includeChildren = true)
	{
		ParticleScaler.ScaleByTransform(particles, scale, includeChildren);
	}
}
