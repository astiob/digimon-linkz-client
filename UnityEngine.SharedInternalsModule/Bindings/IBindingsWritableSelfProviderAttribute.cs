using System;

namespace UnityEngine.Bindings
{
	internal interface IBindingsWritableSelfProviderAttribute : IBindingsAttribute
	{
		bool WritableSelf { get; set; }
	}
}
