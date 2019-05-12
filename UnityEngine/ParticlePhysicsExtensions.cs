using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Method extension for Physics in Particle System.</para>
	/// </summary>
	public static class ParticlePhysicsExtensions
	{
		/// <summary>
		///   <para>Safe array size for use with ParticleSystem.GetCollisionEvents.</para>
		/// </summary>
		/// <param name="ps"></param>
		public static int GetSafeCollisionEventSize(this ParticleSystem ps)
		{
			return ParticleSystemExtensionsImpl.GetSafeCollisionEventSize(ps);
		}

		/// <summary>
		///   <para>Get the particle collision events for a GameObject. Returns the number of events written to the array.</para>
		/// </summary>
		/// <param name="go">The GameObject for which to retrieve collision events.</param>
		/// <param name="collisionEvents">Array to write collision events to.</param>
		/// <param name="ps"></param>
		public static int GetCollisionEvents(this ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents)
		{
			return ParticleSystemExtensionsImpl.GetCollisionEvents(ps, go, collisionEvents);
		}
	}
}
