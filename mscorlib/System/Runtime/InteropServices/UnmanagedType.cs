using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Identifies how to marshal parameters or fields to unmanaged code.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum UnmanagedType
	{
		/// <summary>A 4-byte Boolean value (true != 0, false = 0). This is the Win32 BOOL type.</summary>
		Bool = 2,
		/// <summary>A 1-byte signed integer. You can use this member to transform a Boolean value into a 1-byte, C-style bool (true = 1, false = 0).</summary>
		I1,
		/// <summary>A 1-byte unsigned integer.</summary>
		U1,
		/// <summary>A 2-byte signed integer.</summary>
		I2,
		/// <summary>A 2-byte unsigned integer.</summary>
		U2,
		/// <summary>A 4-byte signed integer.</summary>
		I4,
		/// <summary>A 4-byte unsigned integer.</summary>
		U4,
		/// <summary>An 8-byte signed integer.</summary>
		I8,
		/// <summary>An 8-byte unsigned integer.</summary>
		U8,
		/// <summary>A 4-byte floating point number.</summary>
		R4,
		/// <summary>An 8-byte floating point number.</summary>
		R8,
		/// <summary>Used on a <see cref="T:System.Decimal" /> to marshal the decimal value as a COM currency type instead of as a Decimal.</summary>
		Currency = 15,
		/// <summary>A Unicode character string that is a length-prefixed double byte. You can use this member, which is the default string in COM, on the <see cref="T:System.String" /> data type.</summary>
		BStr = 19,
		/// <summary>A single byte, null-terminated ANSI character string. You can use this member on the <see cref="T:System.String" /> or <see cref="T:System.Text.StringBuilder" /> data types</summary>
		LPStr,
		/// <summary>A 2-byte, null-terminated Unicode character string.</summary>
		LPWStr,
		/// <summary>A platform-dependent character string: ANSI on Windows 98 and Unicode on Windows NT and Windows XP. This value is only supported for platform invoke, and not COM interop, because exporting a string of type LPTStr is not supported.</summary>
		LPTStr,
		/// <summary>Used for in-line, fixed-length character arrays that appear within a structure. The character type used with <see cref="F:System.Runtime.InteropServices.UnmanagedType.ByValTStr" /> is determined by the <see cref="T:System.Runtime.InteropServices.CharSet" /> argument of the <see cref="T:System.Runtime.InteropServices.StructLayoutAttribute" /> applied to the containing structure. Always use the <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.SizeConst" /> field to indicate the size of the array.</summary>
		ByValTStr,
		/// <summary>A COM IUnknown pointer. You can use this member on the <see cref="T:System.Object" /> data type.</summary>
		IUnknown = 25,
		/// <summary>A COM IDispatch pointer (Object in Microsoft Visual Basic 6.0).</summary>
		IDispatch,
		/// <summary>A VARIANT, which is used to marshal managed formatted classes and value types.</summary>
		Struct,
		/// <summary>A COM interface pointer. The <see cref="T:System.Guid" /> of the interface is obtained from the class metadata. Use this member to specify the exact interface type or the default interface type if you apply it to a class. This member produces <see cref="F:System.Runtime.InteropServices.UnmanagedType.IUnknown" /> behavior when you apply it to the <see cref="T:System.Object" /> data type.</summary>
		Interface,
		/// <summary>A SafeArray is a self-describing array that carries the type, rank, and bounds of the associated array data. You can use this member with the <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.SafeArraySubType" /> field to override the default element type.</summary>
		SafeArray,
		/// <summary>When <see cref="P:System.Runtime.InteropServices.MarshalAsAttribute.Value" /> is set to ByValArray, the <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.SizeConst" /> must be set to indicate the number of elements in the array. The <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.ArraySubType" /> field can optionally contain the <see cref="T:System.Runtime.InteropServices.UnmanagedType" /> of the array elements when it is necessary to differentiate among string types. You can only use this <see cref="T:System.Runtime.InteropServices.UnmanagedType" /> on an array that appear as fields in a structure.</summary>
		ByValArray,
		/// <summary>A platform-dependent, signed integer. 4-bytes on 32 bit Windows, 8-bytes on 64 bit Windows.</summary>
		SysInt,
		/// <summary>A platform-dependent, unsigned integer. 4-bytes on 32 bit Windows, 8-bytes on 64 bit Windows.</summary>
		SysUInt,
		/// <summary>Allows Visual Basic 2005 to change a string in unmanaged code, and have the results reflected in managed code. This value is only supported for platform invoke.</summary>
		VBByRefStr = 34,
		/// <summary>An ANSI character string that is a length prefixed, single byte. You can use this member on the <see cref="T:System.String" /> data type.</summary>
		AnsiBStr,
		/// <summary>A length-prefixed, platform-dependent char string. ANSI on Windows 98, Unicode on Windows NT. You rarely use this BSTR-like member.</summary>
		TBStr,
		/// <summary>A 2-byte, OLE-defined VARIANT_BOOL type (true = -1, false = 0).</summary>
		VariantBool,
		/// <summary>An integer that can be used as a C-style function pointer. You can use this member on a <see cref="T:System.Delegate" /> data type or a type that inherits from a <see cref="T:System.Delegate" />.</summary>
		FunctionPtr,
		/// <summary>A dynamic type that determines the type of an object at run time and marshals the object as that type. Valid for platform invoke methods only.</summary>
		AsAny = 40,
		/// <summary>A pointer to the first element of a C-style array. When marshaling from managed to unmanaged, the length of the array is determined by the length of the managed array. When marshaling from unmanaged to managed, the length of the array is determined from the <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.SizeConst" /> and the <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.SizeParamIndex" /> fields, optionally followed by the unmanaged type of the elements within the array when it is necessary to differentiate among string types.</summary>
		LPArray = 42,
		/// <summary>A pointer to a C-style structure that you use to marshal managed formatted classes. Valid for platform invoke methods only.</summary>
		LPStruct,
		/// <summary>Specifies the custom marshaler class when used with <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.MarshalType" /> or <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.MarshalTypeRef" />. The <see cref="F:System.Runtime.InteropServices.MarshalAsAttribute.MarshalCookie" /> field can be used to pass additional information to the custom marshaler. You can use this member on any reference type.</summary>
		CustomMarshaler,
		/// <summary>This native type associated with an <see cref="F:System.Runtime.InteropServices.UnmanagedType.I4" /> or a <see cref="F:System.Runtime.InteropServices.UnmanagedType.U4" /> causes the parameter to be exported as a HRESULT in the exported type library.</summary>
		Error
	}
}
