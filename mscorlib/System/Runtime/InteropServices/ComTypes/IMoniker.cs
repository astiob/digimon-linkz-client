using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Provides the managed definition of the IMoniker interface, with COM functionality from IPersist and IPersistStream.</summary>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000f-0000-0000-c000-000000000046")]
	[ComImport]
	public interface IMoniker
	{
		/// <summary>Retrieves the class identifier (CLSID) of an object.</summary>
		/// <param name="pClassID">When this method returns, contains the CLSID. This parameter is passed uninitialized.</param>
		void GetClassID(out Guid pClassID);

		/// <summary>Checks the object for changes since it was last saved.</summary>
		/// <returns>An S_OKHRESULT value if the object has changed; otherwise, an S_FALSEHRESULT value.</returns>
		[PreserveSig]
		int IsDirty();

		/// <summary>Initializes an object from the stream where it was previously saved.</summary>
		/// <param name="pStm">The stream that the object is loaded from. </param>
		void Load(IStream pStm);

		/// <summary>Saves an object to the specified stream.</summary>
		/// <param name="pStm">The stream to which the object is saved. </param>
		/// <param name="fClearDirty">true to clear the modified flag after the save is complete; otherwise false</param>
		void Save(IStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		/// <summary>Returns the size, in bytes, of the stream needed to save the object.</summary>
		/// <param name="pcbSize">When this method returns, contains a long value indicating the size, in bytes, of the stream needed to save this object. This parameter is passed uninitialized.</param>
		void GetSizeMax(out long pcbSize);

		/// <summary>Uses the moniker to bind to the object that it identifies.</summary>
		/// <param name="pbc">A reference to the IBindCtx interface on the bind context object used in this binding operation. </param>
		/// <param name="pmkToLeft">A reference to the moniker to the left of the current moniker, if the moniker is part of a composite moniker. </param>
		/// <param name="riidResult">The interface identifier (IID) of the interface that the client intends to use to communicate with the object that the moniker identifies. </param>
		/// <param name="ppvResult">When this method returns, contains a reference to the interface requested by <paramref name="riidResult" />. This parameter is passed uninitialized.</param>
		void BindToObject(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		/// <summary>Retrieves an interface pointer to the storage that contains the object identified by the moniker.</summary>
		/// <param name="pbc">A reference to the IBindCtx interface on the bind context object used during this binding operation. </param>
		/// <param name="pmkToLeft">A reference to the moniker to the left of the current moniker, if the moniker is part of a composite moniker. </param>
		/// <param name="riid">The interface identifier (IID) of the storage interface requested. </param>
		/// <param name="ppvObj">When this method returns, contains a reference to the interface requested by <paramref name="riid" />. This parameter is passed uninitialized.</param>
		void BindToStorage(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		/// <summary>Returns a reduced moniker, which is another moniker that refers to the same object as the current moniker but can be bound with equal or greater efficiency.</summary>
		/// <param name="pbc">A reference to the IBindCtx interface on the bind context to use in this binding operation. </param>
		/// <param name="dwReduceHowFar">A value that specifies how far the current moniker should be reduced. </param>
		/// <param name="ppmkToLeft">A reference to the moniker to the left of the current moniker. </param>
		/// <param name="ppmkReduced">When this method returns, contains a reference to the reduced form of the current moniker, which can be null if an error occurs or if the current moniker is reduced to nothing. This parameter is passed uninitialized.</param>
		void Reduce(IBindCtx pbc, int dwReduceHowFar, ref IMoniker ppmkToLeft, out IMoniker ppmkReduced);

		/// <summary>Combines the current moniker with another moniker, creating a new composite moniker.</summary>
		/// <param name="pmkRight">A reference to the IMoniker interface on a moniker to append to the end of the current moniker. </param>
		/// <param name="fOnlyIfNotGeneric">true to indicate that the caller requires a nongeneric composition. The operation proceeds only if <paramref name="pmkRight" /> is a moniker class that the current moniker can combine with in some way other than forming a generic composite. false to indicate that the method can create a generic composite if necessary. </param>
		/// <param name="ppmkComposite">When this method returns, contains a reference to the resulting composite moniker. This parameter is passed uninitialized.</param>
		void ComposeWith(IMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out IMoniker ppmkComposite);

		/// <summary>Supplies a pointer to an enumerator that can enumerate the components of a composite moniker.</summary>
		/// <param name="fForward">true to enumerate the monikers from left to right. false to enumerate from right to left. </param>
		/// <param name="ppenumMoniker">When this method returns, contains a reference to the enumerator object for the moniker. This parameter is passed uninitialized.</param>
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out IEnumMoniker ppenumMoniker);

		/// <summary>Compares the current moniker with a specified moniker and indicates whether they are identical.</summary>
		/// <returns>An S_OKHRESULT value if the monikers are identical; otherwise, an S_FALSEHRESULT value.  </returns>
		/// <param name="pmkOtherMoniker">A reference to the moniker to use for comparison. </param>
		[PreserveSig]
		int IsEqual(IMoniker pmkOtherMoniker);

		/// <summary>Calculates a 32-bit integer using the internal state of the moniker.</summary>
		/// <param name="pdwHash">When this method returns, contains the hash value for this moniker. This parameter is passed uninitialized.</param>
		void Hash(out int pdwHash);

		/// <summary>Determines whether the object that is identified by the current moniker is currently loaded and running.</summary>
		/// <returns>An S_OKHRESULT value if the moniker is running; an S_FALSEHRESULT value if the moniker is not running; or an E_UNEXPECTEDHRESULT value.</returns>
		/// <param name="pbc">A reference to the bind context to use in this binding operation. </param>
		/// <param name="pmkToLeft">A reference to the moniker to the left of the current moniker if the current moniker is part of a composite. </param>
		/// <param name="pmkNewlyRunning">A reference to the moniker most recently added to the Running Object Table (ROT). </param>
		[PreserveSig]
		int IsRunning(IBindCtx pbc, IMoniker pmkToLeft, IMoniker pmkNewlyRunning);

		/// <summary>Provides a number representing the time that the object identified by the current moniker was last changed.</summary>
		/// <param name="pbc">A reference to the bind context to use in this binding operation. </param>
		/// <param name="pmkToLeft">A reference to the moniker to the left of the current moniker, if the moniker is part of a composite moniker. </param>
		/// <param name="pFileTime">When this method returns, contains the time of the last change. This parameter is passed uninitialized.</param>
		void GetTimeOfLastChange(IBindCtx pbc, IMoniker pmkToLeft, out FILETIME pFileTime);

		/// <summary>Provides a moniker that, when composed to the right of the current moniker or one of similar structure, composes to nothing.</summary>
		/// <param name="ppmk">When this method returns, contains a moniker that is the inverse of the current moniker. This parameter is passed uninitialized.</param>
		void Inverse(out IMoniker ppmk);

		/// <summary>Creates a new moniker based on the common prefix that this moniker shares with another moniker.</summary>
		/// <param name="pmkOther">A reference to the IMoniker interface on another moniker to compare with the current moniker for a common prefix. </param>
		/// <param name="ppmkPrefix">When this method returns, contains the moniker that is the common prefix of the current moniker and <paramref name="pmkOther" />. This parameter is passed uninitialized.</param>
		void CommonPrefixWith(IMoniker pmkOther, out IMoniker ppmkPrefix);

		/// <summary>Supplies a moniker that, when appended to the current moniker (or one with a similar structure), yields the specified moniker.</summary>
		/// <param name="pmkOther">A reference to the moniker to which a relative path should be taken. </param>
		/// <param name="ppmkRelPath">When this method returns, contains a reference to the relative moniker. This parameter is passed uninitialized.</param>
		void RelativePathTo(IMoniker pmkOther, out IMoniker ppmkRelPath);

		/// <summary>Gets the display name, which is a user-readable representation of the current moniker.</summary>
		/// <param name="pbc">A reference to the bind context to use in this operation. </param>
		/// <param name="pmkToLeft">A reference to the moniker to the left of the current moniker, if the moniker is part of a composite moniker. </param>
		/// <param name="ppszDisplayName">When this method returns, contains the display name string. This parameter is passed uninitialized.</param>
		void GetDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		/// <summary>Reads as many characters of the specified display name as the <see cref="M:System.Runtime.InteropServices.ComTypes.IMoniker.ParseDisplayName(System.Runtime.InteropServices.ComTypes.IBindCtx,System.Runtime.InteropServices.ComTypes.IMoniker,System.String,System.Int32@,System.Runtime.InteropServices.ComTypes.IMoniker@)" /> understands and builds a moniker corresponding to the portion read.</summary>
		/// <param name="pbc">A reference to the bind context to use in this binding operation. </param>
		/// <param name="pmkToLeft">A reference to the moniker that has been built from the display name up to this point. </param>
		/// <param name="pszDisplayName">A reference to the string containing the remaining display name to parse. </param>
		/// <param name="pchEaten">When this method returns, contains the number of characters that were consumed in parsing <paramref name="pszDisplayName" />. This parameter is passed uninitialized.</param>
		/// <param name="ppmkOut">When this method returns, contains a reference to the moniker that was built from <paramref name="pszDisplayName" />. This parameter is passed uninitialized.</param>
		void ParseDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);

		/// <summary>Indicates whether this moniker is of one of the system-supplied moniker classes.</summary>
		/// <returns>An S_OKHRESULT value if the moniker is a system moniker; otherwise, an S_FALSEHRESULT value.</returns>
		/// <param name="pdwMksys">When this method returns, contains a pointer to an integer that is one of the values from the MKSYS enumeration, and refers to one of the COM moniker classes. This parameter is passed uninitialized.</param>
		[PreserveSig]
		int IsSystemMoniker(out int pdwMksys);
	}
}
