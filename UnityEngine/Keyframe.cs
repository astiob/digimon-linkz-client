using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>A single keyframe that can be injected into an animation curve.</para>
	/// </summary>
	public struct Keyframe
	{
		private float m_Time;

		private float m_Value;

		private float m_InTangent;

		private float m_OutTangent;

		/// <summary>
		///   <para>Create a keyframe.</para>
		/// </summary>
		/// <param name="time"></param>
		/// <param name="value"></param>
		public Keyframe(float time, float value)
		{
			this.m_Time = time;
			this.m_Value = value;
			this.m_InTangent = 0f;
			this.m_OutTangent = 0f;
		}

		/// <summary>
		///   <para>Create a keyframe.</para>
		/// </summary>
		/// <param name="time"></param>
		/// <param name="value"></param>
		/// <param name="inTangent"></param>
		/// <param name="outTangent"></param>
		public Keyframe(float time, float value, float inTangent, float outTangent)
		{
			this.m_Time = time;
			this.m_Value = value;
			this.m_InTangent = inTangent;
			this.m_OutTangent = outTangent;
		}

		/// <summary>
		///   <para>The time of the keyframe.</para>
		/// </summary>
		public float time
		{
			get
			{
				return this.m_Time;
			}
			set
			{
				this.m_Time = value;
			}
		}

		/// <summary>
		///   <para>The value of the curve at keyframe.</para>
		/// </summary>
		public float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		/// <summary>
		///   <para>Describes the tangent when approaching this point from the previous point in the curve.</para>
		/// </summary>
		public float inTangent
		{
			get
			{
				return this.m_InTangent;
			}
			set
			{
				this.m_InTangent = value;
			}
		}

		/// <summary>
		///   <para>Describes the tangent when leaving this point towards the next point in the curve.</para>
		/// </summary>
		public float outTangent
		{
			get
			{
				return this.m_OutTangent;
			}
			set
			{
				this.m_OutTangent = value;
			}
		}

		public int tangentMode
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}
	}
}
