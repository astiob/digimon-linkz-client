using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class SharedBetweenAnimatorsAttribute : Attribute
	{
	}
}
