using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	internal class NativeThrowsAttribute : Attribute, IBindingsThrowsProviderAttribute, IBindingsAttribute
	{
		public NativeThrowsAttribute()
		{
			this.ThrowsException = true;
		}

		public NativeThrowsAttribute(bool throwsException)
		{
			this.ThrowsException = throwsException;
		}

		public bool ThrowsException { get; set; }
	}
}
