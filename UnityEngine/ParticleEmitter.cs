using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>(Legacy Particles) Script interface for particle emitters.</para>
	/// </summary>
	public class ParticleEmitter : Component
	{
		internal ParticleEmitter()
		{
		}

		/// <summary>
		///   <para>Should particles be automatically emitted each frame?</para>
		/// </summary>
		public extern bool emit { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The minimum size each particle can be at the time when it is spawned.</para>
		/// </summary>
		public extern float minSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum size each particle can be at the time when it is spawned.</para>
		/// </summary>
		public extern float maxSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The minimum lifetime of each particle, measured in seconds.</para>
		/// </summary>
		public extern float minEnergy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum lifetime of each particle, measured in seconds.</para>
		/// </summary>
		public extern float maxEnergy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The minimum number of particles that will be spawned every second.</para>
		/// </summary>
		public extern float minEmission { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum number of particles that will be spawned every second.</para>
		/// </summary>
		public extern float maxEmission { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The amount of the emitter's speed that the particles inherit.</para>
		/// </summary>
		public extern float emitterVelocityScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The starting speed of particles in world space, along X, Y, and Z.</para>
		/// </summary>
		public Vector3 worldVelocity
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_worldVelocity(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_worldVelocity(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_worldVelocity(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_worldVelocity(ref Vector3 value);

		/// <summary>
		///   <para>The starting speed of particles along X, Y, and Z, measured in the object's orientation.</para>
		/// </summary>
		public Vector3 localVelocity
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_localVelocity(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localVelocity(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localVelocity(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localVelocity(ref Vector3 value);

		/// <summary>
		///   <para>A random speed along X, Y, and Z that is added to the velocity.</para>
		/// </summary>
		public Vector3 rndVelocity
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_rndVelocity(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rndVelocity(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rndVelocity(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rndVelocity(ref Vector3 value);

		/// <summary>
		///   <para>If enabled, the particles don't move when the emitter moves. If false, when you move the emitter, the particles follow it around.</para>
		/// </summary>
		public extern bool useWorldSpace { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>If enabled, the particles will be spawned with random rotations.</para>
		/// </summary>
		public extern bool rndRotation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The angular velocity of new particles in degrees per second.</para>
		/// </summary>
		public extern float angularVelocity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A random angular velocity modifier for new particles.</para>
		/// </summary>
		public extern float rndAngularVelocity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns a copy of all particles and assigns an array of all particles to be the current particles.</para>
		/// </summary>
		public extern Particle[] particles { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The current number of particles (Read Only).</para>
		/// </summary>
		public extern int particleCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Removes all particles from the particle emitter.</para>
		/// </summary>
		public void ClearParticles()
		{
			ParticleEmitter.INTERNAL_CALL_ClearParticles(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ClearParticles(ParticleEmitter self);

		/// <summary>
		///   <para>Emit a number of particles.</para>
		/// </summary>
		public void Emit()
		{
			this.Emit2((int)Random.Range(this.minEmission, this.maxEmission));
		}

		/// <summary>
		///   <para>Emit count particles immediately.</para>
		/// </summary>
		/// <param name="count"></param>
		public void Emit(int count)
		{
			this.Emit2(count);
		}

		/// <summary>
		///   <para>Emit a single particle with given parameters.</para>
		/// </summary>
		/// <param name="pos">The position of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		/// <param name="size">The size of the particle.</param>
		/// <param name="energy">The remaining lifetime of the particle.</param>
		/// <param name="color">The color of the particle.</param>
		public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color)
		{
			InternalEmitParticleArguments internalEmitParticleArguments = default(InternalEmitParticleArguments);
			internalEmitParticleArguments.pos = pos;
			internalEmitParticleArguments.velocity = velocity;
			internalEmitParticleArguments.size = size;
			internalEmitParticleArguments.energy = energy;
			internalEmitParticleArguments.color = color;
			internalEmitParticleArguments.rotation = 0f;
			internalEmitParticleArguments.angularVelocity = 0f;
			this.Emit3(ref internalEmitParticleArguments);
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="rotation">The initial rotation of the particle in degrees.</param>
		/// <param name="angularVelocity">The angular velocity of the particle in degrees per second.</param>
		/// <param name="pos"></param>
		/// <param name="velocity"></param>
		/// <param name="size"></param>
		/// <param name="energy"></param>
		/// <param name="color"></param>
		public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color, float rotation, float angularVelocity)
		{
			InternalEmitParticleArguments internalEmitParticleArguments = default(InternalEmitParticleArguments);
			internalEmitParticleArguments.pos = pos;
			internalEmitParticleArguments.velocity = velocity;
			internalEmitParticleArguments.size = size;
			internalEmitParticleArguments.energy = energy;
			internalEmitParticleArguments.color = color;
			internalEmitParticleArguments.rotation = rotation;
			internalEmitParticleArguments.angularVelocity = angularVelocity;
			this.Emit3(ref internalEmitParticleArguments);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Emit2(int count);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Emit3(ref InternalEmitParticleArguments args);

		/// <summary>
		///   <para>Advance particle simulation by given time.</para>
		/// </summary>
		/// <param name="deltaTime"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Simulate(float deltaTime);

		/// <summary>
		///   <para>Turns the ParticleEmitter on or off.</para>
		/// </summary>
		public extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
