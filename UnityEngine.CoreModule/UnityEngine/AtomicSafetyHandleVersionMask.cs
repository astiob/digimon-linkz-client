using System;

namespace UnityEngine
{
	[Flags]
	internal enum AtomicSafetyHandleVersionMask
	{
		Read = 1,
		Write = 2,
		Dispose = 4,
		ReadAndWrite = 3,
		ReadWriteAndDispose = 7,
		WriteInv = -3,
		ReadInv = -2,
		ReadAndWriteInv = -4,
		ReadWriteAndDisposeInv = -8
	}
}
