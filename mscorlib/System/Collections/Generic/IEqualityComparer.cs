using System;

namespace System.Collections.Generic
{
	/// <summary>Defines methods to support the comparison of objects for equality.</summary>
	/// <typeparam name="T">The type of objects to compare.</typeparam>
	public interface IEqualityComparer<T>
	{
		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		/// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
		bool Equals(T x, T y);

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <returns>A hash code for the specified object.</returns>
		/// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
		int GetHashCode(T obj);
	}
}
