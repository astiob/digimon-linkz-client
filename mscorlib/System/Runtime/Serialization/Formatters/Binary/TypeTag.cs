using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal enum TypeTag : byte
	{
		PrimitiveType,
		String,
		ObjectType,
		RuntimeType,
		GenericType,
		ArrayOfObject,
		ArrayOfString,
		ArrayOfPrimitiveType
	}
}
