using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	public struct ShaderPassName
	{
		private int m_NameIndex;

		public ShaderPassName(string name)
		{
			this.m_NameIndex = ShaderPassName.Init(name);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Init(string name);

		internal int nameIndex
		{
			get
			{
				return this.m_NameIndex;
			}
		}
	}
}
