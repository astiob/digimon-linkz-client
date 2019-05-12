using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal enum ReturnTypeTag : byte
	{
		Null = 2,
		PrimitiveType = 8,
		ObjectType = 16,
		Exception = 32
	}
}
