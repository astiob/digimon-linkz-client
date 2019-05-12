using System;

namespace System.IO
{
	internal enum FAMCodes
	{
		Changed = 1,
		Deleted,
		StartExecuting,
		StopExecuting,
		Created,
		Moved,
		Acknowledge,
		Exists,
		EndExist
	}
}
