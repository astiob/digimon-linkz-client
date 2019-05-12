using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal enum ArrayStructure : byte
	{
		SingleDimensional,
		Jagged,
		MultiDimensional
	}
}
