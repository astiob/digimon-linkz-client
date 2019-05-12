using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property)]
	internal class StaticAccessorAttribute : Attribute, IBindingsAttribute
	{
		public StaticAccessorAttribute()
		{
		}

		internal StaticAccessorAttribute(string name)
		{
			this.Name = name;
		}

		public StaticAccessorAttribute(StaticAccessorType type)
		{
			this.Type = type;
		}

		public StaticAccessorAttribute(string name, StaticAccessorType type)
		{
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }

		public StaticAccessorType Type { get; set; }
	}
}
