using System;

namespace System.Collections.Generic
{
	/// <summary>Exposes the enumerator, which supports a simple iteration over a collection of a specified type.</summary>
	/// <typeparam name="T">The type of objects to enumerate.</typeparam>
	/// <filterpriority>1</filterpriority>
	public interface IEnumerable<T> : IEnumerable
	{
		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
		/// <filterpriority>1</filterpriority>
		IEnumerator<T> GetEnumerator();
	}
}
