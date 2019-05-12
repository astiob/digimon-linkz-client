using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	/// <summary>Specifies address types for local variables, parameters, and fields in the methods <see cref="M:System.Diagnostics.SymbolStore.ISymbolWriter.DefineLocalVariable(System.String,System.Reflection.FieldAttributes,System.Byte[],System.Diagnostics.SymbolStore.SymAddressKind,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)" />, <see cref="M:System.Diagnostics.SymbolStore.ISymbolWriter.DefineParameter(System.String,System.Reflection.ParameterAttributes,System.Int32,System.Diagnostics.SymbolStore.SymAddressKind,System.Int32,System.Int32,System.Int32)" />, and <see cref="M:System.Diagnostics.SymbolStore.ISymbolWriter.DefineField(System.Diagnostics.SymbolStore.SymbolToken,System.String,System.Reflection.FieldAttributes,System.Byte[],System.Diagnostics.SymbolStore.SymAddressKind,System.Int32,System.Int32,System.Int32)" /> of the <see cref="T:System.Diagnostics.SymbolStore.ISymbolWriter" /> interface.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum SymAddressKind
	{
		/// <summary>A Microsoft intermediate language (MSIL) offset. The <paramref name="addr1" /> parameter is the MSIL local variable or parameter index.</summary>
		ILOffset = 1,
		/// <summary>A native Relevant Virtual Address (RVA). The <paramref name="addr1" /> parameter is the RVA in the module.</summary>
		NativeRVA,
		/// <summary>A native register address. The <paramref name="addr1" /> parameter is the register in which the variable is stored.</summary>
		NativeRegister,
		/// <summary>A register-relative address. The <paramref name="addr1" /> parameter is the register, and the <paramref name="addr2" /> parameter is the offset.</summary>
		NativeRegisterRelative,
		/// <summary>A native offset. The <paramref name="addr1" /> parameter is the offset from the start of the parent.</summary>
		NativeOffset,
		/// <summary>A register-relative address. The <paramref name="addr1" /> parameter is the low-order register, and the <paramref name="addr2" /> parameter is the high-order register.</summary>
		NativeRegisterRegister,
		/// <summary>A register-relative address. The <paramref name="addr1" /> parameter is the low-order register, the <paramref name="addr2" /> parameter is the stack register, and the <paramref name="addr3" /> parameter is the offset from the stack pointer to the high-order part of the value.</summary>
		NativeRegisterStack,
		/// <summary>A register-relative address. The <paramref name="addr1" /> parameter is the stack register, the <paramref name="addr2" /> parameter is the offset from the stack pointer to the low-order part of the value, and the <paramref name="addr3" /> parameter is the high-order register.</summary>
		NativeStackRegister,
		/// <summary>A bit field. The <paramref name="addr1" /> parameter is the position where the field starts, and the <paramref name="addr2" /> parameter is the field length.</summary>
		BitField,
		/// <summary>A native section offset. The <paramref name="addr1" /> parameter is the section, and the <paramref name="addr2" /> parameter is the offset.</summary>
		NativeSectionOffset
	}
}
