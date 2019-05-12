using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Specifies flags that modify the behavior of the cryptographic service providers (CSP).</summary>
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CspProviderFlags
	{
		/// <summary>Use key information from the computer's key store.</summary>
		UseMachineKeyStore = 1,
		/// <summary>Use key information from the default key container.</summary>
		UseDefaultKeyContainer = 2,
		/// <summary>Use key information from the current key.</summary>
		UseExistingKey = 8,
		/// <summary>Don't specify any settings.</summary>
		NoFlags = 0,
		/// <summary>Prevent the CSP from displaying any user interface (UI) for this context.</summary>
		NoPrompt = 64,
		/// <summary>Allow a key to be exported for archival or recovery.</summary>
		UseArchivableKey = 16,
		/// <summary>Use key information that can not be exported.</summary>
		UseNonExportableKey = 4,
		/// <summary>Notify the user through a dialog box or another method when certain actions are attempting to use a key.  This flag is not compatible with the <see cref="F:System.Security.Cryptography.CspProviderFlags.NoPrompt" /> flag.</summary>
		UseUserProtectedKey = 32
	}
}
