using System;

namespace System.Text.RegularExpressions
{
	internal enum Position : ushort
	{
		Any,
		Start,
		StartOfString,
		StartOfLine,
		StartOfScan,
		End,
		EndOfString,
		EndOfLine,
		Boundary,
		NonBoundary
	}
}
