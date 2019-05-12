using System;

namespace System.Text.RegularExpressions
{
	internal enum OpCode : ushort
	{
		False,
		True,
		Position,
		String,
		Reference,
		Character,
		Category,
		NotCategory,
		Range,
		Set,
		In,
		Open,
		Close,
		Balance,
		BalanceStart,
		IfDefined,
		Sub,
		Test,
		Branch,
		Jump,
		Repeat,
		Until,
		FastRepeat,
		Anchor,
		Info
	}
}
