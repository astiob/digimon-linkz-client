using System;

namespace System.Reflection
{
	[Flags]
	internal enum PInfo
	{
		Attributes = 1,
		GetMethod = 2,
		SetMethod = 4,
		ReflectedType = 8,
		DeclaringType = 16,
		Name = 32
	}
}
