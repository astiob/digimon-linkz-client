using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates how to marshal the array elements when an array is marshaled from managed to unmanaged code as a <see cref="F:System.Runtime.InteropServices.UnmanagedType.SafeArray" />.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum VarEnum
	{
		/// <summary>Indicates that a value was not specified.</summary>
		VT_EMPTY,
		/// <summary>Indicates a null value, similar to a null value in SQL.</summary>
		VT_NULL,
		/// <summary>Indicates a short integer.</summary>
		VT_I2,
		/// <summary>Indicates a long integer.</summary>
		VT_I4,
		/// <summary>Indicates a float value.</summary>
		VT_R4,
		/// <summary>Indicates a double value.</summary>
		VT_R8,
		/// <summary>Indicates a currency value.</summary>
		VT_CY,
		/// <summary>Indicates a DATE value.</summary>
		VT_DATE,
		/// <summary>Indicates a BSTR string.</summary>
		VT_BSTR,
		/// <summary>Indicates an IDispatch pointer.</summary>
		VT_DISPATCH,
		/// <summary>Indicates an SCODE.</summary>
		VT_ERROR,
		/// <summary>Indicates a Boolean value.</summary>
		VT_BOOL,
		/// <summary>Indicates a VARIANT far pointer.</summary>
		VT_VARIANT,
		/// <summary>Indicates an IUnknown pointer.</summary>
		VT_UNKNOWN,
		/// <summary>Indicates a decimal value.</summary>
		VT_DECIMAL,
		/// <summary>Indicates a char value.</summary>
		VT_I1 = 16,
		/// <summary>Indicates a byte.</summary>
		VT_UI1,
		/// <summary>Indicates an unsignedshort.</summary>
		VT_UI2,
		/// <summary>Indicates an unsignedlong.</summary>
		VT_UI4,
		/// <summary>Indicates a 64-bit integer.</summary>
		VT_I8,
		/// <summary>Indicates an 64-bit unsigned integer.</summary>
		VT_UI8,
		/// <summary>Indicates an integer value.</summary>
		VT_INT,
		/// <summary>Indicates an unsigned integer value.</summary>
		VT_UINT,
		/// <summary>Indicates a C style void.</summary>
		VT_VOID,
		/// <summary>Indicates an HRESULT.</summary>
		VT_HRESULT,
		/// <summary>Indicates a pointer type.</summary>
		VT_PTR,
		/// <summary>Indicates a SAFEARRAY. Not valid in a VARIANT.</summary>
		VT_SAFEARRAY,
		/// <summary>Indicates a C style array.</summary>
		VT_CARRAY,
		/// <summary>Indicates a user defined type.</summary>
		VT_USERDEFINED,
		/// <summary>Indicates a null-terminated string.</summary>
		VT_LPSTR,
		/// <summary>Indicates a wide string terminated by null.</summary>
		VT_LPWSTR,
		/// <summary>Indicates a user defined type.</summary>
		VT_RECORD = 36,
		/// <summary>Indicates a FILETIME value.</summary>
		VT_FILETIME = 64,
		/// <summary>Indicates length prefixed bytes.</summary>
		VT_BLOB,
		/// <summary>Indicates that the name of a stream follows.</summary>
		VT_STREAM,
		/// <summary>Indicates that the name of a storage follows.</summary>
		VT_STORAGE,
		/// <summary>Indicates that a stream contains an object.</summary>
		VT_STREAMED_OBJECT,
		/// <summary>Indicates that a storage contains an object.</summary>
		VT_STORED_OBJECT,
		/// <summary>Indicates that a blob contains an object.</summary>
		VT_BLOB_OBJECT,
		/// <summary>Indicates the clipboard format.</summary>
		VT_CF,
		/// <summary>Indicates a class ID.</summary>
		VT_CLSID,
		/// <summary>Indicates a simple, counted array.</summary>
		VT_VECTOR = 4096,
		/// <summary>Indicates a SAFEARRAY pointer.</summary>
		VT_ARRAY = 8192,
		/// <summary>Indicates that a value is a reference.</summary>
		VT_BYREF = 16384
	}
}
