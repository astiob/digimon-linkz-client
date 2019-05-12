using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>This class stores the rotation limits that define the muscle for a single human bone.</para>
	/// </summary>
	public struct HumanLimit
	{
		private Vector3 m_Min;

		private Vector3 m_Max;

		private Vector3 m_Center;

		private float m_AxisLength;

		private int m_UseDefaultValues;

		/// <summary>
		///   <para>Should this limit use the default values?</para>
		/// </summary>
		public bool useDefaultValues
		{
			get
			{
				return this.m_UseDefaultValues != 0;
			}
			set
			{
				this.m_UseDefaultValues = ((!value) ? 0 : 1);
			}
		}

		/// <summary>
		///   <para>The maximum negative rotation away from the initial value that this muscle can apply.</para>
		/// </summary>
		public Vector3 min
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
		///   <para>The maximum rotation away from the initial value that this muscle can apply.</para>
		/// </summary>
		public Vector3 max
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
		///   <para>The default orientation of a bone when no muscle action is applied.</para>
		/// </summary>
		public Vector3 center
		{
			get
			{
				return this.m_Center;
			}
			set
			{
				this.m_Center = value;
			}
		}

		/// <summary>
		///   <para>Length of the bone to which the limit is applied.</para>
		/// </summary>
		public float axisLength
		{
			get
			{
				return this.m_AxisLength;
			}
			set
			{
				this.m_AxisLength = value;
			}
		}
	}
}
