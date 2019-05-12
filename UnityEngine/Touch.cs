using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Structure describing the status of a finger touching the screen.</para>
	/// </summary>
	public struct Touch
	{
		private int m_FingerId;

		private Vector2 m_Position;

		private Vector2 m_RawPosition;

		private Vector2 m_PositionDelta;

		private float m_TimeDelta;

		private int m_TapCount;

		private TouchPhase m_Phase;

		private float m_Pressure;

		private float m_maximumPossiblePressure;

		/// <summary>
		///   <para>The unique index for the touch.</para>
		/// </summary>
		public int fingerId
		{
			get
			{
				return this.m_FingerId;
			}
		}

		/// <summary>
		///   <para>The position of the touch in pixel coordinates.</para>
		/// </summary>
		public Vector2 position
		{
			get
			{
				return this.m_Position;
			}
		}

		public Vector2 rawPosition
		{
			get
			{
				return this.m_RawPosition;
			}
		}

		/// <summary>
		///   <para>The position delta since last change.</para>
		/// </summary>
		public Vector2 deltaPosition
		{
			get
			{
				return this.m_PositionDelta;
			}
		}

		/// <summary>
		///   <para>Amount of time that has passed since the last recorded change in Touch values.</para>
		/// </summary>
		public float deltaTime
		{
			get
			{
				return this.m_TimeDelta;
			}
		}

		/// <summary>
		///   <para>Number of taps.</para>
		/// </summary>
		public int tapCount
		{
			get
			{
				return this.m_TapCount;
			}
		}

		/// <summary>
		///   <para>Describes the phase of the touch.</para>
		/// </summary>
		public TouchPhase phase
		{
			get
			{
				return this.m_Phase;
			}
		}

		/// <summary>
		///   <para>The current amount of pressure being applied to a touch.  1.0f is considered to be the pressure of an average touch.  If Input.touchPressureSupported returns false, the value of this property will always be 1.0f.</para>
		/// </summary>
		public float pressure
		{
			get
			{
				return this.m_Pressure;
			}
		}

		/// <summary>
		///   <para>The maximum possible pressure value for a platform.  If Input.touchPressureSupported returns false, the value of this property will always be 1.0f.</para>
		/// </summary>
		public float maximumPossiblePressure
		{
			get
			{
				return this.m_maximumPossiblePressure;
			}
		}
	}
}
