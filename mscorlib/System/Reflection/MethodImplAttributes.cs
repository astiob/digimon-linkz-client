using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Specifies flags for the attributes of a method implementation.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplAttributes
	{
		/// <summary>Specifies flags about code type.</summary>
		CodeTypeMask = 3,
		/// <summary>Specifies that the method implementation is in Microsoft intermediate language (MSIL).</summary>
		IL = 0,
		/// <summary>Specifies that the method implementation is native.</summary>
		Native,
		/// <summary>Specifies that the method implementation is in Optimized Intermediate Language (OPTIL).</summary>
		OPTIL,
		/// <summary>Specifies that the method implementation is provided by the runtime.</summary>
		Runtime,
		/// <summary>Specifies whether the method is implemented in managed or unmanaged code.</summary>
		ManagedMask,
		/// <summary>Specifies that the method is implemented in unmanaged code.</summary>
		Unmanaged = 4,
		/// <summary>Specifies that the method is implemented in managed code. </summary>
		Managed = 0,
		/// <summary>Specifies that the method is not defined.</summary>
		ForwardRef = 16,
		/// <summary>Specifies that the method signature is exported exactly as declared.</summary>
		PreserveSig = 128,
		/// <summary>Specifies an internal call.</summary>
		InternalCall = 4096,
		/// <summary>Specifies that the method is single-threaded through the body. Static methods (Shared in Visual Basic) lock on the type, whereas instance methods lock on the instance. You can also use the C# lock statement or the Visual Basic Lock function for this purpose. </summary>
		Synchronized = 32,
		/// <summary>Specifies that the method cannot be inlined.</summary>
		NoInlining = 8,
		/// <summary>Specifies a range check value.</summary>
		MaxMethodImplVal = 65535
	}
}
