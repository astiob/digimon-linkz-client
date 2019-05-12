using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Data of a lightmap.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class LightmapData
	{
		internal Texture2D m_Light;

		internal Texture2D m_Dir;

		/// <summary>
		///   <para>Lightmap storing the full incoming light.</para>
		/// </summary>
		public Texture2D lightmapFar
		{
			get
			{
				return this.m_Light;
			}
			set
			{
				this.m_Light = value;
			}
		}

		/// <summary>
		///   <para>Lightmap storing only the indirect incoming light.</para>
		/// </summary>
		public Texture2D lightmapNear
		{
			get
			{
				return this.m_Dir;
			}
			set
			{
				this.m_Dir = value;
			}
		}
	}
}
