using System;

namespace UnityEngine.Experimental.Director
{
	/// <summary>
	///   <para>This structure contains the frame information a Playable receives in Playable.PrepareFrame.</para>
	/// </summary>
	public struct FrameData
	{
		internal int m_UpdateId;

		internal double m_Time;

		internal double m_LastTime;

		internal double m_TimeScale;

		/// <summary>
		///   <para>Frame update counter. Can be used to know when to initialize your Playable (when updateid is 0).</para>
		/// </summary>
		public int updateId
		{
			get
			{
				return this.m_UpdateId;
			}
		}

		/// <summary>
		///   <para>Current time at the start of the frame.</para>
		/// </summary>
		public float time
		{
			get
			{
				return (float)this.m_Time;
			}
		}

		/// <summary>
		///   <para>Last frame's start time.</para>
		/// </summary>
		public float lastTime
		{
			get
			{
				return (float)this.m_LastTime;
			}
		}

		/// <summary>
		///   <para>Time difference between this frame and the preceding frame.</para>
		/// </summary>
		public float deltaTime
		{
			get
			{
				return (float)this.m_Time - (float)this.m_LastTime;
			}
		}

		/// <summary>
		///   <para>Time speed multiplier. 1 is normal speed, 0 is stopped.</para>
		/// </summary>
		public float timeScale
		{
			get
			{
				return (float)this.m_TimeScale;
			}
		}

		/// <summary>
		///   <para>Current time at the start of the frame in double precision.</para>
		/// </summary>
		public double dTime
		{
			get
			{
				return this.m_Time;
			}
		}

		/// <summary>
		///   <para>Time difference between this frame and the preceding frame in double precision.</para>
		/// </summary>
		public double dLastTime
		{
			get
			{
				return this.m_LastTime;
			}
		}

		/// <summary>
		///   <para>Time difference between this frame and the preceding frame in double precision.</para>
		/// </summary>
		public double dDeltaTime
		{
			get
			{
				return this.m_Time - this.m_LastTime;
			}
		}

		/// <summary>
		///   <para>Time speed multiplier in double precision.</para>
		/// </summary>
		public double dtimeScale
		{
			get
			{
				return this.m_TimeScale;
			}
		}
	}
}
