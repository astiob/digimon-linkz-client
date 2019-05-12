using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Structure describing acceleration status of the device.</para>
	/// </summary>
	public struct AccelerationEvent
	{
		private Vector3 m_Acceleration;

		private float m_TimeDelta;

		/// <summary>
		///   <para>Value of acceleration.</para>
		/// </summary>
		public Vector3 acceleration
		{
			get
			{
				return this.m_Acceleration;
			}
		}

		/// <summary>
		///   <para>Amount of time passed since last accelerometer measurement.</para>
		/// </summary>
		public float deltaTime
		{
			get
			{
				return this.m_TimeDelta;
			}
		}
	}
}
