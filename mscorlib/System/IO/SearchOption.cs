using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Specifies whether to search the current directory, or the current directory and all subdirectories. </summary>
	[ComVisible(true)]
	[Serializable]
	public enum SearchOption
	{
		/// <summary>Includes only the current directory in a search.</summary>
		TopDirectoryOnly,
		/// <summary>Includes the current directory and all the subdirectories in a search operation. This option includes reparse points like mounted drives and symbolic links in the search.</summary>
		AllDirectories
	}
}
