using System;

namespace UnityEngine
{
	[Flags]
	public enum ParticleSystemSubEmitterProperties
	{
		InheritNothing = 0,
		InheritEverything = 15,
		InheritColor = 1,
		InheritSize = 2,
		InheritRotation = 4,
		InheritLifetime = 8
	}
}
