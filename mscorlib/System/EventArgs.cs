using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	///   <see cref="T:System.EventArgs" /> is the base class for classes containing event data. </summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class EventArgs
	{
		/// <summary>Represents an event with no event data.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly EventArgs Empty = new EventArgs();
	}
}
