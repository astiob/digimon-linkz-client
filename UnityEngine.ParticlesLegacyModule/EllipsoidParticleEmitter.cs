using System;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	[Obsolete("This component is part of the legacy particle system, which is deprecated and will be removed in a future release. Use the ParticleSystem component instead.", false)]
	public sealed class EllipsoidParticleEmitter : ParticleEmitter
	{
		internal EllipsoidParticleEmitter()
		{
		}
	}
}
