using System;

namespace System.Globalization
{
	[Flags]
	internal enum DateTimeFormatFlags
	{
		Unused = 0,
		But = 1,
		Serialized = 2,
		By = 3,
		Microsoft = 4
	}
}
