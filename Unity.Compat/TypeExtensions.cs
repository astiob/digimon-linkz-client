using System;
using System.Reflection;

namespace System
{
	internal static class TypeExtensions
	{
		internal static Type AsType(this Type type)
		{
			return type;
		}

		internal static TypeInfo GetTypeInfo(this Type type)
		{
			return TypeInfo.FromType(type);
		}
	}
}
