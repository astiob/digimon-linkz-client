using System;
using UnityEngine.Scripting;

namespace UnityEngine.Collections
{
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Field)]
	public class DeallocateOnJobCompletionAttribute : Attribute
	{
	}
}
