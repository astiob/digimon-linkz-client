using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class NativeAsStructAttribute : Attribute, IBindingsAttribute
	{
		public NativeAsStructAttribute(string structName)
		{
			this.StructName = structName;
		}

		public string StructName { get; set; }
	}
}
