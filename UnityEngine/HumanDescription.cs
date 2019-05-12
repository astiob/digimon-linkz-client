using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class that holds humanoid avatar parameters to pass to the AvatarBuilder.BuildHumanAvatar function.</para>
	/// </summary>
	public struct HumanDescription
	{
		/// <summary>
		///   <para>Mapping between Mecanim bone names and bone names in the rig.</para>
		/// </summary>
		public HumanBone[] human;

		/// <summary>
		///   <para>List of bone Transforms to include in the model.</para>
		/// </summary>
		public SkeletonBone[] skeleton;

		internal float m_ArmTwist;

		internal float m_ForeArmTwist;

		internal float m_UpperLegTwist;

		internal float m_LegTwist;

		internal float m_ArmStretch;

		internal float m_LegStretch;

		internal float m_FeetSpacing;

		private bool m_HasTranslationDoF;

		/// <summary>
		///   <para>Defines how the lower arm's roll/twisting is distributed between the shoulder and elbow joints.</para>
		/// </summary>
		public float upperArmTwist
		{
			get
			{
				return this.m_ArmTwist;
			}
			set
			{
				this.m_ArmTwist = value;
			}
		}

		/// <summary>
		///   <para>Defines how the lower arm's roll/twisting is distributed between the elbow and wrist joints.</para>
		/// </summary>
		public float lowerArmTwist
		{
			get
			{
				return this.m_ForeArmTwist;
			}
			set
			{
				this.m_ForeArmTwist = value;
			}
		}

		/// <summary>
		///   <para>Defines how the upper leg's roll/twisting is distributed between the thigh and knee joints.</para>
		/// </summary>
		public float upperLegTwist
		{
			get
			{
				return this.m_UpperLegTwist;
			}
			set
			{
				this.m_UpperLegTwist = value;
			}
		}

		/// <summary>
		///   <para>Defines how the lower leg's roll/twisting is distributed between the knee and ankle.</para>
		/// </summary>
		public float lowerLegTwist
		{
			get
			{
				return this.m_LegTwist;
			}
			set
			{
				this.m_LegTwist = value;
			}
		}

		/// <summary>
		///   <para>Amount by which the arm's length is allowed to stretch when using IK.</para>
		/// </summary>
		public float armStretch
		{
			get
			{
				return this.m_ArmStretch;
			}
			set
			{
				this.m_ArmStretch = value;
			}
		}

		/// <summary>
		///   <para>Amount by which the leg's length is allowed to stretch when using IK.</para>
		/// </summary>
		public float legStretch
		{
			get
			{
				return this.m_LegStretch;
			}
			set
			{
				this.m_LegStretch = value;
			}
		}

		/// <summary>
		///   <para>Modification to the minimum distance between the feet of a humanoid model.</para>
		/// </summary>
		public float feetSpacing
		{
			get
			{
				return this.m_FeetSpacing;
			}
			set
			{
				this.m_FeetSpacing = value;
			}
		}

		/// <summary>
		///   <para>True for any human that has a translation Degree of Freedom (DoF). It is set to false by default.</para>
		/// </summary>
		public bool hasTranslationDoF
		{
			get
			{
				return this.m_HasTranslationDoF;
			}
			set
			{
				this.m_HasTranslationDoF = value;
			}
		}
	}
}
