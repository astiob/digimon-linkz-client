using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>(Legacy Particle system).</para>
	/// </summary>
	public struct Particle
	{
		private Vector3 m_Position;

		private Vector3 m_Velocity;

		private float m_Size;

		private float m_Rotation;

		private float m_AngularVelocity;

		private float m_Energy;

		private float m_StartEnergy;

		private Color m_Color;

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
		///   <para>The energy of the particle.</para>
		/// </summary>
		public float energy
		{
			get
			{
				return this.m_Energy;
			}
			set
			{
				this.m_Energy = value;
			}
		}

		/// <summary>
		///   <para>The starting energy of the particle.</para>
		/// </summary>
		public float startEnergy
		{
			get
			{
				return this.m_StartEnergy;
			}
			set
			{
				this.m_StartEnergy = value;
			}
		}

		/// <summary>
		///   <para>The size of the particle.</para>
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

		/// <summary>
		///   <para>The rotation of the particle.</para>
		/// </summary>
		public float rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				this.m_Rotation = value;
			}
		}

		/// <summary>
		///   <para>The angular velocity of the particle.</para>
		/// </summary>
		public float angularVelocity
		{
			get
			{
				return this.m_AngularVelocity;
			}
			set
			{
				this.m_AngularVelocity = value;
			}
		}

		/// <summary>
		///   <para>The color of the particle.</para>
		/// </summary>
		public Color color
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
	}
}
