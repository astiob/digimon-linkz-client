using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Script interface for particle systems (Shuriken).</para>
	/// </summary>
	public sealed class ParticleSystem : Component
	{
		/// <summary>
		///   <para>Start delay in seconds.</para>
		/// </summary>
		public extern float startDelay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is the particle system playing right now ?</para>
		/// </summary>
		public extern bool isPlaying { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the particle system stopped right now ?</para>
		/// </summary>
		public extern bool isStopped { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the particle system paused right now ?</para>
		/// </summary>
		public extern bool isPaused { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the particle system looping?</para>
		/// </summary>
		public extern bool loop { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>If set to true, the particle system will automatically start playing on startup.</para>
		/// </summary>
		public extern bool playOnAwake { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Playback position in seconds.</para>
		/// </summary>
		public extern float time { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The duration of the particle system in seconds (Read Only).</para>
		/// </summary>
		public extern float duration { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The playback speed of the particle system. 1 is normal playback speed.</para>
		/// </summary>
		public extern float playbackSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The current number of particles (Read Only).</para>
		/// </summary>
		public extern int particleCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>When set to false, the particle system will not emit particles.</para>
		/// </summary>
		public extern bool enableEmission { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The rate of emission.</para>
		/// </summary>
		public extern float emissionRate { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The initial speed of particles when emitted. When using curves, this values acts as a scale on the curve.</para>
		/// </summary>
		public extern float startSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The initial size of particles when emitted. When using curves, this values acts as a scale on the curve.</para>
		/// </summary>
		public extern float startSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The initial color of particles when emitted.</para>
		/// </summary>
		public Color startColor
		{
			get
			{
				Color result;
				this.INTERNAL_get_startColor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_startColor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_startColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_startColor(ref Color value);

		/// <summary>
		///   <para>The initial rotation of particles when emitted. When using curves, this values acts as a scale on the curve.</para>
		/// </summary>
		public extern float startRotation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The total lifetime in seconds that particles will have when emitted. When using curves, this values acts as a scale on the curve. This value is set in the particle when it is create by the particle system.</para>
		/// </summary>
		public extern float startLifetime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Scale being applied to the gravity defined by Physics.gravity.</para>
		/// </summary>
		public extern float gravityModifier { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum number of particles to emit.</para>
		/// </summary>
		public extern int maxParticles { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>This selects the space in which to simulate particles. It can be either world or local space.</para>
		/// </summary>
		public extern ParticleSystemSimulationSpace simulationSpace { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Random seed used for the particle system emission. If set to 0, it will be assigned a random value on awake.</para>
		/// </summary>
		public extern uint randomSeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetParticles(ParticleSystem.Particle[] particles, int size);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetParticles(ParticleSystem.Particle[] particles);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Simulate(float t, bool restart);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Play();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Stop();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Pause();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Clear();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_IsAlive();

		/// <summary>
		///   <para>Fastforwards the particle system by simulating particles over given period of time, then pauses it.</para>
		/// </summary>
		/// <param name="t">Time to fastforward the particle system.</param>
		/// <param name="withChildren">Fastforward all child particle systems as well.</param>
		/// <param name="restart">Restart and start from the beginning.</param>
		[ExcludeFromDocs]
		public void Simulate(float t, bool withChildren)
		{
			bool restart = true;
			this.Simulate(t, withChildren, restart);
		}

		/// <summary>
		///   <para>Fastforwards the particle system by simulating particles over given period of time, then pauses it.</para>
		/// </summary>
		/// <param name="t">Time to fastforward the particle system.</param>
		/// <param name="withChildren">Fastforward all child particle systems as well.</param>
		/// <param name="restart">Restart and start from the beginning.</param>
		[ExcludeFromDocs]
		public void Simulate(float t)
		{
			bool restart = true;
			bool withChildren = true;
			this.Simulate(t, withChildren, restart);
		}

		/// <summary>
		///   <para>Fastforwards the particle system by simulating particles over given period of time, then pauses it.</para>
		/// </summary>
		/// <param name="t">Time to fastforward the particle system.</param>
		/// <param name="withChildren">Fastforward all child particle systems as well.</param>
		/// <param name="restart">Restart and start from the beginning.</param>
		public void Simulate(float t, [DefaultValue("true")] bool withChildren, [DefaultValue("true")] bool restart)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = ParticleSystem.GetParticleSystems(this);
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					particleSystem.Internal_Simulate(t, restart);
				}
			}
			else
			{
				this.Internal_Simulate(t, restart);
			}
		}

		[ExcludeFromDocs]
		public void Play()
		{
			bool withChildren = true;
			this.Play(withChildren);
		}

		/// <summary>
		///   <para>Plays the particle system.</para>
		/// </summary>
		/// <param name="withChildren">Play all child particle systems as well.</param>
		public void Play([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = ParticleSystem.GetParticleSystems(this);
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					particleSystem.Internal_Play();
				}
			}
			else
			{
				this.Internal_Play();
			}
		}

		[ExcludeFromDocs]
		public void Stop()
		{
			bool withChildren = true;
			this.Stop(withChildren);
		}

		/// <summary>
		///   <para>Stops playing the particle system.</para>
		/// </summary>
		/// <param name="withChildren">Stop all child particle systems as well.</param>
		public void Stop([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = ParticleSystem.GetParticleSystems(this);
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					particleSystem.Internal_Stop();
				}
			}
			else
			{
				this.Internal_Stop();
			}
		}

		[ExcludeFromDocs]
		public void Pause()
		{
			bool withChildren = true;
			this.Pause(withChildren);
		}

		/// <summary>
		///   <para>Pauses playing the particle system.</para>
		/// </summary>
		/// <param name="withChildren">Pause all child particle systems as well.</param>
		public void Pause([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = ParticleSystem.GetParticleSystems(this);
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					particleSystem.Internal_Pause();
				}
			}
			else
			{
				this.Internal_Pause();
			}
		}

		[ExcludeFromDocs]
		public void Clear()
		{
			bool withChildren = true;
			this.Clear(withChildren);
		}

		/// <summary>
		///   <para>Remove all particles in the particle system.</para>
		/// </summary>
		/// <param name="withChildren">Clear all child particle systems as well.</param>
		public void Clear([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = ParticleSystem.GetParticleSystems(this);
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					particleSystem.Internal_Clear();
				}
			}
			else
			{
				this.Internal_Clear();
			}
		}

		[ExcludeFromDocs]
		public bool IsAlive()
		{
			bool withChildren = true;
			return this.IsAlive(withChildren);
		}

		/// <summary>
		///   <para>Does the system have any live particles (or will produce more)?</para>
		/// </summary>
		/// <param name="withChildren">Check all child particle systems as well.</param>
		/// <returns>
		///   <para>True if the particle system is still "alive", false if the particle system is done emitting particles and all particles are dead.</para>
		/// </returns>
		public bool IsAlive([DefaultValue("true")] bool withChildren)
		{
			if (withChildren)
			{
				ParticleSystem[] particleSystems = ParticleSystem.GetParticleSystems(this);
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					if (particleSystem.Internal_IsAlive())
					{
						return true;
					}
				}
				return false;
			}
			return this.Internal_IsAlive();
		}

		/// <summary>
		///   <para>Emit count particles immediately.</para>
		/// </summary>
		/// <param name="count"></param>
		public void Emit(int count)
		{
			ParticleSystem.INTERNAL_CALL_Emit(this, count);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Emit(ParticleSystem self, int count);

		/// <summary>
		///   <para>Emit a single particle with given parameters.</para>
		/// </summary>
		/// <param name="position">The position of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		/// <param name="size">The size of the particle.</param>
		/// <param name="lifetime">The remaining lifetime of the particle.</param>
		/// <param name="color">The color of the particle.</param>
		public void Emit(Vector3 position, Vector3 velocity, float size, float lifetime, Color32 color)
		{
			ParticleSystem.Particle particle = default(ParticleSystem.Particle);
			particle.position = position;
			particle.velocity = velocity;
			particle.lifetime = lifetime;
			particle.startLifetime = lifetime;
			particle.size = size;
			particle.rotation = 0f;
			particle.angularVelocity = 0f;
			particle.color = color;
			particle.randomSeed = 5u;
			this.Internal_Emit(ref particle);
		}

		public void Emit(ParticleSystem.Particle particle)
		{
			this.Internal_Emit(ref particle);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Emit(ref ParticleSystem.Particle particle);

		internal static ParticleSystem[] GetParticleSystems(ParticleSystem root)
		{
			if (!root)
			{
				return null;
			}
			List<ParticleSystem> list = new List<ParticleSystem>();
			list.Add(root);
			ParticleSystem.GetDirectParticleSystemChildrenRecursive(root.transform, list);
			return list.ToArray();
		}

		private static void GetDirectParticleSystemChildrenRecursive(Transform transform, List<ParticleSystem> particleSystems)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				ParticleSystem component = transform2.gameObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					particleSystems.Add(component);
					ParticleSystem.GetDirectParticleSystemChildrenRecursive(transform2, particleSystems);
				}
			}
		}

		/// <summary>
		///   <para>Script interface for a Particle.</para>
		/// </summary>
		public struct Particle
		{
			private Vector3 m_Position;

			private Vector3 m_Velocity;

			private Vector3 m_AnimatedVelocity;

			private Vector3 m_AxisOfRotation;

			private float m_Rotation;

			private float m_AngularVelocity;

			private float m_Size;

			private Color32 m_Color;

			private uint m_RandomSeed;

			private float m_Lifetime;

			private float m_StartLifetime;

			private float m_EmitAccumulator0;

			private float m_EmitAccumulator1;

			/// <summary>
			///   <para>The position of the particle.</para>
			/// </summary>
			public Vector3 position
			{
				get
				{
					return this.m_Position;
				}
				set
				{
					this.m_Position = value;
				}
			}

			/// <summary>
			///   <para>The velocity of the particle.</para>
			/// </summary>
			public Vector3 velocity
			{
				get
				{
					return this.m_Velocity;
				}
				set
				{
					this.m_Velocity = value;
				}
			}

			/// <summary>
			///   <para>The lifetime of the particle.</para>
			/// </summary>
			public float lifetime
			{
				get
				{
					return this.m_Lifetime;
				}
				set
				{
					this.m_Lifetime = value;
				}
			}

			/// <summary>
			///   <para>The starting lifetime of the particle.</para>
			/// </summary>
			public float startLifetime
			{
				get
				{
					return this.m_StartLifetime;
				}
				set
				{
					this.m_StartLifetime = value;
				}
			}

			/// <summary>
			///   <para>The initial size of the particle. The current size of the particle is calculated procedurally based on this value and the active size modules.</para>
			/// </summary>
			public float size
			{
				get
				{
					return this.m_Size;
				}
				set
				{
					this.m_Size = value;
				}
			}

			public Vector3 axisOfRotation
			{
				get
				{
					return this.m_AxisOfRotation;
				}
				set
				{
					this.m_AxisOfRotation = value;
				}
			}

			/// <summary>
			///   <para>The rotation of the particle.</para>
			/// </summary>
			public float rotation
			{
				get
				{
					return this.m_Rotation * 57.29578f;
				}
				set
				{
					this.m_Rotation = value * 0.0174532924f;
				}
			}

			/// <summary>
			///   <para>The angular velocity of the particle.</para>
			/// </summary>
			public float angularVelocity
			{
				get
				{
					return this.m_AngularVelocity * 57.29578f;
				}
				set
				{
					this.m_AngularVelocity = value * 0.0174532924f;
				}
			}

			/// <summary>
			///   <para>The initial color of the particle. The current color of the particle is calculated procedurally based on this value and the active color modules.</para>
			/// </summary>
			public Color32 color
			{
				get
				{
					return this.m_Color;
				}
				set
				{
					this.m_Color = value;
				}
			}

			/// <summary>
			///   <para>The random value of the particle.</para>
			/// </summary>
			[Obsolete("randomValue property is deprecated. Use randomSeed instead to control random behavior of particles.")]
			public float randomValue
			{
				get
				{
					return BitConverter.ToSingle(BitConverter.GetBytes(this.m_RandomSeed), 0);
				}
				set
				{
					this.m_RandomSeed = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
				}
			}

			/// <summary>
			///   <para>The random seed of the particle.</para>
			/// </summary>
			public uint randomSeed
			{
				get
				{
					return this.m_RandomSeed;
				}
				set
				{
					this.m_RandomSeed = value;
				}
			}
		}
	}
}
