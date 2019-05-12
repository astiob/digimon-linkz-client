using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class NativeWritableSelfAttribute : Attribute, IBindingsWritableSelfProviderAttribute, IBindingsAttribute
	{
		public NativeWritableSelfAttribute()
		{
			this.WritableSelf = true;
		}

		public NativeWritableSelfAttribute(bool writable)
		{
			this.WritableSelf = writable;
		}

		public bool WritableSelf { get; set; }
	}
}
