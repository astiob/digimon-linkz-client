using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal enum BinaryTypeCode : byte
	{
		Boolean = 1,
		Byte,
		Char,
		Decimal = 5,
		Double,
		Int16,
		Int32,
		Int64,
		SByte,
		Single,
		TimeSpan,
		DateTime,
		UInt16,
		UInt32,
		UInt64,
		Null,
		String
	}
}
