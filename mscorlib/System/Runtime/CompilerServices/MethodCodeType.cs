using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Defines how a method is implemented.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum MethodCodeType
	{
		/// <summary>Specifies that the method implementation is in Microsoft intermediate language (MSIL).</summary>
		IL,
		/// <summary>Specifies that the method is implemented in native code.</summary>
		Native,
		/// <summary>Specifies that the method implementation is in optimized intermediate language (OPTIL).</summary>
		OPTIL,
		/// <summary>Specifies that the method implementation is provided by the runtime.</summary>
		Runtime
	}
}
