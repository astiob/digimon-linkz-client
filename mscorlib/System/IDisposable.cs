using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Defines a method to release allocated resources.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public interface IDisposable
	{
		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		/// <filterpriority>2</filterpriority>
		void Dispose();
	}
}
