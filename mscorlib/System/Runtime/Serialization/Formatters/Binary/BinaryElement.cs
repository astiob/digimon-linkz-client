using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal enum BinaryElement : byte
	{
		Header,
		RefTypeObject,
		UntypedRuntimeObject,
		UntypedExternalObject,
		RuntimeObject,
		ExternalObject,
		String,
		GenericArray,
		BoxedPrimitiveTypeValue,
		ObjectReference,
		NullValue,
		End,
		Assembly,
		ArrayFiller8b,
		ArrayFiller32b,
		ArrayOfPrimitiveType,
		ArrayOfObject,
		ArrayOfString,
		Method,
		_Unknown4,
		_Unknown5,
		MethodCall,
		MethodResponse
	}
}
