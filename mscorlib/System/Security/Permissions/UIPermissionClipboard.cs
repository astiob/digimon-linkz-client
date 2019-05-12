using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Specifies the type of clipboard access that is allowed to the calling code.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum UIPermissionClipboard
	{
		/// <summary>Clipboard cannot be used.</summary>
		NoClipboard,
		/// <summary>The ability to put data on the clipboard (Copy, Cut) is unrestricted. Intrinsic controls that accept Paste, such as text box, can accept the clipboard data, but user controls that must programmatically read the clipboard cannot.</summary>
		OwnClipboard,
		/// <summary>Clipboard can be used without restriction.</summary>
		AllClipboard
	}
}
