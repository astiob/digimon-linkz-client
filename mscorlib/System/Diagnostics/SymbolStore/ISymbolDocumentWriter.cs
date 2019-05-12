using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	/// <summary>Represents a document referenced by a symbol store.</summary>
	[ComVisible(true)]
	public interface ISymbolDocumentWriter
	{
		/// <summary>Sets checksum information.</summary>
		/// <param name="algorithmId">The GUID representing the algorithm ID. </param>
		/// <param name="checkSum">The checksum. </param>
		void SetCheckSum(Guid algorithmId, byte[] checkSum);

		/// <summary>Stores the raw source for a document in the symbol store.</summary>
		/// <param name="source">The document source represented as unsigned bytes. </param>
		void SetSource(byte[] source);
	}
}
