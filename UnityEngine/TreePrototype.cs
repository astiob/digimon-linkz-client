using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Simple class that contains a pointer to a tree prototype.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class TreePrototype
	{
		internal GameObject m_Prefab;

		internal float m_BendFactor;

		/// <summary>
		///   <para>Retrieves the actual GameObect used by the tree.</para>
		/// </summary>
		public GameObject prefab
		{
			get
			{
				return this.m_Prefab;
			}
			set
			{
				this.m_Prefab = value;
			}
		}

		/// <summary>
		///   <para>Bend factor of the tree prototype.</para>
		/// </summary>
		public float bendFactor
		{
			get
			{
				return this.m_BendFactor;
			}
			set
			{
				this.m_BendFactor = value;
			}
		}
	}
}
