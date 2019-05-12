using System;

namespace UnityEngine.AI
{
	public struct NavMeshBuildDebugSettings
	{
		private byte m_Flags;

		public NavMeshBuildDebugFlags flags
		{
			get
			{
				return (NavMeshBuildDebugFlags)this.m_Flags;
			}
			set
			{
				this.m_Flags = (byte)value;
			}
		}
	}
}
