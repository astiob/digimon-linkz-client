using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>JointLimits is used by the HingeJoint to limit the joints angle.</para>
	/// </summary>
	public struct JointLimits
	{
		private float m_Min;

		private float m_Max;

		private float m_Bounciness;

		private float m_BounceMinVelocity;

		private float m_ContactDistance;

		[Obsolete("minBounce and maxBounce are replaced by a single JointLimits.bounciness for both limit ends.", true)]
		public float minBounce;

		[Obsolete("minBounce and maxBounce are replaced by a single JointLimits.bounciness for both limit ends.", true)]
		public float maxBounce;

		/// <summary>
		///   <para>The lower angular limit (in degrees) of the joint.</para>
		/// </summary>
		public float min
		{
			get
			{
				return this.m_Min;
			}
			set
			{
				this.m_Min = value;
			}
		}

		/// <summary>
		///   <para>The upper angular limit (in degrees) of the joint.</para>
		/// </summary>
		public float max
		{
			get
			{
				return this.m_Max;
			}
			set
			{
				this.m_Max = value;
			}
		}

		/// <summary>
		///   <para>Determines the size of the bounce when the joint hits it's limit. Also known as restitution.</para>
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
		///   <para>The minimum impact velocity which will cause the joint to bounce.</para>
		/// </summary>
		public float bounceMinVelocity
		{
			get
			{
				return this.m_BounceMinVelocity;
			}
			set
			{
				this.m_BounceMinVelocity = value;
			}
		}

		/// <summary>
		///   <para>Distance inside the limit value at which the limit will be considered to be active by the solver.</para>
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
	}
}
