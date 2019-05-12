using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>A structure describing the webcam device.</para>
	/// </summary>
	public struct WebCamDevice
	{
		internal string m_Name;

		internal int m_Flags;

		/// <summary>
		///   <para>A human-readable name of the device. Varies across different systems.</para>
		/// </summary>
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		/// <summary>
		///   <para>True if camera faces the same direction a screen does, false otherwise.</para>
		/// </summary>
		public bool isFrontFacing
		{
			get
			{
				return (this.m_Flags & 1) == 1;
			}
		}
	}
}
