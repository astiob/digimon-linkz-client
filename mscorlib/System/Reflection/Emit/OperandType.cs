using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Describes the operand type of Microsoft intermediate language (MSIL) instruction.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum OperandType
	{
		/// <summary>The operand is a 32-bit integer branch target.</summary>
		InlineBrTarget,
		/// <summary>The operand is a 32-bit metadata token.</summary>
		InlineField,
		/// <summary>The operand is a 32-bit integer.</summary>
		InlineI,
		/// <summary>The operand is a 64-bit integer.</summary>
		InlineI8,
		/// <summary>The operand is a 32-bit metadata token.</summary>
		InlineMethod,
		/// <summary>No operand.</summary>
		InlineNone,
		/// <summary>The operand is reserved and should not be used.</summary>
		[Obsolete("This API has been deprecated.")]
		InlinePhi,
		/// <summary>The operand is a 64-bit IEEE floating point number.</summary>
		InlineR,
		/// <summary>The operand is a 32-bit metadata signature token.</summary>
		InlineSig = 9,
		/// <summary>The operand is a 32-bit metadata string token.</summary>
		InlineString,
		/// <summary>The operand is the 32-bit integer argument to a switch instruction.</summary>
		InlineSwitch,
		/// <summary>The operand is a FieldRef, MethodRef, or TypeRef token.</summary>
		InlineTok,
		/// <summary>The operand is a 32-bit metadata token.</summary>
		InlineType,
		/// <summary>The operand is 16-bit integer containing the ordinal of a local variable or an argument.</summary>
		InlineVar,
		/// <summary>The operand is an 8-bit integer branch target.</summary>
		ShortInlineBrTarget,
		/// <summary>The operand is an 8-bit integer.</summary>
		ShortInlineI,
		/// <summary>The operand is a 32-bit IEEE floating point number.</summary>
		ShortInlineR,
		/// <summary>The operand is an 8-bit integer containing the ordinal of a local variable or an argumenta.</summary>
		ShortInlineVar
	}
}
