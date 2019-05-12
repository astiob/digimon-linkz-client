using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Prevents the MSIL Disassembler (Ildasm.exe) from disassembling an assembly. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module)]
	public sealed class SuppressIldasmAttribute : Attribute
	{
	}
}
