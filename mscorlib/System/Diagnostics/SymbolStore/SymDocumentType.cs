using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	/// <summary>Holds the public GUIDs for document types to be used with the symbol store.</summary>
	[ComVisible(true)]
	public class SymDocumentType
	{
		/// <summary>Specifies the GUID of the document type to be used with the symbol store.</summary>
		public static readonly Guid Text;
	}
}
