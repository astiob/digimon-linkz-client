using System;

namespace System.Collections.Generic
{
	/// <summary>Supports a simple iteration over a generic collection.</summary>
	/// <typeparam name="T">The type of objects to enumerate.</typeparam>
	/// <filterpriority>1</filterpriority>
	public interface IEnumerator<T> : IEnumerator, IDisposable
	{
		/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		T Current { get; }
	}
}
