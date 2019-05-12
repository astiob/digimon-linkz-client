using System;

namespace System.IO
{
	internal enum MonoFileType
	{
		Unknown,
		Disk,
		Char,
		Pipe,
		Remote = 32768
	}
}
