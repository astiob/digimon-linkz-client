using System;
using UnityEngine.Scripting;

namespace UnityEngine.Collections
{
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Struct)]
	public class NativeContainerSupportsDeallocateOnJobCompletionAttribute : Attribute
	{
	}
}
