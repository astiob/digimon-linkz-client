using System;

namespace System
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoExtensionAttribute : MonoTODOAttribute
	{
		public MonoExtensionAttribute(string comment) : base(comment)
		{
		}
	}
}
