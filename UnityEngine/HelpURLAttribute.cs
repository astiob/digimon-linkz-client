using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class HelpURLAttribute : Attribute
	{
		public HelpURLAttribute(string url)
		{
			this.URL = url;
		}

		public string URL { get; private set; }
	}
}
