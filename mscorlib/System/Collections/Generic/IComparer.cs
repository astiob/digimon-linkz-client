using System;

namespace System.Collections.Generic
{
	/// <summary>Defines a method that a type implements to compare two objects.</summary>
	/// <typeparam name="T">The type of objects to compare.</typeparam>
	/// <filterpriority>1</filterpriority>
	public interface IComparer<T>
	{
		/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <returns>Value Condition Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		int Compare(T x, T y);
	}
}
