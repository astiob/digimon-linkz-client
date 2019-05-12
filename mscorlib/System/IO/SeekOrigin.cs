using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Provides the fields that represent reference points in streams for seeking.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public enum SeekOrigin
	{
		/// <summary>Specifies the beginning of a stream.</summary>
		Begin,
		/// <summary>Specifies the current position within a stream.</summary>
		Current,
		/// <summary>Specifies the end of a stream.</summary>
		End
	}
}
