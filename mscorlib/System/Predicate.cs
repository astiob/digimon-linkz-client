using System;

namespace System
{
	/// <summary>Represents the method that defines a set of criteria and determines whether the specified object meets those criteria.</summary>
	/// <returns>true if <paramref name="obj" /> meets the criteria defined within the method represented by this delegate; otherwise, false.</returns>
	/// <param name="obj">The object to compare against the criteria defined within the method represented by this delegate.</param>
	/// <typeparam name="T">The type of the object to compare.</typeparam>
	/// <filterpriority>2</filterpriority>
	public delegate bool Predicate<T>(T obj);
}
