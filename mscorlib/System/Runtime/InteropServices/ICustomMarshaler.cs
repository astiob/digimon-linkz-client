using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Designed to provide custom wrappers for handling method calls.</summary>
	[ComVisible(true)]
	public interface ICustomMarshaler
	{
		/// <summary>Performs necessary cleanup of the managed data when it is no longer needed.</summary>
		/// <param name="ManagedObj">The managed object to be destroyed. </param>
		void CleanUpManagedData(object ManagedObj);

		/// <summary>Performs necessary cleanup of the unmanaged data when it is no longer needed.</summary>
		/// <param name="pNativeData">A pointer to the unmanaged data to be destroyed. </param>
		void CleanUpNativeData(IntPtr pNativeData);

		/// <summary>Returns the size of the native data to be marshaled.</summary>
		/// <returns>The size in bytes of the native data.</returns>
		int GetNativeDataSize();

		/// <summary>Converts the managed data to unmanaged data.</summary>
		/// <returns>Returns the COM view of the managed object.</returns>
		/// <param name="ManagedObj">The managed object to be converted. </param>
		IntPtr MarshalManagedToNative(object ManagedObj);

		/// <summary>Converts the unmanaged data to managed data.</summary>
		/// <returns>Returns the managed view of the COM data.</returns>
		/// <param name="pNativeData">A pointer to the unmanaged data to be wrapped. </param>
		object MarshalNativeToManaged(IntPtr pNativeData);
	}
}
