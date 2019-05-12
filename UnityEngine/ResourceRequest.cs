using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Asynchronous load request from the Resources bundle.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ResourceRequest : AsyncOperation
	{
		internal string m_Path;

		internal Type m_Type;

		/// <summary>
		///   <para>Asset object being loaded (Read Only).</para>
		/// </summary>
		public Object asset
		{
			get
			{
				return Resources.Load(this.m_Path, this.m_Type);
			}
		}
	}
}
