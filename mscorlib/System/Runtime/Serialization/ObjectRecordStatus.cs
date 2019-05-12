using System;

namespace System.Runtime.Serialization
{
	internal enum ObjectRecordStatus : byte
	{
		Unregistered,
		ReferenceUnsolved,
		ReferenceSolvingDelayed,
		ReferenceSolved
	}
}
