using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The limits defined by the CharacterJoint.</para>
	/// </summary>
	public struct SoftJointLimit
	{
		private float m_Limit;

		private float m_Bounciness;

		private float m_ContactDistance;

		/// <summary>
		///   <para>The limit position/angle of the joint (in degrees).</para>
		/// </summary>
		public float limit
		{
			get
			{
				return this.m_Limit;
			}
			set
			{
				this.m_Limit = value;
			}
		}

		/// <summary>
		///   <para>If greater than zero, the limit is soft. The spring will pull the joint back.</para>
		/// </summary>
		[Obsolete("Spring has been moved to SoftJointLimitSpring class in Unity 5", true)]
		public float spring
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		/// <summary>
		///   <para>If spring is greater than zero, the limit is soft.</para>
		/// </summary>
		[Obsolete("Damper has been moved to SoftJointLimitSpring class in Unity 5", true)]
		public float damper
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		/// <summary>
		///   <para>When the joint hits the limit, it can be made to bounce off it.</para>
		/// </summary>
		public float bounciness
		{
			get
			{
				return this.m_Bounciness;
			}
			set
			{
				this.m_Bounciness = value;
			}
		}

		/// <summary>
		///   <para>Determines how far ahead in space the solver can "see" the joint limit.</para>
		/// </summary>
		public float contactDistance
		{
			get
			{
				return this.m_ContactDistance;
			}
			set
			{
				this.m_ContactDistance = value;
			}
		}

		[Obsolete("Use SoftJointLimit.bounciness instead", true)]
		public float bouncyness
		{
			get
			{
				return this.m_Bounciness;
			}
			set
			{
				this.m_Bounciness = value;
			}
		}
	}
}
