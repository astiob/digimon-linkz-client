using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Defines how to access a function.</summary>
	[Serializable]
	public enum FUNCKIND
	{
		/// <summary>The function is accessed in the same way as <see cref="F:System.Runtime.InteropServices.FUNCKIND.FUNC_PUREVIRTUAL" />, except the function has an implementation.</summary>
		FUNC_VIRTUAL,
		/// <summary>The function is accessed through the virtual function table (VTBL), and takes an implicit this pointer.</summary>
		FUNC_PUREVIRTUAL,
		/// <summary>The function is accessed by static address and takes an implicit this pointer.</summary>
		FUNC_NONVIRTUAL,
		/// <summary>The function is accessed by static address and does not take an implicit this pointer.</summary>
		FUNC_STATIC,
		/// <summary>The function can be accessed only through IDispatch.</summary>
		FUNC_DISPATCH
	}
}
