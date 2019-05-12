using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	/// <summary>Represents a symbol binder for managed code.</summary>
	[ComVisible(true)]
	public interface ISymbolBinder
	{
		/// <summary>Gets the interface of the symbol reader for the current file.</summary>
		/// <returns>The <see cref="T:System.Diagnostics.SymbolStore.ISymbolReader" /> interface that reads the debugging symbols.</returns>
		/// <param name="importer">The metadata import interface. </param>
		/// <param name="filename">The name of the file for which the reader interface is required. </param>
		/// <param name="searchPath">The search path used to locate the symbol file. </param>
		[Obsolete("This interface is not 64-bit clean.  Use ISymbolBinder1 instead", false)]
		ISymbolReader GetReader(int importer, string filename, string searchPath);
	}
}
