using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	internal class NativeHeaderAttribute : Attribute, IBindingsHeaderProviderAttribute, IBindingsAttribute
	{
		public NativeHeaderAttribute()
		{
		}

		public NativeHeaderAttribute(string header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (header == "")
			{
				throw new ArgumentException("header cannot be empty", "header");
			}
			this.Header = header;
		}

		public string Header { get; set; }
	}
}
