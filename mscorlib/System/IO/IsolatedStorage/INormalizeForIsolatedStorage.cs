using System;
using System.Runtime.InteropServices;

namespace System.IO.IsolatedStorage
{
	/// <summary>Enables comparisons between an isolated store and an application domain and assembly's evidence.</summary>
	[ComVisible(true)]
	public interface INormalizeForIsolatedStorage
	{
		/// <summary>When overridden in a derived class, returns a normalized copy of the object on which it is called.</summary>
		/// <returns>A normalized object that represents the instance on which this method was called. This instance can be a string, stream, or any serializable object.</returns>
		object Normalize();
	}
}
