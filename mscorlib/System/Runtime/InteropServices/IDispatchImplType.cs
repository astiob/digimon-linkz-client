using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates which IDispatch implementation to use for a particular class.</summary>
	[Obsolete]
	[ComVisible(true)]
	[Serializable]
	public enum IDispatchImplType
	{
		/// <summary>Specifies that the common language runtime decides which IDispatch implementation to use.</summary>
		SystemDefinedImpl,
		/// <summary>Specifies that the IDispatch implemenation is supplied by the runtime.</summary>
		InternalImpl,
		/// <summary>Specifies that the IDispatch implementation is supplied by passing the type information for the object to the COM CreateStdDispatch API method.</summary>
		CompatibleImpl
	}
}
