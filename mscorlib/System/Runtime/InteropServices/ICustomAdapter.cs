using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Provides a way for clients to access the actual object, rather than the adapter object handed out by a custom marshaler.</summary>
	[ComVisible(true)]
	public interface ICustomAdapter
	{
		/// <summary>Provides access to the underlying object wrapped by a custom marshaler.</summary>
		/// <returns>The object contained by the adapter object.</returns>
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetUnderlyingObject();
	}
}
