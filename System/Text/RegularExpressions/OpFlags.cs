using System;

namespace System.Text.RegularExpressions
{
	[Flags]
	internal enum OpFlags : ushort
	{
		None = 0,
		Negate = 256,
		IgnoreCase = 512,
		RightToLeft = 1024,
		Lazy = 2048
	}
}
