using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Specifies whether to create an asymmetric signature key or an asymmetric exchange key. </summary>
	[ComVisible(true)]
	[Serializable]
	public enum KeyNumber
	{
		/// <summary>An exchange key pair used to encrypt session keys so that they can be safely stored and exchanged with other users.  </summary>
		Exchange = 1,
		/// <summary>A signature key pair used for authenticating digitally signed messages or files.</summary>
		Signature
	}
}
