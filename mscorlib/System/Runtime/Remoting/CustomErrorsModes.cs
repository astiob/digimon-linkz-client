using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	/// <summary>Specifies how custom errors are handled.</summary>
	[ComVisible(true)]
	public enum CustomErrorsModes
	{
		/// <summary>All callers receive filtered exception information.</summary>
		On,
		/// <summary>All callers receive complete exception information.</summary>
		Off,
		/// <summary>Local callers receive complete exception information; remote callers receive filtered exception information.</summary>
		RemoteOnly
	}
}
