using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Provides the managed definition of the IConnectionPointContainer interface.</summary>
	[Guid("b196b284-bab4-101a-b69c-00aa00341d07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IConnectionPointContainer
	{
		/// <summary>Creates an enumerator of all the connection points supported in the connectable object, one connection point per IID.</summary>
		/// <param name="ppEnum">When this method returns, contains the interface pointer of the enumerator. This parameter is passed uninitialized.</param>
		void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);

		/// <summary>Asks the connectable object if it has a connection point for a particular IID, and if so, returns the IConnectionPoint interface pointer to that connection point.</summary>
		/// <param name="riid">A reference to the outgoing interface IID whose connection point is being requested. </param>
		/// <param name="ppCP">When this method returns, contains the connection point that manages the outgoing interface <paramref name="riid" />. This parameter is passed uninitialized.</param>
		void FindConnectionPoint([In] ref Guid riid, out IConnectionPoint ppCP);
	}
}
