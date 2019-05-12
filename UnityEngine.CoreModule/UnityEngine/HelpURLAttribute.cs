using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HelpURLAttribute : Attribute
	{
		internal readonly string m_Url;

		public HelpURLAttribute(string url)
		{
			this.m_Url = url;
		}

		public string URL
		{
			get
			{
				return this.m_Url;
			}
		}
	}
}
