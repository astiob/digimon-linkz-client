using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.ITypeComp" /> instead.</summary>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020403-0000-0000-c000-000000000046")]
	[Obsolete]
	[ComImport]
	public interface UCOMITypeComp
	{
		/// <summary>Maps a name to a member of a type, or binds global variables and functions contained in a type library.</summary>
		/// <param name="szName">The name to bind. </param>
		/// <param name="lHashVal">A hash value for <paramref name="szName" /> computed by LHashValOfNameSys. </param>
		/// <param name="wFlags">A flags word containing one or more of the invoke flags defined in the INVOKEKIND enumeration. </param>
		/// <param name="ppTInfo">On successful return, a reference to the type description that contains the item to which it is bound, if a FUNCDESC or VARDESC was returned. </param>
		/// <param name="pDescKind">A reference to a DESCKIND enumerator that indicates whether the name bound to is a VARDESC, FUNCDESC, or TYPECOMP. </param>
		/// <param name="pBindPtr">A reference to the bound-to VARDESC, FUNCDESC, or ITypeComp interface. </param>
		void Bind([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, short wFlags, out UCOMITypeInfo ppTInfo, out DESCKIND pDescKind, out BINDPTR pBindPtr);

		/// <summary>Binds to the type descriptions contained within a type library.</summary>
		/// <param name="szName">The name to bind. </param>
		/// <param name="lHashVal">A hash value for <paramref name="szName" /> determined by LHashValOfNameSys. </param>
		/// <param name="ppTInfo">On successful return, a reference to an ITypeInfo of the type to which <paramref name="szName" /> was bound. </param>
		/// <param name="ppTComp">On successful return, a reference to an ITypeComp variable. </param>
		void BindType([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, out UCOMITypeInfo ppTInfo, out UCOMITypeComp ppTComp);
	}
}
