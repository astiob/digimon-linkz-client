using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>To specify position and rotation weight mask for Animator::MatchTarget.</para>
	/// </summary>
	public struct MatchTargetWeightMask
	{
		private Vector3 m_PositionXYZWeight;

		private float m_RotationWeight;

		/// <summary>
		///   <para>MatchTargetWeightMask contructor.</para>
		/// </summary>
		/// <param name="positionXYZWeight">Position XYZ weight.</param>
		/// <param name="rotationWeight">Rotation weight.</param>
		public MatchTargetWeightMask(Vector3 positionXYZWeight, float rotationWeight)
		{
			this.m_PositionXYZWeight = positionXYZWeight;
			this.m_RotationWeight = rotationWeight;
		}

		/// <summary>
		///   <para>Position XYZ weight.</para>
		/// </summary>
		public Vector3 positionXYZWeight
		{
			get
			{
				return this.m_PositionXYZWeight;
			}
			set
			{
				this.m_PositionXYZWeight = value;
			}
		}

		/// <summary>
		///   <para>Rotation weight.</para>
		/// </summary>
		public float rotationWeight
		{
			get
			{
				return this.m_RotationWeight;
			}
			set
			{
				this.m_RotationWeight = value;
			}
		}
	}
}
